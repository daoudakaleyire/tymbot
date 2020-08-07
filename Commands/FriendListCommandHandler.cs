namespace tymbot.Commands
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Telegram.Bot.Types;
    using tymbot.Data;

    public class FriendListCommandHandler : CommandHandler
    {
        public override async Task<string> HandleAsync(Message message, TymDbContext db)
        {
            var reply = new StringBuilder();
            var userId = message.From.Id;
            var friends = await db.UserFriends
                    .Include(uf => uf.Friend)
                    .Where(uf => uf.UserId == userId)
                    .ToListAsync();

            if (friends.Any())
            {
                reply.AppendLine("The following friends can see your current time: \n");
                foreach (var uf in friends)
                {
                    string name = string.IsNullOrWhiteSpace(uf.Friend.Name) ? "name-not-available" : uf.Friend.Name;
                    reply.AppendLine($"[{name}](tg://user?id={uf.FriendId})");
                }
            }
            else
            {
                reply.AppendLine("It seems that you have no friends.");
                reply.AppendLine("To add friends reply /friend to their message, they will then be able to see your current time.");
            }

            return reply.ToString();
        }
    }
}