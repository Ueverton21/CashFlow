using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requets;
using FluentAssertions;
using FluentValidation;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("Aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaa1")]
    public void PasswordInvalid(string password)
    {
        //Arrange
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        //Act
        var result = validator.IsValid(new 
            ValidationContext<RequestRegisterUserJson>(
            new RequestRegisterUserJson()),password);
        //Assert
        result.Should().BeFalse();
    }
}
