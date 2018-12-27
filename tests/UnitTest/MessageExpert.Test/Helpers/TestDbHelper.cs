using MessageExpert.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Test.Helpers
{
    public class TestDbHelper: ITestDbHelper
	{
		private IDbContextLocator Locator { get; }

		IDbContextLocator ITestDbHelper.Locator => Locator;

		public TestDbHelper()
		{
			var options = new DbContextOptionsBuilder<MessageExpertDbContext>()
					 .UseInMemoryDatabase(Guid.NewGuid().ToString())
					 .Options;
			MessageExpertDbContext context = new MessageExpertDbContext(options);
			Locator = new DbContextLocator(context);
		}
		
		public void SaveChanges()
		{
			Locator.Current.SaveChanges();
		}
	}
}
