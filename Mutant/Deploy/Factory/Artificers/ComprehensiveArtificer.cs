using System;

namespace Mutant.Deploy.Factory.Artificers
{
    public class ComprehensiveArtificer : Artificer
    {
        public ComprehensiveArtificer(Boolean DisableSetup) : base(DisableSetup)
        {
        }

        public override void CreateArtifact()
        {
            //No artifact needed for Comprehensive deployment
        }
    }
}
