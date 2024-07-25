# PokemonGameLib

This repository contains a solution with two projects:

## Projects Overview

### Project 1: PokemonGameLib

#### Description
`PokemonGameLib` is a .NET library for managing and simulating Pokémon game mechanics. It includes functionalities for handling Pokémon data, moves, battles, and more.

#### Features
- Manage Pokémon data including types, stats, and abilities.
- Simulate battles between Pokémon.
- Implement moves and their effects.
- Calculate damage, status conditions, and other game mechanics.

#### Usage
To use the `PokemonGameLib`, include it in your .NET project and call its methods as needed. Here’s an example:

```csharp
using PokemonGameLib.Models;

// Initialize a Pokémon
var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

// Initialize a move
var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
pikachu.AddMove(thunderbolt);

// Initialize Trainers
var ash = new Trainer("Ash");
ash.AddPokemon(pikachu);

var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
var brock = new Trainer("Brock");
brock.AddPokemon(charizard);

// Setup a battle
var battle = new Battle(ash, brock);
battle.PerformAttack(ash, thunderbolt);
Console.WriteLine(battle.DetermineBattleResult());
```

## Setup and Testing

To set up and test the `PokemonGameLib` library, follow these steps:

### Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Setting Up the Library

1. **Clone the Repository**

   ```bash
   git clone https://github.com/YourUsername/PokemonGameLib.git
   cd PokemonGameLib
   ```

2. **Restore NuGet Packages**

   Restore the packages for both the library and test projects:

   ```bash
   dotnet restore
   ```

3. **Build the Projects**

   Build the solution to ensure everything is set up correctly:

   ```bash
   dotnet build
   ```

### Running Tests

The solution includes a test project that uses [xUnit](https://xunit.net/) for unit testing. Follow these steps to run the tests:

1. **Navigate to the Test Project**

   ```bash
   cd PokemonGameLib.Tests
   ```

2. **Run the Tests**

   Execute the tests to ensure the library functions as expected:

   ```bash
   dotnet test
   ```

### Adding and Running New Tests

1. **Add New Test Cases**

   - Open the `PokemonGameLib.Tests` project in your editor.
   - Add new test classes or methods as needed in the `PokemonGameLib.Tests` folder.
   - Ensure new tests cover the features or bug fixes you’ve added.

2. **Run Tests Again**

   After adding new tests, run `dotnet test` to verify that all tests pass.

## Contribution

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
