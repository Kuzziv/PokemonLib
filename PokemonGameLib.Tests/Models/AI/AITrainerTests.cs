using Xunit;
using PokemonGameLib.Models.AI;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Moves;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Models.Items;
using System.Collections.Generic;


namespace PokemonGameLib.Tests
{
    public class AITrainerTests
    {
        [Fact]
        public void AI_Should_Choose_Super_Effective_Move()
        {
            // Arrange
            var aiTrainer = new AITrainer("AI Trainer");
            var opponentPokemon = new Pokemon("Bulbasaur", PokemonType.Grass, 5, 30, 10, 10);
            var aiPokemon = new Pokemon("Charmander", PokemonType.Fire, 5, 30, 10, 10);

            var ember = new Move("Ember", PokemonType.Fire, 40, 5);
            var scratch = new Move("Scratch", PokemonType.Normal, 40, 5);
            aiPokemon.AddMove(ember);
            aiPokemon.AddMove(scratch);

            aiTrainer.AddPokemon(aiPokemon);
            aiTrainer.CurrentPokemon = aiPokemon;

            var opponentTrainer = new Trainer("Opponent");
            opponentTrainer.AddPokemon(opponentPokemon);
            opponentTrainer.CurrentPokemon = opponentPokemon;

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            aiTrainer.TakeTurn(battle);

            // Assert
            // Check if the AI selected Ember, which is super effective against Grass type
            Assert.Contains("Ember", aiTrainer.CurrentPokemon.Moves);
        }

        [Fact]
        public void AI_Should_Switch_Pokemon_When_Disadvantaged()
        {
            // Arrange
            var aiTrainer = new AITrainer("AI Trainer");
            var opponentPokemon = new Pokemon("Squirtle", PokemonType.Water, 5, 30, 10, 10);
            var aiPokemon1 = new Pokemon("Charmander", PokemonType.Fire, 5, 30, 10, 10);
            var aiPokemon2 = new Pokemon("Pikachu", PokemonType.Electric, 5, 30, 10, 10);

            aiTrainer.AddPokemon(aiPokemon1);
            aiTrainer.AddPokemon(aiPokemon2);
            aiTrainer.CurrentPokemon = aiPokemon1;

            var opponentTrainer = new Trainer("Opponent");
            opponentTrainer.AddPokemon(opponentPokemon);
            opponentTrainer.CurrentPokemon = opponentPokemon;

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            aiTrainer.TakeTurn(battle);

            // Assert
            // Check if the AI switched to Pikachu, which has a type advantage over Water
            Assert.Equal(aiPokemon2, aiTrainer.CurrentPokemon);
        }

        [Fact]
        public void AI_Should_Use_Strongest_Available_Move()
        {
            // Arrange
            var aiTrainer = new AITrainer("AI Trainer");
            var opponentPokemon = new Pokemon("Rattata", PokemonType.Normal, 5, 30, 10, 10);
            var aiPokemon = new Pokemon("Pikachu", PokemonType.Electric, 5, 30, 10, 10);

            var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 5);
            var quickAttack = new Move("Quick Attack", PokemonType.Normal, 40, 5);
            aiPokemon.AddMove(thunderbolt);
            aiPokemon.AddMove(quickAttack);

            aiTrainer.AddPokemon(aiPokemon);
            aiTrainer.CurrentPokemon = aiPokemon;

            var opponentTrainer = new Trainer("Opponent");
            opponentTrainer.AddPokemon(opponentPokemon);
            opponentTrainer.CurrentPokemon = opponentPokemon;

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            aiTrainer.TakeTurn(battle);

            // Assert
            // Check if the AI selected Thunderbolt, the stronger move
            Assert.Contains("Thunderbolt", aiTrainer.CurrentPokemon.Moves);
        }

        [Fact]
        public void AI_Should_Not_Switch_If_No_Better_Pokemon_Available()
        {
            // Arrange
            var aiTrainer = new AITrainer("AI Trainer");
            var opponentPokemon = new Pokemon("Squirtle", PokemonType.Water, 5, 30, 10, 10);
            var aiPokemon1 = new Pokemon("Charmander", PokemonType.Fire, 5, 30, 10, 10);
            var aiPokemon2 = new Pokemon("Charmander", PokemonType.Fire, 5, 30, 10, 10); // Same type

            aiTrainer.AddPokemon(aiPokemon1);
            aiTrainer.AddPokemon(aiPokemon2);
            aiTrainer.CurrentPokemon = aiPokemon1;

            var opponentTrainer = new Trainer("Opponent");
            opponentTrainer.AddPokemon(opponentPokemon);
            opponentTrainer.CurrentPokemon = opponentPokemon;

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            aiTrainer.TakeTurn(battle);

            // Assert
            // AI should not switch because it has no better Pokémon
            Assert.Equal(aiPokemon1, aiTrainer.CurrentPokemon);
        }

        [Fact]
        public void AI_Should_Use_Healing_Item_When_Low_HP()
        {
            // Arrange
            var aiTrainer = new AITrainer("AI Trainer");
            var aiPokemon = new Pokemon("Pikachu", PokemonType.Electric, 5, 30, 10, 10);

            var potion = new Potion("Potion", "Heals 20 HP", 20);

            aiPokemon.TakeDamage(25); // Reduce Pikachu's HP to 5

            aiTrainer.AddPokemon(aiPokemon);
            aiTrainer.CurrentPokemon = aiPokemon;

            var opponentPokemon = new Pokemon("Rattata", PokemonType.Normal, 5, 30, 10, 10);
            var opponentTrainer = new Trainer("Opponent");
            opponentTrainer.AddPokemon(opponentPokemon);
            opponentTrainer.CurrentPokemon = opponentPokemon;

            var battle = new Battle(aiTrainer, opponentTrainer);

            // Act
            aiTrainer.UseItem(potion, aiTrainer, aiTrainer.CurrentPokemon);

            // Assert
            // AI should use the potion to heal Pikachu
            Assert.Equal(25, aiPokemon.CurrentHP);
        }
    }
}
