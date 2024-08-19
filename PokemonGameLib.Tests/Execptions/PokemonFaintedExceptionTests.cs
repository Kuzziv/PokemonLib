using System;
using Xunit;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Tests.Exceptions
{
    public class PokemonFaintedExceptionTests
    {
        [Fact]
        public void PokemonFaintedException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            string expectedMessage = "Pokémon has fainted and cannot be used";

            // Act
            var exception = new PokemonFaintedException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void PokemonFaintedException_WithMessageAndInnerException_ShouldSetMessageAndInnerException()
        {
            // Arrange
            string expectedMessage = "Pokémon has fainted and cannot be used";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new PokemonFaintedException(expectedMessage, innerException);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
