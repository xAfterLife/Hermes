using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Hermes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes;

internal class Program
{
    internal readonly DiscordSocketClient Client;
    internal readonly LoggingService LoggingService;
    internal readonly IServiceProvider Services;

    public Program()
    {
        var configuration = (IConfiguration)new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json", false)
                                            .Build();

        Services = Services = new ServiceCollection()
                              .AddSingleton<DiscordSocketClient>()
                              .AddSingleton<CommandService>()
                              .AddSingleton<HttpClient>()
                              .AddSingleton<LoggingService>()
                              .AddSingleton<InteractionHandlerService>()
                              .AddSingleton<InteractionService>()
                              .AddSingleton<SpotifyService>()
                              .AddSingleton<JsonService>()
                              .AddSingleton(configuration)
                              .BuildServiceProvider();

        LoggingService = Services.GetRequiredService<LoggingService>();
        Client = Services.GetRequiredService<DiscordSocketClient>();

        LoggingService.InfoAsync("Services Initialized");
    }

    private static void Main()
    {
        new Program().MainAsync().GetAwaiter().GetResult();
    }

    public async Task MainAsync()
    {
        await Services.GetRequiredService<InteractionHandlerService>().InitializeAsync();

        _ = LoggingService.InfoAsync("Starting Bot");
        await Client.LoginAsync(TokenType.Bot, Services.GetRequiredService<IConfiguration>().GetValue<string>("DiscordToken"));
        await Client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }
}
