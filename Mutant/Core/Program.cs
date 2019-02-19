using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;
using Mutant.Core.Commands;
using System.Diagnostics;

namespace Mutant.Core
{
    class Program
    {
        static int Main(string[] args)
        {
            var commands = GetCommands();

            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
