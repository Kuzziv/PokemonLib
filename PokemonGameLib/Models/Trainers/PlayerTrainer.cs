using System;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a player-controlled trainer in a Pokémon battle.
    /// </summary>
    public class PlayerTrainer : Trainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTrainer"/> class.
        /// </summary>
        /// <param name="name">The name of the player trainer.</param>
        public PlayerTrainer(string name) : base(name) { }

        /// <summary>
        /// Executes the player's turn in the battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public override void TakeTurn(IBattle battle)
        {
            while (true)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Attack with " + (CurrentPokemon.Name));
                Console.WriteLine("2. Switch Pokémon");
                Console.WriteLine("3. Use Item");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (!PerformAttack(battle))
                        {
                            continue;  // If the player chooses to go back to the main menu, continue the loop
                        }
                        break;  // Return to the main action menu after attacking
                    case "2":
                        if (!SwitchPokemon(battle))
                        {
                            continue;  // If the player chooses to go back to the main menu, continue the loop
                        }
                        break;
                    case "3":
                        if (!UseItem(battle))
                        {
                            continue;  // If the player chooses to go back to the main menu, continue the loop
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;  // Re-display the main menu if an invalid choice is made
                }
                break;  // End the loop after performing an action
            }
        }

        /// <summary>
        /// Performs an attack with the player's current Pokémon.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>True if the attack was performed, otherwise false.</returns>
        private bool PerformAttack(IBattle battle)
        {
            while (true)
            {
                Console.WriteLine("Choose a move:");
                for (int i = 0; i < CurrentPokemon.Moves.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {CurrentPokemon.Moves[i].Name}");
                }
                Console.WriteLine($"{CurrentPokemon.Moves.Count + 1}. Back to Menu");

                if (int.TryParse(Console.ReadLine(), out int moveIndex))
                {
                    if (moveIndex == CurrentPokemon.Moves.Count + 1)
                    {
                        return false;  // Go back to the main action menu
                    }
                    else if (moveIndex >= 1 && moveIndex <= CurrentPokemon.Moves.Count)
                    {
                        IMove selectedMove = CurrentPokemon.Moves[moveIndex - 1];
                        battle.PerformAttack(selectedMove);
                        return true;  // Return after performing an attack
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Please try again.");
                    }
                }
            }
        }

        /// <summary>
        /// Switches the player's current Pokémon to another Pokémon in their party.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>True if the Pokémon was switched, otherwise false.</returns>
        private bool SwitchPokemon(IBattle battle)
        {
            while (true)
            {
                Console.WriteLine("Choose a Pokémon to switch to:");
                for (int i = 0; i < Pokemons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Pokemons[i].Name} (HP: {Pokemons[i].CurrentHP}/{Pokemons[i].MaxHP})");
                }
                Console.WriteLine($"{Pokemons.Count + 1}. Back to Menu");

                if (int.TryParse(Console.ReadLine(), out int pokemonIndex))
                {
                    if (pokemonIndex == Pokemons.Count + 1)
                    {
                        return false;  // Go back to the main action menu
                    }
                    else if (pokemonIndex >= 1 && pokemonIndex <= Pokemons.Count)
                    {
                        IPokemon selectedPokemon = Pokemons[pokemonIndex - 1];
                        if (selectedPokemon.IsFainted())
                        {
                            Console.WriteLine("Cannot switch to a fainted Pokémon. Please choose again.");
                        }
                        else
                        {
                            battle.PerformSwitch(this, selectedPokemon);
                            return true;  // Return after switching Pokémon
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }
            }
        }

        /// <summary>
        /// Uses an item from the player's inventory on a Pokémon.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        /// <returns>True if the item was used, otherwise false.</returns>
        private bool UseItem(IBattle battle)
        {
            while (true)
            {
                Console.WriteLine("Choose an item to use:");
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
                }
                Console.WriteLine($"{Items.Count + 1}. Back to Menu");

                if (int.TryParse(Console.ReadLine(), out int itemIndex))
                {
                    if (itemIndex == Items.Count + 1)
                    {
                        return false;  // Go back to the main action menu
                    }
                    else if (itemIndex >= 1 && itemIndex <= Items.Count)
                    {
                        IItem selectedItem = Items[itemIndex - 1];
                        Console.WriteLine("Choose a Pokémon to use the item on:");
                        for (int i = 0; i < Pokemons.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Pokemons[i].Name} (HP: {Pokemons[i].CurrentHP}/{Pokemons[i].MaxHP})");
                        }
                        Console.WriteLine($"{Pokemons.Count + 1}. Back to Menu");

                        if (int.TryParse(Console.ReadLine(), out int pokemonIndex))
                        {
                            if (pokemonIndex == Pokemons.Count + 1)
                            {
                                return false;  // Go back to the item selection menu
                            }
                            else if (pokemonIndex >= 1 && pokemonIndex <= Pokemons.Count)
                            {
                                IPokemon targetPokemon = Pokemons[pokemonIndex - 1];
                                battle.PerformUseItem(selectedItem, targetPokemon);
                                Items.Remove(selectedItem);
                                return true;  // Return after using an item
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice. Please try again.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid item. Please try again.");
                    }
                }
            }
        }

        /// <summary>
        /// Handles the situation when the player's current Pokémon has fainted.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public override void HandleFaintedPokemon(IBattle battle)
        {
            Console.WriteLine($"{CurrentPokemon.Name} has fainted!");

            while (true)
            {
                Console.WriteLine("Choose a Pokémon to switch to:");
                for (int i = 0; i < Pokemons.Count; i++)
                {
                    if (!Pokemons[i].IsFainted())
                    {
                        Console.WriteLine($"{i + 1}. {Pokemons[i].Name} (HP: {Pokemons[i].CurrentHP}/{Pokemons[i].MaxHP})");
                    }
                }

                if (int.TryParse(Console.ReadLine(), out int pokemonIndex) && pokemonIndex >= 1 && pokemonIndex <= Pokemons.Count)
                {
                    IPokemon selectedPokemon = Pokemons[pokemonIndex - 1];
                    if (selectedPokemon.IsFainted())
                    {
                        Console.WriteLine("Cannot switch to a fainted Pokémon. Please choose again.");
                    }
                    else
                    {
                        CurrentPokemon = selectedPokemon;
                        battle.PerformSwitch(this, selectedPokemon);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

    }
}
