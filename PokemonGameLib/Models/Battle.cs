using System;

/// <summary>
/// Manages the battle between two Pokémon, handling attacks and damage calculation.
/// </summary>
public class Battle
{
    /// <summary>
    /// Gets the first Pokémon in the battle.
    /// </summary>
    public Pokemon Attacker { get; }

    /// <summary>
    /// Gets the second Pokémon in the battle.
    /// </summary>
    public Pokemon Defender { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Battle"/> class.
    /// </summary>
    /// <param name="attacker">The Pokémon initiating the attack.</param>
    /// <param name="defender">The Pokémon receiving the attack.</param>
    /// <exception cref="ArgumentNullException">Thrown if either attacker or defender is null.</exception>
    public Battle(Pokemon attacker, Pokemon defender)
    {
        Attacker = attacker ?? throw new ArgumentNullException(nameof(attacker), "Attacker cannot be null.");
        Defender = defender ?? throw new ArgumentNullException(nameof(defender), "Defender cannot be null.");
    }

    /// <summary>
    /// Performs an attack by the attacker Pokémon on the defender Pokémon.
    /// </summary>
    /// <param name="move">The move used by the attacker.</param>
    /// <exception cref="ArgumentNullException">Thrown if the move is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the attacker is fainted or does not know the move.</exception>
    public void PerformAttack(Move move)
    {
        if (move == null)
        {
            throw new ArgumentNullException(nameof(move), "Move cannot be null.");
        }

        if (Attacker.IsFainted())
        {
            throw new InvalidOperationException("Attacker is fainted and cannot perform an attack.");
        }

        if (!Attacker.Moves.Contains(move))
        {
            throw new InvalidOperationException("Attacker does not know the specified move.");
        }

        if (Defender.IsFainted())
        {
            throw new InvalidOperationException("Defender is already fainted.");
        }

        // Calculate damage
        int damage = move.Power; // Simplified damage calculation
        Defender.TakeDamage(damage);

        Console.WriteLine($"{Attacker.Name} attacks {Defender.Name} with {move.Name}!");
    }

    /// <summary>
    /// Determines the result of the battle.
    /// </summary>
    /// <returns>A string indicating the result of the battle.</returns>
    public string DetermineBattleResult()
    {
        if (Defender.IsFainted())
        {
            return $"{Defender.Name} has fainted. {Attacker.Name} wins!";
        }

        if (Attacker.IsFainted())
        {
            return $"{Attacker.Name} has fainted. {Defender.Name} wins!";
        }

        return "The battle is ongoing.";
    }
}
