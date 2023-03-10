namespace TS.Messengers.Microservice.API.ViewModels.Telegram;

public class TelegramBotWebHookViewModel
{
    public Guid SeretKey { get; set; }
    public string BotToken { get; set; } = null!;
}