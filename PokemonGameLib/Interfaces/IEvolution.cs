using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a Pokémon evolution condition, determining when a Pokémon can evolve.
    /// </summary>
    public interface IEvolution
    {
        /// <summary>
        /// Gets the name of the evolved form.
        /// </summary>
        string EvolvedFormName { get; }

        /// <summary>
        /// Gets the level required for the evolution to occur.
        /// </summary>
        int RequiredLevel { get; }

        /// <summary>
        /// Determines if a Pokémon can evolve based on its level.
        /// </summary>
        /// <param name="pokemon">The Pokémon to check.</param>
        /// <returns><c>true</c> if the Pokémon can evolve; otherwise, <c>false</c>.</returns>
        bool CanEvolve(IPokemon pokemon);
    }
}
