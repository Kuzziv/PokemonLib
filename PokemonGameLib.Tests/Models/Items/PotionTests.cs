using System;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Interfaces;
using Moq;
using Xunit;

namespace PokemonGameLib.Tests.Models.Items
{
    [Collection("Test Collection")]
    public class PotionTests
    {
        [Fact]
        public void Potion_Use_ShouldThrowArgumentNullException_WhenPokemonIsNull()
        {
            // Arrange
            var potion = new Potion("Potion", "Heals 20 HP.", 20);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => potion.Use(null, null));
        }

        [Fact]
        public void Potion_ShouldHealPokemon_WhenUsed()
        {
            // Arrange
            var potion = new Potion("Potion", "Heals 20 HP.", 20);
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 50, 100, 55, 40); // Max HP is 100
            pokemon.TakeDamage(60); // Current HP will be 40

            // Act
            potion.Use(new Mock<ITrainer>().Object, pokemon); // Trainer is mocked

            // Assert
            Assert.Equal(60, pokemon.CurrentHP); // Expect HP to be 40 + 20 = 60
        }

        [Fact]
        public void Potion_ShouldNotExceedMaxHP_WhenUsed()
        {
            // Arrange
            var potion = new Potion("Potion", "Heals 20 HP.", 20);
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 90, 100, 55, 40); // Max HP is 100
            pokemon.TakeDamage(10); // Current HP will be 90

            // Act
            potion.Use(new Mock<ITrainer>().Object, pokemon); // Trainer is mocked

            // Assert
            Assert.Equal(100, pokemon.CurrentHP); // Expect HP to be capped at 100
        }

        [Fact]
        public void Potion_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var potion = new Potion("Potion", "Heals 20 HP.", 20);

            // Assert
            Assert.Equal("Potion", potion.Name);
            Assert.Equal("Heals 20 HP.", potion.Description);
            Assert.Equal(20, potion.HealingAmount);
        }


    }
}
