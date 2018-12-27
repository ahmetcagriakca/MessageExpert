using MessageExpert.Data;
using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using MessageExpert.Domain.Messaging.Services;
using MessageExpert.Domain.Security.Accounts.Services;
using MessageExpert.Domain.Security.Crypto.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Test.Helpers
{
	public class MessagingTestHelper
	{
		private readonly ITestDbHelper dbHelper;
		internal IMessageService MessageService;
		public MessagingTestHelper(ITestDbHelper dbHelper)
		{
			MessageService = new MessageService(new Repository<Message>(dbHelper.Locator), 
				new AccountService(
					new Repository<User>(dbHelper.Locator), 
					new Repository<BlockedUser>(dbHelper.Locator),
					new Repository<UserActivityLog>(dbHelper.Locator),
                    new CryptoService(),
					dbHelper.Locator
					));
            this.dbHelper = dbHelper;
		}

		public void CreateTestMessages()
		{
			MessageService.SendMessage("Merhaba.0-2", "test", "test2");
			MessageService.SendMessage("Selam.2-1", "test2", "test1");
			MessageService.SendMessage("Deneme.1-2", "test1", "test2");
			MessageService.SendMessage("Bu bir test mesajıdır lütfen cevap vermeyiniz.2-0", "test2", "test");
			MessageService.SendMessage("Bu bir test mesajıdır lütfen cevap vermeyiniz.2-1", "test2", "test");
			MessageService.SendMessage("Bu bir test mesajıdır lütfen cevap vermeyiniz.1-2", "test1", "test2");
			MessageService.SendMessage("Bu bir test mesajıdır lütfen cevap vermeyiniz.2-0", "test2", "test");

			dbHelper.SaveChanges();
		}
	}
}
