using Moq;
using Xunit;
using PokemonGameLib.Commands;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Tests.Commands
{
    public class SwitchCommandTests
    {
        [Fact]
        public void Execute_PerformsSwitch()
        {
            // Arrange
            var mockBattle = new Mock<IBattle>();
            var mockTrainer = new Mock<Trainer>("Ash");
            var mockPokemon = new Mock<IPokemon>();
            var switchCommand = new SwitchCommand(mockBattle.Object, mockTrainer.Object, mockPokemon.Object);

            // Act
            switchCommand.Execute();

            // Assert
            Assert.Equal(mockPokemon.Object, mockTrainer.Object.CurrentPokemon);
            mockBattle.Verify(b => b.PerformSwitch(mockTrainer.Object, mockPokemon.Object), Times.Once);
        }
    }
}
