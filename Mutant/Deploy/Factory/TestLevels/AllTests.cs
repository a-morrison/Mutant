using System.Collections.Generic;
using System.IO;

namespace Mutant.Deploy.Factory.TestLevels
{
    public class AllTests : ITestLevel
    {
        public string Level => "RunLocalTests";

        public List<string> FindTests(DirectoryInfo SourceDirectory)
        {
            return new List<string>();
        }
    }
}
