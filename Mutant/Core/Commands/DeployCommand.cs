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
        public DeployCommand()
        {
            this.IsCommand("Deploy", "Deploys changes");

            this.HasLongDescription("Extra info");
        }

        public override int Run(string[] remainingArguments)
        {
            Console.Out.WriteLine("Deploy called!");

            return 0;
        }
    }
}
