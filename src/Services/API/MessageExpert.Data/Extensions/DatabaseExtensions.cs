using MessageExpert.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static DatabaseConfig GetDatabaseConfig(this IConfiguration configuration, string name)
        {
            var section = configuration.GetSection(name);
            var config = new DatabaseConfig();
            section.Bind(config);
            if (configuration["DATABASE_PROVIDER"] != null)
            {
                config.Provider = configuration["DATABASE_PROVIDER"];
            }
            if (configuration["DATABASE_CONNECTION_STRING"] != null)
            {
                config.ConnectionString = configuration["DATABASE_CONNECTION_STRING"];
            }
            return config;
        }

        public static IServiceCollection UseEntityFramework<T>(this IServiceCollection services, DatabaseConfig configuration) where T : DbContext
        {
            if (configuration.Provider.ToUpper() == "postgre".ToUpper())
                return services.AddEntityFrameworkNpgsql().AddDbContext<MessageExpertDbContext>(options => options.UseNpgsql(configuration.ConnectionString));

            if (configuration.Provider.ToUpper() == "mssql".ToUpper())
                return services.AddDbContext<MessageExpertDbContext>(options => options.UseSqlServer(configuration.ConnectionString));

            throw new Exception("InvalidCastException provider name");
        }
    }

}
