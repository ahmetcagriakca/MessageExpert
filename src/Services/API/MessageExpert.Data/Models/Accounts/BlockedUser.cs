using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data.Models.Accounts
{
	public class BlockedUser : Entity<int>
	{
		public User User { get; set; }
		public User Blocked { get; set; }
	}
}
