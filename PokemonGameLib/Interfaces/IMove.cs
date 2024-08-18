using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a move that a Pokémon can use in battle, defining its properties and special effects.
    /// </summary>
    public interface IMove
    {
        /// <summary>
        /// Gets the name of the move.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the move.
        /// </summary>
        PokemonType Type { get; }

        /// <summary>
        /// Gets the power of the move.
        /// </summary>
        int Power { get; }

        /// <summary>
        /// Gets the level at which the Pokémon can learn this move.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets the maximum number of hits the move can deal in one turn.
        /// </summary>
        int MaxHits { get; }

        /// <summary>
        /// Gets the percentage of damage dealt as recoil to the user.
        /// </summary>
        int RecoilPercentage { get; }

        /// <summary>
        /// Gets the percentage of HP healed by the move.
        /// </summary>
        int HealingPercentage { get; }

        /// <summary>
        /// Validates whether this move is compatible with the given Pokémon.
        /// </summary>
        /// <param name="pokemon">The Pokémon to validate against.</param>
        /// <returns><c>true</c> if the move is compatible with the Pokémon's type and level; otherwise, <c>false</c>.</returns>
        bool ValidateMove(IPokemon pokemon);
    }
}
