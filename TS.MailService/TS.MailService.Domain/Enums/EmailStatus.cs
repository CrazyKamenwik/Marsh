namespace TS.MailService.Domain.Enums;

[Flags]
public enum EmailStatus
{
    Draft = 1,
    Queued = 2,
    Sent = 4,
    Failed = 8
}