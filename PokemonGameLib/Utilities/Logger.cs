using System;
using System.IO;
using System.Text;
using PokemonGameLib.Interfaces;
using YamlDotNet.Serialization;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides methods for logging messages and errors to a specified log file with thread-safety.
    /// Logs are formatted as YAML objects.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new();
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with the specified log file path and encoding.
        /// </summary>
        /// <param name="logFilePath">The file path where logs will be written.</param>
        /// <param name="encoding">The encoding to use for the log file. Default is UTF-8.</param>
        public Logger(string logFilePath, Encoding encoding = null)
        {
            _logFilePath = logFilePath;
            _encoding = encoding ?? Encoding.UTF8;
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
        /// Logs a message with a specified severity to the log file, with thread-safety.
        /// </summary>
        /// <param name="severity">The severity of the log message.</param>
        /// <param name="message">The message to log.</param>
        private void Log(string severity, string message)
        {
            try
            {
                string logEntry = FormatLogEntry(severity, message);

                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, _encoding);
                }
            }
            catch (Exception ex)
            {
                // Handle logging failures (e.g., fallback to console logging)
                Console.WriteLine($"Failed to log message. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats a log entry as a YAML string with a timestamp, severity, and message.
        /// </summary>
        /// <param name="severity">The severity of the log message.</param>
        /// <param name="message">The message to log.</param>
        /// <returns>A YAML-formatted log entry string.</returns>
        private string FormatLogEntry(string severity, string message)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Severity = severity,
                Message = message
            };

            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(logEntry);
        }
    }
}
