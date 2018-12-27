using MessageExpert.Data;
using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using MessageExpert.Domain.Security.Accounts.Exceptions;
using MessageExpert.Domain.Security.Accounts.Models;
using MessageExpert.Domain.Security.Crypto;
using MessageExpert.Domain.Security.Crypto.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageExpert.Domain.Security.Accounts.Services
{
	public class AccountService : IAccountService
	{
		private readonly IRepository<User> userRepository;
		private readonly IRepository<BlockedUser> blockedUserRepository;
		private readonly IRepository<UserActivityLog> activityLogRepository;
		private readonly ICryptoService cryptoService;
		private readonly IDbContextLocator dbContextLocator;

		public AccountService(
			IRepository<User> userRepository,
			IRepository<BlockedUser> blockedUserRepository,
			IRepository<UserActivityLog> activityLogRepository,
			ICryptoService cryptoService,
			IDbContextLocator dbContextLocator
			)
		{
			this.userRepository = userRepository;
			this.blockedUserRepository = blockedUserRepository;
			this.activityLogRepository = activityLogRepository;
			this.cryptoService = cryptoService;
			this.dbContextLocator = dbContextLocator;
		}

		/// <summary>
		/// Creating user and checking UserName 
		/// </summary>
		/// <param name="user"></param>
		public void Create(User user)
		{
			if (IsUserExist(user.UserName))
			{
				throw new UserAlreadyExistsException("This username is already used from another user.");
			}
			user.Password = cryptoService.Encrypt(user.Password).Value;
			userRepository.Add(user);
		}

		/// <summary>
		/// Checking UserName is exist
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public bool IsUserExist(string userName)
		{
			return userRepository.Any(en => en.UserName == userName);
		}

		/// <summary>
		/// Getting user by name
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public User GetUserById(int id)
		{
			var user = userRepository.Table.FirstOrDefault(en => en.Id == id);
			return user;
		}

		/// <summary>
		/// Getting user by name
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public User GetUserByName(string userName)
		{
			var user = userRepository.Table.FirstOrDefault(en => en.UserName == userName);
			return user;
		}

		/// <summary>
		/// Getting user info for login informations
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public User GetUser(string userName, string password)
		{
			var user = userRepository.Table.FirstOrDefault(en => en.UserName == userName);
			if (user == null)
			{
				throw new UserNotFoundException("User name or password are wrong");
			}
			if (user.Password != cryptoService.Encrypt(password).Value)
			{
				AddActivity(new UserActivityLog()
				{
					User = user,
					LoginIsSuccess = false,
					LogDateTime = DateTime.Now
				});
				dbContextLocator.Current.SaveChanges();
				throw new PasswordMismatchException("User name or password are wrong");
			}
			AddActivity(new UserActivityLog()
			{
				User = user,
				LoginIsSuccess = true,
				LogDateTime = DateTime.Now
			});
			return user;
		}

		/// <summary>
		/// Checking sender user blocked from receiver user
		/// </summary>
		/// <param name="from">User trying to send message</param>
		/// <param name="to">Receiver user</param>
		/// <returns></returns>
		public bool IsUserBlocked(User from, User to)
		{
			return blockedUserRepository.Table.Any(en => en.User == to && en.Blocked == from);
		}

		private bool IsUserAlreadyBlocked(User user, User blockedUser)
		{
			return blockedUserRepository.Table.Any(en => en.User == user && en.Blocked == blockedUser);
		}

		/// <summary>
		/// User block to other user
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="blockedUserId"></param>
		public void BlockUser(int userId, int blockedUserId)
		{
            var blockedUser = new BlockedUser
            {
                User = userRepository.FindBy(userId),
                Blocked = userRepository.FindBy(blockedUserId)
            };
            if (IsUserAlreadyBlocked(blockedUser.User, blockedUser.Blocked))
			{
				throw new UserBlockedException($"User({blockedUser.Blocked.UserName}) already blocked.");
			}
			blockedUserRepository.Add(blockedUser);
		}


		public void AddActivity(UserActivityLog activityLog)
		{
			activityLogRepository.Add(activityLog);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public IEnumerable<UserActivityLog> GetUserActivityLogs(int id)
		{
			return userRepository.Table
					.Include(en => en.Logs)
					.FirstOrDefault(en => en.Id == id).Logs.ToList();
		}

		public bool TryGetUserContext(string username, string password, out ClientContext context)
		{
			context = null;
			var user = GetUser(username, password);

			context = new ClientContext
			{
				Key = user.UserName,
				UserId = user.Id
			};
			return true;
		}

		public IEnumerable<Message> GetUserIncomingMessages(int id)
		{
			var user = userRepository.Table
				.Include(en => en.IncomingMessages)
				.ThenInclude(x => x.From)
				.Include(en => en.IncomingMessages)
				.ThenInclude(x => x.To)
				.FirstOrDefault(en => en.Id == id);
			if (user == null)
			{
				throw new UserNotFoundException("User not found.");
			}
			return user.IncomingMessages.ToList();
		}

		public IEnumerable<Message> GetUserOutgoingMessages(int id)
		{
			var user = userRepository.Table
				.Include(en => en.OutgoingMessages)
				.ThenInclude(en => en.From)
				.Include(en => en.OutgoingMessages)
				.ThenInclude(en => en.To)
				.FirstOrDefault(en => en.Id == id);
			if (user == null)
			{
				throw new UserNotFoundException("User not found.");
			}
			return user.OutgoingMessages.ToList();
		}
	}
}
