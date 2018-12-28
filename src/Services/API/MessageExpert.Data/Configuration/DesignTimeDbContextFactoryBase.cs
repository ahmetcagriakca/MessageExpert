using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MessageExpert.Data.Configuration
{

    public abstract class DesignTimeDbContextFactoryBase<TContext> :
        IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }
        protected abstract TContext CreateNewInstance(
            DbContextOptions<TContext> options);

        public TContext Create()
        {
            var environmentName =
                System.Environment.GetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        private TContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            var section = configuration.GetSection("DatabaseConfig");
            var config = new DatabaseConfig();
            section.Bind(config);
            return Create(config);
        }

        private TContext Create(DatabaseConfig databaseConfig)
        {
            if (string.IsNullOrEmpty(databaseConfig.ConnectionString))
                throw new ArgumentException(
             $"{nameof(databaseConfig.Provider)} is null or empty.",
             nameof(databaseConfig.Provider));
            var optionsBuilder =
                 new DbContextOptionsBuilder<TContext>();
            Console.WriteLine("DesignTimeDbContextFactoryBase.Create(string): Connection string: {0}",
                databaseConfig.ConnectionString);

            //optionsBuilder.UseSqlServer(databaseConfig.ConnectionString);
            if (databaseConfig.Provider == "PostgreSql")
                optionsBuilder.UseNpgsql<TContext>(databaseConfig.ConnectionString);

            if (databaseConfig.Provider == "MSSql" || databaseConfig.Provider == "SqlExpress")
                optionsBuilder.UseSqlServer(databaseConfig.ConnectionString);
            DbContextOptions<TContext> options = optionsBuilder.Options;
            return CreateNewInstance(options);
        }
        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
             $"{nameof(connectionString)} is null or empty.",
             nameof(connectionString));

            var optionsBuilder =
                 new DbContextOptionsBuilder<TContext>();

            Console.WriteLine(
                "MyDesignTimeDbContextFactory.Create(string): Connection string: {0}",
                connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            DbContextOptions<TContext> options = optionsBuilder.Options;

            return CreateNewInstance(options);
        }
    }
}
