using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;

namespace FireDemo
{
    abstract class AbstractDynamicSprite : SimpleSprite
    {
        protected BitmapLocker poker;
        protected Color[] thePalette;

        protected AbstractDynamicSprite()
        {
        }
        public virtual void Initialize(int width, int height, int magnification)
        {
            Location = new Point(0, 0);
            this.Magnification = magnification;
            this.Width = width;
            this.Height = height;

            //Form = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            //Form = new Bitmap(width, height, PixelFormat.Format16bppRgb565);
            //Form = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            // 32bit with Alpha seems slightly faster, or at the very least not noticibly slower
            // for the largest fire shapes, so no need to parameterize the value for now.
            // Therefore this is fast enough for now.
            Form = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            poker = new BitmapLocker(Form);
        }

        public void SetPalette(Color[] pal)
        {
            thePalette = pal;
        }

        #region EXPERIMENTAL Bicubic Interpolation is too slow
#if false
        // Bicubic interpolation was a fun experiment, and it does look a little better,
        // but this implementation is WAY too slow for what I want to do.
        private void DisplayToScreenInterpolated(Graphics graph)
        {
            // Bicubic interpolation?? Would like to improve the graphics quality
            //graph.DrawImage(this.bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);
            if (Magnification > 1)
            {
                int finalWidth = Width * Magnification;
                int finalHeight = Height * Magnification;
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

                graph.DrawImage(bmToShow, Location.X, Location.Y, Width * Magnification, Height * Magnification);
            }
            else
            {
                graph.DrawImage(front, Location.X, Location.Y, Width * Magnification, Height * Magnification);
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
            if (x >= Width)
                x = Width - 1;
            if (y < 0)
                y = 0;
            if (y >= Height)
                y = Height - 1;
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
            float x = (u * this.Width) - 0.5f;
            int xint = (int)(x);
            float fractx = (float)(x - Math.Floor(x));

            float y = (v * this.Height) - 0.5f;
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
#endif
        #endregion
    }

    abstract class AbstractRealtimeLightEffect : AbstractDynamicSprite
    {
        protected IntensityMap intensityMatrix;
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
            intensityMatrix = new IntensityMap(Width, Height);
        }

        public override void RenderOneFrameToScreen(Graphics graph)
        {
            this.renderStage1SeedShapes();
            this.RenderStage2And3();
            this.DrawOn(graph);
            intensityMatrix.ProgressOneFrame();
            coolingStrategy.ProgressOneFrame();
        }

        protected void renderStage1SeedShapes()
        {
            foreach (ILightShape ls in lightShapes)
            {
                ls.DrawOn(intensityMatrix);
            }
        }

        protected abstract void RenderStage2And3();

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

        protected override void RenderStage2And3()
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
            for (int y = 1; y < Height - 1; ++y)
            {
                for (int x = 1; x < Width - 1; ++x)
                {
                    calc = 0;
                    // Add the surrounding pixels
                    if (f7)
                        calc += intensityMatrix.GetPixelPrevious(x - 1, y - 1);
                    if (f8)
                        calc += intensityMatrix.GetPixelPrevious(x, y - 1);
                    if (f9)
                        calc += intensityMatrix.GetPixelPrevious(x + 1, y - 1);
                    if (f4)
                        calc += intensityMatrix.GetPixelPrevious(x - 1, y);
                    if (f5)
                        calc += intensityMatrix.GetPixelPrevious(x, y);
                    if (f6)
                        calc += intensityMatrix.GetPixelPrevious(x + 1, y);
                    if (f1)
                        calc += intensityMatrix.GetPixelPrevious(x - 1, y + 1);
                    if (f2)
                        calc += intensityMatrix.GetPixelPrevious(x, y + 1);
                    if (f3)
                        calc += intensityMatrix.GetPixelPrevious(x + 1, y + 1);

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

                    intensityMatrix.SetPixelNext(x, y, calc);
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
        protected override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. 8 .
            //. 5 .
            //1 2 3
            int calc, p1, p2, p3, p5, p8, coolingFactor;

            poker.LockBits(ImageLockMode.WriteOnly);
            for (int y = 1; y < Height - 1; ++y)
            {
                p2 = intensityMatrix.GetPixelPrevious(0, y + 1);
                p3 = intensityMatrix.GetPixelPrevious(1, y + 1);

                for (int x = 1; x < Width - 1; ++x)
                {
                    // Add the surrounding pixels
                    p1 = p2;
                    p2 = p3;
                    p8 = intensityMatrix.GetPixelPrevious(x, y - 1);
                    p5 = intensityMatrix.GetPixelPrevious(x, y);
                    p3 = intensityMatrix.GetPixelPrevious(x + 1, y + 1);

                    // Average the colors
                    calc = p8 + p5 + p1 + p2 + p3;
                    calc /=  5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixelNext(x, y, calc);
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
        protected override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. . .
            //. 5 .
            //1 2 3
            //"
            int calc, p1, p2, p3, p5, coolingFactor;

            poker.LockBits(ImageLockMode.WriteOnly);
            for (int y = 1; y < Height - 1; ++y)
            {
                p2 = intensityMatrix.GetPixelPrevious(0, y + 1);
                p3 = intensityMatrix.GetPixelPrevious(1, y + 1);

                for (int x = 1; x < Width - 1; ++x)
                {
                    // Add the surrounding pixels
                    p1 = p2;
                    p2 = p3;
                    p5 = intensityMatrix.GetPixelPrevious(x, y);
                    p3 = intensityMatrix.GetPixelPrevious(x + 1, y + 1);

                    // Average the colors
                    calc = p5 + p1 + p2 + p3;
                    calc /= 4;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixelNext(x, y, calc);
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
        protected override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. . .
            //. 5 .
            //1 2 3
            int calc, p1, p2, p3, p5, coolingFactor;
            int deadZone, endZone, startOpt, x0, y0, x1, y1, x0inner, x1inner, y1inner;
            bool doDraw, doInnerCheck;
            int y0inner;
            int ySkipInitial;

            deadZone = 0;
            endZone = Width + 1;
            startOpt = (int)(Height / 3);

            // TODO: JRDV: I bet these are all off bny 1 given I ported this from Smalltalk
            // But in any case needs to be retuned to whatever values we use in the LightShapeBatman.
            // Hmm, seems to "Just Work"

            // Under the bat: 6@4 20@11.5
            x0 = (int)(6.0f / 52 * (Width - 1) + 1);
            y0 = (int)(4.0f / 18 * (Height * 3 / 4) + (Height / 4));
            x1 = (int)(20.0f / 52 * (Width - 1) + 1);
            y1 = (int)(11.5f / 18 * (Height * 3 / 4) + (Height / 4));

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

            // There are large regions of the screen that will always be black.
            // If we can cheaply skip these regions, then that gives us a cheap speed improvement.
            // Inside the bat: 22.5@4
            // 12@4 to 40@6
            ySkipInitial = 30;
            ySkipInitial = (int)(1.75f / 18 * (Height - 1) + 1); // X / 18  * (299) + 1 == 30; 29 * 18 / 299 = 1.745
            x0inner = (int)(10.0f / 52 * (Width - 1) + 1);
            y0inner = (int)(4.0f / 18 * (Height * 3 / 4) + (Height / 4));
            x1inner = (int)(42.0f / 52 * (Width - 1) + 1);
            y1inner = (int)(5.9f / 18 * (Height * 3 / 4) + (Height / 4));

            poker.LockBits(ImageLockMode.WriteOnly);

            for (int y = 1; y < Height - 1; ++y)
            {
                // There are large areas of pixels that will NEVER change in the Bat Logo.
                // Approximate these regions with rectangles, so we can quickly exclude them"

                if (y > y1)
                {
                    deadZone = x1;
                    endZone = (Width - x1);
                }
                else
                {
                    if (y > y0)
                    {
                        deadZone = x0;
                        endZone = (Width - x0);
                    }
                }

                //	"doShoulderCheck := (y > y0wing) && (y <= y1wing)."
                doInnerCheck = (y > y0inner) && (y <= y1inner);

                p2 = intensityMatrix.GetPixelPrevious(x: 0, y: y + 1);
                p3 = intensityMatrix.GetPixelPrevious(x: 1, y: y + 1);

                for (int x = 1; x < Width - 1; ++x)
                {
                    //2 to: (width - 1) do: [:x |
                    // poke the raw data into the ColorForm.

                    doDraw = (y > ySkipInitial) && (x >= deadZone) && (x <= endZone);
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
                        p5 = intensityMatrix.GetPixelPrevious(x, y);
                        p3 = intensityMatrix.GetPixelPrevious(x + 1, y + 1);

                        // Average the colors
                        calc = p5 + p1 + p2 + p3;
                        calc /= 4;

                        // Subtract the coolingFactor value, if necessary
                        coolingFactor = coolingStrategy.at(x, y);
                        if (calc > coolingFactor)
                            calc -= coolingFactor;
                        else
                            calc = 0;

                        intensityMatrix.SetPixelNext(x, y, calc);
                        //front.SetPixel(x, y, this.thePalette[calc]);
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
    /// Optimized for Flaming Batman Logo Multi Threaded
    /// 
    /// NOTE: this is HIGHLY COUPLED coupled to the LightShapeBatman implementation,
    /// but we could use the generic RealtimeFire class instead of RealtimeFireBatLogoOptimized...
    /// and it would look the same. It's just that this 'optimized' implementation provides noticible speed improvements
    /// </summary>
    class RealtimeFireBatLogoOptimizedMT_Base : RealtimeFire
    {
        protected int numThreads = 4;

        protected void RenderStage2And3_Slice(int iSliceNum)
        {
            int initialY = iSliceNum * Height / numThreads;
            int endY = (iSliceNum + 1) * Height / numThreads - (iSliceNum == numThreads - 1 ? 1 : 0);
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. . .
            //. 5 .
            //1 2 3
            int calc, p1, p2, p3, p5, coolingFactor;
            int deadZone, endZone, startOpt, x0, y0, x1, y1, x0inner, x1inner, y1inner;
            bool doDraw, doInnerCheck;
            int y0inner;
            int ySkipInitial;

            deadZone = 0;
            endZone = Width + 1;
            startOpt = (int)(Height / 3);

            // TODO: JRDV: I bet these are all off bny 1 given I ported this from Smalltalk
            // But in any case needs to be retuned to whatever values we use in the LightShapeBatman

            // Under the bat: 6@4 20@11.5
            x0 = (int)(6.0f / 52 * (Width - 1) + 1);
            y0 = (int)(4.0f / 18 * (Height * 3 / 4) + (Height / 4));
            x1 = (int)(20.0f / 52 * (Width - 1) + 1);
            y1 = (int)(11.5f / 18 * (Height * 3 / 4) + (Height / 4));

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
            ySkipInitial = 30;
            ySkipInitial = (int)(1.75f / 18 * (Height - 1) + 1); // X / 18  * (299) + 1 == 30; 29 * 18 / 299 = 1.745
            x0inner = (int)(10.0f / 52 * (Width - 1) + 1);
            y0inner = (int)(4.0f / 18 * (Height * 3 / 4) + (Height / 4));
            x1inner = (int)(42.0f / 52 * (Width - 1) + 1);
            y1inner = (int)(5.9f / 18 * (Height * 3 / 4) + (Height / 4));

            for (int y = initialY; y < endY; ++y)
            {
                // There are large areas of pixels that will NEVER change in the Bat Logo.
                // Approximate these regions with rectangles, so we can quickly exclude them"

                //if (false)
                //{
                //    //DEBUG: Show me the dead zone
                //    //poker.SetPixel(x, y, this.thePalette[255]);
                //    continue;
                //}

                if (y > y1)
                {
                    deadZone = x1;
                    endZone = (Width - x1);
                }
                else
                {
                    if (y > y0)
                    {
                        deadZone = x0;
                        endZone = (Width - x0);
                    }
                }

                //	"doShoulderCheck := (y > y0wing) && (y <= y1wing)."
                doInnerCheck = (y > y0inner) && (y <= y1inner);

                p2 = intensityMatrix.GetPixelPrevious(x: 0, y: y + 1);
                p3 = intensityMatrix.GetPixelPrevious(x: 1, y: y + 1);

                for (int x = 1; x < Width - 1; ++x)
                {
                    //2 to: (width - 1) do: [:x |
                    // poke the raw data into the ColorForm.

                    doDraw = (y > ySkipInitial) && (x >= deadZone) && (x <= endZone);
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
                        p5 = intensityMatrix.GetPixelPrevious(x, y);
                        p3 = intensityMatrix.GetPixelPrevious(x + 1, y + 1);

                        // Average the colors
                        calc = p5 + p1 + p2 + p3;
                        calc /= 4;

                        // Subtract the coolingFactor value, if necessary
                        coolingFactor = coolingStrategy.at(x, y);
                        if (calc > coolingFactor)
                            calc -= coolingFactor;
                        else
                            calc = 0;

                        intensityMatrix.SetPixelNext(x, y, calc);
                        //front.SetPixel(x, y, this.thePalette[calc]);
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
        }
    }

#if false
    class RealtimeFireBatLogoOptimizedMT_NaiveSubclass : RealtimeFireBatLogoOptimizedMT_Base
    {
        private readonly List<Thread> threads = new List<Thread>();

        /// <summary>
        /// EXPERIMENTAL.
        /// Encapsulates the logic to perform the work on multiple threads.
        /// Uses the naive (expensive) approach of creating new threads every frame,
        /// but it WORKS rendering at a full 60 FPS on Linux.
        /// 
        /// PROS:
        ///  - It works! Renders at 50-60 FPS on Linux (which is faster than the single threaded implementation 28-30 FPS)
        /// CONS:
        ///  - Runs at 45 FPS on Windows (which is slower than the single threaded implementation at 60-65 FPS).
        ///  - Creating new threads every frame is wasteful.
        /// </summary>
        public RealtimeFireBatLogoOptimizedMT_NaiveSubclass()
        {
        }

        public override void RenderStage2And3()
        {
            threads.Clear();
            for (int t = 1; t < numThreads; ++t)
            {
                int tid = t;
                ThreadStart myDelegate = () =>
                {
                    RenderStage2And3_Slice(iSliceNum: tid);
                };
                threads.Add(new Thread(myDelegate));
            }

            poker.LockBits(ImageLockMode.WriteOnly);

            foreach (Thread t in threads)
                t.Start();
            RenderStage2And3_Slice(iSliceNum: 0);
            foreach (Thread t in threads)
                t.Join();

            poker.UnlockBits();
        }
    }

    class RealtimeFireBatLogoOptimizedMT_ManualLongThreads : RealtimeFireBatLogoOptimizedMT_Base
    {
        private readonly List<Thread> threads = new List<Thread>();
        private readonly AutoResetEvent[] startHandles;
        private readonly AutoResetEvent[] doneHandles;
        private bool isShuttingDown = false;

        /// <summary>
        /// EXPERIMENTAL.
        /// Encapsulates the logic to perform the work on multiple threads.
        /// Attempts to reuse long lived threads.
        /// Works at fuill 60 FPS on both Linux and Windows.
        /// 
        /// PROS: It works at full speed on Windows
        /// CONS: Have to code a way to shutdown the threads. Currently have to kill the process when done.
        /// Alternatively, if I use a ThreadPool, then I don't have to shut anything down
        /// </summary>
        public RealtimeFireBatLogoOptimizedMT_ManualLongThreads()
        {
            int numBGThreads = numThreads - 1; // The MAIN thread is one of the threads
            startHandles = new AutoResetEvent[numBGThreads];
            doneHandles = new AutoResetEvent[numBGThreads];
            for (int t = 0; t < numBGThreads; ++t)
            {
                startHandles[t] = new AutoResetEvent(initialState: false);
                doneHandles[t] = new AutoResetEvent(initialState: false);
                int threadIndex = t + 1;
                ThreadStart myDelegate = () =>
                {
                    RenderStage2And3_Wrapper(iSliceNum: threadIndex);
                };
                Thread thread = new Thread(myDelegate);
                threads.Add(thread);
            }
        }

        public override void Initialize(int width, int height, int magnification)
        {
            base.Initialize(width, height, magnification);

            foreach (Thread t in threads)
                t.Start();
        }

        public override void RenderStage2And3()
        {
            poker.LockBits(ImageLockMode.WriteOnly);

            foreach (AutoResetEvent h in startHandles)
                h.Set();

            RenderStage2And3_Slice(iSliceNum: 0);

            // using WaitHandle.WaitAll(doneHandles); <-- requires the program to be MTAThread,
            // but online documentation says that if I'm writing a WinForms app then
            // STAThread is required for that code to be correct:
            // https://devblogs.microsoft.com/vbteam/stathread-vs-mtathread-whorst/
            // https://stackoverflow.com/questions/4192646/when-to-use-mtathread
            //
            // It seems two requirements are in conflict.
            // If I create new threads every frame (eww) then calling thread.Join() does not require MTAThread,
            // but creating threads every frame is wasteful...
            // and even if it appears to be working with MTAThread, there may be subtle
            // issues even if I were to implement this correctly in MTAThread.
            // 
            // We can workaround the MTA issue by explicitly waiting on each of the handles,
            // instead of calling WaitHandle.WaitAll()
            foreach (WaitHandle h in doneHandles)
                h.WaitOne();

            poker.UnlockBits();
        }

        private void RenderStage2And3_Wrapper(int iSliceNum)
        {
            if (iSliceNum == 0)
                throw new ArgumentException("Do NOT run this on the main thread!");

            do
            {
                startHandles[iSliceNum - 1].WaitOne(); // wait for work

                base.RenderStage2And3_Slice(iSliceNum);

                doneHandles[iSliceNum - 1].Set();
            } while (!isShuttingDown);
        }
    }
#endif

    class RealtimeFireBatLogoOptimizedMT_ThreadPool : RealtimeFireBatLogoOptimizedMT_Base
    {
        private readonly AutoResetEvent[] doneHandles;

        /// <summary>
        /// Encapsulates the logic to perform the work on multiple threads.
        /// Use the thread pool instead of managing threads manually
        /// 
        /// PROS:
        ///  - Works at fuill 60 FPS on both Linux and Windows.
        ///  - nothing to clean up or shutdown.
        ///  - simple.
        ///  CONS:
        ///   - none.
        ///   - Really don't want to use any other DynamicSprites at the same time?
        /// </summary>
        public RealtimeFireBatLogoOptimizedMT_ThreadPool()
        {
            int numBGThreads = numThreads - 1; // The MAIN thread is one of the threads

            doneHandles = new AutoResetEvent[numBGThreads];
            for (int t = 0; t < numBGThreads; ++t)
            {
                doneHandles[t] = new AutoResetEvent(initialState: false);
            }

            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            if (workerThreads < numThreads)
                ThreadPool.SetMinThreads(numThreads, completionPortThreads);
        }

        protected override void RenderStage2And3()
        {
            poker.LockBits(ImageLockMode.WriteOnly);

            int numBGThreads = numThreads - 1; // The MAIN thread is one of the threads
            for (int t = 0; t < numBGThreads; ++t)
            {
                int threadIndex = t + 1;
                ThreadPool.QueueUserWorkItem(new WaitCallback(RenderStage2And3_Wrapper), threadIndex);
            }

            RenderStage2And3_Slice(iSliceNum: 0);

            // WaitHandle.WaitAll(doneHandles); <-- it requires using MTAThread,
            // but we have a conflict because Windows Forms Apps require using STAThread,
            // so may cause issues even if I were to implement this correctly in MTAThread.
            // 
            // We can workaround the MTA issue by explicitly waiting on each of the handles,
            // instead of calling WaitHandle.WaitAll()
            foreach (WaitHandle h in doneHandles)
                h.WaitOne();

            poker.UnlockBits();
        }

        private void RenderStage2And3_Wrapper(object state)
        {
            int iSliceNum = (int)state;
            if (iSliceNum == 0)
                throw new ArgumentException("Do NOT run this on the main thread!");

            base.RenderStage2And3_Slice(iSliceNum);

            doneHandles[iSliceNum - 1].Set();
        }
    }


    /// <summary>
    /// Optimized for dissipating in place, like electricity
    /// </summary>
    class RealtimeLightning : AbstractRealtimeLightEffect
    {
        protected override void RenderStage2And3()
        {
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }

            // Average these pixels:
            //. 8 .
            //4 5 6
            //. 2 .
            int calc, p2, p4, p5, p6, p8, coolingFactor;

            poker.LockBits(ImageLockMode.WriteOnly);

            for (int y = 0; y < Height; ++y)
            {
                p5 = 0;
                p6 = intensityMatrix.GetPixelPrevious(0, y);

                for (int x = 0; x < Width; ++x)
                {
                    // Add the surrounding pixels
                    // I'm not sure I like the extra conditions for bounds checks,
                    // but it doesn't seem significantly different perf wise from
                    // the unrolled version, but it's a LOT simpler to keep it all inline.
                    p8 = y > 0 ? intensityMatrix.GetPixelPrevious(x, y - 1) : 0;
                    p4 = p5;
                    p5 = p6;
                    p6 = x < Width - 1 ? intensityMatrix.GetPixelPrevious(x + 1, y) : 0;
                    p2 = y < Height - 1 ? intensityMatrix.GetPixelPrevious(x, y + 1) : 0;

                    // Average the colors
                    calc = p8 + p6 + p5 + p4 + p2;
                    calc /= 5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixelNext(x, y, calc);
                    poker.SetPixel(x, y, this.thePalette[calc]);
                }
            }

            poker.UnlockBits();
        }

#if false
        public override void RenderStage2And3_MEASURE_PERF()
        {
            int iterations = 1000;
            System.Diagnostics.Stopwatch watch_Inline = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch_Separate = new System.Diagnostics.Stopwatch();

            watch_Inline.Start();
            for (int i1 = 0; i1 < iterations; ++i1)
                RenderStage2And3_Inline();
            watch_Inline.Stop();


            watch_Separate.Start();
            for (int iii = 0; iii < iterations; ++iii)
                RenderStage2And3_Separate();
            watch_Separate.Stop();

            long msecInline = watch_Inline.ElapsedMilliseconds;
            long msecSep = watch_Separate.ElapsedMilliseconds;

            string s = string.Format("Inline: {0} and Separate: {1}", msecInline, msecSep);
            System.Windows.Forms.MessageBox.Show(s);

            //throw new Exception("just a test");
        }

        private void RenderStage2And3_Separate()
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

            // Account for the top row
            // And bottom row
            for (int y = 0; y < Height; y += Height - 1)
            {
                p5 = intensityMatrix.GetPixelPrevious(0, y);
                p6 = intensityMatrix.GetPixelPrevious(1, y);
                p8 = 0;
                for (int x = 1; x < Width - 1; ++x)
                {
                    // Add the surrounding pixels
                    p4 = p5;
                    p5 = p6;
                    p6 = intensityMatrix.GetPixelPrevious(x + 1, y);
                    p2 = y == 0 ? intensityMatrix.GetPixelPrevious(x, y + 1) : 0;

                    // Average the colors
                    calc = p8 + p6 + p5 + p4 + p2;
                    calc /= 5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y: 0);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixelNext(x, y: 0, calc);
                    poker.SetPixel(x, y: 0, this.thePalette[calc]);
                }
            }

            // Account for the rest
            for (int y = 1; y < Height - 1; ++y)
            {
                p5 = 0;
                p6 = intensityMatrix.GetPixelPrevious(0, y);

                for (int x = 0; x < Width; ++x)
                {
                    // Add the surrounding pixels
                    p8 = intensityMatrix.GetPixelPrevious(x, y - 1);
                    p4 = p5;
                    p5 = p6;
                    p6 = intensityMatrix.GetPixelPrevious(x + 1, y); // TODO: JRDV: I'm not sure I like the extra condition
                    p2 = intensityMatrix.GetPixelPrevious(x, y + 1);

                    // Average the colors
                    calc = p8 + p6 + p5 + p4 + p2;
                    calc /= 5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixelNext(x, y, calc);
                    poker.SetPixel(x, y, this.thePalette[calc]);
                }
            }

#if true
            // Account for the right/left edges
            // Account for the top row
            for (int y = 1; y < Height - 1; ++y)
            {
                p5 = intensityMatrix.GetPixelPrevious(0, y);
                p6 = intensityMatrix.GetPixelPrevious(1, y);
                for (int x = 0; x < Width; x += Width - 1)
                {
                    // Add the surrounding pixels
                    p8 = intensityMatrix.GetPixelPrevious(x, y - 1);
                    p4 = p5;
                    p5 = p6;
                    p6 = x == 0 ? intensityMatrix.GetPixelPrevious(x + 1, y) : 0;
                    p2 = intensityMatrix.GetPixelPrevious(x, y + 1);

                    // Average the colors
                    calc = p8 + p6 + p5 + p4 + p2;
                    calc /= 5;

                    // Subtract the coolingFactor value, if necessary
                    coolingFactor = coolingStrategy.at(x, y: 0);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.SetPixelNext(x, y: 0, calc);
                    poker.SetPixel(x, y: 0, this.thePalette[calc]);
                }
            }
#endif
            poker.UnlockBits();
        }
#endif
    }


    class TextSprite : AbstractDynamicSprite
    {
        private readonly Random rng = new Random();

        public string Text { get; set; }
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                SetBrushColor(value);
            }
        }
        private Color color;
        public Font Font { get; set; }
        private Brush Brush { get; set; }

        /// <summary>
        ///  Size of the region to blank out when randomly moving the <see cref="Text" />
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Region within which to randomly move the text on <see cref="Period" /> (overrides <see cref="SimpleSprite.Location" />)
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        ///  How often to move the <see cref="Text" />
        /// </summary>
        public TimeSpan Period { get; set; }

        public TextSprite()
        {
            this.Brush = Brushes.White;
            Font = new Font(family: SystemFonts.DefaultFont.FontFamily, emSize: 20.0f, style: FontStyle.Regular);
            Size = new Size(150, 100); // Available

            //Font = new Font(familyName: "Arial", emSize: 20.0f, style: FontStyle.Regular);
            //Font = new Font(familyName: "Times New Roman", emSize: 20.0f, style: FontStyle.Regular);
            //Font = new Font(familyName: "Noto Sans", emSize: 20.0f, style: FontStyle.Regular);
            //Font = new Font(familyName: "Comic Sans MS", emSize: 20.0f, style: FontStyle.Regular);
            // WINDOWS:
            // "Agency FB; Algerian; Arial; Arial Black; Arial Narrow; Arial Rounded MT Bold; Bahnschrift; Bahnschrift Condensed; Bahnschrift Light; Bahnschrift Light Condensed; Bahnschrift Light SemiCondensed; Bahnschrift SemiBold; Bahnschrift SemiBold Condensed; Bahnschrift SemiBold SemiConden; Bahnschrift SemiCondensed; Bahnschrift SemiLight; Bahnschrift SemiLight Condensed; Bahnschrift SemiLight SemiConde; Baskerville Old Face; Bauhaus 93; Bell MT; Berlin Sans FB; Berlin Sans FB Demi; Bernard MT Condensed; Blackadder ITC; Bodoni MT; Bodoni MT Black; Bodoni MT Condensed; Bodoni MT Poster Compressed; Book Antiqua; Bookman Old Style; Bookshelf Symbol 7; Bradley Hand ITC; Britannic Bold; Broadway; Brush Script MT; Calibri; Calibri Light; Californian FB; Calisto MT; Cambria; Cambria Math; Candara; Candara Light; Castellar; Centaur; Century; Century Gothic; Century Schoolbook; Chiller; Colonna MT; Comic Sans MS; Consolas; Constantia; Cooper Black; Copperplate Gothic Bold; Copperplate Gothic Light; Corbel; Corbel Light; Courier New; Curlz MT; Dubai; Dubai Light; Dubai Medium; Ebrima; Edwardian Script ITC; Elephant; Engravers MT; Eras Bold ITC; Eras Demi ITC; Eras Light ITC; Eras Medium ITC; Felix Titling; Footlight MT Light; Forte; Franklin Gothic Book; Franklin Gothic Demi; Franklin Gothic Demi Cond; Franklin Gothic Heavy; Franklin Gothic Medium; Franklin Gothic Medium Cond; Freestyle Script; French Script MT; Gabriola; Gadugi; Garamond; Georgia; Gigi; Gill Sans MT; Gill Sans MT Condensed; Gill Sans MT Ext Condensed Bold; Gill Sans Ultra Bold; Gill Sans Ultra Bold Condensed; Gloucester MT Extra Condensed; Goudy Old Style; Goudy Stout; Haettenschweiler; Harlow Solid Italic; Harrington; High Tower Text; HoloLens MDL2 Assets; Impact; Imprint MT Shadow; Informal Roman; Ink Free; Javanese Text; Jokerman; Juice ITC; Kristen ITC; Kunstler Script; Leelawadee UI; Leelawadee UI Semilight; Lucida Bright; Lucida Calligraphy; Lucida Console; Lucida Fax; Lucida Handwriting; Lucida Sans; Lucida Sans Typewriter; Lucida Sans Unicode; Magneto; Maiandra GD; Malgun Gothic; Malgun Gothic Semilight; Marlett; Matura MT Script Capitals; Microsoft Himalaya; Microsoft JhengHei; Microsoft JhengHei Light; Microsoft JhengHei UI; Microsoft JhengHei UI Light; Microsoft New Tai Lue; Microsoft PhagsPa; Microsoft Sans Serif; Microsoft Tai Le; Microsoft YaHei; Microsoft YaHei Light; Microsoft YaHei UI; Microsoft YaHei UI Light; Microsoft Yi Baiti; MingLiU-ExtB; MingLiU_HKSCS-ExtB; Mistral; Modern No. 20; Mongolian Baiti; Monotype Corsiva; MS Gothic; MS Outlook; MS PGothic; MS Reference Sans Serif; MS Reference Specialty; MS UI Gothic; MT Extra; MV Boli; Myanmar Text; Niagara Engraved; Niagara Solid; Nirmala UI; Nirmala UI Semilight; Noto Sans; NSimSun; OCR A Extended; OCRATTRegular; Old English Text MT; Onyx; Palace Script MT; Palatino Linotype; Papyrus; Parchment; Perpetua; Perpetua Titling MT; Playbill; PMingLiU-ExtB; Poor Richard; Pristina; Rage Italic; Ravie; Rockwell; Rockwell Condensed; Rockwell Extra Bold; Script MT Bold; Segoe MDL2 Assets; Segoe Print; Segoe Script; Segoe UI; Segoe UI Black; Segoe UI Emoji; Segoe UI Historic; Segoe UI Light; Segoe UI Semibold; Segoe UI Semilight; Segoe UI Symbol; Showcard Gothic; SimSun; SimSun-ExtB; Sitka Banner; Sitka Display; Sitka Heading; Sitka Small; Sitka Subheading; Sitka Text; Snap ITC; Stencil; Sylfaen; Symbol; Tahoma; Tempus Sans ITC; Times New Roman; Trebuchet MS; Tw Cen MT; Tw Cen MT Condensed; Tw Cen MT Condensed Extra Bold; Verdana; Viner Hand ITC; Vivaldi; Vladimir Script; Webdings; Wide Latin; Wingdings; Wingdings 2; Wingdings 3; Yu Gothic; Yu Gothic Light; Yu Gothic Medium; Yu Gothic UI; Yu Gothic UI Light; Yu Gothic UI Semibold; Yu Gothic UI Semilight; "
            // LINUX:
            // Noto Kufi Arabic; Noto Serif Armenian; Noto Sans Thai UI; Noto Sans Arabic UI; Linux Biolinum O; DejaVu Serif; Noto Sans Javanese; Noto Sans Kharoshthi; Noto Sans Kannada UI; Noto Sans Buhid; Noto Sans Syriac; Nimbus Mono L; Noto Sans Myanmar UI; Noto Sans Coptic; Liberation Mono; Gentium Basic; Noto Sans Myanmar; Noto Serif Telugu; Noto Sans Mandaic; Noto Sans Runic; Noto Sans Sharada; cmmi10; Noto Sans Yi; Lato; Noto Sans Tagalog; Piboto Condensed; Noto Sans Symbols; stmary10; Noto Sans Sinhala UI; Noto Serif Tamil Slanted; DejaVu Sans; Linux Biolinum Keyboard O; Noto Sans Lydian; Droid Sans Fallback; Linux Libertine O; FreeMono; Nimbus Sans L; Noto Sans Bengali; Century Schoolbook L; FreeSerif; Noto Sans Mahajani; Carlito; Noto Sans Gurmukhi; Noto Sans Georgian; Liberation Serif; Noto Sans Kayah Li; Noto Sans Inscriptional Pahlavi; Noto Sans Linear A; Noto Sans Linear B; Noto Sans Gothic; Noto Sans Warang Citi; Noto Nastaliq Urdu; Noto Sans Tai Tham; Noto Sans Ugaritic; Noto Sans Rejang; Liberation Sans; Noto Sans Mongolian; Noto Sans Glagolitic; eufm10; Inconsolata; Noto Sans Adlam Unjoined; Lato; Noto Sans Ethiopic; Noto Serif Devanagari; Noto Sans Syriac Estrangela; Noto Sans Ogham; Noto Sans Bengali UI; Gentium Book Basic; Noto Sans Egyptian Hieroglyphs; Standard Symbols L; cmex10; Noto Sans Meetei Mayek; Noto Sans Psalter Pahlavi; Noto Naskh Arabic UI; Noto Sans PhagsPa; Noto Serif Hebrew; Noto Sans Old South Arabian; DejaVu Serif; Noto Sans Imperial Aramaic; Noto Serif Bengali; Noto Sans; Noto Sans Lao UI; Bitstream Vera Sans; Noto Sans Grantha; Lato; Noto Sans Khmer UI; Noto Sans Armenian; Noto Sans Khmer; Noto Sans Old Hungarian; Noto Sans Tamil UI; Lato; PibotoLt; Noto Sans Shavian; Noto Sans Tirhuta; Noto Sans Oriya UI; URW Chancery L; Noto Sans Meroitic; Noto Sans Vai; Noto Sans Samaritan; Piboto; Lato; Noto Sans Deseret; wasy10; Noto Serif Ahom; Noto Sans Gujarati; Noto Sans Osmanya; URW Gothic L; Noto Sans Bassa Vah; Noto Sans Malayalam UI; Noto Sans Telugu UI; Noto Sans Hatran; Noto Sans Newa; Noto Sans Cuneiform; Noto Serif Lao; Noto Sans Limbu; Noto Sans Avestan; Quicksand; Noto Sans Malayalam; Noto Sans Old Permic; Noto Sans Tamil; Noto Serif Khmer; cmsy10; Noto Sans Palmyrene; Noto Sans Bhaiksuki; Quicksand Medium; Noto Sans Syriac Eastern; Noto Sans NKo; Noto Serif Ethiopic; FreeSans; Noto Sans Bamum; Noto Sans Devanagari UI; Noto Sans Takri; Noto Sans Gurmukhi UI; OpenSymbol; Linux Libertine Display O; Noto Serif Kannada; Noto Sans Caucasian Albanian; msam10; Noto Serif Tibetan; Noto Sans Sundanese; Linux Libertine Mono O; esint10; Caladea; Noto Sans Devanagari; Noto Sans Old Turkic; Noto Serif Gujarati; Noto Serif Display; URW Palladio L; Noto Sans Old North Arabian; Noto Sans Oriya; Noto Sans Arabic; Noto Sans Telugu; Piboto; Noto Sans Pau Cin Hau; Noto Serif Sinhala; Noto Sans Batak; Noto Sans Saurashtra; Noto Sans Mende Kikakui; Noto Sans Osage; Noto Sans Miao; Noto Sans Buginese; Lato; msbm10; Dingbats; Noto Sans Symbols2; Noto Sans Ol Chiki; Noto Sans Modi; DejaVu Sans Mono; Noto Sans Duployan; Noto Sans Thaana; Lato; Noto Sans Mono; Noto Music; Noto Sans Sinhala; Noto Sans Marchen; Noto Sans Display; Noto Sans Phoenician; Noto Serif Tamil; Noto Sans Old Italic; Noto Sans Tai Le; Noto Sans Tifinagh; Noto Sans Sora Sompeng; Noto Serif Myanmar; Noto Sans Khudawadi; Noto Sans Lepcha; URW Bookman L; Noto Sans Thai; Noto Sans Lisu; Noto Serif; Liberation Sans Narrow; Noto Sans Tibetan; Noto Serif Balinese; Nimbus Roman No9 L; Noto Sans Chakma; Noto Sans Anatolian Hieroglyphs; Noto Sans Mro; Noto Sans Cypriot; Noto Sans Hanunoo; Noto Sans Brahmi; Noto Sans Gujarati UI; Noto Sans Lao; GentiumAlt; Noto Sans Hebrew; Noto Sans Syriac Western; Noto Sans Cherokee; cmr10; Noto Sans Pahawh Hmong; Noto Serif Georgian; Noto Sans New Tai Lue; Noto Serif Gurmukhi; Noto Serif Thai; Linux Libertine Initials O; Noto Serif Malayalam; Noto Sans Manichaean; rsfs10; Noto Sans Syloti Nagri; Lato; Noto Sans Kaithi; Noto Naskh Arabic; Noto Sans Tagbanwa; Noto Sans Carian; Noto Sans Canadian Aboriginal; Noto Sans Kannada; Noto Sans Elbasan; DejaVu Sans; Noto Sans Tai Viet; Noto Sans Adlam; Noto Sans Multani; Bitstream Vera Serif; Noto Sans Inscriptional Parthia; Noto Sans Nabataean; Noto Sans Cham; Quicksand Light; Piboto; Noto Sans Old Persian; DejaVu Sans; Bitstream Vera Sans Mono; Noto Mono; Noto Sans Lycian; DejaVu Math TeX Gyre; Gentium;
            //
            // COMMON FONTS:
            //  Noto Sans; 

            // BUT "Times New Roman" works in Linux!! So there are more commonalities
            //string fam = "";
            //foreach (FontFamily ff in FontFamily.Families)
            //{
            //    fam += ff.Name + "; ";
            //}
            //Brush = new SolidBrush(Color.Orange);
            //string sWindows = "Agency FB; Algerian; Arial; Arial Black; Arial Narrow; Arial Rounded MT Bold; Bahnschrift; Bahnschrift Condensed; Bahnschrift Light; Bahnschrift Light Condensed; Bahnschrift Light SemiCondensed; Bahnschrift SemiBold; Bahnschrift SemiBold Condensed; Bahnschrift SemiBold SemiConden; Bahnschrift SemiCondensed; Bahnschrift SemiLight; Bahnschrift SemiLight Condensed; Bahnschrift SemiLight SemiConde; Baskerville Old Face; Bauhaus 93; Bell MT; Berlin Sans FB; Berlin Sans FB Demi; Bernard MT Condensed; Blackadder ITC; Bodoni MT; Bodoni MT Black; Bodoni MT Condensed; Bodoni MT Poster Compressed; Book Antiqua; Bookman Old Style; Bookshelf Symbol 7; Bradley Hand ITC; Britannic Bold; Broadway; Brush Script MT; Calibri; Calibri Light; Californian FB; Calisto MT; Cambria; Cambria Math; Candara; Candara Light; Castellar; Centaur; Century; Century Gothic; Century Schoolbook; Chiller; Colonna MT; Comic Sans MS; Consolas; Constantia; Cooper Black; Copperplate Gothic Bold; Copperplate Gothic Light; Corbel; Corbel Light; Courier New; Curlz MT; Dubai; Dubai Light; Dubai Medium; Ebrima; Edwardian Script ITC; Elephant; Engravers MT; Eras Bold ITC; Eras Demi ITC; Eras Light ITC; Eras Medium ITC; Felix Titling; Footlight MT Light; Forte; Franklin Gothic Book; Franklin Gothic Demi; Franklin Gothic Demi Cond; Franklin Gothic Heavy; Franklin Gothic Medium; Franklin Gothic Medium Cond; Freestyle Script; French Script MT; Gabriola; Gadugi; Garamond; Georgia; Gigi; Gill Sans MT; Gill Sans MT Condensed; Gill Sans MT Ext Condensed Bold; Gill Sans Ultra Bold; Gill Sans Ultra Bold Condensed; Gloucester MT Extra Condensed; Goudy Old Style; Goudy Stout; Haettenschweiler; Harlow Solid Italic; Harrington; High Tower Text; HoloLens MDL2 Assets; Impact; Imprint MT Shadow; Informal Roman; Ink Free; Javanese Text; Jokerman; Juice ITC; Kristen ITC; Kunstler Script; Leelawadee UI; Leelawadee UI Semilight; Lucida Bright; Lucida Calligraphy; Lucida Console; Lucida Fax; Lucida Handwriting; Lucida Sans; Lucida Sans Typewriter; Lucida Sans Unicode; Magneto; Maiandra GD; Malgun Gothic; Malgun Gothic Semilight; Marlett; Matura MT Script Capitals; Microsoft Himalaya; Microsoft JhengHei; Microsoft JhengHei Light; Microsoft JhengHei UI; Microsoft JhengHei UI Light; Microsoft New Tai Lue; Microsoft PhagsPa; Microsoft Sans Serif; Microsoft Tai Le; Microsoft YaHei; Microsoft YaHei Light; Microsoft YaHei UI; Microsoft YaHei UI Light; Microsoft Yi Baiti; MingLiU-ExtB; MingLiU_HKSCS-ExtB; Mistral; Modern No. 20; Mongolian Baiti; Monotype Corsiva; MS Gothic; MS Outlook; MS PGothic; MS Reference Sans Serif; MS Reference Specialty; MS UI Gothic; MT Extra; MV Boli; Myanmar Text; Niagara Engraved; Niagara Solid; Nirmala UI; Nirmala UI Semilight; Noto Sans; NSimSun; OCR A Extended; OCRATTRegular; Old English Text MT; Onyx; Palace Script MT; Palatino Linotype; Papyrus; Parchment; Perpetua; Perpetua Titling MT; Playbill; PMingLiU-ExtB; Poor Richard; Pristina; Rage Italic; Ravie; Rockwell; Rockwell Condensed; Rockwell Extra Bold; Script MT Bold; Segoe MDL2 Assets; Segoe Print; Segoe Script; Segoe UI; Segoe UI Black; Segoe UI Emoji; Segoe UI Historic; Segoe UI Light; Segoe UI Semibold; Segoe UI Semilight; Segoe UI Symbol; Showcard Gothic; SimSun; SimSun-ExtB; Sitka Banner; Sitka Display; Sitka Heading; Sitka Small; Sitka Subheading; Sitka Text; Snap ITC; Stencil; Sylfaen; Symbol; Tahoma; Tempus Sans ITC; Times New Roman; Trebuchet MS; Tw Cen MT; Tw Cen MT Condensed; Tw Cen MT Condensed Extra Bold; Verdana; Viner Hand ITC; Vivaldi; Vladimir Script; Webdings; Wide Latin; Wingdings; Wingdings 2; Wingdings 3; Yu Gothic; Yu Gothic Light; Yu Gothic Medium; Yu Gothic UI; Yu Gothic UI Light; Yu Gothic UI Semibold; Yu Gothic UI Semilight; ";
            //string sLinux = "Noto Kufi Arabic; Noto Serif Armenian; Noto Sans Thai UI; Noto Sans Arabic UI; Linux Biolinum O; DejaVu Serif; Noto Sans Javanese; Noto Sans Kharoshthi; Noto Sans Kannada UI; Noto Sans Buhid; Noto Sans Syriac; Nimbus Mono L; Noto Sans Myanmar UI; Noto Sans Coptic; Liberation Mono; Gentium Basic; Noto Sans Myanmar; Noto Serif Telugu; Noto Sans Mandaic; Noto Sans Runic; Noto Sans Sharada; cmmi10; Noto Sans Yi; Lato; Noto Sans Tagalog; Piboto Condensed; Noto Sans Symbols; stmary10; Noto Sans Sinhala UI; Noto Serif Tamil Slanted; DejaVu Sans; Linux Biolinum Keyboard O; Noto Sans Lydian; Droid Sans Fallback; Linux Libertine O; FreeMono; Nimbus Sans L; Noto Sans Bengali; Century Schoolbook L; FreeSerif; Noto Sans Mahajani; Carlito; Noto Sans Gurmukhi; Noto Sans Georgian; Liberation Serif; Noto Sans Kayah Li; Noto Sans Inscriptional Pahlavi; Noto Sans Linear A; Noto Sans Linear B; Noto Sans Gothic; Noto Sans Warang Citi; Noto Nastaliq Urdu; Noto Sans Tai Tham; Noto Sans Ugaritic; Noto Sans Rejang; Liberation Sans; Noto Sans Mongolian; Noto Sans Glagolitic; eufm10; Inconsolata; Noto Sans Adlam Unjoined; Lato; Noto Sans Ethiopic; Noto Serif Devanagari; Noto Sans Syriac Estrangela; Noto Sans Ogham; Noto Sans Bengali UI; Gentium Book Basic; Noto Sans Egyptian Hieroglyphs; Standard Symbols L; cmex10; Noto Sans Meetei Mayek; Noto Sans Psalter Pahlavi; Noto Naskh Arabic UI; Noto Sans PhagsPa; Noto Serif Hebrew; Noto Sans Old South Arabian; DejaVu Serif; Noto Sans Imperial Aramaic; Noto Serif Bengali; Noto Sans; Noto Sans Lao UI; Bitstream Vera Sans; Noto Sans Grantha; Lato; Noto Sans Khmer UI; Noto Sans Armenian; Noto Sans Khmer; Noto Sans Old Hungarian; Noto Sans Tamil UI; Lato; PibotoLt; Noto Sans Shavian; Noto Sans Tirhuta; Noto Sans Oriya UI; URW Chancery L; Noto Sans Meroitic; Noto Sans Vai; Noto Sans Samaritan; Piboto; Lato; Noto Sans Deseret; wasy10; Noto Serif Ahom; Noto Sans Gujarati; Noto Sans Osmanya; URW Gothic L; Noto Sans Bassa Vah; Noto Sans Malayalam UI; Noto Sans Telugu UI; Noto Sans Hatran; Noto Sans Newa; Noto Sans Cuneiform; Noto Serif Lao; Noto Sans Limbu; Noto Sans Avestan; Quicksand; Noto Sans Malayalam; Noto Sans Old Permic; Noto Sans Tamil; Noto Serif Khmer; cmsy10; Noto Sans Palmyrene; Noto Sans Bhaiksuki; Quicksand Medium; Noto Sans Syriac Eastern; Noto Sans NKo; Noto Serif Ethiopic; FreeSans; Noto Sans Bamum; Noto Sans Devanagari UI; Noto Sans Takri; Noto Sans Gurmukhi UI; OpenSymbol; Linux Libertine Display O; Noto Serif Kannada; Noto Sans Caucasian Albanian; msam10; Noto Serif Tibetan; Noto Sans Sundanese; Linux Libertine Mono O; esint10; Caladea; Noto Sans Devanagari; Noto Sans Old Turkic; Noto Serif Gujarati; Noto Serif Display; URW Palladio L; Noto Sans Old North Arabian; Noto Sans Oriya; Noto Sans Arabic; Noto Sans Telugu; Piboto; Noto Sans Pau Cin Hau; Noto Serif Sinhala; Noto Sans Batak; Noto Sans Saurashtra; Noto Sans Mende Kikakui; Noto Sans Osage; Noto Sans Miao; Noto Sans Buginese; Lato; msbm10; Dingbats; Noto Sans Symbols2; Noto Sans Ol Chiki; Noto Sans Modi; DejaVu Sans Mono; Noto Sans Duployan; Noto Sans Thaana; Lato; Noto Sans Mono; Noto Music; Noto Sans Sinhala; Noto Sans Marchen; Noto Sans Display; Noto Sans Phoenician; Noto Serif Tamil; Noto Sans Old Italic; Noto Sans Tai Le; Noto Sans Tifinagh; Noto Sans Sora Sompeng; Noto Serif Myanmar; Noto Sans Khudawadi; Noto Sans Lepcha; URW Bookman L; Noto Sans Thai; Noto Sans Lisu; Noto Serif; Liberation Sans Narrow; Noto Sans Tibetan; Noto Serif Balinese; Nimbus Roman No9 L; Noto Sans Chakma; Noto Sans Anatolian Hieroglyphs; Noto Sans Mro; Noto Sans Cypriot; Noto Sans Hanunoo; Noto Sans Brahmi; Noto Sans Gujarati UI; Noto Sans Lao; GentiumAlt; Noto Sans Hebrew; Noto Sans Syriac Western; Noto Sans Cherokee; cmr10; Noto Sans Pahawh Hmong; Noto Serif Georgian; Noto Sans New Tai Lue; Noto Serif Gurmukhi; Noto Serif Thai; Linux Libertine Initials O; Noto Serif Malayalam; Noto Sans Manichaean; rsfs10; Noto Sans Syloti Nagri; Lato; Noto Sans Kaithi; Noto Naskh Arabic; Noto Sans Tagbanwa; Noto Sans Carian; Noto Sans Canadian Aboriginal; Noto Sans Kannada; Noto Sans Elbasan; DejaVu Sans; Noto Sans Tai Viet; Noto Sans Adlam; Noto Sans Multani; Bitstream Vera Serif; Noto Sans Inscriptional Parthia; Noto Sans Nabataean; Noto Sans Cham; Quicksand Light; Piboto; Noto Sans Old Persian; DejaVu Sans; Bitstream Vera Sans Mono; Noto Mono; Noto Sans Lycian; DejaVu Math TeX Gyre; Gentium;";
            //string[] arrWindowsFonts;
            //string[] arrLinuxFonts;
            //List<string> lCommonFonts = new List<string>();
            //char[] separators = { ';' };
            //arrWindowsFonts = sWindows.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //arrLinuxFonts = sLinux.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //foreach (string font in arrWindowsFonts)
            //{
            //    if (arrLinuxFonts.Contains(font))
            //    {
            //        lCommonFonts.Add(font);
            //        fam += font + "; ";
            //    }
            //}

            //Text = fam;
        }

        public void SetBrushColor(Color color)
        {
            if (color == Color.Red)
                Brush = Brushes.Red;
            else if (color == Color.Orange)
                Brush = Brushes.Orange;
            else if (color == Color.Yellow)
                Brush = Brushes.Yellow;
            else if (color == Color.Green)
                Brush = Brushes.Green;
            else if (color == Color.Blue)
                Brush = Brushes.Blue;
            else if (color == Color.Violet)
                Brush = Brushes.Violet;
            else if (color == Color.Black)
                Brush = Brushes.Black;
            else if (color == Color.White)
                Brush = Brushes.White;
            else
                Brush = new SolidBrush(color);
        }

        private DateTime lastMove = DateTime.MinValue;
        public override void RenderOneFrameToScreen(Graphics graph)
        {
            DateTime thisTime = DateTime.Now;
            if (Period != TimeSpan.Zero && lastMove < thisTime - Period)
            {
                graph.FillRectangle(Brushes.Black, Location.X, Location.Y, Size.Width, Size.Height);

                lastMove = thisTime;
                Location = new Point(rng.Next(Bounds.X, Bounds.X + Bounds.Width), rng.Next(Bounds.Y, Bounds.Y + Bounds.Height));
            }
            graph.DrawString(Text, Font, Brush, Location);
        }
    }
}
