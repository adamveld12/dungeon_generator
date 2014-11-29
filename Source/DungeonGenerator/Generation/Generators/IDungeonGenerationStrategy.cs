using DungeonGenerator.Navigation;

namespace DungeonGenerator.Generation.Generators
{
    public interface IDungeonGenerationStrategy
    {
        void Execute(MersennePrimeRandom random, ITileMap map);
    }


    public class PathwayFirstGeneratorStrategy : IDungeonGenerationStrategy
    {
        public void Execute(MersennePrimeRandom random, ITileMap map)
        {
            
        }
    }
    
}