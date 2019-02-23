using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy.Factory.TestLevels;

namespace Mutant.Deploy.Tests
{
    [TestClass()]
    public class DeploymentTests
    {
        [TestMethod()]
        public void DeployTest()
        {
            ArtificerFactory artificerFactory = new SelectiveFactory();
            TestLevelFactory testFactory = new NoTestsFactory();

            Deployment deployment = new Deployment(testFactory, artificerFactory);
            deployment.Deploy();
        }
    }
}