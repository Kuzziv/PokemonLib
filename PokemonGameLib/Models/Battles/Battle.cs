using System;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;
using PokemonGameLib.Utilities;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Services;
using PokemonGameLib.Events;

namespace PokemonGameLib.Models.Battles
{
    public class Battle : IBattle
    {
        private readonly ILogger _logger;
        private readonly BattleCalculator _battleCalculator;
        private bool _isFirstTrainerAttacking;

        public ITrainer FirstTrainer { get; }
        public ITrainer SecondTrainer { get; }

        public ITrainer AttackingTrainer => _isFirstTrainerAttacking ? FirstTrainer : SecondTrainer;
        public ITrainer DefendingTrainer => _isFirstTrainerAttacking ? SecondTrainer : FirstTrainer;

        public event Action<PokemonEventArgs> PokemonFainted;
        public event Action<MoveEventArgs> MoveUsed;

        public Battle(ITrainer firstTrainer, ITrainer secondTrainer)
        {
            FirstTrainer = firstTrainer ?? throw new ArgumentNullException(nameof(firstTrainer));
            SecondTrainer = secondTrainer ?? throw new ArgumentNullException(nameof(secondTrainer));
            _logger = LoggingService.GetLogger();
            _battleCalculator = new BattleCalculator(); // Initialize the BattleCalculator

            FirstTrainer.ValidateTrainer(); // Validate the first trainer
            SecondTrainer.ValidateTrainer(); // Validate the second trainer

            _isFirstTrainerAttacking = true;

            System.Console.WriteLine($"{FirstTrainer.Name} vs. {SecondTrainer.Name}");

            PrintCurrentHP();
            
            System.Console.WriteLine($"{FirstTrainer.Name} has {FirstTrainer.Pokemons.Count} Pokémon.");
            System.Console.WriteLine($"{SecondTrainer.Name} has {SecondTrainer.Pokemons.Count} Pokémon.");
        }

        public void PerformAttack(IMove move)
        {
            PrintCurrentHP();

            var attacker = AttackingTrainer.CurrentPokemon;
            var defender = DefendingTrainer.CurrentPokemon;

            attacker.ApplyStatusEffects();

            if (attacker.IsFainted())
            {
                _logger.LogInfo($"{attacker.Name} is fainted and cannot attack.");
                return;
            }

            ValidateAttackConditions(attacker, defender, move);

            for (int i = 0; i < move.MaxHits; i++)
            {
                ExecuteAttack(attacker, defender, move);

                if (defender.IsFainted())
                {
                    _logger.LogInfo($"{defender.Name} has fainted!");
                    Console.WriteLine($"{defender.Name} has fainted!");
                    OnPokemonFainted(new PokemonEventArgs { FaintedPokemon = defender });
                    break;
                }

                defender.ApplyStatusEffects();

                if (defender.IsFainted())
                {
                    _logger.LogInfo($"{defender.Name} fainted from status effects.");
                    OnPokemonFainted(new PokemonEventArgs { FaintedPokemon = defender });
                    break;
                }
            }

            ApplyRecoilAndHealing(attacker, move);
            _isFirstTrainerAttacking = !_isFirstTrainerAttacking;
        }

        private void ValidateAttackConditions(IPokemon attacker, IPokemon defender, IMove move)
        {
            if (attacker == null) throw new InvalidMoveException("Attacker cannot be null.");
            if (attacker.IsFainted()) throw new InvalidMoveException($"{attacker.Name} is fainted and cannot attack.");
            if (!attacker.Moves.Contains(move)) throw new InvalidMoveException("Invalid move for the current attacker.");
            if (defender == null) throw new InvalidMoveException("Defender cannot be null.");
            if (defender.IsFainted()) throw new InvalidMoveException($"{defender.Name} is fainted and cannot be attacked.");
        }

        private void ExecuteAttack(IPokemon attacker, IPokemon defender, IMove move)
        {
            int damage = _battleCalculator.CalculateDamage(attacker, defender, move);
            defender.TakeDamage(damage);

            double effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(move.Type, defender.Type);
            string effectivenessMessage = _battleCalculator.GetEffectivenessMessage(effectiveness);

            _logger.LogInfo($"{attacker.Name} used {move.Name}! {defender.Name} took {damage} damage.");

            Console.WriteLine($"{attacker.Name} used {move.Name}!");
            Console.WriteLine(effectivenessMessage);
            Console.WriteLine($"{defender.Name} took {damage} damage!");

            OnMoveUsed(new MoveEventArgs { Move = move, Attacker = attacker, Defender = defender });
        }

        private void ApplyRecoilAndHealing(IPokemon attacker, IMove move)
        {
            if (move.RecoilPercentage > 0)
            {
                int recoilDamage = _battleCalculator.CalculateRecoilDamage(attacker, move);
                attacker.TakeDamage(recoilDamage);
                _logger.LogInfo($"{attacker.Name} took {recoilDamage} recoil damage!");
                Console.WriteLine($"{attacker.Name} took {recoilDamage} recoil damage!");
            }

            if (move.HealingPercentage > 0)
            {
                int healingAmount = _battleCalculator.CalculateHealingAmount(attacker, move);
                attacker.Heal(healingAmount);
                _logger.LogInfo($"{attacker.Name} healed {healingAmount} HP!");
                Console.WriteLine($"{attacker.Name} healed {healingAmount} HP!");
            }
        }

        public string DetermineBattleResult()
        {
            if (FirstTrainer.Pokemons.All(p => p.IsFainted()))
            {
                _logger.LogInfo($"{FirstTrainer.Name} has no remaining Pokémon. {SecondTrainer.Name} wins!");
                return $"{FirstTrainer.Name} has no remaining Pokémon. {SecondTrainer.Name} wins!";
            }
            if (SecondTrainer.Pokemons.All(p => p.IsFainted()))
            {
                _logger.LogInfo($"{SecondTrainer.Name} has no remaining Pokémon. {FirstTrainer.Name} wins!");
                return $"{SecondTrainer.Name} has no remaining Pokémon. {FirstTrainer.Name} wins!";
            }
            return "The battle is ongoing.";
        }

        public void SwitchPokemon(ITrainer trainer, IPokemon newPokemon)
        {
            ValidateSwitchConditions(trainer, newPokemon);
            trainer.CurrentPokemon = newPokemon;
            _logger.LogInfo($"{trainer.Name} switched to {newPokemon.Name}!");
            Console.WriteLine($"{trainer.Name} switched to {newPokemon.Name}!");
        }

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

        private void PrintCurrentHP()
        {
            Console.WriteLine($"{AttackingTrainer.CurrentPokemon.Name}'s HP: {AttackingTrainer.CurrentPokemon.CurrentHP}/{AttackingTrainer.CurrentPokemon.MaxHP}");
            Console.WriteLine($"{DefendingTrainer.CurrentPokemon.Name}'s HP: {DefendingTrainer.CurrentPokemon.CurrentHP}/{DefendingTrainer.CurrentPokemon.MaxHP}");
        }

        public void UseItem(ITrainer trainer, IItem item, IPokemon target)
        {
            item.Use(trainer, target);
            _logger.LogInfo($"{trainer.Name} used {item.Name} on {target.Name}.");
        }

        protected virtual void OnPokemonFainted(PokemonEventArgs e)
        {
            PokemonFainted?.Invoke(e);
        }

        protected virtual void OnMoveUsed(MoveEventArgs e)
        {
            MoveUsed?.Invoke(e);
        }
    }
}
