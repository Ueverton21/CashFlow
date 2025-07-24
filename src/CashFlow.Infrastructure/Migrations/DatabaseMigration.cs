using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

namespace CashFlow.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static async Task MigrateDatabse(IServiceProvider service)
    {
        using var dbContext = service.GetRequiredService<CashFlowDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
