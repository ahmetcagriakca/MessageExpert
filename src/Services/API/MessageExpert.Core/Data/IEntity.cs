using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data
{
	public interface IEntity
	{
	}

	public class Entity<T> : IEntity
	{
		public T Id { get; set; }
	}

}
