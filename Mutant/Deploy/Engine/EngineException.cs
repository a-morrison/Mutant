using System;

namespace Mutant.Deploy.Engine
{
    class EngineException : Exception
    {
        public EngineException(string message) : base(message)
        {
        }
    }
}
