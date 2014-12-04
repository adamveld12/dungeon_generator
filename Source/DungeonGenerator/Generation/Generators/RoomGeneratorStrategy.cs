using Dungeon.Generator.Navigation;

namespace Dungeon.Generator.Generation.Generators
{
    public class RoomGeneratorStrategy : IDungeonGenerationStrategy
    {
        private readonly MersennePrimeRandom _random;
        private readonly ITileMap _map;

        public RoomGeneratorStrategy(MersennePrimeRandom random, ITileMap map)
        {
            _random = random;
            _map = map;
        }

        public void Execute()
        {

        }
    }
}