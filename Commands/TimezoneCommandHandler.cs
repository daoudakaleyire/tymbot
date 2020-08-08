namespace tymbot.Commands
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using NodaTime.Extensions;
    using NodaTime.Text;
    using Telegram.Bot.Types;
    using tymbot.Data;

    public class TimezoneCommandHandler : CommandHandler
    {
        public override async Task<string> HandleAsync(Message message, TymDbContext db)
        {
            var textMessage = message.Text.Trim();
            var reply = new StringBuilder();
            var userId = message.From.Id;
            var fromUserId = message.ReplyToMessage?.From.Id;
            var inputZone = textMessage
                .Replace($"/{BotCommands.Timezone}@{BotService.Bot.Username}", "")
                .Replace($"/{BotCommands.Timezone}", "")
                .Trim()
                .ToLower();
            
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
                var user = await db.Users
                    .Where(f => f.UserId == userId)
                    .FirstOrDefaultAsync();
            
                if (user == null)
                {
                    user = new Models.User()
                    {
                        UserId = userId,
                        TimeZone = timezone,
                        Name = message.From.FirstName,
                    };
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                }
                else
                {
                    user.TimeZone = timezone;
                    user.Name = message.From.FirstName;
                    await db.SaveChangesAsync();
                }
                
                var zone = DateTimeZoneProviders.Tzdb[timezone];
                var clock = SystemClock.Instance.InZone(zone);
                var now = clock.GetCurrentZonedDateTime();
                var pattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd MMM dd, yyyy h:mm tt z '('o<g>')'", null);
                reply.AppendLine("Your current time is: ");
                reply.Append(pattern.Format(now));
            }

            return reply.ToString();
        }
    }
}