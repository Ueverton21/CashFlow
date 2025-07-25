﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpenseWriteOnlyRepository
{
    Task Add(Expense expense);
    /// <summary>
    ///     This function return TRUE if the deletion was successful
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
}
