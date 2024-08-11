namespace PokemonGameLib.Exceptions
{
    /// <summary>
    /// Exception thrown when an item is not found in the trainer's inventory.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ItemNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ItemNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
