using System.IO;
using System.Linq;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace OzonEdu.MerchandiseApi.Migrator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration
                .GetSection("DatabaseConnectionOptions:ConnectionString")
                .Get<string>();

            var services = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Program).Assembly)
                    .For
                    .Migrations())
                .AddLogging(builder => builder.AddFluentMigratorConsole());

            var serviceProvider = services.BuildServiceProvider(false);

            using (serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                if (args.Contains("dryrun"))
                    runner.ListMigrations();
                else
                    runner.MigrateUp();
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                connection.ReloadTypes();
            }
        }
    }
}