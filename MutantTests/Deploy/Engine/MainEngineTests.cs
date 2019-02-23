using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Deploy.Engine;
using Mutant.Deploy.Factory.TestLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutant.Deploy.Engine.Tests
{
    [TestClass()]
    public class MainEngineTests
    {
        [TestMethod()]
        public void RunTest()
        {
            TestLevelFactory factory = new NoTestsFactory();
            TestLevel level = factory.CreateTestLevel();

            MainEngine engine = new MainEngine(level);
            engine.Run();
        }
    }
}