﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace Mutant.Deploy.Factory.Artificers
{
    public abstract class Artificer
    {

        private string _baseCommit;
        private readonly string GitEmptyTree = "4b825dc642cb6eb9a060e54bf8d69288fbee4904";

        public string BaseCommit
        {
            get { return _baseCommit; }
            set
            {
                if (value != null)
                {
                    _baseCommit = FindBaseCommit(value);
                }
            }
        }

        private struct SplitString
        {
            public string Left;
            public string Right;
        }

        protected Artificer()
        {
            DestroyExistingArtifacts();
            CreateDirectories();
        }

        protected Artificer(Boolean DisableSetup)
        {
            if (!DisableSetup)
            {
                DestroyExistingArtifacts();
                CreateDirectories();
            }
        }
        
        public abstract void CreateArtifact();
        
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
                    // try again to skip "Folder is not empty" error.
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

        protected Collection<PSObject> RunPowershellCommand(string Command)
        {
            string directory = Directory.GetCurrentDirectory();
            Collection<PSObject> results = new Collection<PSObject>();

            using (PowerShell powershell = PowerShell.Create())
            {
                Console.WriteLine(Command);
                powershell.AddScript(String.Format(@"cd {0}", directory));

                powershell.AddScript(Command);
                
                results = powershell.Invoke();

                foreach (var s in results)
                {
                    Console.WriteLine(s.ToString());
                }
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

                    string targetDirectoryForFile = WorkingDirectory + 
                        directoryByFileType[path.Right] + copyPath.Right;
                    string metaFileSource = fullPath + "-meta.xml";
                    string metaFileName = copyPath.Right + "-meta.xml";

                    string targetDirectoryForMetaFile = directoryByFileType[path.Right] + metaFileName;
                    Console.WriteLine("Adding " + copyPath.Right + " to deployment");
                    Console.WriteLine(fullPath);
                    Console.WriteLine(targetDirectoryForFile);
                    File.Copy(fullPath, targetDirectoryForFile);
                    File.Copy(metaFileSource, targetDirectoryForMetaFile);
                }
                else
                {
                    Console.Out.WriteLine("File not added for deployment: " + Result.ToString());
                }
            }

            string SourcePackage = WorkingDirectory + @"\src\package.xml";
            string TargetPackage = WorkingDirectory + @"\deploy\artifacts\src\package.xml";
            File.Copy(SourcePackage, TargetPackage);
        }

        private SplitString Split(string StringToSplit, string SplitLocation)
        {
            SplitString split = new SplitString();
            string[] splited = StringToSplit.Split(new string[] { SplitLocation }, StringSplitOptions.None);
            split.Left = String.Join(".", splited.Take(splited.Length - 1));
            split.Right = splited.Last();
            return split;
        }

        private string FindBaseCommit(string Commit)
        {
            Collection<PSObject> results = RunPowershellCommand("git log --pretty=format:%H");
            List<PSObject> reversedResults = results.Reverse().ToList();

            PSObject CommitResult = reversedResults.First(r => r.ToString() == Commit);
            if (reversedResults.IndexOf(CommitResult) == 0)
            {
                return GitEmptyTree;
            }
            
            PSObject Previous = reversedResults.ElementAt(reversedResults.IndexOf(CommitResult) - 1);
            return Previous.ToString();
        }
    }
}
