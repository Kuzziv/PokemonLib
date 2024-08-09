// Pokemon.cs
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonGameLib.Models.Abilities;
using PokemonGameLib.Models.Moves;
using PokemonGameLib.Models.Evolutions;

namespace PokemonGameLib.Models.Pokemons
{
    /// <summary>
    /// Represents a Pokémon with properties for name, type, level, stats, abilities, and methods to manage its state.
    /// </summary>
    public class Pokemon
    {
        /// <summary>
        /// Gets the name of the Pokémon.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the Pokémon.
        /// </summary>
        public PokemonType Type { get; }

        /// <summary>
        /// Gets the level of the Pokémon.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the maximum HP of the Pokémon.
        /// </summary>
        public int MaxHP { get; private set; }

        /// <summary>
        /// Gets the current HP of the Pokémon.
        /// </summary>
        public int CurrentHP { get; private set; }

        /// <summary>
        /// Gets the attack stat of the Pokémon.
        /// </summary>
        public int Attack { get; private set; }

        /// <summary>
        /// Gets the defense stat of the Pokémon.
        /// </summary>
        public int Defense { get; private set; }

        /// <summary>
        /// Gets the list of moves the Pokémon knows.
        /// </summary>
        public List<Move> Moves { get; private set; }

        /// <summary>
        /// Gets the list of abilities the Pokémon has.
        /// </summary>
        public List<Ability> Abilities { get; }

        /// <summary>
        /// Gets the status condition of the Pokémon.
        /// </summary>
        public StatusCondition Status { get; private set; }

        /// <summary>
        /// Gets the list of possible evolutions for the Pokémon.
        /// </summary>
        public List<Evolution> Evolutions { get; }

        // Private field to manage sleep duration
        private int sleepCounter = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pokemon"/> class.
        /// </summary>
        /// <param name="name">The name of the Pokémon.</param>
        /// <param name="type">The type of the Pokémon.</param>
        /// <param name="level">The level of the Pokémon.</param>
        /// <param name="maxHp">The maximum HP of the Pokémon.</param>
        /// <param name="attack">The attack stat of the Pokémon.</param>
        /// <param name="defense">The defense stat of the Pokémon.</param>
        /// <param name="abilities">The list of abilities the Pokémon has.</param>
        /// <param name="evolutions">The list of possible evolutions for the Pokémon.</param>
        public Pokemon(string name, PokemonType type, int level, int maxHp, int attack, int defense, List<Ability> abilities = null, List<Evolution> evolutions = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Pokemon name cannot be null or whitespace.");
            if (!Enum.IsDefined(typeof(PokemonType), type))
                throw new ArgumentException("Invalid Pokémon type.", nameof(type));
            if (level < 1)
                throw new ArgumentOutOfRangeException(nameof(level), "Pokemon level cannot be less than 1.");
            if (maxHp < 1)
                throw new ArgumentOutOfRangeException(nameof(maxHp), "Pokemon Max HP must be greater than 0.");
            if (attack < 0)
                throw new ArgumentOutOfRangeException(nameof(attack), "Pokemon attack cannot be negative.");
            if (defense < 0)
                throw new ArgumentOutOfRangeException(nameof(defense), "Pokemon defense cannot be negative.");

            Name = name;
            Type = type;
            Level = level;
            MaxHP = maxHp;
            CurrentHP = maxHp; // Initialize Current HP to Max HP
            Attack = attack;
            Defense = defense;
            Moves = new List<Move>();
            Abilities = abilities ?? new List<Ability>();
            Evolutions = evolutions ?? new List<Evolution>();
            Status = StatusCondition.None;
        }

        /// <summary>
        /// Levels up the Pokémon, increasing its level and improving its stats.
        /// </summary>
        public void LevelUp()
        {
            Level++;

            // Simple stat increase logic
            MaxHP += 10; // Increase MaxHP by a fixed amount
            Attack += 5; // Increase Attack by a fixed amount
            Defense += 5; // Increase Defense by a fixed amount

            // Ensure CurrentHP does not exceed MaxHP
            CurrentHP = Math.Min(CurrentHP + 10, MaxHP);

            Console.WriteLine($"{Name} has leveled up to level {Level}!");
        }

