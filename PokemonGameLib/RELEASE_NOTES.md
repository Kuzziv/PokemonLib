## PokemonGameLib

### Version 8.0.0-Relase

#### Overview

This release introduces significant enhancements and new features to the `PokemonGameLib`, including the addition of a comprehensive `Battle` class to manage Pokémon battles, improvements to the `Trainer`, `Pokemon`, and `Move` classes, and better handling of type effectiveness and damage calculation.

#### Key Updates

1. **New `Battle` Class**
   - **Purpose**: Manages the interaction between two Trainers in a Pokémon battle.
   - **Features**:
     - **Turn Management**: Tracks and manages which Trainer is currently attacking.
     - **Attack Execution**: Facilitates the use of Pokémon moves to perform attacks during a battle.
     - **Damage Calculation**: Determines the damage dealt during an attack based on move power, attacker and defender stats, and type effectiveness.
     - **Battle Outcome**: Determines and displays the result of the battle, including fainted Pokémon and the winning Trainer.

2. **`Trainer` Class Enhancements**
   - **Pokemon Management**: Now includes methods to check if the Trainer has any valid (non-fainted) Pokémon.
   - **Switching Pokémon**: Trainers can switch their current Pokémon during a battle, with validation to prevent switching to fainted Pokémon or Pokémon not owned by the Trainer.

3. **`Pokemon` Class Enhancements**
   - **Move Management**: Pokémon can learn up to 4 moves, with validations to ensure moves are compatible with the Pokémon's type and level.
   - **Damage Handling**: Improved methods for applying damage and determining if a Pokémon has fainted.
   - **Current vs. Max HP**: Clear distinction between a Pokémon's current HP and max HP, improving clarity in code.

4. **`Move` Class Enhancements**
   - **Move Compatibility**: Includes checks to ensure moves are compatible with the Pokémon’s type and level.
   - **Damage Calculation**: Integrated into the `Battle` class for more streamlined attack execution.

5. **Type Effectiveness**
   - **`TypeEffectiveness` Class**: Provides a static method to determine the effectiveness of a move based on type matchups.
   - **Effectiveness Multiplier**: Calculates how effective a move is against a specific Pokémon type, with modifiers for super effective, not very effective, and no effect scenarios.

6. **Battle Flow**
   - **Turn-Based System**: Battle class now manages which Trainer's Pokémon is attacking and switches turns after each move.
   - **Effectiveness Feedback**: After each attack, the game provides feedback on how effective the move was, adding depth to battle strategy.

7. **Error Handling**
   - **Null Checks**: Ensures that critical objects (e.g., Pokémon, moves) are not null before performing actions.
   - **Invalid Actions**: Prevents invalid actions, such as using a move that the current Pokémon doesn’t know or switching to a fainted Pokémon.

#### Usage Updates

- **Expanded Usage Examples**: The README now includes detailed examples covering the creation of Trainers and Pokémon, managing Pokémon teams, adding moves, and performing battles.
- **Switching Pokémon**: Emphasized the need to set the current Pokémon for each Trainer before initiating a battle.
- **Damage Calculation**: Clarified the factors involved in damage calculation, including move power, Pokémon stats, type effectiveness, STAB (Same Type Attack Bonus), and random variability.

#### Future Considerations

- **Enhanced Battle Mechanics**: Explore additional features such as move effects, abilities, and multi-turn moves.
- **Improved Type Effectiveness**: Further refine type matchups and consider dual-type Pokémon for more complex battle scenarios.
- **AI Enhancements**: Implement smarter AI behavior for Trainer battles, including strategy-based move selection and Pokémon switching.
