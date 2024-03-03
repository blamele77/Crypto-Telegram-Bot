using System;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using System.Net.Http.Json;

namespace Telegram_Bot
{
    internal class main
    {
        static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            #region Vars
            string ANSI_RED = "\u001B[31m";

            #endregion

            var botClient = new TelegramBotClient("7129973761:AAHu4rR00NIkdDAMLpR0uVHIN7a64M4VLU0");
            botClient.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync); tt

            Console.ReadLine();
        }
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var _msg = update.Message;

            var _text = _msg.Text;

            Console.WriteLine($"{_msg.Chat.FirstName} | {_text}");

            if (_text.ToLower().Contains("bitcoin"))
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);

                    string price = jsonObject["price"];

                    await botClient.SendTextMessageAsync(_msg.Chat.Id, $"Цена биткоина на данный момент составляет: {price}$");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        static async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
        }

    }
}
