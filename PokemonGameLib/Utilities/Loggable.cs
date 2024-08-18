using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides a base class for logging messages within the Pokémon game library. 
    /// Classes that inherit from <see cref="Loggable"/> can easily log informational, warning, and error messages.
    /// </summary>
    public abstract class Loggable
    {
        /// <summary>
        /// The logger instance used to log messages.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Loggable"/> class and retrieves the logger instance.
        /// </summary>
        protected Loggable()
        {
            _logger = LoggingService.GetLogger();
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected void LogInfo(string message)
        {
            _logger.LogInfo(message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected void LogError(string message)
        {
            _logger.LogError(message);
        }
    }
}
