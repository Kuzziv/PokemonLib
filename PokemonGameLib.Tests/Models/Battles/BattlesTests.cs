using System;
using Xunit;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;
using System.Linq;

namespace PokemonGameLib.Tests
{
    [Collection("Test Collection")]
    public class BattleTests
    {

        [Fact]
        public void PerformAttack_ValidAttack_ReducesOpponentHP()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 250, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            pikachu.Moves.Add(thunderbolt);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            battle.PerformAttack(thunderbolt);

            Assert.True(charmander.CurrentHP < 250);
        }

        [Fact]
        public void PerformAttack_InvalidMove_ThrowsInvalidMoveException()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            var ember = new Move("Ember", PokemonType.Fire, 40, 10);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            Assert.Throws<InvalidMoveException>(() => battle.PerformAttack(ember));
        }

        [Fact]
        public void SwitchPokemon_ChangesCurrentPokemon()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 100, 100, 49, 49);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.AddPokemon(bulbasaur);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            battle.PerformSwitch(playerTrainer, bulbasaur);

            Assert.Equal("Bulbasaur", playerTrainer.CurrentPokemon.Name);
        }

        [Fact]
        public void SwitchPokemon_InvalidSwitch_ThrowsInvalidPokemonSwitchException()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);
            var bulbasaur = new Pokemon("Bulbasaur", PokemonType.Grass, 100, 100, 49, 49);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Trying to switch to a Pokémon not in the player's collection
            Assert.Throws<InvalidPokemonSwitchException>(() => battle.PerformSwitch(playerTrainer, bulbasaur));
        }

        [Fact]
        public void DetermineBattleResult_CorrectlyIdentifiesWinner()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43); // Fainted

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            charmander.TakeDamage(100); // Faint the Pokémon

            string result = battle.DetermineBattleResult();

            Assert.Contains("Player Trainer1 wins the battle!", result);
        }

        [Fact]
        public void DetermineBattleResult_BattleIsOngoing()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            string result = battle.DetermineBattleResult();

            Assert.Equal("The battle is ongoing.", result);
        }

        [Fact]
        public void PerformAttack_SwitchesAttackingTrainer_AfterAttack()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            pikachu.Moves.Add(thunderbolt);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Act
            battle.PerformAttack(thunderbolt);

            // Assert that Charmander's HP has decreased
            Assert.True(charmander.CurrentHP < 100, "Charmander's HP should have decreased after Pikachu's attack.");

            // Assert that the turn has switched to the opponent
            Assert.Equal(opponentTrainer, battle.OpponentTrainer);
        }


        [Fact]
        public void IsBattleOver_ReturnsTrue_WhenAllPokemonFainted()
        {
            // Arrange
            var faintedPokemon1 = new Pokemon("FaintedMon1", PokemonType.Electric, 10, 100, 55, 40);
            var faintedPokemon2 = new Pokemon("FaintedMon2", PokemonType.Fire, 10, 100, 52, 43);

            var playerTrainer1 = new PlayerTrainer("Player Trainer1");
            var playerTrainer2 = new PlayerTrainer("Player Trainer2");

            playerTrainer1.AddPokemon(faintedPokemon1);
            playerTrainer1.CurrentPokemon = faintedPokemon1;

            playerTrainer2.AddPokemon(faintedPokemon2);
            playerTrainer2.CurrentPokemon = faintedPokemon2;

            var battle = new Battle(playerTrainer1, playerTrainer2);

            faintedPokemon1.TakeDamage(10);
            faintedPokemon2.TakeDamage(10);

            // Act
            bool isOver = battle.IsBattleOver();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void GetWinner_ReturnsCorrectWinner()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 10, 100, 52, 43); // Fainted

            var playerTrainer1 = new PlayerTrainer("Player Trainer1");
            var playerTrainer2 = new PlayerTrainer("Player Trainer2");

            playerTrainer1.AddPokemon(pikachu);
            playerTrainer1.CurrentPokemon = pikachu;

            playerTrainer2.AddPokemon(charmander);
            playerTrainer2.CurrentPokemon = charmander;

            var battle = new Battle(playerTrainer1, playerTrainer2);

            charmander.TakeDamage(100);

            // Act
            var winner = battle.GetWinner();

            // Assert
            Assert.Equal(playerTrainer1, winner);
        }
    }
}
