using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Core.Authentication.Exceptions
{
    public class AuthenticationTokenNotFoundException : Exception
    {
        public AuthenticationTokenNotFoundException(string message) : base(message)
        {
        }
    }
}
