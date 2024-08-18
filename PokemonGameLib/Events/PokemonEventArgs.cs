using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Events
{
    public class PokemonEventArgs : EventArgs
    {
        public IPokemon FaintedPokemon { get; set; }
    }
}
