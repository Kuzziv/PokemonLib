using System;
using System.Linq;
using System.Collections.Generic;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;
using PokemonGameLib.Models.Pokemons;

namespace PokemonGameLib.Models.Battles
{
    public class Battle : Loggable, IBattle
    {
        public ITrainer CurrentTrainer { get; private set; }
        public ITrainer OpponentTrainer => CurrentTrainer == _trainer1 ? _trainer2 : _trainer1;

        private readonly ITrainer _trainer1;
        private readonly ITrainer _trainer2;
        private readonly BattleCalculator _battleCalculator;

        private Queue<string> _lastFourActions;

        public Battle(ITrainer trainer1, ITrainer trainer2)
        {
            _trainer1 = trainer1;
            _trainer2 = trainer2;
            CurrentTrainer = _trainer1;
            _battleCalculator = new BattleCalculator();
            _lastFourActions = new Queue<string>();
        }

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
                HandleFaintedPokemon(OpponentTrainer);
            }
        }

        public void PerformSwitch(IPokemon newPokemon)
        {
            string action = $"{CurrentTrainer.Name} switched {CurrentTrainer.CurrentPokemon.Name} out for {newPokemon.Name}.";
            AddToLastTwoActions(action);

            LogInfo(action);

            BattleValidator.ValidatePokemonSwitch(CurrentTrainer, newPokemon);
            CurrentTrainer.CurrentPokemon = newPokemon;
        }

        public void PerformUseItem(IItem item, IPokemon targetPokemon)
        {
            string action = $"{CurrentTrainer.Name} used {item.Name} on {targetPokemon.Name}. and {item.Description}";
            AddToLastTwoActions(action);

            LogInfo(action);

            item.Use(CurrentTrainer, targetPokemon);
        }

        public void HandleFaintedPokemon(ITrainer trainer)
        {
            string faintedAction = $"{trainer.CurrentPokemon.Name} has fainted!";
            AddToLastTwoActions(faintedAction);

            LogInfo(faintedAction);
            Console.WriteLine($"{trainer.CurrentPokemon.Name} has fainted!\n");

            var newPokemon = trainer.Pokemons.FirstOrDefault(p => !p.IsFainted());
            if (newPokemon != null)
            {
                trainer.CurrentPokemon = newPokemon;
                string newPokemonAction = $"{trainer.Name} sent out {newPokemon.Name}!";
                AddToLastTwoActions(newPokemonAction);

                LogInfo(newPokemonAction);
                Console.WriteLine($"{trainer.Name} sent out {newPokemon.Name}!\n");
            }
            else
            {
                LogError($"{trainer.Name} has no Pokémon left!");
                Console.WriteLine($"{trainer.Name} has no Pokémon left!\n");
            }
        }

        public void SwitchTurns()
        {
            CurrentTrainer = OpponentTrainer;
            LogInfo($"It is now {CurrentTrainer.Name}'s turn.");
        }

        public bool IsBattleOver()
        {
            return _trainer1.Pokemons.All(p => p.IsFainted()) || _trainer2.Pokemons.All(p => p.IsFainted());
        }

        public ITrainer GetWinner()
        {
            if (_trainer1.Pokemons.All(p => p.IsFainted())) return _trainer2;
            if (_trainer2.Pokemons.All(p => p.IsFainted())) return _trainer1;
            return null;
        }

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

        private void PrintPokemonStatus(IPokemon pokemon)
        {
            Console.WriteLine($"- {pokemon.Name} (HP: {pokemon.CurrentHP}/{pokemon.MaxHP})");
            if (pokemon.Status != StatusCondition.None)
            {
                Console.WriteLine($"  Status: {pokemon.Status}");
            }
        }

        public void PrintBattleResult()
        {
            Console.Clear();
            Console.WriteLine("===== BATTLE RESULT =====\n");

            var result = DetermineBattleResult();
            Console.WriteLine(result);

            Console.WriteLine("\n=========================");
        }

        private void AddToLastTwoActions(string action)
        {
            if (_lastFourActions.Count >= 4)
            {
                _lastFourActions.Dequeue(); // Remove the oldest action if there are already two
            }

            _lastFourActions.Enqueue(action); // Add the new action
        }

        private void ClearLastTwoActions()
        {
            _lastFourActions.Clear();
        }
    }
}
