using Discord.WebSocket;
using Hermes.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Services;

public class SlashCommandHandlerService : IService
{
    private readonly DiscordSocketClient _discord;
    private readonly LoggingService _logging;
    internal List<ISlashCommand> Commands = new();

    public SlashCommandHandlerService(IServiceProvider services)
    {
        _discord = services.GetRequiredService<DiscordSocketClient>();
        _logging = services.GetRequiredService<LoggingService>();
    }

    public string ServiceName => GetType().Name;

    public Task InitializeAsync()
    {
        _discord.SlashCommandExecuted += SlashCommandHandler;
        Commands = UtilityService.GetCommands();
        InitializeCommands(Commands);
        return Task.CompletedTask;
    }

    private void InitializeCommands(List<ISlashCommand> commands)
    {
        try
        {
            foreach ( var command in commands )
            {
                command.BuildCommand(_discord);
                _logging.InfoAsync($"Initialized Command: {command.CommandName}");
            }
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private Task SlashCommandHandler(SocketSlashCommand command)
    {
        var com = Commands.FirstOrDefault(x => x.CommandName == command.Data.Name);
        _ = com?.ExecuteAsync(command) ?? command.RespondAsync("Command not found");
        return Task.CompletedTask;
    }
}
