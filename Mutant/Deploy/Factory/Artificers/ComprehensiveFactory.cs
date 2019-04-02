namespace Mutant.Deploy.Factory.Artificers
{
    public class ComprehensiveFactory : ArtificerFactory
    {
        public override Artificer CreateArtificer()
        {
            return new ComprehensiveArtificer(true);
        }
    }
}
