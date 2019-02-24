using Mutant.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;

namespace Mutant.Deploy.Factory.Artificers
{
    public class SelectiveArtificer : Artificer
    {
        public override void CreateArtifact()
        {
            try
            {
                Credentials Creds = Credentials.GetInstance();

                DestroyExistingArtifacts();

                CreateDirectories();

                Collection<PSObject> results = RunPowershellCommand("git diff HEAD^ HEAD --name-only");

                ProcessResults(results, Creds.WorkingDirectory);
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
