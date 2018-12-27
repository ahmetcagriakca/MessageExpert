using MessageExpert.Data.Models.Accounts;
using MessageExpert.Domain.Security.Accounts.Exceptions;
using MessageExpert.Domain.Security.Accounts.Models;
using MessageExpert.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MessageExpert.Test
{
	[TestClass]
	public class AccountTests
	{
		AccountTestHelper accountTestHelper;
		ITestDbHelper dbHelper;
		public AccountTests()
		{
			dbHelper = new TestDbHelper();
		}

		[TestInitialize]
		public void AccountTestInitialize()
		{
			accountTestHelper = new AccountTestHelper(dbHelper);
		}

		[TestMethod]
		public void User_Create()
		{
            User user = new User
            {
                UserName = "aca",
                Password = "123456"
            };
            accountTestHelper.AccountService.Create(user);
			dbHelper.SaveChanges();

			var newUser = accountTestHelper.AccountService.GetUserByName(user.UserName);
			Assert.IsNotNull(newUser);
		}

		[TestMethod]
		public void User_Already_Exist_Create()
		{
			{
                User user = new User
                {
                    UserName = "aca",
                    Password = "123456"
                };
                accountTestHelper.AccountService.Create(user);
				dbHelper.SaveChanges();
			}
			{
                User user = new User
                {
                    UserName = "aca",
                    Password = "123456"
                };

                Assert.ThrowsException<UserAlreadyExistsException>(() => { accountTestHelper.AccountService.Create(user); });
			}
		}

		[TestMethod]
		public void Login_User()
		{
			accountTestHelper.CreateTestUsers();
			var userName = "test";
			var password = "123456";
            if(accountTestHelper.AccountService.TryGetUserContext(userName,password, out ClientContext context))
            {

            }
            Assert.AreEqual("test", context.Key);
            //var user = accountTestHelper.AccountService.GetUser(userName, password);
        }

        [TestMethod]
		public void Invalid_Login_User()
		{
			accountTestHelper.CreateTestUsers();
			var userName = "test1";
			var password = "1234567";
			Assert.ThrowsException<PasswordMismatchException>(() => {

                if (accountTestHelper.AccountService.TryGetUserContext(userName, password, out ClientContext context))
                {

                }
                //accountTestHelper.AccountService.GetUser(userName, password);
            });
		}

		[TestMethod]
		public void Block_User()
		{
			accountTestHelper.CreateTestUsers();
			var userName = "test1";
			var blockedUserName = "test2";
			var user = accountTestHelper.AccountService.GetUserByName(userName);
			var blockedUser = accountTestHelper.AccountService.GetUserByName(blockedUserName);
			accountTestHelper.AccountService.BlockUser(user.Id, blockedUser.Id);
			dbHelper.SaveChanges();

			Assert.IsTrue(accountTestHelper.AccountService.IsUserBlocked(blockedUser, user));
		}

		[TestMethod]
		public void Check_User_Activity_Log()
		{
			accountTestHelper.CreateTestUsers();
			dbHelper.SaveChanges();
			var userName = "test";
			{
				var password = "123456";
				var user = accountTestHelper.AccountService.GetUser(userName, password);
				dbHelper.SaveChanges();
			}
			{
				var password = "1234567";
				Assert.ThrowsException<PasswordMismatchException>(() => { accountTestHelper.AccountService.GetUser(userName, password); });
				dbHelper.SaveChanges();
			}

			var userActivityLogs = accountTestHelper.AccountService.GetUserActivityLogs(accountTestHelper.AccountService.GetUserByName(userName).Id);
			Assert.IsTrue(userActivityLogs?.Count() > 0);
		}
	}
}
