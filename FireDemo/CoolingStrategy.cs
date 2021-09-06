using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    interface ICoolingStrategy
    {
        int at(int x, int y);
        void ProgressOneFrame();
    }

    class CoolingStrategyConst : ICoolingStrategy
    {
        private readonly int coolingFactor;
        public CoolingStrategyConst(int value)
        {
            this.coolingFactor = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int at(int x, int y)
        {
            return coolingFactor;
        }

        public void ProgressOneFrame() { /* do nothing*/ }
    };

    public class CoolingStrategyMap : ICoolingStrategy
    {
        // map random width height rotate shift density min max smoothing iCoolingOffset iFrame
        /// <summary>
        /// the map used for the current frame (always the truth)
        /// </summary>
        int[] coolingMap = null;

        /// <summary>
        /// original UNSMOOTHED rotating cooling map, so that the previous map can be smoothed into the next rotation
        /// (calculation scratch space to generate the values to fill into coolingMap)
        /// </summary>
        int[] originalCoolingMap;

        /// <summary>
        /// the smoothed version
        /// (calculation scratch space to generate the values to fill into coolingMap)
        /// </summary>
        int[] rotatingCoolingMap;

        readonly Random rng;
        int iCoolingOffset = 0;
        int iFrame = 0;

        int width;
        int height;

        /// <summary>
        /// Percent of pixels that should be filled with a cooling value. Range is 0.0 to 1.0 inclusive.
        /// </summary>
        float density;

        /// <summary>
        /// Minimum cooling intensity. Range is 0 to 255 inclusive. MUST be less-or-equal to <paramref name="maxValue"/>.
        /// </summary>
        int minValue;

        /// <summary>
        /// Maximum cooling intensity. Range is 0 to 255 inclusive. MUST be greater-or-equal to <paramref name="minValue"/>.
        /// </summary>
        int maxValue;

        /// <summary>
        /// Number of times to smooth out the values to produce a produce a more even distribution.
        /// </summary>
        int smoothing;

        /// <summary>
        /// If TRUE, move the map up one row per frame to give the appearance of rising air currents.
        /// </summary>
        bool shift = false;

        /// <summary>
        /// If TRUE, periodically generate an entirely new map, so the flame doesn't look like a video on repeat.
        /// </summary>
        bool rotate = false;


        public CoolingStrategyMap() : this(Util.NewRandom())
        {
        }

        public CoolingStrategyMap(Random rng)
        {
            this.rng = rng;
        }

        public int at(int x, int y)
        {
            int i = (y * width + x + iCoolingOffset) % (width * height);
            // Uncomment to help debug the cooling map shift
            //if (i == 10)
            //    return -1;
            return coolingMap[i];
        }
           
        // TODO: JRDV: I think at() is expensive, so maybe providing a direct atRaw would be faster? Measure and find out.
        //public int atRaw(int iKnowWhatImDoing)
        //{
        //    // Uncomment to help debug the cooling map shift
        //    //if (iKnowWhatImDoing == 10)
        //    //    return -1;
        //    return coolingMap[iKnowWhatImDoing];
        //}

        public void ProgressOneFrame()
        {
            if (this.shift)
            {
                ++iFrame;
                iCoolingOffset = (width * iFrame) % (width * height);

                if (this.rotate && iFrame > height)
                {
                    UpdateRotatingCoolingMap();
                }
            }
            else
            {
                iFrame = 0;
                iCoolingOffset = 0;
            }
        }

        /// <summary>
        /// Probably should rename this, but creating a new "initialize" method to allow changing the values on the fly
        /// </summary>
        /// <param name="width">The width, in pixels, of the new cooling map.</param>
        /// <param name="height">The height, in pixels, of the new cooling map.</param>
        /// <param name="density">Percent of pixels that should be filled with a cooling value. Range is 0.0 to 1.0 inclusive.</param>
        /// <param name="min">Minimum cooling intensity. Range is 0 to 255 inclusive. MUST be less-or-equal to <paramref name="max"/>.</param>
        /// <param name="max">Maximum cooling intensity. Range is 0 to 255 inclusive. MUST be greater-or-equal to <paramref name="min"/>.</param>
        /// <param name="smoothing">Number of times to smooth out the values to produce a produce a more even distribution.</param>
        /// <param name="shift">If <c>true</c>, move the map up one row per frame to give the appearance of rising air currents.</param>
        /// <param name="rotate">If <c>true</c>, periodically generate an entirely new map, so the flame doesn't look like a video on repeat. Only makes sense to set this when <paramref name="shift"/> is also <c>true</c>.</param>
        public void SetMapParameters(int width, int height, float density, int min, int max, int smoothing = 0, bool shift = true, bool rotate = true)
        {
            if (density < 0 || density > 1)
                throw new ArgumentOutOfRangeException("Density percent must be between 0 and 1.0 inclusive.");
            if (min > max)
                throw new ArgumentOutOfRangeException("Min value must not be larger than max value.");
            if (min < 0 || min > 255)
                throw new ArgumentOutOfRangeException("Min value must be between 0 and 255 inclusive.");
            if (min > max)
                throw new ArgumentOutOfRangeException("Max value must be between 0 and 255 inclusive.");
            if (smoothing < 0)
                throw new ArgumentOutOfRangeException("Smoothing must be non-negative.");

            this.width = width;
            this.height = height;
            this.rotate = rotate;
            this.shift = shift;
            this.density = density;
            this.minValue = min;
            this.maxValue = max;
            this.smoothing = smoothing;

            InitializeCoolingMap();
        }

        private void SmoothCoolingMap(in int[] sourceMap, int[] destinationMap)
        {
            int x, y;

            // Top Row
            for (x = 1; x < width - 1; ++x)
            {
                int iFinal = 0;
                int iOriginal, iNextRow, iPreviousRow;

                iOriginal = x + (0 * width);
                iNextRow = iOriginal + width;
                iPreviousRow = (height * width) + iOriginal - width;

                iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
                destinationMap[iOriginal] = iFinal;
            }

            // All the Middle Rows
            for (y = 1; y < height - 1; ++y)
            {
                for (x = 1; x < width - 1; ++x) // Don't include the left and right edges
                {
                    // Get the surrounding colors, subtract some amount, average them, then write the result
                    int iFinal = 0;

                    int iOriginal, iNextRow, iPreviousRow;

                    iOriginal = x + (y * width);
                    iNextRow = iOriginal + width;
                    iPreviousRow = iOriginal - width;
                    iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
                    destinationMap[iOriginal] = iFinal;
                }
            }

            // Bottom Row
            for (x = 1; x < width - 1; ++x)
            {
                int iFinal = 0;
                int iOriginal, iNextRow, iPreviousRow;

                iOriginal = x + (height * width) - width;
                iNextRow = iOriginal % width;
                iPreviousRow = iOriginal - width;

                iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
                destinationMap[iOriginal] = iFinal;
            }

            // The left side
            for (y = 1; y < height - 1; ++y)
            {
                // Get the surrounding colors, subtract some amount, average them, then write the result
                int iFinal = 0;

                int iOriginal, iNextRow, iPreviousRow;

                iOriginal = 0 + (y * width);
                iNextRow = iOriginal + width;
                iPreviousRow = iOriginal - width;
                iFinal = (sourceMap[iOriginal + width - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
                destinationMap[iOriginal] = iFinal;
            }

            // The right side
            for (y = 1; y < height - 1; ++y)
            {
                // Get the surrounding colors, subtract some amount, average them, then write the result
                int iFinal = 0;

                int iOriginal, iNextRow, iPreviousRow;

                iOriginal = width - 1 + (y * width);
                iNextRow = iOriginal + width;
                iPreviousRow = iOriginal - width;
                iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1 - width] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
                destinationMap[iOriginal] = iFinal;
            }

            // The 4 corners!
            int p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12;

            //          1 2
            //        3 4 5 6
            //        7 8 9 10
            //         11 12
            p1 = sourceMap[(width * height) - 1 - width];
            p2 = sourceMap[(width * height) - width - width];
            p3 = sourceMap[(width * height) - 2];
            p4 = sourceMap[(width * height) - 1];
            p5 = sourceMap[(width * height) - width];
            p6 = sourceMap[(width * height) - width + 1];
            p7 = sourceMap[width - 2];
            p8 = sourceMap[width - 1];
            p9 = sourceMap[0];
            p10 = sourceMap[1];
            p11 = sourceMap[(2 * width) - 1];
            p12 = sourceMap[width];

            destinationMap[0] = (p9 + p5 + p8 + p10 + p12) / 5; // top left
            destinationMap[width - 1] = (p8 + p4 + p7 + p9 + p11) / 5; // top right
            destinationMap[width * height - width] = (p5 + p2 + p4 + p6 + p9) / 5; // bottom left
            destinationMap[width * height - 1] = (p4 + p1 + p3 + p5 + p8) / 5; // bottom right
        }


#region Cooling Map
        private void FillCoolingMap(int[] destination, int start, int end)
        {
            for (int i = start; i < end; ++i)
            {
                if (this.density > rng.NextDouble())
                    destination[i] = rng.Next(this.minValue, this.maxValue + 1);
                else
                    destination[i] = 0;
            }
        }
        private void UpdateRotatingCoolingMap()
        {
            int fireSize = height * width;
            int rotatingCoolingMapSize = fireSize * 2;
            if (originalCoolingMap == null || originalCoolingMap.Length != rotatingCoolingMapSize
                || rotatingCoolingMap == null || rotatingCoolingMap.Length != rotatingCoolingMapSize
                || coolingMap == null || coolingMap.Length != fireSize)
            {
                originalCoolingMap = new int[rotatingCoolingMapSize];
                rotatingCoolingMap = new int[rotatingCoolingMapSize];
                coolingMap = new int[fireSize];
                FillCoolingMap(originalCoolingMap, 0, rotatingCoolingMapSize);
            }
            else
            {
                // The rotating cooling map is TWICE the length so that we can blend the "next" frame with the previous one when switching.
                // The "original" *unmodified* CoolingMap contains the values BEFORE blending,
                // so that when we move the frame, the original old values smoothly blend into the next.
                for (int ii = 0; ii < fireSize; ++ii)
                {
                    originalCoolingMap[ii] = originalCoolingMap[ii + fireSize];
                }
                FillCoolingMap(originalCoolingMap, fireSize, rotatingCoolingMapSize);
            }

            if (this.smoothing == 0)
            {
                for (int ii = 0; ii < coolingMap.Length; ++ii)
                {
                    coolingMap[ii] = originalCoolingMap[ii];
                }
                return;
            }
            else
            {
                SmoothCoolingMap(originalCoolingMap, rotatingCoolingMap);
            }

            if (this.smoothing > 1)
            {
                SmoothCoolingMapDoubleBuffer(ref rotatingCoolingMap, this.smoothing - 1);
            }

            // Just copy over the bits.
            // Theoretically, in the at() method, we could just reference rotatingCoolingMap or originalCoolingMap
            // (depending on whether smoothing is nonzero) but that complicates reading the map,
            // unnecessarily slowing the render.
            // Just always copy to the buffer that is used for the truth
            for (int ii = 0; ii < coolingMap.Length; ++ii)
            {
                coolingMap[ii] = rotatingCoolingMap[ii];
            }
        }

        private void InitializeCoolingMap()
        {
            if (rotate && coolingMap != null)
                return;

            int size = height * width;
            if (coolingMap == null || coolingMap.Length != size)
                coolingMap = new int[size];

            FillCoolingMap(coolingMap, 0, size);

            SmoothCoolingMapDoubleBuffer(ref this.coolingMap, this.smoothing);
        }

        private void SmoothCoolingMapDoubleBuffer(ref int[] source, int cIterations)
        {
            int[] destinationMap = new int[source.Length];
            for (int i = 0; i < cIterations; ++i)
            {
                int[] swapMap;
                SmoothCoolingMap(source, destinationMap);
                swapMap = source;
                source = destinationMap;
                destinationMap = swapMap;
            }
        }
#endregion

    }
}