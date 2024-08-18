

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a command that can be executed within the game.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute();
    }
}
