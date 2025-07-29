﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommomTestsUtilities.Repositories;

public class ExpenseUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpenseUpdateOnlyRepository> _repository;

    public ExpenseUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpenseUpdateOnlyRepository>();
    }
    public ExpenseUpdateOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);

        return this;
    }

    public IExpenseUpdateOnlyRepository Build() => _repository.Object;
}
