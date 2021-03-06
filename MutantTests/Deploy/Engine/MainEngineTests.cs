﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Deploy.Factory.TestLevels;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Mutant.Deploy.Engine.Tests
{
    struct Info
    {
        public string URL;
        public string Username;
        public string Password;
        public string WorkingDirectory;
    }

    [TestClass()]
    public class MainEngineTests
    {
        [TestMethod()]
        public void RunTest()
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

            TestLevelFactory factory = new TestLevelFactory();
            ITestLevel level = factory.CreateTestLevel("None");

            MainEngine engine = new MainEngine(level, "deployZip");
            try
            {
                engine.Run();
            }
            catch (Exception)
            {
                Console.WriteLine("Expected ex.");
            }
        }
    }
}