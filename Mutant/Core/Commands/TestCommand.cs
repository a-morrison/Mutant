using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Mutant.Core.Commands
{
    class TestCommand : ConsoleCommand
    {
        public TestCommand()
        {
            IsCommand("Test", "Runs test command");
        }

        public override int Run(string[] remainingArguments)
        {
            Console.Out.WriteLine("Test called");

            return 0;
        }
    }
}
