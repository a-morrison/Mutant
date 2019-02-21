using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;

namespace Mutant.Deploy.Factory.Artificers
{
    class SelectiveArtificer : Artificer
    {
        public override void CreateArtifact()
        {
            string workingDir = @"C:\Users\Alex Morrison\Documents\MutantTesting\DOH\DC%20DOH\";
            Directory.SetCurrentDirectory(workingDir);
            DestroyExistingArtifacts();

            CreateDirectories();
            
            Collection<PSObject> results = RunPowershellCommand("git diff --name-only");

            ProcessResults(results, workingDir);
        }
    }
}
