namespace Mutant.Deploy.Factory.TestLevels
{
    class SomeTestsFactory : TestLevelFactory
    {
        public override TestLevel CreateTestLevel()
        {
            return new SomeTests();
        }
    }
}
