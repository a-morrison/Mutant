using System.Collections.Generic;
using System.IO;

namespace Mutant.Deploy.Factory.TestLevels
{
    public class NoTests : ITestLevel
    {
        public string Level => "NoTestRun";

        public List<string> FindTests(DirectoryInfo SourceDirectory)
        {
            return new List<string>();
        }
    }
}
