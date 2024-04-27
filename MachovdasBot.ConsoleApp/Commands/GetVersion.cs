using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class GetVersion
    {
        public static async Task Initialize(DiscordSocketClient client, ulong guildId)
        {
            var guild = client.GetGuild(guildId);

            var command = new SlashCommandBuilder()
                .WithName("get-version")
                .WithDescription("Get current version of the bot");

            try
            {
                await guild.CreateApplicationCommandAsync(command.Build());
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

            Console.WriteLine("GUILD command `get-version` initialized successfully");
        }

        public static async Task Response(SocketSlashCommand command)
        {
            await command.RespondAsync("Machovdas Appeals Bot Version 1.0");
        }
    }
}
