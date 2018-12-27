using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Core.Authentication.Models
{

    public class JwtConfig 
    {
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinute { get; set; }
    }
}
