using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Utilities
{
    public abstract class Loggable
    {
        protected readonly ILogger _logger;

        protected Loggable()
        {
            _logger = LoggingService.GetLogger();
        }

        protected void LogInfo(string message)
        {
            _logger.LogInfo(message);
        }

        protected void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        protected void LogError(string message)
        {
            _logger.LogError(message);
        }
    }
}
