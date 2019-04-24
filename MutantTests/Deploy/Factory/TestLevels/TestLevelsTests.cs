using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Deploy.Factory.TestLevels;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MutantTests.Deploy.Factory.TestLevels
{
    [TestClass()]
    public class TestLevelsTests
    {
        private const string HEADER = 
            @"/**
            * @author Alex Morrison 
            * @date 2/13/2019
            *
            * @test SampleTest
            * @description
            */";

        [TestMethod()]
        public void CreateSomeTestsTest()
        {
            CreateFile(@"C:\temp\Example.cls", HEADER);
            CreateFile(@"C:\temp\SampleTest.cls", "test");

            SomeTests Level = new SomeTests();
            List<string> Tests = Level.FindTests(new DirectoryInfo(@"C:\temp"));
            Assert.IsTrue(Tests.Contains("SampleTest"));
        }

        private void CreateFile(string FileName, string Body)
        {
            using (FileStream ExampleClass = File.Create(FileName, 128, FileOptions.None))
            {
                byte[] BodyAsBytes = Encoding.UTF8.GetBytes(Body);
                ExampleClass.Write(BodyAsBytes, 0, BodyAsBytes.Length);
            }
        }
    }
}
