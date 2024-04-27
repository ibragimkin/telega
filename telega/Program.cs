using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
namespace telega
{
    internal class Program
    {
        static async Task Main()
        {
            var botClient = new TelegramBotClient("7064687117:AAHJ7po8ZahJNArafaVqsolssfeAJqYXMxE");
            Console.WriteLine("Подключение к боту...");
            var me = await botClient.GetMeAsync();
            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };
            MyUpdateHandler myUpdateHandler = new MyUpdateHandler();
            botClient.ReceiveAsync(
                myUpdateHandler.HandleUpdateAsync,
                pollingErrorHandler: myUpdateHandler.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
                );
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
            Console.WriteLine($"\nБот @{me.Username} готов к работе.");
            Console.WriteLine("Напишите \"000\" для завершения работы.");
            while (true)
            {
                if (Console.ReadLine() == "000") break;
            }
            // Send cancellation request to stop bot
            cts.Cancel();
        }

    }
}
