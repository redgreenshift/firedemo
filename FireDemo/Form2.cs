using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FireDemo
{
    public partial class Form2 : Form
    {
        Graphics graph;
        public Form2()
        {
            InitializeComponent();
            graph = this.CreateGraphics();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void buttonDemo_Click(object sender, EventArgs e)
        {
            int maxWidth = 21;
            int maxHeight = 75;
            int magnification = 4;
            int top = buttonDemo.Location.Y + buttonDemo.Size.Height;
            int left = buttonDemo.Location.X + buttonDemo.Size.Width;
            // Clear the drawing region to eliminate artifacts from the previous flames
            Bitmap bmEmpty = new Bitmap(maxWidth, maxHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Random rng = new Random();

            //for (int x = 0; x < maxWidth; ++x)
            //    for (int y = 0; y < maxHeight; ++y)
            //        bmEmpty.SetPixel(x, y, Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255)));

            //graph.DrawImage(bmEmpty, left, top, maxWidth*magnification, maxHeight * magnification);


            Color[] palCandle = PaletteGenerator.GetRealPalette();

            ICoolingStrategy coolingStrategy = new CoolingStrategyMap();
            coolingStrategy = new CoolingStrategyConst(2); // JRDV: Temporary just to get this refactor working!
            CoolingStrategyMap coolingStrategyMap = new CoolingStrategyMap();
            coolingStrategyMap.SetMapParameters(maxWidth, maxHeight, bRotate: true, bShift: true, nDensity: 40, nMin: 5, nMax: 13, nSmoothing: 5);
            coolingStrategy = coolingStrategyMap;
            //setWidth: flameExtent x     height: flameExtent y
            //rotate: true shift: true
            //density: 40 min: 5 max: 13 smoothing: 5.

            ILightPen lpCandle = new LightPen(fill: 1.0f, min: 200, max: 255, bUseFullRange: true);
            ILightShape lsCandle = new LightShapeCandle();
            lsCandle.SetPen(lpCandle);

            //AbstractDynamicSprite
            AbstractRealtimeLightEffect dbCandle = new RealtimeCandleflame();
            dbCandle.Initialize(maxWidth, maxHeight, magnification);
            //new extent: flameExtent magnifyBy: flameMagnification).
            //dbCandle coolingStrategy: coolingStrategy.
            dbCandle.SetCoolingStrategy(coolingStrategy);

            //dbCandle palette: palCandle palette.
            dbCandle.SetPalette(palCandle);

            //dbCandle addShape: lsCandle.
            dbCandle.AddShape(lsCandle);


            // Now loop and render?
            for (int yyy = 0; yyy < 300; ++yyy)
            {
                dbCandle.RenderOneFrameToScreen(graph);
//                graph.DrawImage(bmEmpty, left, top, maxWidth * magnification, maxHeight * magnification);
            }
        }
    }
}
