using System.Collections.Generic;

namespace Mutant.Deploy.Factory.TestLevels
{
    public abstract class AbstractTestLevelFactory
    {
        public readonly List<string> TYPES = new List<string>
        {
            "All",
            "None",
            "Some"
        };

        public abstract ITestLevel CreateTestLevel(string Type);
    }
}
