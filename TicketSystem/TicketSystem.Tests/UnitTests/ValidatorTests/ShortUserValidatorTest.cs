using FluentValidation.TestHelper;
using TicketSystem.API.Validators;
using TicketSystem.API.ViewModels.Users;

namespace TicketSystem.Tests.UnitTests.ValidatorTests;

public class ShortUserValidatorTest
{
    private readonly ShortUserValidator _validator;

    public ShortUserValidatorTest()
    {
        _validator = new ShortUserValidator();
    }

    [Theory]
    [InlineData("AA", "A")]
    [InlineData("AA", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "User")]
    public async Task ShortUserValidator_CorrectParameters_Success(string name, string userRole)
    {
        // Arrange
        var user = new ShortUserViewModel
        {
            Name = name,
            UserRole = userRole
        };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("A", "A")]
    [InlineData(null, null)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "User")]
    [InlineData("Vlad", "")]
    [InlineData("Vlad", null)]
    [InlineData(null, "User")]
    public async Task ShortUserValidator_IncorrectParameters_FailValidation(string name, string userRole)
    {
        // Arrange
        var user = new ShortUserViewModel
        {
            Name = name,
            UserRole = userRole
        };

        // Act
        var result = await _validator.TestValidateAsync(user);

        // Assert
        Assert.False(result.IsValid);
    }
}