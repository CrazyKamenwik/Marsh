using System.Net.Mail;
using AutoMapper;
using FluentAssertions;
using TS.MailService.Application.Mappers.EmailMappers;
using TS.MailService.Domain.MapperProfiles;
using TS.MailService.Domain.Models;
using TS.MailService.Infrastructure.Entities;
using TS.MailService.Tests.InitializeModels;

namespace TS.MailService.Tests.UnitTests.MapperTests
{
    public class EmailMapperTests
    {
        private readonly IMapper _mapper;
        private readonly EmailWithRequestMapper _emailWithRequestMapper;
        private readonly EmailWithoutRequestMapper _emailWithoutRequestMapper;

        public EmailMapperTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));

            _mapper = new Mapper(configuration);
            _emailWithRequestMapper = new EmailWithRequestMapper();
            _emailWithoutRequestMapper = new EmailWithoutRequestMapper();
        }

        [Fact]
        public void FromEmailMessageToEmailMessageEntity_SuccessMapping()
        {
            // Arrange
            var emailMessage = InitializeData.GetEmailMessageFromUser();
            var expectedEmailMessage = InitializeData.GetEmailMessageEntityFromUser();

            // Act
            var result = _mapper.Map<EmailMessageEntity>(emailMessage);

            // Assert
            result.Should().BeEquivalentTo(expectedEmailMessage, opt =>
            {
                opt.Excluding(x => x.Id);
                return opt.Excluding(x => x.CreatedAt);
            });
        }

        [Fact]
        public void FromEmailMessageEntityToEmailMessage_SuccessMapping()
        {
            // Arrange
            var emailMessage = InitializeData.GetEmailMessageFromUser();
            var expectedEmailMessage = InitializeData.GetEmailMessageEntityFromUser();

            // Act
            var result = _mapper.Map<EmailMessage>(expectedEmailMessage);

            // Assert
            result.Should().BeEquivalentTo(emailMessage);
        }

        [Fact]
        public void FromShortEmailMessageDtoToEmailMessage_SuccessMapping()
        {
            // Arrange
            var shortEmail = InitializeData.GetShortEmailMessageFromUser();
            var expectedEmailMessage = InitializeData.GetEmailMessageFromUser();

            // Act
            var result = _emailWithRequestMapper.ToEntity(shortEmail);

            // Assert
            result.Should().BeEquivalentTo(expectedEmailMessage, opt =>
            {
                opt.Excluding(x => x.Id);
                return opt.Excluding(x => x.CreatedAt);
            });
        }

        [Fact]
        public void FromEmailMessageToEmailMessageDto_SuccessMapping()
        {
            // Arrange
            var emailMessage = InitializeData.GetEmailMessageFromUser();
            var expectedEmailMessageDto = InitializeData.GetEmailMessageDtoFromUser();

            // Act
            var result = _emailWithoutRequestMapper.FromEntity(emailMessage);

            // Assert
            result.Should().BeEquivalentTo(expectedEmailMessageDto);
        }

        [Fact]
        public void FromEmailMessageToMailMessage_SuccessMapping()
        {
            // Arrange
            var emailMessage = InitializeData.GetEmailMessageFromUser();

            // Act
            var result = _mapper.Map<MailMessage>(emailMessage);

            // Assert
            result.Subject.Should().Be(emailMessage.Subject);
            result.Sender.Should().Be(emailMessage.Sender);
            result.Body.Should().Be(emailMessage.Body);
            result.To.Count.Should().Be(emailMessage.Recipients.Count());
            emailMessage.Recipients.Should().Contain(result.To.Select(x => x.Address));
        }
    }
}