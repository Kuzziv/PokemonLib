using System;

namespace PokemonGameLib.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid move is attempted in a Pokémon battle.
    /// </summary>
    public class InvalidMoveException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMoveException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidMoveException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMoveException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidMoveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
