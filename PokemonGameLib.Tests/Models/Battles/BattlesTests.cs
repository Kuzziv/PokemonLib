using System;
using Xunit;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Tests
{
    [Collection("Test Collection")]
    public class BattleTests
    {
        [Fact]
        public void TestBattle_PerformAttack_DealsDamage()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 250, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            var quickAttack = new Move("Quick Attack", PokemonType.Normal, 40, 10);

            pikachu.Moves.Add(thunderbolt);
            charmander.Moves.Add(quickAttack);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert
            // the charmander's HP should be below 250
            Assert.True(charmander.CurrentHP < 250);
        }

        [Fact]
        public void TestBattle_SwitchPokemon_ChangesCurrentPokemon()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 100, 100, 49, 49);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.AddPokemon(bulbasaur);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Act
            battle.SwitchPokemon(playerTrainer, bulbasaur);

            // Assert
            Assert.Equal("Bulbasaur", playerTrainer.CurrentPokemon.Name);
        }

        [Fact]
        public void TestBattle_DetermineWinner_ReturnsCorrectWinner()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 10, 10, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var quickAttack = new Move("Quick Attack", PokemonType.Normal, 40, 10);

            pikachu.Moves.Add(thunderbolt);
            charmander.Moves.Add(quickAttack);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Act
            battle.PerformAttack(thunderbolt);
            var result = battle.DetermineBattleResult();

            // Assert
            Assert.Contains("Player Trainer1 wins!", result);
        }

    }
}
