using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachovdasBot.ConsoleApp.Commands
{
    public class GetBoardId
    {
        public static async Task Initialize(DiscordSocketClient client, ulong guildId)
        {
            var guild = client.GetGuild(guildId);

            var command = new SlashCommandBuilder()
                .WithName("get-board-id")
                .WithDescription("Retrieves the Trello board ID a bot is currently connected to");

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
            await command.RespondAsync($"Trello Board ID: {ConfigurationManager.AppSettings["boardID"]}");
        }

    }
}
