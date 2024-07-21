namespace PokemonGameLib
{
    /// <summary>
    /// Represents a Pokémon with various attributes and moves.
    /// </summary>
    public class Pokemon
    {
        /// <summary>
        /// Gets or sets the name of the Pokémon.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the Pokémon.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the HP of the Pokémon.
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// Gets or sets the attack stat of the Pokémon.
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// Gets or sets the defense stat of the Pokémon.
        /// </summary>
        public int Defense { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pokemon"/> class.
        /// </summary>
        /// <param name="name">The name of the Pokémon.</param>
        /// <param name="type">The type of the Pokémon.</param>
        /// <param name="hp">The HP of the Pokémon.</param>
        /// <param name="attack">The attack stat of the Pokémon.</param>
        /// <param name="defense">The defense stat of the Pokémon.</param>
        public Pokemon(string name, string type, int hp, int attack, int defense)
        {
            Name = name;
            Type = type;
            HP = hp;
            Attack = attack;
            Defense = defense;
        }
    }
}
