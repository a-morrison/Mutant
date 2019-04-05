namespace Mutant.Deploy.Factory.Artificers
{
    public interface IArtificerFactory
    {
        Artificer GetArtificer(string Type);
    }
}
