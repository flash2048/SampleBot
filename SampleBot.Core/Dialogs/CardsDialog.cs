using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SampleBot.Core.Attributes;
using SampleBot.Core.Interfaces;
using SampleBot.Shared.Resources;

namespace SampleBot.Core.Dialogs
{
    [Serializable]
    [DialogRank(900)]
    public class Cards : ISampleBotDialog
    {
        public string Description { get; set; }
        public List<string> CommandsName { get; set; }
        public bool IsAdmin { get; set; }

        private const string HeroCard = "hero";
        private const string ThumbnailCard = "thumbnail";
        private const string ReceiptCard = "receipt";
        private const string SigninCard = "sign-in";
        private const string AnimationCard = "animation";
        private const string VideoCard = "video";
        private const string AudioCard = "audio";
        public virtual async Task Run(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            if (activity?.Conversation != null)
            {
                activity.Attachments = new List<Attachment>();
                var command = activity.Text.Trim().ToLower();

                Attachment attachment;
                switch (command)
                {
                    case HeroCard:
                        attachment = GetHeroCard();
                        break;
                    case ThumbnailCard:
                        attachment = GetThumbnailCard();
                        break;
                    case ReceiptCard:
                        attachment = GetReceiptCard();
                        break;
                    case SigninCard:
                        attachment = GetSigninCard();
                        break;
                    case AnimationCard:
                        attachment = GetAnimationCard();
                        break;
                    case VideoCard:
                        attachment = GetVideoCard();
                        break;
                    case AudioCard:
                        attachment = GetAudioCard();
                        break;

                    default:
                        attachment = GetDefaultCard();
                        break;
                }

                activity.Attachments.Add(attachment);
            }
            context.Done(activity);
        }

        public Cards()
        {
            CommandsName = new List<string>() { "/cards" };
            Description = StringResources.Display_Card_Types;
        }

        private static Attachment GetDefaultCard()
        {
            var heroCard = new HeroCard
            {
                Title = StringResources.List_Of_Available_Cards,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, "HeroCard", value: "Cards "+HeroCard),
                    new CardAction(ActionTypes.ImBack, "Thumbnail", value: "Cards "+ThumbnailCard),
                    new CardAction(ActionTypes.ImBack, "Receipt", value: "Cards "+ReceiptCard),
                    new CardAction(ActionTypes.ImBack, "Signin", value: "Cards "+SigninCard),
                    new CardAction(ActionTypes.ImBack, "Animation", value: "Cards "+AnimationCard),
                    new CardAction(ActionTypes.ImBack, "Video", value: "Cards "+VideoCard),
                    new CardAction(ActionTypes.ImBack, "Audio", value: "Cards "+AudioCard),
                }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = StringResources.Hero_Card_Title,
                Subtitle = StringResources.Hero_Card_Title_Sub,
                Text = StringResources.Hero_Card_Title_Text,
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, StringResources.Open_Link, value: "https://docs.microsoft.com/bot-framework") }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetThumbnailCard()
        {
            var heroCard = new ThumbnailCard
            {
                Title = StringResources.Thumbnail_Card_Title,
                Subtitle = StringResources.Thumbnail_Card_Sub,
                Text = StringResources.Thumbnail_Card_Text,
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, StringResources.Open_Link, value: "https://docs.microsoft.com/bot-framework") }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetReceiptCard()
        {
            var receiptCard = new ReceiptCard
            {
                Title = StringResources.Receipt_Card_Title,
                Facts = new List<Fact> { new Fact(StringResources.Order_Number, "1234"), new Fact(StringResources.Payment_Method, "VISA 5555-****") },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem(StringResources.Data_Exchange, price: "$ 38.45", quantity: "368", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                    new ReceiptItem(StringResources.Application_Service, price: "$ 45.00", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                Tax = "$ 7.50",
                Total = "$ 90.95",
                Buttons = new List<CardAction>
                {
                    new CardAction(
                        ActionTypes.OpenUrl,
                        StringResources.More_Information,
                        "https://account.windowsazure.com/content/6.10.1.38-.8225.160809-1618/aux-pre/images/offer-icon-freetrial.png",
                        "https://azure.microsoft.com/en-us/pricing/")
                }
            };

            return receiptCard.ToAttachment();
        }

        private static Attachment GetSigninCard()
        {
            var signinCard = new SigninCard
            {
                Text = StringResources.Sign_In_Card_Title,
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, StringResources.Log_In, value: "https://login.microsoftonline.com/") }
            };

            return signinCard.ToAttachment();
        }

        private static Attachment GetAnimationCard()
        {
            var animationCard = new AnimationCard
            {
                Title = StringResources.Animation_Card_Title,
                Subtitle = StringResources.Animation_Card_Sub,
                Image = new ThumbnailUrl
                {
                    Url = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://i.giphy.com/Ki55RUbOV5njy.gif"
                    }
                }
            };

            return animationCard.ToAttachment();
        }

        private static Attachment GetVideoCard()
        {
            var videoCard = new VideoCard
            {
                Title = StringResources.Video_Card_Title,
                Subtitle = StringResources.Video_Card_Sub,
                Text = StringResources.Video_Card_Text,
                Image = new ThumbnailUrl
                {
                    Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/Big_buck_bunny_poster_big.jpg/220px-Big_buck_bunny_poster_big.jpg"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4"
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = StringResources.Open_Link,
                        Type = ActionTypes.OpenUrl,
                        Value = "https://peach.blender.org/"
                    }
                }
            };

            return videoCard.ToAttachment();
        }

        private static Attachment GetAudioCard()
        {
            var audioCard = new AudioCard
            {
                Title = StringResources.Audio_Card_Title,
                Subtitle = StringResources.Audio_Card_Sub,
                Text = StringResources.Audio_Card_Text,
                Image = new ThumbnailUrl
                {
                    Url = "https://upload.wikimedia.org/wikipedia/en/3/3c/SW_-_Empire_Strikes_Back.jpg"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://www.wavlist.com/movies/004/father.wav"
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = StringResources.Open_Link,
                        Type = ActionTypes.OpenUrl,
                        Value = "https://en.wikipedia.org/wiki/The_Empire_Strikes_Back"
                    }
                }
            };

            return audioCard.ToAttachment();
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.Run);
        }
    }
}
