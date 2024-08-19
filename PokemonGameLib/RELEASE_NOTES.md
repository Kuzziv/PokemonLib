## Release Notes - Pokémon Game Library

### New Features

- **Normal-Type Move Compatibility**
  - You can now add Normal-type moves to any Pokémon, regardless of their primary type. This addresses the issue where adding a Normal-type move to a non-Normal Pokémon would previously throw an exception.

### Bug Fixes

- **Switching Pokémon Mid-Fight**
  - Fixed an issue where switching a Pokémon during a battle could throw an `InvalidMoveException`. This ensures that Pokémon can now be switched mid-fight without errors.

- **HP Display Throughout Battle**
  - Improved the battle interface by displaying Pokémon's HP consistently throughout the battle. Players no longer need to keep track of HP mentally.

- **Menu Exit Button**
  - Added an exit option in the item usage menu during battles. Players can now choose to go back to the main action menu instead of being forced to use an item.

- **Potion Usage Error**
  - Resolved an error where using a Potion during battle would throw an `InvalidMoveException`. Healing items can now be used without causing any crashes.

- **Switching to an Already Active Pokémon**
  - Fixed an issue where attempting to switch to the currently active Pokémon would throw an `InvalidPokemonSwitchException`. The game now properly handles this scenario, preventing unnecessary errors.

### Known Issues

- None reported at this time.

### Code Refinement and Restructuring

In this release, significant improvements have been made to the underlying architecture of the Pokémon Game Library:

- **Enhanced Modularity through Interfaces**:
  - The core components of the library have been refactored to utilize interfaces extensively. This change increases modularity and flexibility, allowing developers to easily extend or customize the library's functionality without modifying the core codebase.
  - Key interfaces like `IPokemon`, `IMove`, `ITrainer`, and `IBattle` now define the contract for each component, ensuring consistent and predictable behavior across different implementations.

- **Decoupled and Testable Code**:
  - By leveraging interfaces, the codebase has been decoupled, meaning that individual components are now more independent. This decoupling facilitates better unit testing, as each part of the system can be tested in isolation using mock implementations of these interfaces.
  - The use of dependency injection patterns (passing interfaces instead of concrete classes) has also been adopted, making the system more flexible and easier to maintain.

- **Improved Maintainability**:
  - The restructuring efforts focused on adhering to SOLID principles (Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion). This makes the code easier to maintain, understand, and extend in future development cycles.
  - The introduction of clear and concise logging, via an `ILogger` interface, ensures that critical operations and errors are logged consistently across the application, aiding in debugging and operational monitoring.

- **Enhanced Extensibility**:
  - Developers can now introduce new types of Pokémon, moves, and trainers by simply implementing the corresponding interfaces (`IPokemon`, `IMove`, `ITrainer`). This reduces the risk of breaking existing functionality while adding new features.
  - The refactored structure also allows for easier integration with other libraries or systems, as the interfaces provide a clear boundary for interaction.

### Contributors

- @Kuzziv

