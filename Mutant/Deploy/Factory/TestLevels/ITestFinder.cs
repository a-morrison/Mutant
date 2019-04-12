using System.Collections.Generic;
using System.IO;

namespace Mutant.Deploy.Factory.TestLevels
{
    interface ITestFinder : ITestLevel
    {
        List<string> FindTests(DirectoryInfo SourceDirectory, List<string> Classes);
    }
}
