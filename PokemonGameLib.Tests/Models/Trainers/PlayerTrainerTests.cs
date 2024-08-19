using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Xunit;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Tests.Models.Trainers
{
    [Collection("Test Collection")]
    public class PlayerTrainerTests
    {
        private readonly PlayerTrainer _playerTrainer;
        private readonly Mock<IBattle> _mockBattle;

        public PlayerTrainerTests()
        {
            _playerTrainer = new PlayerTrainer("Ash");
            _mockBattle = new Mock<IBattle>();
        }

        [Fact]
        public void TakeTurn_PerformAttack_UsesMove()
        {
            // Arrange
            var pokemon = new Mock<IPokemon>();
            var move = new Mock<IMove>();
            move.Setup(m => m.Name).Returns("Thunderbolt");
            pokemon.Setup(p => p.Moves).Returns(new List<IMove> { move.Object });
            _playerTrainer.AddPokemon(pokemon.Object);
            _playerTrainer.CurrentPokemon = pokemon.Object;

            // Simulate user input for performing an attack
            var input = "1\n1\n"; // Choose attack
            using (var sr = new StringReader(input))
            {
                Console.SetIn(sr);
                _playerTrainer.TakeTurn(_mockBattle.Object);

                // Assert
                _mockBattle.Verify(b => b.PerformAttack(move.Object), Times.Once);
            }
        }

        [Fact]
        public void TakeTurn_SwitchPokemon_SwitchesPokemon()
        {
            // Arrange
            var pokemon1 = new Mock<IPokemon>();
            var pokemon2 = new Mock<IPokemon>();
            pokemon1.Setup(p => p.Name).Returns("Pikachu");
            pokemon2.Setup(p => p.Name).Returns("Charmander");
            pokemon1.Setup(p => p.IsFainted()).Returns(false);
            pokemon2.Setup(p => p.IsFainted()).Returns(false);
            _playerTrainer.AddPokemon(pokemon1.Object);
            _playerTrainer.AddPokemon(pokemon2.Object);
            _playerTrainer.CurrentPokemon = pokemon1.Object;

            // Simulate user input for switching Pokémon
            var input = "2\n2\n"; // Choose to switch to Charmander
            using (var sr = new StringReader(input))
            {
                Console.SetIn(sr);
                _playerTrainer.TakeTurn(_mockBattle.Object);

                // Assert
                _mockBattle.Verify(b => b.PerformSwitch(_playerTrainer, pokemon2.Object), Times.Once);
            }
        }

        [Fact]
        public void TakeTurn_UseItem_UsesItem()
        {
            // Arrange
            var item = new Mock<IItem>();
            var pokemon = new Mock<IPokemon>();
            item.Setup(i => i.Name).Returns("Potion");
            item.Setup(i => i.Description).Returns("Heals HP");
            _playerTrainer.AddItem(item.Object);
            _playerTrainer.AddPokemon(pokemon.Object);
            _playerTrainer.CurrentPokemon = pokemon.Object;

            // Simulate user input for using item
            var input = "3\n1\n1\n"; // Choose to use the Potion on the first Pokémon
            using (var sr = new StringReader(input))
            {
                Console.SetIn(sr);
                _playerTrainer.TakeTurn(_mockBattle.Object);

                // Assert
                _mockBattle.Verify(b => b.PerformUseItem(item.Object, pokemon.Object), Times.Once);
                Assert.DoesNotContain(item.Object, _playerTrainer.Items);
            }
        }

        [Fact]
        public void HandleFaintedPokemon_SwitchesToNonFaintedPokemon()
        {
            // Arrange
            var faintedPokemon = new Mock<IPokemon>();
            var healthyPokemon = new Mock<IPokemon>();
            faintedPokemon.Setup(p => p.IsFainted()).Returns(true);
            healthyPokemon.Setup(p => p.IsFainted()).Returns(false);
            faintedPokemon.Setup(p => p.Name).Returns("FaintedMon");
            healthyPokemon.Setup(p => p.Name).Returns("HealthyMon");

            _playerTrainer.AddPokemon(faintedPokemon.Object);
            _playerTrainer.AddPokemon(healthyPokemon.Object);
            _playerTrainer.CurrentPokemon = faintedPokemon.Object;

            // Simulate user input for switching to the non-fainted Pokémon
            var input = "2\n"; // Choose to switch to HealthyMon
            using (var sr = new StringReader(input))
            {
                Console.SetIn(sr);
                _playerTrainer.HandleFaintedPokemon(_mockBattle.Object);

                // Assert
                _mockBattle.Verify(b => b.PerformSwitch(_playerTrainer, healthyPokemon.Object), Times.Once);
            }
        }
    }
}
