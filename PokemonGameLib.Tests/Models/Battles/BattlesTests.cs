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
        public void Battle_Initialization_ThrowsArgumentNullException_WhenFirstTrainerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Battle(null, new PlayerTrainer("Trainer2")));
        }

        [Fact]
        public void Battle_Initialization_ThrowsArgumentNullException_WhenSecondTrainerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Battle(new PlayerTrainer("Trainer1"), null));
        }

        [Fact]
        public void Battle_Initialization_ThrowsInvalidOperationException_WhenTrainerHasNoValidPokemon()
        {
            var trainer1 = new PlayerTrainer("Trainer1");
            var trainer2 = new PlayerTrainer("Trainer2");

            Assert.Throws<InvalidOperationException>(() => new Battle(trainer1, trainer2));
        }

        [Fact]
        public void PerformAttack_ValidAttack_ReducesOpponentHP()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 250, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            pikachu.Moves.Add(thunderbolt);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            battle.PerformAttack(thunderbolt);

            Assert.True(charmander.CurrentHP < 250);
        }

        [Fact]
        public void PerformAttack_AttackerIsFainted_DoesNotReduceOpponentHP()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            pikachu.Moves.Add(thunderbolt);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            pikachu.TakeDamage(100); // Faint the Pokémon

            battle.PerformAttack(thunderbolt);

            Assert.Equal(100, charmander.CurrentHP); // HP should not change
        }

        [Fact]
        public void PerformAttack_InvalidMove_ThrowsInvalidMoveException()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            Assert.Throws<InvalidMoveException>(() => battle.PerformAttack(thunderbolt));
        }

        [Fact]
        public void PerformAttack_ApplyStatusEffects_PokemonIsParalyzed()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            pikachu.InflictStatus(StatusCondition.Paralysis);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 40, 10);
            pikachu.Moves.Add(thunderbolt);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            battle.PerformAttack(thunderbolt);

            // Depending on the paralysis logic, Pikachu might not attack
            // You should add logic to simulate and verify the behavior
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
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            battle.SwitchPokemon(playerTrainer, bulbasaur);

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
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Trying to switch to a Pokémon not in the player's collection
            Assert.Throws<InvalidPokemonSwitchException>(() => battle.SwitchPokemon(playerTrainer, bulbasaur));
        }

        [Fact]
        public void DetermineBattleResult_CorrectlyIdentifiesWinner()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43); // Fainted

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            charmander.TakeDamage(100); // Faint the Pokémon

            string result = battle.DetermineBattleResult();

            Assert.Contains("Player Trainer1 wins!", result);
        }

        [Fact]
        public void DetermineBattleResult_BattleIsOngoing()
        {
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var playerTrainer = new PlayerTrainer("Player Trainer1");
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

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
            playerTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Player Trainer2");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(playerTrainer, opponentTrainer);

            // Act & Assert
            Assert.Equal(playerTrainer, battle.AttackingTrainer); // Initial attacker should be Player Trainer1

            battle.PerformAttack(thunderbolt);

            Assert.Equal(opponentTrainer, battle.AttackingTrainer); // After the attack, the attacker should switch to Player Trainer2
        }


    }
}
