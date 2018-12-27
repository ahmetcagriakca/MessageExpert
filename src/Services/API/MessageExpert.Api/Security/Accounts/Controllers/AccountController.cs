using MessageExpert.Api.Security.Accounts.Models;
using MessageExpert.Core.Authentication.Services;
using MessageExpert.Core.Controllers;
using MessageExpert.Domain.Security.Accounts.Models;
using MessageExpert.Domain.Security.Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageExpert.Api.Security.Accounts.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IAccountService accountService;
		private readonly IAuthenticationService authenticationService;

		public AccountController(IAccountService accountService,
			IAuthenticationService authenticationService)
		{
			this.accountService = accountService;
			this.authenticationService = authenticationService;
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult CreateUser([FromBody]CreateUserRequest request)
		{
			var user = request.ToModel();
			accountService.Create(user);
			return Ok(new { IsSuccess = true });
		}

		/// <summary>
		/// If the user is logged in, they can retrieve their own information
		/// </summary>
		/// <returns>User Infos</returns>
		[HttpGet]
		public IActionResult GetUserInfo()
		{
			var user = accountService.GetUserById(authenticationService.GetContext<ClientContext>().UserId);
			var response = AccountModeller.ToGetUserResponse(user);
			return Ok(response);
		}

		[HttpGet("GetUserOutgoingMessages")]
		public IActionResult GetUserOutgoingMessages()
		{
			var userId = authenticationService.GetContext<ClientContext>().UserId;
			var messages = accountService.GetUserOutgoingMessages(userId);
			var response = AccountModeller.ToGetUserMessagesResponse(messages);
			return Ok(response);
		}

		[HttpGet("GetUserIncomingMessages")]
		public IActionResult GetUserIncomingMessages()
		{
			var userId = authenticationService.GetContext<ClientContext>().UserId;
			var messages = accountService.GetUserIncomingMessages(userId);
			var response = AccountModeller.ToGetUserMessagesResponse(messages);
			return Ok(response);
		}

		[HttpPost("BlockUser/{blockedUserId}")]
		public IActionResult BlockUser(int blockedUserId)
		{
			var userId = authenticationService.GetContext<ClientContext>().UserId;
			accountService.BlockUser(userId, blockedUserId);
			return Ok(new { IsSuccess = true });
		}

		[HttpGet("GetUserActivityLogs/{id}")]
		public IActionResult GetUserActivityLogs(int id)
		{
			var userId = authenticationService.GetContext<ClientContext>().UserId;
			var activityLogs = accountService.GetUserActivityLogs(userId);
			var response = AccountModeller.ToGetUserActivityLogsResponse(activityLogs);
			return Ok(response);
		}
	}
}