using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents a command in a Pokémon battle.
    /// </summary>
    public abstract class BattleCommand : ICommand
    {
        /// <summary>
        /// The current battle instance.
        /// </summary>
        protected readonly IBattle _battle;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCommand"/> class.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
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
