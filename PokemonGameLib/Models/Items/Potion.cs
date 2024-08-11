using System;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Models.Items
{
    /// <summary>
    /// Represents a potion item that heals a Pokémon.
    /// </summary>
    public class Potion : Item
    {
        /// <summary>
        /// Gets the healing amount provided by the potion.
        /// </summary>
        public int HealingAmount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Potion"/> class.
        /// </summary>
        /// <param name="name">The name of the potion.</param>
        /// <param name="description">The description of the potion.</param>
        /// <param name="healingAmount">The healing amount provided by the potion.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the healing amount is less than or equal to 0.</exception>
        public Potion(string name, string description, int healingAmount)
            : base(name, description)
        {
            if (healingAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(healingAmount), "Healing amount must be greater than 0.");

            HealingAmount = healingAmount;
        }

        /// <summary>
        /// Uses the potion to heal a Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the potion.</param>
        /// <param name="target">The target Pokémon to heal.</param>
        public override void Use(ITrainer trainer, IPokemon target)
        {
            target.Heal(HealingAmount);
            _logger.LogInfo($"{target.Name} was healed by {HealingAmount} HP using {Name}.");
            Console.WriteLine($"{target.Name} was healed by {HealingAmount} HP!");
        }
    }
}
