﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestsUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }
    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(userReadOnly => 
        userReadOnly.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userReadOnly =>
        userReadOnly.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }
    public IUserReadOnlyRepository Build()
    {
        return _repository.Object; 
    }
}
