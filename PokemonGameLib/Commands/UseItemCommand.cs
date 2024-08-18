using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents a use item command in a Pokémon battle.
    /// </summary>
    public class UseItemCommand : BattleCommand
    {
        private readonly Trainer _trainer;
        private readonly IItem _item;
        private readonly IPokemon _targetPokemon;

        public UseItemCommand(IBattle battle, Trainer trainer, IItem item, IPokemon targetPokemon) : base(battle)
        {
            _trainer = trainer;
            _item = item;
            _targetPokemon = targetPokemon;
        }

        public override void Execute()
        {
            _item.Use(_trainer, _targetPokemon);  // Pass both the trainer and the target Pokemon
            _trainer.RemoveItem(_item); // Assuming item is consumed upon use
        }
    }
}
