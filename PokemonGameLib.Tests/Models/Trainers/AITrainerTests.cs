using System;
using System.Linq;
using Xunit;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Tests.Models.Trainers
{
    public class AITrainerTests
    {
        [Fact]
        public void TakeTurn_PerformsAttack_WhenNoSwitchOrItemIsNeeded()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            pikachu.Moves.Add(thunderbolt);

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(pikachu);
            aiTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            aiTrainer.TakeTurn(battle);

            // Assert
            Assert.True(charmander.CurrentHP < 100, "Charmander's HP should have decreased after Pikachu's attack.");
        }

        [Fact]
        public void ShouldSwitchPokemon_ReturnsTrue_WhenOpponentIsTooStrong()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 100, 100, 84, 78); // Stronger than Pikachu

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(pikachu);
            aiTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charizard);
            opponentTrainer.SwitchPokemon(charizard);

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            var shouldSwitch = aiTrainer.ShouldSwitchPokemon(battle);

            // Assert
            Assert.True(shouldSwitch, "AI Trainer should switch when opponent is significantly stronger.");
        }

        [Fact]
        public void SelectBestMove_ReturnsMostEffectiveMove()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
            var quickAttack = new Move("Quick Attack", PokemonType.Normal, 40, 10);
            pikachu.Moves.Add(thunderbolt);
            pikachu.Moves.Add(quickAttack);

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(pikachu);
            aiTrainer.SwitchPokemon(pikachu);

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.SwitchPokemon(charmander);

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            var bestMove = aiTrainer.SelectBestMove(battle);

            // Assert
            Assert.Equal(thunderbolt, bestMove);
        }
    }
}
