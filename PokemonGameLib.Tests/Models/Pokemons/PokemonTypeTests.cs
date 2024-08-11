using System;
using System.Collections.Generic; // Add this for IEnumerable<T>
using System.Linq; // Add this for Cast<T>() and other LINQ operations
using Xunit;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Tests.Models.Pokemons
{
    [Collection("Test Collection")]
    public class PokemonTypeTests
    {
        [Fact]
        public void PokemonType_ShouldContainAllExpectedValues()
        {
            // Arrange
            var expectedTypes = new List<PokemonType>
            {
                PokemonType.Fire,
                PokemonType.Water,
                PokemonType.Grass,
                PokemonType.Electric,
                PokemonType.Psychic,
                PokemonType.Ice,
                PokemonType.Dragon,
                PokemonType.Dark,
                PokemonType.Fairy,
                PokemonType.Normal,
                PokemonType.Fighting,
                PokemonType.Flying,
                PokemonType.Poison,
                PokemonType.Ground,
                PokemonType.Rock,
                PokemonType.Bug,
                PokemonType.Ghost,
                PokemonType.Steel
            };

            // Act
            var actualTypes = Enum.GetValues(typeof(PokemonType)).Cast<PokemonType>().ToList(); // Cast to PokemonType and convert to List

            // Assert
            Assert.Equal(expectedTypes.Count, actualTypes.Count);

            foreach (var expectedType in expectedTypes)
            {
                Assert.Contains(expectedType, actualTypes);
            }
        }

        [Theory]
        [InlineData(PokemonType.Fire)]
        [InlineData(PokemonType.Water)]
        [InlineData(PokemonType.Grass)]
        [InlineData(PokemonType.Electric)]
        [InlineData(PokemonType.Psychic)]
        [InlineData(PokemonType.Ice)]
        [InlineData(PokemonType.Dragon)]
        [InlineData(PokemonType.Dark)]
        [InlineData(PokemonType.Fairy)]
        [InlineData(PokemonType.Normal)]
        [InlineData(PokemonType.Fighting)]
        [InlineData(PokemonType.Flying)]
        [InlineData(PokemonType.Poison)]
        [InlineData(PokemonType.Ground)]
        [InlineData(PokemonType.Rock)]
        [InlineData(PokemonType.Bug)]
        [InlineData(PokemonType.Ghost)]
        [InlineData(PokemonType.Steel)]
        public void PokemonType_ShouldBeDefined(PokemonType type)
        {
            // Act
            bool isDefined = Enum.IsDefined(typeof(PokemonType), type);

            // Assert
            Assert.True(isDefined);
        }

        [Fact]
        public void PokemonType_ShouldHaveUniqueValues()
        {
            // Arrange
            var values = Enum.GetValues(typeof(PokemonType)).Cast<PokemonType>().ToList();
            var valueSet = new HashSet<PokemonType>();

            // Act & Assert
            foreach (var value in values)
            {
                Assert.DoesNotContain(value, valueSet);
                valueSet.Add(value);
            }
        }
    }
}
