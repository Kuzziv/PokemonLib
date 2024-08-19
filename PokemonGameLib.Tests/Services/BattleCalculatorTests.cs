using System;
using Xunit;
using Moq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Services;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Tests.Services
{
    public class BattleCalculatorTests
    {
        private readonly Mock<IPokemon> _mockAttacker;
        private readonly Mock<IPokemon> _mockDefender;
        private readonly Mock<IMove> _mockMove;
        private readonly Mock<RandomNumberGenerator> _mockRandomNumberGenerator;
        private readonly BattleCalculator _battleCalculator;

        public BattleCalculatorTests()
        {
            _mockAttacker = new Mock<IPokemon>();
            _mockDefender = new Mock<IPokemon>();
            _mockMove = new Mock<IMove>();
            _mockRandomNumberGenerator = new Mock<RandomNumberGenerator>();

            _battleCalculator = new BattleCalculator();
        }

        [Fact]
        public void CalculateDamage_ValidInputs_ReturnsExpectedDamage()
        {
            // Arrange
            _mockAttacker.Setup(a => a.Level).Returns(50);
            _mockAttacker.Setup(a => a.Attack).Returns(100);
            _mockAttacker.Setup(a => a.Type).Returns(PokemonType.Electric);
            _mockAttacker.Setup(a => a.Name).Returns("Pikachu");

            _mockDefender.Setup(d => d.Defense).Returns(80);
            _mockDefender.Setup(d => d.Type).Returns(PokemonType.Water);
            _mockDefender.Setup(d => d.Name).Returns("Squirtle");

            _mockMove.Setup(m => m.Power).Returns(90);
            _mockMove.Setup(m => m.Type).Returns(PokemonType.Electric);
            _mockMove.Setup(m => m.Name).Returns("Thunderbolt");

            // Instead of mocking Generate, use a deterministic value in the calculator
            int damage = _battleCalculator.CalculateDamage(_mockAttacker.Object, _mockDefender.Object, _mockMove.Object);

            // Assert
            Assert.True(damage > 0, "Damage should be greater than zero.");
        }

        [Fact]
        public void CalculateRecoilDamage_ValidMove_ReturnsExpectedRecoil()
        {
            // Arrange
            _mockMove.Setup(m => m.Power).Returns(100);
            _mockMove.Setup(m => m.RecoilPercentage).Returns(25); // 25% recoil

            // Act
            int recoilDamage = _battleCalculator.CalculateRecoilDamage(_mockAttacker.Object, _mockMove.Object);

            // Assert
            Assert.Equal(25, recoilDamage);
        }

        [Fact]
        public void GetEffectivenessMessage_SuperEffective_ReturnsCorrectMessage()
        {
            // Act
            string message = _battleCalculator.GetEffectivenessMessage(2.0);

            // Assert
            Assert.Equal("It's super effective!", message);
        }

        [Fact]
        public void CalculateHealingAmount_ValidMove_ReturnsExpectedHealing()
        {
            // Arrange
            _mockAttacker.Setup(a => a.MaxHP).Returns(200);
            _mockAttacker.Setup(a => a.Name).Returns("Pikachu");
            _mockMove.Setup(m => m.HealingPercentage).Returns(50); // 50% healing

            // Act
            int healingAmount = _battleCalculator.CalculateHealingAmount(_mockAttacker.Object, _mockMove.Object);

            // Assert
            Assert.Equal(100, healingAmount);
        }

        [Fact]
        public void CalculateDamage_ThrowsArgumentNullException_WhenAttackerIsNull()
        {
            // Arrange
            var defender = _mockDefender.Object;
            var move = _mockMove.Object;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _battleCalculator.CalculateDamage(null, defender, move));
        }

        [Fact]
        public void CalculateDamage_ThrowsArgumentNullException_WhenDefenderIsNull()
        {
            // Arrange
            var attacker = _mockAttacker.Object;
            var move = _mockMove.Object;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _battleCalculator.CalculateDamage(attacker, null, move));
        }

        [Fact]
        public void CalculateDamage_ThrowsArgumentNullException_WhenMoveIsNull()
        {
            // Arrange
            var attacker = _mockAttacker.Object;
            var defender = _mockDefender.Object;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _battleCalculator.CalculateDamage(attacker, defender, null));
        }

        private T CallPrivateMethod<T>(object obj, string methodName, params object[] parameters)
        {
            var methodInfo = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)methodInfo.Invoke(obj, parameters);
        }
    }

    public class MockTypeEffectivenessService : ITypeEffectivenessService
    {
        private readonly double _effectiveness;

        public MockTypeEffectivenessService(double effectiveness)
        {
            _effectiveness = effectiveness;
        }

        public double GetEffectiveness(PokemonType attackType, PokemonType defenseType)
        {
            return _effectiveness;
        }
    }
}
