using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutant.Deploy.Factory.TestLevels
{
    class AllTestsFactory : TestLevelFactory
    {
        public override TestLevel CreateTestLevel()
        {
            return new AllTests();
        }
    }
}
