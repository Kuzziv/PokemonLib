using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Battles;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
    /// </summary>
    public interface ITrainer
    {
        string Name { get; }
        IPokemon CurrentPokemon { get; set; }
        IList<IPokemon> Pokemons { get; }

        void AddItem(IItem item);
        void RemoveItem(IItem item);

        void AddPokemon(IPokemon pokemon);
        void RemovePokemon(IPokemon pokemon);
        void ValidateTrainer();
        void TakeTurn(IBattle battle);

    }
}
