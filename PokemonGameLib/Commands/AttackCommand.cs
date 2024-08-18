using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents an attack command in a Pokémon battle.
    /// </summary>
    public class AttackCommand : BattleCommand
    {
        private readonly IMove _move;

        public AttackCommand(IBattle battle, IMove move) : base(battle)
        {
            _move = move;
        }

        public override void Execute()
        {
            _battle.PerformAttack(_move);
        }
    }
}
