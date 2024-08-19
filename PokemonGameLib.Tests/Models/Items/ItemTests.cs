using System;
using PokemonGameLib.Models.Items;
using Xunit;

namespace PokemonGameLib.Tests.Models.Items
{
    public class ItemTests
    {
        [Fact]
        public void Item_Constructor_ShouldInitializeNameAndDescription()
        {
            // Arrange & Act
            var item = new Item("Potion", "Heals 20 HP.");

            // Assert
            Assert.Equal("Potion", item.Name);
            Assert.Equal("Heals 20 HP.", item.Description);
        }

        [Fact]
        public void Item_Constructor_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Item(null, "Heals 20 HP."));
            Assert.Equal("Item name cannot be null or empty. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void Item_Constructor_ShouldThrowArgumentNullException_WhenDescriptionIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Item("Potion", null));
            Assert.Equal("Item description cannot be null or empty. (Parameter 'description')", exception.Message);
        }

        [Fact]
        public void Item_Constructor_ShouldThrowArgumentNullException_WhenNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Item(string.Empty, "Heals 20 HP."));
            Assert.Equal("Item name cannot be null or empty. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void Item_Constructor_ShouldThrowArgumentNullException_WhenDescriptionIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Item("Potion", string.Empty));
            Assert.Equal("Item description cannot be null or empty. (Parameter 'description')", exception.Message);
        }
    }
}
