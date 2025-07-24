using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestsUtilities.Repositories;

public class UserWriteRepositoryBuilder
{
    public static IUserWriteRepository Build()
    {
        var mock = new Mock<IUserWriteRepository>();

        return mock.Object;
    }
}
