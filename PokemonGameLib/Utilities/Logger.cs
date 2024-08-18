using System;
using System.IO;
using System.Text;
using PokemonGameLib.Interfaces;
using YamlDotNet.Serialization;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides a logger implementation that logs messages to a file in YAML format.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// The file path where log entries are written.
        /// </summary>
        private readonly string _logFilePath;

        /// <summary>
        /// Object used to synchronize access to the log file.
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// The encoding used for writing log entries to the file.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with the specified log file path and optional encoding.
        /// </summary>
        /// <param name="logFilePath">The file path where log entries will be written.</param>
        /// <param name="encoding">The encoding to use when writing log entries. If null, UTF-8 encoding is used.</param>
        public Logger(string logFilePath, Encoding? encoding = null)
        {
            _logFilePath = logFilePath;
            _encoding = encoding ?? Encoding.UTF8;
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        /// <summary>
        /// Logs a message with the specified severity.
        /// </summary>
        /// <param name="severity">The severity of the message (e.g., INFO, WARNING, ERROR).</param>
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
                Console.WriteLine($"Failed to log message. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats a log entry with the specified severity and message.
        /// </summary>
        /// <param name="severity">The severity of the log entry.</param>
        /// <param name="message">The message to include in the log entry.</param>
        /// <returns>A formatted log entry as a string in YAML format.</returns>
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
