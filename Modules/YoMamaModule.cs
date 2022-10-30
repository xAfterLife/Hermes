using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace Hermes.Modules;

public class YoMamaModule : InteractionModuleBase<SocketInteractionContext>
{
    private static readonly Random Rnd = new((int)(Environment.TickCount * DateTime.Now.ToFileTimeUtc()));

    private static readonly List<string> Jokes = new()
    {
        "mama's so fat, when she fell I didn't laugh, but the sidewalk cracked up.",
        "mama's so fat, when she skips a meal, the stock market drops.",
        "mama's so fat, it took me two buses and a train to get to her good side.",
        "mama's so fat, when she goes camping, the bears hide their food.",
        "mama's so fat, if she buys a fur coat, a whole species will become extinct.",
        "mama's so fat, she stepped on a scale and it said: 'To be continued.'",
        "mama's so fat, I swerved to miss her in my car and ran out of gas.",
        "mama's so fat, when she wears high heels, she strikes oil.",
        "mama's so fat, she was overthrown by a small militia group, and now she's known as the Republic of Yo Mama.",
        "mama's so fat, when she sits around the house, she SITS AROUND the house.",
        "mama's so fat, her car has stretch marks.",
        "mama's so fat, she can't even jump to a conclusion.",
        "mama's so fat, her blood type is Ragu.",
        "mama's so fat, if she was a Star Wars character, her name would be Admiral Snackbar.",
        "mama's so fat, she brought a spoon to the Super Bowl.",
        "mama's so stupid, she stared at a cup of orange juice for 12 hours because it said 'concentrate.'",
        "mama's so stupid when they said it was chilly outside, she grabbed a bowl.",
        "mama's so stupid, she put lipstick on her forehead to make up her mind.",
        "mama's so stupid, when they said, 'Order in the court' she asked for fries and a shake.",
        "mama's so stupid, she thought a quarterback was a refund.",
        "mama's so stupid, she thought a quarterback was a refund.",
        "mama's so stupid, she got hit by a parked car.",
        "mama's so stupid, when I told her that she lost her mind, she went looking for it.",
        "mama's so stupid when thieves broke into her house and stole the TV, she chased after them shouting 'Wait, you forgot the remote!'",
        "mama's so stupid, she went to the dentist to get a Bluetooth.",
        "mama's so stupid, she took a ruler to bed to see how long she slept.",
        "mama's so stupid, she got locked in the grocery store and starved to death.",
        "mama's so stupid, when I said, 'Drinks on the house' she got a ladder.",
        "mama's so stupid, it takes her two hours to watch 60 Minutes.",
        "mama's so stupid, she put airbags on her computer in case it crashed.",
        "mama's so ugly, she threw a boomerang and it refused to come back.",
        "mama's so old, her social security number is one.",
        "mama's so ugly, she made a blind kid cry.",
        "mama's so ugly, her portraits hang themselves.",
        "mama's so old, she walked out of a museum and the alarm went off.",
        "mama's teeth are so yellow when she smiles at traffic, it slows down.",
        "mama's armpits are so hairy, it looks like she's got Buckwheat in a headlock.",
        "mama's so ugly, when she was little, she had to trick-or-treat by phone.",
        "mama's so ugly, her birth certificate is an apology letter.",
        "mama's so ugly, she looked out the window and was arrested for mooning.",
        "mama's so poor, the ducks throw bread at her.",
        "mama's so poor, she chases the garbage truck with a grocery list.",
        "mama's cooking so nasty, she flys got together and fixed the hole in the window screen.",
        "mama's so depressing, blues singers come to visit her when they've got writer's block.",
        "mama's so short, You can see her feet on her driver's license.",
        "mama's so poor, she can't even afford to pay attention.",
        "mama so big, her belt size is 'equator.'",
        "mama's so classless, she's a Marxist utopia.",
        "mama so short, she went to see Santa and he told her to get back to work.",
        "mama so scary, the government moved Halloween to her birthday.",
        "mama's so nasty, they used to call them jumpolines til Yo mama bounced on one.",
        "mama's teeth so yellow, I can't believe it's not butter.",
        "mama's so poor, Nigerian princes wire her money.",
        "mama so dumb, she went to the eye doctor to get an iPhone.",
        "mama's so lazy, she stuck her nose out the window and let the wind blow it."
    };

    [UserCommand("yomama")]
    [SlashCommand("yomama", "Hits your Target with a YoMama Line")]
    public async Task YoMama([Summary(description: "Das Subreddit woraus geladen werden soll")] SocketUser user)
    {
        await RespondAsync($"{user.Mention} {Jokes[Rnd.Next(0, Jokes.Count - 1)]}");
    }
}
