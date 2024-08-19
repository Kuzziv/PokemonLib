using Xunit;
using Moq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Tests.Utilities
{
    public class ValidationUtilityTests
    {
        [Fact]
        public void ValidatePokemon_ThrowsArgumentNullException_WhenPokemonIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ValidationUtility.ValidatePokemon(null, "pokemon"));
        }

        [Fact]
        public void ValidatePokemon_ThrowsPokemonFaintedException_WhenPokemonIsFainted()
        {
            // Arrange
            var pokemon = new Mock<IPokemon>();
            pokemon.Setup(p => p.Name).Returns("Pikachu");
            pokemon.Setup(p => p.IsFainted()).Returns(true);

            // Act & Assert
            Assert.Throws<PokemonFaintedException>(() => ValidationUtility.ValidatePokemon(pokemon.Object, "pokemon"));
        }

        [Fact]
        public void ValidateMove_ThrowsArgumentNullException_WhenAttackerIsNull()
        {
            // Arrange
            var move = new Mock<IMove>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ValidationUtility.ValidateMove(null, move.Object));
        }

        [Fact]
        public void ValidateMove_ThrowsArgumentNullException_WhenMoveIsNull()
        {
            // Arrange
            var attacker = new Mock<IPokemon>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ValidationUtility.ValidateMove(attacker.Object, null));
        }

        [Fact]
        public void ValidateMove_ThrowsInvalidMoveException_WhenMoveNotKnownByAttacker()
        {
            // Arrange
            var attacker = new Mock<IPokemon>();
            attacker.Setup(a => a.Name).Returns("Pikachu");
            attacker.Setup(a => a.Moves).Returns(new List<IMove>());

            var move = new Mock<IMove>();
            move.Setup(m => m.Name).Returns("Thunderbolt");

            // Act & Assert
            Assert.Throws<InvalidMoveException>(() => ValidationUtility.ValidateMove(attacker.Object, move.Object));
        }
    }
}
