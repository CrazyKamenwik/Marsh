using System.Net.Mail;
using AutoMapper;
using TS.MailService.Domain.Abstraction.Services;
using TS.MailService.Domain.Models;
using TS.MailService.Infrastructure.Abstraction.EmailSenders;
using TS.MailService.Infrastructure.Abstraction.Repository;
using TS.MailService.Infrastructure.Entities;

namespace TS.MailService.Domain.Services;

public class EmailService : IEmailService
{
    private readonly IGenericRepository<EmailMessageEntity> _repository;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public EmailService(IGenericRepository<EmailMessageEntity> repository, IMapper mapper, IEmailSender emailSender)
    {
        _mapper = mapper;
        _repository = repository;
        _emailSender = emailSender;
    }

    public async Task<EmailMessage> SendEmail(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        var mailMessage = _mapper.Map<MailMessage>(emailMessage);

        await _emailSender.SendEmail(mailMessage);

        var emailMessageEntity = _mapper.Map<EmailMessageEntity>(emailMessage);

        await _repository.Create(emailMessageEntity, cancellationToken);

        emailMessage = _mapper.Map<EmailMessage>(emailMessageEntity);

        return emailMessage;
    }

    public async Task<EmailMessage> GetEmail(Guid id, CancellationToken cancellationToken)
    {
        var emailMessageEntity = await _repository.Get(id, cancellationToken);

        return _mapper.Map<EmailMessage>(emailMessageEntity);
    }

    public async Task<IEnumerable<EmailMessage>> GetEmails(CancellationToken cancellationToken)
    {
        var emailMessageEntities = await _repository.GetAll(cancellationToken);

        return _mapper.Map<IEnumerable<EmailMessage>>(emailMessageEntities);
    }
}