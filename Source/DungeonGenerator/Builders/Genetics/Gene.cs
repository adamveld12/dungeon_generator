namespace DungeonGenerator.Builders.Genetics
{
    public struct Gene
    {
        private readonly int _generation;

        public Gene(int generation) : this()
        {
            _generation = generation;
            Lifespan = 32;
            MaxOffspring = 3;
            TurnChance = 0;
            ReproductionChance = 10;
        }

        public int Generation
        {
            get { return _generation; }
        }

        /// <summary>
        /// The number of steps the builder can take before dying
        /// </summary>
        public int Lifespan { get; set; }
        public int MaxOffspring { get; set; }
        public int TurnChance { get; set; }
        public int ReproductionChance { get; set; }
    }
}