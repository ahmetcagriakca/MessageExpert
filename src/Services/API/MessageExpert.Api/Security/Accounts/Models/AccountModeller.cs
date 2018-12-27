using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace MessageExpert.Api.Security.Accounts.Models
{
	public class CreateUserRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
	
	public static class AccountModeller
	{
		public static User ToModel(this CreateUserRequest request)
		{
            var model = new User
            {
                UserName = request.UserName,
                Password = request.Password
            };
            return model;
		}

		public static object ToGetUserResponse(User user)
		{
			return new
			{
				IsSuccess = true,
				ResultObject = new
				{
					UserId = user.Id,
					username = user.UserName
				}
			};
		}

		public static object ToGetUserActivityLogsResponse(IEnumerable<UserActivityLog>  userActivityLogs)
		{
			return new
			{
				IsSuccess = true,
				ResultObject = userActivityLogs.Select(log =>
				new
				{
					log.Id,
					log.LogDateTime,
					log.LoginIsSuccess
				})
			};
		}

		public static object ToGetUserMessagesResponse(IEnumerable<Message> userMessages)
		{
			var result = userMessages.Select(message =>
				  new
				  {
					  message.Id,
					  message.Content,
					  message.SendDateTime,
					  From = new
					  {
						  message.From.Id,
						  message.From.UserName,
					  },
					  To = new
					  {
						  message.To.Id,
						  message.To.UserName,
					  }
				  });
			return new
			{
				IsSuccess = true,
				ResultObject = result
			};
		}
		
	}
}
