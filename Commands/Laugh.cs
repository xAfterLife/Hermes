using Discord;
using Discord.WebSocket;
using Hermes.Models;

namespace Hermes.Commands;

public class Laugh : ISlashCommand
{
    public string CommandName => GetType().Name.ToLower();

    public async Task BuildCommand(DiscordSocketClient discord)
    {
        var command = new SlashCommandBuilder()
                      .WithName(CommandName)
                      .WithDescription("The Bot starts laughing");

        await discord.CreateGlobalApplicationCommandAsync(command.Build());
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        await command.RespondAsync($"Hahaha {command.User.Username} you are so Funny");
    }
}
