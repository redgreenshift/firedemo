using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FireDemo
{
    /// <summary>
    /// Encapsulate optimizations for speeding up Bitmap access by temporarily locking the data into system memory
    /// </summary>
    public class BitmapLocker
    {
        protected readonly Bitmap bitmap = null;
        protected BitmapData bitmapData = null;
        protected IntPtr IptrBitmap = IntPtr.Zero;
        protected ImageLockMode? lockMode = null;
        public byte[] Pixels { get; private set; }
        public readonly int Depth;
        public readonly int Width;
        public readonly int Height;

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
        /// Lock bitmap data into system memory to start accessing faster
        /// </summary>
        /// <param name="flags">An ImageLockMode enumeration that specifies the access level (read/write) for the Bitmap.</param>
        /// <exception cref="InvalidOperationException">If already locked</exception>
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
        /// Unlock bitmap data when done
        /// </summary>
        /// <exception cref="InvalidOperationException">If not already locked</exception>
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
        /// Gets the color of the specified pixel in a Bitmap that has been locked
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to retrieve.</param>
        /// <param name="y">The y-coordinate of the pixel to retrieve.</param>
        /// <returns>A <b>Color</b> structure representing the color of the requested pixel.</returns>
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

                case 16: // Red(5), Green(6), and Blue(5)
                    {
                        byte b = (byte)((Pixels[iPixel] >> 3) & 0x1F);
                        byte g = (byte)(((Pixels[iPixel] << 3) | (Pixels[iPixel + 1] >> 5)) & 0x3F);
                        byte r = (byte)(Pixels[iPixel + 1] & 0x1F);
                        color = Color.FromArgb(r, g, b);
                    }
                    break;

                //case 15: // Red(5), Green(5), and Blue(5)
                //    {
                //        byte b = (byte)((Pixels[iPixel] >> 2) & 0x1F);
                //        byte g = (byte)(((Pixels[iPixel] << 3) | (Pixels[iPixel + 1] >> 5)) & 0x1F);
                //        byte r = (byte)(Pixels[iPixel + 1] & 0x1F);
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
        /// Sets the color of the specified pixel in a Bitmap that has been locked
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        /// <param name="color">A <b>Color</b> structure that represents the color to assign to the specified pixel.</param>
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

                case 16: // Red(5), Green(6), and Blue(5)
                    UInt16 word = (ushort)((color.B << 11) | (color.G << 5) | color.R);
                    Pixels[iPixel] = (byte)(word >> 8);
                    Pixels[iPixel + 1] = (byte)(word & 0xFF);
                    break;

                //case 15: // Red, Green, and Blue (5-bits each)
                //    {
                //        byte b = (byte)((Pixels[iPixel] >> 2) & 0x1F);
                //        byte g = (byte)(((Pixels[iPixel] << 3) | (Pixels[iPixel + 1] >> 5)) & 0x1F);
                //        byte r = (byte)(Pixels[iPixel + 1] & 0x1F);
                //        color = Color.FromArgb(r, g, b);
                //    }
                //    break;

                case 8: // For 8-bit depth, the same value is in Red, Green, and Blue (so we only need to check one)
                    Pixels[iPixel] = color.B;
                    break;
            }
        }
    }
}
