using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;
using PokemonGameLib.Models.Items;

namespace PokemonGameLib.Models.Trainers
{
    public class AITrainer : Trainer
    {
        private readonly ILogger _logger;

        public AITrainer(string name) : base(name)
        {
            _logger = LoggingService.GetLogger();
        }

        public override void TakeTurn(IBattle battle)
        {
            HandleFaintedPokemon(battle);

            if (ShouldUseItem(out IItem? itemToUse, battle))
            {
                battle.PerformUseItem(itemToUse, CurrentPokemon);
                Items.Remove(itemToUse);
                return;
            }

            if (ShouldSwitchPokemon(battle))
            {
                var newPokemon = SelectBestPokemonToSwitchTo(battle);
                if (newPokemon != null)
                {
                    battle.PerformSwitch(this, newPokemon);
                    return;
                }
            }

            var move = SelectBestMove(battle);
            if (move != null)
            {
                battle.PerformAttack(move);
            }
        }

        internal bool ShouldSwitchPokemon(IBattle battle)
        {
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = battle.OpponentTrainer.CurrentPokemon;

            if (currentPokemon == null || opponentPokemon == null) return false;

            double effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(opponentPokemon.Type, currentPokemon.Type);

            bool shouldSwitch = effectiveness > 1.5 || currentPokemon.CurrentHP < currentPokemon.MaxHP / 4;

            if (!shouldSwitch && currentPokemon.Moves.All(m => TypeEffectivenessService.Instance.GetEffectiveness(m.Type, opponentPokemon.Type) < 1.0))
            {
                shouldSwitch = true;
                _logger.LogInfo($"{Name} considers switching due to ineffective moves.");
            }

            _logger.LogInfo($"Effectiveness of {opponentPokemon.Name} against {currentPokemon.Name} is {effectiveness}. Should switch: {shouldSwitch}");

            return shouldSwitch;
        }

        internal IPokemon? SelectBestPokemonToSwitchTo(IBattle battle)
        {
            var opponentPokemon = battle.OpponentTrainer.CurrentPokemon;

            _logger.LogInfo($"Evaluating switch for {Name}. Opponent Pokémon: {opponentPokemon.Name} ({opponentPokemon.Type})");

            var bestPokemon = Pokemons
                .Where(p => !p.IsFainted() && TypeEffectivenessService.Instance.GetEffectiveness(p.Type, opponentPokemon.Type) > 1.0)
                .OrderByDescending(p => p.Level)
                .FirstOrDefault();

            if (bestPokemon != null && bestPokemon != CurrentPokemon)
            {
                _logger.LogInfo($"{Name} chooses to switch to {bestPokemon.Name}.");
                return bestPokemon;
            }
            else
            {
                _logger.LogInfo($"{Name} decides not to switch Pokémon.");
                return null;
            }
        }

        internal IMove? SelectBestMove(IBattle battle)
        {
            var currentPokemon = CurrentPokemon;  // Make sure this is the current Pokémon
            var opponentPokemon = battle.OpponentTrainer.CurrentPokemon;

            if (currentPokemon == null || opponentPokemon == null) return null;

            var bestMove = currentPokemon.Moves
                .OrderByDescending(m => TypeEffectivenessService.Instance.GetEffectiveness(m.Type, opponentPokemon.Type) * m.Power)
                .FirstOrDefault();

            if (bestMove != null)
            {
                _logger.LogInfo($"{Name} chooses to use {bestMove.Name}.");
            }
            else
            {
                _logger.LogInfo($"{Name} finds no suitable move to use.");
            }

            return bestMove;
        }


        internal bool ShouldUseItem(out IItem? itemToUse, IBattle battle)
        {
            var currentPokemon = CurrentPokemon;
            itemToUse = null;

            if (currentPokemon != null && currentPokemon.CurrentHP < currentPokemon.MaxHP / 2)
            {
                var opponentMove = battle.OpponentTrainer.CurrentPokemon.Moves
                    .OrderByDescending(m => TypeEffectivenessService.Instance.GetEffectiveness(m.Type, currentPokemon.Type) * m.Power)
                    .FirstOrDefault();

                if (opponentMove != null && currentPokemon.CurrentHP <= opponentMove.Power)
                {
                    itemToUse = Items.OfType<Potion>().FirstOrDefault();
                    if (itemToUse != null)
                    {
                        _logger.LogInfo($"{Name} decides to use {itemToUse.Name}.");
                        return true;
                    }
                }
            }

            return false;
        }

        public override void HandleFaintedPokemon(IBattle battle)
        {
            if (CurrentPokemon.IsFainted())
            {
                _logger.LogInfo($"{Name}'s {CurrentPokemon.Name} has fainted.");

                var newPokemon = Pokemons.FirstOrDefault(p => !p.IsFainted());

                if (newPokemon != null)
                {
                    CurrentPokemon = newPokemon;
                    battle.PerformSwitch(this, newPokemon);
                }
                else
                {
                    _logger.LogError($"{Name} has no Pokémon left to switch to!");
                    Console.WriteLine($"{Name} has no Pokémon left!");
                }
            }
        }
    }
}
