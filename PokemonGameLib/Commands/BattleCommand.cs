using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents a command in a Pokémon battle.
    /// </summary>
    public abstract class BattleCommand : ICommand
    {
        protected readonly IBattle _battle;

        public BattleCommand(IBattle battle)
        {
            _battle = battle;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public abstract void Execute();
    }
}
