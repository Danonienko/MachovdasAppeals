using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class Echo
    {
        public static async Task Initialize(DiscordSocketClient client)
        {
            var globalCommand = new SlashCommandBuilder()
                .WithName("echo")
                .WithDescription("Echo a text")
                .AddOption("string", ApplicationCommandOptionType.String, "A text to echo");

            try
            {
                await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

            client.SlashCommandExecuted += Response;
        }

        private static async Task Response(SocketSlashCommand command)
        {
            await command.RespondAsync($"{command.Data.Options.First().Value}");
        }
    }
}
