using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mutant.Deploy.Factory.TestLevels;

namespace Mutant.Deploy.Engine
{
    class MainEngine
    {
        private TestLevel TestLevel;

        public MainEngine(TestLevel TestLevel)
        {
            this.TestLevel = TestLevel;
        }

        public void Run()
        {
            string AntHome = Environment.GetEnvironmentVariable("ant_home");
            using (Process ant = new Process())
            {
                ant.StartInfo.UseShellExecute = false;
                ant.StartInfo.FileName = AntHome + @"\bin\ant.bat";
                ant.StartInfo.CreateNoWindow = true;
                ant.Start();
            }
        }
    }
}
