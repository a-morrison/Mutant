using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Mutant.Core.Util;

namespace Mutant.Deploy.Factory.Artificers
{
    public abstract class Artificer
    {
        public abstract string Target
        {
            get;
        }

        private string _baseCommit;
        
        public string BaseCommit
        {
            get { return _baseCommit; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _baseCommit = FindBaseCommit(value);
                }
            }
        }
        
        protected Artificer(Boolean EnableSetup)
        {
            if (EnableSetup)
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

        protected void ProcessResults(Collection<PSObject> Results, string WorkingDirectory)
        {
            Dictionary<string, string> FileToType = new Dictionary<string, string>();
            foreach (PSObject Result in Results)
            {
                string type = Result.ToString().Substring(0, 1);
                string resultWithoutType = Result.ToString().Remove(0, 1).Trim();
                string fullPath = WorkingDirectory + resultWithoutType;
                fullPath = fullPath.Replace('/', '\\');
                SplitString path = Spliter.Split(fullPath, ".");
                if (Artifact.TARGET_DIRECTORIES_BY_EXTENSION.ContainsKey(path.Right))
                {
                    FileToType.Add(resultWithoutType, type);
                }
                else
                {
                    Console.WriteLine("File not added for deployment: " + Result.ToString());
                }
            }

            Artifact ProposedArtifact = new Artifact(WorkingDirectory, FileToType);
            ProposedArtifact.Display();
        }

        private string FindBaseCommit(string Commit)
        {
            Collection<PSObject> results = Shell.RunCommand(Directory.GetCurrentDirectory(), "git log --pretty=format:%H");
            List<PSObject> reversedResults = results.Reverse().ToList();

            PSObject CommitResult = reversedResults.First(r => r.ToString() == Commit);
            if (reversedResults.IndexOf(CommitResult) == 0)
            {
                // Hash of base for every git repo
                string GitEmptyTree = "4b825dc642cb6eb9a060e54bf8d69288fbee4904";
                return GitEmptyTree;
            }
            
            PSObject Previous = reversedResults.ElementAt(reversedResults.IndexOf(CommitResult) - 1);
            return Previous.ToString();
        }
    }
}
