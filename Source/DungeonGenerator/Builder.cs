using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DungeonGenerator
{
    public class Builder
    {
        private readonly Gene _gene;
        private int _stepsRemaining;
        private int _totalSteps;
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
            _totalSteps = 0;
        }

        public Builder(int x, int y) 
            : this(x, y, new Gene(0, 15, 3))
        {
            
        }

        public void Step(Dungeon dungeon, int stepCount, int stepSeed)
        {
            if (!IsDead)
            {
                OnStep(dungeon, stepCount, stepSeed);
                _stepsRemaining--;
                _totalSteps++;
            }
        }

        #region Overrides/API

        protected virtual void OnStep(Dungeon dungeon, int stepCount, int stepSeed)
        {
            dungeon[_x, _y] = TileType.Floor;

            var chance = stepSeed % 100;
            if (chance <= 5)
                _stepsRemaining++;

            if (chance <= 18)
                if(stepSeed%7 > 3)
                  TurnLeft();
                else
                  TurnRight();

            Walk(dungeon);
        }

        protected void SetDirection(Direction direction)
        {
            _direction = direction;
        }

        protected void TurnLeft()
        {
            var directionVal = (int)_direction + 1;
            _direction = (Direction)(directionVal % (int)Direction.Max);
        }

        protected void TurnRight()
        {
            var directionVal = (int)_direction - 1;
            _direction = (Direction)Math.Max(directionVal,  (int)Direction.Min);
        }

        protected void Walk(Dungeon dungeon)
        {
            if (CanWalk(dungeon))
            {
              switch (_direction)
              {
                  case Direction.N:
                      _y ++;
                      break;
                  case Direction.E:
                      _x ++;
                      break;
                  case Direction.W:
                      _x--;
                      break;
                  case Direction.S:
                      _y--;
                      break;
                  default:
                      throw new ArgumentOutOfRangeException();
              }
            }
        }

        protected bool CanWalk(Dungeon dungeon)
        {
            switch (_direction)
            {
                case Direction.N:
                    return (_y + 1) != (dungeon.Height - 1);
                case Direction.E:
                    return (_x + 1) != (dungeon.Width - 1);
                case Direction.W:
                    return (_x - 1) != (0);
                case Direction.S:
                    return (_y - 1) != (0);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        #endregion

        public Builder[] Reproduce(int geneSeed)
        {
            var reproductionCount = geneSeed % 3;
            return Enumerable.Repeat(0, reproductionCount)
                             .Select(x => _gene.Mutate())
                             .Select((gene, index) => {
                                 var builder = new Builder(_x, _y, gene);
                                 builder.SetDirection(_direction);
                                 if(index % 2== 0)
                                   builder.TurnLeft();
                                 else
                                     builder.TurnRight();
                                 return builder;
                             })
                             .ToArray();
        }

        public bool IsDead
        {
            get { return _stepsRemaining <= 0; }
        }
    }
}