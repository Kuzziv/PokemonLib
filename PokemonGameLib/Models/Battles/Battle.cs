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

            ValidateTrainers();

            _isFirstTrainerAttacking = true;

            LogBattleStart();
            PrintCurrentHP();
        }

        private void ValidateTrainers()
        {
            FirstTrainer.ValidateTrainer();
            SecondTrainer.ValidateTrainer();
        }

        private void LogBattleStart()
        {
            _logger.LogInfo($"{FirstTrainer.Name} vs. {SecondTrainer.Name}");
            Console.WriteLine($"{FirstTrainer.Name} has {FirstTrainer.Pokemons.Count} Pokémon.");
            Console.WriteLine($"{SecondTrainer.Name} has {SecondTrainer.Pokemons.Count} Pokémon.");
        }

        public void PerformAttack(IMove move)
        {
            PrintCurrentHP();

            var attacker = AttackingTrainer.CurrentPokemon;
            var defender = DefendingTrainer.CurrentPokemon;

            if (attacker == null || defender == null) throw new InvalidOperationException("Current Pokémon cannot be null.");

            attacker.ApplyStatusEffects();

            if (attacker.IsFainted())
            {
                _logger.LogInfo($"{attacker.Name} is fainted and cannot attack.");
                return;
            }

            ValidateAttackConditions(attacker, defender, move);

            ExecuteMultipleHits(attacker, defender, move);

            ApplyRecoilAndHealing(attacker, move);

            ToggleAttackingTrainer();
        }

        private void ValidateAttackConditions(IPokemon attacker, IPokemon defender, IMove move)
        {
            if (!attacker.Moves.Contains(move))
                throw new InvalidMoveException("Invalid move for the current attacker.");

            if (defender.IsFainted())
                throw new InvalidMoveException($"{defender.Name} is fainted and cannot be attacked.");
        }

        private void ExecuteMultipleHits(IPokemon attacker, IPokemon defender, IMove move)
        {
            for (int i = 0; i < move.MaxHits; i++)
            {
                ExecuteAttack(attacker, defender, move);

                if (defender.IsFainted())
                {
                    HandleDefenderFainting(defender);
                    break;
                }

                defender.ApplyStatusEffects();

                if (defender.IsFainted())
                {
                    HandleDefenderFainting(defender);
                    break;
                }
            }
        }

        private void ExecuteAttack(IPokemon attacker, IPokemon defender, IMove move)
        {
            int damage = _battleCalculator.CalculateDamage(attacker, defender, move);
            defender.TakeDamage(damage);

            string effectivenessMessage = _battleCalculator.GetEffectivenessMessage(
                TypeEffectivenessService.Instance.GetEffectiveness(move.Type, defender.Type));

            _logger.LogInfo($"{attacker.Name} used {move.Name}! {defender.Name} took {damage} damage.");
            Console.WriteLine($"{attacker.Name} used {move.Name}!");
            Console.WriteLine(effectivenessMessage);
            Console.WriteLine($"{defender.Name} took {damage} damage!");

            OnMoveUsed(new MoveEventArgs { Move = move, Attacker = attacker, Defender = defender });
        }

        private void HandleDefenderFainting(IPokemon defender)
        {
            _logger.LogInfo($"{defender.Name} has fainted!");
            Console.WriteLine($"{defender.Name} has fainted!");
            OnPokemonFainted(new PokemonEventArgs { FaintedPokemon = defender });
        }

        private void ApplyRecoilAndHealing(IPokemon attacker, IMove move)
        {
            if (move.RecoilPercentage > 0)
            {
                int recoilDamage = _battleCalculator.CalculateRecoilDamage(attacker, move);
                attacker.TakeDamage(recoilDamage);
                LogRecoilDamage(attacker, recoilDamage);
            }

            if (move.HealingPercentage > 0)
            {
                int healingAmount = _battleCalculator.CalculateHealingAmount(attacker, move);
                attacker.Heal(healingAmount);
                LogHealing(attacker, healingAmount);
            }
        }

        private void LogRecoilDamage(IPokemon attacker, int recoilDamage)
        {
            _logger.LogInfo($"{attacker.Name} took {recoilDamage} recoil damage!");
            Console.WriteLine($"{attacker.Name} took {recoilDamage} recoil damage!");
        }

        private void LogHealing(IPokemon attacker, int healingAmount)
        {
            _logger.LogInfo($"{attacker.Name} healed {healingAmount} HP!");
            Console.WriteLine($"{attacker.Name} healed {healingAmount} HP!");
        }

        private void ToggleAttackingTrainer()
        {
            _isFirstTrainerAttacking = !_isFirstTrainerAttacking;
        }

        public string DetermineBattleResult()
        {
            if (FirstTrainer.Pokemons.All(p => p.IsFainted()))
            {
                return LogBattleResult($"{FirstTrainer.Name} has no remaining Pokémon. {SecondTrainer.Name} wins!");
            }
            if (SecondTrainer.Pokemons.All(p => p.IsFainted()))
            {
                return LogBattleResult($"{SecondTrainer.Name} has no remaining Pokémon. {FirstTrainer.Name} wins!");
            }
            return "The battle is ongoing.";
        }

        private string LogBattleResult(string result)
        {
            _logger.LogInfo(result);
            return result;
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
