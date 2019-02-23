using System;
using System.Diagnostics;
using Mutant.Deploy.Factory.TestLevels;
using Mutant.Core;

namespace Mutant.Deploy.Engine
{
    public class MainEngine
    {
        private TestLevel TestLevel;

        public MainEngine(TestLevel TestLevel)
        {
            this.TestLevel = TestLevel;
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
            Credentials creds = Credentials.getInstance();
            string BuildFile = AppContext.BaseDirectory + "build.xml ";
            string Command = "-buildfile " +
                BuildFile +
                "\"Dsf.serverurl=" +
                creds.URL +
                "\" " +
                "\"Dsf.username=" +
                creds.Username +
                "\" " +
                "\"Dsf.password=" +
                creds.Password +
                "\" ";

            return Command;
        }
    }
}
