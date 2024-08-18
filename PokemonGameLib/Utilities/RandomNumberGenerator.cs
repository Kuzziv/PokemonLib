using System;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides methods for generating random numbers, supporting both integer and double ranges.
    /// </summary>
    public class RandomNumberGenerator
    {
        /// <summary>
        /// A singleton instance of <see cref="RandomNumberGenerator"/>, ensuring only one instance is created.
        /// </summary>
        private static readonly Lazy<RandomNumberGenerator> _instance = new(() => new RandomNumberGenerator());

        /// <summary>
        /// The underlying random number generator.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        public static RandomNumberGenerator Instance => _instance.Value;

        /// <summary>
        /// Generates a random double value within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.</param>
        /// <returns>A double value that is greater than or equal to <paramref name="minValue"/> and less than or equal to <paramref name="maxValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        public double Generate(double minValue, double maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than or equal to max value.");
            return _random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Generates a random integer value within the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.</param>
        /// <returns>An integer value that is greater than or equal to <paramref name="minValue"/> and less than or equal to <paramref name="maxValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        public int Generate(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than or equal to max value.");
            return _random.Next(minValue, maxValue + 1);
        }
    }
}
