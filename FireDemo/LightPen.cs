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

        public LightPen() : this(fill: 1.0f, min: 0, max: 255, useFullRange: false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fill">Percent density of pixels that actually render when drawing. Range is 0.0 to 1.0 inclusive.</param>
        /// <param name="min">Minimum light intensity. Range is 0 to 255 inclusive. MUST be less-or-equal to <paramref name="max"/></param>
        /// <param name="max">Maximum light intensity. Range is 0 to 255 inclusive. MUST be greater-or-equal to <paramref name="min"/>.</param>
        /// <param name="useFullRange">If <c>true</c> then any value between <paramref name="min"/>/<paramref name="max"/> may be used when drawing. If <c>false</c> then only the <paramref name="min"/> and <paramref name="max"/> values may be used, nothing in between.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public LightPen(float fill, int min, int max, bool useFullRange)
        {
            if (fill < 0 || fill > 1)
                throw new ArgumentOutOfRangeException("Density percent must be between 0 and 100 inclusive.");
            if (min > max)
                throw new ArgumentOutOfRangeException("Min value must not be larger than max value.");
            if (min < 0 || min > 255)
                throw new ArgumentOutOfRangeException("Min value must be between 0 and 255 inclusive.");
            if (min > max)
                throw new ArgumentOutOfRangeException("Max value must be between 0 and 255 inclusive.");

            rng = new Random();
            percentFill = fill;
            minIntensity = min;
            maxIntensity = max;
            this.useFullRange = useFullRange;
        }

        /// <summary>
        /// return a random value between the min and max intensity
        /// </summary>
        /// <returns></returns>
        public int NextValue()
        {
            if (useFullRange)
                return rng.Next(minIntensity, maxIntensity + 1);
            else if (rng.NextDouble() < 0.5)
                return minIntensity;
            else
                return maxIntensity;
        }

        /// <summary>
        /// return a boolean value indicating whether or not a new value should be drawn
        /// </summary>
        /// <returns></returns>
        public bool FShouldDrawNext()
        {
            return rng.NextDouble() < percentFill;
        }
    }
}