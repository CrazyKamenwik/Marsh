namespace TS.Messengers.Microservice.BLL.Models.Telegram;

public class TelegramMessageUpdate : BaseModel
{
    public Guid SecretKey { get; set; }
    public string Text { get; set; } = null!;
    public TelegramUserModel User { get; set; } = null!;
}