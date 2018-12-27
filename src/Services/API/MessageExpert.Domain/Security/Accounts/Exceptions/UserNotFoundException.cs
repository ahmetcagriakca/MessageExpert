using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Accounts.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException(string message) : base(message)
		{

		}
	}
}
