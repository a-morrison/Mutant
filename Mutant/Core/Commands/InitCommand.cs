using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace Mutant.Core.Commands
{
    class InitCommand : ConsoleCommand
    {
        private string URL;
        private string Username;
        private string Password;
        private string Directory;

        public InitCommand()
        {
            this.IsCommand("Init", "Initializes process with common parameters like the required username and password. Should be called first.");

            this.HasRequiredOption("t|target-url=", "Required. URL of target org.", v => URL = v);
            this.HasRequiredOption("u|username=", "Required. Username", v => Username = v);
            this.HasRequiredOption("p|password=", "Required. Password", v => Password = v);
            this.HasRequiredOption("d|working-directory", "Required. Full path of working directory.", v => Directory = v);
        }

        public override int Run(string[] remainingArguments)
        {
            return 0;
        }
    }
}
