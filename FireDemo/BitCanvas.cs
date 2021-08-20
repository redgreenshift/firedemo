using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    interface IBitCanvas
    {
        int Width { get; }
        int Height { get; }
        int GetPixel(int x, int y);
        void SetPixel(int x, int y, int val);
    }

    class BitCanvas8Bit : IBitCanvas
    {
        public int Width { get; }
        public int Height { get; }
        int[] intensityMatrixBackBuffer;

        readonly int[] intensityMatrixFront;

        public BitCanvas8Bit(int width, int height) // TODO: JRDV: write these out for clarity
        {
            Width = width;
            Height = height;
            intensityMatrixFront = new int[height * width];
        }

        /// <summary>
        /// Gets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to retrieve.</param>
        /// <param name="y">The x-coordinate of the pixel to retrieve.</param>
        /// <returns>An integer representing the color of the requested pixel.</returns>
        public int GetPixel(int x, int y)
        {
            return intensityMatrixFront[y * Width + x];
        }

        /// <summary>
        /// Sets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        /// <param name="val">An integer representing the intensity of the specified pixel.</param>
        public void SetPixel(int x, int y, int val)
        {
            intensityMatrixFront[y * Width + x] = val;
        }
    }
}
