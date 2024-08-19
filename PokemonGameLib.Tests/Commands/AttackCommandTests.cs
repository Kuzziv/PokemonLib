using Moq;
using Xunit;
using PokemonGameLib.Commands;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Tests.Commands
{
    public class AttackCommandTests
    {
        [Fact]
        public void Execute_PerformsAttack()
        {
            // Arrange
            var mockBattle = new Mock<IBattle>();
            var mockMove = new Mock<IMove>();
            var attackCommand = new AttackCommand(mockBattle.Object, mockMove.Object);

            // Act
            attackCommand.Execute();

            // Assert
            mockBattle.Verify(b => b.PerformAttack(mockMove.Object), Times.Once);
        }
    }
}
