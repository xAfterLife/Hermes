using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyAPI.Web;

namespace Hermes.Services;

public sealed class SpotifyService
{
    private readonly SpotifyClient _client;
    private readonly JsonService _jsonService;
    private readonly LoggingService _logger;

    public SpotifyService(IServiceProvider services)
    {
        _logger = services.GetRequiredService<LoggingService>();
        _jsonService = services.GetRequiredService<JsonService>();
        var config = services.GetRequiredService<IConfiguration>();

        try
        {
            var spotifyId = config.GetValue<string>("SpotifyId");
            var spotifySecret = config.GetValue<string>("SpotifySecret");

            if ( spotifyId != null && spotifySecret != null )
            {
                var spotifyConfig = SpotifyClientConfig
                                    .CreateDefault()
                                    .WithAuthenticator(new ClientCredentialsAuthenticator(spotifyId, spotifySecret));

                _client = new SpotifyClient(spotifyConfig);
            }
            else
            {
                _logger.WarningAsync("Check spotifyId & Secret");
                _client = null!;
            }

            _logger.InfoAsync("SpotifyClient created");
        }
        catch ( Exception e )
        {
            _logger.ErrorAsync(e);
            _client = null!;
        }
    }

    public async Task<FullTrack> GetTrack(string trackId)
    {
        return await _client.Tracks.Get(trackId);
    }

    public async Task<IEnumerable<EmbedBuilder>> Search(SearchRequest.Types type, string query)
    {
        List<EmbedBuilder> embeds = new();

        var response = await _client.Search.Item(new SearchRequest(type, query));

        switch ( type )
        {
            case SearchRequest.Types.Album:
                break;
            case SearchRequest.Types.Artist:
                break;
            case SearchRequest.Types.Playlist:
                break;
            case SearchRequest.Types.Track:
                if ( response.Tracks.Items != null )
                    embeds.AddRange(response.Tracks.Items.Select(track => new EmbedBuilder { Title = track.Name, Description = track.Artists.First().Name, Color = Color.Red, ThumbnailUrl = track.Album.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.Show:
                break;
            case SearchRequest.Types.Episode:
                break;
            case SearchRequest.Types.All:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return embeds;
    }
}
