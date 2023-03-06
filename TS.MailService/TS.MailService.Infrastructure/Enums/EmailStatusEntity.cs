namespace TS.MailService.Infrastructure.Enums;

[Flags]
public enum EmailStatusEntity
{
    Draft = 1,
    Queued = 2,
    Sent = 4,
    Failed = 8
}