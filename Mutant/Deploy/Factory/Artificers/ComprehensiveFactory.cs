namespace Mutant.Deploy.Factory.Artificers
{
    class ComprehensiveFactory : ArtificerFactory
    {
        public override Artificer CreateArtificer()
        {
            return new ComprehensiveArtificer();
        }
    }
}
