using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FireDemo
{
    // seed, coals, path, shape
    interface ILightShape
    {
        void SetPen(ILightPen pen);

        /// <summary>
        /// Render this seed shape to a canvas
        /// </summary>
        /// <param name="canvas"></param>
        void DrawOn(IntensityMap canvas);
    }

    abstract class LightShapeBase : ILightShape
    {
        protected ILightPen pen; // TODO: Revert back to private after moving the pupil drawing code

        public LightShapeBase()
        {
            pen = new LightPen();
        }

        #region LightShape implementation
        public void SetPen(ILightPen pen)
        {
            this.pen = pen;
        }
        abstract public void DrawOn(IntensityMap canvas);
        #endregion // LightShape implementation

        #region Internal Drawing Methods
        /// <summary>
        /// By default, only render to the previous buffer, so the seed values affect the next frame.
        /// For some light effects, like candles, we need to set the seed value to both buffers,
        /// because the last line is not otherwise copied when swapping the buffers.
        /// The old code would copy this last line. Here in the refactored code, we can just draw to both buffers.
        /// This is unnecessary for blending effects that seed in the middle (lightning/batman).
        /// Only needed for shapes that seed on the edge (like candle)
        /// If we blend the last row of pixels, then this becomes unnecessary.
        /// </summary>
        protected bool m_fDrawToBothBuffers = false;
        protected void DrawPixel(int x, int y, IntensityMap canvas)
        {
            if (pen.FShouldDrawNext())
            {
                int nextVal = pen.NextValue();
                canvas.SetPixelPrevious(x, y, nextVal);
                if (m_fDrawToBothBuffers)
                {
                    canvas.SetPixelNext(x, y, nextVal);
                }
            }
        }

        protected void DrawCircle(int xCenter, int yCenter, int radius, IntensityMap canvas)
        {
            // draw a circle from source to destination using the pen
            int x, y, xx, xLast, width;

            xLast = xCenter;
            width = canvas.Width;

            for (int yy = yCenter - radius; yy <= yCenter + radius; ++yy)
            {
                y = (yy - yCenter);
                x = (int)Math.Sqrt(radius * radius - y * y);

                xx = x + xCenter;

                this.DrawLine(xx, yy, xLast, yy, canvas);

                this.DrawLine((width - xx), yy, (width - xLast), yy, canvas);

                xLast = xx;
            }
        }

#if false
        protected void DrawCurveY(int xCenter, int yCenter, int radius, float ptStart, float ptEnd, IntensityMap canvas)
        {
            // draw a circle from source to destination using the pen
            int x, y, xx, xLast, width;

            width = canvas.Width;

            int iStart = (int)((yCenter - radius) + ptStart * (2 * radius));
            int iEnd = (int)((yCenter + radius) - (1.0f - ptEnd) * (2 * radius));
            y = (iStart - yCenter);
            xLast = (int)Math.Sqrt(radius * radius - y * y) + xCenter;
            for (int yy = iStart; yy <= iEnd; ++yy)
            {
                y = (yy - yCenter);
                x = (int)Math.Sqrt(radius * radius - y * y);

                xx = x + xCenter;

                this.DrawLine(xx, yy, xLast, yy, canvas);

                this.DrawLine((width - xx), yy, (width - xLast), yy, canvas);

                xLast = xx;
            }
        }
#endif

        // TODO: JRDV: Generalize the oval drawing code, with optional fill pattern
        protected void DrawCurveX(int xCenter, int yCenter, int radius, float ptStart, float ptEnd, IntensityMap canvas, int xOffset)
        {
            // draw a circle from source to destination using the pen
            int x, y, yy, yLast, height;

            height = canvas.Height;

            int iStart = (int)((xCenter - radius) + ptStart * (2 * radius));
            int iEnd = (int)((xCenter + radius) - (1.0f - ptEnd) * (2 * radius));
            x = (iStart - xCenter);
            yLast = (int)Math.Sqrt(radius * radius - x * x) + yCenter;
            for (int xx = iStart; xx <= iEnd; ++xx)
            {
                x = (xx - xCenter);
                y = (int)Math.Sqrt(radius * radius - x * x);

                yy = y + yCenter;

                this.DrawLine(xx + xOffset, yy, xx + xOffset, yLast, canvas);

                this.DrawLine(xx + xOffset, (height - yy), xx + xOffset, (height - yLast), canvas);

                yLast = yy;
            }
        }

        protected void DrawLine(int x0, int y0, int x1, int y1, IntensityMap canvas)
        {
            // draw a line from source to destination using the pen

            int /*sx, sy, err, */ dx, dy, dxsigned, dysigned, xrender, yrender;

            if (x0 == x1 && y0 == y1)
            {
                this.DrawPixel(x0, y0, canvas);
                return;
            }

            //"INITIALIZE"

            dxsigned = x1 - x0;
            dysigned = y1 - y0;
            dx = Math.Abs(x1 - x0);

            //if (x0 < x1) sx = 1; else sx = -1;
            dy = Math.Abs(y1 - y0);

            //if (y0 < y1) sy = 1; else sy = -1;
            //err = dx + dy;


            if (dx > dy)
            {
                if (x0 < x1)
                {
                    for (int xindex = x0; xindex < x1; ++xindex)
                    {
                        yrender = (int)(y0 + (dysigned * (xindex - x0) / dxsigned));
                        this.DrawPixel(xindex, yrender, canvas);
                    }
                }
                else
                {
                    for (int xindex = x1; xindex < x0; ++xindex)
                    {
                        yrender = (int)(y1 + (dysigned * (xindex - x1) / dxsigned));
                        this.DrawPixel(xindex, yrender, canvas);
                    }
                }
            }
            else
            {
                if (y0 < y1)
                {
                    for (int yindex = y0; yindex < y1; ++yindex)
                    {
                        xrender = (int)(x0 + (dxsigned * (yindex - y0) / dysigned));
                        this.DrawPixel(xrender, yindex, canvas);
                    }
                }
                else
                {
                    for (int yindex = y1; yindex < y0; ++yindex)
                    {
                        xrender = (int)(x1 + (dxsigned * (yindex - y1) / dysigned));
                        this.DrawPixel(xrender, yindex, canvas);
                    }
                }
            }
        }
        #endregion // Internal Drawing Methods
    }

    class LightShapeCandle : LightShapeBase
    {
        public LightShapeCandle()
        {
            // need to draw the seed values to both the front and back buffers,
            // because the seed values are drawn on the edge, and the egde
            // pixels currently don't get copied between the buffers.
            m_fDrawToBothBuffers = true;
        }

        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a candle flame
            // Set the next row of random coals to keep the fire going.
            int width = canvas.Width;
            int height = canvas.Height;
            this.DrawLine(0, height - 1, width - 1, height - 1, canvas);
        }
    }

    class LightShapeLine : LightShapeBase
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a candle flame
            // Set the next row of random coals to keep the fire going.
            int width = canvas.Width;
            int height = canvas.Height;
            int saberHeight = Math.Max(height - 4, 1);
            int saberWidth; // = Math.Max(width, 1);
            saberWidth = 2;
            int x0 = (width - saberWidth) / 2;
            int y0 = (height - saberHeight) / 2;
            int x1 = x0 + saberWidth;
            int y1 = y0 + saberHeight;
            for (int xx = x0; xx < x1; ++xx)
            {
                this.DrawLine(xx, y0, xx, y1, canvas);
            }
        }
    }

    class LightShapeFluxPath : LightShapeBase
    {
        private readonly int m_angle = 0;
        private readonly int m_fluxLineWidth = 1;
        public LightShapeFluxPath(int angle = 0, int step = 7, int lineWidth = 1)
        {
            m_angle = angle;
            m_iStep = step;
            m_fluxLineWidth = lineWidth;
        }
        // start, end, direction, pattern/step/interval
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a candle flame
            // Set the next row of random coals to keep the fire going.
            int width = canvas.Width;
            int height = canvas.Height;
            int fluxHeight = Math.Max(height - 4, 1);
            int lineWidth; // = Math.Max(width, 1);
            lineWidth = m_fluxLineWidth;
            int x0, y0, x1, y1;
            if (m_angle == 0)
            {
                x0 = (width - lineWidth) / 2;
                y0 = (height - fluxHeight) / 2;
                x1 = x0;
                y1 = y0 + fluxHeight;
            }
            else if (m_angle == 45)
            {
                // /
                x0 = width - lineWidth - 1;
                y0 = (height - fluxHeight) / 2;
                x1 = 0;
                y1 = y0 + fluxHeight;
            }
            else if (m_angle == -45)
            {
                // \
                x0 = 0;
                y0 = (height - fluxHeight) / 2;
                x1 = width - lineWidth - 1;
                y1 = y0 + fluxHeight;
            }
            else
            {
                x0 = (width - lineWidth) / 2;
                y0 = (height - fluxHeight) / 2;
                x1 = x0;
                y1 = y0 + fluxHeight;
            }
            ++m_cIteration;
            for (int xx = 0; xx < lineWidth; ++xx)
            {
                this.DrawLineInterval(x0 + xx, y0, x1 + xx, y1, canvas);
                // TODO: JRDV: Either need to dupliucate DrawLine and add a concept of interval, or plumb in something in DrawLine,
                // either way I want to copy DrawLine to prototype it
            }
        }


        private int m_cIteration = 0;
        private readonly int m_cIntervalLength = 8;
        private readonly int m_iStep = 7;
        // TODO: JRDV: Move to internal Drawing Method section above, once I'm happy with the effect

        /// <summary>
        /// Draws a line with intervals on the intensity map.
        /// </summary>
        /// <param name="x0">The starting x-coordinate of the line.</param>
        /// <param name="y0">The starting y-coordinate of the line.</param>
        /// <param name="x1">The ending x-coordinate of the line.</param>
        /// <param name="y1">The ending y-coordinate of the line.</param>
        /// <param name="canvas">The intensity map to draw on.</param>
        protected void DrawLineInterval(int x0, int y0, int x1, int y1, IntensityMap canvas)
        {
            if (m_cIteration % 2 < 1)
                return;
            int offset = (m_cIteration * m_iStep) % m_cIntervalLength;
            // draw a line from source to destination using the pen

            int /*sx, sy, err, */ dx, dy, dxsigned, dysigned, xrender, yrender;

            if (x0 == x1 && y0 == y1)
            {
                this.DrawPixel(x0, y0, canvas);
                return;
            }

            //"INITIALIZE"

            dxsigned = x1 - x0;
            dysigned = y1 - y0;
            dx = Math.Abs(x1 - x0);

            //if (x0 < x1) sx = 1; else sx = -1;
            dy = Math.Abs(y1 - y0);

            //if (y0 < y1) sy = 1; else sy = -1;
            //err = dx + dy;


            if (dx > dy)
            {
                if (x0 < x1)
                {
                    for (int xindex = x0; xindex < x1; ++xindex)
                    {
                        bool fDraw = ((offset + xindex - x0) % (m_cIntervalLength)) < (m_cIntervalLength / 2);
                        if (fDraw)
                        {
                            yrender = (int)(y0 + (dysigned * (xindex - x0) / dxsigned));
                            this.DrawPixel(xindex, yrender, canvas);
                        }
                    }
                }
                else
                {
                    for (int xindex = x1; xindex < x0; ++xindex)
                    {
                        bool fDraw = ((offset + xindex - x1) % (m_cIntervalLength)) < (m_cIntervalLength / 2);
                        if (fDraw)
                        {
                            yrender = (int)(y1 + (dysigned * (xindex - x1) / dxsigned));
                            this.DrawPixel(xindex, yrender, canvas);
                        }
                    }
                }
            }
            else
            {
                if (y0 < y1)
                {
                    for (int yindex = y0; yindex < y1; ++yindex)
                    {
                        bool fDraw = ((offset + yindex - y0) % (m_cIntervalLength)) < (m_cIntervalLength / 2);
                        if (fDraw)
                        {
                            xrender = (int)(x0 + (dxsigned * (yindex - y0) / dysigned));
                            this.DrawPixel(xrender, yindex, canvas);
                        }
                    }
                }
                else
                {
                    for (int yindex = y1; yindex < y0; ++yindex)
                    {
                        bool fDraw = ((offset + yindex - y1) % (m_cIntervalLength)) < (m_cIntervalLength / 2);
                        if (fDraw)
                        {
                            xrender = (int)(x1 + (dxsigned * (yindex - y1) / dysigned));
                            this.DrawPixel(xrender, yindex, canvas);
                        }
                    }
                }
            }
        }
    }

    class LightShapeBatman : LightShapeBase
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a Flaming Batman Logo!

            int x0, x1, y0, y1;
            List<PointF> batArray;
            List<Point> batLogo;
            int width, height;

            width = canvas.Width;
            height = canvas.Height;

            //        batArray:= {
            //                0@0. 17.1@0. 18@2. 20@3.9. 22.5@4. 23.5@3.9. 24@2.8.
            //       "Middle"
            //       24.5@0.9. 25@2.5. 27@2.5. 27.5@0.9.
            //       28@2.8. 28.5@3.9. 29.5@4. 32@3.9. 34@2. 34.9@0. 52@0.

            //"Bottom Half"
            //       46@4. 44@8. 44.5@10.2. 40@10. 36@10.3. 32@11.5. 28@14.
            //       26@18. "Middle of tail"
            //       24@14. 20@11.5. 16@10.3. 12@10. 7.5@10.2. 8@8. 6@4.
            //       0@0.
            //       }.

            batArray = new List<PointF>
            {
                new PointF(0,0), new PointF(17.1f, 0), new PointF(18,2), new PointF(20, 3.9f),
                new PointF(22.5f, 4), new PointF(23.5f, 3.9f), new PointF(24, 2.8f),
                // Middle
                new PointF(24.5f, 0.9f), new PointF(25, 2.5f), new PointF(27, 2.5f), new PointF(27.5f, 0.9f),
                new PointF(28, 2.8f), new PointF(28.5f, 3.9f), new PointF(29.5f, 4), new PointF(32, 3.9f),
                new PointF(34, 2), new PointF(34.9f, 0), new PointF(52, 0),
        
                // Bottom Half
                new PointF(46, 4), new PointF(44, 8), new PointF(44.5f, 10.2f), new PointF(40, 10), new PointF(36, 10.3f), new PointF(32, 11.5f), new PointF(28, 14),
                new PointF(26, 18), // Middle of tail
                new PointF(24, 14), new PointF(20, 11.5f), new PointF(16, 10.3f), new PointF(12, 10), new PointF(7.5f, 10.2f), new PointF(8, 8), new PointF(6, 4),
                new PointF(0, 0)
            };

            //batArray := {0@0. 52@18}.
            //batArray:= { 0@0. 26@18. 52@18. 26@0. 0@0}.

            batLogo = new List<Point>(batArray.Count);

            foreach (PointF p in batArray)
            {
                x0 = (int)(p.X / 52 * (width - 1) + 1);
                y0 = (int)(p.Y / 18 * (height * 3.0f / 4.0f) + (height / 4.0f));

                batLogo.Add(new Point(x0, y0));
            }

            //TODO:
            //Map 0@0 to 0@height / 4
            //Map 0@52

            x1 = batLogo[0].X;
            y1 = batLogo[0].Y;

            foreach (Point p in batLogo)
            {
                x0 = x1;
                y0 = y1;
                x1 = p.X;
                y1 = p.Y;
                this.DrawLine(x0, y0, x1, y1, canvas);
            }
        }
    }

    class LightShapeLightning : LightShapeBase
    {
        protected Random rng;

        public LightShapeLightning()
        {
            rng = Util.NewRandom();
        }

        override public void DrawOn(IntensityMap canvas)
        {
            if (rng.Next(25) == 1)
                DrawOneBolt(canvas);
        }

        protected void DrawOneBolt(IntensityMap canvas)
        {
            // Originally copied from Seed8BitLightning_Branching_Cheap_LINES()
            // Now consolidated into one implementation
            int fireWidth = canvas.Width;
            int fireHeight = canvas.Height;
            DrawOneBolt(xCenter: fireWidth / 2, yCenter: 0, rotationAngle: 0, radius: fireHeight - 1, width: 1, canvas: canvas,
                minDiff: -2, maxDiff: 3, minForkDiff: 2, maxForkDiff: 4, forkingChance1: 360, forkingChance2: 50, yBranchMore: fireHeight * 2 / 3);
        }

        /// <summary>
        /// Currently only used for the Borg ring, but want to combine with the above implementation
        /// </summary>
        /// <param name="xCenter"></param>
        /// <param name="yCenter"></param>
        /// <param name="rotationAngle"></param>
        /// <param name="radius"></param>
        /// <param name="width"></param>
        /// <param name="canvas"></param>
        /// <param name="minDiff"></param>
        /// <param name="maxDiff"></param>
        /// <param name="minForkDiff"></param>
        /// <param name="maxForkDiff"></param>
        /// <param name="forkingChance1"></param>
        /// <param name="forkingChance2"></param>
        /// <param name="yBranchMore"></param>
        protected void DrawOneBolt(int xCenter, int yCenter, int rotationAngle, int radius, int width, IntensityMap canvas,
            int minDiff, int maxDiff, int minForkDiff, int maxForkDiff, int forkingChance1, int forkingChance2 = 0, int yBranchMore = 0)
        {
            int fireWidth = canvas.Width;
            // Originally copied from Seed8BitLightning_ForkingBorg_RandomRotation_EXPERIMENT
            List<int> nodes = new List<int>(1)
            {
                //int xCenter = fireWidth / 2;
                //int yCenter = fireHeight / 2;
                //int radius = Math.Min(xCenter, yCenter) - 2;
                //int rotationAngle = rng.Next(0, 360);

                xCenter
            };

            // Randomly seed the lightning path
            for (int y = yCenter; y < yCenter + radius; ++y)
            {
                /// FIDDLE WITH THE RARITY!
                /// Fork the bolt sometimes.
                /// 
                if (rng.Next(yBranchMore > 0 && y > yBranchMore ? forkingChance2 : forkingChance1) == 0)
                {
                    int nodeToFork = rng.Next(nodes.Count);

                    if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
                    {
                        int diff1 = rng.Next(minForkDiff, maxForkDiff);
                        int diff2 = rng.Next(minForkDiff, maxForkDiff);
                        nodes.Add(nodes[nodeToFork] + diff1);
                        nodes[nodeToFork] -= diff2;
                    }
                }

                for (int n = 0; n < nodes.Count; ++n)
                {
                    int diff = rng.Next(minDiff, maxDiff);

                    int x = nodes[n];
                    x += diff;
                    if (x < 0)
                        x = 0;
                    if (x > fireWidth)
                        x = fireWidth;

                    int delta = x - nodes[n];
                    int step = 0;
                    if (delta < 0)
                        step = -1;
                    else if (delta > 0)
                        step = 1;

                    int eachx = nodes[n]; // Draw every pixel between the last position (nodes[n]) and the new position (x). Then increase the variance (the random diff above) to make more realistic
                    bool isFirstIteration = true;
                    do
                    {
                        if (!isFirstIteration)
                            eachx += step;

                        isFirstIteration = false;

                        int xTemp = eachx - xCenter;
                        int yTemp = y - yCenter;

                        // if we exceed the ring DONE!
                        if (xTemp * xTemp + yTemp * yTemp > radius * radius)
                            return;

                        int xRender = (int)(xTemp * Math.Cos(rotationAngle) - yTemp * Math.Sin(rotationAngle)) + xCenter;
                        int yRender = (int)(xTemp * Math.Sin(rotationAngle) + yTemp * Math.Cos(rotationAngle)) + yCenter;

                        if (pen.FShouldDrawNext())
                        {
                            int intensity = pen.NextValue();
                            canvas.SetPixelPrevious(xRender, yRender, intensity);
                            // Hacky prototype of wider bolts. Probably want something nicer.
                            if (width >= 2)
                                canvas.SetPixelPrevious(xRender + 1, yRender + 1, intensity);
                            if (width >= 3)
                                canvas.SetPixelPrevious(xRender - 1, yRender - 1, intensity);
                            if (width >= 4)
                                canvas.SetPixelPrevious(xRender + 1, yRender - 1, intensity);
                            if (width >= 5)
                                canvas.SetPixelPrevious(xRender - 1, yRender + 1, intensity);
                        }
                    } while (eachx != x);

                    nodes[n] = x;
                }
            }
        }
    }


