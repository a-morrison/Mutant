using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mutant.Deploy.Factory.TestLevels;
using Mutant.Core;
using System.IO;

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
            string TestsCommand = GetTestsCommand(creds);
            string Command = BaseCommand + TestsCommand + _target;

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

        private string GetTestsCommand(Credentials creds)
        {
            string TestCommand = "";
            DirectoryInfo Source = new DirectoryInfo(creds.WorkingDirectory + @"\deploy\artifacts\src\classes");
            List<string> Tests = _testLevel.FindTests(Source);
            if (Tests.Count != 0)
            {
                TestCommand = "\"-Dsf.tests=";
                foreach (string Test in Tests)
                {
                    string TestWithComma = Test + ",";
                    TestCommand = String.Concat(TestCommand, TestWithComma);
                }
                TestCommand.Remove(TestCommand.LastIndexOf(","));
                TestCommand += "\" ";
            }
            return TestCommand;
        }
    }
}
