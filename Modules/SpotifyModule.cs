﻿using Discord;
using Discord.Interactions;
using Hermes.Services;
using SpotifyAPI.Web;

namespace Hermes.Modules;

[Group("spotify", "spotify command group")]
public class SpotifyModule : InteractionModuleBase<SocketInteractionContext>
{
    public SpotifyService SpotifyService { get; set; } = null!;

    [SlashCommand("track", "meow")]
    public async Task Track()
    {
        var track = await SpotifyService.GetTrack("429IbFR4yp2J81CeTwF5iY");
        var embedBuilder = new EmbedBuilder { Title = track.Name, Description = track.Artists.First().Name, Color = Color.Red, ThumbnailUrl = track.Album.Images.FirstOrDefault(x => x.Height == 640)?.Url };

        await RespondAsync(embed: embedBuilder.Build());
    }

    [SlashCommand("query", "meow")]
    public async Task Query(string query)
    {
        await DeferAsync();
        var embeds = await SpotifyService.Search(SearchRequest.Types.Track, query);
        var components = new ComponentBuilder();
        components.AddRow(new ActionRowBuilder().WithButton("Test", style: ButtonStyle.Link, url: @"https://www.youtube.com/watch?v=dXKwnfygss8"));

        await FollowupAsync(embeds: embeds.Select(x => x.Build()).ToArray()[..9], components: components.Build());
    }
}
