using System;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Items;
using Xunit;

namespace PokemonGameLib.Tests.Models.Items
{
    [Collection("Test Collection")]
    public class ReviveTests
    {
        [Fact]
        public void Revive_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var revive = new Revive("Revive", "Revives a fainted Pokémon with partial HP.", 50);

            // Assert
            Assert.Equal("Revive", revive.Name);
            Assert.Equal("Revives a fainted Pokémon with partial HP.", revive.Description);
            Assert.Equal(50, revive.RestorePercentage);
        }

        [Fact]
        public void Revive_Constructor_ShouldThrowArgumentOutOfRangeException_WhenRestorePercentageIsInvalid()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Revive("Revive", "Invalid Revive", 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Revive("Revive", "Invalid Revive", 101));
        }

        [Fact]
        public void Revive_ShouldRestoreCorrectAmountOfHP_WhenUsedOnFaintedPokemon()
        {
            // Arrange
            var revive = new Revive("Revive", "Revives a fainted Pokémon with partial HP.", 50);
            var pokemon = new Pokemon("Bulbasaur", PokemonType.Grass, 100, 200, 49, 49);
            pokemon.TakeDamage(200); // Faint the Pokémon

            // Act
            revive.Use(null, pokemon);

            // Assert
            Assert.Equal(100, pokemon.CurrentHP); // Should restore 50% of max HP
            Assert.False(pokemon.IsFainted());
        }

        [Fact]
        public void Revive_ShouldNotRestoreHP_WhenUsedOnNonFaintedPokemon()
        {
            // Arrange
            var revive = new Revive("Revive", "Revives a fainted Pokémon with partial HP.", 50);
            var pokemon = new Pokemon("Bulbasaur", PokemonType.Grass, 50, 200, 49, 49);

            // Act
            revive.Use(null, pokemon);

            // Assert
            Assert.Equal(200, pokemon.CurrentHP); // HP should remain the same
        }

        [Fact]
        public void Revive_Use_ShouldThrowArgumentNullException_WhenPokemonIsNull()
        {
            // Arrange
            var revive = new Revive("Revive", "Revives a fainted Pokémon with partial HP.", 50);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => revive.Use(null, null));
        }
    }
}
