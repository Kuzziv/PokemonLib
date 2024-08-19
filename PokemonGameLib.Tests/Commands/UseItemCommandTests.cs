using Moq;
using Xunit;
using PokemonGameLib.Commands;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Tests.Commands
{
    [Collection("Test Collection")]
    public class UseItemCommandTests
    {
        private class TestTrainer : Trainer
        {
            public TestTrainer(string name) : base(name)
            {
            }

            public override void TakeTurn(IBattle battle)
            {
                // Implementation not needed for this test
            }

            public override void HandleFaintedPokemon(IBattle battle)
            {
                // Implementation not needed for this test
            }
        }

        [Fact]
        public void Execute_UsesItemAndRemovesItFromTrainer()
        {
            // Arrange
            var mockBattle = new Mock<IBattle>();
            var mockItem = new Mock<IItem>();
            var mockPokemon = new Mock<IPokemon>();

            var trainer = new TestTrainer("Ash");
            trainer.CurrentPokemon = mockPokemon.Object;
            trainer.AddItem(mockItem.Object);

            var useItemCommand = new UseItemCommand(mockBattle.Object, trainer, mockItem.Object, mockPokemon.Object);

            // Act
            useItemCommand.Execute();

            // Assert
            mockItem.Verify(i => i.Use(trainer, mockPokemon.Object), Times.Once);
            Assert.DoesNotContain(mockItem.Object, trainer.Items);
        }
    }
}
