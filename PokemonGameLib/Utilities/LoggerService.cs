using System;
using System.IO;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Utilities
{
    public static class LoggingService
    {
        private static readonly object _lock = new();
        private static ILogger _logger;
        private static bool _isConfigured = false;

        public static void Configure(string logFilePath = null)
        {
            lock (_lock)
            {
                if (_isConfigured)
                {
                    throw new InvalidOperationException("Logger has already been configured. Reconfiguration is not allowed.");
                }

                if (string.IsNullOrEmpty(logFilePath))
                {
                    var logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "tmp_logs");
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    logFilePath = Path.Combine(logDirectory, "Logs.yml");
                }

                _logger = new Logger(logFilePath);
                _isConfigured = true;
            }
        }

        public static ILogger GetLogger()
        {
            lock (_lock)
            {
                if (!_isConfigured)
                {
                    throw new InvalidOperationException("Logger has not been configured. Call LoggingService.Configure first.");
                }

                return _logger;
            }
        }

        public static void ResetConfiguration()
        {
            lock (_lock)
            {
                _logger = null;
                _isConfigured = false;
            }
        }
    }
}
