using PokemonGameLib.Models;
using System;
using System.Linq;

/// <summary>
/// Manages the battle between two Trainers, handling Pokémon attacks and damage calculation.
/// </summary>
public class Battle
{
    /// <summary>
    /// Gets the first Trainer in the battle.
    /// </summary>
    public Trainer AttackingTrainer { get; }

    /// <summary>
    /// Gets the second Trainer in the battle.
    /// </summary>
    public Trainer DefendingTrainer { get; }

    /// <summary>
    /// Gets the current Pokémon of the attacking Trainer.
    /// </summary>
    public Pokemon? Attacker => AttackingTrainer.Pokemons.FirstOrDefault();

    /// <summary>
    /// Gets the current Pokémon of the defending Trainer.
    /// </summary>
    public Pokemon? Defender => DefendingTrainer.Pokemons.FirstOrDefault();

    /// <summary>
    /// Initializes a new instance of the <see cref="Battle"/> class.
    /// </summary>
    /// <param name="attackingTrainer">The Trainer initiating the attack.</param>
    /// <param name="defendingTrainer">The Trainer receiving the attack.</param>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="attackingTrainer"/> or <paramref name="defendingTrainer"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if either Trainer has no valid Pokémon.</exception>
    public Battle(Trainer attackingTrainer, Trainer defendingTrainer)
    {
        AttackingTrainer = attackingTrainer ?? throw new ArgumentNullException(nameof(attackingTrainer), "Attacking Trainer cannot be null.");
        DefendingTrainer = defendingTrainer ?? throw new ArgumentNullException(nameof(defendingTrainer), "Defending Trainer cannot be null.");

        if (!AttackingTrainer.HasValidPokemons())
            throw new InvalidOperationException("Attacking Trainer has no valid Pokémon.");
        if (!DefendingTrainer.HasValidPokemons())
            throw new InvalidOperationException("Defending Trainer has no valid Pokémon.");
    }

    /// <summary>
    /// Performs an attack by the specified Trainer's Pokémon on the opponent's Pokémon.
    /// </summary>
    /// <param name="attackingTrainer">The Trainer whose Pokémon is performing the attack.</param>
    /// <param name="move">The move used by the attacking Pokémon.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="move"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the attacking Pokémon is fainted, does not know the move, or if the defending Pokémon is fainted.</exception>
    public void PerformAttack(Trainer attackingTrainer, Move move)
    {
        if (move == null)
            throw new ArgumentNullException(nameof(move), "Move cannot be null.");

        Pokemon? attacker = attackingTrainer.Pokemons.FirstOrDefault();
        Trainer defendingTrainer = attackingTrainer == AttackingTrainer ? DefendingTrainer : AttackingTrainer;
        Pokemon? defender = defendingTrainer.Pokemons.FirstOrDefault();

        if (attacker == null || attacker.IsFainted())
            throw new InvalidOperationException("Attacking Trainer's Pokémon is fainted and cannot perform an attack.");
        
        if (!attacker.Moves.Contains(move))
            throw new InvalidOperationException("Attacking Trainer's Pokémon does not know the specified move.");
        
        if (defender == null || defender.IsFainted())
            throw new InvalidOperationException("Defending Trainer's Pokémon is fainted and cannot be attacked.");

        double effectiveness = TypeEffectiveness.GetEffectiveness(move.Type, defender.Type);
        int damage = CalculateDamage(attacker, defender, move, effectiveness);
        defender.TakeDamage(damage);

        Console.WriteLine($"{attacker.Name} used {move.Name}!");
        Console.WriteLine($"It's {GetEffectivenessText(effectiveness)}!");
        Console.WriteLine($"{defender.Name} took {damage} damage!");

        if (defender.IsFainted())
        {
            Console.WriteLine($"{defender.Name} has fainted!");
        }
    }

    /// <summary>
    /// Calculates the damage dealt by an attacking Pokémon to a defending Pokémon based on move power, attacker and defender stats, and type effectiveness.
    /// </summary>
    /// <param name="attacker">The attacking Pokémon.</param>
    /// <param name="defender">The defending Pokémon.</param>
    /// <param name="move">The move being used.</param>
    /// <param name="effectiveness">The type effectiveness multiplier.</param>
    /// <returns>The calculated damage.</returns>
    private int CalculateDamage(Pokemon attacker, Pokemon defender, Move move, double effectiveness)
    {
        // Base damage calculation
        double levelFactor = (2.0 * attacker.Level / 5.0) + 2.0;
        double powerFactor = move.Power * attacker.Attack / defender.Defense;
        double baseDamage = (levelFactor * powerFactor / 50.0) + 2.0;
        
        // Apply effectiveness
        double totalDamage = baseDamage * effectiveness;
        
        // Ensure damage is at least 1
        return (int)(totalDamage);
    }

    /// <summary>
    /// Gets a string representing the effectiveness of the move.
    /// </summary>
    /// <param name="effectiveness">The effectiveness multiplier.</param>
    /// <returns>A string describing the effectiveness.</returns>
    private string GetEffectivenessText(double effectiveness)
    {
        return effectiveness switch
        {
            2.0 => "super effective",
            0.5 => "not very effective",
            0.0 => "no effect",
            _ => "effective"
        };
    }

    /// <summary>
    /// Determines the result of the battle.
    /// </summary>
    /// <returns>A string indicating the result of the battle.</returns>
    public string DetermineBattleResult()
    {
        Pokemon? attacker = Attacker;
        Pokemon? defender = Defender;

        if (defender == null || defender.IsFainted())
        {
            return $"{DefendingTrainer.Name}'s {defender?.Name ?? "Pokémon"} has fainted. {AttackingTrainer.Name} wins!";
        }

        if (attacker == null || attacker.IsFainted())
        {
            return $"{AttackingTrainer.Name}'s {attacker?.Name ?? "Pokémon"} has fainted. {DefendingTrainer.Name} wins!";
        }

        return "The battle is ongoing.";
    }
}
