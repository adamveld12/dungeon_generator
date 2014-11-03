using System;

namespace DungeonGenerator
{
    public class Gene
    {
        private readonly int _lifespan;
        private readonly int _maxOffspring;
        private readonly int _generation;

        public Gene(int generation, int lifespan, int maxOffspring)
        {
            _lifespan = lifespan;
            _maxOffspring = maxOffspring;
            _generation = generation;
        }


        /// <summary>
        /// The number of steps the builder can take before dying
        /// </summary>
        public int Lifespan
        {
            get { return _lifespan; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxOffspring
        {
            get { return _maxOffspring; }
        }

        public int Generation
        {
            get { return _generation; }
        }

        /// <summary>
        /// Mutates this gene, returning the new mutated version
        /// </summary>
        /// <returns></returns>
        public Gene Mutate()
        {
            return new Gene(_generation + 1, _lifespan, Math.Max(_maxOffspring - 1, 0));
        }

    }
}