using Xunit;
using System;
using PokemonGameLib.Models.Moves;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Tests.Moves
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
            Assert.Throws<ArgumentException>(() => new Move(string.Empty, PokemonType.Electric, 90, 10));
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

        [Fact]
        public void TestMoveInitialization_WithRecoil()
        {
            // Arrange & Act
            var move = new Move("Volt Tackle", PokemonType.Electric, 120, 10, recoilPercentage: 10);

            // Assert
            Assert.Equal("Volt Tackle", move.Name);
            Assert.Equal(120, move.Power);
            Assert.Equal(10, move.Level);
            Assert.Equal(10, move.RecoilPercentage);
        }

        [Fact]
        public void TestMoveInitialization_WithHealing()
        {
            // Arrange & Act
            var move = new Move("Giga Drain", PokemonType.Grass, 75, 10, healingPercentage: 50);

            // Assert
            Assert.Equal("Giga Drain", move.Name);
            Assert.Equal(75, move.Power);
            Assert.Equal(10, move.Level);
            Assert.Equal(50, move.HealingPercentage);
        }

        [Fact]
        public void TestMoveInitialization_WithMultiHit()
        {
            // Arrange & Act
            var move = new Move("Double Slap", PokemonType.Normal, 15, 10, maxHits: 5);

            // Assert
            Assert.Equal("Double Slap", move.Name);
            Assert.Equal(15, move.Power);
            Assert.Equal(10, move.Level);
            Assert.Equal(5, move.MaxHits);
        }
    }
}
