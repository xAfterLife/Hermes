﻿using Discord;
using Discord.Interactions;

namespace Hermes.Attributes;

public class RequireOwnerAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        switch ( context.Client.TokenType )
        {
            case TokenType.Bot:
                var application = await context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
                return context.User.Id != application.Owner.Id ? PreconditionResult.FromError(ErrorMessage ?? "Command can only be run by the owner of the bot.") : PreconditionResult.FromSuccess();
            case TokenType.Bearer:
            case TokenType.Webhook:
            default:
                return PreconditionResult.FromError($"{nameof(RequireOwnerAttribute)} is not supported by this {nameof(TokenType)}.");
        }
    }
}
