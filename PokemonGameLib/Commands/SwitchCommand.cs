using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;

namespace PokemonGameLib.Commands
{
    /// <summary>
    /// Represents a switch Pokémon command in a Pokémon battle.
    /// </summary>
    public class SwitchCommand : BattleCommand
    {
        private readonly Trainer _trainer;
        private readonly IPokemon _newPokemon;

        public SwitchCommand(IBattle battle, Trainer trainer, IPokemon newPokemon) : base(battle)
        {
            _trainer = trainer;
            _newPokemon = newPokemon;
        }

        public override void Execute()
        {
            _trainer.CurrentPokemon = _newPokemon;
            _battle.PerformSwitch(_newPokemon);
        }
    }
}
