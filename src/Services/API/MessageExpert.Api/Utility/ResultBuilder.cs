using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageExpert.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MessageExpert.Api.Utility
{
	public class ResultBuilder : IResultBuilder
	{
		public IActionResult For(ModelStateDictionary modelState)
		{
			var errors = modelState.Keys.SelectMany(i => modelState[i].Errors).Select(m => m.ErrorMessage).ToArray();
			var obj = new { IsSuccess = false, Errors = errors };
			return GetResult(obj, StatusCodes.Status400BadRequest);
		}

		public IActionResult For(Exception exception)
		{
			var obj = new { IsSuccess = false, Error = exception.Message };
			return GetResult(obj, StatusCodes.Status400BadRequest);
		}


		private JsonResult GetResult(object obj, int statusCode = StatusCodes.Status400BadRequest)
		{
			return new JsonResult(obj)
			{
				StatusCode = statusCode
			};
		}
	}
}
