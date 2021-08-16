using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    interface ILightPen
    {
        int NextValue();
        bool FShouldDrawNext();
    }
    class LightPen : ILightPen
    {
        readonly Random rng;
        readonly float percentFill;
        readonly int minIntensity;
        readonly int maxIntensity;
        readonly bool useFullRange;

        public LightPen()
        {
            rng = new Random();
            percentFill = 1.0f;
            minIntensity = 0;
            maxIntensity = 255;
            useFullRange = false;
        }

        public int NextValue()
        {
            // "return a random value between the min and max intensity"

            if (useFullRange)
                return rng.Next(minIntensity, maxIntensity);
            else if (rng.NextDouble() < 0.5)
                return minIntensity;
            else
                return maxIntensity;
        }

        public bool FShouldDrawNext()
        {
            // "return a boolean value indicating whether or not a new value shoudl be drawn"

            return rng.NextDouble() < percentFill;
        }
    }
}