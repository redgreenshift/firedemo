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
        /// <summary>
        /// Range of 0.0 to 1.0 for probability a given pixel will be drawn on average
        /// </summary>
        readonly float percentFill;

        /// <summary>
        /// Range of 0 to 255 for the minimum value to draw
        /// </summary>
        readonly int minIntensity;

        /// <summary>
        /// Range of 0 to 255 for the maximum value to draw
        /// </summary>
        readonly int maxIntensity;

        /// <summary>
        ///  True if any value between min and max are allowed.
        ///  False if ONLY the min and max values should be used.
        /// </summary>
        readonly bool useFullRange;

        public LightPen() : this(fill: 1.0f, min: 0, max: 255, bUseFullRange: false)
        {
        }

        public LightPen(float fill, int min, int max, bool bUseFullRange)
        {
            rng = new Random();
            percentFill = fill;
            minIntensity = min;
            maxIntensity = max;
            useFullRange = bUseFullRange;
        }

        /// <summary>
        /// return a random value between the min and max intensity
        /// </summary>
        /// <returns></returns>
        public int NextValue()
        {
            if (useFullRange)
                return rng.Next(minIntensity, maxIntensity);
            else if (rng.NextDouble() < 0.5)
                return minIntensity;
            else
                return maxIntensity;
        }

        /// <summary>
        /// return a boolean value indicating whether or not a new value shoudl be drawn
        /// </summary>
        /// <returns></returns>
        public bool FShouldDrawNext()
        {
            return rng.NextDouble() < percentFill;
        }
    }
}