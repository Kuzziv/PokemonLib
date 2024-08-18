using System;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Models.Items
{
    /// <summary>
    /// Represents a revive item that restores a fainted Pokémon to partial HP.
    /// </summary>
    public class Revive : Item
    {
        /// <summary>
        /// Gets the percentage of HP that the revive restores.
        /// </summary>
        public int RestorePercentage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Revive"/> class.
        /// </summary>
        /// <param name="name">The name of the revive item.</param>
        /// <param name="description">The description of the revive item.</param>
        /// <param name="restorePercentage">The percentage of HP to restore.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the restore percentage is less than or equal to 0 or greater than 100.</exception>
        public Revive(string name, string description, int restorePercentage)
            : base(name, description)
        {
            if (restorePercentage <= 0 || restorePercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(restorePercentage), "Restore percentage must be between 1 and 100.");

            RestorePercentage = restorePercentage;
        }

        /// <summary>
        /// Uses the revive item to restore a fainted Pokémon's HP.
        /// </summary>
        /// <param name="trainer">The trainer using the revive.</param>
        /// <param name="target">The fainted Pokémon to restore.</param>
        public override void Use(ITrainer trainer, IPokemon target)
        {
            if (!target.IsFainted())
            {
                _logger.LogWarning($"{target.Name} is not fainted and cannot be revived.");
                Console.WriteLine($"{target.Name} is not fainted and cannot be revived.");
                return;
            }

            int restoreAmount = (target.MaxHP * RestorePercentage) / 100;
            target.Heal(restoreAmount);

            _logger.LogInfo($"{target.Name} was revived with {restoreAmount} HP.");
            Console.WriteLine($"{target.Name} was revived with {restoreAmount} HP!");
        }
    }
}
