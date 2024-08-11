using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Tests.Utilities
{
    public class LoggerTests : IDisposable
    {
        private readonly string _logDirectory;
        private readonly string _logFilePath;

        public LoggerTests()
        {
            // Set up the log file path to the specific Logs.yml file
            _logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "tmp_logs");
            _logFilePath = Path.Combine(_logDirectory, "Logs.yml");

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            // Reset the logger configuration for each test
            LoggingService.ResetConfiguration();

            // Configure the LoggingService to always use Logs.yml
            LoggingService.Configure(_logFilePath);

            Console.WriteLine($"Log file path: {_logFilePath}"); // Debug statement to confirm file path
        }
        
        public void Dispose()
        {
            // Clean up after each test - Decide if you want to clear the log file or leave it
            // if (File.Exists(_logFilePath))
            // {
            //     File.Delete(_logFilePath);
            // }
        }


        [Fact]
        public void Logger_ShouldLogInfoMessage()
        {
            // Act
            var logger = LoggingService.GetLogger();
            logger.LogInfo("This is an info message.");

            // Assert
            var logContents = File.ReadAllText(_logFilePath);
            Assert.Contains("Severity: INFO", logContents);
            Assert.Contains("Message: This is an info message.", logContents);
        }

        [Fact]
        public void Logger_ShouldLogWarningMessage()
        {
            // Act
            var logger = LoggingService.GetLogger();
            logger.LogWarning("This is a warning message.");

            // Assert
            var logContents = File.ReadAllText(_logFilePath);
            Assert.Contains("Severity: WARNING", logContents);
            Assert.Contains("Message: This is a warning message.", logContents);
        }

        [Fact]
        public void Logger_ShouldLogErrorMessage()
        {
            // Act
            var logger = LoggingService.GetLogger();
            logger.LogError("This is an error message.");

            // Assert
            var logContents = File.ReadAllText(_logFilePath);
            Assert.Contains("Severity: ERROR", logContents);
            Assert.Contains("Message: This is an error message.", logContents);
        }

        [Fact]
        public void Logger_ShouldCreateLogFileIfNotExists()
        {
            // Act
            var logger = LoggingService.GetLogger();
            logger.LogInfo("Creating log file.");

            // Assert
            Assert.True(File.Exists(_logFilePath));
        }

        [Fact]
        public void Logger_ShouldBeThreadSafe()
        {
            // Act
            var logger = LoggingService.GetLogger();
            int threadCount = 10;

            Parallel.For(0, threadCount, i =>
            {
                logger.LogInfo($"Log entry from thread {i}");
            });

            // Assert
            var logContents = File.ReadAllText(_logFilePath);
            for (int i = 0; i < threadCount; i++)
            {
                Assert.Contains($"Message: Log entry from thread {i}", logContents);
            }
        }
    }
}
