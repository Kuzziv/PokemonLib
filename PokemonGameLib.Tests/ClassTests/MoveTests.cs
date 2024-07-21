using Xunit;
using System;

namespace PokemonGameLib.Tests
{
    public class MoveTests
    {
        [Fact]
        public void TestMoveInitialization_ValidParameters()
        {
            // Arrange & Act
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 10);

            // Assert
            Assert.Equal("Thunderbolt", move.Name);
            Assert.Equal(PokemonType.Electric, move.Type);
            Assert.Equal(90, move.Power);
            Assert.Equal(10, move.Level);
        }

        [Fact]
        public void TestMoveInitialization_InvalidName()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Move("", PokemonType.Electric, 90, 10));
            Assert.Throws<ArgumentException>(() => new Move(null, PokemonType.Electric, 90, 10));
        }

        [Fact]
        public void TestMoveInitialization_InvalidType()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Move("Thunderbolt", (PokemonType)999, 90, 10));
        }

        [Fact]
        public void TestMoveInitialization_InvalidPower()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Move("Thunderbolt", PokemonType.Electric, -10, 10));
        }

        [Fact]
        public void TestMoveInitialization_InvalidLevel()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Move("Thunderbolt", PokemonType.Electric, 90, 0));
            Assert.Throws<ArgumentException>(() => new Move("Thunderbolt", PokemonType.Electric, 90, -1));
        }

        [Fact]
        public void TestIsCompatibleWith_Valid()
        {
            // Arrange
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 10);

            // Act
            var isCompatible = move.IsCompatibleWith(PokemonType.Electric, 10);

            // Assert
            Assert.True(isCompatible);
        }

        [Fact]
        public void TestIsCompatibleWith_InvalidType()
        {
            // Arrange
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 10);

            // Act
            var isCompatible = move.IsCompatibleWith(PokemonType.Fire, 10);

            // Assert
            Assert.False(isCompatible);
        }

        [Fact]
        public void TestIsCompatibleWith_LevelTooHigh()
        {
            // Arrange
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 10);

            // Act
            var isCompatible = move.IsCompatibleWith(PokemonType.Electric, 5);

            // Assert
            Assert.False(isCompatible);
        }
    }
}
