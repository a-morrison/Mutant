using Mutant.Deploy.Factory.TestLevels;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy.Engine;

namespace Mutant.Deploy
{
    public class Deployment
    {
        private ITestLevel TestLevel;
        private Artificer Artificer;

        public Deployment(ITestLevel TestLevel, Artificer Artificer)
        {
            this.TestLevel = TestLevel;
            this.Artificer = Artificer;
        }

        public void Deploy()
        {
            Artificer.CreateArtifact();

            //create Test level;

            MainEngine runner = new MainEngine(TestLevel, Artificer.Target);
            runner.Run();
        }
    }
}
