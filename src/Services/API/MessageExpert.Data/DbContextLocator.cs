using MessageExpert.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data
{
	public class DbContextLocator : IDbContextLocator
	{
		private readonly MessageExpertDbContext _dbContext;
		public DbContextLocator(MessageExpertDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		DbContext IDbContextLocator.Current
		{
			get
			{
				return _dbContext;
			}
		}
	}
}
