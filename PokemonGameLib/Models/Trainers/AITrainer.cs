using System;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Services;
using PokemonGameLib.Models.Items;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents an AI-controlled trainer in the Pokémon game.
    /// </summary>
    public class AITrainer : Trainer
    {
        public AITrainer(string name) : base(name) { }

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

        private bool ShouldSwitchPokemon(IBattle battle)
        {
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = battle.DefendingTrainer.CurrentPokemon;

            if (currentPokemon == null || opponentPokemon == null) return false;

            double effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(opponentPokemon.Type, currentPokemon.Type);

            return effectiveness > 1.5 || currentPokemon.CurrentHP < currentPokemon.MaxHP / 4;
        }

        private IPokemon SelectBestPokemonToSwitchTo()
        {
            var opponentPokemon = CurrentPokemon;
            return Pokemons
                .Where(p => !p.IsFainted() && TypeEffectivenessService.Instance.GetEffectiveness(p.Type, opponentPokemon.Type) < 1.0)
                .OrderByDescending(p => p.Level)
                .FirstOrDefault();
        }

        private IMove SelectBestMove()
        {
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = CurrentPokemon;

            return currentPokemon?.Moves
                .OrderByDescending(m => TypeEffectivenessService.Instance.GetEffectiveness(m.Type, opponentPokemon.Type) * m.Power)
                .FirstOrDefault();
        }

        private bool ShouldUseItem(out IItem itemToUse)
        {
            var currentPokemon = CurrentPokemon;
            itemToUse = null;

            if (currentPokemon != null && currentPokemon.CurrentHP < currentPokemon.MaxHP / 2)
            {
                itemToUse = Items.OfType<Potion>().FirstOrDefault();
                if (itemToUse != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
