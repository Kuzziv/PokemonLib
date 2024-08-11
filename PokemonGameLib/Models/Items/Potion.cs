using System;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Models.Items
{
    /// <summary>
    /// Represents a potion item that heals a Pokémon.
    /// </summary>
    public class Potion : Item
    {

        /// <summary>
        /// Gets the name of the potion.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the potion.
        /// </summary>
        /// <value>The description of the potion.</value>
        public string Description { get; }

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
        public Potion(string name, string description, int healingAmount)
            : base(name, description)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Potion name cannot be null or empty.", nameof(name));
            }
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Potion description cannot be null or empty.", nameof(description));
            }
            
            if (healingAmount <= 0)
            {
                throw new ArgumentException("Healing amount must be greater than 0.", nameof(healingAmount));
            }
        }

        /// <summary>
        /// Uses the potion to heal a Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the potion.</param>
        /// <param name="target">The target Pokémon to heal.</param>
        public override void Use(ITrainer trainer, IPokemon target)
        {
            target.Heal(HealingAmount);
            Console.WriteLine($"{target.Name} was healed by {HealingAmount} HP!");
        }
    }
}
