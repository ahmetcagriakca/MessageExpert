using MessageExpert.Data.Models.Messaging;
using System.Collections.Generic;

namespace MessageExpert.Data.Models.Accounts
{
	public class User : Entity<int>
	{
		public User()
		{
			OutgoingMessages = new HashSet<Message>();
			IncomingMessages = new HashSet<Message>();
			BlockedUsers = new HashSet<BlockedUser>();
			Logs = new HashSet<UserActivityLog>();
		}
		public string UserName { get; set; }
		public string Password { get; set; }
		public virtual ICollection<Message> OutgoingMessages { get; set; }
		public virtual ICollection<Message> IncomingMessages { get; set; }
		public virtual ICollection<BlockedUser> BlockedUsers { get; set; }
		public virtual ICollection<UserActivityLog> Logs { get; set; }

	}
}