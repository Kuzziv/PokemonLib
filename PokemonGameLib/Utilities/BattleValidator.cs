using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides validation methods for Pokémon battles, ensuring the validity of moves and Pokémon switches.
    /// </summary>
    public static class BattleValidator
    {
        /// <summary>
        /// Validates whether a specified move can be used by the attacking Pokémon.
        /// </summary>
        /// <param name="attacker">The Pokémon attempting to use the move.</param>
        /// <param name="move">The move to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the attacker or the move is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the move is not available to the attacker.</exception>
        public static void ValidateMove(IPokemon attacker, IMove move)
        {
            if (attacker == null || move == null)
                throw new ArgumentNullException("Attacker or move cannot be null.");

            if (!attacker.Moves.Contains(move))
                throw new InvalidOperationException($"{attacker.Name} cannot use {move.Name}.");
        }

        /// <summary>
        /// Validates whether a Pokémon can be switched into battle by the trainer.
        /// </summary>
        /// <param name="trainer">The trainer attempting to switch Pokémon.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the trainer or the new Pokémon is null.</exception>
        /// <exception cref="InvalidPokemonSwitchException">
        /// Thrown if the new Pokémon is not owned by the trainer or if the Pokémon has fainted.
        /// </exception>
        public static void ValidatePokemonSwitch(ITrainer trainer, IPokemon newPokemon)
        {
            if (trainer == null) throw new ArgumentNullException(nameof(trainer));
            if (newPokemon == null) throw new ArgumentNullException(nameof(newPokemon));

            if (!trainer.Pokemons.Contains(newPokemon))
                throw new InvalidPokemonSwitchException($"{trainer.Name} does not own {newPokemon.Name}.");

            if (newPokemon.IsFainted())
                throw new InvalidPokemonSwitchException($"Cannot switch to a fainted Pokémon: {newPokemon.Name}.");
        }
    }
}
