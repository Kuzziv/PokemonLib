using PokemonGameLib.Interfaces;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Services
{
    /// <summary>
    /// Provides methods for calculating various aspects of Pokémon battles.
    /// </summary>
    public class BattleCalculator
    {
        private  readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCalculator"/> class.
        /// </summary>
        public BattleCalculator()
        {
            _randomNumberGenerator = RandomNumberGenerator.Instance; 
            _logger = LoggingService.GetLogger();
        }

        /// <summary>
        /// Calculates the damage dealt by a Pokémon move.
        /// </summary>
        /// <param name="attacker">The attacking Pokémon.</param>
        /// <param name="defender">The defending Pokémon.</param>
        /// <param name="move">The move being used by the attacker.</param>
        /// <returns>The amount of damage dealt to the defender.</returns>
        public int CalculateDamage(IPokemon attacker, IPokemon defender, IMove move)
        {
            if (attacker == null) throw new ArgumentNullException(nameof(attacker));
            if (defender == null) throw new ArgumentNullException(nameof(defender));
            if (move == null) throw new ArgumentNullException(nameof(move));

            double effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(move.Type, defender.Type);
            double randomFactor = _randomNumberGenerator.Generate(0.85, 1.0);
            double stab = attacker.Type == move.Type ? 1.5 : 1.0;
            double critical = IsCriticalHit() ? 2.0 : 1.0;

            double damage = (((2 * attacker.Level / 5.0 + 2) * move.Power * (attacker.Attack / (double)defender.Defense) / 50.0) + 2)
                            * effectiveness * stab * critical * randomFactor;

            _logger.LogInfo($"{attacker.Name} used {move.Name} on {defender.Name}, dealing {damage} damage. Effectiveness: {effectiveness}");
            return (int)damage;
        }

        /// <summary>
        /// Calculates the recoil damage that the attacker takes after using a move.
        /// </summary>
        /// <param name="attacker">The attacking Pokémon.</param>
        /// <param name="move">The move that caused recoil.</param>
        /// <returns>The amount of recoil damage.</returns>
        public int CalculateRecoilDamage(IPokemon attacker, IMove move)
        {
            if (move.RecoilPercentage > 0)
            {
                int recoilDamage = (int)(move.Power * (move.RecoilPercentage / 100.0));
                _logger.LogInfo($"{attacker.Name} took {recoilDamage} recoil damage from using {move.Name}.");
                return recoilDamage;
            }
            return 0;
        }

        /// <summary>
        /// Gets the message describing the effectiveness of the move.
        /// </summary>
        /// <param name="effectiveness">The effectiveness multiplier.</param>
        /// <returns>A string message describing the effectiveness.</returns>
        public string GetEffectivenessMessage(double effectiveness)
        {
            string message = effectiveness switch
            {
                > 1.0 => "It's super effective!",
                < 1.0 => "It's not very effective!",
                _ => "It's effective!"
            };

            _logger.LogInfo($"Effectiveness message: {message}");
            return message;
        }

        /// <summary>
        /// Determines if the move is a critical hit based on the critical hit chance.
        /// </summary>
        /// <returns>True if the move is a critical hit; otherwise, false.</returns>
        private bool IsCriticalHit()
        {
            return _randomNumberGenerator.Generate(0.0, 1.0) < 0.0625; // 6.25% chance by default
        }

        /// <summary>
        /// Calculates the amount of HP a Pokémon should heal based on the move used.
        /// </summary>
        /// <param name="attacker">The Pokémon using the healing move.</param>
        /// <param name="move">The move being used that has a healing effect.</param>
        /// <returns>The amount of HP the Pokémon should recover.</returns>
        public int CalculateHealingAmount(IPokemon attacker, IMove move)
        {
            if (attacker == null) throw new ArgumentNullException(nameof(attacker));
            if (move == null) throw new ArgumentNullException(nameof(move));

            // Calculate the healing amount as a percentage of the attacker's maximum HP
            int healingAmount = (int)(attacker.MaxHP * (move.HealingPercentage / 100.0));

            _logger.LogInfo($"{attacker.Name} is healing {healingAmount} HP using {move.Name}.");

            return healingAmount;
        }

    }
}
