using System.Collections.Generic;

namespace Mutant.Deploy.Factory.Artificers
{
    public abstract class AbstractArtificerFactory
    {
        public readonly List<string> TYPES = new List<string>
        {
            "Selective",
            "Comprehensive"
        };

        public abstract Artificer GetArtificer(string Type);
    }
}
