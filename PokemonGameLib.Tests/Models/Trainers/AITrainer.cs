using System.Collections.Generic;
using Moq;
using Xunit;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;

namespace PokemonGameLib.Tests.Models.Trainers
{
    public class AITrainerTests
    {
        private readonly AITrainer _aiTrainer;
        private readonly Mock<IBattle> _mockBattle;
        private readonly Mock<IPokemon> _mockCurrentPokemon;
        private readonly Mock<IPokemon> _mockOpponentPokemon;
        private readonly Mock<ITypeEffectivenessService> _mockTypeEffectivenessService;

        public AITrainerTests()
        {
            _aiTrainer = new AITrainer("AI Trainer");
            _mockBattle = new Mock<IBattle>();
            _mockCurrentPokemon = new Mock<IPokemon>();
            _mockOpponentPokemon = new Mock<IPokemon>();
            _mockTypeEffectivenessService = new Mock<ITypeEffectivenessService>();
        }

        [Fact]
        public void TakeTurn_ShouldUseItem_WhenHPLow()
        {
            // Arrange
            var potion = new Potion("Health Potion", "Restores 50 HP", 50);
            _aiTrainer.AddItem(potion);
            _mockCurrentPokemon.Setup(p => p.CurrentHP).Returns(30);
            _mockCurrentPokemon.Setup(p => p.MaxHP).Returns(100);
            _aiTrainer.SwitchPokemon(_mockCurrentPokemon.Object);

            // Act
            _aiTrainer.TakeTurn(_mockBattle.Object);

            // Assert
            _mockBattle.Verify(b => b.PerformAttack(It.IsAny<IMove>()), Times.Never);
            _mockBattle.Verify(b => b.SwitchPokemon(It.IsAny<ITrainer>(), It.IsAny<IPokemon>()), Times.Never);
        }

        [Fact]
        public void TakeTurn_ShouldSwitchPokemon_WhenEffective()
        {
            // Arrange
            var effectivePokemon = new Mock<IPokemon>();
            effectivePokemon.Setup(p => p.Name).Returns("Effective Pokemon");
            effectivePokemon.Setup(p => p.IsFainted()).Returns(false);
            effectivePokemon.Setup(p => p.Type).Returns(PokemonType.Fire);

            var ineffectivePokemon = new Mock<IPokemon>();
            ineffectivePokemon.Setup(p => p.Name).Returns("Ineffective Pokemon");
            ineffectivePokemon.Setup(p => p.IsFainted()).Returns(false);
            ineffectivePokemon.Setup(p => p.Type).Returns(PokemonType.Water);

            _mockCurrentPokemon.Setup(p => p.Type).Returns(PokemonType.Electric);
            _mockOpponentPokemon.Setup(p => p.Type).Returns(PokemonType.Ground);
            
            _mockTypeEffectivenessService.Setup(s => s.GetEffectiveness(PokemonType.Electric, PokemonType.Ground)).Returns(2.0);

            _aiTrainer.AddPokemon(ineffectivePokemon.Object);
            _aiTrainer.AddPokemon(effectivePokemon.Object);
            _aiTrainer.SwitchPokemon(_mockCurrentPokemon.Object);

            var mockDefendingTrainer = new Mock<ITrainer>();
            mockDefendingTrainer.Setup(t => t.CurrentPokemon).Returns(_mockOpponentPokemon.Object);
            _mockBattle.Setup(b => b.DefendingTrainer).Returns(mockDefendingTrainer.Object);

            // Act
            _aiTrainer.TakeTurn(_mockBattle.Object);

            // Assert
            _mockBattle.Verify(b => b.SwitchPokemon(_aiTrainer, effectivePokemon.Object), Times.Once);
            _mockBattle.Verify(b => b.PerformAttack(It.IsAny<IMove>()), Times.Never);
        }


        [Fact]
        public void TakeTurn_ShouldSelectBestMove()
        {
            // Arrange
            var bestMove = new Mock<IMove>();
            bestMove.Setup(m => m.Name).Returns("Thunderbolt");
            bestMove.Setup(m => m.Power).Returns(90);

            _mockCurrentPokemon.Setup(p => p.Moves).Returns(new List<IMove> { bestMove.Object });
            _mockCurrentPokemon.Setup(p => p.Type).Returns(PokemonType.Electric);
            _mockOpponentPokemon.Setup(p => p.Type).Returns(PokemonType.Water);

            _mockTypeEffectivenessService.Setup(s => s.GetEffectiveness(PokemonType.Electric, PokemonType.Water)).Returns(2.0);

            _aiTrainer.SwitchPokemon(_mockCurrentPokemon.Object);
            var mockDefendingTrainer = new Mock<ITrainer>();
            mockDefendingTrainer.Setup(t => t.CurrentPokemon).Returns(_mockOpponentPokemon.Object);
            _mockBattle.Setup(b => b.DefendingTrainer).Returns(mockDefendingTrainer.Object);

            // Act
            _aiTrainer.TakeTurn(_mockBattle.Object);

            // Assert
            _mockBattle.Verify(b => b.PerformAttack(bestMove.Object), Times.Once);
            _mockBattle.Verify(b => b.SwitchPokemon(It.IsAny<ITrainer>(), It.IsAny<IPokemon>()), Times.Never);
        }

        [Fact]
        public void ShouldSwitchPokemon_ReturnsTrue_WhenEffective()
        {
            // Arrange
            _mockCurrentPokemon.Setup(p => p.Type).Returns(PokemonType.Electric);
            _mockOpponentPokemon.Setup(p => p.Type).Returns(PokemonType.Ground);

            _mockTypeEffectivenessService.Setup(s => s.GetEffectiveness(PokemonType.Electric, PokemonType.Ground)).Returns(2.0);

            _aiTrainer.SwitchPokemon(_mockCurrentPokemon.Object);

            // Act
            var result = _aiTrainer.ShouldSwitchPokemon(_mockBattle.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldSwitchPokemon_ReturnsFalse_WhenNotEffective()
        {
            // Arrange
            _mockCurrentPokemon.Setup(p => p.Type).Returns(PokemonType.Electric);
            _mockOpponentPokemon.Setup(p => p.Type).Returns(PokemonType.Electric);

            _mockTypeEffectivenessService.Setup(s => s.GetEffectiveness(PokemonType.Electric, PokemonType.Electric)).Returns(1.0);

            _aiTrainer.SwitchPokemon(_mockCurrentPokemon.Object);

            // Act
            var result = _aiTrainer.ShouldSwitchPokemon(_mockBattle.Object);

            // Assert
            Assert.False(result);
        }
    }
}
