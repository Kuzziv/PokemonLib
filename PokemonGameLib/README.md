# PokemonGameLib

## Overview

`PokemonGameLib` is a robust C# library designed to simulate Pokémon battles. It provides a comprehensive set of interfaces, classes, and services that model the core elements of a Pokémon game, including trainers, Pokémon, moves, items, and battle mechanics. This library is highly extensible, allowing developers to create complex battle systems, custom trainers, and unique game mechanics.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Getting Started](#getting-started)
  - [Creating Pokémon and Moves](#creating-pokémon-and-moves)
  - [Setting Up a Battle](#setting-up-a-battle)
  - [Executing Commands](#executing-commands)
  - [Battle Flow](#battle-flow)
- [Logging and Debugging](#logging-and-debugging)
- [Exception Handling](#exception-handling)
- [Extending the Library](#extending-the-library)
  - [Custom Pokémon](#custom-pokémon)
  - [Custom Moves](#custom-moves)
  - [Custom Items](#custom-items)
  - [Custom Battle Mechanics](#custom-battle-mechanics)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Battle Simulation**: Fully functional battle system with support for player and AI-controlled trainers.
- **Command Pattern**: Encapsulate actions like attacking, switching Pokémon, and using items as commands.
- **Comprehensive Pokémon Models**: Define Pokémon with detailed attributes, including type, stats, moves, and evolutions.
- **Item Usage**: Implement items that can heal, revive, or otherwise affect Pokémon during battles.
- **Type Effectiveness**: Built-in type effectiveness calculator for accurate battle damage simulation.
- **Status Conditions**: Handle status effects like paralysis, burn, sleep, and more, with detailed behavior.
- **Evolution System**: Manage Pokémon evolution based on conditions like level or items.
- **Advanced Logging**: Integrated logging system for detailed tracking of battle events.
- **Custom Exceptions**: Rich set of custom exceptions for precise error handling.

## Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Kuzziv/PokemonLib
   ```

2. **Build the Project**
   - Open the solution in Visual Studio or your preferred C# IDE.
   - Build the solution to generate the `PokemonGameLib.dll` file.

3. **Include in Your Project or use the NuGet package**
   - Add a reference to `PokemonGameLib.dll` in your project.
   - https://www.nuget.org/packages/PokemonGameLib


## Getting Started

### Creating Pokémon and Moves

Start by creating Pokémon and their associated moves:

```csharp
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;

var pikachu = new Pokemon(
    name: "Pikachu", 
    type: PokemonType.Electric, 
    level: 10, 
    maxHp: 35, 
    attack: 55, 
    defense: 40
);
pikachu.AddMove(new Move("Thunder Shock", PokemonType.Electric, power: 40, level: 1));

var charmander = new Pokemon(
    name: "Charmander", 
    type: PokemonType.Fire, 
    level: 10, 
    maxHp: 39, 
    attack: 52, 
    defense: 43
);
charmander.AddMove(new Move("Ember", PokemonType.Fire, power: 40, level: 1));
```

### Setting Up a Battle

With your Pokémon ready, set up a battle between two trainers:

```csharp
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Battles;

var playerTrainer = new PlayerTrainer("Ash");
playerTrainer.AddPokemon(pikachu);
playerTrainer.CurrentPokemon = pikachu;

var aiTrainer = new AITrainer("Brock");
aiTrainer.AddPokemon(charmander);
aiTrainer.CurrentPokemon = charmander;

var battle = new Battle(playerTrainer, aiTrainer);
```

### Executing Commands

Commands like attacking, switching Pokémon, or using items can be executed using the command pattern:

```csharp
using PokemonGameLib.Commands;

// Perform an attack
var attackCommand = new AttackCommand(battle, pikachu.Moves.First());
attackCommand.Execute();

// Switch Pokémon
var switchCommand = new SwitchCommand(battle, playerTrainer, charmander);
switchCommand.Execute();

// Use an item
var potion = new Potion("Potion", "Heals 20 HP", 20);
var useItemCommand = new UseItemCommand(battle, playerTrainer, potion, pikachu);
useItemCommand.Execute();
```

### Battle Flow

To run the battle, simply call `StartBattle()` on the `Battle` instance. This will automatically handle turn-taking, Pokémon fainting, and determining the winner.

```csharp
battle.StartBattle();
```

## Logging and Debugging

`PokemonGameLib` includes an integrated logging system. Configure logging at the start of your application:

```csharp
using PokemonGameLib.Utilities;

LoggingService.Configure("path/to/logfile.yml");

// Retrieve and use the logger
var logger = LoggingService.GetLogger();
logger.LogInfo("Battle started!");
```

Logs will be written in YAML format, making it easy to trace and debug the flow of battles.

## Exception Handling

The library provides several custom exceptions to handle common issues in Pokémon battles:

- **`InvalidMoveException`**: Thrown when a Pokémon tries to use a move it cannot perform.
- **`InvalidPokemonSwitchException`**: Thrown when an invalid Pokémon switch is attempted.
- **`ItemNotFoundException`**: Thrown when an item is not found in a trainer's inventory.
- **`PokemonFaintedException`**: Thrown when trying to use a fainted Pokémon.

Example of handling exceptions:

```csharp
try
{
    var invalidMove = new Move("InvalidMove", PokemonType.Fire, power: -1, level: 1);
    pikachu.AddMove(invalidMove);
}
catch (InvalidMoveException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Extending the Library

`PokemonGameLib` is designed to be easily extendable. You can add new Pokémon, moves, items, and even modify battle mechanics to suit your needs.

### Custom Pokémon

Create a new Pokémon by extending the `Pokemon` class or implementing the `IPokemon` interface:

```csharp
var bulbasaur = new Pokemon(
    name: "Bulbasaur", 
    type: PokemonType.Grass, 
    level: 5, 
    maxHp: 45, 
    attack: 49, 
    defense: 49
);
```

### Custom Moves

Define new moves by extending the `Move` class or implementing the `IMove` interface:

```csharp
var solarBeam = new Move(
    name: "Solar Beam", 
    type: PokemonType.Grass, 
    power: 120, 
    level: 50, 
    maxHits: 1
);
```

### Custom Items

Create new items by extending the `Item` class or implementing the `IItem` interface:

```csharp
using PokemonGameLib.Models.Items;

var maxPotion = new Potion("Max Potion", "Fully restores HP", healingAmount: 100);
```

### Custom Battle Mechanics

To implement custom battle mechanics, extend or modify the `Battle` class. Override methods like `PerformAttack` or `SwitchTurns` to introduce new logic.

```csharp
using PokemonGameLib.Models.Battles;

public class CustomBattle : Battle
{
    public CustomBattle(ITrainer trainer1, ITrainer trainer2) : base(trainer1, trainer2)
    {
    }

    protected override void PerformAttack(IMove move)
    {
        // Custom attack logic
    }
}
```

## Contributing

We welcome contributions! Whether it's new features, bug fixes, or improvements, your input is valuable.

### How to Contribute

1. **Fork the Repository**: Create a personal fork of the repository.
2. **Create a Branch**: Make your changes in a new branch.
3. **Implement Changes**: Write and test your code.
4. **Submit a Pull Request**: Propose your changes to be merged into the main repository.

Please follow our [code of conduct](../CODE_OF_CONDUCT.md) and [contributing guidelines](../CONTRIBUTING.md).

## License

`PokemonGameLib` is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

This README provides a comprehensive guide to using and extending `PokemonGameLib`. Dive into the code, experiment with the API, and create your own Pokémon battles! For more detailed examples and API documentation, refer to the inline code comments and explore the provided interfaces and classes. Enjoy building your Pokémon adventure!