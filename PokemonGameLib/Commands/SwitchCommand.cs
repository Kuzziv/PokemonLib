using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Commands
{
    public class SwitchCommand : ICommand
    {
        private readonly IBattle _battle;
        private readonly ITrainer _trainer;
        private readonly IPokemon _newPokemon;

        public SwitchCommand(IBattle battle, ITrainer trainer, IPokemon newPokemon)
        {
            _battle = battle;
            _trainer = trainer;
            _newPokemon = newPokemon;
        }

        public void Execute()
        {
            _battle.SwitchPokemon(_trainer, _newPokemon);
        }
    }
}
