using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class Echo
    {
        public static async Task Initialize(DiscordSocketClient client, ulong guildId)
        {
            var guild = client.GetGuild(guildId);

            var command = new SlashCommandBuilder()
                .WithName("echo")
                .WithDescription("Echoes back provided text")
                .AddOption("text", ApplicationCommandOptionType.String, "A text to echo", true);

            try
            {
                await client.Rest.CreateGuildCommand(command.Build(), guildId);
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

            Console.WriteLine("GUILD echo command initialized successfully");
        }

        public static async Task Response(SocketSlashCommand command)
        {
            await command.RespondAsync($"Echo: {command.Data.Options.First().Value}");
        }
    }
}