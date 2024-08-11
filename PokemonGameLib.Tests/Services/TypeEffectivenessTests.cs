using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Services;

namespace PokemonGameLib.Tests.Services
{
    public class TypeEffectivenessTests
    {
        [Theory]
        [InlineData(PokemonType.Fire, PokemonType.Grass, 2.0)]
        [InlineData(PokemonType.Water, PokemonType.Fire, 2.0)]
        [InlineData(PokemonType.Grass, PokemonType.Water, 2.0)]
        [InlineData(PokemonType.Electric, PokemonType.Water, 2.0)]
        [InlineData(PokemonType.Ice, PokemonType.Grass, 2.0)]
        [InlineData(PokemonType.Dragon, PokemonType.Dragon, 2.0)]
        [InlineData(PokemonType.Dark, PokemonType.Psychic, 2.0)]
        [InlineData(PokemonType.Fairy, PokemonType.Dragon, 2.0)]
        [InlineData(PokemonType.Fighting, PokemonType.Normal, 2.0)]
        [InlineData(PokemonType.Flying, PokemonType.Grass, 2.0)]
        [InlineData(PokemonType.Poison, PokemonType.Grass, 2.0)]
        [InlineData(PokemonType.Ground, PokemonType.Fire, 2.0)]
        [InlineData(PokemonType.Rock, PokemonType.Fire, 2.0)]
        [InlineData(PokemonType.Bug, PokemonType.Grass, 2.0)]
        [InlineData(PokemonType.Ghost, PokemonType.Psychic, 2.0)]
        [InlineData(PokemonType.Steel, PokemonType.Ice, 2.0)]
        public void TestSuperEffective(PokemonType attackType, PokemonType defenseType, double expectedEffectiveness)
        {
            // Act
            var effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(attackType, defenseType);

            // Assert
            Assert.Equal(expectedEffectiveness, effectiveness);
        }

        [Theory]
        [InlineData(PokemonType.Fire, PokemonType.Fire, 0.5)]
        [InlineData(PokemonType.Water, PokemonType.Grass, 0.5)]
        [InlineData(PokemonType.Grass, PokemonType.Fire, 0.5)]
        [InlineData(PokemonType.Electric, PokemonType.Electric, 0.5)]
        [InlineData(PokemonType.Electric, PokemonType.Grass, 0.5)]
        [InlineData(PokemonType.Ice, PokemonType.Fire, 0.5)]
        [InlineData(PokemonType.Dragon, PokemonType.Steel, 0.5)]
        [InlineData(PokemonType.Dark, PokemonType.Dark, 0.5)]
        [InlineData(PokemonType.Fairy, PokemonType.Fire, 0.5)]
        [InlineData(PokemonType.Fighting, PokemonType.Poison, 0.5)]
        [InlineData(PokemonType.Flying, PokemonType.Rock, 0.5)]
        [InlineData(PokemonType.Poison, PokemonType.Rock, 0.5)]
        [InlineData(PokemonType.Ground, PokemonType.Grass, 0.5)]
        [InlineData(PokemonType.Rock, PokemonType.Ground, 0.5)]
        [InlineData(PokemonType.Bug, PokemonType.Fire, 0.5)]
        [InlineData(PokemonType.Ghost, PokemonType.Dark, 0.5)]
        [InlineData(PokemonType.Steel, PokemonType.Fire, 0.5)]
        public void TestNotVeryEffective(PokemonType attackType, PokemonType defenseType, double expectedEffectiveness)
        {
            // Act
            var effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(attackType, defenseType);

            // Assert
            Assert.Equal(expectedEffectiveness, effectiveness);
        }

        [Theory]
        [InlineData(PokemonType.Electric, PokemonType.Ground, 0.0)]
        [InlineData(PokemonType.Normal, PokemonType.Ghost, 0.0)]
        [InlineData(PokemonType.Poison, PokemonType.Steel, 0.0)]
        [InlineData(PokemonType.Dragon, PokemonType.Fairy, 0.0)]
        [InlineData(PokemonType.Ghost, PokemonType.Normal, 0.0)]
        public void TestNoEffect(PokemonType attackType, PokemonType defenseType, double expectedEffectiveness)
        {
            // Act
            var effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(attackType, defenseType);
            
            // Assert
            Assert.Equal(expectedEffectiveness, effectiveness);
        }

        [Theory]
        [InlineData(PokemonType.Normal, PokemonType.Grass, 1.0)]
        [InlineData(PokemonType.Normal, PokemonType.Normal, 1.0)]
        [InlineData(PokemonType.Fire, PokemonType.Normal, 1.0)]
        [InlineData(PokemonType.Water, PokemonType.Normal, 1.0)]
        [InlineData(PokemonType.Grass, PokemonType.Normal, 1.0)]
        [InlineData(PokemonType.Electric, PokemonType.Normal, 1.0)]
        [InlineData(PokemonType.Ice, PokemonType.Normal, 1.0)]
        [InlineData(PokemonType.Ghost, PokemonType.Dark, 0.5)]
        public void TestNeutralEffectiveness(PokemonType attackType, PokemonType defenseType, double expectedEffectiveness)
        {
            // Act
            var effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(attackType, defenseType);

            // Assert
            Assert.Equal(expectedEffectiveness, effectiveness);
        }

        [Fact]
        public void TestDictionaryCompleteness()
        {
            // Arrange
            var types = Enum.GetValues(typeof(PokemonType)).Cast<PokemonType>();
            var totalCombinations = types.Count() * types.Count();

            // Act
            var definedCombinations = TypeEffectivenessService.Instance.GetEffectivenessDictionary().Count;

            // Assert
            Assert.True(definedCombinations < totalCombinations, "The dictionary should not define all possible combinations explicitly.");
        }
    }
}
