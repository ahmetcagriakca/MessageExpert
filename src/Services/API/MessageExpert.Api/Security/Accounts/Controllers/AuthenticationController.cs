using MessageExpert.Api.Security.Accounts.Models;
using MessageExpert.Core.Authentication.Models;
using MessageExpert.Core.Authentication.Services;
using MessageExpert.Core.Controllers;
using MessageExpert.Core.Logging;
using MessageExpert.Domain.Security.Accounts.Models;
using MessageExpert.Domain.Security.Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MessageExpert.Api.Security.Accounts.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IAccountService accountService;
        private readonly IAuthenticationService authenticationService;
        private readonly ILogger logger;
        private readonly JwtConfig _settings;

        public AuthenticationController(IAccountService accountService,
            IAuthenticationService authenticationService,
            ILogger logger,
            JwtConfig settings
            )
        {
            this.accountService = accountService;
            this.authenticationService = authenticationService;
            this.logger = logger;
            _settings = settings;
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

        [HttpGet("settings")]
        [AllowAnonymous]
        public IActionResult GetSettings()
        {
            return Json(_settings);
        }
    }
}