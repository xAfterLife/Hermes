using Discord;
using Discord.WebSocket;
using Hermes.Models;

namespace Hermes.Commands;

public class Echo : ISlashCommand
{
    public string CommandName => GetType().Name.ToLower();

    public async Task BuildCommand(DiscordSocketClient discord)
    {
        var command = new SlashCommandBuilder()
                      .WithName(CommandName)
                      .WithDescription("Echoing the User-Input")
                      .AddOption("Text", ApplicationCommandOptionType.String, "The Text the Bot will Echo", true);

        await discord.CreateGlobalApplicationCommandAsync(command.Build());
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        await command.RespondAsync($"{command.Data.Options.FirstOrDefault()?.Value}");
    }
}
