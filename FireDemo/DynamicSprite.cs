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
            // TODO: JRDV: How do I set the palette? How did I do it in the main program?I just used 32bit. No need to use palette inside the bitmap
            thePalette = PaletteGenerator.GetRealPalette();
        }

        public void SetPalette(Color[] pal)
        {
            thePalette = pal;
        }

        public abstract void RenderOneFrameToScreen(Graphics graph);

        public void displayToScreen(Graphics graph)
        {
            // TODO: Bicubic intrpolation??
            //graph.DrawImage(this.bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);
            graph.DrawImage(front, Location.X, Location.Y, width * magnification, height * magnification);
        }
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
            //Initialize(width: 10, height: 10, magnification: 1); // TODO: JRDV: I think I want to delete this. The caller MUST specify the size before use!!
        }

        public override void Initialize(int width, int height, int magnification)
        {
            base.Initialize(width, height, magnification);
            // TODO: JRDV: How do I call super class method implemetation? How do I call other constructors???
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
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }
            //"


            //" Average these pixels:
            //.X.
            //.X.
            //X X X
            //"
            int calc, p1, p2, p3, p5, p8, coolingFactor;

            //"I get an out of bounds error when calculating the edge.  need to do outside the loop"
            for (int y = 2; y < height - 1; ++y)
            {
                //3 to: (height - 1) do: [:y |
                p2 = intensityMatrix.Get(0, y + 1);
                p3 = intensityMatrix.Get(1, y + 1);
                //2 to: (width - 1) do: [:x |
                for (int x = 1; x < width - 1; ++x) // TODO: JRDV: "width - 2" is probably correct, but this looks better
                {
                    //"Add the surrounding pixels"
                    p1 = p2;
                    p2 = p3;
                    p8 = intensityMatrix.Get(x, y - 1);
                    p5 = intensityMatrix.Get(x, y);
                    p3 = intensityMatrix.Get(x + 1, y + 1);

                    //"Average the colors"
                    calc = p8 + p5 + p1 + p2 + p3;
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
            //"
            //{ For flame effect scroll through every pixel and  }
            //{ choose some other pixels around it. Divide by    }
            //{ the ammount of pixels you added up and then      }
            //{ subtract a decay ammount.                        }
            //"


            //" Average these pixels:
            //.X.
            //.X.
            //X X X
            //"
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

            for (int y = 1; y < height - 1; ++y)
            {
                for (int x = 1; x < width - 1; ++x) // TODO: JRDV: "width - 2" is probably correct, but this looks better
                {
                    calc = 0;
                    //"Add the surrounding pixels"
                    if (f7)
                        calc += intensityMatrix.Get(x - 1, y - 1);
                    if (f8)
                        calc += intensityMatrix.Get(x, y - 1);
                    if (f9)
                        calc += intensityMatrix.Get(x + 1, y - 1);
                    if (f4)
                        calc += intensityMatrix.Get(x - 1, y);
                    if (f5)
                        calc += intensityMatrix.Get(x, y);
                    if (f6)
                        calc += intensityMatrix.Get(x + 1, y);
                    if (f1)
                        calc += intensityMatrix.Get(x - 1, y + 1);
                    if (f2)
                        calc += intensityMatrix.Get(x, y + 1);
                    if (f3)
                        calc += intensityMatrix.Get(x + 1, y + 1);

                    //"Average the colors"
                    calc = (int)(calc / cPixelsToAverage);

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
