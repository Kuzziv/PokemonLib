using System;
using System.Linq;
using PokemonGameLib.Interfaces;
using PokemonGameLib.Utilities;
using PokemonGameLib.Services;

namespace PokemonGameLib.Models.Battles
{
    public class Battle : Loggable, IBattle
    {
        public ITrainer CurrentTrainer { get; private set; }
        public ITrainer OpponentTrainer => CurrentTrainer == _trainer1 ? _trainer2 : _trainer1;

        private readonly ITrainer _trainer1;
        private readonly ITrainer _trainer2;
        private readonly BattleCalculator _battleCalculator;

        public Battle(ITrainer trainer1, ITrainer trainer2)
        {
            _trainer1 = trainer1;
            _trainer2 = trainer2;
            CurrentTrainer = _trainer1;
            _battleCalculator = new BattleCalculator();
        }

        public void StartBattle()
        {
            LogInfo("The battle has started!");

            while (!IsBattleOver())
            {
                PrintBattle();
                
                Console.WriteLine($"{CurrentTrainer.Name}'s turn.");

                CurrentTrainer.TakeTurn(this);

                if (!IsBattleOver())
                {
                    SwitchTurns();
                }
            }

            ITrainer winner = GetWinner();
            if (winner != null)
            {
                LogInfo($"{winner.Name} wins the battle!");
                Console.WriteLine($"{winner.Name} wins the battle!");
            }
            else
            {
                LogInfo("The battle ended in a draw!");
                Console.WriteLine("The battle ended in a draw!");
            }
        }

        public void PerformAttack(IMove move)
        {
            LogInfo($"{CurrentTrainer.Name} is attacking with {CurrentTrainer.CurrentPokemon.Name} using {move.Name}.");

            var attacker = CurrentTrainer.CurrentPokemon;
            var defender = OpponentTrainer.CurrentPokemon;

            BattleValidator.ValidateMove(attacker, move);

            int damage = _battleCalculator.CalculateDamage(attacker, defender, move);
            defender.TakeDamage(damage);

            LogInfo($"{defender.Name} took {damage} damage. HP is now {defender.CurrentHP}/{defender.MaxHP}.");

            if (defender.CurrentHP <= 0)
            {
                HandleFaintedPokemon(OpponentTrainer);
            }
        }

        public void PerformSwitch(IPokemon newPokemon)
        {
            LogInfo($"{CurrentTrainer.Name} is switching Pokémon from {CurrentTrainer.CurrentPokemon.Name} to {newPokemon.Name}.");

            BattleValidator.ValidatePokemonSwitch(CurrentTrainer, newPokemon);
            CurrentTrainer.CurrentPokemon = newPokemon;
        }

        public void PerformUseItem(IItem item, IPokemon targetPokemon)
        {
            LogInfo($"{CurrentTrainer.Name} is using {item.Name} on {targetPokemon.Name}.");
            item.Use(CurrentTrainer, targetPokemon);
        }

        public void HandleFaintedPokemon(ITrainer trainer)
        {
            LogInfo($"{trainer.CurrentPokemon.Name} has fainted!");

            var newPokemon = trainer.Pokemons.FirstOrDefault(p => !p.IsFainted());
            if (newPokemon != null)
            {
                trainer.CurrentPokemon = newPokemon;
                LogInfo($"{trainer.Name} sent out {newPokemon.Name}!");
            }
            else
            {
                LogError($"{trainer.Name} has no Pokémon left!");
                Console.WriteLine($"{trainer.Name} has no Pokémon left!");
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

        public void PrintBattle()
        {
            Console.WriteLine("Battle status:");
            Console.WriteLine($"{_trainer1.Name}: {(CurrentTrainer == _trainer1 ? "(current)" : "")}");
            Console.WriteLine($"{_trainer1.CurrentPokemon.Name} HP: {_trainer1.CurrentPokemon.CurrentHP}/{_trainer1.CurrentPokemon.MaxHP}");

            Console.WriteLine($"{_trainer2.Name}: {(CurrentTrainer == _trainer2 ? "(current)" : "")}");
            Console.WriteLine($"{_trainer2.CurrentPokemon.Name} HP: {_trainer2.CurrentPokemon.CurrentHP}/{_trainer2.CurrentPokemon.MaxHP}");

            Console.WriteLine("----");
            Console.WriteLine($"{_trainer1.CurrentPokemon.Name} (controlled by {_trainer1.Name}) vs. {_trainer2.CurrentPokemon.Name} (controlled by {_trainer2.Name})");
        }
    }
}
