using PokemonGameLib.Interfaces;

namespace PokemonGameLib.Interfaces
{
    /// <summary>
    /// Represents a trainer in the Pokémon game, managing a team of Pokémon and facilitating interactions during battles.
    /// </summary>
    public interface ITrainer
    {
        /// <summary>
        /// Gets the name of the Trainer.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the current Pokémon of the Trainer.
        /// </summary>
        IPokemon CurrentPokemon { get; set; }

        /// <summary>
        /// Gets the list of Pokemons owned by the Trainer.
        /// </summary>
        IList<IPokemon> Pokemons { get; }

        /// <summary>
        /// Adds a Pokémon to the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokémon to be added.</param>
        void AddPokemon(IPokemon pokemon);

        /// <summary>
        /// Removes a Pokémon from the Trainer's list of Pokemons.
        /// </summary>
        /// <param name="pokemon">The Pokémon to remove.</param>
        void RemovePokemon(IPokemon pokemon);

        /// <summary>
        /// Switches the Trainer's current Pokémon to the specified new Pokémon.
        /// </summary>
        /// <param name="newPokemon">The new Pokémon to switch to.</param>
        void SwitchPokemon(IPokemon newPokemon);

        /// <summary>
        /// Uses an item on the specified Pokémon.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <param name="target">The Pokémon to use the item on.</param>
        void UseItem(IItem item, IPokemon target);

        /// <summary>
        /// Takes the Trainer's turn during a battle.
        /// </summary>
        /// <param name="battle">The current battle instance.</param>
        void TakeTurn(IBattle battle);
    }
}
