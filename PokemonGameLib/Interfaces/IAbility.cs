using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents an ability that a Pokémon can have, providing special effects or benefits in battle.
    /// </summary>
    public interface IAbility
    {
        /// <summary>
        /// Gets the name of the ability.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the ability.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Applies the ability effect during a battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <param name="user">The Pokémon using the ability.</param>
        /// <param name="target">The target Pokémon of the ability.</param>
        void ApplyEffect(IBattle battle, IPokemon user, IPokemon target);
    }
}
