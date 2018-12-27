using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageExpert.Core
{
    /// <summary>
    /// <see cref="ResultBuilder"/>
    /// </summary>
    public interface IResultBuilder
    {
        IActionResult For(ModelStateDictionary dictionary);
        IActionResult For(Exception exception);
    }
}
