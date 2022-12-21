using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Services;

public sealed class FaceItService
{
    private readonly string? _apiKey;
    private readonly LoggingService _logger;

    public FaceItService(IServiceProvider services)
    {
        _logger = services.GetRequiredService<LoggingService>();

        var config = services.GetRequiredService<IConfiguration>();

        try
        {
            _apiKey = config.GetValue<string>("FaceItApiKey")!;
            if ( _apiKey == null )
                _logger.WarningAsync("Check spotifyId & Secret");
        }
        catch ( Exception e )
        {
            _logger.ErrorAsync(e);
        }
    }

    public async ValueTask<StatsResponse?> GetUserStats(string userId)
    {
        try
        {
            var url = $"https://open.faceit.com/data/v4/players/{userId}/stats/csgo";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            using var response = await client.GetAsync(url);

            if ( !response.IsSuccessStatusCode )
            {
                await _logger.WarningAsync($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<StatsResponse>(responseStream);

            return json ?? null;
        }
        catch ( Exception ex )
        {
            await _logger.ErrorAsync(ex);
            return null;
        }
    }

    public async ValueTask<UserResponse?> GetUserByName(string userName)
    {
        try
        {
            var url = $"https://open.faceit.com/data/v4/players?nickname={userName}&game=csgo";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            using var response = await client.GetAsync(url);

            if ( !response.IsSuccessStatusCode )
            {
                await _logger.WarningAsync($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<UserResponse>(responseStream);

            return json ?? null;
        }
        catch ( Exception ex )
        {
            await _logger.ErrorAsync(ex);
            return null;
        }
    }

#region Stats

    public record Lifetime([property: JsonPropertyName("Recent Results")] IReadOnlyList<string> RecentResults,
                           [property: JsonPropertyName("Average K/D Ratio")] string AverageKDRatio,
                           [property: JsonPropertyName("Current Win Streak")] string CurrentWinStreak,
                           [property: JsonPropertyName("Average Headshots %")] string AverageHeadshots,
                           [property: JsonPropertyName("Matches")] string Matches,
                           [property: JsonPropertyName("Win Rate %")] string WinRate,
                           [property: JsonPropertyName("Longest Win Streak")] string LongestWinStreak,
                           [property: JsonPropertyName("Total Headshots %")] string TotalHeadshots,
                           [property: JsonPropertyName("Wins")] string Wins,
                           [property: JsonPropertyName("K/D Ratio")] string KDRatio);

    public record StatsResponse([property: JsonPropertyName("player_id")] string PlayerId,
                                [property: JsonPropertyName("game_id")] string GameId,
                                [property: JsonPropertyName("lifetime")] Lifetime Lifetime,
                                [property: JsonPropertyName("segments")] IReadOnlyList<Segment> Segments);

    public record Segment([property: JsonPropertyName("type")] string Type,
                          [property: JsonPropertyName("mode")] string Mode,
                          [property: JsonPropertyName("label")] string Label,
                          [property: JsonPropertyName("img_small")] string ImgSmall,
                          [property: JsonPropertyName("img_regular")] string ImgRegular,
                          [property: JsonPropertyName("stats")] Stats Stats);

    public record Stats([property: JsonPropertyName("Average Quadro Kills")] string AverageQuadroKills,
                        [property: JsonPropertyName("Average Assists")] string AverageAssists,
                        [property: JsonPropertyName("Rounds")] string Rounds,
                        [property: JsonPropertyName("Average Deaths")] string AverageDeaths,
                        [property: JsonPropertyName("Triple Kills")] string TripleKills,
                        [property: JsonPropertyName("Deaths")] string Deaths,
                        [property: JsonPropertyName("Matches")] string Matches,
                        [property: JsonPropertyName("Assists")] string Assists,
                        [property: JsonPropertyName("Average Kills")] string AverageKills,
                        [property: JsonPropertyName("Average Headshots %")] string AverageHeadshots,
                        [property: JsonPropertyName("Quadro Kills")] string QuadroKills,
                        [property: JsonPropertyName("MVPs")] string MVPs,
                        [property: JsonPropertyName("Penta Kills")] string PentaKills,
                        [property: JsonPropertyName("Average MVPs")] string AverageMVPs,
                        [property: JsonPropertyName("K/R Ratio")] string KRRatio,
                        [property: JsonPropertyName("Kills")] string Kills,
                        [property: JsonPropertyName("Average Triple Kills")] string AverageTripleKills,
                        [property: JsonPropertyName("Win Rate %")] string WinRate,
                        [property: JsonPropertyName("Average K/R Ratio")] string AverageKRRatio,
                        [property: JsonPropertyName("K/D Ratio")] string KDRatio,
                        [property: JsonPropertyName("Headshots per Match")] string HeadshotsPerMatch,
                        [property: JsonPropertyName("Average K/D Ratio")] string AverageKDRatio,
                        [property: JsonPropertyName("Total Headshots %")] string TotalHeadshots,
                        [property: JsonPropertyName("Average Penta Kills")] string AveragePentaKills,
                        [property: JsonPropertyName("Wins")] string Wins,
                        [property: JsonPropertyName("Headshots")] string Headshots);

#endregion

#region Match

    public record Entity([property: JsonPropertyName("name")] string Name,
                         [property: JsonPropertyName("class_name")] string ClassName,
                         [property: JsonPropertyName("game_location_id")] string GameLocationId,
                         [property: JsonPropertyName("guid")] string Guid,
                         [property: JsonPropertyName("image_lg")] string ImageLg,
                         [property: JsonPropertyName("image_sm")] string ImageSm,
                         [property: JsonPropertyName("game_map_id")] string GameMapId);

    public record Faction([property: JsonPropertyName("faction_id")] string FactionId,
                          [property: JsonPropertyName("leader")] string Leader,
                          [property: JsonPropertyName("avatar")] string Avatar,
                          [property: JsonPropertyName("roster")] IReadOnlyList<Roster> Roster,
                          [property: JsonPropertyName("substituted")] bool? Substituted,
                          [property: JsonPropertyName("name")] string Name,
                          [property: JsonPropertyName("type")] string Type);

    public record Location([property: JsonPropertyName("entities")] IReadOnlyList<Entity> Entities,
                           [property: JsonPropertyName("pick")] IReadOnlyList<string> Pick);

    public record Map([property: JsonPropertyName("entities")] IReadOnlyList<Entity> Entities,
                      [property: JsonPropertyName("pick")] IReadOnlyList<string> Pick);

    public record Results([property: JsonPropertyName("winner")] string Winner,
                          [property: JsonPropertyName("score")] Score Score);

    public record MatchResponse([property: JsonPropertyName("match_id")] string MatchId,
                                [property: JsonPropertyName("version")] int? Version,
                                [property: JsonPropertyName("game")] string Game,
                                [property: JsonPropertyName("region")] string Region,
                                [property: JsonPropertyName("competition_id")] string CompetitionId,
                                [property: JsonPropertyName("competition_type")] string CompetitionType,
                                [property: JsonPropertyName("competition_name")] string CompetitionName,
                                [property: JsonPropertyName("organizer_id")] string OrganizerId,
                                [property: JsonPropertyName("teams")] Teams Teams,
                                [property: JsonPropertyName("voting")] Voting Voting,
                                [property: JsonPropertyName("calculate_elo")] bool? CalculateElo,
                                [property: JsonPropertyName("configured_at")] int? ConfiguredAt,
                                [property: JsonPropertyName("started_at")] int? StartedAt,
                                [property: JsonPropertyName("finished_at")] int? FinishedAt,
                                [property: JsonPropertyName("demo_url")] IReadOnlyList<string> DemoUrl,
                                [property: JsonPropertyName("chat_room_id")] string ChatRoomId,
                                [property: JsonPropertyName("best_of")] int? BestOf,
                                [property: JsonPropertyName("results")] Results Results,
                                [property: JsonPropertyName("status")] string Status,
                                [property: JsonPropertyName("faceit_url")] string FaceitUrl);

    public record Roster([property: JsonPropertyName("player_id")] string PlayerId,
                         [property: JsonPropertyName("nickname")] string Nickname,
                         [property: JsonPropertyName("avatar")] string Avatar,
                         [property: JsonPropertyName("membership")] string Membership,
                         [property: JsonPropertyName("game_player_id")] string GamePlayerId,
                         [property: JsonPropertyName("game_player_name")] string GamePlayerName,
                         [property: JsonPropertyName("game_skill_level")] int? GameSkillLevel,
                         [property: JsonPropertyName("anticheat_required")] bool? AnticheatRequired);

    public record Score([property: JsonPropertyName("faction2")] int? Faction2,
                        [property: JsonPropertyName("faction1")] int? Faction1);

    public record Teams([property: JsonPropertyName("faction1")] Faction Faction1,
                        [property: JsonPropertyName("faction2")] Faction Faction2);

    public record Voting([property: JsonPropertyName("location")] Location Location,
                         [property: JsonPropertyName("map")] Map Map,
                         [property: JsonPropertyName("voted_entity_types")] IReadOnlyList<string> VotedEntityTypes);

#endregion

#region User

    public record Csgo([property: JsonPropertyName("region")] string Region,
                       [property: JsonPropertyName("game_player_id")] string GamePlayerId,
                       [property: JsonPropertyName("skill_level")] int? SkillLevel,
                       [property: JsonPropertyName("faceit_elo")] int? FaceitElo,
                       [property: JsonPropertyName("game_player_name")] string GamePlayerName,
                       [property: JsonPropertyName("skill_level_label")] string SkillLevelLabel,
                       [property: JsonPropertyName("regions")] Regions Regions,
                       [property: JsonPropertyName("game_profile_id")] string GameProfileId);

    public record Games([property: JsonPropertyName("csgo")] Csgo Csgo,
                        [property: JsonPropertyName("pubg")] Pubg Pubg);

    public record Infractions;

    public record Platforms([property: JsonPropertyName("steam")] string Steam);

    public record Pubg([property: JsonPropertyName("region")] string Region,
                       [property: JsonPropertyName("game_player_id")] string GamePlayerId,
                       [property: JsonPropertyName("skill_level")] int? SkillLevel,
                       [property: JsonPropertyName("faceit_elo")] int? FaceitElo,
                       [property: JsonPropertyName("game_player_name")] string GamePlayerName,
                       [property: JsonPropertyName("skill_level_label")] string SkillLevelLabel,
                       [property: JsonPropertyName("regions")] Regions Regions,
                       [property: JsonPropertyName("game_profile_id")] string GameProfileId);

    public record Regions;

    public record UserResponse([property: JsonPropertyName("player_id")] string PlayerId,
                               [property: JsonPropertyName("nickname")] string Nickname,
                               [property: JsonPropertyName("avatar")] string Avatar,
                               [property: JsonPropertyName("country")] string Country,
                               [property: JsonPropertyName("cover_image")] string CoverImage,
                               [property: JsonPropertyName("platforms")] Platforms Platforms,
                               [property: JsonPropertyName("games")] Games Games,
                               [property: JsonPropertyName("settings")] Settings Settings,
                               [property: JsonPropertyName("friends_ids")] IReadOnlyList<string> FriendsIds,
                               [property: JsonPropertyName("new_steam_id")] string NewSteamId,
                               [property: JsonPropertyName("steam_id_64")] string SteamId64,
                               [property: JsonPropertyName("steam_nickname")] string SteamNickname,
                               [property: JsonPropertyName("memberships")] IReadOnlyList<string> Memberships,
                               [property: JsonPropertyName("faceit_url")] string FaceitUrl,
                               [property: JsonPropertyName("membership_type")] string MembershipType,
                               [property: JsonPropertyName("cover_featured_image")] string CoverFeaturedImage,
                               [property: JsonPropertyName("infractions")] Infractions Infractions);

    public record Settings([property: JsonPropertyName("language")] string Language);

#endregion
}
