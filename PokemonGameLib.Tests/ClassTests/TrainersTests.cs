using Xunit;
using PokemonGameLib.Models;
using System;
using System.Linq;

namespace PokemonGameLib.Tests
{
    public class TrainerTests
    {
        [Fact]
        public void TestTrainerInitialization()
        {
            // Arrange & Act
            var trainer = new Trainer("Ash");

            // Assert
            Assert.Equal("Ash", trainer.Name);
            Assert.Empty(trainer.Pokemons);
        }

        [Fact]
        public void TestAddPokemon()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            trainer.AddPokemon(pikachu);

            // Assert
            Assert.Single(trainer.Pokemons);
            Assert.Contains(pikachu, trainer.Pokemons);
        }

        [Fact]
        public void TestHasValidPokemon_WithValidPokemon()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            trainer.AddPokemon(pikachu);

            // Act
            var hasValidPokemon = trainer.HasValidPokemon();

            // Assert
            Assert.True(hasValidPokemon);
        }

        [Fact]
        public void TestHasValidPokemon_WithFaintedPokemon()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var faintedPikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            faintedPikachu.TakeDamage(100); // Simulate fainting

            trainer.AddPokemon(faintedPikachu);

            // Act
            var hasValidPokemon = trainer.HasValidPokemon();

            // Assert
            Assert.False(hasValidPokemon);
        }

        [Fact]
        public void TestHasValidPokemon_WithMixedPokemon()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var faintedCharizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);

            faintedCharizard.TakeDamage(100); // Simulate fainting

            trainer.AddPokemon(pikachu);
            trainer.AddPokemon(faintedCharizard);

            // Act
            var hasValidPokemon = trainer.HasValidPokemon();

            // Assert
            Assert.True(hasValidPokemon);
        }

        [Fact]
        public void TestRemovePokemon_Success()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            trainer.AddPokemon(pikachu);

            // Act
            trainer.RemovePokemon(pikachu);

            // Assert
            Assert.Empty(trainer.Pokemons);
        }

        [Fact]
        public void TestRemovePokemon_NullPokemon_ThrowsArgumentNullException()
        {
            // Arrange
            var trainer = new Trainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.RemovePokemon(null));
        }

        [Fact]
        public void TestRemovePokemon_PokemonNotInList_ThrowsInvalidOperationException()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);

            trainer.AddPokemon(pikachu);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.RemovePokemon(charizard));
        }
    }
}
