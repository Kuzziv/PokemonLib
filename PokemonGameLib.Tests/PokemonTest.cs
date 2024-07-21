using Xunit;

namespace PokemonGameLib.Tests
{
    public class PokemonTests
    {
        [Fact]
        public void TestPokemonInitialization()
        {
            var pokemon = new Pokemon("Pikachu", "Electric", 100, 55, 40);

            Assert.Equal("Pikachu", pokemon.Name);
            Assert.Equal("Electric", pokemon.Type);
            Assert.Equal(100, pokemon.HP);
            Assert.Equal(55, pokemon.Attack);
            Assert.Equal(40, pokemon.Defense);
        }

        
        
    }
}
