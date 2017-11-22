using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SampleBot.Core.Attributes;
using SampleBot.Core.Interfaces;
using SampleBot.Shared.Resources;

namespace SampleBot.Core.Dialogs.Tools
{
    [Serializable]
    [DialogRank(900)]
    [NotShowInHelp]
    public class ToUpperDialog : ISampleBotDialog, ITool
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }
        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            if (activity?.Conversation != null)
            {
                if (!String.IsNullOrEmpty(activity.Text))
                {
                    activity.Text = activity.Text.ToUpper();
                    context.Done(activity);
                }
                else
                {
                    await context.PostAsync(StringResources.Enter_Text);
                    context.Wait(Run);
                }
            }
        }

        public ToUpperDialog()
        {
            CommandsName = new List<string>() { "/toupper" };
            Description = StringResources.Bring_Text_To_Uppercase;
        }
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
