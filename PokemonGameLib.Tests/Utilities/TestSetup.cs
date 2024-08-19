using System;
using System.IO;
using Xunit;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Tests.Utilities
{
    [CollectionDefinition("Test Collection")]
    public class TestCollection : ICollectionFixture<TestSetup>
    {
    }

    // Use this setup to configure the logger only once
    public class TestSetup : IDisposable
    {
        private static bool _isConfigured = false;

        public TestSetup()
        {
            if (!_isConfigured)
            {
                string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "tmp_logs");
                string logFilePath = Path.Combine(logDirectory, "Logs.yml");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                LoggingService.ResetConfiguration();
                LoggingService.Configure(logFilePath);
                _isConfigured = true;
            }
        }

        public void Dispose()
        {
            // Any cleanup if necessary
        }
    }

}
