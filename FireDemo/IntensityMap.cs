using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    class IntensityMap
    {
        public int Width { get; }
        public int Height { get; }

        /// <summary>
        /// The last frame displayed to the screen.
        /// 
        /// It is used as the SOURCE when generating the dissipation for the next frame.
        /// As such, it is the DESTINATION when writing the "seed" values.
        /// 
        /// This is where the new "seed" values are drawn, because this is the SOURCE frame used for generating the NEXT frame.
        /// This was the last frame displayed. The "seed" values are drawn to this buffer before dissipating to the destination buufferCurrent? Source?
        /// </summary>
        int[] intensityMatrixPrevious;

        /// <summary>
        /// The destination buffer
        /// </summary>
        int[] intensityMatrixNext;

        public IntensityMap(int width, int height)
        {
            Width = width;
            Height = height;
            intensityMatrixPrevious = new int[height * width];
            intensityMatrixNext = new int[height * width];
        }

        /// <summary>
        /// Gets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to retrieve.</param>
        /// <param name="y">The x-coordinate of the pixel to retrieve.</param>
        /// <returns>An integer representing the color of the requested pixel.</returns>
        public int GetPixelPrevious(int x, int y)
        {
            return intensityMatrixPrevious[y * Width + x];
        }

        /// <summary>
        /// Sets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        /// <param name="val">An integer representing the intensity of the specified pixel.</param>
        public void SetPixelPrevious(int x, int y, int val)
        {
            //if (x >= 0 && x < Width && 0 <= y && y < Height)
            intensityMatrixPrevious[y * Width + x] = val;
        }

        /// <summary>
        /// Sets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        /// <param name="val">An integer representing the intensity of the specified pixel.</param>
        public void SetPixelNext(int x, int y, int val)
        {
            //if (x >= 0 && x < Width && 0 <= y && y < Height)
            intensityMatrixNext[y * Width + x] = val;
        }

        public void ProgressOneFrame()
        {
            int[] temp = intensityMatrixPrevious;
            intensityMatrixPrevious = intensityMatrixNext;
            intensityMatrixNext = temp;
        }
    }
}
