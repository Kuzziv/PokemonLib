using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a trainer in the Pokémon game. A trainer manages a team of Pokémon and interacts with the game during battles.
    /// </summary>
    public abstract class Trainer : Loggable, ITrainer
    {
        /// <summary>
        /// A list of Pokémon owned by the trainer.
        /// </summary>
        private readonly List<IPokemon> _pokemons;

        /// <summary>
        /// Gets the name of the trainer.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the current Pokémon being used by the trainer.
        /// </summary>
        public IPokemon CurrentPokemon { get; set; }

        /// <summary>
        /// Gets a read-only list of all Pokémon owned by the trainer.
        /// </summary>
        public IList<IPokemon> Pokemons => _pokemons.AsReadOnly();

        /// <summary>
        /// A list of items owned by the trainer, used internally within the class.
        /// </summary>
        internal List<IItem> Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the trainer.</param>
        /// <exception cref="ArgumentNullException">Thrown when the name is null.</exception>
        public Trainer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Trainer name cannot be null.");
            _pokemons = new List<IPokemon>();
            Items = new List<IItem>();
        }

        /// <summary>
        /// Validates the trainer's properties to ensure they meet certain criteria.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the trainer's name is null or whitespace, 
        /// if the current Pokémon is null or not in the trainer's collection, 
        /// or if the trainer has no Pokémon or only fainted Pokémon in their collection.</exception>
        public void ValidateTrainer()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new InvalidOperationException("Trainer must have a name.");
            if (CurrentPokemon == null)
                throw new InvalidOperationException("Trainer must have a current Pokémon.");
            if (!Pokemons.Contains(CurrentPokemon))
                throw new InvalidOperationException("Current Pokémon must be in the Trainer's collection.");
            if (!Pokemons.Any())
                throw new InvalidOperationException("Trainer must have at least one Pokémon in their collection.");
            if (Pokemons.All(p => p.IsFainted()))
                throw new InvalidOperationException("Trainer cannot have only fainted Pokémon in their collection.");
        }

        /// <summary>
        /// Adds a Pokémon to the trainer's collection.
        /// </summary>
        /// <param name="pokemon">The Pokémon to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the Pokémon is null.</exception>
        public void AddPokemon(IPokemon pokemon)
        {
            if (pokemon == null)
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
            _pokemons.Add(pokemon);
            _logger.LogInfo($"{Name} added {pokemon.Name} to their collection.");
        }

        /// <summary>
        /// Removes a Pokémon from the trainer's collection.
        /// </summary>
        /// <param name="pokemon">The Pokémon to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown if the Pokémon is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the Pokémon to remove is not in the trainer's list.</exception>
        public void RemovePokemon(IPokemon pokemon)
        {
            if (pokemon == null)
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
            if (!_pokemons.Contains(pokemon))
                throw new InvalidOperationException("The Pokémon to remove is not in the Trainer's list.");
            _pokemons.Remove(pokemon);
            _logger.LogInfo($"{Name} removed {pokemon.Name} from their collection.");
        }

        /// <summary>
        /// Adds an item to the trainer's inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(IItem item)
        {
            Items.Add(item);
            _logger.LogInfo($"{Name} added {item.Name} to their inventory.");
        }

        /// <summary>
        /// Removes an item from the trainer's inventory.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown if the item is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the item to remove is not in the trainer's inventory.</exception>
        public void RemoveItem(IItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            if (!Items.Contains(item))
                throw new InvalidOperationException("The item to remove is not in the Trainer's inventory.");
            Items.Remove(item);
            _logger.LogInfo($"{Name} removed {item.Name} from their inventory.");
        }

        /// <summary>
        /// Executes the trainer's turn in the battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public abstract void TakeTurn(IBattle battle);

        /// <summary>
        /// Handles the situation when the trainer's current Pokémon has fainted.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public abstract void HandleFaintedPokemon(IBattle battle);
    }
}
