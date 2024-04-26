using Discord;
using Discord.WebSocket;

namespace MachovdasBot.ConsoleApp
{
    public class Program
    {
        private static DiscordSocketClient _client;

        public static async Task Main()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("botToken", EnvironmentVariableTarget.User));
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
