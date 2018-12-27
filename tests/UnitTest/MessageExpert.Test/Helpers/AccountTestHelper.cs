using MessageExpert.Data;
using MessageExpert.Data.Models.Accounts;
using MessageExpert.Domain.Security.Accounts.Services;
using MessageExpert.Domain.Security.Crypto.Services;

namespace MessageExpert.Test.Helpers
{
    public class AccountTestHelper
    {
        internal IAccountService AccountService;
        internal readonly ITestDbHelper dbHelper;

        public AccountTestHelper(ITestDbHelper dbHelper)
        {
            AccountService = new AccountService(new Repository<User>(dbHelper.Locator),
                new Repository<BlockedUser>(dbHelper.Locator),
                new Repository<UserActivityLog>(dbHelper.Locator),
                new CryptoService(),
				dbHelper.Locator
				);
            this.dbHelper = dbHelper;
        }

        public void CreateTestUsers()
        {
            {
                User user = new User();
                user.UserName = "test";
                user.Password = "123456";
                AccountService.Create(user);
            }
            {
                User user = new User();
                user.UserName = "test1";
                user.Password = "123456";
                AccountService.Create(user);
            }
            {
                User user = new User();
                user.UserName = "test2";
                user.Password = "123456";
                AccountService.Create(user);
            }
            dbHelper.SaveChanges();
        }


    }
}
