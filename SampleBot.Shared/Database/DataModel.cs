using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Connector;
using SampleBot.Shared.Database.MongoRepositories;
using SampleBot.Shared.Models;

namespace SampleBot.Shared.Database
{
    public class DataModel
    {

        public static List<User> Users
        {
            get
            {
                var db = new MongoUserRepository();
                return db.GetAll().ToList();
            }
        }

        public static User GetOrCreateUser(Activity activity)
        {
            if (activity != null)
            {
                try
                {
                    var db = new MongoUserRepository();

                    var user = db.GetAll().FirstOrDefault(x => x.ChannelId == activity.ChannelId && x.UserId == activity.From.Id &&
                                                               x.UserName == activity.From.Name);
                    if (user == null)
                    {
                        user = new User(activity);
                        db.Add(user);
                    }
                    return user;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            return null;
        }

        public static void SetUserLocale(Activity activity, string locale)
        {
            if (activity != null)
            {
                try
                {
                    var db = new MongoUserRepository();
                    var user = db.GetAll().FirstOrDefault(x => x.ChannelId == activity.ChannelId && x.UserId == activity.From.Id &&
                                                               x.UserName == activity.From.Name);
                    if (user != null)
                    {
                        user.Locale = locale;
                        db.Update(user);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }
    }
}
