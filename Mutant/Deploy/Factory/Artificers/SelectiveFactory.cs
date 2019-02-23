namespace Mutant.Deploy.Factory.Artificers
{
    public class SelectiveFactory : ArtificerFactory
    {
        public override Artificer CreateArtificer()
        {
            return new SelectiveArtificer();
        }
    }
}
