using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Accounts.Exceptions
{
	public class UserAlreadyExistsException : Exception
	{
		public UserAlreadyExistsException(string message) : base(message)
		{

		}
	}
}
