using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using SampleBot.Core.Attributes;
using SampleBot.Core.Interfaces;
using SampleBot.Shared.Constants;
using SampleBot.Shared.Database.MongoRepositories;
using SampleBot.Shared.Models;
using SampleBot.Shared.Resources;

namespace SampleBot.Core.Dialogs
{
    [Serializable]
    [DialogRank(600)]
    class InterviewDialog: ISampleBotDialog
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }

        public static string CurrentLacale;

        public InterviewDialog()
        {
            Description = StringResources.Example_Of_Data_Collection_From_A_User;
            CommandsName = new List<string>() { "/interview" };
        }

        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            if (activity?.Conversation != null)
            {
                CurrentLacale = Thread.CurrentThread.CurrentCulture.Name;
                await context.Forward(MakeInterviewDialog(), ResumeAfterNewDialog, activity, CancellationToken.None);
            }
        }

        public static IForm<InterviewInfoModel> BuildForm()
        {
            Thread.CurrentThread.CurrentCulture = Constants.GetCulture(CurrentLacale);
            Thread.CurrentThread.CurrentUICulture = Constants.GetCulture(CurrentLacale);

            var form = new FormBuilder<InterviewInfoModel>()
                .Field(nameof(InterviewInfoModel.Name))
                .Field(nameof(InterviewInfoModel.Age))
                .Field(nameof(InterviewInfoModel.Messenger))
                .Field(nameof(InterviewInfoModel.Grade))
                .Field(nameof(InterviewInfoModel.Language))
                .Field(nameof(InterviewInfoModel.Comments))
                .Build(Assembly.GetAssembly(typeof(AutoResources)), "AutoResources");
            return form;
        }

        internal static IDialog<InterviewInfoModel> MakeInterviewDialog()
        {
            return Chain.From(() => FormDialog.FromForm(BuildForm));
        }

        private async Task ResumeAfterNewDialog(IDialogContext context, IAwaitable<object> result)
        {
            var interview = await result as InterviewInfoModel;
            if (interview != null)
            {
                var db = new MongoInterviewRepository();
                db.Add(interview);
                await context.PostAsync(StringResources.Your_Data_Has_Been_Successfully_Received);
                context.Done(result);
            }
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(Run);
        }
    }
}
