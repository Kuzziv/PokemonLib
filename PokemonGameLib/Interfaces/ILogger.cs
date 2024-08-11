


namespace PokemonGameLib.Interfaces
{

    /// <summary>
    /// Represents a logger that can be used to log messages to the console.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message to the console.
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning message to the console.
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message to the console.
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);
    }
 
}
