# Pokémon Game Library

This repository contains a solution with two projects designed to simulate Pokémon game mechanics within a .NET environment.

## Projects Overview

### Project 1: PokemonGameLib

#### Description
`PokemonGameLib` is a robust .NET library that provides all the necessary functionalities to manage and simulate core Pokémon game mechanics. It is designed for developers who want to create Pokémon-like games, implement battle simulations, or study Pokémon game mechanics in a programmatic way.

#### Features
- **Pokémon Management**: Handle Pokémon data including types, stats, abilities, moves, and evolutions.
- **Battle System**: Simulate battles between Pokémon, considering type advantages, status conditions, and move effectiveness.
- **Move Implementation**: Define moves, calculate damage, apply status effects, and manage special abilities.
- **Trainer Management**: Manage trainers and their Pokémon teams, enabling AI or human-controlled battle scenarios.
- **Logging**: Integrated logging to track battle events and other key operations.

#### Usage
To use `PokemonGameLib`, include it in your .NET project. Below is an example demonstrating how to set up a battle between two Pokémon:

```csharp
using PokemonGameLib.Models;
using PokemonGameLib.Models.Pokemons;
using PokemonGameLib.Models.Pokemons.Moves;
using PokemonGameLib.Models.Trainers;
using PokemonGameLib.Models.Battles;

// Initialize a Pokémon
var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);

// Initialize a move
var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
pikachu.AddMove(thunderbolt);

// Initialize Trainers
var ash = new PlayerTrainer("Ash");
ash.AddPokemon(pikachu);
ash.CurrentPokemon = pikachu;

var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
var brock = new AITrainer("Brock");
brock.AddPokemon(charizard);
brock.CurrentPokemon = charizard;

// Setup a battle
var battle = new Battle(ash, brock);
battle.PerformAttack(thunderbolt);
Console.WriteLine(battle.DetermineBattleResult());
```

### Project 2: PokemonGameLib.Tests

#### Description
The `PokemonGameLib.Tests` project is a collection of unit tests that ensure the library's functionalities work as expected. The tests are built using [xUnit](https://xunit.net/), a popular .NET testing framework.

## Setup and Testing

### Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Setting Up the Library

1. **Clone the Repository**

   ```bash
   git clone https://github.com/kuzziv/pokemonLib.git
   cd pokemonLib
   ```

2. **Restore NuGet Packages**

   Restore the necessary packages for both the library and test projects:

   ```bash
   dotnet restore
   ```

3. **Build the Projects**

   Build the solution to ensure everything is configured correctly:

   ```bash
   dotnet build
   ```

### Running Tests

The solution includes a test project that uses xUnit for unit testing. Follow these steps to run the tests:

1. **Navigate to the Test Project**

   ```bash
   cd PokemonGameLib.Tests
   ```

2. **Run the Tests**

   Execute the tests to ensure the library's functionalities work as expected:

   ```bash
   dotnet test
   ```

### Adding and Running New Tests

1. **Add New Test Cases**

   - Open the `PokemonGameLib.Tests` project in your editor.
   - Add new test classes or methods as needed in the `PokemonGameLib.Tests` folder.
   - Ensure new tests cover the features or bug fixes you’ve implemented.

2. **Run Tests Again**

   After adding new tests, run `dotnet test` to verify that all tests pass successfully.

## Contribution

Contributions to **PokemonGameLib** are welcome! If you’d like to contribute:

1. **Fork the Repository**: Create a fork of the repository on GitHub.
2. **Create a Branch**: Create a new branch for your changes.
3. **Make Changes**: Implement your changes and add tests if applicable.
4. **Submit a Pull Request**: Submit a pull request with a detailed description of your changes.

## License

**PokemonGameLib** is licensed under the [MIT License](https://github.com/kuzziv/pokemonLib/blob/main/LICENSE). See the [LICENSE](https://github.com/kuzziv/pokemonLib/blob/main/LICENSE) file for more details.

## Contact

For any questions or support, please contact the author:

- **Name**: Mads Ludvigsen
- **Email**: [Mads72q2@edu.zealand.dk](mailto:Mads72q2@edu.zealand.dk)

You can also follow the project or contribute directly via its [GitHub repository](https://github.com/kuzziv/pokemonLib).

