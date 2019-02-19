using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;
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

            ICommand command = new DeploySomeCommand();

            if (IsNonSelective && RunAllTests)
            {
                command = new DeployAllCommand();
            }

            if (IsNonSelective && !RunAllTests)
            {
                command = new DeployAllNoTestsCommand();
            }

            if (!IsNonSelective && !RunAllTests)
            {
                command = new DeploySomeNoTestsCommand();
            }

            if (!IsNonSelective && RunSelectiveTests)
            {
                command = new DeploySomeSelectedTestsCommand();
            }

            command.Deploy();

            return 0;
        }
    }
}
