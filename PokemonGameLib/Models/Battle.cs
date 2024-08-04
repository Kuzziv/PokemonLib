using PokemonGameLib.Models;
using System;
using System.Linq;

/// <summary>
/// Manages the battle between two Trainers, handling Pokémon attacks, damage calculation, and turn-based mechanics.
/// </summary>
public class Battle
{
    /// <summary>
    /// Gets the Trainer initiating the attack.
    /// </summary>
    public Trainer AttackingTrainer { get; }

    /// <summary>
    /// Gets the Trainer receiving the attack.
    /// </summary>
    public Trainer DefendingTrainer { get; }

    /// <summary>
    /// Indicates whether it is the attacking Trainer's turn.
    /// </summary>
    public bool IsAttackingTurn { get; private set; } = true;

    /// <summary>
    /// Gets the current Pokémon of the active Trainer.
    /// </summary>
    public Pokemon? Attacker => IsAttackingTurn ? AttackingTrainer.Pokemons.FirstOrDefault(p => !p.IsFainted()) : DefendingTrainer.Pokemons.FirstOrDefault(p => !p.IsFainted());

    /// <summary>
    /// Gets the current Pokémon of the opposing Trainer.
    /// </summary>
    public Pokemon? Defender => IsAttackingTurn ? DefendingTrainer.Pokemons.FirstOrDefault(p => !p.IsFainted()) : AttackingTrainer.Pokemons.FirstOrDefault(p => !p.IsFainted());

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
    /// Performs an attack by the current active Pokémon.
    /// </summary>
    /// <param name="move">The move used by the attacking Pokémon.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="move"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the attacking Pokémon is fainted, does not know the move, or if the defending Pokémon is fainted.</exception>
    public void PerformAttack(Move move)
    {
        if (move == null)
            throw new ArgumentNullException(nameof(move), "Move cannot be null.");

        Pokemon? attacker = Attacker;
        Pokemon? defender = Defender;

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

            // Handle Pokémon switching if the Trainer has other Pokémon
            if (DefendingTrainer.HasValidPokemons())
            {
                Console.WriteLine($"{DefendingTrainer.Name}, please select your next Pokémon.");
                Pokemon? newPokemon = GetPokemonSelection(DefendingTrainer);

                if (newPokemon == null || newPokemon.IsFainted())
                    throw new InvalidOperationException("Selected Pokémon is fainted or invalid.");

                SwitchPokemon(DefendingTrainer, newPokemon);
            }
            else
            {
                Console.WriteLine($"{DefendingTrainer.Name} has no more Pokémon. {AttackingTrainer.Name} wins!");
                return;
            }
        }

        // Switch turns
        IsAttackingTurn = !IsAttackingTurn;
    }

    /// <summary>
    /// Prompts the Trainer to select a new Pokémon to switch in.
    /// </summary>
    /// <param name="trainer">The Trainer who needs to select a new Pokémon.</param>
    /// <returns>The selected Pokémon.</returns>
    private Pokemon? GetPokemonSelection(Trainer trainer)
    {
        if (trainer == null) throw new ArgumentNullException(nameof(trainer), "Trainer cannot be null.");
        var availablePokemons = trainer.Pokemons.Where(p => !p.IsFainted()).ToList();

        if (!availablePokemons.Any())
            throw new InvalidOperationException("No available Pokémon to switch in.");

        Console.WriteLine($"Available Pokémon for {trainer.Name}:");
        for (int i = 0; i < availablePokemons.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {availablePokemons[i].Name}");
        }

        int selectedIndex = -1;
        while (selectedIndex < 0 || selectedIndex >= availablePokemons.Count)
        {
            Console.WriteLine("Select the number of the Pokémon you want to switch to:");
            if (int.TryParse(Console.ReadLine(), out selectedIndex))
            {
                selectedIndex--; // Convert to zero-based index
            }
        }

        return availablePokemons[selectedIndex];
    }

    /// <summary>
    /// Switches the current Pokémon of the specified Trainer with another Pokémon from the Trainer's team.
    /// </summary>
    /// <param name="trainer">The Trainer whose Pokémon will be switched.</param>
    /// <param name="newPokemon">The new Pokémon to switch in.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="trainer"/> or <paramref name="newPokemon"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the new Pokémon is not in the Trainer's team.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the new Pokémon is fainted.</exception>
    public void SwitchPokemon(Trainer trainer, Pokemon newPokemon)
    {
        if (trainer == null)
            throw new ArgumentNullException(nameof(trainer), "Trainer cannot be null.");
        if (newPokemon == null)
            throw new ArgumentNullException(nameof(newPokemon), "New Pokémon cannot be null.");

        // Ensure the new Pokémon is in the Trainer's list
        if (!trainer.Pokemons.Contains(newPokemon))
            throw new ArgumentException("The new Pokémon must be one of the Trainer's current Pokémon.");

        // Ensure the new Pokémon is not fainted
        if (newPokemon.IsFainted())
            throw new InvalidOperationException("The new Pokémon cannot be switched in because it is fainted.");

        // Perform the switch
        trainer.RemovePokemon(Attacker);
        trainer.AddPokemon(newPokemon);
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

        // Ensure damage is at least a whole number
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
    /// Determines the result of the battle based on the state of the Pokémon.
    /// </summary>
    /// <returns>A string indicating the result of the battle.</returns>
    public string DetermineBattleResult()
    {
        // Check if the trainers are null
        if (AttackingTrainer == null || DefendingTrainer == null)
        {
            throw new InvalidOperationException("Battle cannot be determined; trainers cannot be null.");
        }

        // Determine if all Pokémon of the attacking trainer are fainted
        bool attackerAllFainted = AttackingTrainer.Pokemons.All(p => p.IsFainted());

        // Determine if all Pokémon of the defending trainer are fainted
        bool defenderAllFainted = DefendingTrainer.Pokemons.All(p => p.IsFainted());

        if (attackerAllFainted && defenderAllFainted)
        {
            return "All Pokémon have fainted. It's a draw!";
        }

        if (attackerAllFainted)
        {
            return $"{AttackingTrainer.Name}'s Pokémon have all fainted. {DefendingTrainer.Name} wins!";
        }

        if (defenderAllFainted)
        {
            return $"{DefendingTrainer.Name}'s Pokémon have all fainted. {AttackingTrainer.Name} wins!";
        }

        // If neither trainer's Pokémon are all fainted, the battle is still ongoing
        return "The battle is ongoing.";
    }
}
