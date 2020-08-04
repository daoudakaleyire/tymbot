namespace tymbot
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.DependencyInjection;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using tymbot.Commands;
    using tymbot.Data;

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
                    Command = BotCommands.Time,
                    Description = "User local time",
                },
                new BotCommand()
                {
                    Command = BotCommands.Friend,
                    Description = "Add friend to view your local time.",
                },
                new BotCommand()
                {
                    Command = BotCommands.Timezone,
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
            string text = e.Message.Text?.Trim();
            if (text == null || !text.StartsWith("/")) {
                return;
            }

            var command = GetCommandFromMessage(text);
            var handler = CommandHandlerFactory.GetHandler(command);
            if (handler == null) {
                return;
            }

            using var scope = this.serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<TymDbContext>();
            var response = await handler.HandleAsync(e.Message, db);
            if (response.Length > 0) {
                await botClient.SendTextMessageAsync(e.Message.Chat, response, replyToMessageId: e.Message.MessageId); 
            }
        }

        private string GetCommandFromMessage(string message)
        {
            StringBuilder command = new StringBuilder();
            for (int i = 1; i < message.Length; i++) {
                if (char.IsWhiteSpace(message[i])) {
                    break;
                }
                command.Append(message[i]);
            }

            return command.ToString();
        }
    }
}