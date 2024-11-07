using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetById;
public interface IGetByIdUseCase
{
    public Task<ResponseExpenseJson> Execute(long id);
}
