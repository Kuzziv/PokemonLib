using System;
using PokemonGameLib.Models;
using Xunit;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Abilities;

namespace PokemonGameLib.Tests.Models.Pokemons.Abilities
{
    public class AbilityTests
    {
        [Fact]
        public void ApplyEffect_ShouldLowerOpponentAttack_WhenAbilityIsIntimidate()
        {
            // Arrange
            var intimidate = new Ability("Intimidate", "Lowers the target's attack.");
            var attacker = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            var defender = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);

            // Act
            intimidate.ApplyEffect(null, attacker, defender);

            // Assert
            Assert.Equal(49, defender.Attack);
        }

        [Fact]
        public void Ability_ShouldNotChangeStats_WhenNoEffectDefined()
        {
            // Arrange
            var unknownAbility = new Ability("Unknown", "No effect.");
            var attacker = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            var defender = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);

            // Act
            unknownAbility.ApplyEffect(null, attacker, defender);

            // Assert
            Assert.Equal(50, defender.Attack);
        }

        [Fact]
        public void ApplyEffect_ShouldThrowException_WhenUserIsNull()
        {
            // Arrange
            var intimidate = new Ability("Intimidate", "Lowers the target's attack.");
            var defender = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => intimidate.ApplyEffect(null, null, defender));
        }

        [Fact]
        public void Ability_ShouldNotAffect_WhenTargetIsNull()
        {
            // Arrange
            var intimidate = new Ability("Intimidate", "Lowers the target's attack.");
            var attacker = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);

            // Act
            intimidate.ApplyEffect(null, attacker, null);

            // Assert
            // No exception should be thrown and no change in attacker's state
            Assert.Equal(50, attacker.Attack); // Ensure attacker's attack is unchanged
        }

        [Fact]
        public void Ability_ShouldThrowException_WhenNameIsNullOrEmpty()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Ability(null, "Some description"));
            Assert.Throws<ArgumentException>(() => new Ability("", "Some description"));
        }

        [Fact]
        public void Ability_ShouldThrowException_WhenDescriptionIsNullOrEmpty()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Ability("Some Ability", null));
            Assert.Throws<ArgumentException>(() => new Ability("Some Ability", ""));
        }

        [Fact]
        public void Ability_ShouldBeCreatedCorrectly_WhenValidParametersAreProvided()
        {
            // Arrange
            string name = "Overgrow";
            string description = "Boosts the power of Grass-type moves when the Pokémon's HP is low.";

            // Act
            var ability = new Ability(name, description);

            // Assert
            Assert.Equal(name, ability.Name);
            Assert.Equal(description, ability.Description);
        }
    }
}
