using CashFlow.Communication.Requets;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}