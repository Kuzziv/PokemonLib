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

        /// <summary>
        /// Configures the logger with a specified log file path.
        /// This method is thread-safe and ensures the logger is configured only once.
        /// </summary>
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

                _logger = new Logger(logFilePath); // ILogger is assigned with Logger instance
                _isConfigured = true;
            }
        }

        /// <summary>
        /// Gets the configured logger. Throws an exception if the logger has not been configured.
        /// This method is thread-safe.
        /// </summary>
        /// <returns>The configured logger instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the logger is not configured.</exception>
        public static ILogger GetLogger()  // Return ILogger instead of Logger
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

        /// <summary>
        /// Resets the logger configuration, allowing the logger to be reconfigured.
        /// This method is thread-safe.
        /// </summary>
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
