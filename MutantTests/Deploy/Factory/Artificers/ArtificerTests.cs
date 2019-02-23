using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Mutant.Deploy.Factory.Artificers.Tests
{
    [TestClass()]
    public class ArtificerTests
    {
        [TestMethod()]
        public void CreateArtifactTest()
        {
            ArtificerFactory Factory = new SelectiveFactory();
            Artificer artificer = Factory.CreateArtificer();

            artificer.CreateArtifact();

            Assert.IsTrue(Directory.Exists(@"deploy\artifacts"));
        }
    }
}