        /// <summary>
        /// Applies damage to the Pokémon.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        public void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                CurrentHP -= damage;
                if (CurrentHP < 0) CurrentHP = 0;
            }
        }

        /// <summary>
        /// Heals the Pokémon by a specified amount.
        /// </summary>
        /// <param name="amount">The amount to heal.</param>
        public void Heal(int amount)
        {
            CurrentHP += amount;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        }

        /// <summary>
        /// Lowers a specific stat of the Pokémon by a specified amount.
        /// </summary>
        /// <param name="stat">The stat to lower (e.g., "Attack" or "Defense").</param>
        /// <param name="amount">The amount to lower the stat by.</param>
        public void LowerStat(string stat, int amount)
        {
            if (stat == "Attack") Attack -= amount;
            if (stat == "Defense") Defense -= amount;
        }

        /// <summary>
        /// Determines whether the Pokémon has fainted.
        /// </summary>
        /// <returns><c>true</c> if the Pokémon's HP is less than or equal to 0; otherwise, <c>false</c>.</returns>
        public bool IsFainted() => CurrentHP <= 0;

        /// <summary>
        /// Adds a move to the Pokémon's move list.
        /// </summary>
        /// <param name="move">The move to add to the Pokémon's move list.</param>
        /// <exception cref="ArgumentNullException">Thrown if the move is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the Pokémon already knows the maximum number of moves.</exception>
        /// <exception cref="ArgumentException">Thrown if the move is already known by the Pokémon, is not compatible with the Pokémon's type, or is not compatible with the Pokémon's level.</exception>
        public void AddMove(Move move)
        {
            if (move == null)
                throw new ArgumentNullException(nameof(move), "Move cannot be null.");
            if (Moves.Count >= 4)
                throw new InvalidOperationException("A Pokémon can only know up to 4 moves.");
            if (Moves.Contains(move))
                throw new ArgumentException("Pokémon already knows this move.", nameof(move));
            if (move.Type != Type)
                throw new ArgumentException("Move is not compatible with the Pokémon's type.", nameof(move));
            if (move.Level > Level)
                throw new ArgumentException("Move is not compatible with the Pokémon's level.", nameof(move));

            Moves.Add(move);
        }

        /// <summary>
        /// Inflicts a status condition on the Pokémon.
        /// </summary>
        /// <param name="status">The status condition to inflict.</param>
        public void InflictStatus(StatusCondition status)
        {
            // Only apply the status if the Pokémon is not already affected by another condition
            if (Status == StatusCondition.None)
            {
                Status = status;

                switch (status)
                {
                    case StatusCondition.Paralysis:
                        Console.WriteLine($"{Name} is paralyzed and may not be able to move!");
                        break;
                        
                    case StatusCondition.Burn:
                        Console.WriteLine($"{Name} is burned and will take damage each turn!");
                        break;
                        
                    case StatusCondition.Poison:
                        Console.WriteLine($"{Name} is poisoned and will take damage each turn!");
                        break;
                        
                    case StatusCondition.Sleep:
                        Console.WriteLine($"{Name} has fallen asleep and can't move!");
                        break;
                        
                    case StatusCondition.Freeze:
                        Console.WriteLine($"{Name} is frozen solid and can't move!");
                        break;
                }
            }
        }

        /// <summary>
        /// Applies the effects of the current status condition on the Pokémon.
        /// This method should be called at the end of each turn to process ongoing status effects.
        /// </summary>
        public void ApplyStatusEffects()
        {
            switch (Status)
            {
                case StatusCondition.Burn:
                    // Burn deals damage equal to 1/16 of MaxHP each turn
                    int burnDamage = MaxHP / 16;
                    TakeDamage(burnDamage);
                    Console.WriteLine($"{Name} is hurt by its burn and takes {burnDamage} damage!");
                    break;

                case StatusCondition.Poison:
                    // Poison deals damage equal to 1/8 of MaxHP each turn
                    int poisonDamage = MaxHP / 8;
                    TakeDamage(poisonDamage);
                    Console.WriteLine($"{Name} is hurt by poison and takes {poisonDamage} damage!");
                    break;

                case StatusCondition.Paralysis:
                    // 25% chance to be unable to move due to paralysis
                    if (new Random().NextDouble() < 0.25)
                    {
                        Console.WriteLine($"{Name} is paralyzed and can't move!");
                        // You can add logic here to skip the attack or action
                    }
                    break;

                case StatusCondition.Sleep:
                    // Sleep prevents action for a set number of turns, decrement the sleep counter
                    if (sleepCounter > 0)
                    {
                        sleepCounter--;
                        Console.WriteLine($"{Name} is fast asleep.");
                    }
                    else
                    {
                        Console.WriteLine($"{Name} woke up!");
                        CureStatus(); // Cure sleep once the counter hits zero
                    }
                    break;

                case StatusCondition.Freeze:
                    // 20% chance to thaw each turn
                    if (new Random().NextDouble() < 0.20)
                    {
                        Console.WriteLine($"{Name} thawed out!");
                        CureStatus();
                    }
                    else
                    {
                        Console.WriteLine($"{Name} is frozen solid and can't move!");
                        // Logic to skip attack or action goes here
                    }
                    break;
            }
        }

        /// <summary>
        /// Cures the Pokémon of any status condition.
        /// </summary>
        public void CureStatus()
        {
            Status = StatusCondition.None;
            Console.WriteLine($"{Name} is cured of its status condition!");
        }

        /// <summary>
        /// Sets the sleep duration when the Pokémon falls asleep.
        /// </summary>
        /// <param name="duration">The number of turns the Pokémon will remain asleep.</param>
        public void SetSleepDuration(int duration)
        {
            sleepCounter = duration;
        }

        /// <summary>
        /// Determines whether the Pokémon can evolve based on its current state and evolution criteria.
        /// </summary>
        /// <returns><c>true</c> if the Pokémon can evolve; otherwise, <c>false</c>.</returns>
        public bool CanEvolve()
        {
            foreach (var evolution in Evolutions)
            {
                if (evolution.CanEvolve(this))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evolves the Pokémon to its next form if the criteria are met.
        /// </summary>
        public void Evolve()
        {
            foreach (var evolution in Evolutions)
            {
                if (evolution.CanEvolve(this))
                {
                    Console.WriteLine($"{Name} evolved into {evolution.EvolvedFormName}!");
                    Name = evolution.EvolvedFormName;
                    Level++; // Example stat increase
                    MaxHP += 20; // Increase Max HP upon evolution
                    Attack += 10; // Increase Attack upon evolution
                    Defense += 10; // Increase Defense upon evolution
                    CurrentHP = MaxHP; // Heal to full HP upon evolution
                    break; // Add break to prevent multiple evolutions
                }
            }
        }
    }
}
