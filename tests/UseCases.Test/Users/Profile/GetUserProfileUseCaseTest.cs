using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Domain.Entities;
using CommomTestsUtilities.Entities;
using CommomTestsUtilities.LoggedUser;
using CommomTestsUtilities.Mapper;
using FluentAssertions;

namespace UseCases.Test.Users.Profile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var response = await useCase.Execute();

        response.Should().NotBeNull();

        response.Name.Should().Be(user.Name);
        response.Email.Should().Be(user.Email);
    }
    private GetUserProfileUseCase CreateUseCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();

        return new GetUserProfileUseCase(loggedUser,mapper);
    }
}
