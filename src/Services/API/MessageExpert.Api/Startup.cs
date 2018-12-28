using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageExpert.Core.Filters;
using MessageExpert.Api.Utility;
using MessageExpert.Core.Authentication.Extensions;
using MessageExpert.Core.Logging;
using MessageExpert.Data;
using MessageExpert.Domain.Messaging.Services;
using MessageExpert.Domain.Security.Accounts.Services;
using MessageExpert.Domain.Security.Crypto.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using MessageExpert.Core;
using MessageExpert.Data.Extensions;

namespace MessageExpert.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSwaggerGen(options => { options.SwaggerDoc("v1", new Info { Title = "Message Manager API", Version = "v1" }); });
			//services.AddDbContext<MessageExpertDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
		    services.UseEntityFramework<MessageExpertDbContext>(Configuration.GetDatabaseConfig("DatabaseConfig"));

            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(ApiExceptionFilter), int.MinValue);
                x.Filters.Add(typeof(ApiAuthorization));
                x.Filters.Add(typeof(ValidationFilter), 3);
                x.Filters.Add(typeof(TransactionFilter), int.MaxValue);
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
				.AddJsonOptions(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.TryAddScoped<IDbContextLocator, DbContextLocator>();
			services.TryAddScoped(typeof(IRepository<>),typeof(Repository<>) );
			services.TryAddScoped<ICryptoService, CryptoService>();
			services.TryAddScoped<IAccountService, AccountService>();
			services.TryAddScoped<IMessageService, MessageService>();
			services.TryAddScoped<IResultBuilder, ResultBuilder>();
			services.TryAddScoped<ILogger, BaseLogger>();
            
            services.UseJwtAuthentication(Configuration.GetJwtConfig("JwtConfig"));

        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Message Manager API V1");
				c.RoutePrefix = string.Empty;
			});
			app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id:int}");
            });
        }
    }
}
