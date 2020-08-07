namespace tymbot.Commands
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Telegram.Bot.Types;
    using tymbot.Data;

    public class StartCommandHandler : CommandHandler
    {
        public override async Task<string> HandleAsync(Message message, TymDbContext db)
        {
            var reply = new StringBuilder();
            var userId = message.From.Id;
            var user = await db.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync();
            
            if (user == null)
            {
                user = new Models.User()
                {
                    UserId = userId,
                    ChatId = message.Chat.Id,
                    Name = message.From.FirstName,
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                reply.AppendLine($"Hi {message.From.FirstName}!");
                reply.AppendLine($"Thank you for starting me. You can start by setting your timezone using /timezone command.");
            }
            else 
            {
                user.ChatId = message.Chat.Id;
                user.Name = message.From.FirstName;
                await db.SaveChangesAsync();
                reply.AppendLine($"Hi {message.From.FirstName}!");
            }

            return reply.ToString();
        }
    }
}