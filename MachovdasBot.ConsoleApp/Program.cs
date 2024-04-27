using Discord;
using Discord.WebSocket;
using MachovdasBot.ConsoleApp.Commands;
using System.Configuration;

namespace MachovdasBot.ConsoleApp
{
    public class Program
    {
        private static DiscordSocketClient _client;
        private static readonly ulong _guildID = ulong.Parse(ConfigurationManager.AppSettings["guildID"]);

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
            await ConsoleInput();
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

                    await ConsoleInput();
                    break;

                case "flush":
                    await Flush();

                    await ConsoleInput();
                    break;

                default:
                    Console.WriteLine("Invalid Console Command");

                    await ConsoleInput();
                    break;
            }
        }

        private static async Task Flush()
        {
            var globalCommands = await _client.GetGlobalApplicationCommandsAsync();
            var guildCommands = await _client.Rest.GetGuildApplicationCommands(_guildID);

            Console.WriteLine("\nDeleting commands...\n");
            foreach (var command in globalCommands)
            {
                int count = 0;
                int amount = globalCommands.Count;

                Console.WriteLine($"DELETED GLOBAL COMMAND {command.Name} ({count + 1}/{amount})");
                await command.DeleteAsync();
            }

            foreach (var command in guildCommands)
            {
                int count = 0;
                int amount = guildCommands.Count;

                Console.WriteLine($"DELETED GUILD COMMAND {command.Name} ({count + 1}/{amount})");
                await command.DeleteAsync();
            }

            Console.WriteLine("\nInitializing new commands...\n");
            try
            {
                await GetVersion.Initialize(_client, _guildID);
                await Echo.Initialize(_client, _guildID);
                await GetBoardId.Initialize(_client, _guildID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("\nConsole ready");
        }
    }
}
