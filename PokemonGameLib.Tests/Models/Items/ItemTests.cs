// ItemTests.cs
using System;
using PokemonGameLib.Models;
using Xunit;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Items;

namespace PokemonGameLib.Tests.Models.Items
{
    public class ItemTests
    {
        [Fact]
        public void Potion_ShouldHealPokemon_WhenUsed()
        {
            // Arrange
            var potion = new Potion("Potion", "Heals 20 HP.", 20);
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            pokemon.TakeDamage(30);

            // Act
            potion.Use(null, pokemon);

            // Assert
            Assert.Equal(90, pokemon.CurrentHP);
        }

        [Fact]
        public void Potion_ShouldNotExceedMaxHP_WhenUsed()
        {
            // Arrange
            var potion = new Potion("Potion", "Heals 20 HP.", 20);
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            pokemon.TakeDamage(10);

            // Act
            potion.Use(null, pokemon);

            // Assert
            Assert.Equal(100, pokemon.CurrentHP);
        }
    }
}
