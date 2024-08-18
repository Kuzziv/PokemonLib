using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a battle between two trainers in the Pokémon game.
    /// </summary>
    public interface IBattle
    {
        /// <summary>
        /// Gets the current trainer whose turn it is.
        /// </summary>
        ITrainer CurrentTrainer { get; }

        /// <summary>
        /// Gets the opponent trainer in the current battle.
        /// </summary>
        ITrainer OpponentTrainer { get; }

        /// <summary>
        /// Starts the battle between the two trainers.
        /// </summary>
        void StartBattle();

        /// <summary>
        /// Executes an attack using the specified move.
        /// </summary>
        /// <param name="move">The move to be used in the attack.</param>
        void PerformAttack(IMove move);

        /// <summary>
        /// Switches the current Pokémon with the specified new Pokémon.
        /// </summary>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <param name="trainer">The trainer who is switching Pokémon.</param>
        void PerformSwitch(ITrainer trainer, IPokemon newPokemon);

        /// <summary>
        /// Uses the specified item on the target Pokémon.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <param name="targetPokemon">The target Pokémon to use the item on.</param>
        void PerformUseItem(IItem item, IPokemon targetPokemon);

        /// <summary>
        /// Switches the turn to the other trainer.
        /// </summary>
        void SwitchTurns();

        /// <summary>
        /// Determines whether the battle is over.
        /// </summary>
        /// <returns>True if the battle is over; otherwise, false.</returns>
        bool IsBattleOver();

        /// <summary>
        /// Gets the winner of the battle.
        /// </summary>
        /// <returns>The winning trainer.</returns>
        ITrainer GetWinner();

        /// <summary>
        /// Determines the result of the battle.
        /// </summary>
        /// <returns>A string representing the result of the battle.</returns>
        string DetermineBattleResult();
    }
}
