﻿using CashFlow.Domain.Repositories;
using Moq;

namespace CommomTestsUtilities.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}
