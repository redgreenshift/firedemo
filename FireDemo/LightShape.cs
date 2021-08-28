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
        ILightPen pen;

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
        protected void DrawPixel(int x, int y, IntensityMap canvas)
        {
            if (pen.FShouldDrawNext())
                canvas.SetPixelPrevious(x, y, pen.NextValue());
        }

        protected void DrawCircle(int xCenter, int yCenter, int radius, IntensityMap canvas)
        {
            // draw a circle from source to destination using the pen
            int x, y, xx, xLast, width;

            xLast = radius;
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
        override public void DrawOn(IntensityMap canvas)
        {
            // Draw the seed coal values for a candle flame
            // Set the next row of random coals to keep the fire going.
            int width = canvas.Width;
            int height = canvas.Height;
            this.DrawLine(0, height - 1, width - 1, height - 1, canvas);
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
            rng = new Random();
        }
 
        override public void DrawOn(IntensityMap canvas)
        {
            if (rng.Next(25) == 1)
                DrawOneBolt(canvas);
        }

        protected void DrawOneBolt(IntensityMap canvas)
        {
            // Copied from Seed8BitLightning_Branching_Cheap_LINES()
            int fireWidth = canvas.Width;
            int fireHeight = canvas.Height;
            List<int> nodes = new List<int>(1);

            nodes.Add(fireWidth / 2);

            int yBranchMore = fireHeight * 2 / 3;

            // Randomly seed the lightning path
            for (int y = 0; y < fireHeight - 1; ++y)
            {
                /// FIDDLE WITH THE RARITY!
                /// Fork the bolt sometimes.
                /// 
                if (rng.Next(y > yBranchMore ? 50 : 360) == 0)
                {
                    int nodeToFork = rng.Next(nodes.Count);

                    if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
                    {
                        int diff1 = rng.Next(2, 4);
                        int diff2 = rng.Next(2, 4);
                        nodes.Add(nodes[nodeToFork] + diff1);
                        nodes[nodeToFork] -= diff2;
                    }
                }

                for (int n = 0; n < nodes.Count; ++n)
                {
                    int diff = rng.Next(-2, 3);

                    int x = nodes[n];
                    x += diff;
                    if (x < 0)
                        x = 0;
                    if (x > fireWidth)
                        x = fireWidth;

                    //nodes[n] = x;

                    //flameIntensityMatrixFront[x + y * fireWidth] = 255;

                    int delta = x - nodes[n];
                    int step = 0;
                    if (delta < 0)
                        step = -1;
                    else if (delta > 0)
                        step = 1;

                    int eachx = nodes[n];
                    bool isFirstIteration = true;
                    do
                    {
                        if (!isFirstIteration)
                            eachx += step;

                        isFirstIteration = false;

                        canvas.SetPixelPrevious(eachx, y, 255); // TODO: JRDV: Should some bolts start dimmer? No.
                    } while (eachx != x);

                    nodes[n] = x; // TODO: Draw every pixel between the last position and this position! Then increase the variance (the random delta above)
                }
            }
        }

        protected void DrawOneBolt(int xCenter, int yCenter, int rotationAngle, int radius, int width, IntensityMap canvas)
        {
            int fireWidth = canvas.Width;
            //int fireHeight = callback.Height;
            // Copied from Seed8BitLightning_ForkingBorg_RandomRotation_EXPERIMENT
            List<int> nodes = new List<int>(1);

            //int xCenter = fireWidth / 2;
            //int yCenter = fireHeight / 2;
            //int radius = Math.Min(xCenter, yCenter) - 2;
            //int rotationAngle = rng.Next(0, 360);

            nodes.Add(xCenter);

            // Randomly seed the lightning path
            for (int y = yCenter; y < yCenter + radius; ++y)
            {
                /// FIDDLE WITH THE RARITY!
                /// Fork the bolt sometimes.
                /// 
                if (rng.Next(106) == 0)
                {
                    int nodeToFork = rng.Next(nodes.Count);

                    if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
                    {
                        int diff1 = rng.Next(1, 4);
                        int diff2 = rng.Next(1, 4);
                        nodes.Add(nodes[nodeToFork] + diff1);
                        nodes[nodeToFork] -= diff2;
                    }
                }

                for (int n = 0; n < nodes.Count; ++n)
                {
                    int diff = rng.Next(-2, 3);

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

                    int eachx = nodes[n];
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

                        canvas.SetPixelPrevious(xRender, yRender, 255);
                        // Hacky prototype of wider bolts. Probably want something nicer.
                        // Want to also implment TEXT
                        if (width >= 2)
                            canvas.SetPixelPrevious(xRender + 1, yRender + 1, 255);
                        if (width >= 3)
                            canvas.SetPixelPrevious(xRender - 1, yRender - 1, 255);
                        if (width >= 4)
                            canvas.SetPixelPrevious(xRender + 1, yRender - 1, 255);
                        if (width >= 5)
                            canvas.SetPixelPrevious(xRender - 1, yRender + 1, 255);
                    } while (eachx != x);

                    nodes[n] = x; // TODO: Draw every pixel between the last position and this position! Then increase the variance (the random delta above)
                }
            }
        }
    }

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

            this.DrawOneBolt(xCenter, yCenter, rotationAngle: degrees, radius: radius, width: rng.Next(-2,6), canvas: canvas);
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
}