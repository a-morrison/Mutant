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

        public Deployment(TestLevel TestLevel, Artificer Artificer)
        {
            this.TestLevel = TestLevel;
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
