# Pokémon Game Library

Welcome to the **Pokémon Game Library**—a comprehensive C# library for building a Pokémon-style game. This library provides robust interfaces and classes to manage Pokémon, implement battle mechanics, and create trainers, items, and abilities. With built-in utilities for logging and AI, this library is designed to streamline your game development process.

## Features

- **Pokémon Management**: Easily create and manage Pokémon with various types, abilities, moves, and evolutions.
- **Battle System**: Implement a full-featured Pokémon battle system, including turn-based mechanics, status effects, and type effectiveness.
- **Items and Abilities**: Define and use items and abilities that influence battles, enhancing gameplay strategies.
- **Trainer AI**: Includes an AI system for trainers, allowing for both human and AI-controlled opponents with smart decision-making.
- **Logging**: Integrated logging service for tracking game events, errors, and debugging.

## Installation

Install the library via NuGet:

```bash
dotnet add package PokemonGameLib
```

Or using the NuGet Package Manager Console:

```bash
Install-Package PokemonGameLib
```

## Getting Started

### Setting Up a Pokémon

Begin by creating a Pokémon with specified attributes:

```csharp
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Models.Pokemons.Abilities;

var pikachu = new Pokemon(
    name: "Pikachu",
    type: PokemonType.Electric,
    level: 5,
    maxHp: 35,
    attack: 55,
    defense: 40,
    abilities: new List<IAbility> { new Ability("Static", "May cause paralysis if touched.") }
);
```

### Adding Moves

Add moves to your Pokémon to enhance its battle capabilities:

```csharp
var thunderShock = new Move("ThunderShock", PokemonType.Electric, power: 40, level: 1);
pikachu.AddMove(thunderShock);
```

### Creating a Trainer

Create a trainer and assign Pokémon to them:

```csharp
using PokemonGameLib.Models.Trainers;

var trainerAsh = new PlayerTrainer("Ash");
trainerAsh.AddPokemon(pikachu);
trainerAsh.CurrentPokemon = pikachu;
```

### Starting a Battle

Initiate a battle between two trainers:

```csharp
var trainerMisty = new AITrainer("Misty");

var battle = new Battle(trainerAsh, trainerMisty);
battle.PerformAttack(thunderShock);
```

### Logging

Configure logging to monitor and debug game events:

```csharp
using PokemonGameLib.Utilities;

LoggingService.Configure(); // Optional: Specify a custom log file path.
```

## Exception Handling

The library includes custom exceptions to handle various invalid operations, ensuring robust error management:

- **InvalidMoveException**: Thrown when an invalid move is attempted.
- **InvalidPokemonSwitchException**: Thrown when an invalid Pokémon switch is attempted.
- **ItemNotFoundException**: Thrown when an item is not found in the trainer's inventory.
- **PokemonFaintedException**: Thrown when attempting to use a fainted Pokémon in battle.

## Advanced Features

### Trainer AI

The library includes a built-in AI system that allows trainers to make smart decisions during battles. The AI considers factors like type advantages, move effectiveness, and Pokémon health when choosing actions.

```csharp
var aiTrainer = new AITrainer("AI Opponent");
aiTrainer.TakeTurn(battle);
```

### Status Effects and Abilities

Incorporate abilities and status effects into your game to add depth and strategy:

```csharp
pikachu.InflictStatus(StatusCondition.Paralysis);
pikachu.ApplyStatusEffects();
```

### Type Effectiveness

The library provides a comprehensive type effectiveness system to ensure battles are true to the Pokémon style:

```csharp
var effectiveness = TypeEffectivenessService.Instance.GetEffectiveness(PokemonType.Fire, PokemonType.Grass);
```

## Contributing

We welcome contributions! If you’d like to contribute, please fork the repository and submit a pull request on our [GitHub repository](https://github.com/kuzziv/pokemonLib).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Support

If you encounter any issues or have feature requests, please open an issue on the [GitHub repository](https://github.com/kuzziv/pokemonLib).

## Acknowledgments

This library is inspired by the Pokémon series, developed by Game Freak and published by Nintendo. This project is an independent creation and is not affiliated with or endorsed by Nintendo, Game Freak, or The Pokémon Company.

---

Start building your Pokémon adventure today with the **Pokémon Game Library**! 

[GitHub Repository](https://github.com/kuzziv/pokemonLib)