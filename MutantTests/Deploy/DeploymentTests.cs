﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy.Factory.TestLevels;
using Newtonsoft.Json;
using System.IO;

namespace Mutant.Deploy.Tests
{
    struct Info
    {
        public string URL;
        public string Username;
        public string Password;
        public string WorkingDirectory;
    }

    [TestClass()]
    public class DeploymentTests
    {
        [TestMethod()]
        public void DeployTest()
        {
            Info MutantInfo = new Info();
            MutantInfo.URL = "Test";
            MutantInfo.Username = "Test";
            MutantInfo.Password = "Test";
            MutantInfo.WorkingDirectory = "Test";

            string CurrentDirectory = Directory.GetCurrentDirectory();
            using (StreamWriter file = File.CreateText(CurrentDirectory + @"\.credentials"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, MutantInfo);
            }

            ArtificerFactory artificerFactory = new SelectiveFactory();
            TestLevelFactory testFactory = new NoTestsFactory();

            Deployment deployment = new Deployment(testFactory, artificerFactory);
            deployment.Deploy();
        }
    }
}