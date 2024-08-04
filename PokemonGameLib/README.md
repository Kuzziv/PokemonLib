```markdown
# PokemonGameLib

**PokemonGameLib** is a .NET library designed for creating and managing a Pokémon game. It provides essential classes and functionality for building a Pokémon game, including features for Pokémon, moves, trainers, and battles.

## Installation

You can install the library via NuGet. Run the following command in your project directory:

```bash
dotnet add package PokemonGameLib
```

## Usage

Here’s a quick example of how to use **PokemonGameLib** in your project:

### 1. Create a Pokémon

To create a Pokémon, instantiate the `Pokemon` class with its name, type, level, maximum HP, attack, and defense stats.

```csharp
using PokemonGameLib.Models;

var pikachu = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
```

### 2. Access Pokémon Properties

You can access the properties of a Pokémon object to retrieve information such as its name, type, current HP, attack, and defense stats.

```csharp
Console.WriteLine($"Name: {pikachu.Name}");
Console.WriteLine($"Type: {pikachu.Type}");
Console.WriteLine($"Level: {pikachu.Level}");
Console.WriteLine($"Max HP: {pikachu.MaxHP}");
Console.WriteLine($"Current HP: {pikachu.CurrentHP}");
Console.WriteLine($"Attack: {pikachu.Attack}");
Console.WriteLine($"Defense: {pikachu.Defense}");
```

### 3. Add Moves to Pokémon

To add moves to a Pokémon, create a `Move` object and add it to the Pokémon's move list using the `AddMove` method.

```csharp
var thunderbolt = new Move("Thunderbolt", PokemonType.Electric, 90, 10);
pikachu.AddMove(thunderbolt);
```

### 4. Create Trainers

Create trainer objects to manage teams of Pokémon. Each trainer can have multiple Pokémon.

```csharp
var ash = new Trainer("Ash");
ash.AddPokemon(pikachu);

var charizard = new Pokemon("Charizard", PokemonType.Fire, 10, 150, 70, 50);
var brock = new Trainer("Brock");
brock.AddPokemon(charizard);
```

### 5. Setup a Battle

Initialize a battle between two trainers using the `Battle` class and perform attacks using their Pokémon's moves.

```csharp
var battle = new Battle(ash, brock);

// Ensure the attacking Pokémon is set
ash.SwitchPokemon(pikachu);
brock.SwitchPokemon(charizard);

// Perform attack
battle.PerformAttack(thunderbolt);
```

### 6. Determine Battle Result

After performing attacks, determine the result of the battle using the `DetermineBattleResult` method.

```csharp
var result = battle.DetermineBattleResult();
Console.WriteLine(result);
```

## Trainer Class

The `Trainer` class represents a Pokémon trainer and manages their Pokémon team. Here's how you can use it:

### Create a Trainer

Instantiate the `Trainer` class to create a new trainer.

```csharp
var trainer = new Trainer("TrainerName");
```

### Add Pokémon to Trainer

Add Pokémon to a trainer's team using the `AddPokemon` method.

```csharp
var pokemon = new Pokemon("Pikachu", PokemonType.Electric, 10, 100, 55, 40);
trainer.AddPokemon(pokemon);
```

### Access Pokémon of a Trainer

Access the list of a trainer's Pokémon using the `Pokemons` property.

```csharp
var trainerPokemons = trainer.Pokemons;
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## Contact

For any inquiries or feedback, please contact:

- **Name**: Mads Ludvigsen
- **Email**: [Mads72q2@edu.zealand.dk](mailto:Mads72q2@edu.zealand.dk)
```

### Key Updates:

- **Corrected Property Names**: Changed `HP` to `CurrentHP` and `MaxHP` for clarity and accuracy.
- **Switching Pokémon**: Added the step to ensure Pokémon are switched in trainers before battles, as `SwitchPokemon` must be called to set the current Pokémon.
- **Usage Example**: Expanded code examples to include all steps necessary to use the library effectively.
- **Contact Information**: Included an email link for easy communication.

This README now provides a more comprehensive guide to using the `PokemonGameLib` library and ensures that the examples are correct and useful. Let me know if there are any other details you would like to include!