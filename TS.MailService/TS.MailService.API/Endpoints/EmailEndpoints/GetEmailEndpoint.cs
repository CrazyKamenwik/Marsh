using FastEndpoints;
using Newtonsoft.Json;
using TS.MailService.Application.Mappers.EmailMappers;
using TS.MailService.Application.Models;
using TS.MailService.Domain.Abstraction.Services;

namespace TS.MailService.Application.Endpoints.EmailEndpoints;

public class GetEmailEndpoint : Endpoint<EmailMessageRequest, EmailMessageDto, EmailWithRequestMapper>
{
    private readonly IEmailService _emailService;

    public GetEmailEndpoint(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public override void Configure()
    {
        Get("api/email/{Id}");
        Description(b => b.WithTags("Email"));
        Summary(s =>
        {
            s.Summary = "Get email by id.";
            s.Params["Id"] = "Email unique identifier";
        });
    }

    public override async Task HandleAsync(EmailMessageRequest emailRequest, CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Get email by id: {emailRequest.Id}");

        var emailMessage = await _emailService.GetEmail(emailRequest.Id, cancellationToken);

        var emailMessageDto = Map.FromEntity(emailMessage);

        await SendAsync(emailMessageDto, cancellation: cancellationToken);
    }
}