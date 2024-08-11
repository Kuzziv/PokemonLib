using System;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides utility methods for validating Pokémon game objects.
    /// </summary>
    public static class ValidationUtility
    {
        /// <summary>
        /// Ensures that a Pokémon is not null or fainted.
        /// </summary>
        /// <param name="pokemon">The Pokémon to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown if the Pokémon is null.</exception>
        /// <exception cref="PokemonFaintedException">Thrown if the Pokémon has fainted.</exception>
        public static void ValidatePokemon(IPokemon pokemon, string parameterName)
        {
            if (pokemon == null)
                throw new ArgumentNullException(parameterName, "Pokemon cannot be null.");

            if (pokemon.IsFainted())
                throw new PokemonFaintedException($"{pokemon.Name} has fainted and cannot perform this action.");
        }

        /// <summary>
        /// Ensures that a move is valid for a Pokémon.
        /// </summary>
        /// <param name="pokemon">The Pokémon to validate the move against.</param>
        /// <param name="move">The move to validate.</param>
        /// <exception cref="InvalidMoveException">Thrown if the move is invalid for the Pokémon.</exception>
        public static void ValidateMove(IPokemon pokemon, IMove move)
        {
            if (pokemon == null) throw new ArgumentNullException(nameof(pokemon));
            if (move == null) throw new ArgumentNullException(nameof(move));

            if (!pokemon.Moves.Contains(move))
                throw new InvalidMoveException($"{pokemon.Name} cannot use the move {move.Name}.");
        }
    }
}
