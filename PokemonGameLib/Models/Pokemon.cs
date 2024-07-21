using System;
using System.Collections.Generic;

/// <summary>
/// Represents a Pokémon with properties for name, type, level, stats, and methods to manage its state.
/// </summary>
public class Pokemon
{
    /// <summary>
    /// Gets the name of the Pokémon.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the Pokémon.
    /// </summary>
    public PokemonType Type { get; }

    /// <summary>
    /// Gets the level of the Pokémon.
    /// </summary>
    public int Level { get; }

    /// <summary>
    /// Gets the current HP of the Pokémon.
    /// </summary>
    public int HP { get; private set; }

    /// <summary>
    /// Gets the attack stat of the Pokémon.
    /// </summary>
    public int Attack { get; }

    /// <summary>
    /// Gets the defense stat of the Pokémon.
    /// </summary>
    public int Defense { get; }

    /// <summary>
    /// Gets the list of moves the Pokémon knows.
    /// </summary>
    public List<Move> Moves { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pokemon"/> class.
    /// </summary>
    /// <param name="name">The name of the Pokémon.</param>
    /// <param name="type">The type of the Pokémon.</param>
    /// <param name="level">The level of the Pokémon.</param>
    /// <param name="hp">The starting HP of the Pokémon.</param>
    /// <param name="attack">The attack stat of the Pokémon.</param>
    /// <param name="defense">The defense stat of the Pokémon.</param>
    public Pokemon(string name, PokemonType type, int level, int hp, int attack, int defense)
    {
        Name = name;
        Type = type;
        Level = level;
        HP = hp;
        Attack = attack;
        Defense = defense;
        Moves = new List<Move>();
    }

    /// <summary>
    /// Applies damage to the Pokémon.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }
    }

    /// <summary>
    /// Determines whether the Pokémon has fainted.
    /// </summary>
    /// <returns><c>true</c> if the Pokémon's HP is less than or equal to 0; otherwise, <c>false</c>.</returns>
    public bool IsFainted()
    {
        return HP <= 0;
    }

    /// <summary>
    /// Adds a move to the Pokémon's move list.
    /// </summary>
    /// <param name="move">The move to add to the Pokémon's move list.</param>
    /// <exception cref="ArgumentNullException">Thrown if the move is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the Pokémon already knows the maximum number of moves.</exception>
    /// <exception cref="ArgumentException">Thrown if the move is already known by the Pokémon.</exception>
    /// <exception cref="ArgumentException">Thrown if the move is not compatible with the Pokémon's type.</exception>
    /// <exception cref="ArgumentException">Thrown if the move is not compatible with the Pokémon's level.</exception>
    public void AddMove(Move move)
    {
        if (move == null)
        {
            throw new ArgumentNullException(nameof(move), "Move cannot be null.");
        }

        if (Moves.Count >= 4)
        {
            throw new InvalidOperationException("A Pokémon can only know up to 4 moves.");
        }

        if (Moves.Contains(move))
        {
            throw new ArgumentException("Pokémon already knows this move.", nameof(move));
        }

        if (move.Type != Type)
        {
            throw new ArgumentException("Move is not compatible with the Pokémon's type.", nameof(move));
        }

        if (move.Level > Level)
        {
            throw new ArgumentException("Move is not compatible with the Pokémon's level.", nameof(move));
        }

        Moves.Add(move);
    }
}
