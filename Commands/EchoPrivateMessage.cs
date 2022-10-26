using Discord;
using Discord.WebSocket;
using Hermes.Models;

namespace Hermes.Commands;

public class EchoPrivateMessage : ISlashCommand
{
    public string CommandName => GetType().Name.ToLower();

    public async Task BuildCommand(DiscordSocketClient discord)
    {
        var command = new SlashCommandBuilder()
                      .WithName(CommandName)
                      .WithDescription("Echoing the User-Input in Privat Message")
                      .AddOption("Text", ApplicationCommandOptionType.String, "The Text the Bot will Echo", true);

        await discord.CreateGlobalApplicationCommandAsync(command.Build());
    }

    public async Task ExecuteAsync(SocketSlashCommand command)
    {
        await command.DeferAsync(true);
        var channel = await command.User.CreateDMChannelAsync();
        await channel.SendMessageAsync($"{command.Data.Options.FirstOrDefault()?.Value}");
        await channel.CloseAsync();
        await command.FollowupAsync("Ok, done", ephemeral: true);
    }
}
