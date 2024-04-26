using Discord.WebSocket;
using MachovdasBot.ConsoleApp.Commands;

namespace MachovdasBot.ConsoleApp
{
    public class CommandHandler
    {
        public static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.CommandName)
            {
                case "get-version":
                    await GetVersion.Response(command);
                    break;
            }
        }
    }
}
