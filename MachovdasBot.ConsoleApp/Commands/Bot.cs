using System.Configuration;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class Bot
    {
        private static DiscordSocketClient _client;
        private static readonly ulong _guildID = ulong.Parse(ConfigurationManager.AppSettings["guildID"]);

        public async Task Start()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("botToken", EnvironmentVariableTarget.User));
            await _client.StartAsync();

            _client.Ready += ClientReady;

            await Task.Delay(-1);
        }
        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task ClientReady()
        {
            _client.SlashCommandExecuted += CommandHandler.SlashCommandHandler;

            Console.WriteLine("\nConsole ready to use. Available commands:");
            GetHelp();
            await ConsoleInput();
        }

        private void GetHelp()
        {
            Console.WriteLine("\nhelp - Get list of all available commands" +
                "\nflush - Deletes and creates all GLOBAL and GUILD commands.");
        }

        private async Task ConsoleInput()
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

        public async Task<List<IApplicationCommand>> GetAllCommands()
        {
            var guildCommands = await _client.Rest.GetGuildApplicationCommands(_guildID);
            var globalCommands = await _client.GetGlobalApplicationCommandsAsync();

            var gCommands = guildCommands.Cast<IApplicationCommand>();
            var appCommands = globalCommands.Cast<IApplicationCommand>();

            var allCommands = gCommands.Union(appCommands);

            return allCommands.ToList();
        }

        private async Task Flush()
        {
            var globalCommands = await _client.GetGlobalApplicationCommandsAsync();
            var guildCommands = await _client.Rest.GetGuildApplicationCommands(_guildID);

            int count = 1;
            if (globalCommands.Count != 0)
            {
                Console.WriteLine("\nDeleting global commands...\n");
                foreach (var command in globalCommands)
                {
                    int amount = globalCommands.Count;

                    Console.WriteLine($"GLOBAL command `{command.Name}` DELETED ({count++}/{amount})");
                    await command.DeleteAsync();
                }
            }

            count = 1;
            if (guildCommands.Count != 0)
            {
                Console.WriteLine("Deleting guild commands...");
                foreach (var command in guildCommands)
                {
                    int amount = guildCommands.Count;

                    Console.WriteLine($"GUILD command `{command.Name}` DELETED ({count++}/{amount})");
                    await command.DeleteAsync();
                }
            }

            Console.WriteLine("\nInitializing new commands...\n");
            try
            {
                await GetVersion.Initialize(_client, _guildID);
                await Echo.Initialize(_client, _guildID);
                await GetBoardId.Initialize(_client, _guildID);
                await NewWatch.Initialize(_client, _guildID);
                await FileName.Initialize(_client, _guildID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error ocurred while trying to initialize command. \nException message: \n\n{ex.Message}");
            }

            Console.WriteLine("\nConsole ready");
        }
    }
}
