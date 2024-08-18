using System;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;

namespace PokemonGameLib.Models.Trainers
{
    /// <summary>
    /// Represents a human player trainer in the Pokémon game.
    /// </summary>
    public class PlayerTrainer : Trainer
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTrainer"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the player trainer.</param>
        /// <param name="logger">The logger to use for logging actions.</param>
        public PlayerTrainer(string name) : base(name)
        {
            _logger = LoggingService.GetLogger(); // Retrieve the logger from the LoggingService
        }

        /// <summary>
        /// Takes the player's turn during a battle.
        /// The player can choose to attack, switch Pokémon, or use an item.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
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

        /// <summary>
        /// Handles the player's choice to perform an attack.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
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
                var attackCommand = CreateAttackCommand(battle, selectedMove);
                attackCommand.Execute();
                _logger.LogInfo($"{Name} used {selectedMove.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid move. Turn skipped.");
                _logger.LogWarning($"{Name} attempted to use an invalid move. Turn skipped.");
            }
        }

        /// <summary>
        /// Handles the player's choice to switch Pokémon.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
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
                    _logger.LogWarning($"{Name} attempted to switch to a fainted Pokémon. Turn skipped.");
                }
                else
                {
                    var switchCommand = CreateSwitchCommand(battle, selectedPokemon);
                    switchCommand.Execute();
                    _logger.LogInfo($"{Name} switched to {selectedPokemon.Name}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Turn skipped.");
                _logger.LogWarning($"{Name} attempted to switch to an invalid Pokémon. Turn skipped.");
            }
        }

        /// <summary>
        /// Handles the player's choice to use an item.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
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
                var useItemCommand = CreateUseItemCommand(battle, selectedItem, CurrentPokemon);
                useItemCommand.Execute();
                _logger.LogInfo($"{Name} used {selectedItem.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid item. Turn skipped.");
                _logger.LogWarning($"{Name} attempted to use an invalid item. Turn skipped.");
            }
        }
    }
}
