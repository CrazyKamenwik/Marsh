using FluentValidation.TestHelper;
using Shouldly;
using TicketSystem.API.Validators;
using TicketSystem.API.ViewModels.Messages;

namespace TicketSystem.Tests.UnitTests.ValidatorTests;

public class ShortMessageValidatorTest
{
    private readonly ShortMessageValidator _validator;

    public ShortMessageValidatorTest()
    {
        _validator = new ShortMessageValidator();
    }

    [Theory]
    [InlineData("H", null, 1)]
    [InlineData("H", 1, 1)]
    [InlineData("H", int.MaxValue, int.MaxValue)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", int.MinValue, int.MaxValue)]
    [InlineData("Hello", null, 123456789)]
    public async Task ShortMessageValidator_CorrectParameters_Success(string text, int? ticketId, int userId)
    {
        // Arrange
        var shortMessage = new ShortMessageViewModel
        {
            Text = text,
            TicketId = ticketId,
            UserId = userId
        };

        // Act
        var result = await _validator.TestValidateAsync(shortMessage);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null, null, int.MinValue)]
    [InlineData(null, 1, 1)]
    [InlineData("H", int.MaxValue, 0)]
    [InlineData("", int.MinValue, int.MaxValue)]
    [InlineData("Hello", null, -1)]
    public async Task ShortMessageValidator_IncorrectParameters_FailValidation(string text, int? ticketId, int userId)
    {
        // Arrange
        var shortMessage = new ShortMessageViewModel
        {
            Text = text,
            TicketId = ticketId,
            UserId = userId
        };

        // Act
        var result = await _validator.TestValidateAsync(shortMessage);

        // Assert
        result.IsValid.ShouldBeFalse();
    }
}