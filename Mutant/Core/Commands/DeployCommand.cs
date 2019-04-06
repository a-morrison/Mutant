using System;
using ManyConsole;
using Mutant.Deploy.Factory.TestLevels;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy;

namespace Mutant.Core.Commands
{
    public class DeployCommand : ConsoleCommand
    {
        private string ArtificeType = "Selective";
        private string TestType = "None";
        private string BaseCommit;

        public DeployCommand()
        {
            this.IsCommand("Deploy", "Deploys changes");

            this.HasOption("d|deployment-type:", "Optional. If not used, tool defaults to selective deployment. " +
                "Comprehensive deployment. Pushes all objects regardless of status.", 
                v => ArtificeType = v);
            this.HasOption("t|test-level:", "Optional. Specifies test level.", 
                v => TestType = v);
            this.HasOption("c|base-commit:", "Optional. Deploys changes from HEAD to specified commit hash.", 
                v => BaseCommit = v);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                TestLevelFactory TestLevel = new TestLevelFactory();
                TestLevel tests = TestLevel.CreateTestLevel(TestType);

                ArtificerFactory ArtificerFactory = new ArtificerFactory();
                Artificer artificer = ArtificerFactory.GetArtificer(ArtificeType);

                Deployment deployment = new Deployment(tests, artificer);
                deployment.BaseCommit = BaseCommit;

                deployment.Deploy();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }
    }
}
