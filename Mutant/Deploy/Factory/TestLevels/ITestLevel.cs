using System.Collections.Generic;
using System.IO;

namespace Mutant.Deploy.Factory.TestLevels
{
    public interface ITestLevel
    {
        string Level
        {
            get;
        }

        List<string> FindTests(DirectoryInfo SourceDirectory);
    }
}
