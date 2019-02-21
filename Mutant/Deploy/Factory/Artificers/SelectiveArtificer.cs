using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace Mutant.Deploy.Factory.Artificers
{
    class SelectiveArtificer : Artificer
    {
        private Dictionary<string, string> directoryByFileType = new Dictionary<string, string>
        {
            { "cls", @"deploy\artifacts\src\classes\" },
            { "trigger", @"deploy\artifacts\src\triggers\" },
            { "page", @"deploy\artifacts\src\pages\" },
            { "component", @"deploy\artifacts\src\components\" }
        };

        private Dictionary<string, string> placeToSplit = new Dictionary<string, string>
        {
            { "cls", @"classes\" },
            { "trigger", @"triggers\" },
            { "page", @"pages\" },
            { "component", @"components\" }
        };

        public override void CreateArtifact()
        {
            string workingDir = @"C:\Users\Alex Morrison\Documents\MutantTesting\DOH\DC%20DOH\";
            Directory.SetCurrentDirectory(workingDir);
            DestroyExistingArtifacts();

            CreateDirectories();
            
            Collection<PSObject> results = RunPowershellCommand("git diff --name-only");

            foreach (PSObject result in results)
            {
                string fullPath = workingDir + result.ToString();
                fullPath = fullPath.Replace('/', '\\');
                Console.WriteLine(fullPath);
                string[] split = fullPath.Split('.');
                string path = string.Join(".", split.Take(split.Length - 1));
                string extension = split.Last();

                if (directoryByFileType.ContainsKey(extension))
                {
                    string splitLocation = placeToSplit[extension];

                    string[] otherSplit = fullPath.Split(new string[] { splitLocation }, StringSplitOptions.None);
                    string unneededPath = string.Join(".", otherSplit.Take(otherSplit.Length - 1));
                    string newFileName = otherSplit.Last();

                    string targetDirectoryForFile = workingDir + directoryByFileType[extension] + newFileName;
                    string metaFileSource = fullPath + "-meta.xml";
                    string metaFileName = newFileName + "-meta.xml";

                    string targetDirectoryForMetaFile = directoryByFileType[extension] + metaFileName;
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

        private void DestroyExistingArtifacts()
        {
            if (Directory.Exists(@"deploy\artifacts"))
            {
                try
                {
                    Directory.Delete(@"deploy\artifacts", true);
                }
                catch (IOException)
                {
                    Thread.Sleep(0);
                    Directory.Delete(@"deploy\artifacts", true);
                }
            }
        }

        private void CreateDirectories()
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

        private Collection<PSObject> RunPowershellCommand(string Command)
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
    }
}
