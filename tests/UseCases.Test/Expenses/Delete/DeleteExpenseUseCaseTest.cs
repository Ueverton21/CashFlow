using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CommomTestsUtilities.Entities;
using CommomTestsUtilities.LoggedUser;
using CommomTestsUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user,expense);

        var act = () => useCase.Execute(expense.Id);

        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user, expense);

        var act = () => useCase.Execute(10);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense expense = null)
    {
        var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(repositoryWriteOnly, unitOfWork, repository, loggedUser);
    }
}
