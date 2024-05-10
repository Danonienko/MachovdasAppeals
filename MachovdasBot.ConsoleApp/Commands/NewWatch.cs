using Manatee.Trello;
using System.Configuration;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class NewWatch
    {
        private static DiscordSocketClient _client;

        public static async Task Initialize(DiscordSocketClient client, ulong guildId)
        {
            _client = client;

            var command = new SlashCommandBuilder()
                .WithName("new-watch")
                .WithDescription("Adds new player to watch into Watchlist")
                .AddOption("username", ApplicationCommandOptionType.String, "Player's Username", true)
                .AddOption("reason", ApplicationCommandOptionType.String, "Suspect reason", true)
                .AddOption("string", ApplicationCommandOptionType.String, "Text evidence", false)
                .AddOption("attachment", ApplicationCommandOptionType.Attachment, "Graphical evidence", false)
                .AddOption("extra-notes", ApplicationCommandOptionType.String, "Any extra notes that could be useful", false);

            try
            {
                await client.Rest.CreateGuildCommand(command.Build(), guildId);
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine($"GUILD command `new-wach` could not be initialized \n\n{json}");

                return;
            }

            Console.WriteLine("GUILD command `new-watch` initialized successfully");
        }

        public static async Task Response(SocketSlashCommand command)
        {
            var guild = _client.GetGuild(command.GuildId.Value);
            if (command.User.Id != guild.OwnerId)
            {
                await command.RespondAsync("You do not have permission to run this command");
            }

            var boardId = ConfigurationManager.AppSettings["watchlistBoardID"];
            var trelloKey = Environment.GetEnvironmentVariable("trelloKey", EnvironmentVariableTarget.User);
            var trelloToken = Environment.GetEnvironmentVariable("trelloToken", EnvironmentVariableTarget.User);

            var auth = new TrelloAuthorization()
            {
                AppKey = trelloKey,
                UserToken = trelloToken
            };

            var list = new List(boardId, auth);
            
            var options = command.Data.Options.ToList();

            string description = 
                $"**Name of Moderator**:{command.User.GlobalName}" +
                $"\n\n**Name of User Being Watchlisted**:{options[0].Value}" +
                $"\n\n**Date**:{DateTime.Now}" +
                $"\n\n**Reason**:{options[1].Value}" +
                $"\n\n**Relevant Evidence**:{options[2].Value}" +
                $"\n\n**Extra Notes**:{options[4].Value}";

            await command.RespondAsync("Command executed successfully");
        }

    }
}
