using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonGameLib.Models
{
    /// <summary>
    /// Represents a Trainer in the Pokemon game.
    /// </summary>
    public class Trainer
    {
        // Private backing field for Pokemons
        private readonly List<Pokemon> _pokemons;

        /// <summary>
        /// Gets the list of Pokemons owned by the Trainer.
        /// </summary>
        public IReadOnlyList<Pokemon> Pokemons => _pokemons.AsReadOnly();

        /// <summary>
        /// Gets or sets the name of the Trainer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the Trainer.</param>
        public Trainer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Trainer name cannot be null.");
            _pokemons = new List<Pokemon>();
        }

        /// <summary>
        /// Adds a Pokemon to the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokemon to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="pokemon"/> is null.</exception>
        public void AddPokemon(Pokemon pokemon)
        {
            if (pokemon == null) 
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
            
            _pokemons.Add(pokemon);
        }

        /// <summary>
        /// Determines whether the trainer has any Pokémon that are not fainted.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the trainer has at least one Pokémon that is not fainted; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValidPokemons()
        {
            return _pokemons.Any(p => !p.IsFainted());
        }

        /// <summary>
        /// Removes a Pokémon from the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokémon to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="pokemon"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the Pokémon is not in the Trainer's list.</exception>
        public void RemovePokemon(Pokemon pokemon)
        {
            if (pokemon == null)
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");

            if (!_pokemons.Contains(pokemon))
                throw new InvalidOperationException("The Pokémon to remove is not in the Trainer's list.");

            _pokemons.Remove(pokemon);
        }
    }
}
