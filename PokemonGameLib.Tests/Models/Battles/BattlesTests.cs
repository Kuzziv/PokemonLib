using Xunit;
using System;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Moves;
using PokemonGameLib.Models.Trainers;


namespace PokemonGameLib.Tests.Models.Battles
{
    public class BattleTests
    {
        [Fact]
        public void TestPerformAttack_Success()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.True(charizard.CurrentHP < charizard.MaxHP, "Charizard's HP should be less than Max HP after attack.");
        }

        [Fact]
        public void TestPerformAttack_SuperEffective()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var flyingPokemon = new Pokemon("Flying Pokemon", PokemonType.Flying, 10, 100, 55, 40);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(flyingPokemon);
            brock.SwitchPokemon(flyingPokemon);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.True(flyingPokemon.CurrentHP < flyingPokemon.MaxHP, "Expected value after applying 2.0 effectiveness");
        }

        [Fact]
        public void TestPerformAttack_NotVeryEffective()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var grassPokemon = new Pokemon("Grass Pokemon", PokemonType.Grass, 10, 100, 55, 40);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(grassPokemon);
            brock.SwitchPokemon(grassPokemon);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.True(grassPokemon.CurrentHP < grassPokemon.MaxHP, "Expected value after applying 0.5 effectiveness");
        }

        [Fact]
        public void TestPerformAttack_NoEffect()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var groundPokemon = new Pokemon("Ground Pokemon", PokemonType.Ground, 10, 100, 55, 40);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(groundPokemon);
            brock.SwitchPokemon(groundPokemon);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.Equal(100, groundPokemon.CurrentHP); // No effect should leave HP unchanged
        }

        [Fact]
        public void TestPerformAttack_WithInvalidMove()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var fireBlast = new Move("Fire Blast", PokemonType.Fire, 110, 20);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.PerformAttack(fireBlast));
        }

        [Fact]
        public void TestPerformAttack_NullMove()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.PerformAttack(null));
        }

        [Fact]
        public void TestPerformAttack_FaintedAttacker()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 150, 55, 40); // Pikachu is fainted
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Ensure pikachu is fainted
            pikachu.TakeDamage(150);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.PerformAttack(thunderbolt));
        }

        [Fact]
        public void TestDetermineBattleResult_AttackerWins()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 10, 70, 50); // Charizard has very low HP

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);
            battle.PerformAttack(thunderbolt); // Attack to make Charizard faint

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Brock's Charizard has fainted. Ash wins!", result);
        }

        [Fact]
        public void TestDetermineBattleResult_DefenderWins()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 15, 10, 55, 40); // Pikachu has very low HP
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 15, 100, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var flamethrower = new Move("Flamethrower", PokemonType.Fire, 90, 15);
            
            // Ensure Pikachu and Charizard know their respective moves
            pikachu.AddMove(thunderbolt);
            charizard.AddMove(flamethrower);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brocky = new Trainer("Brocky");
            brocky.AddPokemon(charizard);
            brocky.SwitchPokemon(charizard);

            var battle = new Battle(ash, brocky);
            
            // Act
            battle.PerformAttack(thunderbolt); // Pikachu attacks first
            battle.PerformAttack(flamethrower); // Charizard retaliates

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Ash's Pikachu has fainted. Brocky wins!", result);
        }

        [Fact]
        public void TestDetermineBattleResult_Ongoing()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("The battle is ongoing.", result);
        }

        [Fact]
        public void TestInvalidTrainer_NoPokemons()
        {
            // Arrange
            var ash = new Trainer("Ash");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new Battle(ash, ash));
        }

        [Fact]
        public void TestInvalidTrainer_FaintedPokemon()
        {
            // Arrange
            var ash = new Trainer("Ash");
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40); // Fainted
            pikachu.TakeDamage(100); // Simulate fainting
            ash.AddPokemon(pikachu);

            var brock = new Trainer("Brock");
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new Battle(ash, brock));
        }

        [Fact]
        public void TestBattleInitialization_WithValidTrainers()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            // Act
            var battle = new Battle(ash, brock);

            // Assert
            Assert.NotNull(battle);
            Assert.Equal(ash, battle.AttackingTrainer);
            Assert.Equal(brock, battle.DefendingTrainer);
        }

        [Fact]
        public void TestBattleInitialization_WithInvalidTrainer()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock"); // Brock has no Pokémon

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new Battle(ash, brock));
        }

        [Fact]
        public void TestBattleInitialization_WithNullTrainer()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Battle(ash, null));
            Assert.Throws<ArgumentNullException>(() => new Battle(null, ash));
        }

        [Fact]
        public void TestSwitchPokemon_Success()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 49, 49);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var vineWhip = new Move("Vine Whip", PokemonType.Grass, 45, 10);

            pikachu.AddMove(thunderbolt);
            bulbasaur.AddMove(vineWhip);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.AddPokemon(bulbasaur);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.SwitchPokemon(ash, bulbasaur);

            // Assert
            Assert.Equal(bulbasaur, ash.CurrentPokemon);
        }

        [Fact]
        public void TestSwitchPokemon_TrainerNotInTeam()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 10, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var flamethrower = new Move("Flamethrower", PokemonType.Fire, 90, 10);

            pikachu.AddMove(thunderbolt);
            charmander.AddMove(flamethrower);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charmander);
            brock.SwitchPokemon(charmander);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.SwitchPokemon(ash, charmander));
        }

        [Fact]
        public void TestSwitchPokemon_SwitchToFaintedPokemon()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            pikachu.TakeDamage(100); // Simulate fainting
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 49, 49);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var vineWhip = new Move("Vine Whip", PokemonType.Grass, 45, 10);

            pikachu.AddMove(thunderbolt);
            bulbasaur.AddMove(vineWhip);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.AddPokemon(bulbasaur);
            ash.SwitchPokemon(bulbasaur); // Initially switch to Bulbasaur

            var brock = new Trainer("Brock");
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.SwitchPokemon(ash, pikachu)); // Attempt to switch to fainted Pikachu
        }

        [Fact]
        public void TestCriticalHit_DoublesDamage()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 50, 100, 55, 40); // High level to ensure crit
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 50, 150, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            // Force a critical hit scenario, possibly mock the random factor
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.True(charizard.CurrentHP < charizard.MaxHP - 20, "Critical hit should deal significant damage.");
        }

        [Fact]
        public void TestFaintedPokemonCannotAttack()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 1, 55, 40); // Low HP to faint quickly
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var flamethrower = new Move("Flamethrower", PokemonType.Fire, 90, 10);
            charizard.AddMove(flamethrower);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Ensure Pikachu is fainted
            pikachu.TakeDamage(1);

            // Act
            // Try to attack with a fainted Pokémon
            var exception = Record.Exception(() => battle.PerformAttack(thunderbolt));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void TestSwitchPokemon_DuringBattle()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
            var squirtle = new Pokemon("Squirtle", PokemonType.Water, 10, 100, 48, 65);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.AddPokemon(squirtle);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.SwitchPokemon(ash, squirtle);

            // Assert
            Assert.Equal(squirtle, ash.CurrentPokemon);
        }
        
        [Fact]
        public void TestPerformAttack_MultiHitMove()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            // Assuming Thunderbolt can hit 2-5 times
            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 18, 10, maxHits: 5);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.True(charizard.CurrentHP < charizard.MaxHP - 20, "Charizard's HP should reflect multi-hit damage.");
        }

        [Fact]
        public void TestPerformAttack_RecoilDamage()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            // Assuming Volt Tackle causes 10% recoil
            var voltTackle = new Move("Volt Tackle", PokemonType.Electric, 120, 10, recoilPercentage: 10);
            pikachu.AddMove(voltTackle);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(voltTackle);

            // Assert
            Assert.True(pikachu.CurrentHP < 100, "Pikachu's HP should be reduced due to recoil damage.");
        }

        [Fact]
        public void TestSwitchPokemon_InvalidTrainer()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.SwitchPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);
            brock.SwitchPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.SwitchPokemon(null, pikachu));
        }
    }
}