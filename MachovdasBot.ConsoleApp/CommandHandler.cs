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

                case "echo":
                    await Echo.Response(command);
                    break;

                case "get-boards-id":
                    await GetBoardId.Response(command);
                    break;

                case "new-watch":
                    await NewWatch.Response(command);
                    break;
            }
        }
    }
}
