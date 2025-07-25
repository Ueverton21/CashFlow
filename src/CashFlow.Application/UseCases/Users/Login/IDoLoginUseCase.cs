﻿using CashFlow.Communication.Requets;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
