using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Mutant.Core.Commands
{
    class ReportCommand : ConsoleCommand
    {
        public ReportCommand()
        {
            this.IsCommand("Report", "creates report");
        }

        public override int Run(string[] remainingArguments)
        {
            Console.Out.Write("Report called");

            return 0;
        }
    }
}
