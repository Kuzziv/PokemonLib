Certainly! Here’s an updated `README.md` that covers the basic information, installation instructions, usage examples, and how to contribute:

---

# PokemonGameLib

**PokemonGameLib** is a .NET library designed for creating and managing a Pokémon game. It provides essential classes and functionality for building a Pokémon game, including features for Pokémon, moves, and battles.

## Installation

You can install the library via NuGet. Run the following command in your project directory:

```bash
dotnet add package PokemonGameLib
```

## Usage

Here’s a quick example of how to use **PokemonGameLib** in your project:

1. **Create a Pokémon**

   ```csharp
   using PokemonGameLib;

   var pikachu = new Pokemon("Pikachu", "Electric", 100, 55, 40);
   ```

2. **Access Pokémon Properties**

   ```csharp
   Console.WriteLine($"Name: {pikachu.Name}");
   Console.WriteLine($"Type: {pikachu.Type}");
   Console.WriteLine($"HP: {pikachu.HP}");
   Console.WriteLine($"Attack: {pikachu.Attack}");
   Console.WriteLine($"Defense: {pikachu.Defense}");
   ```

3. **Adding Moves and Battling**

   The library includes functionality for adding moves and setting up battles. For detailed usage, check out the [documentation](https://www.nuget.org/packages/PokemonGameLib).

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

---

Feel free to adjust the email and any other details as needed. This README provides a clear introduction, installation instructions, usage examples, contribution guidelines, and contact information.