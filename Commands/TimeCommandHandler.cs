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

    public class TimeCommandHandler : CommandHandler
    {
        public override async Task<string> HandleAsync(Message message, TymDbContext db)
        {
            var reply = new StringBuilder();
            var userId = message.From.Id;
            var fromUserId = message.ReplyToMessage?.From.Id;
            if (fromUserId == null)
            {
                var user = await db.Users
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    reply.AppendLine("Please set your timezone using /timezone command");
                }
                else
                {
                    var zone = DateTimeZoneProviders.Tzdb[user.TimeZone];
                    var clock = SystemClock.Instance.InZone(zone);
                    var now = clock.GetCurrentZonedDateTime();
                    var pattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd MMM dd, yyyy h:mm tt z '('o<g>')'", null);
                    reply.AppendLine("Your current time is: ");
                    reply.Append(pattern.Format(now));
                }
            }
            else
            {
                var userFriend = await db.UserFriends
                    .Include(f => f.User)
                    .Where(f => f.UserId == fromUserId && f.FriendId == userId)
                    .FirstOrDefaultAsync();

                if (userFriend == null) 
                {
                    reply.AppendLine($"No time info found for {message.ReplyToMessage.From.Username}.");
                }
                else
                {
                    var zone = DateTimeZoneProviders.Tzdb[userFriend.User.TimeZone];
                    var clock = SystemClock.Instance.InZone(zone);
                    var now = clock.GetCurrentZonedDateTime();
                    var pattern = ZonedDateTimePattern.CreateWithInvariantCulture("dddd MMM dd, yyyy h:mm tt z '('o<g>')'", null);
                    reply.AppendLine($"{message.ReplyToMessage.From.Username} current time is: ");
                    reply.Append(pattern.Format(now));
                }
            }

            return reply.ToString();
        }
    }
}