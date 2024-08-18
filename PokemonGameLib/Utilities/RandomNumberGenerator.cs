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

        public RandomNumberGenerator()
        {
            _random = new Random();
        }

        public static RandomNumberGenerator Instance => _instance.Value;

        public double Generate(double minValue, double maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than or equal to max value.");
            return _random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public int Generate(int minValue, int maxValue)
        {
            if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "Min value must be less than or equal to max value.");
            return _random.Next(minValue, maxValue + 1);
        }
    }
}
