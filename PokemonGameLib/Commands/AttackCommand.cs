using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents an attack command in a Pokémon battle.
    /// </summary>
    public class AttackCommand : BattleCommand
    {
        /// <summary>
        /// The move to be used in the attack.
        /// </summary>
        private readonly IMove _move;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttackCommand"/> class.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <param name="move">The move to be used in the attack.</param>
        public AttackCommand(IBattle battle, IMove move) : base(battle)
        {
            _move = move;
        }

        /// <summary>
        /// Executes the attack command, performing an attack in the battle.
        /// </summary>
        public override void Execute()
        {
            _battle.PerformAttack(_move);
        }
    }
}
