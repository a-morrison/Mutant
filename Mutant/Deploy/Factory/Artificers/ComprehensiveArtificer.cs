using System;

namespace Mutant.Deploy.Factory.Artificers
{
    public class ComprehensiveArtificer : Artificer
    {
        public override string Target => "comprehensiveDeploment";

        public ComprehensiveArtificer() : base(true)
        {
        }

        public override void CreateArtifact()
        {
            //No artifact needed for Comprehensive deployment
        }
    }
}
