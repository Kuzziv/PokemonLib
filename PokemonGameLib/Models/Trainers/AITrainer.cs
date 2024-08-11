using System;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents an AI-controlled trainer in the Pokémon game.
    /// </summary>
    public class AITrainer : Trainer
    {
        private readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AITrainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the AI trainer.</param>
        public AITrainer(string name) : base(name)
        {
            _logger = LoggingService.GetLogger(); // Retrieve the logger from the LoggingService
        }

        /// <summary>
        /// Takes the AI's turn during a battle.
        /// The AI will choose the best action to maximize its chances of winning.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public override void TakeTurn(IBattle battle)
        {
            if (ShouldUseItem(out IItem itemToUse))
            {
                UseItem(itemToUse, CurrentPokemon);
                return;
            }

            if (ShouldSwitchPokemon(battle))
            {
                var newPokemon = SelectBestPokemonToSwitchTo();
                if (newPokemon != null)
                {
                    battle.SwitchPokemon(this, newPokemon);
                    return;
                }
            }

            var move = SelectBestMove();
            if (move != null)
            {
                battle.PerformAttack(move);
            }
        }

        /// <summary>
        /// Determines whether the AI should switch Pokémon based on the battle situation.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns><c>true</c> if the AI should switch Pokémon; otherwise, <c>false</c>.</returns>
        private bool ShouldSwitchPokemon(IBattle battle)
        {
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = battle.DefendingTrainer.CurrentPokemon;

            if (currentPokemon == null || opponentPokemon == null) return false;

            double effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(opponentPokemon.Type, currentPokemon.Type);

            var shouldSwitch = effectiveness > 1.5 || currentPokemon.CurrentHP < currentPokemon.MaxHP / 4;
            if (shouldSwitch)
            {
                _logger.LogInfo($"{Name} decides to switch Pokémon.");
            }

            return shouldSwitch;
        }

        /// <summary>
        /// Selects the best Pokémon to switch to based on type advantage and health.
        /// </summary>
        /// <returns>The best Pokémon to switch to, or <c>null</c> if no switch is needed.</returns>
        private IPokemon SelectBestPokemonToSwitchTo()
        {
            var opponentPokemon = CurrentPokemon;
            var bestPokemon = Pokemons
                .Where(p => !p.IsFainted() && TypeEffectivenessService.Instance.GetEffectiveness(p.Type, opponentPokemon.Type) < 1.0)
                .OrderByDescending(p => p.Level)
                .FirstOrDefault();

            if (bestPokemon != null)
            {
                _logger.LogInfo($"{Name} chooses to switch to {bestPokemon.Name}.");
            }
            else
            {
                _logger.LogInfo($"{Name} decides not to switch Pokémon.");
            }

            return bestPokemon;
        }

        /// <summary>
        /// Selects the best move to use based on effectiveness and power.
        /// </summary>
        /// <returns>The best move to use, or <c>null</c> if no move is suitable.</returns>
        private IMove SelectBestMove()
        {
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = CurrentPokemon;

            var bestMove = currentPokemon?.Moves
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
        /// Determines whether the AI should use an item based on the current Pokémon's health.
        /// </summary>
        /// <param name="itemToUse">The item to use, if any.</param>
        /// <returns><c>true</c> if an item should be used; otherwise, <c>false</c>.</returns>
        private bool ShouldUseItem(out IItem itemToUse)
        {
            var currentPokemon = CurrentPokemon;
            itemToUse = null;

            if (currentPokemon != null && currentPokemon.CurrentHP < currentPokemon.MaxHP / 2)
            {
                itemToUse = Items.OfType<Potion>().FirstOrDefault();
                if (itemToUse != null)
                {
                    _logger.LogInfo($"{Name} decides to use {itemToUse.Name}.");
                    return true;
                }
            }

            return false;
        }
    }
}
