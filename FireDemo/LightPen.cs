using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    /// <summary>
    /// Defines the interface for a light pen that generates random intensities
    /// and governs pixel visibility for shaped radiation.
    /// </summary>
    interface ILightPen
    {
        /// <summary>
        /// Attempts to emit a random intensity value based on the current fill rate.
        /// </summary>
        /// <param name="intensity">The output for the emitted light value.</param>
        /// <returns><c>true</c> if an emission occurred and <paramref name="intensity"/> is updated;<br/>
        /// <c>false</c> if the caller should skip drawing the next pixel (and do not use <paramref name="intensity"/>).</returns>
        bool TryEmit(out int intensity);
    }

    /// <summary>
    /// An adaptive-rate light pen that generates random intensities and
    /// governs pixel visibility for shaped radiation. A variable fill rate
    /// and intensity range can be specified to control the density and brightness
    /// of the light pen's output.
    /// </summary>
    class LightPen : ILightPen
    {
        readonly Random rng;
        /// <summary>
        /// The target fill rate (probability) for emitting a value; [0.0 to 1.0] inclusive.
        /// </summary>
        readonly float percentFill;

        /// <summary>
        /// Lower-bound intensity value for draws; [0..255] inclusive.
        /// </summary>
        readonly int minIntensity;

        /// <summary>
        /// Upper-bound intensity value for draws; [0..255] inclusive.
        /// </summary>
        readonly int maxIntensity;

        /// <summary>
        /// True if any value between the <paramref name="minIntensity"/> and <paramref name="maxIntensity"/> is allowed; otherwise, only use the boundaries.
        /// </summary>
        readonly bool useFullRange;

        /// <inheritdoc cref="LightPen"/>
        public LightPen() : this(fill: 1.0f, min: 0, max: 255, useFullRange: false)
        {
        }

        /// <inheritdoc cref="LightPen"/>
        /// <param name="fill"><inheritdoc cref="LightPen.percentFill" path="/summary"/></param>
        /// <param name="min"><inheritdoc cref="LightPen.minIntensity" path="/summary"/>MUST be less-or-equal to <paramref name="max"/></param>
        /// <param name="max"><inheritdoc cref="LightPen.maxIntensity" path="/summary"/>MUST be greater-or-equal to <paramref name="min"/>.</param>
        /// <param name="useFullRange"><inheritdoc cref="LightPen.useFullRange" path="/summary"/></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public LightPen(float fill, int min, int max, bool useFullRange)
        {
            if (fill < 0 || fill > 1)
                throw new ArgumentOutOfRangeException("Density percent must be between 0.0 and 1.0 inclusive.");
            if (min > max)
                throw new ArgumentOutOfRangeException("Min value must not be larger than max value.");
            if (min < 0 || min > 255)
                throw new ArgumentOutOfRangeException("Min value must be between 0 and 255 inclusive.");
            if (min > max)
                throw new ArgumentOutOfRangeException("Max value must be between 0 and 255 inclusive.");

            rng = Util.NewRandom();
            percentFill = fill;
            minIntensity = min;
            maxIntensity = max;
            this.useFullRange = useFullRange;
        }

        /// <summary>
        /// Generates a random intensity value within the defined range; used as the data for one successful draw.
        /// </summary>
        /// <remarks>
        /// This should typically be called only once after each successful call to <see cref="FShouldDrawNext"/>.
        /// </remarks>
        /// <returns>The generated intensity value between <see cref="minIntensity"/> and <see cref="maxIntensity"/>.</returns>
        private int NextValue()
        {
            if (useFullRange)
                return rng.Next(minIntensity, maxIntensity + 1);
            else if (rng.NextDouble() < 0.5)
                return minIntensity;
            else
                return maxIntensity;
        }

        /// <summary>
        /// Returns true if an emission should occur based on the current fill rate.
        /// </summary>
        /// <remarks>
        /// Caller must call this before each call to <see cref="NextValue"/> to determine if the next value should be drawn or skipped.
        /// </remarks>
        /// <returns><c>true</c> if a new value should be emitted; <c>false</c> otherwise.</returns>
        private bool FShouldDrawNext()
        {
            return rng.NextDouble() < percentFill;
        }

        /// <inheritdoc cref="ILightPen.TryEmit"/>
        public bool TryEmit(out int intensity)
        {
            if (FShouldDrawNext())
            {
                intensity = NextValue();
                return true;
            }

            intensity = default;
            return false;
        }
    }
}