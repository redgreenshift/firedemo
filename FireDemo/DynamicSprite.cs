using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace FireDemo
{
    abstract class AbstractDynamicSprite
    {
        protected Bitmap front;
        protected BitmapLocker poker;
        protected Color[] thePalette;
        public int height;
        public int width;
        public int magnification;
        public Point Location { get; set; }

        public virtual void Initialize(int width, int height, int magnification)
        {
            Location = new Point(0, 0);
            this.magnification = magnification;
            this.width = width;
            this.height = height;
            //front = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            front = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            poker = new BitmapLocker(front);
            // TODO: JRDV: How do I set the palette? How did I do it in the main program?I just used 32bit. No need to use palette inside the bitmap
            thePalette = PaletteGenerator.GetHardCodedFirePalette();
        }

        public void SetPalette(Color[] pal)
        {
            thePalette = pal;
        }

        public abstract void RenderOneFrameToScreen(Graphics graph);

        public void DisplayToScreen(Graphics graph)
        {
            bool fInterpolate = false;
            if (fInterpolate && magnification > 1)
            {
                DisplayToScreenInterpolated(graph);
            }
            else
            {
                CompositingMode cm = graph.CompositingMode; // Default SourceOver
                InterpolationMode im = graph.InterpolationMode; // Default Bilinear

                graph.CompositingMode = CompositingMode.SourceCopy;
                if (magnification == 1)
                {
                    int notUnused = 0;
                    graph.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graph.DrawImageUnscaled(front, Location.X, Location.Y, notUnused, notUnused);
                }
                else
                {
                    graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graph.DrawImage(front, Location.X, Location.Y, width * magnification, height * magnification);
                }

                graph.CompositingMode = cm;
                graph.InterpolationMode = im;
            }
        }

        // Bicubic interpolation was a fun experiment, and it does look a little better,
        // but it's WAY too slow for what I want to do. I doubt I could optimize it enough
        // given the benefits are not as great as I had hoped.
        #region EXPERIMENTAL Bicubic Interpolation is too slow
        private void DisplayToScreenInterpolated(Graphics graph)
        {
            // Bicubic interpolation?? Would like to improve the graphics quality
            //graph.DrawImage(this.bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);
            if (magnification > 1)
            {
                int finalWidth = width * magnification;
                int finalHeight = height * magnification;
                Bitmap bmToShow = new Bitmap(finalWidth, finalHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                for (int y = 0; y < finalHeight; ++y)
                {
                    for (int x = 0; x < finalWidth; ++x)
                    {
                        float u = (float)(x) / finalWidth;
                        float v = (float)(y) / finalHeight;
                        Color color = BicubicInterpolate(u, v);
                        bmToShow.SetPixel(x, y, color);
                    }
                }

                graph.DrawImage(bmToShow, Location.X, Location.Y, width * magnification, height * magnification);
            }
            else
            {
                graph.DrawImage(front, Location.X, Location.Y, width * magnification, height * magnification);
            }
        }

        private float CubicPolate(float v0, float v1, float v2, float v3, float fracty)
        {
            float A = (v3 - v2) - (v0 - v1);
            float B = (v0 - v1) - A;
            float C = v2 - v0;
            float D = v1;

            //return (float)(A * Math.Pow(fracty, 3) + B * Math.Pow(fracty, 2) + C * fracty + D);
            return D + fracty * (C + fracty * (B + fracty * A));
        }

        private float CubicPolate(byte v0, byte v1, byte v2, byte v3, float fracty)
        {
            float f0 = v0 / 255.0f;
            float f1 = v1 / 255.0f;
            float f2 = v2 / 255.0f;
            float f3 = v3 / 255.0f;

            float result = CubicPolate(f0, f1, f2, f3, fracty);

            return result;
        }

        private Color GetPixelInternal(int x, int y)
        {
            if (x < 0)
                x = 0;
            if (x >= width)
                x = width - 1;
            if (y < 0)
                y = 0;
            if (y >= height)
                y = height - 1;
            return this.front.GetPixel(x, y);
        }

        byte ConvertFloatToByte(float f)
        {
            if (f < 0.0f)
                f = 0.0f;
            else if (f > 1.0f)
                f = 1.0f;

            return (byte)(f * 255.0f);
        }
        private Color BicubicInterpolate(float u, float v)
        {
            float x = (u * this.width) - 0.5f;
            int xint = (int)(x);
            float fractx = (float)(x - Math.Floor(x));

            float y = (v * this.height) - 0.5f;
            int yint = (int)(y);
            float fracty = (float)(y - Math.Floor(y));

            // 1st row
            Color p00 = GetPixelInternal(xint - 1, yint - 1);
            Color p10 = GetPixelInternal(xint + 0, yint - 1);
            Color p20 = GetPixelInternal(xint + 1, yint - 1);
            Color p30 = GetPixelInternal(xint + 2, yint - 1);

            // 2nd row
            Color p01 = GetPixelInternal(xint - 1, yint + 0);
            Color p11 = GetPixelInternal(xint + 0, yint + 0);
            Color p21 = GetPixelInternal(xint + 1, yint + 0);
            Color p31 = GetPixelInternal(xint + 2, yint + 0);

            // 3rd row
            Color p02 = GetPixelInternal(xint - 1, yint + 1);
            Color p12 = GetPixelInternal(xint + 0, yint + 1);
            Color p22 = GetPixelInternal(xint + 1, yint + 1);
            Color p32 = GetPixelInternal(xint + 2, yint + 1);

            // 4th row
            Color p03 = GetPixelInternal(xint - 1, yint + 2);
            Color p13 = GetPixelInternal(xint + 0, yint + 2);
            Color p23 = GetPixelInternal(xint + 1, yint + 2);
            Color p33 = GetPixelInternal(xint + 2, yint + 2);

            float x1 = CubicPolate(p00.R, p10.R, p20.R, p30.R, fractx);
            float x2 = CubicPolate(p01.R, p11.R, p21.R, p31.R, fractx);
            float x3 = CubicPolate(p02.R, p12.R, p22.R, p32.R, fractx);
            float x4 = CubicPolate(p03.R, p13.R, p23.R, p33.R, fractx);

            float R = CubicPolate(x1, x2, x3, x4, fracty);

            x1 = CubicPolate(p00.G, p10.G, p20.G, p30.G, fractx);
            x2 = CubicPolate(p01.G, p11.G, p21.G, p31.G, fractx);
            x3 = CubicPolate(p02.G, p12.G, p22.G, p32.G, fractx);
            x4 = CubicPolate(p03.G, p13.G, p23.G, p33.G, fractx);
            float G = CubicPolate(x1, x2, x3, x4, fracty);

            x1 = CubicPolate(p00.B, p10.B, p20.B, p30.B, fractx);
            x2 = CubicPolate(p01.B, p11.B, p21.B, p31.B, fractx);
            x3 = CubicPolate(p02.B, p12.B, p22.B, p32.B, fractx);
            x4 = CubicPolate(p03.B, p13.B, p23.B, p33.B, fractx);
            float B = CubicPolate(x1, x2, x3, x4, fracty);

            return Color.FromArgb(ConvertFloatToByte(R), ConvertFloatToByte(G), ConvertFloatToByte(B));
        }
        #endregion
    }

    abstract class AbstractRealtimeLightEffect : AbstractDynamicSprite
    {
        protected IBitCanvas intensityMatrix;
        protected Random rng;
        protected ICoolingStrategy coolingStrategy;
        List<ILightShape> lightShapes;

        public AbstractRealtimeLightEffect()
        {
            rng = new Random();
        }

        public override void Initialize(int width, int height, int magnification)
        {
            base.Initialize(width, height, magnification);
            // OVERRIDE INITIALIZE and call super???
            OnSize();
        }
        public void OnSize()
        {
            intensityMatrix = new BitCanvas8Bit(width, height);
        }

        public override void RenderOneFrameToScreen(Graphics graph)
        {
            this.renderStage1SeedShapes();
            this.RenderStage2And3();
            this.DisplayToScreen(graph);
            coolingStrategy.progressOneFrame();
        }

        void renderStage1SeedShapes()
        {
            foreach (ILightShape ls in lightShapes)
            {
                ls.DrawOn(intensityMatrix);
            }
        }

        public abstract void RenderStage2And3();

        public void AddShape(ILightShape shape)
        {
            if (lightShapes == null)
                lightShapes = new List<ILightShape>();

            lightShapes.Add(shape);
        }

        public void SetCoolingStrategy(ICoolingStrategy cs)
        {
            coolingStrategy = cs;
        }
    }

    /// <summary>
    /// Generalized implementation that allows changing the flame algorithm at runtime. Potentially slower, but more versatile.
    /// </summary>
    class GenericRealtimeFlame : AbstractRealtimeLightEffect
    {
        bool f1, f2, f3, f4, f5, f6, f7, f8, f9;

        public GenericRealtimeFlame()
        {
            SetPixelMatrix(f8: true, f5: true, f1: true, f2: true, f3: true);
        }
        public void SetPixelMatrix(bool f1 = false, bool f2 = false, bool f3 = false, bool f4 = false, bool f5 = false, bool f6 = false, bool f7 = false, bool f8 = false, bool f9 = false)
        {
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
            this.f4 = f4;
            this.f5 = f5;
            this.f6 = f6;
            this.f7 = f7;
            this.f8 = f8;
            this.f9 = f9;
        }

        public override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average the designated pixels:
            int calc, coolingFactor, cPixelsToAverage = 0;

            if (f1)
                ++cPixelsToAverage;
            if (f2)
                ++cPixelsToAverage;
            if (f3)
                ++cPixelsToAverage;
            if (f4)
                ++cPixelsToAverage;
            if (f5)
                ++cPixelsToAverage;
            if (f6)
                ++cPixelsToAverage;
            if (f7)
                ++cPixelsToAverage;
            if (f8)
                ++cPixelsToAverage;
            if (f9)
                ++cPixelsToAverage;

            if (cPixelsToAverage < 0)
                cPixelsToAverage = 0;

            poker.LockBits(ImageLockMode.WriteOnly);
            for (int y = 1; y < height - 1; ++y)
            {
                for (int x = 1; x < width - 1; ++x)
                {
                    calc = 0;
                    // Add the surrounding pixels
                    if (f7)
                        calc += intensityMatrix.GetPixel(x - 1, y - 1);
                    if (f8)
                        calc += intensityMatrix.GetPixel(x, y - 1);
                    if (f9)
                        calc += intensityMatrix.GetPixel(x + 1, y - 1);
                    if (f4)
                        calc += intensityMatrix.GetPixel(x - 1, y);
                    if (f5)
                        calc += intensityMatrix.GetPixel(x, y);
                    if (f6)
                        calc += intensityMatrix.GetPixel(x + 1, y);
                    if (f1)
                        calc += intensityMatrix.GetPixel(x - 1, y + 1);
                    if (f2)
                        calc += intensityMatrix.GetPixel(x, y + 1);
                    if (f3)
                        calc += intensityMatrix.GetPixel(x + 1, y + 1);

                    // Average the colors
                    calc /= cPixelsToAverage;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    // Uncomment to help debug the cooling map shift
                    //if (coolingFactor < 0)
                    //    calc = 255;
                    //else
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixel(x, y, calc);
                    poker.SetPixel(x, y, this.thePalette[calc]);
                }
            }
            poker.UnlockBits();
        }
    }

    /// <summary>
    ///  Optimized for a single small flame, like a candle
    /// </summary>
    class RealtimeCandleflame : AbstractRealtimeLightEffect
    {
        public override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. X .
            //. X .
            //X X X
            int calc, p1, p2, p3, p5, p8, coolingFactor;

            poker.LockBits(ImageLockMode.WriteOnly);
            for (int y = 1; y < height - 1; ++y)
            {
                p2 = intensityMatrix.GetPixel(0, y + 1);
                p3 = intensityMatrix.GetPixel(1, y + 1);

                for (int x = 1; x < width - 1; ++x)
                {
                    // Add the surrounding pixels
                    p1 = p2;
                    p2 = p3;
                    p8 = intensityMatrix.GetPixel(x, y - 1);
                    p5 = intensityMatrix.GetPixel(x, y);
                    p3 = intensityMatrix.GetPixel(x + 1, y + 1);

                    // Average the colors
                    calc = p8 + p5 + p1 + p2 + p3;
                    calc /=  5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixel(x, y, calc);
                    poker.SetPixel(x, y, this.thePalette[calc]);
                }
            }
            poker.UnlockBits();
        }
    }

    /// <summary>
    /// Optimized for regular flames
    /// </summary>
    class RealtimeFire : AbstractRealtimeLightEffect
    {
        public override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. . .
            //. X .
            //X X X
            //"
            int calc, p1, p2, p3, p5, coolingFactor;

            poker.LockBits(ImageLockMode.WriteOnly);
            for (int y = 1; y < height - 1; ++y)
            {
                p2 = intensityMatrix.GetPixel(0, y + 1);
                p3 = intensityMatrix.GetPixel(1, y + 1);

                for (int x = 1; x < width - 1; ++x)
                {
                    // Add the surrounding pixels
                    p1 = p2;
                    p2 = p3;
                    p5 = intensityMatrix.GetPixel(x, y);
                    p3 = intensityMatrix.GetPixel(x + 1, y + 1);

                    // Average the colors
                    calc = p5 + p1 + p2 + p3;
                    calc /= 4;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixel(x, y, calc);
                    poker.SetPixel(x, y, this.thePalette[calc]);
                }
            }
            poker.UnlockBits();
        }
    }

    /// <summary>
    /// Optimized for Flaming Batman Logo
    /// 
    /// NOTE: this is HIGHLY COUPLED coupled to the LightShapeBatman implementation,
    /// but we could use the generic RealtimeFire class instead of RealtimeFireBatLogoOptimized...
    /// and it would look the same. It's just that this 'optimized' implementation provides noticible speed improvements
    /// </summary>
    class RealtimeFireBatLogoOptimized : RealtimeFire
    {
        public override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. . .
            //. X .
            //X X X
            int calc, p1, p2, p3, p5, coolingFactor;
            int deadZone, endZone, startOpt, x0, y0, x1, y1, x0inner, x1inner, y1inner;
            bool doDraw, doInnerCheck;
            int y0inner;

            deadZone = 0;
            endZone = width + 1;
            startOpt = (int)(height / 3);

            // TODO: JRDV: I bet these are all off bny 1 given I ported this from Smalltalk
            // But in any case needs to be retuned to whatever values we use in the LightShapeBatman

            // Under the bat: 6@4 20@11.5
            x0 = (int)(6.0f / 52 * (width - 1) + 1);
            y0 = (int)(4.0f / 18 * (height * 3 / 4) + (height / 4));
            x1 = (int)(20.0f / 52 * (width - 1) + 1);
            y1 = (int)(11.5f / 18 * (height * 3 / 4) + (height / 4));

            //"Inside the bat: 22.5@4
            //8@0 to 17@4
            //"
            //" This turned out to be SLIGHTLY SLOWER! Or at least not measurably faster
            // := (8 / 52 * (width - 1) + 1) asInteger.
            // := (0 / 18 * (height * 3 / 4) + (height / 4)) asInteger.
            // := (17 / 52 * (width - 1) + 1) asInteger.
            // := (4 / 18 * (height * 3 / 4) + (height / 4)) asInteger.
            // := width - x0wing.
            // := width - x1wing."

            // Inside the bat: 22.5@4
            // 12@4 to 40@6
            x0inner = (int)(10.0f / 52 * (width - 1) + 1);
            y0inner = (int)(4.0f / 18 * (height * 3 / 4) + (height / 4));
            x1inner = (int)(42.0f / 52 * (width - 1) + 1);
            y1inner = (int)(5.9f / 18 * (height * 3 / 4) + (height / 4));

            poker.LockBits(ImageLockMode.WriteOnly);

            for (int y = 1; y < height - 1; ++y)
            {
                // There are large areas of pixels that will NEVER change in the Bat Logo.
                // Approximate these regions with rectangles, so we can quickly exclude them"

                if (y > y1)
                {
                    deadZone = x1;
                    endZone = (width - x1);
                }
                else
                {
                    if (y > y0)
                    {
                        deadZone = x0;
                        endZone = (width - x0);
                    }
                }

                //	"doShoulderCheck := (y > y0wing) && (y <= y1wing)."
                doInnerCheck = (y > y0inner) && (y <= y1inner);

                p2 = intensityMatrix.GetPixel(x: 0, y: y + 1);
                p3 = intensityMatrix.GetPixel(x: 1, y: y + 1);

                for (int x = 1; x < width - 1; ++x)
                {
                    //2 to: (width - 1) do: [:x |
                    // poke the raw data into the ColorForm.

                    doDraw = (x >= deadZone) && (x <= endZone);
                    if (doDraw && doInnerCheck)
                        doDraw = (x < x0inner) || (x > x1inner);
                    //"doDraw && doShoulderCheck ifTrue: [
                    //doDraw:= (x < x0wing) || (x > x1rightWing) || ((x > x1wing) && (x < x0rightWing)).
                    //	]."

                    if (doDraw)
                    {
                        // Add the surrounding pixels
                        //p8:= (flameArr at: x at: y + 2)
                        //p5:= (flameArr at: x at: y).
                        //p1:= (flameArr at: x - 1 at: y + 1).
                        //p2:= (flameArr at: x at: y + 1).
                        //p3:= (flameArr at: x + 1 at: y + 1).

                        p1 = p2;
                        p2 = p3;
                        p5 = intensityMatrix.GetPixel(x, y);
                        p3 = intensityMatrix.GetPixel(x + 1, y + 1);

                        // Average the colors
                        calc = p5 + p1 + p2 + p3;
                        calc /= 4;

                        // Subtract the coolingFactor value, if necessary
                        coolingFactor = coolingStrategy.at(x, y);
                        if (calc > coolingFactor)
                            calc -= coolingFactor;
                        else
                            calc = 0;

                        intensityMatrix.SetPixel(x, y, calc);
                        //front.SetPixel(x, y, this.thePalette[calc]); // This is by far the most expensive part.
                        poker.SetPixel(x, y, this.thePalette[calc]);
                    }
                    else
                    {
                        p1 = p2;
                        p2 = p3;
                        p5 = 0;
                        p3 = 0;

                        //DEBUG: Show me the dead zone
                        //poker.SetPixel(x, y, this.thePalette[255]);
                    }
                }
            }

            poker.UnlockBits();
        }
    }

    /// <summary>
    /// Optimized for dissipating in place, like electricity
    /// </summary>
    class RealtimeLightning : AbstractRealtimeLightEffect
    {
        public override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. X .
            //X X X
            //. X .
            int calc, p2, p4, p5, p6, p8, coolingFactor;

            poker.LockBits(ImageLockMode.WriteOnly);

            for (int y = 1; y < height - 1; ++y)
            {
                p5 = intensityMatrix.GetPixel(0, y);
                p6 = intensityMatrix.GetPixel(1, y);

                for (int x = 1; x < width - 1; ++x)
                {
                    // Add the surrounding pixels
                    p8 = intensityMatrix.GetPixel(x, y - 1);
                    p4 = p5;
                    p5 = p6;
                    p6 = intensityMatrix.GetPixel(x + 1, y);
                    p2 = intensityMatrix.GetPixel(x, y + 1);

                    // Average the colors
                    calc = p8 + p6 + p5 + p4 + p2;
                    calc /= 5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixel(x, y, calc);
                    poker.SetPixel(x, y, this.thePalette[calc]);
                }
            }
            poker.UnlockBits();
        }
    }
}
