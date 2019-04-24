using System;

namespace Mutant.Deploy.Factory.Artificers
{
    public class ArtificerFactory : AbstractArtificerFactory
    {
        public override Artificer GetArtificer(string Type)
        {
            switch (Type)
            {
                case "Comprehensive":
                    return new ComprehensiveArtificer();
                case "Selective":
                    return new SelectiveArtificer();
                default:
                    string Message = GetArgumentMessage();
                    throw new ArgumentException(Message);
            }
        }

        private string GetArgumentMessage()
        {
            string Message = "Type should be one of the following: ";
            foreach (string Type in this.TYPES)
            {
                Message = String.Concat(Message, Type + ", ");
            }
            Message = Message.Remove(Message.LastIndexOf(','));
            return Message;
        }
    }
}
