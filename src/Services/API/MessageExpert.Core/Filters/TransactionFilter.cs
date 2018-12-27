using MessageExpert.Data;
using MessageExpert.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageExpert.Core.Filters
{
	public class TransactionFilter : IResultFilter
	{
		private readonly IDbContextLocator dbContextLocator;
		private readonly ILogger logger;

		public TransactionFilter(IDbContextLocator dbContextLocator,
			ILogger logger
			)
		{
			this.dbContextLocator = dbContextLocator;
			this.logger = logger;
		}

		/// <summary>
		/// Most cases catching on exception filter but some case like linq expression exceptions aren't handled on exception filter. For this cases checking "context.Exception" value and logging Fatal error.
		/// </summary>
		/// <param name="context"></param>
		public void OnResultExecuted(ResultExecutedContext context)
		{
			if (context.Exception != null)
			{
				logger.Fatal(context.Exception);
			}
			else
			{
				///TODO: Rollback operations can be added
				dbContextLocator.Current.SaveChanges();
			}
		}

		public void OnResultExecuting(ResultExecutingContext context)
		{

		}
	}
}
