namespace tymbot.Commands
{
    public class CommandHandlerFactory
    {
        public static CommandHandler GetHandler(string command)
        {
            return command switch
            {
                BotCommands.Time => new TimeCommandHandler(),
                BotCommands.Timezone => new TimezoneCommandHandler(),
                BotCommands.Friend => new FriendCommandHandler(),
                _ => null,
            };
        }
    }
}