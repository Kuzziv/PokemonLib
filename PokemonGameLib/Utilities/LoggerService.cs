using System;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// A service for configuring and accessing a logger.
    /// This service ensures that the logger is configured only once and provides thread-safe access.
    /// </summary>
    public static class LoggingService
    {
        private static readonly object _lock = new();
        private static Logger _logger;
        private static bool _isConfigured = false;

        /// <summary>
        /// Configures the logger with a specified log file path and optional maximum file size.
        /// This method is thread-safe and ensures the logger is configured only once.
        /// </summary>
        /// <param name="logFilePath">The path to the log file.</param>
        /// <param name="maxFileSizeInMB">The maximum size of the log file before it rotates (in MB).</param>
        public static void Configure(string logFilePath, int maxFileSizeInMB = 10)
        {
            lock (_lock)
            {
                if (_isConfigured)
                {
                    throw new InvalidOperationException("Logger has already been configured. Reconfiguration is not allowed.");
                }

                _logger = new Logger(logFilePath, maxFileSizeInMB);
                _isConfigured = true;
            }
        }

        /// <summary>
        /// Gets the configured logger. Throws an exception if the logger has not been configured.
        /// This method is thread-safe.
        /// </summary>
        /// <returns>The configured logger instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the logger is not configured.</exception>
        public static Logger GetLogger()
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
    }
}
