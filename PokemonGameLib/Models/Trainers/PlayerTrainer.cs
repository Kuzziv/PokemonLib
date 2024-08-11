using System;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a human player trainer in the Pokémon game.
    /// </summary>
    public class PlayerTrainer : Trainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTrainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the player trainer.</param>
        public PlayerTrainer(string name) : base(name) { }

        /// <summary>
        /// Takes the player's turn during a battle.
        /// The player can choose to attack, switch Pokémon, or use an item.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        public override void TakeTurn(IBattle battle)
        {
            while (true)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Switch Pokémon");
                Console.WriteLine("3. Use Item");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PerformAttack(battle);
                        return;
                    case "2":
                        SwitchPokemon();
                        return;
                    case "3":
                        UseItem();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void PerformAttack(IBattle battle)
        {
            Console.WriteLine("Choose a move:");
            for (int i = 0; i < CurrentPokemon.Moves.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {CurrentPokemon.Moves[i].Name}");
            }

            if (int.TryParse(Console.ReadLine(), out int moveIndex) && moveIndex >= 1 && moveIndex <= CurrentPokemon.Moves.Count)
            {
                IMove selectedMove = CurrentPokemon.Moves[moveIndex - 1];
                battle.PerformAttack(selectedMove);
            }
            else
            {
                Console.WriteLine("Invalid move. Turn skipped.");
            }
        }

        private void SwitchPokemon()
        {
            Console.WriteLine("Choose a Pokémon to switch to:");
            for (int i = 0; i < Pokemons.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Pokemons[i].Name} (HP: {Pokemons[i].CurrentHP}/{Pokemons[i].MaxHP})");
            }

            if (int.TryParse(Console.ReadLine(), out int pokemonIndex) && pokemonIndex >= 1 && pokemonIndex <= Pokemons.Count)
            {
                IPokemon selectedPokemon = Pokemons[pokemonIndex - 1];
                if (selectedPokemon.IsFainted())
                {
                    Console.WriteLine("Cannot switch to a fainted Pokémon. Turn skipped.");
                }
                else
                {
                    SwitchPokemon(selectedPokemon);
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Turn skipped.");
            }
        }

        private void UseItem()
        {
            Console.WriteLine("Choose an item to use:");
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
            }

            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= Items.Count)
            {
                IItem selectedItem = Items[itemIndex - 1];
                UseItem(selectedItem, CurrentPokemon);
            }
            else
            {
                Console.WriteLine("Invalid item. Turn skipped.");
            }
        }
    }
}
