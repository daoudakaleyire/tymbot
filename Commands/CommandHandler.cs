namespace tymbot.Commands
{
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using tymbot.Data;

    public abstract class CommandHandler
    {
        public abstract Task<string> HandleAsync(Message message, TymDbContext db);
    }
}