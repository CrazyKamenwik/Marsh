namespace TS.MailService.Infrastructure.Enums;

[Flags]
public enum EmailStatusEntity
{
    Draft = 1,
    Queued = 2,
    Sent = 4,
    Failed = 8

    // REQUESTS	DELIVERED	OPENS	UNIQUE OPENS	CLICKS	UNIQUE CLICKS	UNSUBSCRIBES	BOUNCES	SPAM REPORTS	BLOCKS	BOUNCE DROPS	SPAM REPORT DROPS	UNSUBSCRIBE DROPS	INVALID EMAILS
}