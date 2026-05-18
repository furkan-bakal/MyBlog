using Serilog;

namespace Service.Logger
{
    public class SerilogAppLogger<T> : IAppLogger<T>
    {
        private readonly Serilog.ILogger _logger;

        public SerilogAppLogger()
        {
            _logger = Log.ForContext<T>();
        }

        public void Information(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _logger.Warning(message, args);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }
    }
}
