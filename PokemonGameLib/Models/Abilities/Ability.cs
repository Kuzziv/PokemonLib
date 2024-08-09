using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Battles;

namespace PokemonGameLib.Models.Abilities
{
    /// <summary>
    /// Represents an ability that a Pokémon can have, providing special effects or benefits in battle.
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// Gets the name of the ability.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the ability.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class.
        /// </summary>
        /// <param name="name">The name of the ability.</param>
        /// <param name="description">The description of the ability.</param>
        public Ability(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Ability name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Ability description cannot be null or empty.", nameof(description));
            }

            Name = name;
            Description = description;
        }

        /// <summary>
        /// Applies the ability effect during a battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <param name="user">The Pokémon using the ability.</param>
        /// <param name="target">The target Pokémon of the ability.</param>
        /// <exception cref="ArgumentNullException">Thrown if the user is null.</exception>
        public void ApplyEffect(Battle battle, Pokemon user, Pokemon target)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (Name == "Intimidate" && target != null)
            {
                target.LowerStat("Attack", 1);
                Console.WriteLine($"{user.Name}'s {Name} ability lowered {target.Name}'s Attack!");
            }

        }
    }
}
