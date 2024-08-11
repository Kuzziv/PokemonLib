using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Tests.Utilities
{
    public class LoggerTests : IDisposable
    {
        private readonly string _testLogFilePath;

        public LoggerTests()
        {
            // Set up a unique log file path for each test to avoid conflicts.
            _testLogFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.log");
        }

        public void Dispose()
        {
            // Clean up after each test
            if (File.Exists(_testLogFilePath))
            {
                File.Delete(_testLogFilePath);
            }
        }

        [Fact]
        public void Logger_ShouldLogInfoMessage()
        {
            // Arrange
            var logger = new Logger(_testLogFilePath);

            // Act
            logger.LogInfo("This is an info message.");

            // Assert
            var logContents = File.ReadAllText(_testLogFilePath);
            Assert.Contains("[INFO] This is an info message.", logContents);
        }

        [Fact]
        public void Logger_ShouldLogWarningMessage()
        {
            // Arrange
            var logger = new Logger(_testLogFilePath);

            // Act
            logger.LogWarning("This is a warning message.");

            // Assert
            var logContents = File.ReadAllText(_testLogFilePath);
            Assert.Contains("[WARNING] This is a warning message.", logContents);
        }

        [Fact]
        public void Logger_ShouldLogErrorMessage()
        {
            // Arrange
            var logger = new Logger(_testLogFilePath);

            // Act
            logger.LogError("This is an error message.");

            // Assert
            var logContents = File.ReadAllText(_testLogFilePath);
            Assert.Contains("[ERROR] This is an error message.", logContents);
        }

        [Fact]
        public void Logger_ShouldCreateLogFileIfNotExists()
        {
            // Arrange
            var logger = new Logger(_testLogFilePath);

            // Act
            logger.LogInfo("Creating log file.");

            // Assert
            Assert.True(File.Exists(_testLogFilePath));
        }

        [Fact]
        public void Logger_ShouldRotateLogFileWhenMaxSizeExceeded()
        {
            // Arrange
            int maxFileSizeInMB = 1; // Small size for test
            var logger = new Logger(_testLogFilePath, maxFileSizeInMB);

            // Act
            for (int i = 0; i < 10000; i++) // Generate a large number of log entries
            {
                logger.LogInfo("This is a log message.");
            }

            // Assert
            // Check if rotation happened by verifying if a rotated log file exists
            var logDirectory = Path.GetDirectoryName(_testLogFilePath);
            var logFiles = Directory.GetFiles(logDirectory, "*.log").Where(f => f.Contains(Path.GetFileNameWithoutExtension(_testLogFilePath))).ToList();

            Assert.True(logFiles.Count > 1, "Log file rotation did not occur as expected.");
        }

        [Fact]
        public void Logger_ShouldBeThreadSafe()
        {
            // Arrange
            var logger = new Logger(_testLogFilePath);
            int threadCount = 10;

            // Act
            Parallel.For(0, threadCount, i =>
            {
                logger.LogInfo($"Log entry from thread {i}");
            });

            // Assert
            var logContents = File.ReadAllText(_testLogFilePath);
            for (int i = 0; i < threadCount; i++)
            {
                Assert.Contains($"Log entry from thread {i}", logContents);
            }
        }
    }
}
