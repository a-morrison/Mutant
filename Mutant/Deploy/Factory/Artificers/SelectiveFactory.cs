using System;

namespace Mutant.Deploy.Factory.Artificers
{
    class SelectiveFactory : ArtificerFactory
    {
        public override Artificer CreateArtificer()
        {
            return new SelectiveArtificer();
        }
    }
}
