namespace FireDemo
{
    /// <summary>
    /// Provides double-buffered intensity storage for real-time simulation of light radiation effects.
    /// </summary>
    /// <remarks>
    /// This class does not do the computation.
    /// <see cref="ILightShape"/> does some of the drawing <i>(seeding the intensity values),</i>
    /// and implementaions of <see cref="RealtimeLightEffect"/>
    /// do the intense calculations to draw the next frame <i>(blending the
    /// values from one frame to the next in particular ways to produce
    /// different light effects).</i>
    /// </remarks>
    class IntensityMap
    {
        /// <summary>
        /// The width of the intensity map.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// The height of the intensity map.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The current most recently completed frame:
        /// the <b>destination</b> for new seed values and
        /// the <b>source</b> buffer for dissipation calculations.
        /// </summary>
        int[] intensityMatrixPrevious;

        /// <summary>
        /// The <b>destination</b> buffer: target for dissipation calculations when generating the next frame.
        /// </summary>
        int[] intensityMatrixNext;

        /// <inheritdoc cref="IntensityMap"/>
        /// <param name="width"><inheritdoc cref="IntensityMap.Width" path="/summary"/></param>
        /// <param name="height"><inheritdoc cref="IntensityMap.Height" path="/summary"/></param>
        public IntensityMap(int width, int height)
        {
            Width = width;
            Height = height;
            intensityMatrixPrevious = new int[height * width];
            intensityMatrixNext = new int[height * width];
        }

        /// <summary>
        /// Gets the intensity of the specified pixel
        /// </summary>
        /// <param name="x">Horizontal coordinate of the pixel to retrieve.</param>
        /// <param name="y">Vertical coordinate of the pixel to retrieve.</param>
        /// <returns>An integer representing the intensity of the requested pixel.</returns>
        public int GetPixelPrevious(int x, int y)
        {
            return intensityMatrixPrevious[y * Width + x];
        }

        /// <summary>
        /// Sets the intensity of the specified pixel
        /// </summary>
        /// <param name="x">Horizontal coordinate of the pixel to set.</param>
        /// <param name="y">Vertical coordinate of the pixel to set.</param>
        /// <param name="val">An integer representing the intensity of the specified pixel.</param>
        public void SetPixelPrevious(int x, int y, int val)
        {
            //if (x >= 0 && x < Width && 0 <= y && y < Height)
            intensityMatrixPrevious[y * Width + x] = val;
        }

        /// <summary>
        /// Sets the intensity of the specified pixel
        /// </summary>
        /// <param name="x">Horizontal coordinate of the pixel to set.</param>
        /// <param name="y">Vertical coordinate of the pixel to set.</param>
        /// <param name="val">An integer representing the intensity of the specified pixel.</param>
        public void SetPixelNext(int x, int y, int val)
        {
            //if (x >= 0 && x < Width && 0 <= y && y < Height)
            intensityMatrixNext[y * Width + x] = val;
        }

        /// <summary>
        /// Transitions simulation state forward by one frame, swapping the previous and next buffers.
        /// </summary>
        public void AdvanceFrame()
        {
            (intensityMatrixNext, intensityMatrixPrevious) = (intensityMatrixPrevious, intensityMatrixNext);
        }
    }
}
