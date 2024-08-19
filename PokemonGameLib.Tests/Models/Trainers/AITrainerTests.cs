using System.Linq;
using Xunit;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Battles;
using Moq;

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
            aiTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battleMock = new Mock<IBattle>();
            battleMock.Setup(b => b.OpponentTrainer).Returns(opponentTrainer);

            // Act
            aiTrainer.TakeTurn(battleMock.Object);

            // Assert
            battleMock.Verify(b => b.PerformAttack(thunderbolt), Times.Once);
        }

        [Fact]
        public void ShouldSwitchPokemon_ReturnsTrue_WhenOpponentIsTooStrong()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 100, 100, 55, 40);
            var charizard = new Pokemon("Charizard", PokemonType.Fire, 100, 100, 84, 78); // Stronger than Pikachu

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(pikachu);
            aiTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charizard);
            opponentTrainer.CurrentPokemon = charizard;

            var battleMock = new Mock<IBattle>();
            battleMock.Setup(b => b.OpponentTrainer).Returns(opponentTrainer);

            // Act
            var shouldSwitch = aiTrainer.ShouldSwitchPokemon(battleMock.Object);

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
            aiTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battleMock = new Mock<IBattle>();
            battleMock.Setup(b => b.OpponentTrainer).Returns(opponentTrainer);

            // Act
            var bestMove = aiTrainer.SelectBestMove(battleMock.Object);

            // Assert
            Assert.Equal(thunderbolt, bestMove);
        }

        [Fact]
        public void ShouldUseItem_ReturnsTrue_WhenCurrentPokemonIsInDanger()
        {
            // Arrange
            var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 50, 100, 55, 40);
            var potion = new Potion("Potion", "Heals 20 HP.", 20);
            var charmander = new Pokemon("Charmander", PokemonType.Fire, 100, 100, 52, 43);
            var flamethrower = new Move("Flamethrower", PokemonType.Fire, 90, 15); // Strong move

            charmander.Moves.Add(flamethrower); // Add a strong move to Charmander

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(pikachu);
            aiTrainer.AddItem(potion);
            aiTrainer.CurrentPokemon = pikachu;

            var opponentTrainer = new PlayerTrainer("Opponent Trainer");
            opponentTrainer.AddPokemon(charmander);
            opponentTrainer.CurrentPokemon = charmander;

            var battle = new Battle(aiTrainer, opponentTrainer);

            pikachu.TakeDamage(90); // Pikachu is now at 10 HP

            // Act
            var shouldUseItem = aiTrainer.ShouldUseItem(out var itemToUse, battle);

            // Assert
            Assert.True(shouldUseItem, "AI Trainer should use an item when Pikachu is in danger.");
            Assert.Equal(potion, itemToUse);
        }

        [Fact]
        public void HandleFaintedPokemon_SwitchesToNonFaintedPokemon()
        {
            // Arrange
            var faintedPokemon = new Pokemon("FaintedMon", PokemonType.Electric, 10, 100, 55, 40);
            var healthyPokemon = new Pokemon("HealthyMon", PokemonType.Electric, 100, 100, 55, 40);

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(faintedPokemon);
            aiTrainer.AddPokemon(healthyPokemon);
            aiTrainer.CurrentPokemon = faintedPokemon;

            // Simulate fainting the Pokémon by setting its HP to 0
            faintedPokemon.TakeDamage(100); // Faint the Pokémon

            var battle = new Battle(aiTrainer, new PlayerTrainer("Opponent Trainer"));

            // Act
            aiTrainer.HandleFaintedPokemon(battle);

            // Assert
            Assert.Equal(healthyPokemon, aiTrainer.CurrentPokemon);
        }


        [Fact]
        public void HandleFaintedPokemon_DoesNothing_WhenAllPokemonAreFainted()
        {
            // Arrange
            var faintedPokemon1 = new Pokemon("FaintedMon1", PokemonType.Electric, 10, 100, 55, 40);
            var faintedPokemon2 = new Pokemon("FaintedMon2", PokemonType.Fire, 10, 100, 55, 40);

            var aiTrainer = new AITrainer("AI Trainer");
            aiTrainer.AddPokemon(faintedPokemon1);
            aiTrainer.AddPokemon(faintedPokemon2);
            aiTrainer.CurrentPokemon = faintedPokemon1;

            var battleMock = new Mock<IBattle>();

            // Act
            aiTrainer.HandleFaintedPokemon(battleMock.Object);

            // Assert
            Assert.Equal(faintedPokemon1, aiTrainer.CurrentPokemon);
            battleMock.Verify(b => b.PerformSwitch(aiTrainer, It.IsAny<IPokemon>()), Times.Never);
        }
    }
}
