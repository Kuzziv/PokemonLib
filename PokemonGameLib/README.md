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

- **Name**: Mads Ludvigsen
- **Email**: [Mads72q2@edu.zealand.dk]
