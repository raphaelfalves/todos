using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ToDosProject.Infraestructure.Context;

namespace ToDosProject.ApiService.Extensions;

public static class ApiDbInitializer
{
    public static void InitializeDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        EnsureDatabase(dbContext);
        RunMigration(dbContext);
    }

    private static void EnsureDatabase(AppDbContext dbContext)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            // Create the database if it does not exist.
            if (!dbCreator.Exists())
            {
                dbCreator.Create();
            }
        });
    }

    private static void RunMigration(AppDbContext dbContext)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            using var transaction = dbContext.Database.BeginTransaction();
            dbContext.Database.Migrate();
            transaction.Commit();
        });
    }
}
