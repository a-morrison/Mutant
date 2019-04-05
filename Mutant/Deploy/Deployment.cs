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

        public Deployment(TestLevelFactory TestLevel, Artificer Artificer)
        {
            this.TestLevel = TestLevel.CreateTestLevel();
            this.Artificer = Artificer;
        }

        public void Deploy()
        {
            Artificer.BaseCommit = BaseCommit;
            Artificer.CreateArtifact();

            MainEngine runner = new MainEngine(TestLevel);
            runner.Run();
        }
    }
}
