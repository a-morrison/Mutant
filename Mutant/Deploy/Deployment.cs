using Mutant.Deploy.Factory.TestLevels;
using Mutant.Deploy.Factory.Artificers;

namespace Mutant.Deploy
{
    class Deployment
    {
        private TestLevel TestLevel;
        private Artificer Artificer;

        public Deployment(TestLevelFactory TestLevel, ArtificerFactory Artificer)
        {
            this.TestLevel = TestLevel.CreateTestLevel();
            this.Artificer = Artificer.CreateArtificer();
        }

        public void Deploy()
        {
            Artificer.CreateArtifact();
        }
    }
}
