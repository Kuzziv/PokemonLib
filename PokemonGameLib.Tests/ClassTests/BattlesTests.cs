using Xunit;
using System;
using System.Collections.Generic;

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

            var battle = new Battle(pikachu, charizard);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            Assert.Equal(60, charizard.HP); // Thunderbolt should deal 90 damage, Charizard should have 60 HP left
        }

        [Fact]
        public void TestPerformAttack_WithFaintedDefender()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 0, 70, 50); // Charizard is already fainted

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var battle = new Battle(pikachu, charizard);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => battle.PerformAttack(thunderbolt));
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
            var battle = new Battle(pikachu, charizard);

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

            var battle = new Battle(pikachu, charizard);
            battle.PerformAttack(thunderbolt);

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Charizard has fainted. Pikachu wins!", result);
        }

        [Fact]
        public void TestDetermineBattleResult_DefenderWins()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 10, 55, 40); // Pikachu has very low HP
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var battle = new Battle(pikachu, charizard);
            battle.PerformAttack(thunderbolt);

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("Pikachu has fainted. Charizard wins!", result);
        }

        [Fact]
        public void TestDetermineBattleResult_OngoingBattle()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 100, 70, 50);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.AddMove(thunderbolt);

            var battle = new Battle(pikachu, charizard);
            battle.PerformAttack(thunderbolt);

            // Act
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Equal("The battle is ongoing.", result);
        }
    }
}