#if false // Experimental
    class DirectedLightning : LightShapeLightning
    {
        public float Angle { get; set; }
        public int Length { get; set; }
        public Point Location { get; set; }

        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for the inner lighting bolts for a plasma disc
            int degrees, xCenter, yCenter;
            int variance = rng.Next(-45, 45);
            variance = 0; // TODO: JRDV: add this back in once I get it working

            xCenter = canvas.Width / 2;
            yCenter = canvas.Height / 2;

            if (Angle == 0)
            {
                xCenter = canvas.Width / 2;
                yCenter = 0;
            }
            if (Angle == 90)
            {
                xCenter = 0;
                yCenter = canvas.Height / 2;
            }
            if (Angle == 270)
            {
                xCenter = canvas.Width - 1;
                yCenter = canvas.Height / 2;
            }
            degrees = (int)(Angle + variance);


            xCenter = Location.X;
            yCenter = Location.Y;

            this.DrawOneBolt(xCenter, yCenter, rotationAngle: degrees, radius: Length, width: rng.Next(-2, 6) /*TODO: make better*/, canvas: canvas);
        }
    }
#endif

    #region Borg Light Drawing
    class LightShapeBorgPlasma : LightShapeLightning
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for the inner lighting bolts for a plasma disc
            int degrees, radius, xCenter, yCenter;

            xCenter = canvas.Width / 2;
            yCenter = canvas.Height / 2;
            degrees = rng.Next(0, 360);
            radius = Math.Min(xCenter, yCenter) - 2;

            this.DrawOneBolt(xCenter, yCenter, rotationAngle: degrees, radius: radius, width: rng.Next(-2, 6), canvas: canvas,
                minDiff: -2, maxDiff: 3, minForkDiff: 1, maxForkDiff: 4, forkingChance1: 106);
        }
    }

    class LightShapeBorgRing : LightShapeBase
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for the outer ring of a plasma disc

            int xCenter, yCenter, radius;

            xCenter = canvas.Width / 2;
            yCenter = canvas.Height / 2;
            radius = Math.Min(xCenter, yCenter) - 2;

            this.DrawCircle(xCenter, yCenter, radius, canvas);
        }
    }
