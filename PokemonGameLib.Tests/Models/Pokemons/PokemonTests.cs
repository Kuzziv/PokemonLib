using Xunit;
using System;
using System.Collections.Generic;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Moves;
using PokemonGameLib.Models.Abilities;
using PokemonGameLib.Models.Evolutions;

namespace PokemonGameLib.Tests.Models.Pokemons
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
            Assert.Equal(100, pokemon.CurrentHP);
            Assert.Equal(55, pokemon.Attack);
            Assert.Equal(40, pokemon.Defense);
            Assert.Empty(pokemon.Moves);
            Assert.Empty(pokemon.Abilities);
            Assert.Empty(pokemon.Evolutions);
            Assert.Equal(StatusCondition.None, pokemon.Status);
        }

        [Fact]
        public void TestPokemonInitialization_WithAbilitiesAndEvolutions()
        {
            // Arrange
            var abilities = new List<Ability> { new Ability("Static", "May cause paralysis if hit by a move.") };
            var evolutions = new List<Evolution> { new Evolution("Raichu", 20) };

            // Act
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40, abilities, evolutions);

            // Assert
            Assert.Single(pokemon.Abilities);
            Assert.Single(pokemon.Evolutions);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TestPokemonInitialization_InvalidName_ShouldThrowException(string name)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Pokemon(name, PokemonType.Electric, 10, 100, 55, 40));
        }

        [Fact]
        public void TestPokemonInitialization_InvalidType_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Pokemon("Pikachu", (PokemonType)999, 10, 100, 55, 40));
        }

        [Fact]
        public void TestPokemonInitialization_InvalidLevel_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Pokemon("Pikachu", PokemonType.Electric, 0, 100, 55, 40));
        }

        [Fact]
        public void TestPokemonInitialization_InvalidMaxHP_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Pokemon("Pikachu", PokemonType.Electric, 10, 0, 55, 40));
        }

        [Fact]
        public void TestPokemonInitialization_InvalidAttack_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Pokemon("Pikachu", PokemonType.Electric, 10, 100, -1, 40));
        }

        [Fact]
        public void TestPokemonInitialization_InvalidDefense_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, -1));
        }

        [Fact]
        public void TestLevelUp()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.LevelUp();

            // Assert
            Assert.Equal(11, pokemon.Level);
            Assert.Equal(110, pokemon.MaxHP);
            Assert.Equal(110, pokemon.CurrentHP); // Healed after level up
            Assert.Equal(60, pokemon.Attack);
            Assert.Equal(45, pokemon.Defense);
        }

        [Fact]
        public void TestLevelUp_MaxHPOverflowProtection()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.TakeDamage(5);

            // Act
            pokemon.LevelUp();

            // Assert
            Assert.Equal(110, pokemon.MaxHP);
            Assert.Equal(105, pokemon.CurrentHP); // CurrentHP is min(CurrentHP + 10, MaxHP)
        }

        [Fact]
        public void TestTakeDamage()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.TakeDamage(30);

            // Assert
            Assert.Equal(70, pokemon.CurrentHP);
        }

        [Fact]
        public void TestTakeDamageShouldNotGoBelowZero()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 30, 55, 40);

            // Act
            pokemon.TakeDamage(40);

            // Assert
            Assert.Equal(0, pokemon.CurrentHP);
        }

        [Fact]
        public void TestTakeDamage_NegativeDamage_NoEffect()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.TakeDamage(-10);

            // Assert
            Assert.Equal(100, pokemon.CurrentHP); // Negative damage should have no effect
        }

        [Fact]
        public void TestHeal()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.TakeDamage(30); // Reduce HP to 70

            // Act
            pokemon.Heal(20);

            // Assert
            Assert.Equal(90, pokemon.CurrentHP);
        }

        [Fact]
        public void TestHeal_OverMaxHP_NoEffect()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.TakeDamage(20); // Reduce HP to 80

            // Act
            pokemon.Heal(30);

            // Assert
            Assert.Equal(100, pokemon.CurrentHP); // HP should not exceed max
        }

        [Fact]
        public void TestLowerStat()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.LowerStat("Attack", 5);
            pokemon.LowerStat("Defense", 10);

            // Assert
            Assert.Equal(50, pokemon.Attack);
            Assert.Equal(30, pokemon.Defense);
        }

        [Fact]
        public void TestLowerStat_InvalidStatName_NoEffect()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.LowerStat("Speed", 10);

            // Assert
            Assert.Equal(55, pokemon.Attack);
            Assert.Equal(40, pokemon.Defense); // No effect on invalid stat
        }

        [Fact]
        public void TestIsFainted()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.TakeDamage(100); // Simulate fainting
            var isFainted = pokemon.IsFainted();

            // Assert
            Assert.True(isFainted);
        }

        [Fact]
        public void TestIsFainted_NotFainted()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            var isFainted = pokemon.IsFainted();

            // Assert
            Assert.False(isFainted);
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
        public void TestAddMove_NullMove_ShouldThrowException()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => pokemon.AddMove(null));
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

        [Fact]
        public void TestInflictStatus()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            // Act
            pokemon.InflictStatus(StatusCondition.Poison);

            // Assert
            Assert.Equal(StatusCondition.Poison, pokemon.Status);
        }

        [Fact]
        public void TestInflictStatus_AlreadyAffected_NoEffect()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Burn);

            // Act
            pokemon.InflictStatus(StatusCondition.Poison);

            // Assert
            Assert.Equal(StatusCondition.Burn, pokemon.Status); // No change in status
        }

        [Fact]
        public void TestApplyStatusEffects_Burn()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Burn);

            // Act
            pokemon.ApplyStatusEffects();

            // Assert
            Assert.Equal(94, pokemon.CurrentHP); // Takes 6 damage from burn (100 / 16)
        }

        [Fact]
        public void TestApplyStatusEffects_Poison()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Poison);

            // Act
            pokemon.ApplyStatusEffects();

            // Assert
            Assert.Equal(88, pokemon.CurrentHP); // Takes 12 damage from poison (100 / 8)
        }

        [Fact]
        public void TestApplyStatusEffects_Paralysis()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Paralysis);

            // Act
            pokemon.ApplyStatusEffects();
            // Cannot assert random paralysis effect, but ensure no damage is taken
            Assert.Equal(100, pokemon.CurrentHP); 
        }

        [Fact]
        public void TestApplyStatusEffects_Sleep()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Sleep);
            pokemon.SetSleepDuration(2);

            // Act
            pokemon.ApplyStatusEffects(); // 1st turn asleep
            pokemon.ApplyStatusEffects(); // 2nd turn asleep

            // Assert
            Assert.Equal(StatusCondition.Sleep, pokemon.Status);
        }

        [Fact]
        public void TestApplyStatusEffects_Sleep_WakeUp()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Sleep);
            pokemon.SetSleepDuration(1);

            // Act
            pokemon.ApplyStatusEffects(); // 1st turn asleep
            pokemon.ApplyStatusEffects(); // Wake up

            // Assert
            Assert.Equal(StatusCondition.None, pokemon.Status);
        }

        [Fact]
        public void TestApplyStatusEffects_Freeze()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Freeze);

            // Act
            for (int i = 0; i < 5; i++) // Simulate multiple turns
            {
                pokemon.ApplyStatusEffects();
                if (pokemon.Status == StatusCondition.None)
                    break; // Pokemon has thawed out
            }

            // Assert
            Assert.True(pokemon.Status == StatusCondition.None || pokemon.Status == StatusCondition.Freeze);
        }

        [Fact]
        public void TestCureStatus()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Burn);

            // Act
            pokemon.CureStatus();

            // Assert
            Assert.Equal(StatusCondition.None, pokemon.Status);
        }

        [Fact]
        public void TestSetSleepDuration()
        {
            // Arrange
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pokemon.InflictStatus(StatusCondition.Sleep);

            // Act
            pokemon.SetSleepDuration(3);
            pokemon.ApplyStatusEffects(); // 1st turn asleep
            pokemon.ApplyStatusEffects(); // 2nd turn asleep
            pokemon.ApplyStatusEffects(); // 3rd turn asleep
            pokemon.ApplyStatusEffects(); // Wake up

            // Assert
            Assert.Equal(StatusCondition.None, pokemon.Status);
        }

        [Fact]
        public void TestCanEvolve()
        {
            // Arrange
            var evolutions = new List<Evolution> { new Evolution("Raichu", 20) };
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 20, 100, 55, 40, evolutions: evolutions);

            // Act
            var canEvolve = pokemon.CanEvolve();

            // Assert
            Assert.True(canEvolve);
        }

        [Fact]
        public void TestCanEvolve_CannotEvolve()
        {
            // Arrange
            var evolutions = new List<Evolution> { new Evolution("Raichu", 25) };
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 20, 100, 55, 40, evolutions: evolutions);

            // Act
            var canEvolve = pokemon.CanEvolve();

            // Assert
            Assert.False(canEvolve);
        }

        [Fact]
        public void TestEvolve()
        {
            // Arrange
            var evolutions = new List<Evolution> { new Evolution("Raichu", 20) };
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 19, 100, 55, 40, evolutions: evolutions);

            // Act
            pokemon.LevelUp();
            pokemon.Evolve();

            // Assert
            Assert.Equal("Raichu", pokemon.Name);
            Assert.Equal(21, pokemon.Level); // Level should increment during evolve
            Assert.Equal(130, pokemon.MaxHP); // MaxHP should increase
            Assert.Equal(130, pokemon.CurrentHP); // Fully healed
            Assert.Equal(70, pokemon.Attack); // Attack should increase
            Assert.Equal(55, pokemon.Defense); // Defense should increase
        }

        [Fact]
        public void TestEvolve_NoEligibleEvolution()
        {
            // Arrange
            var evolutions = new List<Evolution> { new Evolution("Raichu", 25) };
            var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 20, 100, 55, 40, evolutions: evolutions);

            // Act
            pokemon.Evolve();

            // Assert
            Assert.Equal("Pikachu", pokemon.Name); // No evolution should occur
        }
    }
}
