using System;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Services;

namespace PokemonGameLib.Models.Battles
{
    /// <summary>
    /// Represents a Pokémon battle between two trainers, handling battle mechanics and interactions.
    /// </summary>
    public class Battle : IBattle
    {
        private readonly ITypeEffectivenessService _typeEffectivenessService;
        private readonly Random _random = new();
        private bool _isFirstTrainerAttacking;

        /// <summary>
        /// Gets the first trainer in the battle.
        /// </summary>
        public ITrainer FirstTrainer { get; }

        /// <summary>
        /// Gets the second trainer in the battle.
        /// </summary>
        public ITrainer SecondTrainer { get; }

        /// <summary>
        /// Gets the trainer currently attacking based on the turn.
        /// </summary>
        public ITrainer AttackingTrainer => _isFirstTrainerAttacking ? FirstTrainer : SecondTrainer;

        /// <summary>
        /// Gets the current defending trainer based on the turn.
        /// </summary>
        public ITrainer DefendingTrainer => _isFirstTrainerAttacking ? SecondTrainer : FirstTrainer;

        private IPokemon Attacker => AttackingTrainer.CurrentPokemon;
        private IPokemon Defender => DefendingTrainer.CurrentPokemon;

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="firstTrainer">The first trainer in the battle.</param>
        /// <param name="secondTrainer">The second trainer in the battle.</param>
        /// <param name="typeEffectivenessService">Service for calculating type effectiveness.</param>
        public Battle(ITrainer firstTrainer, ITrainer secondTrainer, ITypeEffectivenessService typeEffectivenessService)
        {
            FirstTrainer = firstTrainer ?? throw new ArgumentNullException(nameof(firstTrainer));
            SecondTrainer = secondTrainer ?? throw new ArgumentNullException(nameof(secondTrainer));
            _typeEffectivenessService = typeEffectivenessService ?? throw new ArgumentNullException(nameof(typeEffectivenessService));

            ValidateTrainers();

            _isFirstTrainerAttacking = true;
        }

        /// <summary>
        /// Validates that both trainers have valid, non-fainted Pokémon.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if a trainer has no valid Pokémon to battle with.</exception>
        private void ValidateTrainers()
        {
            if (!FirstTrainer.Pokemons.Any(p => !p.IsFainted()) || !SecondTrainer.Pokemons.Any(p => !p.IsFainted()))
            {
                throw new InvalidOperationException("Both trainers must have valid Pokémon to start the battle.");
            }
        }

        /// <summary>
        /// Performs an attack using the specified move.
        /// </summary>
        /// <param name="move">The move to be used for the attack.</param>
        /// <exception cref="InvalidOperationException">Thrown if any condition for performing the attack is invalid.</exception>
        public void PerformAttack(IMove move)
        {
            ValidateAttackConditions(move);

            for (int i = 0; i < move.MaxHits; i++)
            {
                ExecuteAttack(move);

                if (Defender.IsFainted())
                {
                    Console.WriteLine($"{Defender.Name} has fainted!");
                    break;
                }
            }

            ApplyRecoilAndHealing(move);

            _isFirstTrainerAttacking = !_isFirstTrainerAttacking;
        }

        /// <summary>
        /// Validates that the current attack conditions are valid.
        /// </summary>
        /// <param name="move">The move being validated.</param>
        /// <exception cref="InvalidOperationException">Thrown if any condition is invalid.</exception>
        private void ValidateAttackConditions(IMove move)
        {
            if (Attacker == null) throw new InvalidOperationException("Attacker cannot be null.");
            if (Attacker.IsFainted()) throw new InvalidOperationException($"{Attacker.Name} is fainted and cannot attack.");
            if (!Attacker.Moves.Contains(move)) throw new InvalidOperationException("Invalid move for the current attacker.");
            if (Defender == null) throw new InvalidOperationException("Defender cannot be null.");
            if (Defender.IsFainted()) throw new InvalidOperationException($"{Defender.Name} is fainted and cannot be attacked.");
        }

