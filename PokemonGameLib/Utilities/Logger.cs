using System;
using System.IO;
using System.Text;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides methods for logging messages and errors to a specified log file with thread-safety and optional log rotation.
    /// </summary>
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new();
        private readonly long _maxFileSizeInBytes;
        private readonly string _logFileExtension = ".log";
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with the specified log file path, optional log rotation, and encoding.
        /// </summary>
        /// <param name="logFilePath">The file path where logs will be written.</param>
        /// <param name="maxFileSizeInMB">The maximum size of the log file in megabytes before it is rotated. Default is 5 MB.</param>
        /// <param name="encoding">The encoding to use for the log file. Default is UTF-8.</param>
        public Logger(string logFilePath, int maxFileSizeInMB = 5, Encoding encoding = null)
        {
            _logFilePath = logFilePath;
            _maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;
            _encoding = encoding ?? Encoding.UTF8;

            if (!Path.HasExtension(_logFilePath))
            {
                _logFilePath += _logFileExtension;
            }
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
        /// Logs a message with a specified severity to the log file, with thread-safety and optional log rotation.
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
                    EnsureLogRotation();
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
        /// Formats a log entry with a timestamp, severity, and message.
        /// </summary>
        /// <param name="severity">The severity of the log message.</param>
        /// <param name="message">The message to log.</param>
        /// <returns>A formatted log entry string.</returns>
        private string FormatLogEntry(string severity, string message)
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{severity}] {message}";
        }

        /// <summary>
        /// Checks the log file size and rotates it if it exceeds the maximum size.
        /// </summary>
        private void EnsureLogRotation()
        {
            try
            {
                FileInfo logFileInfo = new(_logFilePath);
                if (logFileInfo.Exists && logFileInfo.Length > _maxFileSizeInBytes)
                {
                    RotateLogFile();
                }
            }
            catch (Exception ex)
            {
                // Handle log rotation failures (e.g., log rotation failure in the existing log)
                Console.WriteLine($"Failed to rotate log file. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Rotates the current log file by renaming it with a timestamp.
        /// </summary>
        private void RotateLogFile()
        {
            string directory = Path.GetDirectoryName(_logFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_logFilePath);
            string newFileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMdd_HHmmss}{_logFileExtension}";

            string newFilePath = Path.Combine(directory, newFileName);
            File.Move(_logFilePath, newFilePath);
        }
    }
}
