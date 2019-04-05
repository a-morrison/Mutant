﻿using System;
using ManyConsole;
using Mutant.Deploy.Factory.TestLevels;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy;

namespace Mutant.Core.Commands
{
    public class DeployCommand : ConsoleCommand
    {
        private string ArtificeType = "Selective";
        private bool IsComprehensive = false;
        private bool RunAllTests = false;
        private bool RunSelectiveTests = false;
        private string BaseCommit;

        public DeployCommand()
        {
            this.IsCommand("Deploy", "Deploys changes");

            this.HasLongDescription("Extra info");

            this.HasOption("a|all:", "Optional. If not used, tool defaults to selective deployment. Comprehensive deployment. Pushes all objects regardless of status.", 
                v => ArtificeType = v);
            this.HasOption("t|run-tests:", "Optional. Required if pushing to production.", v => RunAllTests = v == null ? true : Convert.ToBoolean(v));
            this.HasOption("s|selective-tests:", "Optional. If chosen runs tests based on @test annotation in class.", v => RunSelectiveTests = v == null ? true : Convert.ToBoolean(v));
            this.HasOption("c|base-commit:", "Optional. Deploys changes from HEAD to specified commit hash.", v => BaseCommit = v);
        }

        public override int Run(string[] remainingArguments)
        {
            if (String.Equals())
            TestLevelFactory TestLevel = new NoTestsFactory();
            ArtificerFactory ArtificerFactory = new ArtificerFactory();
            Artificer artificer = ArtificerFactory.GetArtificer(ArtificeType);

            if (IsComprehensive)
            {
                Artificer = new ComprehensiveFactory();
            }

            if (RunAllTests)
            {
                TestLevel = new AllTestsFactory();
            }

            if (RunSelectiveTests)
            {
                TestLevel = new SomeTestsFactory();
            }

            Deployment deployment = new Deployment(TestLevel, artificer);
            if (String.IsNullOrWhiteSpace(BaseCommit))
            {
                deployment.BaseCommit = BaseCommit;
            }

            try
            {
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
