using MessageExpert.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MessageExpert.Data
{

	public class Repository<T> : IRepository<T> where T : class, IEntity, new()
	{
		protected readonly DbContext Context;
		public Repository(
			IDbContextLocator contextLocator
			)
		{
			if (contextLocator == null)
			{
				throw new ArgumentNullException(nameof(contextLocator));
			}

			Context = contextLocator.Current;
		}
		public IQueryable<T> Table
		{
			get
			{
				return Context.Set<T>();
			}
		}

		public T FindBy(object id)
		{
			return Context.Set<T>().Find(id);
		}
		public void Add(T entity)
		{
			Context.Set<T>().Add(entity);
		}
		public void Delete(T entity)
		{
			Context.Set<T>().Attach(entity);
			Context.Set<T>().Remove(entity);
		}

		public void Update(T entity)
		{
			Context.Set<T>().Attach(entity);
			Context.Entry(entity).State = EntityState.Modified;
		}

		public bool Any(Expression<Func<T, bool>> predicate)
		{
			return Table.Any(predicate);
		}
	}
}
