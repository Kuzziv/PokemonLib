using Xunit;
using Moq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Tests.Utilities
{
    public class BattleValidatorTests
    {
        [Fact]
        public void ValidateMove_ThrowsArgumentNullException_WhenAttackerIsNull()
        {
            // Arrange
            var move = new Mock<IMove>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BattleValidator.ValidateMove(null, move.Object));
        }

        [Fact]
        public void ValidateMove_ThrowsArgumentNullException_WhenMoveIsNull()
        {
            // Arrange
            var attacker = new Mock<IPokemon>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BattleValidator.ValidateMove(attacker.Object, null));
        }

        [Fact]
        public void ValidateMove_ThrowsInvalidOperationException_WhenMoveNotKnownByAttacker()
        {
            // Arrange
            var attacker = new Mock<IPokemon>();
            attacker.Setup(a => a.Name).Returns("Pikachu");
            attacker.Setup(a => a.Moves).Returns(new List<IMove>());

            var move = new Mock<IMove>();
            move.Setup(m => m.Name).Returns("Thunderbolt");

            // Act & Assert
            Assert.Throws<PokemonGameLib.Exceptions.InvalidMoveException>(() => BattleValidator.ValidateMove(attacker.Object, move.Object));
        }

        [Fact]
        public void ValidatePokemonSwitch_ThrowsArgumentNullException_WhenTrainerIsNull()
        {
            // Arrange
            var pokemon = new Mock<IPokemon>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BattleValidator.ValidatePokemonSwitch(null, pokemon.Object));
        }

        [Fact]
        public void ValidatePokemonSwitch_ThrowsArgumentNullException_WhenPokemonIsNull()
        {
            // Arrange
            var trainer = new Mock<ITrainer>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BattleValidator.ValidatePokemonSwitch(trainer.Object, null));
        }

        [Fact]
        public void ValidatePokemonSwitch_ThrowsInvalidPokemonSwitchException_WhenPokemonNotOwnedByTrainer()
        {
            // Arrange
            var trainer = new Mock<ITrainer>();
            trainer.Setup(t => t.Name).Returns("Ash");
            trainer.Setup(t => t.Pokemons).Returns(new List<IPokemon>());

            var pokemon = new Mock<IPokemon>();
            pokemon.Setup(p => p.Name).Returns("Pikachu");

            // Act & Assert
            Assert.Throws<InvalidPokemonSwitchException>(() => BattleValidator.ValidatePokemonSwitch(trainer.Object, pokemon.Object));
        }

        [Fact]
        public void ValidatePokemonSwitch_ThrowsInvalidPokemonSwitchException_WhenPokemonIsFainted()
        {
            // Arrange
            var pokemon = new Mock<IPokemon>();
            pokemon.Setup(p => p.Name).Returns("Pikachu");
            pokemon.Setup(p => p.IsFainted()).Returns(true);

            var trainer = new Mock<ITrainer>();
            trainer.Setup(t => t.Name).Returns("Ash");
            trainer.Setup(t => t.Pokemons).Returns(new List<IPokemon> { pokemon.Object });

            // Act & Assert
            Assert.Throws<InvalidPokemonSwitchException>(() => BattleValidator.ValidatePokemonSwitch(trainer.Object, pokemon.Object));
        }
    }
}
