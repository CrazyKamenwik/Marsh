using AutoMapper;
using FluentAssertions;
using Moq;
using TS.MailService.Domain.Abstraction.Services;
using TS.MailService.Domain.Models;
using TS.MailService.Domain.Services;
using TS.MailService.Infrastructure.Abstraction.Repository;
using TS.MailService.Infrastructure.Entities;
using TS.MailService.Tests.InitializeModels;
using TS.MailService.Tests.UnitTests.Moq;
using IEmailSender = TS.MailService.Infrastructure.Abstraction.EmailSenders.IEmailSender;

namespace TS.MailService.Tests.UnitTests.ServicesTests
{
    public class EmailServiceTest
    {
        private readonly IEmailService _emailService;
        private readonly Mock<IGenericRepository<EmailMessageEntity>> _genericRepository;
        private readonly Mock<IMapper> _mapper;

        public EmailServiceTest()
        {
            _genericRepository = GenericRepositoryMock<EmailMessageEntity>.GetMock(InitializeData.GetEmailMessageEntities());
            _mapper = new Mock<IMapper>();
            Mock<IEmailSender> emailSender = new();
            _emailService = new EmailService(_genericRepository.Object, _mapper.Object, emailSender.Object);
        }

        [Fact]
        public async Task SendEmail_CallCorrectMethodOfRepoWithCorrectTypeAndReturnEmailModel()
        {
            // Arrange
            var email = InitializeData.GetEmailMessageFromUser();
            var emailEntity = InitializeData.GetEmailMessageEntityFromUser();
            _mapper
                .Setup(x => x.Map<EmailMessage>(emailEntity))
                .Returns(email);
            _mapper
                .Setup(x => x.Map<EmailMessageEntity>(email))
                .Returns(emailEntity);

            // Act
            var result = await _emailService.SendEmail(email, CancellationToken.None);

            // Assert
            result.Should().BeOfType<EmailMessage>();
            result.Should().BeEquivalentTo(email);
            _genericRepository.Verify(x => 
                x.Create(emailEntity, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetEmailById_CallCorrectMethodOfRepoWithCorrectTypeAndReturnEmailModel()
        {
            // Arrange
            var userEmailId = InitializeData.UserEmailId;
            _mapper
                .Setup(x => x.Map<EmailMessage>(It.IsAny<EmailMessageEntity>()))
                .Returns(InitializeData.GetEmailMessageFromUser());

            // Act
            var result = await _emailService.GetEmail(userEmailId, CancellationToken.None);

            // Assert
            result.Should().BeOfType<EmailMessage>();
            _genericRepository.Verify(x =>
                x.Get(userEmailId, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetEmails_CallCorrectMethodOfRepoWithCorrectTypeAndReturnEmailModel()
        {
            // Arrange
            _mapper
                .Setup(x => x.Map<IEnumerable<EmailMessage>>(It.IsAny<IEnumerable<EmailMessageEntity>>()))
                .Returns(InitializeData.GetEmailMessages());

            // Act
            var result = await _emailService.GetEmails(CancellationToken.None);

            // Assert
            result.Should().AllBeOfType<EmailMessage>();
            result.Should().NotBeEmpty();
            _genericRepository.Verify(x =>
                x.GetAll(CancellationToken.None), Times.Once);
        }
    }
}