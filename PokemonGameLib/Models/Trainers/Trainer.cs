using System;
using System.Collections.Generic;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;
using PokemonGameLib.Commands;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
    /// </summary>
    public abstract class Trainer : ITrainer
    {
        private readonly List<IPokemon> _pokemons;
        protected readonly ILogger _logger;

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
        internal List<IItem> Items { get; }

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
        /// Validates the Trainer to ensure that it is in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the Trainer is not in a valid state.</exception>
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

        public void AddItem(IItem item)
        {
            Items.Add(item);
            _logger.LogInfo($"{Name} added {item.Name} to their inventory.");
        }

        public ICommand CreateAttackCommand(IBattle battle, IMove move)
        {
            return new AttackCommand(battle, move);
        }

        public ICommand CreateSwitchCommand(IBattle battle, IPokemon newPokemon)
        {
            return new SwitchCommand(battle, this, newPokemon);
        }

        public ICommand CreateUseItemCommand(IBattle battle, IItem item, IPokemon target)
        {
            return new UseItemCommand(battle, this, item, target);
        }

        public abstract void TakeTurn(IBattle battle);
    }
}
