using System;
using System.Linq;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Moves;
using PokemonGameLib.Models.Items;
using PokemonGameLib.Models.Battles;
using PokemonGameLib.Services;

namespace PokemonGameLib.Models.AI
{
    public class AITrainer : Trainer
    {
        public AITrainer(string name) : base(name) { }

        public void TakeTurn(Battle battle)
        {
            // Determine if the AI should switch Pokémon
            if (ShouldSwitchPokemon(battle))
            {
                var newPokemon = SelectBestPokemonToSwitchTo();
                if (newPokemon != null)
                {
                    battle.SwitchPokemon(this, newPokemon);
                    return;
                }
            }

            // Otherwise, choose the best move
            var move = SelectBestMove();
            if (move != null)
            {
                battle.PerformAttack(move);
            }
        }

        private bool ShouldSwitchPokemon(Battle battle)
        {
            // Simple logic: switch if current Pokémon has a type disadvantage or low HP
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = battle.DefendingTrainer.CurrentPokemon;

            if (currentPokemon == null || opponentPokemon == null) return false;

            double effectiveness = TypeEffectiveness.GetEffectiveness(opponentPokemon.Type, currentPokemon.Type);

            return effectiveness > 1.5 || currentPokemon.CurrentHP < currentPokemon.MaxHP / 4;
        }

        private Pokemon SelectBestPokemonToSwitchTo()
        {
            // Choose a Pokémon that has the best type advantage against the opponent
            var opponentPokemon = CurrentPokemon; // Assuming the opponent's current Pokémon is known
            return Pokemons
                .Where(p => !p.IsFainted() && TypeEffectiveness.GetEffectiveness(p.Type, opponentPokemon.Type) < 1.0)
                .OrderByDescending(p => p.Level)
                .FirstOrDefault();
        }

        private Move SelectBestMove()
        {
            // Choose the move with the highest power that is also super effective
            var currentPokemon = CurrentPokemon;
            var opponentPokemon = CurrentPokemon; // Assuming the opponent's current Pokémon is known

            return currentPokemon?.Moves
                .OrderByDescending(m => TypeEffectiveness.GetEffectiveness(m.Type, opponentPokemon.Type) * m.Power)
                .FirstOrDefault();
        }
    }
}
