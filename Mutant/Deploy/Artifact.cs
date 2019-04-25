using System;
using System.Collections.Generic;
using System.IO;
using Mutant.Core.Util;

namespace Mutant.Deploy
{
    public class Artifact
    {
        private readonly string WorkingDirectory;
        private readonly string FileName;

        private readonly Dictionary<string, string> directoryByFileType = new Dictionary<string, string>
        {
            { "cls", @"\deploy\artifacts\src\classes\" },
            { "trigger", @"\deploy\artifacts\src\triggers\" },
            { "page", @"\deploy\artifacts\src\pages\" },
            { "component", @"\deploy\artifacts\src\components\" }
        };

        private readonly Dictionary<string, string> placeToSplit = new Dictionary<string, string>
        {
            { "cls", @"classes\" },
            { "trigger", @"triggers\" },
            { "page", @"pages\" },
            { "component", @"components\" }
        };

        public Artifact(string WorkingDirectory, string FileName)
        {
            this.WorkingDirectory = WorkingDirectory;
            this.FileName = Sanitize(FileName);
        }

        private string Sanitize(string StringToSanitize)
        {
            string SanitizedString = StringToSanitize;
            SanitizedString = SanitizedString.Replace('/', '\\');
            if (!SanitizedString.StartsWith(@"\\") || !SanitizedString.StartsWith(@"\"))
            {
                SanitizedString = String.Concat(@"\", SanitizedString);
            }
            return SanitizedString;
        }

        public void Move()
        {
            string fullPath = WorkingDirectory + FileName;
            fullPath = fullPath.Replace('/', '\\');
            SplitString path = Spliter.Split(fullPath, ".");

            if (directoryByFileType.ContainsKey(path.Right))
            {
                string splitLocation = placeToSplit[path.Right];

                SplitString copyPath = Spliter.Split(fullPath, splitLocation);

                string targetDirectoryForFile = WorkingDirectory +
                    directoryByFileType[path.Right] + copyPath.Right;

                string metaFileSource = fullPath + "-meta.xml";
                string metaFileName = copyPath.Right + "-meta.xml";
                string targetDirectoryForMetaFile = WorkingDirectory +
                    directoryByFileType[path.Right] + metaFileName;

                Console.WriteLine("Adding " + copyPath.Right + " to deployment");
                File.Copy(fullPath, targetDirectoryForFile);
                File.Copy(metaFileSource, targetDirectoryForMetaFile);
            }
            else
            {
                Console.WriteLine("File not added for deployment: " + FileName);
            }
        }
    }
}
