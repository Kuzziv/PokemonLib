using System;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;

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

        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="description">The description of the item.</param>
        /// <exception cref="ArgumentNullException">Thrown if the name or description is null or empty.</exception>
        public Item(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Item name cannot be null or empty.");
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description), "Item description cannot be null or empty.");

            Name = name;
            Description = description;
            _logger = LoggingService.GetLogger(); // Retrieve the logger from the LoggingService
        }

        /// <summary>
        /// Uses the item on a target Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the item.</param>
        /// <param name="target">The target Pokémon.</param>
        public virtual void Use(ITrainer trainer, IPokemon target)
        {
            _logger.LogInfo($"{trainer.Name} used {Name} on {target.Name}.");
        }
    }
}
