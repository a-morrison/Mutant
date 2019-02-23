namespace Mutant.Deploy.Factory.TestLevels
{
    public class AllTestsFactory : TestLevelFactory
    {
        public override TestLevel CreateTestLevel()
        {
            return new AllTests();
        }
    }
}
