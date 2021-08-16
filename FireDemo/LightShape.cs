using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    interface ILightShape
    {
        void SetPen(LightPen pen);

        /// <summary>
        /// Render this shape to a canvas
        /// </summary>
        /// <param name="bc"></param>
        void DrawOn(BitCanvas bc);
    }

    abstract class LightShapeBase : ILightShape
    {
        LightPen pen;

        public LightShapeBase()
        {
            pen = new LightPen();
        }

        #region LightShape implementation
        public void SetPen(LightPen pen)
        {
            this.pen = pen;
        }
        abstract public void DrawOn(BitCanvas bc);
        #endregion // LightShape implementation

        #region Internal Drawing Methods
        protected void DrawPixel(int x, int y, BitCanvas bc)
        {
            if (pen.FShouldDrawNext())
                bc.Put(x, y, pen.NextValue());
        }

        protected void DrawCircle(int xCenter, int yCenter, int radius, BitCanvas bc)
        {
            // "draw a circle from source to destination using the pen"

            int x, y, xx, xLast, width;

            xLast = radius;
            width = bc.Width;


            for (int yy = yCenter - radius; yy <= yCenter + radius; ++yy)
            {
                y = (yy - yCenter);
                x = (int)Math.Sqrt(radius * radius - y * y);

                xx = x + xCenter;

                this.DrawLine(xx, yy, xLast, yy, bc);

                this.DrawLine((width - xx), yy, (width - xLast), yy, bc);

                xLast = xx;
            }
        }

        protected void DrawLine(int x0, int y0, int x1, int y1, BitCanvas bc)
        {
            // "draw a line from source to destination using the pen"

            int sx, sy, err, dx, dy, dxsigned, dysigned, xrender, yrender;

            if (x0 == x1 && y0 == y1)
            {
                this.DrawPixel(x0, y0, bc);
                return;
            }

            //"INITIALIZE"

            dxsigned = x1 - x0;
            dysigned = y1 - y0;
            dx = Math.Abs(x1 - x0);

            if (x0 < x1) sx = 1; else sx = -1;
            dy = Math.Abs(y1 - y0);

            if (y0 < y1) sy = 1; else sy = -1;
            err = dx + dy;


            if (dx > dy)
            {
                if (x0 < x1)
                {
                    for (int xindex = x0; xindex < x1; ++xindex)
                    {
                        yrender = (int)(y0 + (dysigned * (xindex - x0) / dxsigned));
                        this.DrawPixel(xindex, yrender, bc);
                    }
                }
                else
                {
                    for (int xindex = x1; xindex < x0; ++xindex)
                    {
                        yrender = (int)(y1 + (dysigned * (xindex - x1) / dxsigned));
                        this.DrawPixel(xindex, yrender, bc);
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
                        this.DrawPixel(xrender, yindex, bc);

                    }
                }
                else
                {
                    for (int yindex = y1; yindex < y0; ++yindex)
                    {
                        xrender = (int)(x1 + (dxsigned * (yindex - y1) / dysigned));
                        this.DrawPixel(xrender, yindex, bc);
                    }
                }
            }
        }
        #endregion // Internal Drawing Methods
    }

    class LightShapeCandle : LightShapeBase
    {
        override public void DrawOn(BitCanvas bc)
        {
            // "Draw the seed coal values for a candle flame"

            // "Set the next row of random coals to keep the fire going."

            int width, height;
            width = bc.Width;
            height = bc.Height;
            this.DrawLine(0, height - 1, width - 1, height - 1, bc);
        }
    }
}