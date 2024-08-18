using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    public class AttackCommand : ICommand
    {
        private readonly IBattle _battle;
        private readonly IMove _move;

        public AttackCommand(IBattle battle, IMove move)
        {
            _battle = battle;
            _move = move;
        }

        public void Execute()
        {
            _battle.PerformAttack(_move);
        }
    }
}
