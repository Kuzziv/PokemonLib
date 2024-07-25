using Xunit;
using System;
using PokemonGameLib.Models;
using System.Runtime.CompilerServices;

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
            battle.PerformAttack(ash, thunderbolt);

            // Assert
            Assert.Equal(60, charizard.HP); // Thunderbolt should deal 90 damage, Charizard should have 60 HP left
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
            Assert.Throws<InvalidOperationException>(() => battle.PerformAttack(ash, fireBlast));
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
            battle.PerformAttack(ash, thunderbolt);

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

            var flamethrower = new Move("Flamethrower", PokemonType.Fire, 90, 15);
            charizard.AddMove(flamethrower);

            var ash = new Trainer("Ash");
            ash.AddPokemon(pikachu);

            var brock = new Trainer("Brock");
            brock.AddPokemon(charizard);

            var battle = new Battle(ash, brock);
            battle.PerformAttack(brock, flamethrower);

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Ash's Pikachu has fainted. Brock wins!", result);
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
    }
}
