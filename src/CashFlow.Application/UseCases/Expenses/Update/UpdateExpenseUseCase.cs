﻿using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requets;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpenseUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpenseUseCase(IMapper mapper, IUnitOfWork unitOfWork, IExpenseUpdateOnlyRepository repository, ILoggedUser loggedUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetById(loggedUser,id);

        if (expense == null || expense.UserId != loggedUser.Id)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        _mapper.Map(request, expense);

        _repository.Update(id, expense);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorsMessages = result.Errors.Select(f => f.ErrorMessage).ToList();


            throw new ErrorOnValidationException(errorsMessages);
        }
    }
}
