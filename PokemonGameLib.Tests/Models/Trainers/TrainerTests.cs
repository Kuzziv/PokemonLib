using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;

namespace PokemonGameLib.Tests
{
    // A concrete implementation of Trainer for testing purposes
    public class TestTrainer : Trainer
    {
        public TestTrainer(string name) : base(name) { }

        public override void TakeTurn(IBattle battle)
        {
            // Mock implementation for abstract method
        }

        // Expose protected Items property for testing
        public IList<IItem> GetItems() => Items;
    }

    public class TrainerTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenNameIsNull()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new TestTrainer(null));
            Assert.Equal("Trainer name cannot be null. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            var name = "Ash";

            // Act
            var trainer = new TestTrainer(name);

            // Assert
            Assert.Equal(name, trainer.Name);
            Assert.NotNull(trainer.Pokemons);
            Assert.NotNull(trainer.GetItems());
            Assert.Empty(trainer.Pokemons);
            Assert.Empty(trainer.GetItems());
            Assert.Null(trainer.CurrentPokemon);
        }

        [Fact]
        public void AddPokemon_ThrowsArgumentNullException_WhenPokemonIsNull()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.AddPokemon(null));
        }

        [Fact]
        public void AddPokemon_AddsPokemonToList()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon = new Mock<IPokemon>();
            mockPokemon.Setup(p => p.Name).Returns("Pikachu");

            // Act
            trainer.AddPokemon(mockPokemon.Object);

            // Assert
            Assert.Contains(mockPokemon.Object, trainer.Pokemons);
        }

        [Fact]
        public void RemovePokemon_ThrowsArgumentNullException_WhenPokemonIsNull()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.RemovePokemon(null));
        }

        [Fact]
        public void RemovePokemon_ThrowsInvalidOperationException_WhenPokemonNotInList()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon = new Mock<IPokemon>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.RemovePokemon(mockPokemon.Object));
        }

        [Fact]
        public void RemovePokemon_RemovesPokemonFromList()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon = new Mock<IPokemon>();
            mockPokemon.Setup(p => p.Name).Returns("Pikachu");
            trainer.AddPokemon(mockPokemon.Object);

            // Act
            trainer.RemovePokemon(mockPokemon.Object);

            // Assert
            Assert.DoesNotContain(mockPokemon.Object, trainer.Pokemons);
        }

        [Fact]
        public void SwitchPokemon_ThrowsArgumentNullException_WhenNewPokemonIsNull()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.SwitchPokemon(null));
        }

        [Fact]
        public void SwitchPokemon_ThrowsInvalidOperationException_WhenPokemonNotInList()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon = new Mock<IPokemon>();
            var mockPokemon2 = new Mock<IPokemon>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.SwitchPokemon(mockPokemon.Object));
        }

        [Fact]
        public void SwitchPokemon_ThrowsInvalidOperationException_WhenPokemonIsFainted()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon = new Mock<IPokemon>();
            mockPokemon.Setup(p => p.IsFainted()).Returns(true);
            trainer.AddPokemon(mockPokemon.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => trainer.SwitchPokemon(mockPokemon.Object));
        }

        [Fact]
        public void SwitchPokemon_ChangesCurrentPokemon()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon1 = new Mock<IPokemon>();
            var mockPokemon2 = new Mock<IPokemon>();
            mockPokemon1.Setup(p => p.Name).Returns("Pikachu");
            mockPokemon2.Setup(p => p.Name).Returns("Charmander");
            trainer.AddPokemon(mockPokemon1.Object);
            trainer.AddPokemon(mockPokemon2.Object);

            // Act
            trainer.SwitchPokemon(mockPokemon2.Object);

            // Assert
            Assert.Equal(mockPokemon2.Object, trainer.CurrentPokemon);
        }

        [Fact]
        public void UseItem_ThrowsArgumentNullException_WhenItemIsNull()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockPokemon = new Mock<IPokemon>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.UseItem(null, mockPokemon.Object));
        }

        [Fact]
        public void UseItem_ThrowsArgumentNullException_WhenTargetIsNull()
        {
            // Arrange
            var trainer = new TestTrainer("Ash");
            var mockItem = new Mock<IItem>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => trainer.UseItem(mockItem.Object, null));
        }

        [Fact]
        public void UseItem_UsesItemAndRemovesItFromInventory()
        {
            // Arrange
            var mockLogger = new Mock<Logger>(); // Custom Logger class
            var trainer = new TestTrainer("Ash");
            var mockItem = new Mock<IItem>();
            var mockPokemon = new Mock<IPokemon>();
            mockItem.Setup(i => i.Name).Returns("Potion");
            trainer.AddItem(mockItem.Object);
            mockItem.Setup(i => i.Use(trainer, mockPokemon.Object)).Verifiable();
            
            // Act
            trainer.UseItem(mockItem.Object, mockPokemon.Object);

            // Assert
            mockItem.Verify(i => i.Use(trainer, mockPokemon.Object), Times.Once);
            Assert.DoesNotContain(mockItem.Object, trainer.GetItems());
        }

        [Fact]
        public void UseItem_LogsWarningWhenItemNotInInventory()
        {
            // Arrange
            var mockLogger = new Mock<Logger>(); // Custom Logger class
            var trainer = new TestTrainer("Ash");
            var mockItem = new Mock<IItem>();
            var mockPokemon = new Mock<IPokemon>();

            // Act
            trainer.UseItem(mockItem.Object, mockPokemon.Object);

            // Assert
            mockLogger.Verify(l => l.LogWarning(It.IsAny<string>()), Times.Once);
        }
    }
}
