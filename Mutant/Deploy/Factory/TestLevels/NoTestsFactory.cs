namespace Mutant.Deploy.Factory.TestLevels
{
    public class NoTestsFactory : TestLevelFactory
    {
        public override TestLevel CreateTestLevel()
        {
            return new NoTests();
        }
    }
}
