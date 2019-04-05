using System;
using System.Collections.Generic;
using System.Text;

namespace Mutant.Deploy.Factory.Artificers
{
    class ArtificerFactory : IArtificerFactory
    {
        public Artificer GetArtificer(string Type)
        {
            switch (Type)
            {
                case "Comprehensive":
                    return new ComprehensiveArtificer(true);
                case "Selective":
                    return new SelectiveArtificer();
                default:
                    return null;
            }
        }
    }
}
