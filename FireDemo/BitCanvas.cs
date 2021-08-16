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
        int Get(int x, int y);
        void Put(int x, int y, int val);
    }

    class BitCanvas8Bit : IBitCanvas
    {
        public int Width { get; }
        public int Height { get; }
        int[] intensityMatrixFront;
        int[] intensityMatrixBackBuffer;

        public BitCanvas8Bit(int w, int h)
        {
            Width = w;
            Height = h;
            intensityMatrixFront = new int[h * w];
        }

        public int Get(int x, int y)
        {
            return intensityMatrixFront[y * Width + x];
        }

        public void Put(int x, int y, int val)
        {
            intensityMatrixFront[y * Width + x] = val;
        }
    }
}
