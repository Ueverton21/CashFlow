using CashFlow.Application.UseCases.Users;
using CashFlow.Exception;
using CommomTestsUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new UserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void NameEmpty(string name)
    {
        //Arrange
        var validator = new UserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;
        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_REQUIRED));
    }
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void ErrorEmailEmpty(string email)
    {
        //Arrange
        var validator = new UserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;
        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_REQUIRED));
    }

    [Fact]
    public void ErrorEmailInvalid()
    {
        //Arrange
        var validator = new UserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "uebe";
        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_VALID));
    }
    [Fact]
    public void ErrorPasswordEmpty()
    {
        //Arrange
        var validator = new UserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;
        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_INVALID));
    }
}
