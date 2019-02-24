using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

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
                WorkingDirectory = "Test"
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
    }
}