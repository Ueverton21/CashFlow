using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security.Tokens;
using CashFlow.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();

        AddRepositories(services);
        AddToken(services,configuration);

        if (configuration.IsTestEnviroment() is false)
        {
            AddDbContext(services, configuration);
        }
        
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(options => 
            new JwtTokenGenerator(expirationTimeMinutes,signingKey!));
    }
    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpenseWriteOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpenseReadOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpenseUpdateOnlyRepository, ExpensesRepository>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connnection = configuration.GetConnectionString("Connection");
        var serverVersion = ServerVersion.AutoDetect(connnection);

        services.AddDbContext<CashFlowDbContext>(config => 
            config.UseMySql(connnection, serverVersion));
    }
}
