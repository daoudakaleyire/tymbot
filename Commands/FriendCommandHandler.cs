namespace tymbot.Commands
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Telegram.Bot.Types;
    using tymbot.Data;
    using tymbot.Models;

    public class FriendCommandHandler : CommandHandler
    {
        public override async Task<string> HandleAsync(Message message, TymDbContext db)
        {
            var reply = new StringBuilder();
            var userId = message.From.Id;
            var fromUserId = message.ReplyToMessage?.From.Id;
            
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
                    reply.AppendLine($"{message.ReplyToMessage.From.FirstName} can now see your time.");
                }
            }

            return reply.ToString();
        }
    }
}