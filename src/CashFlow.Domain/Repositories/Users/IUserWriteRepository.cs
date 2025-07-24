using CashFlow.Domain.Entities;
namespace CashFlow.Domain.Repositories.Users;

public interface IUserWriteRepository
{
    Task Register(User user);
}
