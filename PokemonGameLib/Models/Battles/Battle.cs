using System;
using System.Linq;
using System.Collections.Generic;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Models.Battles
{
    /// <summary>
    /// Represents a Pokémon battle between two trainers.
    /// </summary>
    public class Battle : Loggable, IBattle
    {
        /// <summary>
        /// Gets the current trainer whose turn it is.
        /// </summary>
        public ITrainer CurrentTrainer { get; private set; }

        /// <summary>
        /// Gets the opposing trainer in the battle.
        /// </summary>
        public ITrainer OpponentTrainer => CurrentTrainer == _trainer1 ? _trainer2 : _trainer1;

        private readonly ITrainer _trainer1;
        private readonly ITrainer _trainer2;
        private readonly BattleCalculator _battleCalculator;

        private Queue<string> _lastFourActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="trainer1">The first trainer participating in the battle.</param>
        /// <param name="trainer2">The second trainer participating in the battle.</param>
        public Battle(ITrainer trainer1, ITrainer trainer2)
        {
            _trainer1 = trainer1;
            _trainer2 = trainer2;
            CurrentTrainer = _trainer1;
            _battleCalculator = new BattleCalculator();
            _lastFourActions = new Queue<string>();
        }

        /// <summary>
        /// Starts the Pokémon battle and continues until a trainer is victorious.
        /// </summary>
        public void StartBattle()
        {
            LogInfo("The battle has started!");

            while (!IsBattleOver())
            {
                PrintBattleStatus();
                
                Console.WriteLine($"{CurrentTrainer.Name}'s turn.\n");

                CurrentTrainer.TakeTurn(this);

                if (!IsBattleOver())
                {
                    SwitchTurns();
                }
            }

            PrintBattleResult();
        }

        /// <summary>
        /// Performs an attack using a specified move in the battle.
        /// </summary>
        /// <param name="move">The move to be used for the attack.</param>
        public void PerformAttack(IMove move)
        {
            var attacker = CurrentTrainer.CurrentPokemon;
            var defender = OpponentTrainer.CurrentPokemon;

            BattleValidator.ValidateMove(attacker, move);

            int damage = _battleCalculator.CalculateDamage(attacker, defender, move);
            defender.TakeDamage(damage);

            string action = $"{CurrentTrainer.Name}'s {attacker.Name} used {move.Name} on {defender.Name}, dealing {damage} damage!";
            AddToLastTwoActions(action);

            LogInfo(action);

            if (defender.CurrentHP <= 0)
            {
                // check if the trainer has any pokemons not finted pokemon left
                if (OpponentTrainer.Pokemons.Any(p => !p.IsFainted()))
                {
                    LogInfo($"{defender.Name} fainted!");
                    OpponentTrainer.HandleFaintedPokemon(this);
                }
                else
                {
                    LogInfo($"{defender.Name} fainted!");
                    LogInfo($"{OpponentTrainer.Name} has no more Pokémon left!");
                    GetWinner();
                }
            }
        }

        /// <summary>
        /// Switches the current Pokémon of a trainer with a new Pokémon.
        /// </summary>
        /// <param name="trainer">The trainer performing the switch.</param>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        public void PerformSwitch(ITrainer trainer, IPokemon newPokemon)
        {
            string action = $"{trainer.Name} switched {trainer.CurrentPokemon.Name} out for {newPokemon.Name}.";
            AddToLastTwoActions(action);

            LogInfo(action);

            BattleValidator.ValidatePokemonSwitch(trainer, newPokemon);
            trainer.CurrentPokemon = newPokemon;
        }

        /// <summary>
        /// Uses an item on a target Pokémon during the battle.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <param name="targetPokemon">The target Pokémon that the item will be used on.</param>
        public void PerformUseItem(IItem item, IPokemon targetPokemon)
        {
            string action = $"{CurrentTrainer.Name} used {item.Name} on {targetPokemon.Name}. and {item.Description}";
            AddToLastTwoActions(action);

            LogInfo(action);

            item.Use(CurrentTrainer, targetPokemon);
        }

        /// <summary>
        /// Switches turns between the two trainers.
        /// </summary>
        public void SwitchTurns()
        {
            CurrentTrainer = OpponentTrainer;
            LogInfo($"It is now {CurrentTrainer.Name}'s turn.");
        }

        /// <summary>
        /// Determines if the battle is over, i.e., if all Pokémon of either trainer have fainted.
        /// </summary>
        /// <returns>True if the battle is over (all Pokémon of either trainer have fainted), otherwise false.</returns>
        public bool IsBattleOver()
        {
            bool trainer1AllFainted = _trainer1.Pokemons.All(p => p.IsFainted());
            bool trainer2AllFainted = _trainer2.Pokemons.All(p => p.IsFainted());

            return trainer1AllFainted || trainer2AllFainted;
        }

        /// <summary>
        /// Gets the winner of the battle.
        /// </summary>
        /// <returns>The trainer who won the battle, or null if the battle is not yet over.</returns>
        public ITrainer? GetWinner()
        {
            if (_trainer1.Pokemons.All(p => p.IsFainted())) return _trainer2;
            if (_trainer2.Pokemons.All(p => p.IsFainted())) return _trainer1;
            return null;
        }

        /// <summary>
        /// Determines the result of the battle.
        /// </summary>
        /// <returns>A string representing the result of the battle.</returns>
        public string DetermineBattleResult()
        {
            if (IsBattleOver())
            {
                var winner = GetWinner();
                if (winner != null)
                {
                    return $"{winner.Name} wins the battle!";
                }
                else
                {
                    return "The battle ended in a draw!";
                }
            }

            return "The battle is ongoing.";
        }

        /// <summary>
        /// Prints the current status of the battle, including each trainer's active Pokémon and their HP.
        /// </summary>
        public void PrintBattleStatus()
        {
            Console.Clear(); // Clear the console for a cleaner output
            Console.WriteLine("===== BATTLE STATUS =====\n");

            Console.WriteLine($"{_trainer1.Name}: {(CurrentTrainer == _trainer1 ? "(Current)" : "")}");
            PrintPokemonStatus(_trainer1.CurrentPokemon);

            Console.WriteLine($"{_trainer2.Name}: {(CurrentTrainer == _trainer2 ? "(Current)" : "")}");
            PrintPokemonStatus(_trainer2.CurrentPokemon);

            if (_lastFourActions.Count > 0)
            {
                Console.WriteLine("\nLast four actions:");
                foreach (var action in _lastFourActions)
                {
                    Console.WriteLine($"- {action}");
                }
            }

            Console.WriteLine("\n=========================");
            Console.WriteLine($"{_trainer1.CurrentPokemon.Name} (controlled by {_trainer1.Name}) vs. {_trainer2.CurrentPokemon.Name} (controlled by {_trainer2.Name})\n");
        }

        /// <summary>
        /// Prints the status of a given Pokémon, including its name and current HP.
        /// </summary>
        /// <param name="pokemon">The Pokémon whose status is being printed.</param>
        private void PrintPokemonStatus(IPokemon pokemon)
        {
            Console.WriteLine($"- {pokemon.Name} (HP: {pokemon.CurrentHP}/{pokemon.MaxHP})");
            if (pokemon.Status != StatusCondition.None)
            {
                Console.WriteLine($"  Status: {pokemon.Status}");
            }
        }

        /// <summary>
        /// Prints the result of the battle.
        /// </summary>
        public void PrintBattleResult()
        {
            Console.Clear();
            Console.WriteLine("===== BATTLE RESULT =====\n");

            var result = DetermineBattleResult();
            Console.WriteLine(result);

            Console.WriteLine("\n=========================");
        }

        /// <summary>
        /// Adds the latest action to the queue of the last four actions performed in the battle.
        /// </summary>
        /// <param name="action">The action to be added.</param>
        private void AddToLastTwoActions(string action)
        {
            if (_lastFourActions.Count >= 4)
            {
                _lastFourActions.Dequeue(); // Remove the oldest action if there are already four
            }

            _lastFourActions.Enqueue(action); // Add the new action
        }

        /// <summary>
        /// Clears the list of the last two actions performed in the battle.
        /// </summary>
        private void ClearLastTwoActions()
        {
            _lastFourActions.Clear();
        }
    }
}
