using System;
using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Models.Trainers
{
    public class PlayerTrainer : Trainer
    {
        public PlayerTrainer(string name) : base(name) { }

        public override void TakeTurn(IBattle battle)
        {
            int invalidAttempts = 0;
            const int maxInvalidAttempts = 3;
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
                        SwitchPokemon(battle);
                        return;
                    case "3":
                        UseItem(battle);
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        invalidAttempts++;
                        if (invalidAttempts >= maxInvalidAttempts)
                        {
                            Console.WriteLine("Too many invalid attempts. Turn skipped.");
                            return;
                        }
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

        private void SwitchPokemon(IBattle battle)
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
                    battle.PerformSwitch(selectedPokemon);
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Turn skipped.");
            }
        }

        private void UseItem(IBattle battle)
        {
            Console.WriteLine("Choose an item to use:");
            for (int i = 0; i < Items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Items[i].Name} - {Items[i].Description}");
            }

            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= Items.Count)
            {
                IItem selectedItem = Items[itemIndex - 1];
                Console.WriteLine("Choose a Pokémon to use the item on:");
                for (int i = 0; i < Pokemons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Pokemons[i].Name} (HP: {Pokemons[i].CurrentHP}/{Pokemons[i].MaxHP})");
                }

                if (int.TryParse(Console.ReadLine(), out int pokemonIndex) && pokemonIndex >= 1 && pokemonIndex <= Pokemons.Count)
                {
                    IPokemon targetPokemon = Pokemons[pokemonIndex - 1];
                    battle.PerformUseItem(selectedItem, targetPokemon);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Turn skipped.");
                }
            }
            else
            {
                Console.WriteLine("Invalid item. Turn skipped.");
            }
        }
    }
}
