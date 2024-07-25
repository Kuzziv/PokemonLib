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
    /// <exception cref="ArgumentNullException">Thrown if either attackingTrainer or defendingTrainer is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if either trainer is invalid.</exception>
    public Battle(Trainer attackingTrainer, Trainer defendingTrainer)
    {
        if (attackingTrainer == null) throw new ArgumentNullException(nameof(attackingTrainer), "Attacking Trainer cannot be null.");
        if (defendingTrainer == null) throw new ArgumentNullException(nameof(defendingTrainer), "Defending Trainer cannot be null.");

        if (!attackingTrainer.HasValidPokemons()) throw new InvalidOperationException("Attacking Trainer is invalid.");
        if (!defendingTrainer.HasValidPokemons()) throw new InvalidOperationException("Defending Trainer is invalid.");

        AttackingTrainer = attackingTrainer;
        DefendingTrainer = defendingTrainer;
    }

    /// <summary>
    /// Performs an attack by the specified Trainer's Pokémon on the opponent's Pokémon.
    /// </summary>
    /// <param name="attackingTrainer">The Trainer whose Pokémon is performing the attack.</param>
    /// <param name="move">The move used by the attacking Pokémon.</param>
    /// <exception cref="ArgumentNullException">Thrown if the move is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the attacking Pokémon is fainted or does not know the move.</exception>
    public void PerformAttack(Trainer attackingTrainer, Move move)
    {
        if (move == null)
        {
            throw new ArgumentNullException(nameof(move), "Move cannot be null.");
        }

        Pokemon? attacker = attackingTrainer.Pokemons.FirstOrDefault();
        Trainer defendingTrainer = attackingTrainer == AttackingTrainer ? DefendingTrainer : AttackingTrainer;
        Pokemon? defender = defendingTrainer.Pokemons.FirstOrDefault();

        if (attacker == null || attacker.IsFainted())
        {
            throw new InvalidOperationException("Attacking Trainer's Pokémon is fainted and cannot perform an attack.");
        }

        if (!attacker.Moves.Contains(move))
        {
            throw new InvalidOperationException("Attacking Trainer's Pokémon does not know the specified move.");
        }

        if (defender == null || defender.IsFainted())
        {
            throw new InvalidOperationException("Defending Trainer's Pokémon is already fainted.");
        }

        // Calculate damage
        int damage = move.Power; // Simplified damage calculation
        defender.TakeDamage(damage);

        Console.WriteLine($"{attacker.Name} attacks {defender.Name} with {move.Name}!");
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
