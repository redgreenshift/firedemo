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
        Graphics m_graph;
        Color[] m_palette;
        ICoolingStrategy m_coolingStrategy;
        ILightPen m_lightPen;
        ILightShape m_lightShape;
        AbstractDynamicSprite m_dbSprite;
        private GenericRealtimeFlame m_genericFlame;
        int m_fireWidth;
        int m_fireHeight;
        int m_framesPerSecond = 64;

        public Form2()
        {
            InitializeComponent();
            m_graph = this.CreateGraphics();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int fireWidth = 21;
            int fireHeight = 75;
            int magnification = 4;
            int top = buttonDemo.Location.Y + buttonDemo.Size.Height;
            int left = buttonDemo.Location.X + buttonDemo.Size.Width;
            // Clear the drawing region to eliminate artifacts from the previous flames
            Bitmap bmEmpty = new Bitmap(fireWidth, fireHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Random rng = new Random();

            //for (int x = 0; x < maxWidth; ++x)
            //    for (int y = 0; y < maxHeight; ++y)
            //        bmEmpty.SetPixel(x, y, Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255)));

            //graph.DrawImage(bmEmpty, left, top, maxWidth*magnification, maxHeight * magnification);


            Color[] palCandle = PaletteGenerator.GetRealPalette();

            ICoolingStrategy coolingStrategy = new CoolingStrategyMap();
            //coolingStrategy = new CoolingStrategyConst(2); // JRDV: Temporary just to get this refactor working!
            CoolingStrategyMap coolingStrategyMap = new CoolingStrategyMap();
            coolingStrategyMap.SetMapParameters(fireWidth, fireHeight,
                bRotate: true, bShift: true,
                nDensity: 40, nMin: 5, nMax: 13, nSmoothing: 5);
            coolingStrategy = coolingStrategyMap;

            ILightPen lpCandle = new LightPen(fill: 1.0f, min: 200, max: 255, bUseFullRange: true);
            ILightShape lsCandle = new LightShapeCandle();
            lsCandle.SetPen(lpCandle);

            //AbstractDynamicSprite
            AbstractRealtimeLightEffect dbCandle = new RealtimeCandleflame();
            //m_genericFlame = new GenericRealtimeFlame();
            //dbCandle = m_genericFlame;
            dbCandle.Initialize(fireWidth, fireHeight, magnification);
            dbCandle.SetCoolingStrategy(coolingStrategy);
            dbCandle.SetPalette(palCandle);
            dbCandle.AddShape(lsCandle);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2;
            dbCandle.Location = new Point(x: left, y: top);

            m_coolingStrategy = coolingStrategy;
            m_palette = palCandle;
            m_lightPen = lpCandle;
            m_lightShape = lsCandle;
            m_dbSprite = dbCandle;
        }

        private void buttonDemo_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                //UpdateFireDimensions();

                //if (rotateCoolingMapCheckBox.Checked)
                //    UpdateRotatingCoolingMap();

                //Bitmap bmEmpty = new Bitmap(fireWidth, fireHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                //for (int xx = 0; xx < fireWidth; ++xx)
                //    for (int yy = 0; yy < fireHeight; ++yy)
                //    {
                //        bmEmpty.SetPixel(xx, yy, Color.Black);
                //    }

                //Bitmap bmEmpty = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                //m_graph.DrawImage(bmEmpty, 0, 0, this.Width, this.Height);
                this.BackColor = Color.Black;

                buttonDemo.Text = "Stop!";
                timer1.Interval = (int)(1000 / m_framesPerSecond);
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                buttonDemo.Text = "Start!";
            }

            // Now loop and render?
            //for (int yyy = 0; yyy < 500; ++yyy)
            //{
            //    dbCandle.RenderOneFrameToScreen(m_graph);
            //    graph.DrawImage(bmEmpty, left, top, maxWidth * magnification, maxHeight * magnification);
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (m_fUpdateFireDimensionsAfterNextFrame)
            //{
            //    UpdateFireDimensions();
            //    m_fUpdateFireDimensionsAfterNextFrame = false;
            //}

            // Draw the frame once per tick.
            // Select 32bit vs pallete color algorithm... and flame decay, intensity, color, etc...
            m_dbSprite.RenderOneFrameToScreen(m_graph);
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            int len = m_palette.Length;
            for (int i = 0; i < len / 2; ++i)
            {
                Color temp = m_palette[i];
                m_palette[i] = m_palette[len - i - 1];
                m_palette[len - i - 1] = temp;
            }
            //m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
            //m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true, f8: true);
        }
    }
}
