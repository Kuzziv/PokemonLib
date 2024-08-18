using System;
using System.IO;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides a service for configuring and retrieving a logger instance used across the application.
    /// This service ensures that the logger is a singleton and is thread-safe.
    /// </summary>
    public static class LoggingService
    {
        /// <summary>
        /// Object used to synchronize access to the logger.
        /// </summary>
        private static readonly object _lock = new();

        /// <summary>
        /// The logger instance managed by this service.
        /// </summary>
        private static ILogger _logger;

        /// <summary>
        /// Indicates whether the logger has been configured.
        /// </summary>
        private static bool _isConfigured = false;

        /// <summary>
        /// Configures the logging service by setting up the logger instance with the specified file path.
        /// If no file path is provided, a default path on the desktop is used.
        /// </summary>
        /// <param name="logFilePath">The file path where logs will be written. If null, a default path is used.</param>
        /// <exception cref="InvalidOperationException">Thrown if the logger has already been configured.</exception>
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

        /// <summary>
        /// Retrieves the logger instance managed by this service.
        /// </summary>
        /// <returns>The logger instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the logger has not been configured.</exception>
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

        /// <summary>
        /// Resets the configuration of the logging service, allowing it to be reconfigured.
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
