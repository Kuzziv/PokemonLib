using System;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Models.Items
{
    /// <summary>
    /// Represents an item that can be used in battles to affect Pokémon or trainers.
    /// </summary>
    public class Item : IItem
    {
        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="description">The description of the item.</param>
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Uses the item on a target Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the item.</param>
        /// <param name="target">The target Pokémon.</param>
        public virtual void Use(ITrainer trainer, IPokemon target)
        {
            // Default implementation does nothing
        }
    }
}
