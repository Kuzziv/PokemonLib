using System;
using System.Collections.Generic;
using Xunit;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Items;
using Moq;

namespace PokemonGameLib.Tests.Models.Trainers
{
    [Collection("Test Collection")]
    public class TrainerTests
    {
        private readonly Trainer _trainer;
        private readonly Mock<ILogger> _loggerMock;

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
            Assert.Throws<NullReferenceException>(() => _trainer.AddItem(null));
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItemFromInventory()
        {
            // Arrange
            var potion = new Potion("Super Potion", "Heals 50 HP", 50);
            _trainer.AddItem(potion);

            // Act
            _trainer.RemoveItem(potion);

            // Assert
            Assert.DoesNotContain(potion, _trainer.Items);
        }

        [Fact]
        public void RemoveItem_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _trainer.RemoveItem(null));
        }

        [Fact]
        public void RemoveItem_ShouldThrowInvalidOperationException_WhenItemNotInList()
        {
            // Arrange
            var potion = new Potion("Super Potion", "Heals 50 HP", 50);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.RemoveItem(potion));
        }

        [Fact]
        public void ValidateTrainer_ShouldThrowInvalidOperationException_WhenNameIsNullOrWhitespace()
        {
            var invalidTrainer = new TestTrainer(" ");
            Assert.Throws<InvalidOperationException>(() => invalidTrainer.ValidateTrainer());
        }

        [Fact]
        public void ValidateTrainer_ShouldThrowInvalidOperationException_WhenCurrentPokemonIsNull()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            _trainer.AddPokemon(pikachu);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.ValidateTrainer());
        }

        [Fact]
        public void ValidateTrainer_ShouldThrowInvalidOperationException_WhenCurrentPokemonNotInCollection()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 50, 40);
            _trainer.AddPokemon(pikachu);
            _trainer.CurrentPokemon = bulbasaur;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.ValidateTrainer());
        }

        [Fact]
        public void ValidateTrainer_ShouldThrowInvalidOperationException_WhenNoPokemonsInCollection()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.ValidateTrainer());
        }

        [Fact]
        public void ValidateTrainer_ShouldThrowInvalidOperationException_WhenAllPokemonsAreFainted()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 50, 40);
            _trainer.AddPokemon(pikachu);
            _trainer.CurrentPokemon = pikachu;
            pikachu.TakeDamage(150); // Faint Pikachu

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _trainer.ValidateTrainer());
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

        public override void HandleFaintedPokemon(IBattle battle)
        {
            // Implementation for test purposes
        }
    }
}
