using System;
using Xunit;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Tests.Exceptions
{
    public class InvalidMoveExceptionTests
    {
        [Fact]
        public void InvalidMoveException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            string expectedMessage = "Invalid move attempted";

            // Act
            var exception = new InvalidMoveException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void InvalidMoveException_WithMessageAndInnerException_ShouldSetMessageAndInnerException()
        {
            // Arrange
            string expectedMessage = "Invalid move attempted";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new InvalidMoveException(expectedMessage, innerException);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
