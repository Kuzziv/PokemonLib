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
        private readonly ILogger _logger;

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
            // Check if the current Pokémon has fainted and handle it
            HandleFaintedPokemon(battle);

            if (ShouldUseItem(out IItem? itemToUse, battle))
            {
                var useItemCommand = CreateUseItemCommand(battle, itemToUse, CurrentPokemon);
                useItemCommand.Execute();
                return;
            }

            if (ShouldSwitchPokemon(battle))
            {
                var newPokemon = SelectBestPokemonToSwitchTo(battle);
                if (newPokemon != null)
                {
                    var switchCommand = CreateSwitchCommand(battle, newPokemon);
                    switchCommand.Execute();
                    return;
                }
            }

            var move = SelectBestMove(battle);
            if (move != null)
            {
                var attackCommand = CreateAttackCommand(battle, move);
                attackCommand.Execute();
            }
        }

        private void HandleFaintedPokemon(IBattle battle)
        {
            if (CurrentPokemon.IsFainted())
            {
                _logger.LogInfo($"{Name}'s {CurrentPokemon.Name} has fainted.");

                var newPokemon = Pokemons.FirstOrDefault(p => !p.IsFainted());

                if (newPokemon != null)
                {
                    var switchCommand = CreateSwitchCommand(battle, newPokemon);
                    switchCommand.Execute();
                    CurrentPokemon = newPokemon;
                }
                else
                {
                    _logger.LogError($"{Name} has no Pokémon left to switch to!");
                    Console.WriteLine($"{Name} has no Pokémon left!");
                }
            }
        }

        internal bool ShouldSwitchPokemon(IBattle battle)
        {
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = battle.DefendingTrainer.CurrentPokemon;

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
            var opponentPokemon = battle.DefendingTrainer.CurrentPokemon;

            _logger.LogInfo($"Evaluating switch for {Name}. Opponent Pokémon: {opponentPokemon.Name} ({opponentPokemon.Type})");

            foreach (var pokemon in Pokemons)
            {
                if (!pokemon.IsFainted())
                {
                    double effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(pokemon.Type, opponentPokemon.Type);
                    _logger.LogInfo($"Effectiveness of {pokemon.Name} ({pokemon.Type}) against {opponentPokemon.Name} ({opponentPokemon.Type}): {effectiveness}");

                    if (effectiveness > 1.0)
                    {
                        _logger.LogInfo($"{pokemon.Name} is considered for switching due to type advantage.");
                    }
                }
            }

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
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = battle.DefendingTrainer.CurrentPokemon;

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

        internal bool ShouldUseItem(out IItem? itemToUse, IBattle battle)
        {
            var currentPokemon = CurrentPokemon;
            itemToUse = null;

            if (currentPokemon != null && currentPokemon.CurrentHP < currentPokemon.MaxHP / 2)
            {
                var opponentMove = battle.DefendingTrainer.CurrentPokemon.Moves
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
    }
}
