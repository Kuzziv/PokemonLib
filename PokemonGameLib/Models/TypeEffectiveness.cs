using System;
using System.Collections.Generic;

/// <summary>
/// Provides methods to determine type effectiveness in Pokémon battles.
/// </summary>
public static class TypeEffectiveness
{
    /// <summary>
    /// A dictionary that maps a tuple of attacking and defending Pokémon types to their effectiveness multiplier.
    /// </summary>
    private static readonly Dictionary<(PokemonType, PokemonType), double> effectiveness = new Dictionary<(PokemonType, PokemonType), double>
    {
        // Fire type effectiveness
        {(PokemonType.Fire, PokemonType.Grass), 2.0},
        {(PokemonType.Fire, PokemonType.Ice), 2.0},
        {(PokemonType.Fire, PokemonType.Bug), 2.0},
        {(PokemonType.Fire, PokemonType.Steel), 2.0},
        {(PokemonType.Fire, PokemonType.Fire), 0.5},
        {(PokemonType.Fire, PokemonType.Water), 0.5},
        {(PokemonType.Fire, PokemonType.Rock), 0.5},
        {(PokemonType.Fire, PokemonType.Dragon), 0.5},
        // Water type effectiveness
        {(PokemonType.Water, PokemonType.Fire), 2.0},
        {(PokemonType.Water, PokemonType.Ground), 2.0},
        {(PokemonType.Water, PokemonType.Rock), 2.0},
        {(PokemonType.Water, PokemonType.Water), 0.5},
        {(PokemonType.Water, PokemonType.Grass), 0.5},
        {(PokemonType.Water, PokemonType.Dragon), 0.5},
        // Grass type effectiveness
        {(PokemonType.Grass, PokemonType.Water), 2.0},
        {(PokemonType.Grass, PokemonType.Ground), 2.0},
        {(PokemonType.Grass, PokemonType.Rock), 2.0},
        {(PokemonType.Grass, PokemonType.Fire), 0.5},
        {(PokemonType.Grass, PokemonType.Grass), 0.5},
        {(PokemonType.Grass, PokemonType.Poison), 0.5},
        {(PokemonType.Grass, PokemonType.Flying), 0.5},
        {(PokemonType.Grass, PokemonType.Bug), 0.5},
        {(PokemonType.Grass, PokemonType.Dragon), 0.5},
        {(PokemonType.Grass, PokemonType.Steel), 0.5},
        // Electric type effectiveness
        {(PokemonType.Electric, PokemonType.Water), 2.0},
        {(PokemonType.Electric, PokemonType.Flying), 2.0},
        {(PokemonType.Electric, PokemonType.Electric), 0.5},
        {(PokemonType.Electric, PokemonType.Grass), 0.5},
        {(PokemonType.Electric, PokemonType.Dragon), 0.5},
        {(PokemonType.Electric, PokemonType.Ground), 0.0},
        // Psychic type effectiveness
        {(PokemonType.Psychic, PokemonType.Fighting), 2.0},
        {(PokemonType.Psychic, PokemonType.Poison), 2.0},
        {(PokemonType.Psychic, PokemonType.Psychic), 0.5},
        {(PokemonType.Psychic, PokemonType.Steel), 0.5},
        {(PokemonType.Psychic, PokemonType.Dark), 0.0},
        // Ice type effectiveness
        {(PokemonType.Ice, PokemonType.Grass), 2.0},
        {(PokemonType.Ice, PokemonType.Ground), 2.0},
        {(PokemonType.Ice, PokemonType.Flying), 2.0},
        {(PokemonType.Ice, PokemonType.Dragon), 2.0},
        {(PokemonType.Ice, PokemonType.Fire), 0.5},
        {(PokemonType.Ice, PokemonType.Water), 0.5},
        {(PokemonType.Ice, PokemonType.Ice), 0.5},
        {(PokemonType.Ice, PokemonType.Steel), 0.5},
        // Dragon type effectiveness
        {(PokemonType.Dragon, PokemonType.Dragon), 2.0},
        {(PokemonType.Dragon, PokemonType.Steel), 0.5},
        {(PokemonType.Dragon, PokemonType.Fairy), 0.0},
        // Dark type effectiveness
        {(PokemonType.Dark, PokemonType.Psychic), 2.0},
        {(PokemonType.Dark, PokemonType.Ghost), 2.0},
        {(PokemonType.Dark, PokemonType.Fighting), 0.5},
        {(PokemonType.Dark, PokemonType.Dark), 0.5},
        {(PokemonType.Dark, PokemonType.Fairy), 0.5},
        // Fairy type effectiveness
        {(PokemonType.Fairy, PokemonType.Fighting), 2.0},
        {(PokemonType.Fairy, PokemonType.Dragon), 2.0},
        {(PokemonType.Fairy, PokemonType.Dark), 2.0},
        {(PokemonType.Fairy, PokemonType.Fire), 0.5},
        {(PokemonType.Fairy, PokemonType.Poison), 0.5},
        {(PokemonType.Fairy, PokemonType.Steel), 0.5},
        // Normal type effectiveness
        {(PokemonType.Normal, PokemonType.Rock), 0.5},
        {(PokemonType.Normal, PokemonType.Ghost), 0.0},
        {(PokemonType.Normal, PokemonType.Steel), 0.5},
        // Fighting type effectiveness
        {(PokemonType.Fighting, PokemonType.Normal), 2.0},
        {(PokemonType.Fighting, PokemonType.Ice), 2.0},
        {(PokemonType.Fighting, PokemonType.Rock), 2.0},
        {(PokemonType.Fighting, PokemonType.Dark), 2.0},
        {(PokemonType.Fighting, PokemonType.Steel), 2.0},
        {(PokemonType.Fighting, PokemonType.Poison), 0.5},
        {(PokemonType.Fighting, PokemonType.Flying), 0.5},
        {(PokemonType.Fighting, PokemonType.Psychic), 0.5},
        {(PokemonType.Fighting, PokemonType.Bug), 0.5},
        {(PokemonType.Fighting, PokemonType.Fairy), 0.5},
        {(PokemonType.Fighting, PokemonType.Ghost), 0.0},
        // Flying type effectiveness
        {(PokemonType.Flying, PokemonType.Grass), 2.0},
        {(PokemonType.Flying, PokemonType.Fighting), 2.0},
        {(PokemonType.Flying, PokemonType.Bug), 2.0},
        {(PokemonType.Flying, PokemonType.Electric), 0.5},
        {(PokemonType.Flying, PokemonType.Rock), 0.5},
        {(PokemonType.Flying, PokemonType.Steel), 0.5},
        // Poison type effectiveness
        {(PokemonType.Poison, PokemonType.Grass), 2.0},
        {(PokemonType.Poison, PokemonType.Fairy), 2.0},
        {(PokemonType.Poison, PokemonType.Poison), 0.5},
        {(PokemonType.Poison, PokemonType.Ground), 0.5},
        {(PokemonType.Poison, PokemonType.Rock), 0.5},
        {(PokemonType.Poison, PokemonType.Ghost), 0.5},
        {(PokemonType.Poison, PokemonType.Steel), 0.0},
        // Ground type effectiveness
        {(PokemonType.Ground, PokemonType.Fire), 2.0},
        {(PokemonType.Ground, PokemonType.Electric), 2.0},
        {(PokemonType.Ground, PokemonType.Poison), 2.0},
        {(PokemonType.Ground, PokemonType.Rock), 2.0},
        {(PokemonType.Ground, PokemonType.Steel), 2.0},
        {(PokemonType.Ground, PokemonType.Grass), 0.5},
        {(PokemonType.Ground, PokemonType.Bug), 0.5},
        {(PokemonType.Ground, PokemonType.Flying), 0.0},
        // Rock type effectiveness
        {(PokemonType.Rock, PokemonType.Fire), 2.0},
        {(PokemonType.Rock, PokemonType.Ice), 2.0},
        {(PokemonType.Rock, PokemonType.Flying), 2.0},
        {(PokemonType.Rock, PokemonType.Bug), 2.0},
        {(PokemonType.Rock, PokemonType.Fighting), 0.5},
        {(PokemonType.Rock, PokemonType.Ground), 0.5},
        {(PokemonType.Rock, PokemonType.Steel), 0.5},
        // Bug type effectiveness
        {(PokemonType.Bug, PokemonType.Grass), 2.0},
        {(PokemonType.Bug, PokemonType.Psychic), 2.0},
        {(PokemonType.Bug, PokemonType.Dark), 2.0},
        {(PokemonType.Bug, PokemonType.Fire), 0.5},
        {(PokemonType.Bug, PokemonType.Fighting), 0.5},
        {(PokemonType.Bug, PokemonType.Poison), 0.5},
        {(PokemonType.Bug, PokemonType.Flying), 0.5},
        {(PokemonType.Bug, PokemonType.Ghost), 0.5},
        {(PokemonType.Bug, PokemonType.Steel), 0.5},
        {(PokemonType.Bug, PokemonType.Fairy), 0.5},
        // Ghost type effectiveness
        {(PokemonType.Ghost, PokemonType.Psychic), 2.0},
        {(PokemonType.Ghost, PokemonType.Ghost), 2.0},
        {(PokemonType.Ghost, PokemonType.Dark), 0.5},
        {(PokemonType.Ghost, PokemonType.Normal), 0.0},
        // Steel type effectiveness
        {(PokemonType.Steel, PokemonType.Ice), 2.0},
        {(PokemonType.Steel, PokemonType.Rock), 2.0},
        {(PokemonType.Steel, PokemonType.Fairy), 2.0},
        {(PokemonType.Steel, PokemonType.Fire), 0.5},
        {(PokemonType.Steel, PokemonType.Water), 0.5},
        {(PokemonType.Steel, PokemonType.Electric), 0.5},
        {(PokemonType.Steel, PokemonType.Steel), 0.5},
        {(PokemonType.Steel, PokemonType.Poison), 0.5},

    };

    /// <summary>
    /// Gets the effectiveness of an attacking Pokémon's move type against a defending Pokémon's type.
    /// </summary>
    /// <param name="attackType">The type of the attacking move.</param>
    /// <param name="defenseType">The type of the defending Pokémon.</param>
    /// <returns>A multiplier representing the effectiveness of the attack.</returns>
    public static double GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (effectiveness.TryGetValue((attackType, defenseType), out var value))
        {
            return value;
        }
        return 1.0; // Neutral effectiveness
    }
}
