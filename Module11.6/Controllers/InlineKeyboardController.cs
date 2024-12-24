using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Module11._6.Configuration;
using Module11._6.Services;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;
using System.Threading;

namespace Module11._6.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;
        private readonly TextMessageController _textMessageController;


        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, Update update, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).ActionCode = callbackQuery.Data;

            switch (callbackQuery?.Data)
            {
                case "count":
                    {
                        // Отправляем в ответ уведомление о выборе
                        await _telegramClient.SendMessage(callbackQuery.From.Id,
                            $"<b> Вы выбрали подсчет символов в строке.{Environment.NewLine}</b>" +
                            $"Введите любую строку.{Environment.NewLine}" +
                            $"{Environment.NewLine} Можно поменять выбор в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);

                        //(ValueButtons.valueButtons).Add("button1", "count");
                        ValueButtons.command = "count";

                        break;
                    }

                case "sum":
                    {
                        await _telegramClient.SendMessage(callbackQuery.From.Id,
                            $"<b> Вы выбрали подсчет суммы чисел.</b>{Environment.NewLine}" +
                            $"Введите целые числа через пробел.{Environment.NewLine}" +
                            $"{Environment.NewLine} Можно поменять выбор в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);

                        //(ValueButtons.valueButtons).Add("button2", "sum");

                        ValueButtons.command = "sum";

                        break;
                    }

            }
        }
    }
}
