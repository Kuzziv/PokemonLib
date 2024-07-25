using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGameLib.Models
{
    /// <summary>
    /// Represents a Trainer in the Pokemon game.
    /// </summary>
    public class Trainer
    {
        /// <summary>
        /// Gets or sets the list of Pokemons owned by the Trainer.
        /// </summary>
        public List<Pokemon> Pokemons { get; set; }

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
            Name = name;
            Pokemons = new List<Pokemon>();
        }

        /// <summary>
        /// Adds a Pokemon to the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokemon to be added.</param>
        public void AddPokemon(Pokemon pokemon)
        {
            Pokemons.Add(pokemon);
        }

        /// <summary>
        /// Determines whether the trainer has any Pokémon that are not fainted.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the trainer has at least one Pokémon that is not fainted; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks the list of Pokémon associated with the trainer and returns <c>true</c> if there is any Pokémon
        /// that is not in a fainted state (i.e., it has non-zero HP). If all Pokémon are fainted or if the list is empty,
        /// the method will return <c>false</c>.
        /// </remarks>
        public bool HasValidPokemons()
        {
            return Pokemons.Any(p => !p.IsFainted());
        }

    }
}
