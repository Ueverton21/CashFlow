using CashFlow.Communication.Requets;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Users.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _encripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter encripter,
        IAccessTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _encripter = encripter;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetUserByEmail(request.Email);

        if (user == null) {
            throw new InvalidLoginException();
        }

        if(!_encripter.VerifyPassword(request.Password, user.Password))
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }
}
