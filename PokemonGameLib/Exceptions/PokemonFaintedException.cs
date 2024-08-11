using System;

namespace PokemonGameLib.Exceptions
{
    /// <summary>
    /// Exception thrown when a Pokémon that has fainted is attempted to be used in battle.
    /// </summary>
    public class PokemonFaintedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PokemonFaintedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PokemonFaintedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokemonFaintedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PokemonFaintedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
