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
                WorkingDirectory = @"C:\temp\MutantTests"
            };

            
            Directory.CreateDirectory(MutantInfo.WorkingDirectory);
            Directory.SetCurrentDirectory(MutantInfo.WorkingDirectory);
            using (StreamWriter file = File.CreateText(MutantInfo.WorkingDirectory + @"\.credentials"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, MutantInfo);
            }
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\src\classes");
            //Directory.Delete(Directory.GetCurrentDirectory() + @"\.git", true);
            File.Create(Directory.GetCurrentDirectory() + @"\src\package.xml");
            Collection<PSObject> results = RunPowershellCommand("git init");

            File.Create(Directory.GetCurrentDirectory() + @"\src\classes\test1.cls");
            File.Create(Directory.GetCurrentDirectory() + @"\src\classes\test1.cls-meta.xml");
            Console.WriteLine(File.Exists(@"src\classes\test1.cls"));
            Console.WriteLine("git add " + Directory.GetCurrentDirectory() + "\\src\\classes\\test1.cls");
            results = RunPowershellCommand("git add " + Directory.GetCurrentDirectory() + "\\src\\classes\\test1.cls");
            results = RunPowershellCommand("git status");
            results = RunPowershellCommand("git commit -m \"test\"");
            results = RunPowershellCommand("git rev-parse HEAD");
            string BaseCommit = results[0].ToString();
            
            File.Create(Directory.GetCurrentDirectory() + @"\src\classes\test2.cls");
            File.Create(Directory.GetCurrentDirectory() + @"\src\classes\test2.cls-meta.xml");
            results = RunPowershellCommand("git add " + Directory.GetCurrentDirectory() + @"\src\classes\test2.cls");
            results = RunPowershellCommand("git commit -m \"testagain\"");
            

            ArtificerFactory Factory = new SelectiveFactory();
            Artificer artificer = Factory.CreateArtificer();
            
            artificer.BaseCommit = BaseCommit;

            artificer.CreateArtifact();

            Assert.IsTrue(File.Exists(MutantInfo.WorkingDirectory + @"\deploy\artifacts\src\classes\test1.cls"));
        }

        private Collection<PSObject> RunPowershellCommand(string Command)
        {
            string directory = Directory.GetCurrentDirectory();
            Collection<PSObject> results = new Collection<PSObject>();

            using (PowerShell powershell = PowerShell.Create())
            {
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
    }
}