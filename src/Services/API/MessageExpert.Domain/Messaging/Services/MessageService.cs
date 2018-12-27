using System;
using MessageExpert.Data;
using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using MessageExpert.Domain.Security.Accounts.Exceptions;
using MessageExpert.Domain.Security.Accounts.Services;

namespace MessageExpert.Domain.Messaging.Services
{
    public class MessageService : IMessageService
	{
		private readonly IRepository<Message> messageRepository;
		private readonly IAccountService accountService;

		public MessageService(IRepository<Message> messageRepository,
			IAccountService accountService)
		{
			this.messageRepository = messageRepository;
			this.accountService = accountService;
		}

		/// <summary>
		/// Message sending operation
		/// Checking users and user blocked status
		/// </summary>
		/// <param name="content">Message content</param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void SendMessage(string content, string from, string to)
		{
			var fromUser = accountService.GetUserByName(from);
			if (fromUser == null)
			{
				throw new UserNotFoundException("Sender user not found");
			}

			var toUser = accountService.GetUserByName(to);
			if (toUser == null)
			{
				throw new UserNotFoundException($"User {to} not found");
			}

			if (accountService.IsUserBlocked(fromUser, toUser))
			{
				throw new UserBlockedException($"User is blocked by {to}. Message can't send.");
			}
			SendMessage(content, fromUser, toUser);
		}

		private void SendMessage(string content, User fromUser, User toUser)
		{
            var message = new Message
            {
                Content = content,
                From = fromUser,
                To = toUser,
                SendDateTime = DateTime.Now
            };
            messageRepository.Add(message);
		}
	}
}
