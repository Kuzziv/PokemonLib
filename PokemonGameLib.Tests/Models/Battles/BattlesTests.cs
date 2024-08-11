using Moq;
using Xunit;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;
using PokemonGameLib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonGameLib.Tests
{
    public class BattleTests
    {
        private readonly Mock<ITrainer> _firstTrainer;
        private readonly Mock<ITrainer> _secondTrainer;
        private readonly Mock<ITypeEffectivenessService> _typeEffectivenessService;
        private readonly Mock<RandomNumberGenerator> _randomNumberGenerator;
        private readonly Mock<Logger> _logger;
        private readonly Battle _battle;

        public BattleTests()
        {
            _firstTrainer = new Mock<ITrainer>();
            _secondTrainer = new Mock<ITrainer>();
            _typeEffectivenessService = new Mock<ITypeEffectivenessService>();
            _randomNumberGenerator = new Mock<RandomNumberGenerator>();
            _logger = new Mock<Logger>();

            _battle = new Battle(
                _firstTrainer.Object,
                _secondTrainer.Object,
                _typeEffectivenessService.Object,
                _randomNumberGenerator.Object
            );

            // Initialize logger if needed
            // LoggingService.SetLogger(_logger.Object); // Check this method or remove if not applicable
        }

        [Fact]
        public void Battle_Initialization_WithNullTrainer_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Battle(null, _secondTrainer.Object, _typeEffectivenessService.Object, _randomNumberGenerator.Object));
            Assert.Throws<ArgumentNullException>(() => new Battle(_firstTrainer.Object, null, _typeEffectivenessService.Object, _randomNumberGenerator.Object));
            Assert.Throws<ArgumentNullException>(() => new Battle(_firstTrainer.Object, _secondTrainer.Object, null, _randomNumberGenerator.Object));
            Assert.Throws<ArgumentNullException>(() => new Battle(_firstTrainer.Object, _secondTrainer.Object, _typeEffectivenessService.Object, null));
        }

        [Fact]
        public void Battle_Initialization_WithNoValidPokemons_ThrowsInvalidOperationException()
        {
            _firstTrainer.Setup(t => t.Pokemons).Returns(Enumerable.Empty<IPokemon>().ToList());
            _secondTrainer.Setup(t => t.Pokemons).Returns(Enumerable.Empty<IPokemon>().ToList());

            Assert.Throws<InvalidOperationException>(() => new Battle(_firstTrainer.Object, _secondTrainer.Object, _typeEffectivenessService.Object, _randomNumberGenerator.Object));
        }

        [Fact]
        public void PerformAttack_WithFaintedAttacker_ThrowsInvalidMoveException()
        {
            var attacker = new Mock<IPokemon>();
            attacker.Setup(a => a.IsFainted()).Returns(true);
            _firstTrainer.Setup(t => t.CurrentPokemon).Returns(attacker.Object);

            var move = new Mock<IMove>();

            Assert.Throws<InvalidMoveException>(() => _battle.PerformAttack(move.Object));
        }

        [Fact]
        public void PerformAttack_WithInvalidMove_ThrowsInvalidMoveException()
        {
            var attacker = new Mock<IPokemon>();
            var defender = new Mock<IPokemon>();
            var move = new Mock<IMove>();

            attacker.Setup(a => a.IsFainted()).Returns(false);
            attacker.Setup(a => a.Moves).Returns(Enumerable.Empty<IMove>().ToList());
            _firstTrainer.Setup(t => t.CurrentPokemon).Returns(attacker.Object);
            _secondTrainer.Setup(t => t.CurrentPokemon).Returns(defender.Object);

            Assert.Throws<InvalidMoveException>(() => _battle.PerformAttack(move.Object));
        }

        [Fact]
        public void PerformAttack_ExecutesAttackSuccessfully()
        {
            var attacker = new Mock<IPokemon>();
            var defender = new Mock<IPokemon>();
            var move = new Mock<IMove>();
            move.Setup(m => m.Power).Returns(50);
            move.Setup(m => m.MaxHits).Returns(1);
            move.Setup(m => m.RecoilPercentage).Returns(10);
            move.Setup(m => m.HealingPercentage).Returns(10);

            attacker.Setup(a => a.IsFainted()).Returns(false);
            attacker.Setup(a => a.Moves).Returns(new List<IMove> { move.Object });
            defender.Setup(d => d.IsFainted()).Returns(false);

            _typeEffectivenessService.Setup(t => t.GetEffectiveness(It.IsAny<PokemonType>(), It.IsAny<PokemonType>())).Returns(1.0);
            _randomNumberGenerator.Setup(r => r.Generate(It.IsAny<double>(), It.IsAny<double>())).Returns(1.0);

            _battle.PerformAttack(move.Object);

            // Assertions
            attacker.Verify(a => a.TakeDamage(It.IsAny<int>()), Times.Once);
            defender.Verify(d => d.TakeDamage(It.IsAny<int>()), Times.Once);
            _logger.Verify(l => l.LogInfo(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void SwitchPokemon_WithValidConditions_SwitchesPokemon()
        {
            var trainer = new Mock<ITrainer>();
            var newPokemon = new Mock<IPokemon>();

            trainer.Setup(t => t.Pokemons).Returns(new List<IPokemon> { newPokemon.Object });
            trainer.Setup(t => t.CurrentPokemon).Returns(newPokemon.Object);

            _battle.SwitchPokemon(trainer.Object, newPokemon.Object);

            trainer.VerifySet(t => t.CurrentPokemon = newPokemon.Object, Times.Once);
            _logger.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SwitchPokemon_WithInvalidConditions_ThrowsInvalidPokemonSwitchException()
        {
            var trainer = new Mock<ITrainer>();
            var newPokemon = new Mock<IPokemon>();

            trainer.Setup(t => t.Pokemons).Returns(Enumerable.Empty<IPokemon>().ToList());

            Assert.Throws<InvalidPokemonSwitchException>(() => _battle.SwitchPokemon(trainer.Object, newPokemon.Object));
        }

        [Fact]
        public void DetermineBattleResult_WhenAttackerFainted_ReturnsDefenderWinMessage()
        {
            var attacker = new Mock<IPokemon>();
            var defender = new Mock<IPokemon>();

            attacker.Setup(a => a.IsFainted()).Returns(true);
            defender.Setup(d => d.IsFainted()).Returns(false);
            _firstTrainer.Setup(t => t.CurrentPokemon).Returns(attacker.Object);
            _secondTrainer.Setup(t => t.CurrentPokemon).Returns(defender.Object);

            var result = _battle.DetermineBattleResult();

            Assert.Equal($"{_firstTrainer.Object.Name}'s {attacker.Object.Name} has fainted. {_secondTrainer.Object.Name} wins!", result);
        }

        [Fact]
        public void DetermineBattleResult_WhenDefenderFainted_ReturnsAttackerWinMessage()
        {
            var attacker = new Mock<IPokemon>();
            var defender = new Mock<IPokemon>();

            attacker.Setup(a => a.IsFainted()).Returns(false);
            defender.Setup(d => d.IsFainted()).Returns(true);
            _firstTrainer.Setup(t => t.CurrentPokemon).Returns(attacker.Object);
            _secondTrainer.Setup(t => t.CurrentPokemon).Returns(defender.Object);

            var result = _battle.DetermineBattleResult();

            Assert.Equal($"{_secondTrainer.Object.Name}'s {defender.Object.Name} has fainted. {_firstTrainer.Object.Name} wins!", result);
        }

        [Fact]
        public void DetermineBattleResult_WhenNoPokemonFainted_ReturnsOngoingMessage()
        {
            var attacker = new Mock<IPokemon>();
            var defender = new Mock<IPokemon>();

            attacker.Setup(a => a.IsFainted()).Returns(false);
            defender.Setup(d => d.IsFainted()).Returns(false);
            _firstTrainer.Setup(t => t.CurrentPokemon).Returns(attacker.Object);
            _secondTrainer.Setup(t => t.CurrentPokemon).Returns(defender.Object);

            var result = _battle.DetermineBattleResult();

            Assert.Equal("The battle is ongoing.", result);
        }
    }
}
