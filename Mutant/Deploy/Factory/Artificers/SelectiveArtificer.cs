using Mutant.Core;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Mutant.Deploy.Factory.Artificers
{
    public class SelectiveArtificer : Artificer
    {
        public override void CreateArtifact()
        {
            try
            {
                Credentials Creds = new Credentials();

                DestroyExistingArtifacts();

                CreateDirectories();

                string Difference = "HEAD^ HEAD";
                if (!String.IsNullOrEmpty(BaseCommit))
                {
                    Difference = "HEAD " + BaseCommit;
                }

                string Command = String.Format("git diff {0} --name-only", Difference);
                Collection<PSObject> results = RunPowershellCommand(Command);

                ProcessResults(results, Creds.WorkingDirectory);
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
