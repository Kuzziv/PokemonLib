using Xunit;
using PokemonGameLib.Models;
using System;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Tests.Models.Trainers
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
            Assert.Null(trainer.CurrentPokemon);
        }

        [Fact]
        public void TestTrainerInitialization_WithNullName_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Trainer(null));
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
        public void TestAddPokemon_NullPokemon_ShouldThrowException()
        {
            // Arrange
            var trainer = new Trainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.AddPokemon(null));
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
        public void TestRemovePokemon_NullPokemon_ShouldThrowException()
        {
            // Arrange
            var trainer = new Trainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.RemovePokemon(null));
        }

        [Fact]
        public void TestRemovePokemon_PokemonNotInList_ShouldThrowException()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);

            trainer.AddPokemon(pikachu);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.RemovePokemon(charizard));
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
        public void TestSwitchPokemon_CurrentPokemonSetCorrectly()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);

            trainer.AddPokemon(pikachu);
            trainer.AddPokemon(bulbasaur);

            // Act
            trainer.SwitchPokemon(bulbasaur);

            // Assert
            Assert.Equal(bulbasaur, trainer.CurrentPokemon);
        }

        [Fact]
        public void TestSwitchPokemon_ToFaintedPokemon_ShouldThrowException()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pikachu.TakeDamage(100); // Faint Pikachu

            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);

            trainer.AddPokemon(pikachu);
            trainer.AddPokemon(bulbasaur);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.SwitchPokemon(pikachu));
        }

        [Fact]
        public void TestSwitchPokemon_PokemonNotOwned_ShouldThrowException()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);
            trainer.AddPokemon(pikachu);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.SwitchPokemon(bulbasaur));
        }

        [Fact]
        public void TestSwitchPokemon_NullPokemon_ShouldThrowException()
        {
            // Arrange
            var trainer = new Trainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.SwitchPokemon(null));
        }
    }
}
