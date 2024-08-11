using System;
using System.IO;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides methods for logging messages and errors.
    /// </summary>
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath = "game_log.txt")
        {
            _logFilePath = logFilePath;
        }

        /// <summary>
        /// Logs an informational message to the log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        /// <summary>
        /// Logs a warning message to the log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        /// <summary>
        /// Logs an error message to the log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        /// <summary>
        /// Logs a message with a specified severity to the log file.
        /// </summary>
        /// <param name="severity">The severity of the log message.</param>
        /// <param name="message">The message to log.</param>
        private void Log(string severity, string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{severity}] {message}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }
}
