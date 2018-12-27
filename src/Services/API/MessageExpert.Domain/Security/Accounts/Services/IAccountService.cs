using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using MessageExpert.Domain.Security.Accounts.Models;
using System.Collections.Generic;

namespace MessageExpert.Domain.Security.Accounts.Services
{
    /// <summary>
    /// <see cref="AccountService"/>
    /// </summary>
    public interface IAccountService
    {
        void Create(User user);
        User GetUserByName(string userName);
        User GetUser(string userName, string password);
        void BlockUser(int userId, int blockUserId);
        bool IsUserBlocked(User user, User blockedUser);
        void AddActivity(UserActivityLog activityLog);
        IEnumerable<UserActivityLog> GetUserActivityLogs(int id);
        bool TryGetUserContext(string username, string password, out ClientContext context);
        User GetUserById(int id);
		IEnumerable<Message> GetUserOutgoingMessages(int id);
		IEnumerable<Message> GetUserIncomingMessages(int id);
	}
}
