using Mutant.Deploy.Factory.TestLevels;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy.Engine;

namespace Mutant.Deploy
{
    public class Deployment
    {
        private TestLevel TestLevel;
        private Artificer Artificer;
        public string BaseCommit { private get; set; }

        public Deployment(TestLevelFactory TestLevel, ArtificerFactory Artificer)
        {
            this.TestLevel = TestLevel.CreateTestLevel();
            this.Artificer = Artificer.CreateArtificer();
        }

        public void Deploy()
        {
            Artificer.CreateArtifact();
            Artificer.BaseCommit = BaseCommit;

            MainEngine runner = new MainEngine(TestLevel);
            runner.Run();
        }
    }
}
