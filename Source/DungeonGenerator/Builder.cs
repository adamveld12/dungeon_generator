using System.Linq;

namespace DungeonGenerator
{
    public class Builder
    {
        private readonly Gene _gene;
        private int _stepsRemaining;
        private int _x = 0;
        private int _y = 0;
        private Direction _direction;

        public Builder(int x, int y, Gene gene)
        {
            _direction = Direction.N;
            _x = x;
            _y = y;
            _gene = gene;
            _stepsRemaining = gene.Lifespan;
        }

        public Builder(int x, int y) : this(x, y, new Gene(0, 15, 3))
        {
            
        }

        public void Step(Dungeon dungeon)
        {
            if (!IsDead)
            {
                OnStep(dungeon);

                _stepsRemaining--;
            }
        }

        public virtual void OnStep(Dungeon dungeon)
        {
            
        }

        public Builder[] Reproduce(int geneSeed)
        {
            var reproductionCount = geneSeed % 3;
            return Enumerable.Repeat(0, reproductionCount)
                             .Select(x => _gene.Mutate())
                             .Select(gene => new Builder(_x, _y, gene))
                             .ToArray();
        }

        public bool IsDead
        {
            get { return _stepsRemaining <= 0; }
        }

    }
}