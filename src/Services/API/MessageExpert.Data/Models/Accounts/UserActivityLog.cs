using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data.Models.Accounts
{
	public class UserActivityLog : Entity<int>
	{
		public User User { get; set; }
		public bool LoginIsSuccess { get; set; }
		public DateTime LogDateTime { get; set; }
	}
}
