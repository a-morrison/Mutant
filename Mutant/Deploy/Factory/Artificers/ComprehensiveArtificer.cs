using System;

namespace Mutant.Deploy.Factory.Artificers
{
    public class ComprehensiveArtificer : Artificer
    {
        public ComprehensiveArtificer() : base(true)
        {
        }

        public override void CreateArtifact()
        {
            //No artifact needed for Comprehensive deployment
        }
    }
}
