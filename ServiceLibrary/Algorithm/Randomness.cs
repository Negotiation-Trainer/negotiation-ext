
using System;

namespace ServiceLibrary.Algorithm
{
    public class Randomness
    {
        private readonly Random _random;

        public Randomness(Random random)
        {
            _random = random;
        }

        /// <summary>
        /// Calculates if a random event should happen based on the chance of it happening.
        /// </summary>
        /// <param name="changeChance">The change of randomizing the event</param>
        /// <returns>A bool to decide if the random event happened</returns>
        public bool Calculate(float changeChance)
        {
            return (_random.NextDouble() < changeChance);
        }

        /// <summary>
        /// Calculates a random amount between the minimum and maximum value.
        /// </summary>
        /// <param name="minimum">The minimum allowed value</param>
        /// <param name="maximum">The maximum allowed value</param>
        /// <returns>Returns the chosen value</returns>
        public int CalculateAmount(int minimum, int maximum)
        {
            return _random.Next(minimum, maximum);
        }
    }
}