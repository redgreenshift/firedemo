using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    interface CoolingStrategy
    {
        int at(int x, int y);
        void progressOneFrame();
    }

    class CoolingStrategyConst : CoolingStrategy
    {
        private readonly int coolingFactor;
        CoolingStrategyConst(int value)
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

        public void progressOneFrame() { /* do nothing*/ }
    };

    class CoolingStrategyMap : CoolingStrategy
    {
        // map random width height rotate shift density min max smoothing iCoolingOffset iFrame
        int[] coolingMap = null;
        readonly Random rng;
        int iCoolingOffset = 0;
        int iFrame = 0;
        int width;
        int height;
        int density;
        bool shift = false;
        bool rotate = false;
        int min;
        int max;
        int smoothing;
        CoolingStrategyMap()
        {
            rng = new Random();
        }

        public int at(int x, int y)
        {
            return 1;
        }

        public void progressOneFrame()
        {
            if (this.shift)
            {
                ++iFrame;
                iCoolingOffset = (width * iFrame) % (width * height);
            }
            else
            {
                iFrame = 0;
                iCoolingOffset = 0;
            }

            if (this.rotate && iFrame > height)
            {
                this.fillCoolingMap(coolingMap, iCoolingOffset + 1, width);
            }
        }

        void initializeCoolingMap(int w, int h, bool bRotate, bool bShift, int nDensity, int nMin, int nMax, int nSmoothing)
        {
            width = w;
            height = h;
            rotate = bRotate;
            shift = bShift;
            density = nDensity;
            min = nMin;
            max = nMax;
            smoothing = nSmoothing;

            int size = height * width;

            if (coolingMap == null)
                coolingMap = new int[size];

            this.fillCoolingMap(coolingMap, 0, size);

            this.smoothCoolingMap(smoothing);
        }
        private void fillCoolingMap(int[] coolingMap, int start, int end)
        {
            for (int i = start; i < end; ++i)
            {
                coolingMap[i] = 0;

                if (rng.Next(100) < this.density)
                    coolingMap[i] = min + rng.Next(max - min + 1) - 1;
    		}
        }

        /// <summary>
        /// Smooth the top and bottom of the buffer, so we don't get a "seam" when scrolling.
        /// </summary>
        /// <param name="iterations"></param>
        private void smoothCoolingMap(int iterations)
        {
            int[] destMap = new int[coolingMap.Length];

            for (int x = 0; x < iterations; ++x)
            {
                this.SmoothCoolingMap(ref coolingMap, ref destMap);
                int[] tempSwap = coolingMap;
                coolingMap = destMap;
                destMap = tempSwap;
            };

            destMap = null;
        }

        private void SmoothCoolingMap(ref int[] sourceMap, ref int[] destinationMap)
        {
            int x, y;

            // Top Row
            for (x = 1; x < width - 1; ++x)
            {
                int iFinal = 0;
                //int p1, p2, p3, p4, p5, p6, p7, p8, p9;
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
                //int p1, p2, p3, p4, p5, p6, p7, p8, p9;
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
    }
}