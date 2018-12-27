using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Core.Logging
{
    /// <summary>
    /// <see cref="BaseLogger"/>
    /// </summary>
    public interface ILogger
    {
        void Debug(string debug);
        void Error(Exception exception);
        void Error(string error);
        void Fatal(Exception exception);
        void Info(string info);
        void Warning(string warning);
    }
}
