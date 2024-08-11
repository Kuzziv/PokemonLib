using System;
using Xunit;

namespace PokemonGameLib.Tests.Utilities 
{
    [CollectionDefinition("Test Collection")]
    public class TestCollection : ICollectionFixture<TestSetup>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}