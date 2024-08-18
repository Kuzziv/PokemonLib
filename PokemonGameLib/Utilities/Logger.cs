using System;
using System.IO;
using System.Text;
using PokemonGameLib.Interfaces;
using YamlDotNet.Serialization;

namespace PokemonGameLib.Utilities
{
    public class Logger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new();
        private readonly Encoding _encoding;

        public Logger(string logFilePath, Encoding encoding = null)
        {
            _logFilePath = logFilePath;
            _encoding = encoding ?? Encoding.UTF8;
        }

        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        public void LogError(string message)
        {
            Log("ERROR", message);
        }

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
