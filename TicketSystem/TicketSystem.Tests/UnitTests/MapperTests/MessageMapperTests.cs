using AutoMapper;
using FluentAssertions;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.Tests.UnitTests.InitializeModels;

namespace TicketSystem.Tests.UnitTests.MapperTests;

public class MessageMapperTests
{
    private readonly IMapper _mapper;

    public MessageMapperTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BLL.MapperProfiles.MapperProfile());
            cfg.AddProfile(new API.MapperProfiles.MapperProfile());
        });

        _mapper = new Mapper(configuration);
    }

    [Fact]
    public Task FromShortMessageToMessage_AllFields_Success()
    {
        // Arrange
        var shortMessage = InitializeData.GetShortMessageViewModelFromOperator();

        // Act
        var message = _mapper.Map<Message>(shortMessage);

        // Assert
        message.Should().BeEquivalentTo(shortMessage);
        return Task.CompletedTask;
    }

    [Fact]
    public Task FromShortMessageToMessage_WithoutTicketId_Success()
    {
        // Arrange
        var shortMessage = InitializeData.GetShortMessageViewModelFromUser();
        var expectedMessage = new Message
        {
            Text = shortMessage.Text,
            UserId = shortMessage.UserId
        };

        // Act
        var result = _mapper.Map<Message>(shortMessage);

        // Assert
        result.Should().BeEquivalentTo(expectedMessage, o => o.Excluding(m => m.CreatedAt));
        return Task.CompletedTask;
    }

    [Fact]
    public Task FromMessageToMessageViewModel_Success()
    {
        // Arrange
        var shortMessage = InitializeData.GetMessageModelFromOperator();
        var expectedMessage = new MessageViewModel
        {
            Text = shortMessage.Text,
            Id = shortMessage.Id
        };

        // Act
        var result = _mapper.Map<MessageViewModel>(shortMessage);

        // Assert
        result.Should().BeEquivalentTo(expectedMessage);
        return Task.CompletedTask;
    }

    [Fact]
    public Task FromMessageEntityToMessage_Success()
    {
        // Arrange
        var messageEntity = InitializeData.GetMessageEntityFromUser();

        // Act
        var result = _mapper.Map<Message>(messageEntity);

        // Assert
        result.Should().BeEquivalentTo(messageEntity);
        return Task.CompletedTask;
    }

    [Fact]
    public Task FromMessageToMessageEntity_Success()
    {
        // Arrange
        var message = InitializeData.GetMessageModelFromUser();

        // Act
        var result = _mapper.Map<MessageEntity>(message);

        // Assert
        result.Should().BeEquivalentTo(message);
        return Task.CompletedTask;
    }
}