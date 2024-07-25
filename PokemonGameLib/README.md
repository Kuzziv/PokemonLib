# PokemonGameLib

**PokemonGameLib** is a .NET library designed for creating and managing a Pokémon game. It provides essential classes and functionality for building a Pokémon game, including features for Pokémon, moves, Trainers, and battles.

## Installation

You can install the library via NuGet. Run the following command in your project directory:

```bash
dotnet add package PokemonGameLib
```

## Usage

Here’s a quick example of how to use **PokemonGameLib** in your project:

### 1. Create a Pokémon

```csharp
using PokemonGameLib.Models;

var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
```

### 2. Access Pokémon Properties

```csharp
Console.WriteLine($"Name: {pikachu.Name}");
Console.WriteLine($"Type: {pikachu.Type}");
Console.WriteLine($"HP: {pikachu.HP}");
Console.WriteLine($"Attack: {pikachu.Attack}");
Console.WriteLine($"Defense: {pikachu.Defense}");
```

### 3. Add Moves to Pokémon

```csharp
var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
pikachu.AddMove(thunderbolt);
```

### 4. Create Trainers

```csharp
var ash = new Trainer("Ash");
ash.AddPokemon(pikachu);

var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
var brock = new Trainer("Brock");
brock.AddPokemon(charizard);
```

### 5. Setup a Battle

```csharp
var battle = new Battle(ash, brock);
battle.PerformAttack(ash, thunderbolt);
```

### 6. Determine Battle Result

```csharp
var result = battle.DetermineBattleResult();
Console.WriteLine(result);
```

## Trainer Class

The `Trainer` class represents a Pokémon Trainer and manages their Pokémon team. Here's how you can use it:

### Create a Trainer

```csharp
var trainer = new Trainer("TrainerName");
```

### Add Pokémon to Trainer

```csharp
var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
trainer.AddPokemon(pokemon);
```

### Access Pokémon of a Trainer

```csharp
var trainerPokemons = trainer.Pokemons;
```

## Release Notes

### Version 1.0.0-beta.1.2

#### New Features
- **Enhanced `Battle` Class Functionality:**
  - Added the ability to specify which Trainer's Pokémon is performing the attack. This allows greater flexibility in simulating battles.
  - Introduced a new public method `PerformAttack(Trainer attackingTrainer, Move move)` in the `Battle` class. This method allows attacks to be executed by a specified Trainer's Pokémon against the opponent's Pokémon.

- **Trainer Class:**
  - Introduced the `Trainer` class to manage Pokémon teams. Trainers can now add Pokémon, and the class provides a way to manage and interact with a Trainer's Pokémon.

#### Bug Fixes
- **Fixed Possible Null Reference Warnings:**
  - Addressed warnings related to possible null references in the `Battle` class. Updated code to ensure proper handling of null values and prevent runtime exceptions.

#### Internal Visibility
- **Internal Method Exposure:**
  - Added `InternalsVisibleTo("PokemonGameLib.Tests")` in the `AssemblyInfo.cs` file. This change allows the test project to access internal methods for improved testing coverage.

#### Tests
- **Updated Test Cases:**
  - Modified existing test cases to accommodate new functionality, such as specifying the Trainer in the `PerformAttack` method.
  - Added tests to ensure the correct behavior of the new features and verify that the bug fixes are effective.

## Documentation

For more information on how to use the library, please refer to the [API Documentation](https://www.nuget.org/packages/PokemonGameLib).

## Contributing

Contributions to **PokemonGameLib** are welcome! If you’d like to contribute:

1. **Fork the Repository**: Create a fork of the repository on GitHub.
2. **Create a Branch**: Create a new branch for your changes.
3. **Make Changes**: Implement your changes and add tests if applicable.
4. **Submit a Pull Request**: Submit a pull request with a description of your changes.

## License

**PokemonGameLib** is licensed under the [MIT License](LICENSE). See the [LICENSE](LICENSE) file for more details.

## Contact

For any questions or support, please contact the author:

- **Name**: Mads Ludvigsen
- **Email**: [Mads72q2@edu.zealand.dk]
