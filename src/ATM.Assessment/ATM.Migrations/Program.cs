using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ATM.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        // Usually you'd parse the connection string from config or environment variables
        var dbLocation = Path.Combine(Path.GetTempPath(), "atm.db");
        var connectionString = $"Data Source={dbLocation}";

        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddSQLite()         // or .AddPostgres(), .AddSQLite(), etc.
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());
            })
            .Build();

        // Run the migrations
        var runner = host.Services.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();

        // Optionally: runner.MigrateDown(version); if you need rollback
    }
}