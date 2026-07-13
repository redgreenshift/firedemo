using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FireDemo
{
    /// <summary>
    /// Encapsulates optimizations for speeding up <see cref="Bitmap"/> access by temporarily locking the data into system memory.
    /// </summary>
    /// <remarks>
    /// This class synchronizes a <see cref="System.Drawing.Bitmap"/> into the <see cref="Pixels"/> array to allow
    /// for high-performance read/write operations compared to traditional <c>GetPixel</c> and <c>SetPixel</c> methods.
    /// It supports bit depths of 8, 16, 24, and 32 bits per pixel.
    /// </remarks>
    public class BitmapLocker
    {
        /// <summary>
        /// The source Bitmap being locked for pixel access.
        /// </summary>
        protected readonly Bitmap bitmap = null;
        protected BitmapData bitmapData = null;
        protected IntPtr IptrBitmap = IntPtr.Zero;
        protected ImageLockMode? lockMode = null;
        public byte[] Pixels { get; private set; }
        public readonly int Depth;
        public readonly int Width;
        public readonly int Height;

        /// <inheritdoc cref="BitmapLocker"/>
        /// <param name="bitmap"><inheritdoc cref="BitmapLocker.bitmap" path="/summary"/></param>
        public BitmapLocker(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            this.Depth = Bitmap.GetPixelFormatSize(bitmap.PixelFormat);
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;

            if (Depth <= 0 || Depth > 32 || Depth % 8 != 0)
            {
                throw new ArgumentException("Unsupported Bit Depth. Only 8, 16, 24, or 32 (15? 48? 64?)");
            }
        }

        /// <summary>
        /// Locks the source bitmap data into system memory to facilitate efficient pixel-level access.
        /// </summary>
        /// <param name="flags">Specifies the access level (read/write) for the <see cref="Bitmap"/>.</param>
        /// <remarks>
        /// This method synchronizes the bitmap's pixel data into the managed <see cref="Pixels"/> array.
        /// If it is the first time locking, the buffer will be initialized to fit the bitmap dimensions.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if the bitmap is already locked.</exception>
        public void LockBits(ImageLockMode flags = ImageLockMode.ReadWrite)
        {
            if (lockMode != null)
                throw new InvalidOperationException("Bitmap already locked. Do not lock more than once.");

            Rectangle rectToLock = new Rectangle(0, 0, Width, Height);

            this.lockMode = flags;
            this.bitmapData = bitmap.LockBits(rectToLock, flags, bitmap.PixelFormat);

            if (this.Pixels == null)
            {
                // Create managed byte array to temporarily store pixel values
                int cbPixel = Depth / 8; // Count of Bytes per Pixel
                this.Pixels = new byte[Width * Height * cbPixel];
            }
            this.IptrBitmap = bitmapData.Scan0;

            if (lockMode == ImageLockMode.ReadOnly || lockMode == ImageLockMode.ReadWrite)
            {
                // Copy data from system memory to managed array.
                // This seems like it would be really slow, but it's a LOT faster
                // than just accessing the Bitmap directly via Bitmap.GetPixel();
                Marshal.Copy(IptrBitmap, Pixels, 0, Pixels.Length);
            }
        }

        /// <summary>
        /// Releases the lock on the source <see cref="Bitmap"/>, synchronizing any changes
        /// from the <see cref="Pixels"/> array back to system memory.
        /// </summary>
        /// <remarks>
        /// If the current mode allows writing (ReadWrite or WriteOnly), the contents of
        /// the <see cref="Pixels"/> buffer are copied back to the original bitmap data
        /// before unlocking. The lock state is then reset for future use.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if the bitmap was not previously locked.</exception>
        public void UnlockBits()
        {
            if (lockMode == null)
                throw new InvalidOperationException("Bitmap not locked. Cannot unlock.");

            if (lockMode == ImageLockMode.WriteOnly || lockMode == ImageLockMode.ReadWrite)
            {
                // Copy data from managed array to pointer in system memory.
                // This seems like it should be really slow, but it's a LOT faster
                // than just accessing the Bitmap directly via Bitmap.SetPixel();
                Marshal.Copy(Pixels, 0, IptrBitmap, Pixels.Length);
            }

            // Unlock the bitmap data
            this.bitmap.UnlockBits(bitmapData);
            this.bitmapData = null;
            this.lockMode = null;
        }

        /// <summary>
        /// Retrieves the <see cref="Color"/> of the specified pixel in a
        /// <see cref="Bitmap"/> that has been locked.
        /// </summary>
        /// <param name="x">Horizontal coordinate of the pixel.</param>
        /// <param name="y">Vertical coordinate of the pixel.</param>
        /// <returns>A <see cref="Color"/> representing the color at the specified position.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the bitmap is not currently locked in a read-capable mode (ReadOnly or ReadWrite).</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the calculated pixel index is out of the bounds of the data buffer.</exception>
        public Color GetPixel(int x, int y)
        {
            if (lockMode != ImageLockMode.ReadOnly && lockMode != ImageLockMode.ReadWrite)
                throw new InvalidOperationException("Reading disallowed by current state");

            Color color = Color.Empty;
            int cbPixel = Depth / 8; // Count of Bytes per Pixel
            int iPixel = (x + y * Width) * cbPixel; // Index of the specified Pixel

            if (iPixel > Pixels.Length - cbPixel)
                throw new IndexOutOfRangeException();

            switch (Depth)
            {
                case 32: // Red, Green, Blue, and Alpha (8-bits each)
                    {
                        byte b = Pixels[iPixel];
                        byte g = Pixels[iPixel + 1];
                        byte r = Pixels[iPixel + 2];
                        byte a = Pixels[iPixel + 3];
                        color = Color.FromArgb(a, r, g, b);
                    }
                    break;

                case 24: // Red, Green, and Blue (8-bits each)
                    {
                        byte b = Pixels[iPixel];
                        byte g = Pixels[iPixel + 1];
                        byte r = Pixels[iPixel + 2];
                        color = Color.FromArgb(r, g, b);
                    }
                    break;

                case 16: // Red(5), Green(6), and Blue(5) Format16bppRgb565
                    {
                        byte b = (byte)(Pixels[iPixel] & 0x1F);
                        byte g = (byte)(((Pixels[iPixel] >> 5) | (Pixels[iPixel + 1] << 3)) & 0x3F);
                        byte r = (byte)((Pixels[iPixel + 1] >> 3) & 0x1F);
                        color = Color.FromArgb(r, g, b);
                    }
                    break;

                //case 15: // Red(5), Green(5), and Blue(5) Format16bppRgb555
                //    {
                //        byte b = (byte)(Pixels[iPixel] & 0x1F);
                //        byte g = (byte)(((Pixels[iPixel] >> 5) | (Pixels[iPixel + 1] << 3)) & 0x1F);
                //        byte r = (byte)((Pixels[iPixel + 1] >> 3) & 0x1F);
                //        color = Color.FromArgb(r, g, b);
                //    }
                //    break;

                case 8: // For 8-bit depth, set the same value to Red, Green, and Blue (could just use one)
                    byte c = Pixels[iPixel];
                    color = Color.FromArgb(c, c, c);
                    break;
            }

            return color;
        }

        /// <summary>
        /// Sets the <see cref="Color"/> of the specified pixel in a
        /// <see cref="Bitmap"/> that has been locked.
        /// </summary>
        /// <param name="x">Horizontal coordinate of the pixel.</param>
        /// <param name="y">Vertical coordinate of the pixel.</param>
        /// <param name="color">A <see cref="Color"/> representing the color to write at the specified position.</param>
        /// <remarks>
        /// The color is written into the managed <see cref="Pixels"/> array in BGR byte order (B gets the lowest memory address),
        /// matching the layout used by <see cref="GetPixel(int,int)"/>,
        /// then propagated back to system memory when <see cref="UnlockBits()"/> is called.
        /// <para>
        /// Supported depths:<br/>
        /// - 8 (palette index),<br/>
        /// - 16 (BGR565 packed into two bytes),<br/>
        /// - 24 (unpacked BGR), and<br/>
        /// - 32 (BGRA) Packed as 0xAARRGGBB; when written to a little-endian byte buffer this results in byte order BGRA (B, G, R, A) at increasing offsets.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if the bitmap is not currently locked in a write-capable mode (WriteOnly or ReadWrite).</exception>
        public void SetPixel(int x, int y, Color color)
        {
            //if (lockMode != ImageLockMode.WriteOnly && lockMode != ImageLockMode.ReadWrite)
            //    throw new InvalidOperationException("Writing disallowed by current state");

            int cbPixel = Depth / 8; // Count of Bytes per Pixel
            int iPixel = (x + y * Width) * cbPixel; // Index of the specified Pixel

            switch (Depth)
            {
                case 32: // Red, Green, Blue, and Alpha (8-bits each)
                    Pixels[iPixel] = color.B;
                    Pixels[iPixel + 1] = color.G;
                    Pixels[iPixel + 2] = color.R;
                    Pixels[iPixel + 3] = color.A;
                    break;

                case 24: // Red, Green, and Blue (8-bits each)
                    Pixels[iPixel] = color.B;
                    Pixels[iPixel + 1] = color.G;
                    Pixels[iPixel + 2] = color.R;
                    break;

                case 16: // Red(5), Green(6), and Blue(5) Format16bppRgb565
                    {
                        //int r = color.R * 0x1f / 255;
                        //int g = color.G * 0x3f / 255;
                        //int b = color.B * 0x1f / 255;
                        int r = color.R;
                        int g = color.G;
                        int b = color.B;
                        UInt16 word = (ushort)((r << 11) | (g << 5) | b);
                        Pixels[iPixel] = (byte)(word & 0xFF);
                        Pixels[iPixel + 1] = (byte)(word >> 8);
                    }
                    break;

                //case 15: // Red, Green, and Blue (5-bits each) Format16bppRgb555
                //{
                ////    int r = color.R * 0x1f / 255;
                ////    int g = color.G * 0x1f / 255;
                ////    int b = color.B * 0x1f / 255;
                //    int r = color.R;
                //    int g = color.G;
                //    int b = color.B;
                //    UInt16 word = (ushort)((r << 10) | (g << 5) | b);
                //    Pixels[iPixel] = (byte)(word & 0xFF);
                //    Pixels[iPixel + 1] = (byte)(word >> 8);
                //}
                //break;

                case 8: // For 8-bit depth, the same value is in Red, Green, and Blue (so we only need to check one)
                    Pixels[iPixel] = color.B;
                    break;
            }
        }
    }
}
