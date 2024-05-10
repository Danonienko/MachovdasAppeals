using Discord.WebSocket;
using Discord;
using FluentAssertions;

namespace MachovdasBot.ConsoleApp.Commands.Tests
{
    [TestClass()]
    public class BotTests
    {
        [TestMethod()]
        public void GetAllCommandsTest()
        {
            Bot bot = new();
            bot.Start();

            var actual = bot.GetAllCommands().Result;
        }
    }
}