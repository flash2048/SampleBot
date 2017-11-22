using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace SampleBot.Core.Interfaces
{
    interface ISampleBotDialog : IDialog<object>
    {
        string Description { get; set; }
        List<string> CommandsName { get; set; }
        bool IsAdmin { get; set; }
        Task Run(IDialogContext context, IAwaitable<IMessageActivity> result);

    }
}
