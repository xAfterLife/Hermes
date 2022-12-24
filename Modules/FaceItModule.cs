using System.Security.Cryptography.X509Certificates;
using Discord;
using Discord.Interactions;
using Hermes.Services;
using Microsoft.VisualBasic;

namespace Hermes.Modules;

[Group("faceit", "FaceIt command group")]
public class FaceItModule : InteractionModuleBase<SocketInteractionContext>
{
    public LoggingService LoggingService { get; set; } = null!;
    public FaceItService FaceItService { get; set; } = null!;

    [SlashCommand("getuserbyname", "Gets the UserID by Name")]
    public async Task GetUserByName([Summary("userName", "FaceIt UserName")] string userName)
    {
        try
        {
            var userId = await FaceItService.GetUserByName(userName);

            if (userId == null)
                await RespondAsync("User not found");
            else
                await RespondAsync($"PlayerID: {userId}");
        }
        catch (Exception ex)
        {
            _ = LoggingService.ErrorAsync(ex);
            await RespondAsync("An Error occured, please try again");
        }
    }

    [SlashCommand("userstats-name", "Shows you the stats of the User in CSGO")]
    public async Task UserStatsByName([Summary("userName", "FaceIt ID")] string userName)
    {
        try
        {
            var user = await FaceItService.GetUserByName(userName);
            if (user == null)
            {
                await RespondAsync($"User {userName} not found");
                return;
            }

            var json = await FaceItService.GetUserStats(user.PlayerId);
            if (json == null)
            {
                await RespondAsync($"Stats not found");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithAuthor($"{user.Nickname}\t({user.Games.Csgo.FaceitElo} Elo)", $@"https://beta.leetify.com/assets/images/rank-icons/faceit{user.Games.Csgo.SkillLevel}.png", $@"https://www.faceit.com/de/players/{user.Nickname}");
            embed.WithTitle($"The stats for {user.Nickname} ({user.Games.Csgo.GamePlayerName})");
            embed.WithThumbnailUrl(user.Avatar);
            embed.WithImageUrl(user.CoverImage);
            embed.WithColor(user.Games.Csgo.SkillLevel switch
            {
                10               => Color.DarkRed,
                (> 6) and (< 10) => Color.DarkOrange,
                (> 4) and (< 8)  => Color.Gold,
                (> 1) and (< 4)  => Color.DarkGreen,
                1                => Color.LightGrey,
                _                => Color.DarkPurple
            });

            embed.WithDescription("Ø K/D: " + json.Lifetime.AverageKDRatio
                                                  + "\nØ HS%: " + json.Lifetime.AverageHeadshots
                                                  + "\nWinrate: " + json.Lifetime.WinRate
                                                  + "\nGames: " + json.Lifetime.Matches);

            foreach (var seg in json.Segments)
                embed.AddField(seg.Label,
                               "Ø K/D: " + seg.Stats.AverageKDRatio
                                               + "\nØ Kills: " + seg.Stats.AverageKills
                                               + "\nØ HS%: " + seg.Stats.AverageHeadshots
                                               + "\nWinrate: " + seg.Stats.WinRate
                                               + "\nGames: " + seg.Stats.Matches,
                               true);

            await RespondAsync(embed: embed.Build());
        }
        catch (Exception ex)
        {
            _ = LoggingService.ErrorAsync(ex);
        }
    }

    [SlashCommand("userstats-id", "Shows you the stats of the User in CSGO")]
    public async Task UserStatsById([Summary("userId", "FaceIt ID")] string userId)
    {
        try
        {
            var json = await FaceItService.GetUserStats(userId);

            if (json == null)
            {
                await RespondAsync($"User {userId} not found");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithTitle($"The stats for {userId}");
            embed.WithColor(Color.Red);

            embed.WithDescription("Average K/D: " + json.Lifetime.AverageKDRatio
                                                  + "\nAverage Headshot%: " + json.Lifetime.AverageHeadshots
                                                  + "\nOverall Winrate: " + json.Lifetime.WinRate
                                                  + "\nMatches Played: " + json.Lifetime.Matches);

            foreach (var seg in json.Segments)
                embed.AddField(seg.Label,
                               "Average K/D: " + seg.Stats.AverageKDRatio
                                               + "\nAverage Kills: " + seg.Stats.AverageKills
                                               + "\nAverage Headshot%: " + seg.Stats.AverageHeadshots
                                               + "\nOverall Winrate: " + seg.Stats.WinRate
                                               + "\nMatches Played: " + seg.Stats.Matches,
                               true);

            await RespondAsync(embed: embed.Build());
        }
        catch (Exception ex)
        {
            _ = LoggingService.ErrorAsync(ex);
        }
    }
}
