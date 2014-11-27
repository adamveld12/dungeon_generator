using DungeonGenerator.Navigation;

namespace DungeonGenerator.Generation.Generators
{
    public interface IDungeonGenerationStrategy
    {
        void Execute(ITileMap map);
    }


    public class PathwayFirstGeneratorStrategy : IDungeonGenerationStrategy
    {
        public void Execute(ITileMap map)
        {
            
        }
    }
    
}