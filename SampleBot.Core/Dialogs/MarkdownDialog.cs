using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SampleBot.Core.Attributes;
using SampleBot.Core.Interfaces;
using SampleBot.Shared.Resources;

namespace SampleBot.Core.Dialogs
{
    [Serializable]
    [DialogRank(800)]
    public class MarkdownDialog : ISampleBotDialog
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }
        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            if (activity?.Conversation != null)
            {
                var str = new StringBuilder();
                str.Append($"**{StringResources.Bold_Text}**\n\r");
                str.Append($"*{StringResources.Italics}*\n\r");
                str.Append($"# {StringResources.Header}\n\r");
                str.Append($"~~{StringResources.Strikethrough_Text}~~\n\r");
                str.Append("---\n\r");
                str.Append($"* {StringResources.Unordered_List_Item}\n\r");
                str.Append($"1. {StringResources.Sorted_List_Item}\n\r");
                str.Append($"`{StringResources.Preformatted_Text}`\n\r");
                str.Append($"> {StringResources.Quote}\n\r");
                str.Append($"[{StringResources.Link}](http://www.bing.com)\n\r");
                str.Append($"![{StringResources.Picture}](http://aka.ms/Fo983c)\n\r");
                activity.Text = str.ToString();
            }
            context.Done(activity);
        }

        public MarkdownDialog()
        {
            CommandsName = new List<string>() { "/markdown" };
            Description = StringResources.Displays_Available_Text_Formatting;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
