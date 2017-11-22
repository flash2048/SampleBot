using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Microsoft.Bot.Builder.FormFlow;
using SampleBot.Shared.Resources;

namespace SampleBot.Shared.Models
{
    public enum MessengerOptions
    {
        Skype,
        Telegram,
        Viber,
        WhatsApp,
        FacebookMessenger,
        Icq
    };

    public enum LangOptions
    {
        Russian,
        English,
        Spanish,
        Italian,
        German,
        Chinese,
        Other
    };

    [Serializable]
    public class InterviewInfoModel
    {
        public int Id;
        public string Name;

        [Numeric(18, 100)]
        public int Age = 18;

        public List<MessengerOptions> Messenger;
        [Optional]
        public LangOptions? Language;

        [Numeric(1, 10)]
        public int Grade = 6;

        public string Comments;
    }
}
