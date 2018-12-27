using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Domain.Security.Crypto.Models
{

    public class HashString
    {
        public HashString(string hash)
        {
            this.Value = hash;
        }
        public string Value { get; }
    }
}
