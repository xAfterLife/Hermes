using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyAPI.Web;

namespace Hermes.Services;

public sealed class SpotifyService
{
    private readonly SpotifyClient _client;
    private readonly LoggingService _logger;

    public SpotifyService(IServiceProvider services)
    {
        _logger = services.GetRequiredService<LoggingService>();
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
                if ( response.Albums.Items != null )
                    embeds.AddRange(response.Albums.Items.Select(item => new EmbedBuilder { Title = item.Name, Description = item.Artists.First().Name, Color = Color.Red, ThumbnailUrl = item.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.Artist:
                if ( response.Artists.Items != null )
                    embeds.AddRange(response.Artists.Items.Select(item => new EmbedBuilder { Title = item.Name, Description = item.Name, Color = Color.Red, ThumbnailUrl = item.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.Playlist:
                if ( response.Playlists.Items != null )
                    embeds.AddRange(response.Playlists.Items.Select(item => new EmbedBuilder { Title = item.Name, Description = item.Owner.DisplayName, Color = Color.Red, ThumbnailUrl = item.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.Track:
                if ( response.Tracks.Items != null )
                    embeds.AddRange(response.Tracks.Items.Select(item => new EmbedBuilder { Title = item.Name, Description = item.Artists.First().Name, Color = Color.Red, ThumbnailUrl = item.Album.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.Show:
                if ( response.Shows.Items != null )
                    embeds.AddRange(response.Shows.Items.Select(item => new EmbedBuilder { Title = item.Name, Description = item.Description, Color = Color.Red, ThumbnailUrl = item.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.Episode:
                if ( response.Episodes.Items != null )
                    embeds.AddRange(response.Episodes.Items.Select(item => new EmbedBuilder { Title = item.Name, Description = item.Description, Color = Color.Red, ThumbnailUrl = item.Images.FirstOrDefault(x => x.Height == 640)?.Url }));
                break;
            case SearchRequest.Types.All:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return embeds;
    }
}
