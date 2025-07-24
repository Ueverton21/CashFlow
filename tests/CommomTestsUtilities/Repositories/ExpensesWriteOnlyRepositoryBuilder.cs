using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommomTestsUtilities.Repositories;

public class ExpensesWriteOnlyRepositoryBuilder
{
    public static IExpenseWriteOnlyRepository Build()
    {
        var mock = new Mock<IExpenseWriteOnlyRepository>();

        return mock.Object;
    }
}
