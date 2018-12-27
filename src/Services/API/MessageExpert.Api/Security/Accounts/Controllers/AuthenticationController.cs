using MessageExpert.Api.Security.Accounts.Models;
using MessageExpert.Core.Authentication.Services;
using MessageExpert.Core.Controllers;
using MessageExpert.Core.Logging;
using MessageExpert.Domain.Security.Accounts.Models;
using MessageExpert.Domain.Security.Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageExpert.Api.Security.Accounts.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IAccountService accountService;
        private readonly IAuthenticationService authenticationService;
        private readonly ILogger logger;

        public AuthenticationController(IAccountService accountService,
            IAuthenticationService authenticationService,
            ILogger logger)
        {
            this.accountService = accountService;
            this.authenticationService = authenticationService;
            this.logger = logger;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult Token([FromBody]GetTokenRequest request)
        {
            if (accountService.TryGetUserContext(request.Username, request.Password, out ClientContext context))
            {
                var token = authenticationService.CreateIdentity(context);
                logger.Info($"Client ({context.Key}) is authenticated.");
				return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}