using System;

namespace MessageExpert.Core.Logging.Models
{
    public class LogData
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
    }
}
