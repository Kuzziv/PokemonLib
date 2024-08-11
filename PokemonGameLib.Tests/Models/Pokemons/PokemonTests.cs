using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Utilities;
using Xunit;

namespace PokemonGameLib.Tests.Models.Pokemons
{
    [Collection("Test Collection")]
    public class PokemonTests
    {
        private readonly Mock<Logger> _loggerMock;
        private readonly Pokemon _pokemon;

        public PokemonTests()
        {
            _loggerMock = new Mock<Logger>();
            _pokemon = new Pokemon(
                name: "Pikachu",
                type: PokemonType.Electric,
                level: 5,
                maxHp: 35,
                attack: 55,
                defense: 40,
                abilities: new List<IAbility>(),
                evolutions: new List<IEvolution>()
            );
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenNameIsNullOrWhitespace()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Pokemon(null, PokemonType.Electric, 5, 35, 55, 40));
            Assert.Throws<ArgumentNullException>(() =>
                new Pokemon(" ", PokemonType.Electric, 5, 35, 55, 40));
        }

        [Fact]
        public void Constructor_ThrowsArgumentException_WhenTypeIsInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
                new Pokemon("Pikachu", (PokemonType)999, 5, 35, 55, 40));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenLevelIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Pokemon("Pikachu", PokemonType.Electric, 0, 35, 55, 40));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenMaxHpIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Pokemon("Pikachu", PokemonType.Electric, 5, 0, 55, 40));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenAttackIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Pokemon("Pikachu", PokemonType.Electric, 5, 35, -1, 40));
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenDefenseIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Pokemon("Pikachu", PokemonType.Electric, 5, 35, 55, -1));
        }

        [Fact]
        public void LevelUp_IncreasesLevelAndStats()
        {
            _pokemon.LevelUp();

            Assert.Equal(6, _pokemon.Level);
            Assert.Equal(45, _pokemon.MaxHP);
            Assert.Equal(60, _pokemon.Attack);
            Assert.Equal(45, _pokemon.Defense);
            Assert.Equal(45, _pokemon.CurrentHP);
        }

        [Fact]
        public void TakeDamage_UpdatesCurrentHP()
        {
            _pokemon.TakeDamage(10);
            Assert.Equal(25, _pokemon.CurrentHP);
        }

        [Fact]
        public void Heal_UpdatesCurrentHP()
        {
            _pokemon.TakeDamage(10);
            _pokemon.Heal(5);
            Assert.Equal(30, _pokemon.CurrentHP);
        }

        [Fact]
        public void LowerStat_UpdatesStats()
        {
            _pokemon.LowerStat("Attack", 10);
            Assert.Equal(45, _pokemon.Attack);
        }

        [Fact]
        public void IsFainted_ReturnsTrue_WhenCurrentHPIsZero()
        {
            _pokemon.TakeDamage(35);
            Assert.True(_pokemon.IsFainted());
        }

        [Fact]
        public void AddMove_ThrowsArgumentExceptionForIncompatibleMoveType()
        {
            // Arrange
            var moveMock = new Mock<IMove>();
            moveMock.Setup(m => m.Type).Returns(PokemonType.Water); // Assume Water is incompatible
            moveMock.Setup(m => m.Level).Returns(1);

            var pokemon = new Pokemon("Charmander", PokemonType.Fire, 5, 5, 5,5); // Assuming Type is set in the constructor

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pokemon.AddMove(moveMock.Object));
        }

        [Fact]
        public void AddAbility_ThrowsExceptionsForInvalidAbilities()
        {
            var abilityMock = new Mock<IAbility>();

            _pokemon.AddAbility(abilityMock.Object);
            Assert.Contains(abilityMock.Object, _pokemon.Abilities);

            Assert.Throws<ArgumentException>(() => _pokemon.AddAbility(abilityMock.Object));
        }

        [Fact]
        public void RemoveAbility_RemovesAbility()
        {
            var abilityMock = new Mock<IAbility>();
            _pokemon.AddAbility(abilityMock.Object);
            
            _pokemon.RemoveAbility(abilityMock.Object);
            Assert.DoesNotContain(abilityMock.Object, _pokemon.Abilities);
        }

        [Fact]
        public void InflictStatus_UpdatesStatusCondition()
        {
            _pokemon.InflictStatus(StatusCondition.Burn);
            Assert.Equal(StatusCondition.Burn, _pokemon.Status);
        }

        [Fact]
        public void ApplyStatusEffects_HandlesStatusConditions()
        {
            // Apply Burn status and check the HP after applying effects
            _pokemon.InflictStatus(StatusCondition.Burn);
            _pokemon.ApplyStatusEffects();
            Assert.Equal(33, _pokemon.CurrentHP); // Ensure burn damage is correctly applied
            
            // Cure the Burn status before inflicting Sleep
            _pokemon.CureStatus();
            Assert.Equal(StatusCondition.None, _pokemon.Status); // Ensure the status is reset
            
            // Apply Sleep status and check that it takes effect
            _pokemon.InflictStatus(StatusCondition.Sleep);
            _pokemon.SetSleepDuration(1);
            _pokemon.ApplyStatusEffects();
            Assert.Equal(StatusCondition.Sleep, _pokemon.Status); // Check that Sleep status is correctly applied
            
            // Advance the sleep counter and ensure the status changes back to None
            _pokemon.SetSleepDuration(0);
            _pokemon.ApplyStatusEffects();
            Assert.Equal(StatusCondition.None, _pokemon.Status); // Ensure the Pokémon wakes up
        }


        [Fact]
        public void CureStatus_ResetsStatusCondition()
        {
            _pokemon.InflictStatus(StatusCondition.Burn);
            _pokemon.CureStatus();
            Assert.Equal(StatusCondition.None, _pokemon.Status);
        }

        [Fact]
        public void SetSleepDuration_SetsSleepCounter()
        {
            // Use reflection to access protected field
            SetProtectedField(_pokemon, "_sleepCounter", 3);
            Assert.Equal(3, GetProtectedField<int>(_pokemon, "_sleepCounter"));
        }

        [Fact]
        public void CanEvolve_ReturnsTrue_WhenEvolutionCriteriaMet()
        {
            var evolutionMock = new Mock<IEvolution>();
            evolutionMock.Setup(e => e.CanEvolve(It.IsAny<Pokemon>())).Returns(true);
            _pokemon.Evolutions.Add(evolutionMock.Object);
            
            Assert.True(_pokemon.CanEvolve());
        }

        [Fact]
        public void Evolve_PerformsEvolution()
        {
            var evolutionMock = new Mock<IEvolution>();
            evolutionMock.Setup(e => e.CanEvolve(It.IsAny<Pokemon>())).Returns(true);
            evolutionMock.Setup(e => e.EvolvedFormName).Returns("Raichu");
            _pokemon.Evolutions.Add(evolutionMock.Object);

            _pokemon.Evolve();
            Assert.Equal("Raichu", _pokemon.Name);
            Assert.Equal(6, _pokemon.Level);
            Assert.Equal(55, _pokemon.MaxHP);
            Assert.Equal(65, _pokemon.Attack);
            Assert.Equal(50, _pokemon.Defense);
            Assert.Equal(55, _pokemon.CurrentHP);
        }

        private static T GetProtectedField<T>(object obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)fieldInfo.GetValue(obj);
        }

        private static void SetProtectedField(object obj, string fieldName, object value)
        {
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(obj, value);
        }
    }
}
