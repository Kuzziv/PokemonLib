using System;
using System.Collections.Generic;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Models.Evolutions
{
    /// <summary>
    /// Represents a Pokémon evolution condition, determining when a Pokémon can evolve.
    /// </summary>
    public class Evolution
    {
        /// <summary>
        /// Gets the name of the evolved form.
        /// </summary>
        public string EvolvedFormName { get; }

        /// <summary>
        /// Gets the level required for the evolution to occur.
        /// </summary>
        public int RequiredLevel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Evolution"/> class.
        /// </summary>
        /// <param name="evolvedFormName">The name of the evolved form.</param>
        /// <param name="requiredLevel">The level required for evolution.</param>
        /// <exception cref="ArgumentException">Thrown if the evolved form name is null or empty, or if the required level is less than 1.</exception>
        public Evolution(string evolvedFormName, int requiredLevel)
        {
            if (string.IsNullOrEmpty(evolvedFormName))
            {
                throw new ArgumentException("Evolved form name cannot be null or empty.", nameof(evolvedFormName));
            }

            if (requiredLevel < 1)
            {
                throw new ArgumentException("Required level must be greater than 0.", nameof(requiredLevel));
            }

            EvolvedFormName = evolvedFormName;
            RequiredLevel = requiredLevel;
        }

        /// <summary>
        /// Determines if a Pokémon can evolve based on its level.
        /// </summary>
        /// <param name="pokemon">The Pokémon to check.</param>
        /// <returns><c>true</c> if the Pokémon can evolve; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the Pokémon is null.</exception>
        public bool CanEvolve(Pokemon pokemon)
        {
            if (pokemon == null)
            {
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
            }

            return pokemon.Level >= RequiredLevel;
        }
    }
}
