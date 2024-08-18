using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Events
{
    public class MoveEventArgs : EventArgs
    {
        public IMove Move { get; set; }
        public IPokemon Attacker { get; set; }
        public IPokemon Defender { get; set; }
    }
}
