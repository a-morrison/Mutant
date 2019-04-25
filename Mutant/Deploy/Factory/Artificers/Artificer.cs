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

        protected void ProcessResults(Collection<PSObject> Results, string WorkingDirectory)
        {
            foreach (PSObject Result in Results)
            {
                Artifact ProposedArtifact = new Artifact(WorkingDirectory, Result.ToString());
                ProposedArtifact.Move();
            }

            CopyPackageXML(WorkingDirectory);
        }

        private void CopyPackageXML(string WorkingDirectory)
        {
            string SourcePackage = WorkingDirectory + @"\src\package.xml";
            string TargetPackage = WorkingDirectory + @"\deploy\artifacts\src\package.xml";
            File.Copy(SourcePackage, TargetPackage);
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
