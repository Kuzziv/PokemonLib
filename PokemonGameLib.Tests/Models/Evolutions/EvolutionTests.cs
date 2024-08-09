using System;
using PokemonGameLib.Models;
using Xunit;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Evolutions;

namespace PokemonGameLib.Tests.Models.Evolutions
{
    public class EvolutionTests
    {
        [Fact]
        public void CanEvolve_ShouldReturnTrue_WhenPokemonLevelIsGreaterThanOrEqualToRequiredLevel()
        {
            // Arrange
            var evolution = new Evolution("Charizard", 36);
            var pokemon = new Pokemon("Charmeleon", PokemonType.Fire, 36, 100, 60, 50);

            // Act
            bool canEvolve = evolution.CanEvolve(pokemon);

            // Assert
            Assert.True(canEvolve);
        }

        [Fact]
        public void CanEvolve_ShouldReturnFalse_WhenPokemonLevelIsLessThanRequiredLevel()
        {
            // Arrange
            var evolution = new Evolution("Charizard", 36);
            var pokemon = new Pokemon("Charmeleon", PokemonType.Fire, 35, 100, 60, 50);

            // Act
            bool canEvolve = evolution.CanEvolve(pokemon);

            // Assert
            Assert.False(canEvolve);
        }

        [Fact]
        public void CanEvolve_ShouldThrowException_WhenPokemonIsNull()
        {
            // Arrange
            var evolution = new Evolution("Charizard", 36);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => evolution.CanEvolve(null));
        }

        [Fact]
        public void Evolution_ShouldThrowException_WhenEvolvedFormNameIsNullOrEmpty()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Evolution(null, 36));
            Assert.Throws<ArgumentException>(() => new Evolution("", 36));
        }

        [Fact]
        public void Evolution_ShouldThrowException_WhenRequiredLevelIsLessThanOne()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Evolution("Charizard", 0));
            Assert.Throws<ArgumentException>(() => new Evolution("Charizard", -1));
        }

        [Fact]
        public void Evolution_ShouldBeCreatedCorrectly_WhenValidParametersAreProvided()
        {
            // Arrange
            string evolvedFormName = "Charizard";
            int requiredLevel = 36;

            // Act
            var evolution = new Evolution(evolvedFormName, requiredLevel);

            // Assert
            Assert.Equal(evolvedFormName, evolution.EvolvedFormName);
            Assert.Equal(requiredLevel, evolution.RequiredLevel);
        }
    }
}
