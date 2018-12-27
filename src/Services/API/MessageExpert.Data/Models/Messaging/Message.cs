using System;
using System.Collections.Generic;
using System.Text;
using MessageExpert.Data.Models.Accounts;

namespace MessageExpert.Data.Models.Messaging
{
	public class Message : Entity<int>
    {
		public string Content { get; set; }
		public DateTime SendDateTime { get; set; }
		public User From { get; set; }
		public User To { get; set; }
	}
}
