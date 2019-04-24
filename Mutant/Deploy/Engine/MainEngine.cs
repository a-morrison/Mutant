using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mutant.Deploy.Factory.TestLevels;
using Mutant.Core;
using System.IO;
using System.Xml.Linq;

namespace Mutant.Deploy.Engine
{
    public class MainEngine
    {
        private string _target;
        private ITestLevel _testLevel;

        public MainEngine(ITestLevel TestLevel, string Target)
        {
            this._target = Target;
            this._testLevel = TestLevel;
        }

        public void Run()
        {
            Console.WriteLine("Run called!");
            string AntHome = Environment.GetEnvironmentVariable("ant_home");
            using (Process ant = new Process())
            {
                ant.StartInfo.UseShellExecute = false;
                ant.StartInfo.FileName = AntHome + @"\bin\ant.bat";
                ant.StartInfo.Arguments = BuildArguments();

                ant.Start();
                ant.WaitForExit();
            }
        }

        private string BuildArguments()
        {
            Credentials creds = new Credentials();
            string BaseCommand = GetBaseCommand(creds);
            FindTests(creds);
            string Command = BaseCommand + _target;

            return Command;
        }

        private string GetBaseCommand(Credentials creds)
        {
            string BuildFile = AppContext.BaseDirectory + "build.xml";
            string BaseCommand = "-buildfile " +
                "\"" + BuildFile + "\" " +
                "\"-Dsf.serverurl=" +
                creds.URL +
                "\" " +
                "\"-Dsf.username=" +
                creds.Username +
                "\" " +
                "\"-Dsf.password=" +
                creds.Password +
                "\" " +
                "\"-Dsf.workingdirectory=" +
                creds.WorkingDirectory +
                "\" " +
                "\"-Dsf.testlevel=" +
                _testLevel.Level +
                "\" ";
            return BaseCommand;
        }

        private void FindTests(Credentials creds)
        {
            DirectoryInfo Source = new DirectoryInfo(creds.WorkingDirectory + @"\deploy\artifacts\src\classes");
            List<string> Tests = _testLevel.FindTests(Source);
            if (Tests.Count != 0)
            {
                CreateTestsXML(Tests);
            }
        }

        private void CreateTestsXML(List<string> Tests)
        {
            XDocument TestsXML = new XDocument();
            foreach (string Test in Tests)
            {
                XElement runTest = new XElement("runTest", Test);
                TestsXML.Add(runTest);
            }
            string OutputFile = AppContext.BaseDirectory + "tests.xml";
            TestsXML.Save(OutputFile);
        }
    }
}
