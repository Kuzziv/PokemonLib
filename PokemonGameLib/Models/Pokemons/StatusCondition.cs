// StatusCondition.cs
namespace PokemonGameLib.Models.Pokemons
{
    /// <summary>
    /// Represents status conditions that can affect a Pokémon during battle.
    /// </summary>
    public enum StatusCondition
    {
        /// <summary>
        /// No status condition is present.
        /// </summary>
        None,

        /// <summary>
        /// The Pokémon is paralyzed and has a 25% chance of being unable to move each turn.
        /// </summary>
        Paralysis,

        /// <summary>
        /// The Pokémon is asleep and cannot move for 1-3 turns.
        /// </summary>
        Sleep,

        /// <summary>
        /// The Pokémon is burned and loses 1/8 of its maximum HP each turn.
        /// </summary>
        Burn,

        /// <summary>
        /// The Pokémon is frozen and cannot move until thawed.
        /// </summary>
        Freeze,

        /// <summary>
        /// The Pokémon is poisoned and loses 1/8 of its maximum HP each turn.
        /// </summary>
        Poison
    }
}
