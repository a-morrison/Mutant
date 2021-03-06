﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Newtonsoft.Json;

namespace Mutant.Core.Tests
{
    [TestClass()]
    public class CredentialsTests
    {
        [TestMethod()]
        public void TestNoFile()
        {
            string WorkingDirectory = Directory.GetCurrentDirectory();
            Credentials Creds = null;
            try
            {
                Creds = new Credentials();
            } catch (Exception)
            {
                Assert.IsFalse(File.Exists(WorkingDirectory + @"\.credentials"));
                Assert.IsNull(Creds);
            }
        }

        [TestMethod()]
        public void TestFileExists()
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

            Credentials Creds = new Credentials();

            StringAssert.Equals(Creds.Username, MutantInfo.Username);
            StringAssert.Equals(Creds.Password, MutantInfo.Password);
            StringAssert.Equals(Creds.URL, MutantInfo.URL);
            StringAssert.Equals(Creds.WorkingDirectory, MutantInfo.WorkingDirectory);
        }

        struct Info
        {
            public string URL;
            public string Username;
            public string Password;
            public string WorkingDirectory;
        }
    }
}