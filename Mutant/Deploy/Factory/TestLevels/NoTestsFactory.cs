namespace Mutant.Deploy.Factory.TestLevels
{
    class NoTestsFactory : TestLevelFactory
    {
        public override TestLevel CreateTestLevel()
        {
            return new NoTests();
        }
    }
}
