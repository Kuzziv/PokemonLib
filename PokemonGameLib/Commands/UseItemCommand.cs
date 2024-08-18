using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    public class UseItemCommand : ICommand
    {
        private readonly IBattle _battle;
        private readonly ITrainer _trainer;
        private readonly IItem _item;
        private readonly IPokemon _target;

        public UseItemCommand(IBattle battle, ITrainer trainer, IItem item, IPokemon target)
        {
            _battle = battle;
            _trainer = trainer;
            _item = item;
            _target = target;
        }

        public void Execute()
        {
            _battle.UseItem(_trainer, _item, _target);
        }
    }
}
