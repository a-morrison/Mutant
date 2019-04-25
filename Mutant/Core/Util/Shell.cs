using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Mutant.Core.Util
{
    public static class Shell
    {
        public static Collection<PSObject> RunCommand(string Directory, string Command)
        {
            Collection<PSObject> results = new Collection<PSObject>();

            using (PowerShell powershell = PowerShell.Create())
            {
                Console.WriteLine(Command);
                powershell.AddScript(String.Format(@"cd {0}", Directory));

                powershell.AddScript(Command);

                results = powershell.Invoke();
            }

            return results;
        }
    }
}
