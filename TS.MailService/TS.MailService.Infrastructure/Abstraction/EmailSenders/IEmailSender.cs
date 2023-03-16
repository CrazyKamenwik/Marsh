using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TS.MailService.Infrastructure.Entities;

namespace TS.MailService.Infrastructure.Abstraction.EmailSenders
{
    public interface IEmailSender
    {
        public Task SendEmail(MailMessage message, CancellationToken cancellationToken);
    }
}