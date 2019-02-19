using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Mutant.Core.Commands
{
    class DeployCommand : ConsoleCommand
    {

        private bool IsNonSelective = false;
        private bool RunAllTests = false;

        public DeployCommand()
        {
            this.IsCommand("Deploy", "Deploys changes");

            this.HasLongDescription("Extra info");

            this.HasOption("a|all:", "Optional. If not used, tool defaults to selective deployment. Non-Selective deployment. Pushes all objects regardless of status.", 
                v => IsNonSelective = v == null ? true : Convert.ToBoolean(v));
        }

        public override int Run(string[] remainingArguments)
        {
            Console.Out.WriteLine("Deploy called!");

            if (IsNonSelective)
            {
                Console.Out.WriteLine("Deploying all objects!");
            }

            return 0;
        }
    }
}
