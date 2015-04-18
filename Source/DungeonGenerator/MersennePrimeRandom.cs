using System;

namespace Dungeon.Generator
{
    /// <summary>
    /// Implementation of Mersenne Twister random number generator
    /// </summary>
    internal class MersennePrimeRandom
    {
        private readonly uint[] _matrix = new uint[624];
        private int _index;

        public MersennePrimeRandom() : this((uint)(0xFFFFFFFF & DateTime.Now.Ticks)) { }

        /// <summary>
        /// Initializes a new instance of the MersennePrimeRNG with a seed
        /// </summary>
        /// <param name="seed"></param>
        public MersennePrimeRandom(uint seed)
        {
            _matrix[0] = seed;
            for (int i = 1; i < _matrix.Length; i++)
                _matrix[i] = (1812433253 * (_matrix[i - 1] ^ ((_matrix[i - 1]) >> 30) + 1));
        }

        /// <summary>
        /// Generates a new matrix table
        /// </summary>
        private void Generate()
        {
            for (int i = 0; i < _matrix.Length; i++)
            {
                uint y = (_matrix[i] >> 31) + ((_matrix[(i + 1) & 623]) << 1);
                _matrix[i] = _matrix[(i + 397) & 623] ^ (y >> 1);
                if (y % 2 != 0)
                    _matrix[i] = (_matrix[i] ^ (2567483615));
            }
        }

        /// <summary>
        /// Generates and returns a random number
        /// </summary>
        /// <returns></returns>
        public int Next()
        {
            if (_index == 0)
                Generate();

            uint y = _matrix[_index];
            y = y ^ (y >> 11);
            y = (y ^ (y << 7) & (2636928640));
            y = (y ^ (y << 15) & (4022730752));
            y = (y ^ (y >> 18));

            _index = (_index + 1) % 623;
            return (int)(y % int.MaxValue);
        }

        /// <summary>
        /// Generates and returns a random number
        /// </summary>
        /// <param name="max">The highest value that can be returned</param>
        /// <returns></returns>
        public int Next(int max)
        {
            var randomValue = Next();
            return randomValue % max;
        }

        /// <summary>
        /// Generates and returns a random number
        /// </summary>
        /// <param name="min">The lowest value returned</param>
        /// <param name="max">The highest value returned</param>
        /// <returns></returns>
        public int Next(int min, int max)
        {
            if (min == max)
                return max;
            if (min > max)
                throw new ArgumentException("min cannot be greater than max", "min");
            return min + Next(min - max);
        }

        public bool Chance(int percent)
        {
            return Chance(percent/100.0f);
        }

        public bool Chance(float percent)
        {
            return Next(100)/100.0f <= percent;
        }
    }
}