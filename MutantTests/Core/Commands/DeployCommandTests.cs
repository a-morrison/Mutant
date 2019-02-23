using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mutant.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mutant.Core.Commands.Tests
{
    [TestClass()]
    public class DeployCommandTests
    {
        private readonly DeployCommand command;

        public DeployCommandTests()
        {
            command = new DeployCommand();
        }

        [TestMethod()]
        public void IsComprehensiveTest()
        {
            Assert.Fail();
        }
    }
}