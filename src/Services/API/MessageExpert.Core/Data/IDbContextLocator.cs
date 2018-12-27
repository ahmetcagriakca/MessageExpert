using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data
{
    /// <summary>
    /// <see cref="DbContextLocator"/>
    /// </summary>
    public interface IDbContextLocator
	{
		DbContext Current { get; }
	}
}
