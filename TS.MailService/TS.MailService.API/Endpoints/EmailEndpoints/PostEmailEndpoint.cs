using FastEndpoints;
using Newtonsoft.Json;
using TS.MailService.Application.Mappers.EmailMappers;
using TS.MailService.Application.Models;
using TS.MailService.Domain.Abstraction.Services;

namespace TS.MailService.Application.Endpoints.EmailEndpoints;

public class PostEmailEndpoint : Endpoint<ShortEmailMessageDto, EmailMessageDto, EmailWithRequestMapper>
{
    private readonly IEmailService _emailService;

    public PostEmailEndpoint(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public override void Configure()
    {
        Post("api/email");
        Description(b => b.WithTags("Email"));
        Summary(s => s.Summary = "Post new email.");
    }

    public override async Task HandleAsync(ShortEmailMessageDto shortEmail, CancellationToken cancellationToken)
    {
        Logger.LogInformation($"{JsonConvert.SerializeObject(shortEmail)}");

        var emailMessage = Map.ToEntity(shortEmail);

        emailMessage = await _emailService.SendEmail(emailMessage, cancellationToken);

        var emailMessageDto = Map.FromEntity(emailMessage);

        await SendAsync(emailMessageDto, cancellation: cancellationToken);
    }
}