using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;
using PokemonGameLib.Models.Items;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents an AI-controlled trainer in a Pokémon battle.
    /// </summary>
    public class AITrainer : Trainer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AITrainer"/> class.
        /// </summary>
        /// <param name="name">The name of the AI trainer.</param>
        public AITrainer(string name) : base(name)
        {
        }

        /// <summary>
        /// Executes the AI trainer's turn in the battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public override void TakeTurn(IBattle battle)
        {
            HandleFaintedPokemon(battle);

            if (ShouldUseItem(out IItem? itemToUse, battle) && itemToUse != null)
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

        /// <summary>
        /// Determines whether the AI trainer should switch Pokémon based on the battle situation.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>True if the AI should switch Pokémon, otherwise false.</returns>
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

        /// <summary>
        /// Selects the best Pokémon for the AI trainer to switch to.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>The best Pokémon to switch to, or null if no suitable Pokémon is available.</returns>
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

        /// <summary>
        /// Selects the best move for the AI trainer's Pokémon to use.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>The best move to use, or null if no suitable move is available.</returns>
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

        /// <summary>
        /// Determines whether the AI trainer should use an item during the battle.
        /// </summary>
        /// <param name="itemToUse">The item that should be used, if any.</param>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>True if the AI should use an item, otherwise false.</returns>
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

        /// <summary>
        /// Handles the situation when the AI trainer's current Pokémon has fainted.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
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
