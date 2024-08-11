using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Xunit;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Utilities;

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
            _playerTrainer.SwitchPokemon(pokemon.Object); // Ensure CurrentPokemon is set

            // Simulate user input for performing attack
            var input = "1\n1\n"; // Choose attack
            using (var sw = new StringWriter())
            {
                Console.SetIn(new StringReader(input));
                _playerTrainer.TakeTurn(_mockBattle.Object);
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
            _playerTrainer.SwitchPokemon(pokemon1.Object); // Ensure starting with Pikachu

            // Simulate user input for switching Pokémon
            var input = "2\n2\n"; // Choose switch Pokémon
            using (var sw = new StringWriter())
            {
                Console.SetIn(new StringReader(input));
                _playerTrainer.TakeTurn(_mockBattle.Object);
                _mockBattle.Verify(b => b.SwitchPokemon(_playerTrainer, pokemon2.Object), Times.Once);
            }
        }

        [Fact]
        public void TakeTurn_UseItem_UsesItem()
        {
            // Arrange
            var item = new Mock<IItem>();
            item.Setup(i => i.Name).Returns("Potion");
            item.Setup(i => i.Description).Returns("Heals HP");
            var pokemon = new Mock<IPokemon>();
            _playerTrainer.AddItem(item.Object);
            _playerTrainer.AddPokemon(pokemon.Object);
            _playerTrainer.SwitchPokemon(pokemon.Object); // Ensure CurrentPokemon is set

            // Simulate user input for using item
            var input = "3\n1\n"; // Choose use item
            using (var sw = new StringWriter())
            {
                Console.SetIn(new StringReader(input));
                _playerTrainer.TakeTurn(_mockBattle.Object);
                // Verify the item was used, skipping the logger check
                item.Verify(i => i.Use(_playerTrainer, pokemon.Object), Times.Once);
            }
        }

        [Fact]
        public void TakeTurn_InvalidChoice_DoesNothing()
        {
            // Arrange
            var input = "4\n4\n4\n"; // Invalid choices

            // Simulate user input for invalid choice
            using (var sw = new StringWriter())
            {
                Console.SetIn(new StringReader(input));
                _playerTrainer.TakeTurn(_mockBattle.Object);
                // Verify that no actions were taken
                _mockBattle.Verify(b => b.PerformAttack(It.IsAny<IMove>()), Times.Never);
                _mockBattle.Verify(b => b.SwitchPokemon(It.IsAny<Trainer>(), It.IsAny<IPokemon>()), Times.Never);
            }
        }

        [Fact]
        public void TakeTurn_EmptyPokemonList_DoesNothing()
        {
            // Arrange
            var input = "2\n1\n"; // Choose switch Pokémon

            // Simulate user input when Pokémon list is empty
            if (_playerTrainer.Pokemons.Count > 0)
                _playerTrainer.RemovePokemon(_playerTrainer.Pokemons[0]);

            using (var sw = new StringWriter())
            {
                Console.SetIn(new StringReader(input));
                _playerTrainer.TakeTurn(_mockBattle.Object);
                // Verify that no Pokémon was switched
                _mockBattle.Verify(b => b.SwitchPokemon(It.IsAny<Trainer>(), It.IsAny<IPokemon>()), Times.Never);
            }
        }
    }
}
