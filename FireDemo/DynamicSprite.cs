using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FireDemo
{
    abstract class AbstractDynamicSprite
    {
        protected Bitmap front;
        protected Color[] thePalette;
        public int height;
        public int width;
        protected int magnification;
        public Point Position { get; set; }

        void Initialize(int w, int h)
        {
            Position = new Point(0, 0);
            magnification = 1;
            width = w;
            height = h;
            //front = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            front = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            // TODO: JRDV: How do I set the palette? How did I do it in the main program?I just used 32bit. No need to use palette inside the bitmap
            thePalette = PaletteGenerator.GetRealPalette();
        }

        public abstract void RenderOneFrameToScreen(Graphics graph);

        public void displayToScreen(Graphics graph)
        {
            // TODO: Bicubic intrpolation??
            //graph.DrawImage(this.bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);
            graph.DrawImage(front, Position.X, Position.Y, width * magnification, height * magnification);
        }
    }

    abstract class AbstractRealtimeLightEffect : AbstractDynamicSprite
    {
        protected IBitCanvas intensityMatrix; // BitCanvas?
        protected Random rng;
        protected ICoolingStrategy coolingStrategy;
        List<ILightShape> lightShapes;

        public AbstractRealtimeLightEffect()
        {
            rng = new Random();
            intensityMatrix = new BitCanvas8Bit(width, height);
        }

        public override void RenderOneFrameToScreen(Graphics graph)
        {
            this.renderStage1SeedShapes();
            this.RenderStage2And3();
            this.displayToScreen(graph);
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

    class RealtimeCandleflame : AbstractRealtimeLightEffect
    {
        public override void RenderStage2And3()
        {
            //"
            //{ For flame effect scroll through every pixel and  }
            //            { choose some other pixels around it. Divide by    }
            //            { the ammount of pixels you added up and then      }
            //            { subtract a decay ammount.                        }
            //            "


            //" Average these pixels:
            //.X.
            //.X.
            //X X X
            //"
            int calc, p1, p2, p3, p5, p8, coolingFactor;

            //"I get an out of bounds error when calculating the edge.  need to do outside the loop"
            for (int y = 2; y < height - 2; ++y)
            {
                //3 to: (height - 1) do: [:y |
                p2 = intensityMatrix.Get(1, y + 1);
                p3 = intensityMatrix.Get(2, y + 1);
                //2 to: (width - 1) do: [:x |
                for (int x = 1; x < width - 2; ++x)
                {
                    //"Add the surrounding pixels"
                    p1 = p2;
                    p2 = p3;
                    p8 = intensityMatrix.Get(x, y - 1);
                    p5 = intensityMatrix.Get(x, y);
                    p3 = intensityMatrix.Get(x + 1, y + 1);

                    calc = p8 + p5 + p1 + p2 + p3;

                    //"Average the colors"

                    calc = (int)(calc / 5);

                    //"Subtract the coolingFactor value, if necessary"

                    coolingFactor = coolingStrategy.at(x, y);
                    if (calc > coolingFactor)
                        calc -= coolingFactor;
                    else
                        calc = 0;

                    intensityMatrix.Put(x, y, calc);
                    front.SetPixel(x, y, this.thePalette[calc]);
                }
            }
        }
    }

}
