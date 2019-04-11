using System;

namespace Mutant.Deploy.Factory.TestLevels
{
    public class TestLevelFactory : AbstractTestLevelFactory
    {
        public override ITestLevel CreateTestLevel(string Type)
        {
            switch (Type)
            {
                case "All":
                    return new AllTests();
                case "None":
                    return new NoTests();
                case "Some":
                    return new SomeTests();
                default:
                    string Message = GetArgumentMessage();
                    throw new ArgumentException(Message);
            }
        }

        private string GetArgumentMessage()
        {
            string Message = "Test Level should be one of the following: ";
            foreach (string Type in this.TYPES)
            {
                Message = String.Concat(Message, Type + ", ");
            }
            Message = Message.Remove(Message.LastIndexOf(','));
            return Message;
        }
    }
}
