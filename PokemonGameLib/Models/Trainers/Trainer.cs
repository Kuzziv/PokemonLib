using System;
using System.Collections.Generic;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Commands;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
    /// </summary>
    public abstract class Trainer : Loggable, ITrainer
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
            _logger = LoggingService.GetLogger();
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

        /// <summary>
        /// Adds an item to the Trainer's inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(IItem item)
        {
            Items.Add(item);
            _logger.LogInfo($"{Name} added {item.Name} to their inventory.");
        }

        /// <summary>
        /// Removes an item from the Trainer's inventory.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown if the item is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the item is not in the Trainer's inventory.</exception>
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
        /// Forces the trainer to switch to a non-fainted Pokémon.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        private void ForceSwitchPokemon(IBattle battle)
        {
            while (true)
            {
                Console.WriteLine("Choose a Pokémon to switch to:");
                for (int i = 0; i < Pokemons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Pokemons[i].Name} (HP: {Pokemons[i].CurrentHP}/{Pokemons[i].MaxHP})");
                }

                if (int.TryParse(Console.ReadLine(), out int pokemonIndex) && pokemonIndex >= 1 && pokemonIndex <= Pokemons.Count)
                {
                    IPokemon selectedPokemon = Pokemons[pokemonIndex - 1];
                    if (selectedPokemon.IsFainted())
                    {
                        Console.WriteLine("Cannot switch to a fainted Pokémon. Choose again.");
                        _logger.LogWarning($"{Name} attempted to switch to a fainted Pokémon.");
                    }
                    else
                    {
                        CurrentPokemon = selectedPokemon;
                        _logger.LogInfo($"{Name} switched to {selectedPokemon.Name}.");
                        Console.WriteLine($"{Name} switched to {selectedPokemon.Name}!");
                        break; // Exit the loop once a valid Pokémon is selected
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    _logger.LogWarning($"{Name} attempted to switch to an invalid Pokémon.");
                }
            }
        }

        /// <summary>
        /// Forces the trainer to switch Pokémon if the current one has fainted.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        protected void HandleFaintedPokemon(IBattle battle)
        {
            if (CurrentPokemon.IsFainted())
            {
                Console.WriteLine($"{CurrentPokemon.Name} has fainted!");
                _logger.LogInfo($"{Name}'s {CurrentPokemon.Name} has fainted!");
                ForceSwitchPokemon(battle);
            }
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
