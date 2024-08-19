using System;
using Xunit;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Tests.Exceptions
{
    public class ItemNotFoundExceptionTests
    {
        [Fact]
        public void ItemNotFoundException_WithMessage_ShouldSetMessage()
        {
            // Arrange
            string expectedMessage = "Item not found in inventory";

            // Act
            var exception = new ItemNotFoundException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ItemNotFoundException_WithMessageAndInnerException_ShouldSetMessageAndInnerException()
        {
            // Arrange
            string expectedMessage = "Item not found in inventory";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new ItemNotFoundException(expectedMessage, innerException);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
