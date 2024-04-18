using FileProcessing;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace telega
{
    internal class MyUpdateHandler : IUpdateHandler
    {
        private Dictionary<long, Message> lastMessages = new Dictionary<long, Message>(); // Последнее сообщение от пользователя.

        private Dictionary<long, int> userStatus = new Dictionary<long, int>(); // Статус пользователя.
        /// <summary>
        /// Значения статусов: 0 - Начало. 1 - Получен файл. 2 - Сортировка. 3 - Фильтрация (выбор поля). 4 - Фильтрация (ввод слова). 5 - Сортировка в обр. сторону.
        /// </summary>

        private Dictionary<long, List<IceHill>> fileData = new Dictionary<long, List<IceHill>>(); // Последний загруженный файл от пользователя
        private DataProcessing dataProcesser = new DataProcessing();
        private CSVProcessing CSVProcessing = new CSVProcessing();
        private JsonProcessing jsonProcessing = new JsonProcessing();
        private ReplyKeyboardMarkup replyKeyboardMarkup = GetKeyboard(new[] { new[] { "Сортировка", "Сорт. в обр. сторону", "Фильтрация" }, new[] { "Получить файл JSON", "Получить CSV" } }); // Часто используется, поэтому сохранён отдельно.

        /// <summary>
        /// Основной метод. Вызывается после каждого сообщения, далее обрабатывает их.
        /// </summary>
        /// <param name="botClient">Бот, нужен для отправки сообщений пользователю.</param>
        /// <param name="update">Содержит сообщение и данные пользователя.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Проверяем, является ли это сообщение
            if (update.Message != null)
            {
                var message = update.Message;
                long userId = message.Chat.Id;
                // Проверяем, было ли уже сообщение от этого пользователя
                if (lastMessages.ContainsKey(userId) && message.Text != "/start")
                {
                    if (update.Message.Type == MessageType.Document)
                    {
                        FileHandler(botClient, message);
                    }
                    Console.WriteLine($"Received a text message in chat {userId}: \"{message.Text}\"");
                    if (userStatus[userId] >= 1)
                    {
                        if (userStatus[userId] == 2) await SortingHandler(botClient, update);
                        else if (message.Text == "Сортировка")
                        {
                            await botClient.SendTextMessageAsync(userId, "Выберите поле для сортировки в меню: " + IceHill.GetFieldNamesString(), replyMarkup: GetKeyboard(IceHill.GetAllFieldNamesKeyboard()));
                            userStatus[userId] = 2;
                        }
                        else if (message.Text == "Сорт. в обр. сторону")
                        {
                            await botClient.SendTextMessageAsync(userId, "Выберите поле для сортировки в обр. сторону в меню: " + IceHill.GetFieldNamesString(), replyMarkup: GetKeyboard(IceHill.GetAllFieldNamesKeyboard()));
                            userStatus[userId] = 5;
                        }
                        else if (userStatus[userId] == 5) await SortingHandler(botClient, update, true);
                        else if (message.Text == "Получить CSV")
                        {
                            await using Stream stream = CSVProcessing.Write(fileData[userId]);
                            await botClient.SendDocumentAsync(userId, InputFile.FromStream(stream: stream, fileName: "ice-hill-edited.csv"));
                        }
                        else if (message.Text == "Фильтрация")
                        {
                            userStatus[userId] = 3;
                            await botClient.SendTextMessageAsync(userId, "Выберите поле для фильтрации в меню: " + IceHill.GetFieldNamesString(), replyMarkup: GetKeyboard(IceHill.GetAllFieldNamesKeyboard()));
                        }
                        else if (userStatus[userId] == 3)
                        {
                            dataProcesser.filtrationPropertyName = update.Message.Text;
                            await botClient.SendTextMessageAsync(userId, "Введите слово для фильтрации:");
                            userStatus[userId] = 4;

                        }
                        else if (userStatus[userId] == 4)
                        {
                            await FiltrationHandler(botClient, update, message.Text);

                        }
                        else if (message.Text == "Получить файл JSON")
                        {
                            await using Stream stream = await jsonProcessing.Write(fileData[userId]);
                            await botClient.SendDocumentAsync(userId, InputFile.FromStream(stream: stream, fileName: "ice-hill-edited.json"));
                        }
                        else await botClient.SendTextMessageAsync(userId, "Не знаю такой команды", replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(userId, "Пришлите файл ice-hills.csv или .json");
                    }
                }
                else
                {
                    await Starter(botClient, message, userId);
                }
                // Обновляем последнее сообщение для этого пользователя
                lastMessages[userId] = message;
            }
        }

        /// <summary>
        /// Приветствует пользователя.
        /// </summary>
        /// <param name="botClient">Бот, нужен для отправки сообщения.</param>
        /// <param name="message">Хранит данные о пользователе.</param>
        /// <param name="userId">ID пользователя.</param>
        /// <returns></returns>
        private async Task Starter(ITelegramBotClient botClient, Message message, long userId)
        {
            lastMessages.Remove(userId);
            fileData.Remove(userId); // При /start очищает данные пользователя.
            await botClient.SendTextMessageAsync(userId, $"Привет, {message.Chat.Username}.\nЯ - бот для работы с CSV и JSON файлами ice-hills (вариант 11). Я умею сортировать и фильровать данные.\nПришлите CSV или JSON файл, с которым я должен работать.");
            Console.WriteLine($"Received the first message from chat {userId}");
            userStatus[userId] = 0;
        }

        /// <summary>
        /// Фильтрует коллекцию по слову.
        /// </summary>
        /// <param name="botClient">Бот, нужен для отправки сообщения.</param>
        /// <param name="update">Содержит сообщение и данные пользователя.</param>
        /// <param name="keyword">Слово для фильтрации.</param>
        /// <returns></returns>
        private async Task FiltrationHandler(ITelegramBotClient botClient, Update update, string keyword)
        {
            var messageText = update.Message.Text;
            if (!IceHill.GetFieldNames().Contains(dataProcesser.filtrationPropertyName)) await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Такого поля нет в файле. ({dataProcesser.filtrationPropertyName})", replyMarkup: replyKeyboardMarkup);
            else
            {
                fileData[update.Message.Chat.Id] = dataProcesser.Filtration(fileData[update.Message.Chat.Id], update.Message.Text);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Успешно отфильтровано.", replyMarkup: replyKeyboardMarkup);
                userStatus[update.Message.Chat.Id] = 1;

            }
        }

        /// <summary>
        /// Сортирует коллекцию по полю, которое ввёл пользователь.
        /// </summary>
        /// <param name="botClient">Бот, нужен для отправки сообщения.</param>
        /// <param name="update">Содержит сообщение и данные пользователя.</param>
        /// <param name="reverseOrder">Нужно ли сортировать в обратную сторону.</param>
        /// <returns></returns>
        private async Task SortingHandler(ITelegramBotClient botClient, Update update, bool reverseOrder = false)
        {
            var messageText = update.Message.Text;
            var userId = update.Message.Chat.Id;
            if (!IceHill.GetFieldNames().Contains(messageText))
            {
                await botClient.SendTextMessageAsync(userId, "Такого поля нет в файле.", replyMarkup: replyKeyboardMarkup);
                userStatus[userId] = 1;
            }
            else
            {
                if (!reverseOrder) fileData[userId] = dataProcesser.Sort(fileData[userId], messageText);
                else fileData[userId] = dataProcesser.SortReverse(fileData[userId], messageText);
                await botClient.SendTextMessageAsync(userId, "Успешно отсортировано.", replyMarkup: replyKeyboardMarkup);
                userStatus[userId] = 1;

            }
        }

        /// <summary>
        /// Удобно создает кнопки выбора для тг чата.
        /// </summary>
        /// <param name="rows">Массив массивов кнопок.</param>
        /// <returns>Готовые кнопки.</returns>
        private static ReplyKeyboardMarkup GetKeyboard(params string[][] rows)
        {
            var keyboardButtons = new KeyboardButton[rows.Length][];
            for (int i = 0; i < rows.Length; i++)
            {
                keyboardButtons[i] = new KeyboardButton[rows[i].Length];
                for (int j = 0; j < rows[i].Length; j++)
                {
                    keyboardButtons[i][j] = new KeyboardButton(rows[i][j]);
                }
            }
            var result = new ReplyKeyboardMarkup(keyboardButtons);
            result.IsPersistent = true;
            result.ResizeKeyboard = true;
            result.OneTimeKeyboard = true;
            return result;
        }

        /// <summary>
        /// Работает с файлом.
        /// </summary>
        /// <param name="botClient">Бот, нужен для отправки сообщения.</param>
        /// <param name="message">Сообщение, содержащее файл (желательно).</param>
        private async void FileHandler(ITelegramBotClient botClient, Message message)
        {
            if (message == null || message.Type != MessageType.Document) return;
            var document = message.Document;
            var file = await botClient.GetFileAsync(document.FileId);
            var userId = message.Chat.Id;
            string fileExtension = Path.GetExtension(document.FileName) ?? "";
            if (fileExtension != ".json" && fileExtension != ".csv") await botClient.SendTextMessageAsync(userId, "Неправильный формат файла.");
            using (var stream = new MemoryStream())
            {
                await botClient.DownloadFileAsync(file.FilePath, stream);

                if (fileExtension == ".csv")
                {
                    var data = await CSVProcessing.Read(stream);
                    if (data == null)
                    {
                        await botClient.SendTextMessageAsync(userId, "Файл не принят. Неправильный формат файла.");
                        return;
                    }
                    fileData[userId] = data;
                }
                else
                {
                    var data = await jsonProcessing.Read(stream);
                    if (data == null)
                    {
                        await botClient.SendTextMessageAsync(userId, "Файл не принят. Неправильный формат файла.");
                        return;
                    }
                    fileData[userId] = data;
                }

                userStatus[userId] = 1;
                await botClient.SendTextMessageAsync(userId, "Файл принят. Выберите действие", replyMarkup: replyKeyboardMarkup);
            }

            Console.WriteLine($"Received a '{message.Document.FileName}' file in chat {message.Chat.Username}.");
        }

        /// <summary>
        /// Активируется при возникновении ошибок.
        /// </summary>
        /// <param name="botClient">Бот.</param>
        /// <param name="exception">Исключение, из-за которого был вызван.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage); // Надеюсь не пригодится
            return Task.CompletedTask;
        }
    }
}
