using FastEndpoints;
using Microsoft.Extensions.Logging;
using TS.MailService.Application.Mappers.EmailMappers;
using TS.MailService.Application.Models;
using TS.MailService.Domain.Abstraction.Services;

namespace TS.MailService.Application.Endpoints.EmailEndpoints
{
    public class GetEmailsEndpoint : EndpointWithoutRequest<IEnumerable<EmailMessageDto>, EmailWithoutRequestMapper>
    {
        private readonly IEmailService _emailService;

        public GetEmailsEndpoint(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public override void Configure()
        {
            Get("api/email");
            Description(b => b.WithTags("Email"));
            Summary(s => s.Summary = "Get all emails");
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Get all emails");

            var emailMessages = await _emailService.GetEmails(cancellationToken);

            var emailsDto = new List<EmailMessageDto>();

            emailsDto.AddRange(emailMessages.Select(Map.FromEntity));

            await SendAsync(emailsDto, cancellation: cancellationToken);
        }
    }
}