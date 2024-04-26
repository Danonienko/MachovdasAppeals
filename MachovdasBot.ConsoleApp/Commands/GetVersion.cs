using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class GetVersion()
    {
        public async Task Initialize(DiscordSocketClient client)
        {
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("get-version");
            globalCommand.WithDescription("Gets the current version of the bot.");

            try
            {
                await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

            client.SlashCommandExecuted += SlashCommandHandler;
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"Machovdas Bot Version 1.0");
        }
    }
}
