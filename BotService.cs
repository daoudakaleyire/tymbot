namespace tymbot
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NodaTime;
    using NodaTime.Extensions;
    using NodaTime.Text;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using tymbot.Data;
    using tymbot.Models;

    public class BotService
    {
        private readonly ITelegramBotClient botClient;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public BotService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            var token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_ACCESS_TOKEN");
            this.botClient = new TelegramBotClient(token);
        }

        public void Initialize()
        {
            IList<BotCommand> commands = new List<BotCommand>()
            {
                new BotCommand()
                {
                    Command = "time",
                    Description = "User local time",
                },
                new BotCommand()
                {
                    Command = "friend",
                    Description = "Add friend to view your local time.",
                },
                new BotCommand()
                {
                    Command = "timezone",
                    Description = "Set your timezone."
                }
            };

            botClient.SetMyCommandsAsync(commands)
                .GetAwaiter()
                .GetResult();
            botClient.OnMessage += HandlerMessageAsync;
            botClient.StartReceiving();
        }

        private async void HandlerMessageAsync(object sender, MessageEventArgs e)
        {
            string message = e.Message.Text;
            if (message == null) {
                return;
            }

            using var scope = this.serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<TymDbContext>();
            var reply = new StringBuilder();
            var userId = e.Message.From.Id;
            var fromUserId = e.Message.ReplyToMessage?.From.Id;
            
            if ("/time".Equals(message))
            {
                if (fromUserId == null)
                {
                    var userZone = await db.UserTimeZones
                        .Where(u => u.UserId == userId)
                        .FirstOrDefaultAsync();

                    if (userZone == null)
                    {
                        reply.AppendLine("Please set your timezone using /timezone command");
                    }
                    else
                    {
                        var zone = DateTimeZoneProviders.Tzdb[userZone.TimeZone];
                        var clock = SystemClock.Instance.InZone(zone);
                        var now = clock.GetCurrentZonedDateTime();
                        var pattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd MMM dd, yyyy h:mm tt z '('o<g>')'", null);
                        reply.Append(pattern.Format(now));
                    }
                }
                else
                {
                    var userFriend = await db.UserFriends
                        .Include(f => f.UserTimeZone)
                        .Where(f => f.UserId == fromUserId && f.FriendId == userId)
                        .FirstOrDefaultAsync();

                    if (userFriend == null) 
                    {
                        reply.AppendLine("No time info found.");
                    }
                    else
                    {
                        var zone = DateTimeZoneProviders.Tzdb[userFriend.UserTimeZone.TimeZone];
                        var clock = SystemClock.Instance.InZone(zone);
                        var now = clock.GetCurrentZonedDateTime();
                        var pattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd MMM dd, yyyy h:mm tt z '('o<g>')'", null);
                        reply.Append(pattern.Format(now));
                    }
                }
            }
            else if ("/friend".Equals(message))
            {
                if (fromUserId == null)
                {
                    reply.AppendLine("Reply to a message using /friend command.");
                }
                else
                {
                    var userFriend = await db.UserFriends
                        .Where(f => f.UserId == userId && f.FriendId == fromUserId)
                        .FirstOrDefaultAsync();
                
                    if (userFriend == null)
                    {
                        userFriend = new UserFriend()
                        {
                            FriendId = fromUserId.Value,
                            UserId = userId,
                        };
                        db.UserFriends.Add(userFriend);
                        await db.SaveChangesAsync();
                        reply.AppendLine($"{e.Message.ReplyToMessage.From.FirstName} can now see your time.");
                    }
                }
            }
            else if (message.StartsWith("/timezone")) 
            {
                var inputZone = message.Substring(9).Trim().ToLower();
                var timezone = DateTimeZoneProviders.Tzdb.Ids.SingleOrDefault(id => id.ToLower() == inputZone);
                if (timezone == null)
                {
                    var timezones = DateTimeZoneProviders.Tzdb.Ids
                        .Where(id => id.ToLower().Contains(inputZone))
                        .Take(10);

                    if (timezones.Count() == 0) 
                    {
                        timezones = DateTimeZoneProviders.Tzdb.Ids.Take(10);
                    }

                    reply.AppendLine("Sample usage: /timezone Asia/Tokyo");
                    reply.AppendLine("\nAre you looking for these timezones?\n");
                    foreach (var z in timezones)
                    {
                        reply.AppendLine(z);
                    }         
                }
                else 
                {
                    var userTimeZone = await db.UserTimeZones
                        .Where(f => f.UserId == userId)
                        .FirstOrDefaultAsync();
                
                    if (userTimeZone == null)
                    {
                        userTimeZone = new UserTimeZone()
                        {
                            UserId = userId,
                            TimeZone = timezone,
                        };
                        db.UserTimeZones.Add(userTimeZone);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        userTimeZone.TimeZone = timezone;
                        await db.SaveChangesAsync();
                    }
                    
                    var zone = DateTimeZoneProviders.Tzdb[timezone];
                    var clock = SystemClock.Instance.InZone(zone);
                    var now = clock.GetCurrentZonedDateTime();
                    var pattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd MMM dd, yyyy h:mm tt z '('o<g>')'", null);
                    reply.Append(pattern.Format(now));
                }
            }

            if (reply.Length > 0) {
                await botClient.SendTextMessageAsync(e.Message.Chat, reply.ToString()); 
            }
        }
    }
}