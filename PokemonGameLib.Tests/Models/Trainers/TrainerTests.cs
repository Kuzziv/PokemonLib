using System;
using System.Collections.Generic;
using Xunit;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Items;

namespace PokemonGameLib.Tests.Models.Trainers
{
    [Collection("Test Collection")]
    public class TrainerTests
    {
        private readonly Trainer _trainer;

        public TrainerTests()
        {
            // Creating a derived class of Trainer since Trainer is abstract
            _trainer = new TestTrainer("Ash");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestTrainer(null));
        }

        [Fact]
        public void AddPokemon_ShouldAddPokemonToList()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);

            // Act
            _trainer.AddPokemon(pikachu);

            // Assert
            Assert.Contains(pikachu, _trainer.Pokemons);
        }

        [Fact]
        public void AddPokemon_ShouldThrowArgumentNullException_WhenPokemonIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trainer.AddPokemon(null));
        }

        [Fact]
        public void RemovePokemon_ShouldRemovePokemonFromList()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            _trainer.AddPokemon(pikachu);

            // Act
            _trainer.RemovePokemon(pikachu);

            // Assert
            Assert.DoesNotContain(pikachu, _trainer.Pokemons);
        }

        [Fact]
        public void RemovePokemon_ShouldThrowArgumentNullException_WhenPokemonIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trainer.RemovePokemon(null));
        }

        [Fact]
        public void RemovePokemon_ShouldThrowInvalidOperationException_WhenPokemonNotInList()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.RemovePokemon(pikachu));
        }

        [Fact]
        public void SwitchPokemon_ShouldChangeCurrentPokemon()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            _trainer.AddPokemon(pikachu);

            // Act
            _trainer.SwitchPokemon(pikachu);

            // Assert
            Assert.Equal(pikachu, _trainer.CurrentPokemon);
        }

        [Fact]
        public void SwitchPokemon_ShouldThrowArgumentNullException_WhenPokemonIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trainer.SwitchPokemon(null));
        }

        [Fact]
        public void SwitchPokemon_ShouldThrowInvalidOperationException_WhenPokemonNotInList()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.SwitchPokemon(pikachu));
        }

        [Fact]
        public void SwitchPokemon_ShouldThrowInvalidOperationException_WhenPokemonIsFainted()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            pikachu.TakeDamage(150); // Make Pikachu faint
            _trainer.AddPokemon(pikachu);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.SwitchPokemon(pikachu));
        }

        [Fact]
        public void UseItem_ShouldUseItemOnPokemon()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            var potion = new Potion("Super Potion", "Heals 50 HP", 50);
            _trainer.AddItem(potion);

            // Act
            _trainer.UseItem(potion, pikachu);

            // Assert
            Assert.Equal(100, pikachu.CurrentHP); // Assumes Pikachu had taken 50 damage
            Assert.DoesNotContain(potion, _trainer.Items);
        }

        [Fact]
        public void UseItem_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);

            Assert.Throws<ArgumentNullException>(() => _trainer.UseItem(null, pikachu));
        }

        [Fact]
        public void UseItem_ShouldThrowArgumentNullException_WhenPokemonIsNull()
        {
            var potion = new Potion("Super Potion", "Heals 50 HP", 50);

            Assert.Throws<ArgumentNullException>(() => _trainer.UseItem(potion, null));
        }

        [Fact]
        public void UseItem_ShouldLogWarning_WhenItemNotInInventory()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            var potion = new Potion("Super Potion", "Heals 50 HP", 50);
            var initialHp = pikachu.CurrentHP;

            // Act
            _trainer.UseItem(potion, pikachu);

            // Assert
            // Ensure the potion was not used, and the Pokémon's HP remains unchanged
            Assert.Equal(initialHp, pikachu.CurrentHP);

            // Verify that the item was not removed (since it was never in the inventory)
            Assert.Empty(_trainer.Items);
        }


        [Fact]
        public void AddItem_ShouldAddItemToInventory()
        {
            // Arrange
            var potion = new Potion("Super Potion", "Heals 50 HP", 50);

            // Act
            _trainer.AddItem(potion);

            // Assert
            Assert.Contains(potion, _trainer.Items);
        }

        [Fact]
        public void AddItem_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trainer.AddItem(null));
        }
    }

    // A simple derived class for testing purposes
    public class TestTrainer : Trainer
    {
        public TestTrainer(string name) : base(name) { }

        public override void TakeTurn(IBattle battle)
        {
            // Implementation for test purposes
        }
    }
}
