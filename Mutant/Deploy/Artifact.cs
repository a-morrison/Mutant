using System;
using System.Collections.Generic;
using System.IO;
using Mutant.Core.Util;
using System.Xml.Linq;

namespace Mutant.Deploy
{
    public class Artifact
    {
        public static readonly IReadOnlyDictionary<string, string> TARGET_DIRECTORIES_BY_EXTENSION = new Dictionary<string, string>
        {
            { "cls", @"\deploy\artifacts\src\classes\" },
            { "trigger", @"\deploy\artifacts\src\triggers\" },
            { "page", @"\deploy\artifacts\src\pages\" },
            { "component", @"\deploy\artifacts\src\components\" }
        };

        private readonly string WorkingDirectory;
        private readonly Dictionary<string, string> FileToChangeType;

        private readonly Dictionary<string, string> placeToSplit = new Dictionary<string, string>
        {
            { "cls", @"classes\" },
            { "trigger", @"triggers\" },
            { "page", @"pages\" },
            { "component", @"components\" }
        };

        public Artifact(string WorkingDirectory, Dictionary<string, string> Files)
        {
            this.WorkingDirectory = WorkingDirectory;
            this.FileToChangeType = Sanitize(Files);
        }

        private Dictionary<string, string> Sanitize(Dictionary<string, string> FilesToSanitize)
        {
            Dictionary<string, string> SanitizedFiles = new Dictionary<string, string>();
            foreach (string File in FilesToSanitize.Keys)
            {
                string SanitizedFile = File;
                SanitizedFile = SanitizedFile.Replace('/', '\\');
                if (!SanitizedFile.StartsWith(@"\\") || !SanitizedFile.StartsWith(@"\"))
                {
                    SanitizedFile = String.Concat(@"\", SanitizedFile);
                }
                SanitizedFiles.Add(SanitizedFile, FilesToSanitize[File]);
            }
            
            return SanitizedFiles;
        }

        public void Display()
        {
            Dictionary<string, List<string>> DestructiveChanges = new Dictionary<string, List<string>>();
            foreach (string File in FileToChangeType.Keys)
            {
                string fullPath = WorkingDirectory + File;
                SplitString path = Spliter.Split(fullPath, ".");

                string splitLocation = placeToSplit[path.Right];
                SplitString copyPath = Spliter.Split(fullPath, splitLocation);

                string targetDirectoryForFile = WorkingDirectory +
                    TARGET_DIRECTORIES_BY_EXTENSION[path.Right] + copyPath.Right;

                string metaFileSource = fullPath + "-meta.xml";
                string metaFileName = copyPath.Right + "-meta.xml";
                string targetDirectoryForMetaFile = WorkingDirectory +
                    TARGET_DIRECTORIES_BY_EXTENSION[path.Right] + metaFileName;

                string changeType = FileToChangeType[File];
                if (changeType.Equals("D"))
                {
                    SplitString extensionSplit = Spliter.Split(copyPath.Right, ".");
                    if (!DestructiveChanges.ContainsKey(extensionSplit.Right))
                    {
                        DestructiveChanges.Add(extensionSplit.Right, new List<string>());
                    }
                    DestructiveChanges[extensionSplit.Right].Add(extensionSplit.Left);
                }

                Console.WriteLine("Adding " + copyPath.Right + " to deployment");
                try
                {
                    System.IO.File.Copy(fullPath, targetDirectoryForFile);
                    System.IO.File.Copy(metaFileSource, targetDirectoryForMetaFile);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("Possible destructive change detected!");
                    Console.WriteLine(ex.FileName + " not added to artifact.");
                }
            }
            if (DestructiveChanges.Count != 0)
            {
                CreateDestructiveChangesXML(DestructiveChanges);
            }
            CopyPackageXML();
        }

        private void CopyPackageXML()
        {
            string SourcePackage = this.WorkingDirectory + @"\src\package.xml";
            string TargetPackage = this.WorkingDirectory + @"\deploy\artifacts\src\package.xml";
            File.Copy(SourcePackage, TargetPackage);
        }

        private void CreateDestructiveChangesXML(Dictionary<string, List<string>> ExtensionToFiles)
        {
            var ns = XNamespace.Get("http://soap.sforce.com/2006/04/metadata");
            XDocument ChangesXML = new XDocument(new XDeclaration("1.0", "utf-8", null));
            var root = new XElement(ns + "Package");
            foreach (string Extension in ExtensionToFiles.Keys)
            {
                var type = new XElement("type");
                string ExtensionName = GetExtensionName(Extension);
                List<string> Files = ExtensionToFiles[Extension];
                foreach (string File in Files)
                {
                    var member = new XElement("members", File);
                    type.Add(member);
                }
                var typeName = new XElement("name", ExtensionName);
                type.Add(typeName);
                root.Add(type);
            }
            ChangesXML.Add(root);

            string TargetDirectory = this.WorkingDirectory + @"\deploy\artifacts\src\destructiveChangesPost.xml";
            ChangesXML.Save(TargetDirectory);
        }

        private string GetExtensionName(string Extension)
        {
            switch (Extension)
            {
                case "cls":
                    return "ApexClass";
                case "trigger":
                    return "ApexTrigger";
                case "page":
                    return "ApexPage";
                case "component":
                    return "ApexComponent";
                default:
                    return Extension;
            }
        }
    }
}
