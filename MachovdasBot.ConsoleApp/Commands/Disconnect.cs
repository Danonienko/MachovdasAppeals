using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class Disconnect()
    {
        private static DiscordSocketClient _client;

        public static async Task Initialize(DiscordSocketClient client)
        {
            _client = client;

            var globalCommand = new SlashCommandBuilder()
                .WithName("disconnect")
                .WithDescription("Kill the connection making the bot offline. DEV ONLY");

            try
            {
                await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException ex)
            {
                var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

            client.SlashCommandExecuted += Respone;
        }

        private static async Task Respone(SocketSlashCommand command)
        {
            await _client.StopAsync();
        }
    }
}
