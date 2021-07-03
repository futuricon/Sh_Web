using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Telegram.Bot.Types.Enums;
using Sh.Domain.Interfaces;

namespace Sh.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendToAllTGAsync(string msg)
        {
            try
            {
                var bot = new Telegram.Bot.TelegramBotClient(_config.GetSection("TelegramAPIToken").Value);
                await bot.SendTextMessageAsync(_config.GetSection("TelegramChanelId").Value, msg, ParseMode.Html);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }
    }
}
