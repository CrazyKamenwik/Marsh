﻿namespace TS.MailService.Infrastructure.Entities;

internal class SmtpEntity
{
    public const string Position = "Smtp";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}