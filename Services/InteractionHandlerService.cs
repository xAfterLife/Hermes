using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Services;

public class InteractionHandlerService
{
    private readonly DiscordSocketClient _client;
    private readonly IConfiguration _configuration;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;

    public InteractionHandlerService(DiscordSocketClient client, InteractionService handler, IServiceProvider services, IConfiguration config)
    {
        _client = client;
        _handler = handler;
        _services = services;
        _configuration = config;
    }

    public async Task InitializeAsync()
    {
        // Process when the client is ready, so we can register our commands.
        _client.Ready += ReadyAsync;

        // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        // Process the InteractionCreated payloads to execute Interactions commands
        _client.InteractionCreated += HandleInteraction;
    }

    private async Task ReadyAsync()
    {
        var commands =
#if DEBUG
            await _handler.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("testGuild"));
#else
            await _handler.RegisterCommandsGloballyAsync();
#endif

        foreach ( var command in commands )
            _ = _services.GetRequiredService<LoggingService>().DebugAsync($"Name:{command.Name} Type.{command.Type} loaded");
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_client, interaction);

            // Execute the incoming command.
            var result = await _handler.ExecuteCommandAsync(context, _services);

            if ( !result.IsSuccess )
                switch ( result.Error )
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                }
        }
        catch
        {
            // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if ( interaction.Type is InteractionType.ApplicationCommand )
                await interaction.GetOriginalResponseAsync().ContinueWith(async msg => await msg.Result.DeleteAsync());
        }
    }
}
