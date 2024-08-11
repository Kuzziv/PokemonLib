using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
    /// </summary>
    public abstract class Trainer : ITrainer
    {
        private readonly List<IPokemon> _pokemons;
        protected readonly Logger _logger;

        /// <summary>
        /// Gets the name of the Trainer.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the current Pokémon that the Trainer is using in battle.
        /// </summary>
        public IPokemon CurrentPokemon { get; set; }

        /// <summary>
        /// Gets the list of Pokémon that the Trainer owns.
        /// </summary>
        public IList<IPokemon> Pokemons => _pokemons.AsReadOnly();

        /// <summary>
        /// Gets the list of items that the Trainer possesses.
        /// </summary>
        protected List<IItem> Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the Trainer.</param>
        /// <exception cref="ArgumentNullException">Thrown if the name is null.</exception>
        public Trainer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Trainer name cannot be null.");
            _pokemons = new List<IPokemon>();
            Items = new List<IItem>();
            _logger = LoggingService.GetLogger(); // Retrieve the logger from the LoggingService
        }

        /// <summary>
        /// Adds a Pokémon to the Trainer's collection.
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
        /// Removes a Pokémon from the Trainer's collection.
        /// </summary>
        /// <param name="pokemon">The Pokémon to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown if the Pokémon is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the Pokémon is not in the Trainer's collection.</exception>
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
        /// Switches the current Pokémon to a different one from the Trainer's collection.
        /// </summary>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="ArgumentNullException">Thrown if the new Pokémon is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the Pokémon is not in the Trainer's collection or if it is fainted.</exception>
        public void SwitchPokemon(IPokemon newPokemon)
        {
            if (newPokemon == null)
                throw new ArgumentNullException(nameof(newPokemon), "New Pokémon cannot be null.");

            if (!_pokemons.Contains(newPokemon))
                throw new InvalidOperationException("Cannot switch to a Pokémon not owned by the trainer.");

            if (newPokemon.IsFainted())
                throw new InvalidOperationException("Cannot switch to a fainted Pokémon.");

            CurrentPokemon = newPokemon;
            _logger.LogInfo($"{Name} switched to {newPokemon.Name}!");
            Console.WriteLine($"{Name} switched to {newPokemon.Name}!");
        }

        /// <summary>
        /// Uses an item on a target Pokémon.
        /// </summary>
        /// <param name="item">The item to use.</param>
        /// <param name="target">The target Pokémon to use the item on.</param>
        /// <exception cref="ArgumentNullException">Thrown if the item or target is null.</exception>
        public void UseItem(IItem item, IPokemon target)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");

            if (target == null)
                throw new ArgumentNullException(nameof(target), "Target Pokémon cannot be null.");

            if (Items.Contains(item))
            {
                item.Use(this, target);
                Items.Remove(item);
                _logger.LogInfo($"{Name} used {item.Name} on {target.Name}.");
            }
            else
            {
                Console.WriteLine("Item not available in inventory.");
                _logger.LogWarning($"{Name} attempted to use an item not available in inventory.");
            }
        }

        /// <summary>
        /// Represents the action that the Trainer takes during a battle.
        /// </summary>
        /// <param name="battle">The battle in which the Trainer is participating.</param>
        public abstract void TakeTurn(IBattle battle);
    }
}
