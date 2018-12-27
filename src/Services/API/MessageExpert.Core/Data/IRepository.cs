using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MessageExpert.Data
{
    /// <summary>
    /// <see cref="Repository{T}"/>
    /// </summary>
    public interface IRepository<T> where T : IEntity, new()
	{
		IQueryable<T> Table { get; }

		T FindBy(object id);
		void Add(T entity);
		void Delete(T entity);
		void Update(T entity);
		bool Any(Expression<Func<T, bool>> predicate);
	}
}
