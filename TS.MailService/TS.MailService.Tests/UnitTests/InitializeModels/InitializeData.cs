using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.MailService.Application.Enums;
using TS.MailService.Application.Models;
using TS.MailService.Domain.Enums;
using TS.MailService.Domain.Models;
using TS.MailService.Infrastructure.Entities;
using TS.MailService.Infrastructure.Enums;

namespace TS.MailService.Tests.UnitTests.InitializeModels
{
    public static class InitializeData
    {
        public static readonly Guid UserEmailId = Guid.NewGuid();
        public static readonly Guid SupportEmailId = Guid.NewGuid();

        private const string UserEmail = "user@gmail.com";
        private const string SupportEmail = "support@gmail.com";

        private const string UserEmailSubject = "I have a problem";
        private const string SupportEmailSubject = "Solution of the problem";

        private static readonly string[] UserRecipients = { "user@gmail.com" };
        private static readonly string[] SupportRecipients = { "support@gmail.com" };

        private const string UserEmailBody = "When I go to the site I do not see the product page";
        private const string SupportEmailBody = "To go to the product page you need to log in";

        private static readonly DateTime UserEmailCreatedAt = new(2023, 03, 1);
        private static readonly DateTime SupportEmailCreatedAt = new(2023, 03, 2);

        public static ShortEmailMessageDto GetShortEmailMessageFromUser() => new()
        {
            Body = UserEmailBody,
            Recipients = UserRecipients,
            Sender = UserEmail,
            Subject = UserEmailSubject
        };

        public static EmailMessageDto GetEmailMessageDtoFromUser() => new()
        {
            Id = UserEmailId,
            Body = UserEmailBody,
            Recipients = UserRecipients,
            Sender = UserEmail,
            Subject = UserEmailSubject,
            CreatedAt = UserEmailCreatedAt
        };

        public static EmailMessage GetEmailMessageFromUser() => new()
        {
            Id = UserEmailId,
            Body = UserEmailBody,
            CreatedAt = UserEmailCreatedAt,
            Recipients = UserRecipients,
            Sender = UserEmail,
            Subject = UserEmailSubject
        };

        public static EmailMessage GetEmailMessageFromSupport() => new()
        {
            Id = SupportEmailId,
            Body = SupportEmailBody,
            CreatedAt = SupportEmailCreatedAt,
            Recipients = SupportRecipients,
            Sender = SupportEmail,
            Subject = SupportEmailSubject
        };

        public static IEnumerable<EmailMessage> GetEmailMessages() => new List<EmailMessage>()
        {
            GetEmailMessageFromUser(),
            GetEmailMessageFromSupport()
        };

        public static EmailMessageEntity GetEmailMessageEntityFromUser() => new()
        {
            Id = UserEmailId,
            Body = UserEmailBody,
            CreatedAt = UserEmailCreatedAt,
            Recipients = UserRecipients,
            Sender = UserEmail,
            Subject = UserEmailSubject
        };

        public static EmailMessageEntity GetEmailMessageEntityFromSupport() => new()
        {
            Id = SupportEmailId,
            Body = SupportEmailBody,
            CreatedAt = SupportEmailCreatedAt,
            Recipients = SupportRecipients,
            Sender = SupportEmail,
            Subject = SupportEmailSubject
        };

        public static IEnumerable<EmailMessageEntity> GetEmailMessageEntities() => new List<EmailMessageEntity>()
        {
            GetEmailMessageEntityFromUser(),
            GetEmailMessageEntityFromSupport()
        };
    }
}
