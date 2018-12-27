using MessageExpert.Core;
using MessageExpert.Core.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageExpert.Core.Filters
{
    ///
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IResultBuilder resultBuilder;
        private readonly ILogger logger;

        public ApiExceptionFilter(
            IResultBuilder resultBuilder,
            ILogger logger
            )
        {
            this.resultBuilder = resultBuilder;
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            logger.Error(context.Exception);
            context.Result = resultBuilder.For(context.Exception);
        }
    }
}
