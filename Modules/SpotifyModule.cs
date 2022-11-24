using Discord;
using Discord.Interactions;
using Hermes.Services;
using SpotifyAPI.Web;

namespace Hermes.Modules;

public class SpotifyModule : InteractionModuleBase<SocketInteractionContext>
{
    public SpotifyService SpotifyService { get; set; } = null!;

    [SlashCommand("spotify-track", "meow")]
    public async Task SpotifyTest()
    {
        var track = await SpotifyService.GetTrack("429IbFR4yp2J81CeTwF5iY");
        var embedBuilder = new EmbedBuilder { Title = track.Name, Description = track.Artists.First().Name, Color = Color.Red, ThumbnailUrl = track.Album.Images.FirstOrDefault(x => x.Height == 640)?.Url };

        await RespondAsync(embed: embedBuilder.Build());
    }

    [SlashCommand("spotify-query", "meow")]
    public async Task SpotifyQuery(string query)
    {
        await DeferAsync();
        var embeds = await SpotifyService.Search(SearchRequest.Types.Track, query);
        await FollowupAsync(embeds: embeds.Select(x => x.Build()).ToArray()[..9]);
    }
}
