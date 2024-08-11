using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Service for calculating the effectiveness of a move's type against a Pokémon's type.
    /// </summary>
    public interface ITypeEffectivenessService
    {
        /// <summary>
        /// Gets the effectiveness multiplier of a move's type against a Pokémon's type.
        /// </summary>
        /// <param name="attackType">The type of the move being used.</param>
        /// <param name="defenseType">The type of the defending Pokémon.</param>
        /// <returns>A multiplier representing the effectiveness.</returns>
        double GetEffectiveness(PokemonType attackType, PokemonType defenseType);
    }
}
