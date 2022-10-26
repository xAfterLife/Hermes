using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hermes.Models;
using Hermes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes;

internal class Program
{
    internal DiscordSocketClient? Client;

    private static void Main()
    {
        new Program().MainAsync().GetAwaiter().GetResult();
    }

    public async Task MainAsync()
    {
        await using var services = ConfigureServices();
        var logger = services.GetRequiredService<LoggingService>();
        await logger.DebugAsync("Initializing Services");

        Client = services.GetRequiredService<DiscordSocketClient>();
        await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.User));
        await Client.StartAsync();

        Client.Ready += () => Task.Run(() => InitializeServices(UtilityService.GetInitializableServices(services)));
        await Task.Delay(Timeout.Infinite);
    }

    private static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton<CommandService>()
               .AddSingleton<HttpClient>()
               .AddSingleton<UtilityService>()
               .AddSingleton<SlashCommandHandlerService>()
               .AddSingleton<LoggingService>()
               .BuildServiceProvider();
    }

    private static async void InitializeServices(List<IService> services)
    {
        try
        {
            foreach ( var service in services )
                await service.InitializeAsync();
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
