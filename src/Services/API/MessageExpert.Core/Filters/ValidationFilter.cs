using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace MessageExpert.Core.Filters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly IResultBuilder resultBuilder;

        public ValidationFilter(IResultBuilder resultBuilder)
        {
            this.resultBuilder = resultBuilder;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// ModelState and values checking
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Any(kv => kv.Value == null) || !context.ModelState.IsValid)
            {
                context.Result = resultBuilder.For(context.ModelState);
            }
        }
    }
}
