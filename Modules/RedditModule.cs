using Discord;
using Discord.Interactions;
using Newtonsoft.Json.Linq;

namespace Hermes.Modules;

[Group("reddit", "reddit command group")]
public class RedditModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("reddit", "Posts a Random Post from this Subreddit")]
    public async Task Reddit([Summary(description: "Das Subreddit woraus geladen werden soll")] string subreddit = "Hentai_Gif", [Summary(description: "The amount of posts found")] int postCount = 1, [Summary(description: "Should everyone see this or only you?")] bool onlyMe = false)
    {
        await DeferAsync(onlyMe);

        var embeds = new Embed[postCount];
        var client = new HttpClient();

        for ( var i = 0; i < postCount; i++ )
        {
            var result = await client.GetStringAsync($"https://reddit.com/r/{subreddit}/random.json?limit=1");

            if ( !result.StartsWith("[") )
            {
                await FollowupAsync("This subreddit doesn't exist!");
                return;
            }

            var arr = JArray.Parse(result);
            var post = JObject.Parse(arr[0]["data"]?["children"]?[0]?["data"]?.ToString()!);

            var builder = new EmbedBuilder()
                          .WithImageUrl(post["url"]?.ToString())
                          .WithColor(new Color(33, 176, 252))
                          .WithTitle(post["title"]?.ToString())
                          .WithUrl("https://reddit.com" + post["permalink"])
                          .WithFooter($"🗨️ {post["num_comments"]} ⬆️ {post["ups"]}");
            embeds[i] = builder.Build();
        }

        await FollowupAsync(embeds: embeds, ephemeral: onlyMe);
    }
}
