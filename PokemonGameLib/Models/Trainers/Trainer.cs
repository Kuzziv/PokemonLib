// Trainer.cs
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a Trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
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
        /// Gets or sets the current Pokémon of the Trainer.
        /// </summary>
        public Pokemon? CurrentPokemon { get; set; }  // Nullable to avoid initialization issues

        /// <summary>
        /// Initializes a new instance of the <see cref="Trainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the Trainer.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name is null.</exception>
        public Trainer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Trainer name cannot be null.");
            _pokemons = new List<Pokemon>();
        }

        /// <summary>
        /// Adds a Pokémon to the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokémon to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown when the Pokémon is null.</exception>
        public void AddPokemon(Pokemon pokemon)
        {
            if (pokemon == null) 
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
            
            _pokemons.Add(pokemon);
        }

        /// <summary>
        /// Determines whether the Trainer has any Pokémon that are not fainted.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the Trainer has at least one Pokémon that is not fainted; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValidPokemon()
        {
            return _pokemons.Any(p => !p.IsFainted());
        }

        /// <summary>
        /// Removes a Pokémon from the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokémon to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the Pokémon is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the Pokémon is not in the Trainer's list.</exception>
        public void RemovePokemon(Pokemon pokemon)
        {
            if (pokemon == null)
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");

            if (!_pokemons.Contains(pokemon))
                throw new InvalidOperationException("The Pokémon to remove is not in the Trainer's list.");

            _pokemons.Remove(pokemon);
        }

        /// <summary>
        /// Switches the Trainer's current Pokémon to the specified new Pokémon.
        /// </summary>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="InvalidOperationException">Thrown if the new Pokémon is fainted or not owned by the Trainer.</exception>
        public void SwitchPokemon(Pokemon newPokemon)
        {
            if (newPokemon == null)
                throw new ArgumentNullException(nameof(newPokemon), "New Pokémon cannot be null.");
                
            if (!_pokemons.Contains(newPokemon))
                throw new InvalidOperationException("Cannot switch to a Pokémon not owned by the trainer.");

            if (newPokemon.IsFainted())
                throw new InvalidOperationException("Cannot switch to a fainted Pokémon.");

            CurrentPokemon = newPokemon;
            Console.WriteLine($"{Name} switched to {newPokemon.Name}!");
        }
    }
}
