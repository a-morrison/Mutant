﻿using System;
using ManyConsole;
using Mutant.Deploy.Factory.TestLevels;
using Mutant.Deploy.Factory.Artificers;
using Mutant.Deploy;

namespace Mutant.Core.Commands
{
    class DeployCommand : ConsoleCommand
    {
        private bool IsNonSelective = false;
        private bool RunAllTests = false;
        private bool RunSelectiveTests = false;

        public DeployCommand()
        {
            this.IsCommand("Deploy", "Deploys changes");

            this.HasLongDescription("Extra info");

            this.HasOption("a|all:", "Optional. If not used, tool defaults to selective deployment. Non-Selective deployment. Pushes all objects regardless of status.", 
                v => IsNonSelective = v == null ? true : Convert.ToBoolean(v));
            this.HasOption("t|run-tests:", "Optional. Required if pushing to production.", v => RunAllTests = v == null ? true : Convert.ToBoolean(v));
            this.HasOption("s|selective-tests:", "Optional. If chosen runs tests based on @test annotation in class.", v => RunSelectiveTests = v == null ? true : Convert.ToBoolean(v));
        }

        public override int Run(string[] remainingArguments)
        {
            Console.Out.WriteLine("Deploy called!");
            TestLevelFactory TestLevel = new NoTestsFactory();
            ArtificerFactory Artificer = new SelectiveFactory();

            if (IsNonSelective)
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

            Deployment deployment = new Deployment(TestLevel, Artificer);
            deployment.Deploy();

            return 0;
        }
    }
}
