using MessageExpert.Core.Authentication.Models;
using MessageExpert.Core.Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageExpert.Core.Authentication.Extensions
{
    public static class AuthenticationExtensions
    {
        public static JwtConfig GetJwtConfig(this IConfiguration configuration, string name)
        {
            var section = configuration.GetSection(name);
            var config = new JwtConfig();
            section.Bind(config);
            return config;
        }

        public static IServiceCollection UseJwtAuthentication(this IServiceCollection services, JwtConfig config)
        {
            return services
                .AddSingleton(typeof(JwtConfig), config)
                .AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));
        }
    }
}
