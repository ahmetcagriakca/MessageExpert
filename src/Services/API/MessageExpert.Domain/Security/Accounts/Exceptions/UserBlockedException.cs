using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Accounts.Exceptions
{
	public class UserBlockedException : Exception
	{
		public UserBlockedException(string message) : base(message)
		{

		}
	}
}
