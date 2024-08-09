// Item.cs
using System;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Models.Items
{
    /// <summary>
    /// Represents an item that can be used in battles to affect Pokémon or trainers.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="description">The description of the item.</param>
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Uses the item on a target Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the item.</param>
        /// <param name="target">The target Pokémon.</param>
        public virtual void Use(Trainer trainer, Pokemon target)
        {
            // Default implementation does nothing
        }
    }

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
        public Potion(string name, string description, int healingAmount)
            : base(name, description)
        {
            HealingAmount = healingAmount;
        }

        /// <summary>
        /// Uses the potion to heal a Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer using the potion.</param>
        /// <param name="target">The target Pokémon to heal.</param>
        public override void Use(Trainer trainer, Pokemon target)
        {
            target.Heal(HealingAmount);
            Console.WriteLine($"{target.Name} was healed by {HealingAmount} HP!");
        }
    }
}
