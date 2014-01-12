using System;

namespace NervDog.Utilities
{
    public static class RandomHelper
    {
        private static uint _seed = (uint) DateTime.Now.Ticks;
        private static readonly Random _rand = new Random();

        public static float NextFloat(float min, float max)
        {
            _seed = 214013*_seed + 2531011;
            return min + (_seed >> 16)*(1.0f/65535.0f)*(max - min);
        }

        public static int NextInt(int min, int max)
        {
            return _rand.Next(min, max);
        }
    }
}