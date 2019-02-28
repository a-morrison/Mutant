using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;

namespace Mutant.Deploy.Factory.Artificers.Tests
{
    struct Info
    {
        public string URL;
        public string Username;
        public string Password;
        public string WorkingDirectory;
    }

    [TestClass()]
    public class ArtificerTests
    {
        [TestMethod()]
        public void CreateArtifactTest()
        {
            Info MutantInfo = new Info
            {
                URL = "Test",
                Username = "Test",
                Password = "Test",
                WorkingDirectory = Directory.GetCurrentDirectory() + "\\"
            };

            string CurrentDirectory = Directory.GetCurrentDirectory();
            using (StreamWriter file = File.CreateText(CurrentDirectory + @"\.credentials"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, MutantInfo);
            }

            ArtificerFactory Factory = new SelectiveFactory();
            Artificer artificer = Factory.CreateArtificer();

            artificer.CreateArtifact();

            Assert.IsTrue(Directory.Exists(@"deploy\artifacts"));
        }

        [TestMethod()]
        public void CreateWithBaseCommit()
        {
            Info MutantInfo = new Info
            {
                URL = "Test",
                Username = "Test",
                Password = "Test",
                WorkingDirectory = @"C:\temp\MutantTests\"
            };

            if (Directory.Exists(MutantInfo.WorkingDirectory + @".git\"))
            {
                Console.WriteLine("Deleteing .git");
                DeleteDirectory(MutantInfo.WorkingDirectory + @".git\");
            }
            if (Directory.Exists(MutantInfo.WorkingDirectory + @"src\"))
            {
                Console.WriteLine("Deleteing src");
                DeleteDirectory(MutantInfo.WorkingDirectory + @"src\");
            }
            if (Directory.Exists(MutantInfo.WorkingDirectory + @"deploy\"))
            {
                Console.WriteLine("Deleteing deploy");
                DeleteDirectory(MutantInfo.WorkingDirectory + @"deploy\");
            }
            Directory.CreateDirectory(MutantInfo.WorkingDirectory);
            Directory.SetCurrentDirectory(MutantInfo.WorkingDirectory);
            using (StreamWriter file = File.CreateText(MutantInfo.WorkingDirectory + @"\.credentials"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, MutantInfo);
            }
            Collection<PSObject> results = new Collection<PSObject>();
            results = RunPowershellCommand("git init", MutantInfo.WorkingDirectory);
            Directory.CreateDirectory(MutantInfo.WorkingDirectory + @"\src\classes");
            File.Create(Directory.GetCurrentDirectory() + @"\src\package.xml");

            using (File.Create(MutantInfo.WorkingDirectory + @"\src\classes\test1.cls")) { }
            using (File.Create(MutantInfo.WorkingDirectory + @"\src\classes\test1.cls-meta.xml")) { }

            results = RunPowershellCommand("git update-index --no-assume-unchanged " + MutantInfo.WorkingDirectory + @"src\classes\test1.cls", MutantInfo.WorkingDirectory);
            results = RunPowershellCommand("git add -f " + MutantInfo.WorkingDirectory + @"src\classes\test1.cls", MutantInfo.WorkingDirectory);
            results = RunPowershellCommand("git commit -m \"some\"", MutantInfo.WorkingDirectory);

            using (File.Create(MutantInfo.WorkingDirectory + @"\src\classes\test2.cls")) { }
            using (File.Create(MutantInfo.WorkingDirectory + @"\src\classes\test2.cls-meta.xml")) { }

            results = RunPowershellCommand("git update-index --no-assume-unchanged " + MutantInfo.WorkingDirectory + @"src\classes\test2.cls", MutantInfo.WorkingDirectory);
            results = RunPowershellCommand("git add -f " + MutantInfo.WorkingDirectory + @"src\classes\test2.cls", MutantInfo.WorkingDirectory);
            results = RunPowershellCommand("git commit -m \"testagain\"", MutantInfo.WorkingDirectory);
            results = RunPowershellCommand("git log --pretty=format:%H", MutantInfo.WorkingDirectory);
            string BaseCommit = results[1].ToString();

            ArtificerFactory Factory = new SelectiveFactory();
            Artificer artificer = Factory.CreateArtificer();

            artificer.BaseCommit = BaseCommit;

            artificer.CreateArtifact();

            Assert.IsTrue(File.Exists(MutantInfo.WorkingDirectory + @"\deploy\artifacts\src\classes\test1.cls"));
            Assert.IsTrue(File.Exists(MutantInfo.WorkingDirectory + @"\deploy\artifacts\src\classes\test2.cls"));
        }

        private Collection<PSObject> RunPowershellCommand(string Command, string WorkingDirectory)
        {
            Collection<PSObject> results = new Collection<PSObject>();

            using (PowerShell powershell = PowerShell.Create())
            {
                Console.WriteLine(Command + " in " + WorkingDirectory);
                powershell.AddScript(String.Format(@"cd {0}", WorkingDirectory));

                powershell.AddScript(Command);

                results = powershell.Invoke();

                foreach (var s in results)
                {
                    Console.WriteLine(s.ToString());
                }
            }

            return results;
        }

        private void DeleteDirectory(string d)
        {
            foreach (var sub in Directory.EnumerateDirectories(d))
            {
                DeleteDirectory(sub);
            }
            foreach (var f in Directory.EnumerateFiles(d))
            {
                var fi = new FileInfo(f);
                fi.Attributes = FileAttributes.Normal;
                fi.Delete();
            }
            Directory.Delete(d);
        }
    }
}