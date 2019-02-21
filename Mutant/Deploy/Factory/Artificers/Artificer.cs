using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace Mutant.Deploy.Factory.Artificers
{
    abstract class Artificer
    {
        public abstract void CreateArtifact();

        private struct SplitString
        {
            public string Left;
            public string Right;
        }

        protected void DestroyExistingArtifacts()
        {
            if (Directory.Exists(@"deploy\artifacts"))
            {
                try
                {
                    Directory.Delete(@"deploy\artifacts", true);
                }
                catch (IOException)
                {
                    // try again to skip "Folder is not empty" error.
                    Thread.Sleep(0);
                    Directory.Delete(@"deploy\artifacts", true);
                }
            }
        }

        protected void CreateDirectories()
        {
            List<string> directoriesToCreate = new List<string>
            {
                { @"deploy\artifacts" },
                { @"deploy\artifacts\src\classes" },
                { @"deploy\artifacts\src\triggers" },
                { @"deploy\artifacts\src\pages" },
                { @"deploy\artifacts\src\components" }
            };

            foreach (string dirctoryToCreate in directoriesToCreate)
            {
                Directory.CreateDirectory(dirctoryToCreate);
            }
        }

        protected Collection<PSObject> RunPowershellCommand(string Command)
        {
            string directory = Directory.GetCurrentDirectory();
            Collection<PSObject> results = new Collection<PSObject>();

            using (PowerShell powershell = PowerShell.Create())
            {
                powershell.AddScript(String.Format(@"cd {0}", directory));

                powershell.AddScript(@Command);

                results = powershell.Invoke();
            }

            return results;
        }

        protected void ProcessResults(Collection<PSObject> Results, string WorkingDirectory)
        {
            Dictionary<string, string> directoryByFileType = new Dictionary<string, string>
            {
                { "cls", @"deploy\artifacts\src\classes\" },
                { "trigger", @"deploy\artifacts\src\triggers\" },
                { "page", @"deploy\artifacts\src\pages\" },
                { "component", @"deploy\artifacts\src\components\" }
            };

            Dictionary<string, string> placeToSplit = new Dictionary<string, string>
            {
                { "cls", @"classes\" },
                { "trigger", @"triggers\" },
                { "page", @"pages\" },
                { "component", @"components\" }
            };

            foreach (PSObject Result in Results)
            {
                string fullPath = WorkingDirectory + Result.ToString();
                fullPath = fullPath.Replace('/', '\\');
                SplitString path = Split(fullPath, ".");

                if (directoryByFileType.ContainsKey(path.Right))
                {
                    string splitLocation = placeToSplit[path.Right];

                    SplitString copyPath = Split(fullPath, splitLocation);

                    string targetDirectoryForFile = WorkingDirectory + directoryByFileType[path.Right] + copyPath.Right;
                    string metaFileSource = fullPath + "-meta.xml";
                    string metaFileName = copyPath.Right + "-meta.xml";

                    string targetDirectoryForMetaFile = directoryByFileType[path.Right] + metaFileName;
                    File.Copy(fullPath, targetDirectoryForFile);
                    File.Copy(metaFileSource, targetDirectoryForMetaFile);
                }
                else
                {
                    Console.Out.WriteLine("No");
                }
            }

            File.Copy(@"src\package.xml", @"deploy\artifacts\src\package.xml");
        }

        private SplitString Split(string StringToSplit, string SplitLocation)
        {
            SplitString split = new SplitString();
            string[] splited = StringToSplit.Split(new string[] { SplitLocation }, StringSplitOptions.None);
            split.Left = String.Join(".", splited.Take(splited.Length - 1));
            split.Right = splited.Last();
            return split;
        }
    }
}
