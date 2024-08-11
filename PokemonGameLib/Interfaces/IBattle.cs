using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a Pokémon battle between two trainers, handling battle mechanics and interactions.
    /// </summary>
    public interface IBattle
    {
        /// <summary>
        /// Gets the first trainer in the battle.
        /// </summary>
        ITrainer FirstTrainer { get; }

        /// <summary>
        /// Gets the second trainer in the battle.
        /// </summary>
        ITrainer SecondTrainer { get; }

        /// <summary>
        /// Gets the trainer currently attacking based on the turn.
        /// </summary>
        ITrainer AttackingTrainer { get; }

        /// <summary>
        /// Gets the current defending trainer based on the turn.
        /// </summary>
        ITrainer DefendingTrainer { get; }

        /// <summary>
        /// Performs an attack using the specified move.
        /// </summary>
        /// <param name="move">The move to be used for the attack.</param>
        /// <exception cref="InvalidOperationException">Thrown if any condition for performing the attack is invalid.</exception>
        void PerformAttack(IMove move);

        /// <summary>
        /// Determines the result of the battle.
        /// </summary>
        /// <returns>A string describing the result of the battle.</returns>
        string DetermineBattleResult();

        /// <summary>
        /// Switches the current Pokémon of the specified trainer to the new Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer who is switching Pokémon.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="ArgumentNullException">Thrown if trainer or newPokemon is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the switch is invalid (e.g., fainted Pokémon, Pokémon not owned by the trainer).</exception>
        void SwitchPokemon(ITrainer trainer, IPokemon newPokemon);
    }
}
