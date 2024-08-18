using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Battles;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
    /// </summary>
    public interface ITrainer
    {
        /// <summary>
        /// The name of the trainer.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The frist pokemon for the trainer.
        /// </summary>
        IPokemon CurrentPokemon { get; set; }

        /// <summary>
        /// The list of Pokémon in the trainer's collection.
        /// </summary>
        IList<IPokemon> Pokemons { get; }

        /// <summary>
        /// Adds an item to the trainer's inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void AddItem(IItem item);

        /// <summary>
        /// Removes an item from the trainer's inventory.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void RemoveItem(IItem item);

        /// <summary>
        /// Adds a Pokémon to the trainer's collection.
        /// </summary>
        /// <param name="pokemon">The Pokémon to add.</param>
        void AddPokemon(IPokemon pokemon);

        /// <summary>
        /// Removes a Pokémon from the trainer's collection.
        /// </summary>
        /// <param name="pokemon">The Pokémon to remove.</param>
        void RemovePokemon(IPokemon pokemon);

        /// <summary>
        /// Validates the trainer's state, ensuring they have a name, a current Pokémon, at least one Pokémon in their collection, and no fainted Pokémon.
        /// </summary>
        void ValidateTrainer();

        /// <summary>
        /// The trainer takes a turn in the battle, selecting a move or switching Pokémon.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        void TakeTurn(IBattle battle);

        /// <summary>
        /// Handles the event of a Pokémon fainting in battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        void HandleFaintedPokemon(IBattle battle);

    }
}
