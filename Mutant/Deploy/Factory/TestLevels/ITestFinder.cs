using System;
using System.Collections.Generic;
using System.Text;

namespace Mutant.Deploy.Factory.TestLevels
{
    interface ITestFinder : ITestLevel
    {
        void FindTests();
    }
}
