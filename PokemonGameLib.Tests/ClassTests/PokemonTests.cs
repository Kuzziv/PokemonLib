using Xunit;
using System;

namespace PokemonGameLib.Tests
{
    public class PokemonTests
    {
        [Fact]
        public void TestPokemonInitialization()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act & Assert
            Assert.Equal("Pikachu", pokemon.Name);
            Assert.Equal(PokemonType.Electric, pokemon.Type);
            Assert.Equal(10, pokemon.Level);
            Assert.Equal(100, pokemon.HP);
            Assert.Equal(55, pokemon.Attack);
            Assert.Equal(40, pokemon.Defense);
        }

        [Fact]
        public void TestTakeDamage()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.TakeDamage(30);

            // Assert
            Assert.Equal(70, pokemon.HP);
        }

        [Fact]
        public void TestTakeDamageShouldNotGoBelowZero()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 30, 55, 40);

            // Act
            pokemon.TakeDamage(40);

            // Assert
            Assert.Equal(0, pokemon.HP);
        }

        [Fact]
        public void TestIsFainted()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 0, 55, 40);

            // Act
            var isFainted = pokemon.IsFainted();

            // Assert
            Assert.True(isFainted);
        }

        [Fact]
        public void TestAddMove_Success()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 10);

            // Act
            pokemon.AddMove(move);

            // Assert
            Assert.Contains(move, pokemon.Moves);
        }

        [Fact]
        public void TestAddMove_TooManyMoves()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 40, 100, 55, 40);
            var move1 = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var move2 = new Move("Quick Attack", PokemonType.Electric, 40, 1);
            var move3 = new Move("Iron Tail", PokemonType.Electric, 100, 20);
            var move4 = new Move("Electro Ball", PokemonType.Electric, 100, 25);
            var move5 = new Move("Volt Tackle", PokemonType.Electric, 120, 35);

            pokemon.AddMove(move1);
            pokemon.AddMove(move2);
            pokemon.AddMove(move3);
            pokemon.AddMove(move4);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pokemon.AddMove(move5));
        }

        [Fact]
        public void TestAddMove_AlreadyKnownMove()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pokemon.AddMove(move);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pokemon.AddMove(move));
        }

        [Fact]
        public void TestAddMove_IncompatibleType()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var move = new Move("Flamethrower", PokemonType.Fire, 90, 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pokemon.AddMove(move));
        }

        [Fact]
        public void TestAddMove_LevelTooHigh()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var move = new Move("Thunderbolt", PokemonType.Electric, 90, 15); // Pokemon level is 10

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pokemon.AddMove(move));
        }
    }
}
