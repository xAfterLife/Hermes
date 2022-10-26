using Discord.WebSocket;

namespace Hermes.Models;

public interface ISlashCommand
{
    public string CommandName { get; }
    public Task BuildCommand(DiscordSocketClient discord);
    public Task ExecuteAsync(SocketSlashCommand command);
}
