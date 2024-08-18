using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents a switch Pokémon command in a Pokémon battle.
    /// </summary>
    public class SwitchCommand : BattleCommand
    {
        /// <summary>
        /// The trainer performing the switch.
        /// </summary>
        private readonly Trainer _trainer;

        /// <summary>
        /// The new Pokémon to switch to.
        /// </summary>
        private readonly IPokemon _newPokemon;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchCommand"/> class.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <param name="trainer">The trainer performing the switch.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        public SwitchCommand(IBattle battle, Trainer trainer, IPokemon newPokemon) : base(battle)
        {
            _trainer = trainer;
            _newPokemon = newPokemon;
        }

        /// <summary>
        /// Executes the switch command, switching the trainer's current Pokémon with a new one.
        /// </summary>
        public override void Execute()
        {
            _trainer.CurrentPokemon = _newPokemon;
            _battle.PerformSwitch(_trainer, _newPokemon);
        }
    }
}
