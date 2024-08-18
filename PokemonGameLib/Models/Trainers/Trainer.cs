using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Models.Trainers
{
    public abstract class Trainer : Loggable, ITrainer
    {
        private readonly List<IPokemon> _pokemons;
        protected readonly ILogger _logger;

        public string Name { get; }
        public IPokemon CurrentPokemon { get; set; }
        public IList<IPokemon> Pokemons => _pokemons.AsReadOnly();
        internal List<IItem> Items { get; }

        public Trainer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Trainer name cannot be null.");
            _pokemons = new List<IPokemon>();
            Items = new List<IItem>();
            _logger = LoggingService.GetLogger();
        }

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

        public void AddPokemon(IPokemon pokemon)
        {
            if (pokemon == null)
                throw new ArgumentNullException(nameof(pokemon), "Pokemon cannot be null.");
            _pokemons.Add(pokemon);
            _logger.LogInfo($"{Name} added {pokemon.Name} to their collection.");
        }

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

        public void RemoveItem(IItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            if (!Items.Contains(item))
                throw new InvalidOperationException("The item to remove is not in the Trainer's inventory.");
            Items.Remove(item);
            _logger.LogInfo($"{Name} removed {item.Name} from their inventory.");
        }

        public abstract void TakeTurn(IBattle battle);
    }
}
