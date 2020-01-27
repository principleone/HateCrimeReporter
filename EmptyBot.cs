// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EmptyBot .NET Template version v4.7.0

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace HateCrimeReporterCSharp
{
    public class EmptyBot : ActivityHandler
    {
        public const string WhatMessage = "What type of hate crime are you reporting?";

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hi! You are talking to the Hate Crime Report Bot. Give me the details of what happened, and I will let the club know so they can take action!"), cancellationToken);
                    await turnContext.SendActivityAsync(MessageFactory.Text(WhatMessage), cancellationToken: cancellationToken);                    
                }
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var text = turnContext.Activity.Text.ToLowerInvariant();
            await turnContext.SendActivityAsync($"You want to report {text}.", cancellationToken: cancellationToken);

        }
    }
}
