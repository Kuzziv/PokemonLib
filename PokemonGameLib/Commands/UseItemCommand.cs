using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents a use item command in a Pokémon battle.
    /// </summary>
    public class UseItemCommand : BattleCommand
    {
        /// <summary>
        /// The trainer using the item.
        /// </summary>
        private readonly Trainer _trainer;

        /// <summary>
        /// The item to be used.
        /// </summary>
        private readonly IItem _item;

        /// <summary>
        /// The target Pokémon of the item.
        /// </summary>
        private readonly IPokemon _targetPokemon;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseItemCommand"/> class.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <param name="trainer">The trainer using the item.</param>
        /// <param name="item">The item to be used.</param>
        /// <param name="targetPokemon">The target Pokémon of the item.</param>
        public UseItemCommand(IBattle battle, Trainer trainer, IItem item, IPokemon targetPokemon) : base(battle)
        {
            _trainer = trainer;
            _item = item;
            _targetPokemon = targetPokemon;
        }

        /// <summary>
        /// Executes the use item command, using the item on the target Pokémon.
        /// </summary>
        public override void Execute()
        {
            _item.Use(_trainer, _targetPokemon);  // Pass both the trainer and the target Pokemon
            _trainer.RemoveItem(_item); // Assuming item is consumed upon use
        }
    }
}
