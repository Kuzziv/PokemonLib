using System;

/// <summary>
/// Represents a move that a Pokémon can use in battle.
/// </summary>
public class Move
{
    /// <summary>
    /// Gets the name of the move.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the move.
    /// </summary>
    public PokemonType Type { get; }

    /// <summary>
    /// Gets the power of the move.
    /// </summary>
    public int Power { get; }

    /// <summary>
    /// Gets the level at which the Pokémon can learn this move.
    /// </summary>
    public int Level { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="name">The name of the move.</param>
    /// <param name="type">The type of the move.</param>
    /// <param name="power">The power of the move.</param>
    /// <param name="level">The level at which the move can be learned.</param>
    /// <exception cref="ArgumentException">Thrown if the move name is null or empty, the type is not a valid <see cref="PokemonType"/>, the power is negative, or the level is less than 1.</exception>
    public Move(string name, PokemonType type, int power, int level)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Move name cannot be null or empty.", nameof(name));
        }

        if (!Enum.IsDefined(typeof(PokemonType), type))
        {
            throw new ArgumentException("Invalid move type.", nameof(type));
        }

        if (power < 0)
        {
            throw new ArgumentException("Move power cannot be negative.", nameof(power));
        }

        if (level < 1)
        {
            throw new ArgumentException("Move level must be greater than 0.", nameof(level));
        }

        Name = name;
        Type = type;
        Power = power;
        Level = level;
    }

    /// <summary>
    /// Determines whether this move is compatible with a given Pokémon type and level.
    /// </summary>
    /// <param name="pokemonType">The type of the Pokémon.</param>
    /// <param name="pokemonLevel">The level of the Pokémon.</param>
    /// <returns><c>true</c> if the move type is compatible with the Pokémon type and the Pokémon's level is sufficient to learn the move; otherwise, <c>false</c>.</returns>
    public bool IsCompatibleWith(PokemonType pokemonType, int pokemonLevel)
    {
        return Type == pokemonType && Level <= pokemonLevel;
    }
}
