using System;
using System.Collections.Generic;
using System.IO;
using Mutant.Core.Util;

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
        private readonly List<string> Files;

        private readonly Dictionary<string, string> placeToSplit = new Dictionary<string, string>
        {
            { "cls", @"classes\" },
            { "trigger", @"triggers\" },
            { "page", @"pages\" },
            { "component", @"components\" }
        };

        public Artifact(string WorkingDirectory, List<string> Files)
        {
            this.WorkingDirectory = WorkingDirectory;
            this.Files = Sanitize(Files);
        }

        private List<string> Sanitize(List<string> FilesToSanitize)
        {
            List<string> SanitizedFiles = new List<string>();
            foreach (string File in FilesToSanitize)
            {
                string SanitizedFile = File;
                SanitizedFile = SanitizedFile.Replace('/', '\\');
                if (!SanitizedFile.StartsWith(@"\\") || !SanitizedFile.StartsWith(@"\"))
                {
                    SanitizedFile = String.Concat(@"\", SanitizedFile);
                }
                SanitizedFiles.Add(SanitizedFile);
            }
            
            return SanitizedFiles;
        }

        public void Display()
        {
            foreach (string File in Files)
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

                Console.WriteLine("Adding " + copyPath.Right + " to deployment");
                System.IO.File.Copy(fullPath, targetDirectoryForFile);
                System.IO.File.Copy(metaFileSource, targetDirectoryForMetaFile);
            }
        }
    }
}
