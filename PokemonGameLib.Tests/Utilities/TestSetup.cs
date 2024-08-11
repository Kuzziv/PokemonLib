using System;
using Xunit;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Tests.Utilities 
{
    public class TestSetup : IDisposable
    {
        public TestSetup()
        {
            // Configure the logger here
            LoggingService.Configure();
        }

        // This method will be called after all the tests in the collection have run
        public void Dispose()
        {
            // You can add any teardown logic here if needed
        }
    }
}