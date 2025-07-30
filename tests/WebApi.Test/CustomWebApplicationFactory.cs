using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommomTestsUtilities.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!;
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var tokenGenerator = scope.ServiceProvider.GetService<IAccessTokenGenerator>();

                StartDatabase(context,passwordEncripter, tokenGenerator);
            });
    }
    private void StartDatabase(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(context, passwordEncripter,tokenGenerator);
        Expense_MemberTeam = new ExpenseIdentityManager(AddExpenses(context, userTeamMember,expenseId: 1));

        var userAdmin = AddUserAdmin(context, passwordEncripter, tokenGenerator);
        Expense_Admin = new ExpenseIdentityManager(AddExpenses(context, userAdmin, expenseId: 2));

        context.SaveChanges();
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncrypter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        
        user.Password = passwordEncrypter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }
    private User AddUserAdmin(CashFlowDbContext dbContext, IPasswordEncripter passwordEncrypter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(role: Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        
        user.Password = passwordEncrypter.Encrypt(user.Password);
        user.Role = Roles.ADMIN;

        dbContext.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        dbContext.Expenses.Add(expense);

        return expense;
    }

}
