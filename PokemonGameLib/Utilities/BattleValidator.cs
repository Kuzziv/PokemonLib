using PokemonGameLib.Interfaces;
using PokemonGameLib.Exceptions;

namespace PokemonGameLib.Utilities
{
    public static class BattleValidator
    {
        public static void ValidateMove(IPokemon attacker, IMove move)
        {
            if (attacker == null) throw new ArgumentNullException(nameof(attacker));
            if (move == null) throw new ArgumentNullException(nameof(move));

            if (!attacker.Moves.Contains(move))
                throw new InvalidMoveException($"{attacker.Name} cannot use {move.Name}.");
        }

        public static void ValidatePokemonSwitch(ITrainer trainer, IPokemon newPokemon)
        {
            if (trainer == null) throw new ArgumentNullException(nameof(trainer));
            if (newPokemon == null) throw new ArgumentNullException(nameof(newPokemon));

            if (!trainer.Pokemons.Contains(newPokemon))
                throw new InvalidPokemonSwitchException($"Trainer does not own {newPokemon.Name}.");

            if (newPokemon.IsFainted())
                throw new InvalidPokemonSwitchException($"Cannot switch to a fainted Pokémon: {newPokemon.Name}.");
        }
    }
}
