using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mutant.Core.Tests
{
    [TestClass()]
    public class CredentialsTests
    {
        [TestMethod()]
        public void TestNoFile()
        {
            Credentials Creds = null;
            try
            {
                Creds = Credentials.GetInstance();
            } catch (Exception ex)
            {

            }

            Assert.IsNull(Creds);
        }

        [TestMethod()]
        public void TestFileExists()
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

            Credentials Creds = Credentials.GetInstance();

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