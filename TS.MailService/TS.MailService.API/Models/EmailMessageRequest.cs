using Microsoft.AspNetCore.Mvc;

namespace TS.MailService.Application.Models
{
    public class EmailMessageRequest
    {
        [FromRoute]
        public Guid Id { get; set; }
    }
}