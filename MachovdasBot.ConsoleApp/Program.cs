using Discord;
using Discord.WebSocket;
using MachovdasBot.ConsoleApp.Commands;

namespace MachovdasBot.ConsoleApp
{
    public class Program
    {
        private static DiscordSocketClient _client;
        private static readonly ulong _guildID = 514399873710161921;

        public static async Task Main()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("botToken", EnvironmentVariableTarget.User));
            await _client.StartAsync();


            _client.Ready += ClientReady;

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private static async Task ClientReady()
        {
            _client.SlashCommandExecuted += CommandHandler.SlashCommandHandler;

            Console.WriteLine("\nConsole ready to use. Available commands:");
            GetHelp();
            ConsoleInput();
        }
        private static void GetHelp()
        {
            Console.WriteLine("\nhelp - Get list of all available commands" +
                "\nflush - Deletes and creates all GLOBAL and GUILD commands.");
        }

        private static async Task ConsoleInput()
        {
            var consoleInput = Console.ReadLine();

            switch (consoleInput)
            {
                case "help":
                    GetHelp();

                    ConsoleInput();
                    break;

                case "flush":
                    await Flush();

                    ConsoleInput();
                    break;

                default:
                    Console.WriteLine("Invalid Console Command");

                    ConsoleInput();
                    break;
            }
        }

        private static async Task Flush()
        {
            var globalCommands = await _client.GetGlobalApplicationCommandsAsync();
            var guildCommands = await _client.Rest.GetGuildApplicationCommands(_guildID);

            foreach (var command in globalCommands)
            {
                Console.WriteLine($"DELETED GLOBAL COMMAND {command.Name}");
                await command.DeleteAsync();
            }

            foreach (var command in guildCommands)
            {
                Console.WriteLine($"DELETED GUILD COMMAND {command.Name}");
                await command.DeleteAsync();
            }

            try
            {
                await GetVersion.Initialize(_client, _guildID);
                await Echo.Initialize(_client, _guildID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
