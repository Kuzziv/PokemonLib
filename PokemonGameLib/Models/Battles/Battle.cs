using System;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;
using PokemonGameLib.Utilities;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Models.Battles
{
    /// <summary>
    /// Represents a Pokémon battle between two trainers, handling battle mechanics and interactions.
    /// </summary>
    public class Battle : IBattle
    {
        private readonly ITypeEffectivenessService _typeEffectivenessService;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ILogger _logger;
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

        /// <summary>
        /// Gets the current attacking Pokémon.
        /// </summary>
        private IPokemon Attacker => AttackingTrainer.CurrentPokemon;

        /// <summary>
        /// Gets the current defending Pokémon.
        /// </summary>
        private IPokemon Defender => DefendingTrainer.CurrentPokemon;

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="firstTrainer">The first trainer in the battle.</param>
        /// <param name="secondTrainer">The second trainer in the battle.</param>
        /// <param name="typeEffectivenessService">Service for calculating type effectiveness.</param>
        /// <param name="randomNumberGenerator">Service for generating random numbers.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any of the required dependencies (trainers, services) are null.
        /// </exception>
        public Battle(
            ITrainer firstTrainer,
            ITrainer secondTrainer,
            ITypeEffectivenessService typeEffectivenessService,
            RandomNumberGenerator randomNumberGenerator)
        {
            FirstTrainer = firstTrainer ?? throw new ArgumentNullException(nameof(firstTrainer));
            SecondTrainer = secondTrainer ?? throw new ArgumentNullException(nameof(secondTrainer));
            _typeEffectivenessService = typeEffectivenessService ?? throw new ArgumentNullException(nameof(typeEffectivenessService));
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
            _logger = LoggingService.GetLogger(); // Retrieve the logger from the LoggingService

            ValidateTrainers();
            _isFirstTrainerAttacking = true;
        }

        /// <summary>
        /// Validates that both trainers have valid, non-fainted Pokémon.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a trainer has no valid Pokémon to battle with.
        /// </exception>
        private void ValidateTrainers()
        {
            if (!FirstTrainer.Pokemons.Any(p => !p.IsFainted()) || !SecondTrainer.Pokemons.Any(p => !p.IsFainted()))
            {
                _logger.LogError("One or both trainers have no valid Pokémon to start the battle.");
                throw new InvalidOperationException("Both trainers must have valid Pokémon to start the battle.");
            }
        }

        /// <summary>
        /// Performs an attack using the specified move.
        /// </summary>
        /// <param name="move">The move to be used for the attack.</param>
        /// <exception cref="InvalidMoveException">Thrown if the move is invalid for the current battle state.</exception>
        public void PerformAttack(IMove move)
        {
            // Apply status effects before attacking (e.g., Paralysis might prevent the move)
            Attacker.ApplyStatusEffects();

            if (Attacker.IsFainted())
            {
                _logger.LogInfo($"{Attacker.Name} is fainted and cannot attack.");
                return;
            }

            ValidateAttackConditions(move);

            for (int i = 0; i < move.MaxHits; i++)
            {
                ExecuteAttack(move);

                if (Defender.IsFainted())
                {
                    _logger.LogInfo($"{Defender.Name} has fainted!");
                    Console.WriteLine($"{Defender.Name} has fainted!");
                    break;
                }

                // Apply status effects after the attack (e.g., Poison might deal damage)
                Defender.ApplyStatusEffects();

                if (Defender.IsFainted())
                {
                    _logger.LogInfo($"{Defender.Name} fainted from status effects.");
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
        /// <exception cref="InvalidMoveException">Thrown if any condition is invalid.</exception>
        private void ValidateAttackConditions(IMove move)
        {
            if (Attacker == null) throw new InvalidMoveException("Attacker cannot be null.");
            if (Attacker.IsFainted()) throw new InvalidMoveException($"{Attacker.Name} is fainted and cannot attack.");
            if (!Attacker.Moves.Contains(move)) throw new InvalidMoveException("Invalid move for the current attacker.");
            if (Defender == null) throw new InvalidMoveException("Defender cannot be null.");
            if (Defender.IsFainted()) throw new InvalidMoveException($"{Defender.Name} is fainted and cannot be attacked.");
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

            _logger.LogInfo($"{Attacker.Name} used {move.Name}! {Defender.Name} took {damage} damage.");

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
                _logger.LogInfo($"{Attacker.Name} took {recoilDamage} recoil damage!");
                Console.WriteLine($"{Attacker.Name} took {recoilDamage} recoil damage!");
            }

            if (move.HealingPercentage > 0)
            {
                int healingAmount = (int)(Attacker.MaxHP * (move.HealingPercentage / 100.0));
                Attacker.Heal(healingAmount);
                _logger.LogInfo($"{Attacker.Name} healed {healingAmount} HP!");
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
            double randomFactor = _randomNumberGenerator.Generate(0.85, 1.0);
            double stab = Attacker.Type == moveType ? 1.5 : 1.0;
            double critical = _randomNumberGenerator.Generate(0.0, 1.0) < 0.0625 ? 2.0 : 1.0;

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
            {
                _logger.LogInfo($"{AttackingTrainer.Name}'s {Attacker.Name} has fainted. {DefendingTrainer.Name} wins!");
                return $"{AttackingTrainer.Name}'s {Attacker.Name} has fainted. {DefendingTrainer.Name} wins!";
            }
            if (Defender != null && Defender.IsFainted())
            {
                _logger.LogInfo($"{DefendingTrainer.Name}'s {Defender.Name} has fainted. {AttackingTrainer.Name} wins!");
                return $"{DefendingTrainer.Name}'s {Defender.Name} has fainted. {AttackingTrainer.Name} wins!";
            }
            return "The battle is ongoing.";
        }

        /// <summary>
        /// Switches the current Pokémon of a trainer with another Pokémon in their party.
        /// </summary>
        /// <param name="trainer">The trainer switching Pokémon.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="InvalidPokemonSwitchException">Thrown if the switch conditions are invalid.</exception>
        public void SwitchPokemon(ITrainer trainer, IPokemon newPokemon)
        {
            ValidateSwitchConditions(trainer, newPokemon);
            trainer.CurrentPokemon = newPokemon;
            _logger.LogInfo($"{trainer.Name} switched to {newPokemon.Name}!");
            Console.WriteLine($"{trainer.Name} switched to {newPokemon.Name}!");
        }

        /// <summary>
        /// Validates the conditions for switching Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer switching Pokémon.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        /// <exception cref="InvalidPokemonSwitchException">Thrown if the switch is invalid.</exception>
        private static void ValidateSwitchConditions(ITrainer trainer, IPokemon newPokemon)
        {
            if (trainer == null) throw new InvalidPokemonSwitchException("Trainer cannot be null.");
            if (newPokemon == null) throw new InvalidPokemonSwitchException("New Pokémon cannot be null.");

            if (!trainer.Pokemons.Contains(newPokemon))
                throw new InvalidPokemonSwitchException("Trainer does not own the specified Pokémon.");

            if (trainer.CurrentPokemon == newPokemon)
                throw new InvalidPokemonSwitchException("Trainer is already using the specified Pokémon.");

            if (newPokemon.IsFainted())
                throw new InvalidPokemonSwitchException("Cannot switch to a fainted Pokémon.");
        }
    }
}
