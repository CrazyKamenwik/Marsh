namespace TS.MailService.Application.Enums;

[Flags]
public enum EmailStatusDto
{
    Draft = 1,
    Queued = 2,
    Sent = 4,
    Failed = 8
}