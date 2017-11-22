using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SampleBot.Core.Attributes;
using SampleBot.Core.Interfaces;
using SampleBot.Shared.Constants;
using SampleBot.Shared.Database;
using SampleBot.Shared.Resources;

namespace SampleBot.Core.Dialogs
{
    [Serializable]
    [DialogRank(1000)]
    class LocalizationDialog: ISampleBotDialog
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }
        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var supportedLanguages = new Dictionary<string, string>();
            supportedLanguages.Add("ru-RU", "Русский");
            supportedLanguages.Add("en-US", "English");

            var activity = await result as Activity;
            if (activity?.Conversation != null)
            {

                if (!String.IsNullOrEmpty(activity.Text))
                {
                    if (activity.Text != "back")
                    {
                        Thread.CurrentThread.CurrentCulture = Constants.GetCulture(activity.Text);
                        Thread.CurrentThread.CurrentUICulture = Constants.GetCulture(activity.Text);
                        DataModel.SetUserLocale(activity, Thread.CurrentThread.CurrentCulture.Name);
                        activity.Text = StringResources.Current_Language;
                    }
                    else
                    {
                        activity.Text = String.Empty;
                    }
                   context.Done(activity);
                }
                else
                {
                    var reply = context.MakeMessage();

                    var heroCard = new ThumbnailCard
                    {
                        Title = StringResources.Change_The_Current_Language
                    };

                    var buttons = new List<CardAction>();

                    foreach (var supportedLanguage in supportedLanguages)
                    {
                       // if (supportedLanguage.Key != Thread.CurrentThread.CurrentCulture.Name)
                       // {
                            buttons.Add(new CardAction(ActionTypes.ImBack, supportedLanguage.Value, value: supportedLanguage.Key));
                       // }
                    }
                   // buttons.Add(new CardAction(ActionTypes.ImBack, StringResources.Back, value: "back"));
                    heroCard.Buttons = buttons;

                    reply.Attachments = new List<Attachment>() { heroCard.ToAttachment() };
                    await context.PostAsync(reply);
                    context.Wait(Run);
                }

               
            }
        }

        public LocalizationDialog()
        {
            CommandsName = new List<string>() { "/localization", "/lang" };
            Description = StringResources.Change_The_Current_Language;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
