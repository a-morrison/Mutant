namespace Mutant.Deploy.Factory.TestLevels
{
    public class SomeTestsFactory : TestLevelFactory
    {
        public override TestLevel CreateTestLevel()
        {
            return new SomeTests();
        }
    }
}
