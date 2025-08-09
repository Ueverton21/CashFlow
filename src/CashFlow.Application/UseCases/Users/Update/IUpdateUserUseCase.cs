using CashFlow.Communication.Requets;

namespace CashFlow.Application.UseCases.Users.Update
{
    public interface IUpdateUserUseCase
    {
        Task Execute(RequestUpdateUserJson request);
    }
}
