namespace TS.Messengers.Microservice.DAL.Entities.Telegram;

public class TelegramBotWebHookEntity : BaseEntity
{
    public Guid SeretKey { get; set; }
    public string BotToken { get; set; } = null!;
}