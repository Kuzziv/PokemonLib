// StatusConditionTests.cs
using System;
using PokemonGameLib.Models;
using Xunit;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Tests.Models.Pokemons
{
    public class StatusConditionTests
    {
        [Fact]
        public void InflictStatus_ShouldApplyStatus_WhenStatusIsNone()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);

            // Act
            pokemon.InflictStatus(StatusCondition.Paralysis);

            // Assert
            Assert.Equal(StatusCondition.Paralysis, pokemon.Status);
        }

        [Fact]
        public void InflictStatus_ShouldNotChangeStatus_WhenStatusAlreadyExists()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            pokemon.InflictStatus(StatusCondition.Paralysis);

            // Act
            pokemon.InflictStatus(StatusCondition.Burn);

            // Assert
            Assert.Equal(StatusCondition.Paralysis, pokemon.Status);
        }

        [Fact]
        public void CureStatus_ShouldRemoveStatus_WhenStatusExists()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            pokemon.InflictStatus(StatusCondition.Paralysis);

            // Act
            pokemon.CureStatus();

            // Assert
            Assert.Equal(StatusCondition.None, pokemon.Status);
        }
    }
}
