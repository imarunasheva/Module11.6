using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Module11._6.Configuration;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using System.IO;
using Module11._6.Controllers;

namespace Module11._6.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, Update update, CancellationToken ct)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    {
                        if (message.Text == "/start")
                        {
                            // Объект, представляющий кнопки
                            var buttons = new List<InlineKeyboardButton[]>();
                            buttons.Add(new[]
                            {
                                        InlineKeyboardButton.WithCallbackData($" Кол-во символов" , $"count"),
                                        InlineKeyboardButton.WithCallbackData($" Сумма цифр" , $"sum")
                                    });

                            // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                            await _telegramClient.SendMessage(message.Chat.Id, $"<b> Наш бот может подсчитать кол-во символов в сообщении или вычислить сумму чисел, введенных через пробел: </b>{Environment.NewLine}",
                                cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                        }
                        
                        else if (ValueButtons.command == "count")
                        {
                            await _telegramClient.SendMessage(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: ct);
                        }

                        else if (ValueButtons.command == "sum")
                        {
                            string s = update.Message.Text;
                            if (!Regex.IsMatch(s, @"[^\d\s]") == true)
                            {
                                string[] items = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                var result = items.Select(i => Convert.ToInt32(i)).Sum();
                                await _telegramClient.SendMessage(update.Message.From.Id, $"Сумма цифр: {result}", cancellationToken: ct);
                            }

                            else
                            {
                                await _telegramClient.SendMessage(update.Message.From.Id, $"Введите целые числа через пробел.", cancellationToken: ct);
                            }
                        }
                        break;
                    }

                default:
                    await _telegramClient.SendMessage(update.Message.From.Id, $"Данный тип сообщений не поддерживается.", cancellationToken: ct);
                    break;
            }

        }
    }
}