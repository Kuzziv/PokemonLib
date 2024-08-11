using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents an item that can be used in battles to affect Pokémon or trainers.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the item.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Uses the item on a target Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the item.</param>
        /// <param name="target">The target Pokémon.</param>
        void Use(ITrainer trainer, IPokemon target);
    }
}
