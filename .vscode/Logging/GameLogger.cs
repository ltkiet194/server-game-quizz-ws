using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.Logging
{
    public class GameLogger : IGameLogger
    {   
        private readonly ILogger _logger;

        public GameLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logging/log-.txt",LogEventLevel.Error,rollingInterval:RollingInterval.Day)
                .CreateLogger();
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Info(string message)
        {
            _logger.Information($">>>> {message}");

        }

        public void Print(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message, Exception exception)
        {
            _logger.Warning(message, exception);
        }
    }
}
