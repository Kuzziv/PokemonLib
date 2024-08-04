using System;
using System.Linq;
using PokemonGameLib.Models; // Add this using directive to ensure access to Trainer and Pokemon

namespace PokemonGameLib.Models
{
    /// <summary>
    /// Represents a Pokémon battle between two trainers.
    /// </summary>
    public class Battle
    {
        private bool isFirstTrainerAttacking; // A flag to keep track of the turn

        /// <summary>
        /// Gets the first trainer in the battle.
        /// </summary>
        public Trainer FirstTrainer { get; private set; }

        /// <summary>
        /// Gets the second trainer in the battle.
        /// </summary>
        public Trainer SecondTrainer { get; private set; }

        /// <summary>
        /// Gets the current attacking trainer based on the turn.
        /// </summary>
        public Trainer AttackingTrainer => isFirstTrainerAttacking ? FirstTrainer : SecondTrainer;

        /// <summary>
        /// Gets the current defending trainer based on the turn.
        /// </summary>
        public Trainer DefendingTrainer => isFirstTrainerAttacking ? SecondTrainer : FirstTrainer;

        /// <summary>
        /// Gets the current attacking Pokémon based on the turn.
        /// </summary>
        private Pokemon? Attacker => AttackingTrainer.CurrentPokemon; // Mark as nullable

        /// <summary>
        /// Gets the current defending Pokémon based on the turn.
        /// </summary>
        private Pokemon? Defender => DefendingTrainer.CurrentPokemon; // Mark as nullable

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="firstTrainer">The first trainer in the battle.</param>
        /// <param name="secondTrainer">The second trainer in the battle.</param>
        /// <exception cref="ArgumentNullException">Thrown when either trainer is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when either trainer has no valid Pokémon to battle.</exception>
        public Battle(Trainer firstTrainer, Trainer secondTrainer)
        {
            if (firstTrainer == null || secondTrainer == null)
                throw new ArgumentNullException("Trainers cannot be null");

            if (!firstTrainer.HasValidPokemon() || !secondTrainer.HasValidPokemon())
                throw new InvalidOperationException("Both trainers must have valid Pokémon to start the battle");

            FirstTrainer = firstTrainer;
            SecondTrainer = secondTrainer;
            isFirstTrainerAttacking = true; // Set the initial turn
        }

        /// <summary>
        /// Performs an attack using the specified move.
        /// </summary>
        /// <param name="move">The move to be used for the attack.</param>
        /// <exception cref="InvalidOperationException">Thrown when the move is not valid for the attacker.</exception>
        public void PerformAttack(Move move)
        {
            if (Attacker == null || !Attacker.Moves.Contains(move))
                throw new InvalidOperationException("Invalid move for the current attacker");

            if (Defender == null)
                throw new InvalidOperationException("Defender cannot be null");

            double effectiveness = CalculateEffectiveness(move.Type, Defender.Type);
            double damage = CalculateDamage(move.Power, Attacker.Attack, Defender.Defense, effectiveness, move.Type);
            Defender.TakeDamage((int)damage);

            Console.WriteLine($"{Attacker.Name} used {move.Name}!");
            Console.WriteLine(EffectivenessMessage(effectiveness));
            Console.WriteLine($"{Defender.Name} took {damage} damage!");

            // Switch turns after the attack
            isFirstTrainerAttacking = !isFirstTrainerAttacking;
        }

        /// <summary>
        /// Calculates the effectiveness of a move based on types.
        /// </summary>
        /// <param name="moveType">The type of the move.</param>
        /// <param name="pokemonType">The type of the defending Pokémon.</param>
        /// <returns>The effectiveness multiplier.</returns>
        private double CalculateEffectiveness(PokemonType moveType, PokemonType pokemonType)
        {
            return TypeEffectiveness.GetEffectiveness(moveType, pokemonType);
        }

        /// <summary>
        /// Calculates the damage dealt by a move, incorporating factors like effectiveness, STAB (Same Type Attack Bonus), 
        /// critical hits, and a random factor to simulate variability in battle outcomes.
        /// </summary>
        /// <param name="movePower">The base power of the move being used, which determines the move's strength.</param>
        /// <param name="attackerAttack">The attack stat of the attacking Pokémon, affecting the potency of physical attacks.</param>
        /// <param name="defenderDefense">The defense stat of the defending Pokémon, reducing damage from incoming physical attacks.</param>
        /// <param name="effectiveness">
        /// A multiplier representing the effectiveness of the move against the defender's type. 
        /// Values greater than 1.0 indicate super effectiveness, values less than 1.0 indicate reduced effectiveness, 
        /// and a value of 0 means no effect.
        /// </param>
        /// <param name="moveType">The type of the move being used, which interacts with the defender's type to determine effectiveness.</param>
        /// <returns>The total calculated damage to be dealt to the defender, after applying all modifiers.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when either the attacker or the defender Pokémon is null, indicating a misconfiguration of the battle state.
        /// </exception>
        private double CalculateDamage(int movePower, int attackerAttack, int defenderDefense, double effectiveness, PokemonType moveType)
        {
            if (Attacker == null)
            {
                throw new InvalidOperationException("Attacker cannot be null.");
            }

            if (Defender == null)
            {
                throw new InvalidOperationException("Defender cannot be null.");
            }
            
            double randomFactor = new Random().Next(85, 101) / 100.0; // Random factor between 0.85 and 1.0
            double stab = Attacker.Type == moveType ? 1.5 : 1.0; // STAB
            double critical = new Random().NextDouble() < 0.0625 ? 2.0 : 1.0; // 6.25% chance for a critical hit

            return (((2 * Attacker.Level / 5.0 + 2) * movePower * (attackerAttack / (double)defenderDefense) / 50.0) + 2) * effectiveness * stab * critical * randomFactor;
        }

        /// <summary>
        /// Provides a message based on the effectiveness of the move.
        /// </summary>
        /// <param name="effectiveness">The effectiveness multiplier.</param>
        /// <returns>A string message describing the effectiveness.</returns>
        private string EffectivenessMessage(double effectiveness)
        {
            if (effectiveness > 1.0) return "It's super effective!";
            if (effectiveness < 1.0) return "It's not very effective!";
            return "It's effective!";
        }

        /// <summary>
        /// Determines the result of the battle.
        /// </summary>
        /// <returns>A string describing the result of the battle.</returns>
        public string DetermineBattleResult()
        {
            if (Attacker != null && Attacker.IsFainted())
                return $"{AttackingTrainer.Name}'s {Attacker.Name} has fainted. {DefendingTrainer.Name} wins!";
            if (Defender != null && Defender.IsFainted())
                return $"{DefendingTrainer.Name}'s {Defender.Name} has fainted. {AttackingTrainer.Name} wins!";
            return "The battle is ongoing.";
        }

        /// <summary>
        /// Switches the current attacking Pokémon to a new Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer requesting the switch.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="InvalidOperationException">Thrown when attempting to switch to a fainted Pokémon or a Pokémon not owned by the trainer.</exception>
        public void SwitchPokemon(Trainer trainer, Pokemon newPokemon)
        {
            if (!trainer.Pokemons.Contains(newPokemon))
                throw new InvalidOperationException("Cannot switch to a Pokémon not owned by the trainer");

            if (newPokemon.IsFainted())
                throw new InvalidOperationException("Cannot switch to a fainted Pokémon");

            trainer.CurrentPokemon = newPokemon;
            Console.WriteLine($"{trainer.Name} switched to {newPokemon.Name}!");
        }
    }
}
