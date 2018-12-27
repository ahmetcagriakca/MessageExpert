using MessageExpert.Core.Authentication.Models;
using MessageExpert.Core.Authentication.Services;
using MessageExpert.Core.Logging.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Text;

namespace MessageExpert.Core.Logging
{
    public class BaseLogger : ILogger
    {
        public string FileNameWithoutSuffix { get; private set; }
        public DirectoryInfo DirectoryInfo { get; private set; }
        private readonly IAuthenticationService authenticationService;
        private readonly IHostingEnvironment environment;

        public BaseLogger(IAuthenticationService authenticationService,
            IHostingEnvironment environment)
        {
            this.authenticationService = authenticationService;
            this.environment = environment;
            DirectoryInfo = CreateDirectory(Path.Combine(environment.ContentRootPath,"Logs"));
            FileNameWithoutSuffix = GetFileNameWithoutSuffix();
        }
        private string GetFileNameWithoutSuffix()
        {
            return Path.Combine(DirectoryInfo.FullName, "Log_{0}.txt");
        }
        private string GetFileName()
        {
            return string.Format(FileNameWithoutSuffix, DateTime.Now.ToString("yyyyMMddHH"));
        }

        private DirectoryInfo CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return Directory.CreateDirectory(path);
            }

            return new DirectoryInfo(path);
        }

        public void Debug(string debug)
        {
            Log(debug, LogLevel.Debug);
        }

        public void Error(Exception exception)
        {
            Log(exception.ToString(), LogLevel.Error);
        }

        public void Error(string error)
        {
            Log(error, LogLevel.Error);
        }

        public void Fatal(Exception exception)
        {
            Log(exception.ToString(), LogLevel.Fatal);
        }

        public void Info(string info)
        {
            Log(info, LogLevel.Info);
        }

        public void Warning(string warning)
        {
            Log(warning, LogLevel.Warn);
        }

        public void Log(string log, LogLevel logLevels)
        {
            var logData = Create(logLevels, log);
            var text = Format(logData).ToString();
            WriteToFile(text, GetFileName());
        }

        public void WriteToFile(string text, string path)
        {
            try
            {
                //    _readWriteLock.EnterWriteLock();
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                //  _readWriteLock.ExitWriteLock();
            }
        }

        public StringBuilder Format(LogData logData)
        {
            var builder = new StringBuilder();

            builder
                .Append(DateTime.Now.ToString("HH:mm:ss:fff"))
                .Append(" Level: ")
                .Append(logData.LogLevel)
                .AppendLine()
                .Append("User: ")
                .Append(logData.User)
                .AppendLine()
                .Append("Message: ")
                .Append(logData.Message)
                .AppendLine()
                ;
            return builder;
        }

        public LogData Create(LogLevel logLevels, string message)
        {
            var data = new LogData
            {
                LogLevel = logLevels,
                Timestamp = DateTime.UtcNow,
                Message = message
            };

            if (authenticationService.IsAuthenticated)
            {
                data.User = authenticationService.GetContext<IClientContext>().Key;
            }
            else
            {
                data.User = "None";
            }

            return data;
        }
    }
}
