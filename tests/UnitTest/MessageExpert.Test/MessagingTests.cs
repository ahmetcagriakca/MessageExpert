using MessageExpert.Data;
using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using MessageExpert.Domain.Messaging.Services;
using MessageExpert.Domain.Security.Accounts.Exceptions;
using MessageExpert.Domain.Security.Accounts.Services;
using MessageExpert.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageExpert.Test
{
	[TestClass]
	public class MessagingTests
	{
		MessagingTestHelper messagingTestHelper;
		AccountTestHelper accountTestHelper;
		ITestDbHelper dbHelper;
		public MessagingTests()
		{
			dbHelper = new TestDbHelper();

		}

		[TestInitialize]
		public void AccountTestInitialize()
		{
			messagingTestHelper = new MessagingTestHelper(dbHelper);
			accountTestHelper = new AccountTestHelper(dbHelper);
		}

		[TestMethod]
		public void Send_Message()
		{
			accountTestHelper.CreateTestUsers();
			messagingTestHelper.MessageService.SendMessage("Merhaba", "test", "test1");
			dbHelper.SaveChanges();
			var userOutgoingMessages = accountTestHelper.AccountService.GetUserOutgoingMessages(accountTestHelper.AccountService.GetUserByName("test").Id);
			Assert.IsTrue(userOutgoingMessages?.Count() > 0);
		}

		[TestMethod]
		public void Send_Message_To_Incorrect_User()
		{
			accountTestHelper.CreateTestUsers();
			Assert.ThrowsException<UserNotFoundException>(() => { messagingTestHelper.MessageService.SendMessage("Merhaba", "test", "test3"); });
		}

		[TestMethod]
		public void Get_User_Incoming_Messages()
		{
			accountTestHelper.CreateTestUsers();
			messagingTestHelper.CreateTestMessages();
			var userIncomingMessages = accountTestHelper.AccountService.GetUserIncomingMessages(accountTestHelper.AccountService.GetUserByName("test2").Id);
			Assert.IsTrue(userIncomingMessages?.Count() > 0);
		}

		[TestMethod]
		public void Send_Message_To_Blocked_User()
		{
			accountTestHelper.CreateTestUsers();
			messagingTestHelper.CreateTestMessages();

			var userName = "test1";
			var blockedUserName = "test2";
			var user = accountTestHelper.AccountService.GetUserByName(userName);
			var blockedUser = accountTestHelper.AccountService.GetUserByName(blockedUserName);
			accountTestHelper.AccountService.BlockUser(user.Id, blockedUser.Id);
			dbHelper.SaveChanges();

			Assert.ThrowsException<UserBlockedException>(() => { messagingTestHelper.MessageService.SendMessage("Merhaba", "test2", "test1"); });
			
		}
	}
}
