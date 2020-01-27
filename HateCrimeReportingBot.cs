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
    public class HateCrimeReportingBot : ActivityHandler
    {
        public const string WhatMessage = "What type of hate crime are you reporting?";
        public const string Greeting = "Hi! You are talking to the Hate Crime Report Bot. Give me the details of what happened, and I will let the club know so they can take action!";
      private readonly UserState userState;

      public HateCrimeReportingBot(UserState userState)
        {
         this.userState = userState;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {                    
                    await turnContext.SendActivityAsync(MessageFactory.Text(Greeting), cancellationToken);
                    await turnContext.SendActivityAsync(MessageFactory.Text(WhatMessage), cancellationToken: cancellationToken);                    
                }
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var text = turnContext.Activity.Text.ToLowerInvariant();
            var reportingUserStateAccessor = this.userState.CreateProperty<ReportingStateProperties>("reportState");
            var reportingState = await reportingUserStateAccessor.GetAsync(turnContext, () => new ReportingStateProperties());            

            if (text.Equals("restart"))
            {
                await reportingUserStateAccessor.SetAsync(turnContext, new ReportingStateProperties(), cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text(Greeting), cancellationToken);
            }
            else if (string.IsNullOrEmpty(reportingState.CrimeName))
            {               
                reportingState.CrimeName = text; 
                await turnContext.SendActivityAsync($"When did the incident happen? You can give the exact time or the number of minutes into the game if you'd prefer.", cancellationToken: cancellationToken);
            } else if (string.IsNullOrEmpty(reportingState.CrimeTime)){
                reportingState.CrimeTime = text;
                await turnContext.SendActivityAsync($"What behaviour did you witness? If you heard exact wording, put it into speech marks");
            } 
            else if (string.IsNullOrEmpty(reportingState.CrimeBehaviour))
            {
                reportingState.CrimeBehaviour = text;
                await turnContext.SendActivityAsync($"Thank you, I have stored all that information. Let’s move onto where this happened.");
                
                
                

            }

            await userState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }
    }
}
