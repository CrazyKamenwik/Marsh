using FluentAssertions;
using FluentValidation.Results;
using TS.MailService.Application.Models;
using TS.MailService.Application.Validators.EmailValidators;

namespace TS.MailService.Tests.UnitTests.ValidatorsTests.EmailValidators;

public class EmailMessageValidatorTest
{
    [Theory]
    [InlineData("aaaaa@", "Important email", new[] { "example@gmail.com" })]
    [InlineData("a", "Important email", new[] { "example@gmail.com" })]
    [InlineData("", "Important email", new[] { "example@gmail.com" })]
    [InlineData(null, "Important email", new[] { "example@gmail.com" })]
    [InlineData("example@example", "", new[] { "example@gmail.com" })]
    [InlineData("example@example", null, new[] { "example@gmail.com" })]
    [InlineData("example@example", "Important email", new[] { "example@gmail.com" , "example.com" })]
    public void EmailMessageRequestValidatorTest_FailValidation(string sender, string subject, IEnumerable<string> recipients)
    {
        // Arrange
        ShortEmailMessageDto shortEmail = new()
        {
            Sender = sender,
            Subject = subject,
            Recipients = recipients,
        };

        var validator = new EmailMessageRequestValidator();

        // Act
        var result = validator.Validate(shortEmail);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("example@example", "Important email", new[] { "example@gmail.com" })]
    [InlineData("example@example.com", "Important email", new[] { "example@gmail.com" })]
    [InlineData("example@example.com", "A", new[] { "example@gmail.com" })]
    [InlineData("example@example.com", "Important email", new[] { "example@example" })]
    [InlineData("example@example.com", "Important email", new[] { "example@example.com" })]
    public void EmailMessageRequestValidatorTest_SuccessValidation(string sender, string subject, IEnumerable<string> recipients)
    {
        // Arrange
        ShortEmailMessageDto shortEmail = new()
        {
            Sender = sender,
            Subject = subject,
            Recipients = recipients,
        };

        var validator = new EmailMessageRequestValidator();

        // Act
        var result = validator.Validate(shortEmail);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}