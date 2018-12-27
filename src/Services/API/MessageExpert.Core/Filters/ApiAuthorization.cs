using MessageExpert.Core.Authentication.Models;
using MessageExpert.Core.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MessageExpert.Core.Filters
{
    public class ApiAuthorization : Attribute, IAuthorizationFilter
    {
        public string Permission { get; set; }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                var authenticationService = context.HttpContext.RequestServices.GetService<IAuthenticationService>();
                var user = authenticationService.GetContext<IClientContext>();
                if (user==null)
                    context.Result = new UnauthorizedResult();
            }
        }
    }
}
