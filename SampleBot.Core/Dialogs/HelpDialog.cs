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

namespace SampleBot.Core.Dialogs
{
    [NotShowInHelp]
    [Serializable]
    public class HelpDialog : ISampleBotDialog
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }

        protected string CommandText { get; set; }

        public HelpDialog()
        {
            Description = StringResources.Giving_help;
            CommandsName = new List<string>() { "/help" };
        }

        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;


            var resultStr = new StringBuilder();
            var baseInterfaceType = typeof(ISampleBotDialog);
            var botCommands = Assembly.GetAssembly(baseInterfaceType)
                .GetTypes()
                .Where(types => types.IsClass && !types.IsAbstract && types.GetInterface(typeof(ISampleBotDialog).Name) != null);
            foreach (var botCommand in botCommands.OrderByDescending(x=> ((DialogRankAttribute)x.GetCustomAttribute(typeof(DialogRankAttribute)))?.Rank ?? 0) )
            {
                if (!botCommand.GetCustomAttributes(typeof(NotShowInHelpAttribute)).Any())
                {
                    var command = (ISampleBotDialog)Activator.CreateInstance(botCommand);
                    var commandStr = command.CommandsName.First();
                    if (activity?.ChannelId == "skype")
                    {
                        commandStr = commandStr.TrimStart('/');
                    }
                    resultStr.Append($"{commandStr} - {command.Description}\n\r");
                }
            }
            CommandText = resultStr.ToString();

            if (activity?.Conversation != null)
            {
                activity.Text = CommandText;
            }
            context.Done(activity);
        }


        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
