using MessageExpert.Core.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Accounts.Models
{

    public class ClientContext : IClientContext
    {
        public string Key { get; set; }
        public int UserId { get; set; }
    }
}
