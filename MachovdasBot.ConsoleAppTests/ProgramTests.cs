using Microsoft.VisualStudio.TestTools.UnitTesting;
using MachovdasBot.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachovdasBot.ConsoleApp.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void GetCommandsTest()
        {
            var commands = Program.GetCommands();
        }
    }
}