        /// <summary>
        /// Executes the attack, calculating damage and applying effects.
        /// </summary>
        /// <param name="move">The move to be executed.</param>
        private void ExecuteAttack(IMove move)
        {
            double effectiveness = _typeEffectivenessService.GetEffectiveness(move.Type, Defender.Type);
            double damage = CalculateDamage(move.Power, Attacker.Attack, Defender.Defense, effectiveness, move.Type);
            Defender.TakeDamage((int)damage);

            Console.WriteLine($"{Attacker.Name} used {move.Name}!");
            Console.WriteLine(GetEffectivenessMessage(effectiveness));
            Console.WriteLine($"{Defender.Name} took {damage} damage!");
        }

        /// <summary>
        /// Applies recoil damage to the attacker and healing effects if applicable.
        /// </summary>
        /// <param name="move">The move being used.</param>
        private void ApplyRecoilAndHealing(IMove move)
        {
            if (move.RecoilPercentage > 0)
            {
                int recoilDamage = (int)(move.Power * (move.RecoilPercentage / 100.0));
                Attacker.TakeDamage(recoilDamage);
                Console.WriteLine($"{Attacker.Name} took {recoilDamage} recoil damage!");
            }

            if (move.HealingPercentage > 0)
            {
                int healingAmount = (int)(Attacker.MaxHP * (move.HealingPercentage / 100.0));
                Attacker.Heal(healingAmount);
                Console.WriteLine($"{Attacker.Name} healed {healingAmount} HP!");
            }
        }

        /// <summary>
        /// Calculates the damage dealt by a move.
        /// </summary>
        /// <param name="movePower">The base power of the move.</param>
        /// <param name="attackerAttack">The attack stat of the attacking Pokémon.</param>
        /// <param name="defenderDefense">The defense stat of the defending Pokémon.</param>
        /// <param name="effectiveness">The effectiveness multiplier.</param>
        /// <param name="moveType">The type of the move.</param>
        /// <returns>The total calculated damage.</returns>
        private double CalculateDamage(int movePower, int attackerAttack, int defenderDefense, double effectiveness, PokemonType moveType)
        {
            double randomFactor = _random.Next(85, 101) / 100.0;
            double stab = Attacker.Type == moveType ? 1.5 : 1.0;
            double critical = _random.NextDouble() < 0.0625 ? 2.0 : 1.0;

            return (((2 * Attacker.Level / 5.0 + 2) * movePower * (attackerAttack / (double)defenderDefense) / 50.0) + 2)
                   * effectiveness * stab * critical * randomFactor;
        }

        /// <summary>
        /// Generates a message based on the effectiveness of the move.
        /// </summary>
        /// <param name="effectiveness">The effectiveness multiplier.</param>
        /// <returns>A string message describing the effectiveness.</returns>
        private static string GetEffectivenessMessage(double effectiveness)
        {
            return effectiveness switch
            {
                > 1.0 => "It's super effective!",
                < 1.0 => "It's not very effective!",
                _ => "It's effective!"
            };
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
        /// Switches the current Pokémon of a trainer with another Pokémon in their party.
        /// </summary>
        /// <param name="trainer">The trainer switching Pokémon.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        public void SwitchPokemon(ITrainer trainer, IPokemon newPokemon)
        {
            ValidateSwitchConditions(trainer, newPokemon);
            trainer.CurrentPokemon = newPokemon;
            Console.WriteLine($"{trainer.Name} switched to {newPokemon.Name}!");
        }

        /// <summary>
        /// Validates the conditions for switching Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer switching Pokémon.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="ArgumentNullException">Thrown if trainer or newPokemon is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the switch is invalid.</exception>
        private static void ValidateSwitchConditions(ITrainer trainer, IPokemon newPokemon)
        {
            if (trainer == null) throw new ArgumentNullException(nameof(trainer));
            if (newPokemon == null) throw new ArgumentNullException(nameof(newPokemon));

            if (!trainer.Pokemons.Contains(newPokemon))
                throw new InvalidOperationException("Trainer does not own the specified Pokémon.");

            if (trainer.CurrentPokemon == newPokemon)
                throw new InvalidOperationException("Trainer is already using the specified Pokémon.");

            if (newPokemon.IsFainted())
                throw new InvalidOperationException("Cannot switch to a fainted Pokémon.");
        }
    }
}
