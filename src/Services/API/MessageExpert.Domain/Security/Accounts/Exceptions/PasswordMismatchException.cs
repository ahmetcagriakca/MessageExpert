using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Accounts.Exceptions
{
	public class PasswordMismatchException : Exception
	{
		public PasswordMismatchException(string message) : base(message)
		{

		}
	}
}
