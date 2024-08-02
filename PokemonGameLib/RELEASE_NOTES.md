## Release Notes

### Version beta.1.3.0

#### Overview

This release introduces a changes to, `Battle.cs`

#### Key Changes

1. **New `Battle` Class**
   - **Description:** Manages the interaction between two Trainers in a Pokémon battle, including attack execution, damage calculation, and battle outcome determination.
   - **Features:**
     - `AttackingTrainer`: Gets the Trainer initiating the attack.
     - `DefendingTrainer`: Gets the Trainer receiving the attack.
     - `Attacker`: Gets the current Pokémon of the attacking Trainer.
     - `Defender`: Gets the current Pokémon of the defending Trainer.

2. **Constructor Enhancements**
   - Added validation to ensure that neither `attackingTrainer` nor `defendingTrainer` is null.
   - Validates that both Trainers have valid Pokémon before initiating a battle.

3. **`PerformAttack` Method**
   - Executes an attack from the specified Trainer's Pokémon.
   - Validates that the Pokémon performing the attack is not fainted and knows the move.
   - Validates that the defending Pokémon is not fainted.
   - Calculates and applies damage based on move effectiveness and Pokémon stats.
   - Outputs detailed battle messages including attack effectiveness and damage taken.

4. **Damage Calculation**
   - **Method:** `CalculateDamage`
   - **Description:** Computes the damage dealt during an attack based on move power, attacker and defender stats, and type effectiveness.

5. **Effectiveness Text**
   - **Method:** `GetEffectivenessText`
   - **Description:** Returns a descriptive string indicating how effective the move was (e.g., super effective, not very effective).

6. **Battle Result Determination**
   - **Method:** `DetermineBattleResult`
   - **Description:** Provides a string indicating the result of the battle, including which Trainer wins if a Pokémon has fainted.

7. **Supporting Classes**
   - **`Trainer` Class:** Represents a Trainer with a list of Pokémon and methods to check if the Trainer has valid Pokémon.
   - **`Pokemon` Class:** Represents a Pokémon with properties for health, stats, moves, and methods for taking damage and checking if it’s fainted.
   - **`Move` Class:** Represents a Pokémon move with properties for name, power, and type.
   - **`TypeEffectiveness` Class:** Provides a static method to determine the effectiveness of a move based on type.

#### Error Handling

- Throws `ArgumentNullException` if critical parameters (like moves) are null.
- Throws `InvalidOperationException` if actions are attempted under invalid conditions (e.g., attacking with a fainted Pokémon).

#### Future Considerations

- Enhance type effectiveness logic in `TypeEffectiveness`.
- Expand error handling to cover more edge cases.
- Add features for move effects and Pokémon abilities.

---

This release note provides a clear overview of the new features and changes made in the `Battle` class, as well as the supporting classes that enable its functionality.