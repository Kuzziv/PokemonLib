using Xunit;
using System;
using PokemonGameLib.Models;

namespace PokemonGameLib.Tests
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.Equal(137, charizard.HP); // Verify based on your actual formula
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(flyingPokemon);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.Equal(67, flyingPokemon.HP); // Expected value after applying 2.0 effectiveness
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(grassPokemon);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.Equal(92, grassPokemon.HP); // Expected value after applying 0.5 effectiveness
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(groundPokemon);

            var battle = new Battle(ash, brock);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.Equal(100, groundPokemon.HP); // No effect should leave HP unchanged
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.PerformAttack(fireBlast));
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);
            battle.PerformAttack(thunderbolt); // Attack to make Charizard faint

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Brock's Pokémon have all fainted. Ash wins!", result);
        }

        [Fact]
        public void TestDetermineBattleResult_DefenderWins()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 15, 10, 55, 40); // Pikachu has very low HP
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 15, 100, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var flamethrower = new Move("Flamethrower", PokemonType.Fire, 90000, 15);
            charizard.AddMove(flamethrower);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);

            var brocky = new Trainer("Brocky");
            brocky.AddPokemon(charizard);

            var battle = new Battle(ash, brocky);
            battle.PerformAttack(thunderbolt); 
            battle.PerformAttack(flamethrower); // Attack to make Pikachu faint

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

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

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
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 0, 55, 40); // Fainted
            ash.AddPokemon(pikachu);

            var brock = new Trainer("Brock");
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);
            brock.AddPokemon(charizard);

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

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

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

            var brock = new Trainer("Brock"); // Brock has no Pokémon

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new Battle(ash, brock));
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

            var brock = new Trainer("Brock");
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            battle.SwitchPokemon(ash, bulbasaur);

            // Assert
            Assert.Contains(bulbasaur, ash.Pokemons);
            Assert.DoesNotContain(pikachu, ash.Pokemons);
            Assert.Equal(bulbasaur, battle.Attacker);
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

            var brock = new Trainer("Brock");
            brock.AddPokemon(charmander);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => battle.SwitchPokemon(ash, charmander));
        }

        [Fact]
        public void TestSwitchPokemon_FaintedCurrentPokemon()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 0, 55, 40); // Fainted
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 10, 100, 49, 49);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var vineWhip = new Move("Vine Whip", PokemonType.Grass, 45, 10);

            pikachu.AddMove(thunderbolt);
            bulbasaur.AddMove(vineWhip);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);
            ash.AddPokemon(bulbasaur);

            var brock = new Trainer("Brock");
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.SwitchPokemon(ash, bulbasaur));
        }

        [Fact]
        public void TestSwitchPokemon_WithOnlyOnePokemon()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 15, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 15, 100, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90000, 10);
            pikachu.AddMove(thunderbolt);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu); // Only Pikachu available

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);

            // Act
            // Perform an attack and make sure we handle the case where there's only one Pokémon
            battle.PerformAttack(thunderbolt); 
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Brock's Pokémon have all fainted. Ash wins!", result);
        }


        
    }
}
