namespace TS.Messengers.Microservice.BLL.Models.Telegram;

public class TelegramBotWebHook : BaseModel
{
    public Guid SeretKey { get; set; }
    public string BotToken { get; set; } = null!;
}