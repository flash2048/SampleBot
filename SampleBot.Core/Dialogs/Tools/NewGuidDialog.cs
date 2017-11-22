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
    [DialogRank(1000)]
    [NotShowInHelp]
    public class NewGuidDialog : ISampleBotDialog, ITool
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }
        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            if (activity?.Conversation != null)
            {
                activity.Text = Guid.NewGuid().ToString();
            }
            context.Done(activity);
        }

        public NewGuidDialog()
        {
            CommandsName = new List<string>() { "/newguid", "/guid" };
            Description = StringResources.Get_Unique_Identifier;
        }
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
