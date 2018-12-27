using MessageExpert.Api.Security.Accounts.Models;
using MessageExpert.Core.Authentication.Services;
using MessageExpert.Core.Controllers;
using MessageExpert.Domain.Messaging.Services;
using MessageExpert.Domain.Security.Accounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageExpert.Api.Security.Accounts.Controllers
{
	public class MessageController : BaseController
	{
		private readonly IMessageService messageService;
		private readonly IAuthenticationService authenticationService;

		public MessageController(IMessageService messageService,
			IAuthenticationService authenticationService)
		{
			this.messageService = messageService;
			this.authenticationService = authenticationService;
		}

		[HttpPost]
		public IActionResult SendMessage([FromBody]SendMessageRequest request)
		{
			var from = authenticationService.GetContext<ClientContext>().Key;
			messageService.SendMessage(request.Content, from, request.To);
			return Ok(new { IsSuccess = true });
		}
	}
}