using System.Collections.Generic;
using DungeonGenerator.Builders.Genetics;

namespace DungeonGenerator.Builders
{
    public class CorridorBuilder : Builder
    {
        public CorridorBuilder(int x, int y, Gene gene = new Gene()) 
            : base(x, y, gene)
        { }

        protected override void OnStep(ITileMap map, int stepCount, int stepSeed)
        {
            Carve(map, 1, 1, TileType.Floor);

            var chance = stepSeed % 100;
            var turnChance = Genetics.TurnChance;

            var tile = Location.LookAhead(map);

            // increase chance to randomly turn if there is a floor already 
            // carved out infront of us
            if (tile == TileType.Floor)
                turnChance += (int)(turnChance * 0.3);

            if (chance < turnChance)
                TakeRandomTurn();

            while(!Location.CanWalk(map, 1))
                TakeRandomTurn();

            Location.Walk();

        }

        protected override IEnumerable<Builder> Spawn(Gene mutatedGene, int seed)
        {

            yield return new RoomBuilder(Location.X, Location.Y, mutatedGene);
        }

        protected override Gene Mutate(int seed)
        {
            var gene = base.Mutate(seed);

            gene.ReproductionChance = gene.ReproductionChance - gene.Generation;
            gene.TurnChance = 33;

            return gene;
        }
    }
}
