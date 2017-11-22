using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SampleBot.Core.Attributes;
using SampleBot.Core.Interfaces;
using SampleBot.Shared.Resources;

namespace SampleBot.Core.Dialogs.Tools
{
    [Serializable]
    [DialogRank(700)]
    public class ToolsDialog : ISampleBotDialog
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }
        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            var resultStr = new StringBuilder();
            var baseInterfaceType = typeof(ISampleBotDialog);
            var botCommands = Assembly.GetAssembly(baseInterfaceType)
                .GetTypes()
                .Where(types => types.IsClass && !types.IsAbstract && types.GetInterface(typeof(ITool).Name) != null && types.GetInterface(typeof(ISampleBotDialog).Name) != null);
            foreach (var botCommand in botCommands.OrderByDescending(x => ((DialogRankAttribute)x.GetCustomAttribute(typeof(DialogRankAttribute)))?.Rank ?? 0))
            {
                var command = (ISampleBotDialog)Activator.CreateInstance(botCommand);
                var commandStr = command.CommandsName.First();
                if (activity?.ChannelId == "skype")
                {
                    commandStr = commandStr.TrimStart('/');
                }
                resultStr.Append($"{commandStr} - {command.Description}\n\r");
            }
            var commandText = resultStr.ToString();

            if (activity?.Conversation != null)
            {
                activity.Text = commandText;
            }
            context.Done(activity);
        }

        public ToolsDialog()
        {
            CommandsName = new List<string>() { "/tools" };
            Description = StringResources.Get_Unique_Identifier;
        }
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
