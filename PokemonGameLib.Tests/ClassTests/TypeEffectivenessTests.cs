using Xunit;

namespace PokemonGameLib.Tests
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
        public void TestSuperEffective(PokemonType attackType, PokemonType defenseType, double expectedEffectiveness)
        {
            // Act
            var effectiveness = TypeEffectiveness.GetEffectiveness(attackType, defenseType);

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
        public void TestNotVeryEffective(PokemonType attackType, PokemonType defenseType, double expectedEffectiveness)
        {
            // Act
            var effectiveness = TypeEffectiveness.GetEffectiveness(attackType, defenseType);

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
            var effectiveness = TypeEffectiveness.GetEffectiveness(attackType, defenseType);

            // Assert
            Assert.Equal(expectedEffectiveness, effectiveness);
        }
    }
}