#endregion // Borg Light Drawing

#region EXPERIMENT Sauron experiment
    class LightShapeSauronV1_PupilOutward : LightShapeBase
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a Flaming Sauron Eye!

            int width = canvas.Width;
            int height = canvas.Height;
            int radius = Math.Min(height, width) / 2;

            /*
             * < () >
             */
            //DrawLine(width / 2, 0, width / 2, height - 1, canvas);
            //DrawCurveY(width / 2, height / 4 * 3, radius, 0, 0.2f, canvas);
            //DrawCurveY(width / 2, height / 7 * 3, radius, 0.8f, 1, canvas);
            int xOffset = -19; // TODO: Verify this is the correct offset
            DrawCurveX(width / 4 * 3, height / 2, radius, 0, 0.165f, canvas, xOffset);
            DrawCurveX(width / 7 * 3, height / 2, radius, 0.845f, 1, canvas, xOffset);
        }
    }

    class LightShapeSauronV3_PupilNarrow : LightShapeBase
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a Flaming Sauron Eye!

            int width = canvas.Width;
            int height = canvas.Height;
            int radius = Math.Min(height, width);

            /*
             * < () >
             */
            //DrawLine(width / 2, 0, width / 2, height - 1, canvas);
            //DrawCurveY(width / 2, height / 4 * 3, radius, 0, 0.2f, canvas);
            //DrawCurveY(width / 2, height / 7 * 3, radius, 0.8f, 1, canvas);
            int xOffset = -19; // TODO: Verify this is the correct offset
            // Worked fine at width 200
            //DrawCurveX(width / 4 * 3 + 61, height / 2, radius, 0, 0.035f, canvas, xOffset);
            //DrawCurveX(width / 7 * 3 - 58, height / 2, radius, 0.97f, 1, canvas, xOffset);
            DrawCurveX(width / 4 * 3 + 84, height / 2, radius, 0, 0.035f, canvas, xOffset);
            DrawCurveX(width / 7 * 3 - 67, height / 2, radius, 0.97f, 1, canvas, xOffset);

            //TemporaryHack_DetectPupilRegion(canvas);
            // Draw the pupil in dark? Or color it in after blending?
            TemporaryHackyFillInPupil(canvas); // proof-of-concept (TODO: replace with generalized code)
        }
        // TODO: JRDV: Refactor this as a separate shape, or a FILL PATTERN, when generalizing the oval drawing code
        private void TemporaryHackyFillInPupil(IntensityMap canvas)
        {
            ILightPen temp = this.pen;
            this.pen = temporaryHack;
            DrawLine(47, 16, 49, 16, canvas); DrawLine(47, 17, 50, 17, canvas); DrawLine(47, 18, 50, 18, canvas); DrawLine(46, 19, 51, 19, canvas); DrawLine(46, 20, 51, 20, canvas); DrawLine(46, 21, 51, 21, canvas); DrawLine(45, 22, 52, 22, canvas); DrawLine(45, 23, 52, 23, canvas); DrawLine(45, 24, 52, 24, canvas); DrawLine(45, 25, 52, 25, canvas); DrawLine(44, 26, 53, 26, canvas); DrawLine(44, 27, 53, 27, canvas); DrawLine(44, 28, 53, 28, canvas); DrawLine(44, 29, 53, 29, canvas); DrawLine(44, 30, 53, 30, canvas); DrawLine(43, 31, 54, 31, canvas); DrawLine(43, 32, 54, 32, canvas); DrawLine(43, 33, 54, 33, canvas); DrawLine(43, 34, 54, 34, canvas); DrawLine(43, 35, 54, 35, canvas); DrawLine(42, 36, 55, 36, canvas); DrawLine(42, 37, 55, 37, canvas); DrawLine(42, 38, 55, 38, canvas); DrawLine(42, 39, 55, 39, canvas); DrawLine(42, 40, 55, 40, canvas); DrawLine(42, 41, 55, 41, canvas); DrawLine(42, 42, 55, 42, canvas); DrawLine(42, 43, 55, 43, canvas); DrawLine(42, 44, 55, 44, canvas); DrawLine(42, 45, 55, 45, canvas); DrawLine(42, 46, 55, 46, canvas); DrawLine(42, 47, 55, 47, canvas); DrawLine(42, 48, 55, 48, canvas); DrawLine(42, 49, 55, 49, canvas); DrawLine(41, 50, 40, 50, canvas); DrawLine(42, 51, 55, 51, canvas); DrawLine(42, 52, 55, 52, canvas); DrawLine(42, 53, 55, 53, canvas); DrawLine(42, 54, 55, 54, canvas); DrawLine(42, 55, 55, 55, canvas); DrawLine(42, 56, 55, 56, canvas); DrawLine(42, 57, 55, 57, canvas); DrawLine(42, 58, 55, 58, canvas); DrawLine(42, 59, 55, 59, canvas); DrawLine(42, 60, 55, 60, canvas); DrawLine(42, 61, 55, 61, canvas); DrawLine(42, 62, 55, 62, canvas); DrawLine(42, 63, 55, 63, canvas); DrawLine(43, 64, 54, 64, canvas); DrawLine(43, 65, 54, 65, canvas); DrawLine(43, 66, 54, 66, canvas); DrawLine(43, 67, 54, 67, canvas); DrawLine(43, 68, 54, 68, canvas); DrawLine(44, 69, 53, 69, canvas); DrawLine(44, 70, 53, 70, canvas); DrawLine(44, 71, 53, 71, canvas); DrawLine(44, 72, 53, 72, canvas); DrawLine(44, 73, 53, 73, canvas); DrawLine(45, 74, 52, 74, canvas); DrawLine(45, 75, 52, 75, canvas); DrawLine(45, 76, 52, 76, canvas); DrawLine(45, 77, 52, 77, canvas); DrawLine(46, 78, 51, 78, canvas); DrawLine(46, 79, 51, 79, canvas); DrawLine(46, 80, 51, 80, canvas); DrawLine(47, 81, 50, 81, canvas); DrawLine(47, 82, 50, 82, canvas); DrawLine(47, 83, 50, 83, canvas); DrawLine(48, 84, 49, 84, canvas);
            this.pen = temp;
        }
        readonly ILightPen temporaryHack = new LightPen(fill: 1.0f, min: 20, max: 20, useFullRange: false);

        //private void TemporaryHack_DetectPupilRegion(IntensityMap canvas)
        //{
        //    string acc = string.Empty;
        //    for (int yy = 0; yy < canvas.Height; ++yy)
        //    {
        //        int xStart = -1;
        //        int xEnd = -1;
        //        for (int xx = 0; xx < canvas.Width; ++xx)
        //        {

        //            int p = canvas.GetPixelPrevious(xx, yy);
        //            if (p != 0)
        //            {
        //                if (xStart != -1)
        //                {
        //                    xEnd = xx;
        //                    break;
        //                }
        //                else
        //                {
        //                    xStart = xx;
        //                }
        //            }

        //        }

        //        if (xStart != -1 && xEnd != -1 && xStart != xEnd)
        //            acc += string.Format("DrawLine({0}, {2}, {1}, {2}, canvas);", xStart+1, xEnd-1, yy);
        //    }
        //    int breakpoint = 0;
        //}
    }

    class LightShapeSauronV2_Inward : LightShapeBase
    {
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a Flaming Sauron Eye!

            int width = canvas.Width;
            int height = canvas.Height;
            //int radius = Math.Min(height, width) / 2;

            /*
             * < () >
             */
            DrawLine(0, height / 2, width / 2, 0, canvas);
            DrawLine(0, height / 2, width / 2, height - 1, canvas);
            DrawLine(width / 2, 0, width - 1, height / 2, canvas);
            DrawLine(width / 2, height - 1, width -1, height / 2, canvas);
        }
    }
#endregion

}