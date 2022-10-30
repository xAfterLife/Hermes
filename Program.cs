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
    internal readonly IConfiguration Configuration;
    internal readonly LoggingService LoggingService;
    internal readonly IServiceProvider Services;
    internal InteractionService InteractionService;

    public Program()
    {
        Configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false)
                        .Build();

        Services = Services = new ServiceCollection()
                              .AddSingleton<DiscordSocketClient>()
                              .AddSingleton<CommandService>()
                              .AddSingleton<HttpClient>()
                              .AddSingleton<LoggingService>()
                              .AddSingleton<InteractionHandlerService>()
                              .AddSingleton<InteractionService>()
                              .AddSingleton(Configuration)
                              .BuildServiceProvider();

        Client = Services.GetRequiredService<DiscordSocketClient>();
        LoggingService = Services.GetRequiredService<LoggingService>();
        InteractionService = Services.GetRequiredService<InteractionService>();
    }

    private static void Main()
    {
        new Program().MainAsync().GetAwaiter().GetResult();
    }

    public async Task MainAsync()
    {
        await Services.GetRequiredService<InteractionHandlerService>().InitializeAsync();

        await Client.LoginAsync(TokenType.Bot, Configuration.GetValue<string>("Token"));
        await Client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }
}
