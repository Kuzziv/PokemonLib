using Xunit;
using PokemonGameLib.Models;

namespace PokemonGameLib.Tests
{
    public class TrainerTests
    {
        [Fact]
        public void TestTrainerInitialization()
        {
            // Arrange
            var trainer = new Trainer("Ash");

            // Act
            var name = trainer.Name;

            // Assert
            Assert.Equal("Ash", name);
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
        public void TestHasValidPokemons_WithValidPokemon()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            trainer.AddPokemon(pikachu);

            // Act
            var hasValidPokemons = trainer.HasValidPokemons();

            // Assert
            Assert.True(hasValidPokemons);
        }

        [Fact]
        public void TestHasValidPokemons_WithFaintedPokemon()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var faintedPikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 0, 55, 40); // Fainted

            trainer.AddPokemon(faintedPikachu);

            // Act
            var hasValidPokemons = trainer.HasValidPokemons();

            // Assert
            Assert.False(hasValidPokemons);
        }

        [Fact]
        public void TestHasValidPokemons_WithMixedPokemons()
        {
            // Arrange
            var trainer = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var faintedCharizard = new Pokemon("Charizard", PokemonType.Fire, 10, 0, 70, 50); // Fainted

            trainer.AddPokemon(pikachu);
            trainer.AddPokemon(faintedCharizard);

            // Act
            var hasValidPokemons = trainer.HasValidPokemons();

            // Assert
            Assert.True(hasValidPokemons);
        }
    }
}
