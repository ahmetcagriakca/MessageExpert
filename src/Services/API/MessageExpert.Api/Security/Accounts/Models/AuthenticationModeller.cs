using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageExpert.Api.Security.Accounts.Models
{
    public class GetTokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class GetTokenResponse
    {
        public string Value { get; set; }
    }

}
