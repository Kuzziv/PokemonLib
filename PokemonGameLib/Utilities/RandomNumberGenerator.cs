using System;

namespace PokemonGameLib.Utilities
{
    /// <summary>
    /// Provides methods for generating random numbers.
    /// </summary>
    public class RandomNumberGenerator
    {
        private static readonly Lazy<RandomNumberGenerator> _instance = new(() => new RandomNumberGenerator());
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        public static RandomNumberGenerator Instance => _instance.Value;

        /// <summary>
        /// Generates a random number between the specified minimum and maximum values.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.</param>
        /// <returns>A double precision floating point number between minValue and maxValue.</returns>
        public double Generate(double minValue, double maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than or equal to max value.");
            return _random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Generates a random integer between the specified minimum and maximum values.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.</param>
        /// <returns>An integer between minValue and maxValue.</returns>
        public int Generate(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than or equal to max value.");
            return _random.Next(minValue, maxValue + 1);
        }
    }
}
