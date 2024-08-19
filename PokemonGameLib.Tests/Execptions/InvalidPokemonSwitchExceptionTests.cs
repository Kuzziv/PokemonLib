using System;
using Xunit;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Tests.Exceptions
{
    public class InvalidPokemonSwitchExceptionTests
    {
        [Fact]
        public void InvalidPokemonSwitchException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            string expectedMessage = "Invalid Pokémon switch attempted";

            // Act
            var exception = new InvalidPokemonSwitchException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void InvalidPokemonSwitchException_WithMessageAndInnerException_ShouldSetMessageAndInnerException()
        {
            // Arrange
            string expectedMessage = "Invalid Pokémon switch attempted";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new InvalidPokemonSwitchException(expectedMessage, innerException);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
