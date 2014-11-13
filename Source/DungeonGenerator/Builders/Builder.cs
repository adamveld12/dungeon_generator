using System;
using System.Collections.Generic;
using DungeonGenerator.Builders.Genetics;
using DungeonGenerator.Navigation;

namespace DungeonGenerator.Builders
{
    public class Builder
    {
        #region Fields

        private readonly Gene _gene;
        private readonly Location _location;

        private int _currentStepSeed;
        private int _stepsRemaining;

        #endregion

        public Builder(int x, int y, Gene gene = default(Gene))
        {
            _location = new Location
            {
                X = x,
                Y = y
            };
            _gene = gene.Generation == 0 ? new Gene(1) : gene;

            IsDead = false;

            _stepsRemaining = _gene.Lifespan;
        }

        public void Step(Dungeon dungeon, int stepCount, int stepSeed)
        {
            if (!IsDead)
            {
                _currentStepSeed = stepSeed;
                OnStep(dungeon, stepCount, stepSeed);

                _stepsRemaining--;
                IsDead = _stepsRemaining <= 0;
            }
        }

        #region Overrides/API

        protected void TakeRandomTurn(int? seed = null)
        {
            if((seed ?? _currentStepSeed) % 3 >= 1)
              _location.TurnLeft();
            else
              _location.TurnRight();

            
        }

        protected void Carve(ITileMap map, int width, int height, TileType type)
        {
            map[_location.X, _location.Y] = type;
            // swap these around because we're facing a different axis
            if (_location.Direction == Direction.E || _location.Direction == Direction.W)
            {
                var temp = height;
                height = width;
                width = temp;
            }

            for (int x = _location.X; x < _location.X + width; x++)
                for (int y = _location.Y; y < _location.Y + height; y++)
                    map[x, y] = type;
        }

        protected virtual void OnStep(ITileMap map, int stepCount, int stepSeed)
        {
            Carve(map, 3, 1, TileType.Floor);

            var chance = stepSeed % 100;

            // chance to turn
            if (chance < _gene.TurnChance)
                TakeRandomTurn();

            while(!_location.CanWalk(map, 3))
                TakeRandomTurn();

            _location.Walk();
        }

        protected virtual Gene Mutate(int seed)
        {
            var lifespanDelta = _gene.Lifespan - (seed% _gene.Lifespan);

            return new Gene(_gene.Generation + 1)
            {
                Lifespan = lifespanDelta,
                ReproductionChance = _gene.ReproductionChance - _gene.Generation,
                MaxOffspring =  Math.Max(1, _gene.MaxOffspring - 1),
                TurnChance = 10
            };
        }

        protected virtual IEnumerable<Builder> Spawn(Gene mutatedGene, int seed)
        {
            var builderRight =  new CorridorBuilder(Location.X, Location.Y+1, mutatedGene);
            builderRight.Location.Direction = Location.Direction;
            builderRight.Location.TurnRight();

            var builderLeft = new CorridorBuilder(Location.X, Location.Y, mutatedGene);
            builderLeft.Location.Direction = Location.Direction;
            builderLeft.Location.TurnLeft();

            yield return builderRight;
            yield return builderLeft;
        }

        #endregion


        public IEnumerable<Builder> Reproduce(int seed)
        {
            var chance = seed % 100;
            var shouldReproduce = chance <= _gene.ReproductionChance && _gene.MaxOffspring > 0 && _gene.Lifespan > 5;

            if (shouldReproduce)
            {
                var gene = Mutate(seed);
                var builders = Spawn(gene, seed);
                foreach (var builder in builders)
                    yield return builder;
            }
            yield return this;
        }

        #region Properties

        public bool IsDead { get; set; }

        public Location Location
        {
            get { return _location; }
        }

        public Gene Genetics
        {
            get { return _gene; }
        }

        #endregion
    }
}