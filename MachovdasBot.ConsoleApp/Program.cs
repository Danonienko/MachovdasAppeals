using MachovdasBot.ConsoleApp.Commands;

namespace MachovdasBot.ConsoleApp
{
    public class Program
    {
        private static async Task Main()
        {
            await new Bot().Start();
        }
    }
}
