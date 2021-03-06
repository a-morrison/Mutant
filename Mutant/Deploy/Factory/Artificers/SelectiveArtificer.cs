﻿using Mutant.Core;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using Mutant.Core.Util;

namespace Mutant.Deploy.Factory.Artificers
{
    public class SelectiveArtificer : Artificer
    {
        public override string Target => "deployZip";

        public SelectiveArtificer() : base(true)
        {
        }

        public override void CreateArtifact()
        {
            try
            {
                Credentials Creds = new Credentials();

                string Difference = "HEAD^ HEAD";
                if (!String.IsNullOrEmpty(BaseCommit))
                {
                    Difference = "HEAD " + BaseCommit;
                }

                string Command = String.Format("git diff-tree --no-commit-id --name-status -r {0}", Difference);
                Collection<PSObject> results = Shell.RunCommand(Creds.WorkingDirectory, Command);

                ProcessResults(results, Creds.WorkingDirectory);
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
