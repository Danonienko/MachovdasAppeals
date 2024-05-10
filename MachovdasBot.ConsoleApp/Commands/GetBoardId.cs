using System.Configuration;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class GetBoardId
    {
        public static async Task Initialize(DiscordSocketClient client, ulong guildId)
        {
            var guild = client.GetGuild(guildId);

            var command = new SlashCommandBuilder()
                .WithName("get-boards-id")
                .WithDescription("Retrieves the ID of Trello boards the bot is currently connected to");

            try
            {
                await guild.CreateApplicationCommandAsync(command.Build());
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

            Console.WriteLine("GUILD command `get-board-id` initialized successfully");
        }

        public static async Task Response(SocketSlashCommand command)
        {
            await command.RespondAsync($"Testing Board: [CLASSIFIED]" +
                $"\nWatchlist board: {ConfigurationManager.AppSettings["watchlistBoardID"]}" +
                $"\nAppeal Cooldowns board: {ConfigurationManager.AppSettings["appealCooldownBoardID"]}");
        }
    }
}
