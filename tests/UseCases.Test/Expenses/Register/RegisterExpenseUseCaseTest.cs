using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommomTestsUtilities.Entities;
using CommomTestsUtilities.LoggedUser;
using CommomTestsUtilities.Mapper;
using CommomTestsUtilities.Repositories;
using CommomTestsUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public async Task Error_Title_Empty(string title)
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = title;
        var useCase = CreateUseCase(loggedUser);

        var result = () => useCase.Execute(request);

        await result.Should().ThrowAsync<ErrorOnValidationException>();
    }

    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new RegisterExpenseUseCase(repository, unitOfWork, mapper,loggedUser);
    }
}
