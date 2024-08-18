using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a Pokémon with properties for name, type, level, stats, abilities, and methods to manage its state.
    /// </summary>
    public interface IPokemon
    {
        /// <summary>
        /// Gets the name of the Pokémon.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the Pokémon.
        /// </summary>
        PokemonType Type { get; }

        /// <summary>
        /// Gets the level of the Pokémon.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets the maximum HP of the Pokémon.
        /// </summary>
        int MaxHP { get; }

        /// <summary>
        /// Gets the current HP of the Pokémon.
        /// </summary>
        int CurrentHP { get; }

        /// <summary>
        /// Gets the attack stat of the Pokémon.
        /// </summary>
        int Attack { get; }

        /// <summary>
        /// Gets the defense stat of the Pokémon.
        /// </summary>
        int Defense { get; }

        /// <summary>
        /// Gets the list of moves the Pokémon knows.
        /// </summary>
        IList<IMove> Moves { get; }

        /// <summary>
        /// Gets the status condition of the Pokémon.
        /// </summary>
        StatusCondition Status { get; }

        /// <summary>
        /// Gets the list of possible evolutions for the Pokémon.
        /// </summary>
        IList<IEvolution> Evolutions { get; }

        /// <summary>
        /// Levels up the Pokémon, increasing its level and improving its stats.
        /// </summary>
        void LevelUp();

        /// <summary>
        /// Applies damage to the Pokémon.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        void TakeDamage(int damage);

        /// <summary>
        /// Heals the Pokémon by a specified amount.
        /// </summary>
        /// <param name="amount">The amount to heal.</param>
        void Heal(int amount);

        /// <summary>
        /// Lowers a specific stat of the Pokémon by a specified amount.
        /// </summary>
        /// <param name="stat">The stat to lower (e.g., "Attack" or "Defense").</param>
        /// <param name="amount">The amount to lower the stat by.</param>
        void LowerStat(string stat, int amount);

        /// <summary>
        /// Determines whether the Pokémon has fainted.
        /// </summary>
        /// <returns><c>true</c> if the Pokémon's HP is less than or equal to 0; otherwise, <c>false</c>.</returns>
        bool IsFainted();

        /// <summary>
        /// Adds a move to the Pokémon's move list.
        /// </summary>
        /// <param name="move">The move to add to the Pokémon's move list.</param>
        void AddMove(IMove move);

        /// <summary>
        /// Inflicts a status condition on the Pokémon.
        /// </summary>
        /// <param name="status">The status condition to inflict.</param>
        void InflictStatus(StatusCondition status);

        /// <summary>
        /// Applies the effects of the current status condition on the Pokémon.
        /// </summary>
        void ApplyStatusEffects();

        /// <summary>
        /// Cures the Pokémon of any status condition.
        /// </summary>
        void CureStatus();

        /// <summary>
        /// Sets the sleep duration when the Pokémon falls asleep.
        /// </summary>
        /// <param name="duration">The number of turns the Pokémon will remain asleep.</param>
        void SetSleepDuration(int duration);

        /// <summary>
        /// Determines whether the Pokémon can evolve based on its current state and evolution criteria.
        /// </summary>
        /// <returns><c>true</c> if the Pokémon can evolve; otherwise, <c>false</c>.</returns>
        bool CanEvolve();

        /// <summary>
        /// Evolves the Pokémon to its next form if the criteria are met.
        /// </summary>
        void Evolve();
    }
}
