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
        private readonly ITypeEffectivenessService _typeEffectivenessService;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCalculator"/> class.
        /// </summary>
        /// <param name="typeEffectivenessService">Service for calculating type effectiveness.</param>
        /// <param name="randomNumberGenerator">Service for generating random numbers.</param>
        /// <param name="logger">Logger for recording calculation events.</param>
        public BattleCalculator(ITypeEffectivenessService typeEffectivenessService, RandomNumberGenerator randomNumberGenerator, Logger logger)
        {
            _typeEffectivenessService = typeEffectivenessService ?? throw new ArgumentNullException(nameof(typeEffectivenessService));
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            double effectiveness = _typeEffectivenessService.GetEffectiveness(move.Type, defender.Type);
            double randomFactor = _randomNumberGenerator.Generate(0.85, 1.0);
            double stab = attacker.Type == move.Type ? 1.5 : 1.0;
            double critical = _randomNumberGenerator.Generate(0.0, 1.0) < 0.0625 ? 2.0 : 1.0;

            double damage = (((2 * attacker.Level / 5.0 + 2) * move.Power * (attacker.Attack / (double)defender.Defense) / 50.0) + 2)
                            * effectiveness * stab * critical * randomFactor;

            _logger.LogInfo($"{attacker.Name} used {move.Name} on {defender.Name}, dealing {damage} damage.");
            return (int)damage;
        }

        /// <summary>
        /// Calculates the chance of a status effect being applied.
        /// </summary>
        /// <param name="chance">The base chance of the status effect.</param>
        /// <returns>True if the status effect should be applied, otherwise false.</returns>
        public bool CalculateStatusEffectChance(double chance)
        {
            bool result = _randomNumberGenerator.Generate(0.0, 1.0) <= chance;
            _logger.LogInfo($"Status effect chance calculation resulted in: {result}");
            return result;
        }
    }
}
