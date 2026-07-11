using System.Drawing;

namespace FireDemo
{
    /// <summary>
    /// ColorRange anchors a color at a point in a 0–255 palette index space.
    ///
    /// The palette generator uses each instance's range to determine how far
    /// the next anchored point is, then interpolates the colors in between.
    /// If range is -1, the range extends from the current palette position
    /// to index 255 ("the rest"). Together with <see cref="PaletteGenerator"/>,
    /// color + range drive how anchors become a smooth gradient.
    /// </summary>
    public struct ColorRange
    {
        /// <inheritdoc cref="ColorRange"/>
        /// <param name="color"><inheritdoc cref="ColorRange.color" path="/summary"/></param>
        /// <param name="range"><inheritdoc cref="ColorRange.range" path="/summary"/></param>
        public ColorRange(
            Color color,
            int range)
        {
            this.color = color;
            this.range = range;
        }

        /// <summary>
        /// The step distance from this anchored position to the next
        /// anchored position in palette index space. A value of
        /// -1 means "use the remainder of the palette up to index 255."
        /// </summary>
        public int range;

        /// <summary>
        /// The anchored palette color at a given palette index position.
        /// </summary>
        public readonly Color color;
    }
}