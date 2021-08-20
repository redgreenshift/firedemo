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
        CoolingStrategyMap m_coolingStrategy;
        ILightPen m_lightPen;
        List<ILightShape> m_lightShapes = new List<ILightShape>();
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
            SimpleCandle();
            DemoBatman();
            //DemoLightning();
            //DemoBorg();
        }

        private void DemoBorg()
        {
            int fireWidth = 129;
            int fireHeight = 131;
            int magnification = 2;
            int left, top;

            Color[] palBorg = PaletteGenerator.GetHardCodedBorgPalette();
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(7);
            ILightPen lpPlasma = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsBorgRing = new LightShapeBorgRing();
            ILightShape lsBorgPlasma = new LightShapeBorgPlasma();
            lsBorgRing.SetPen(lpPlasma);
            lsBorgPlasma.SetPen(lpPlasma);

            AbstractRealtimeLightEffect dbPlasmaDisc = new RealtimeLightning();
            dbPlasmaDisc.Initialize(fireWidth, fireHeight, magnification);
            dbPlasmaDisc.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc.SetPalette(palBorg);
            dbPlasmaDisc.AddShape(lsBorgPlasma);
            dbPlasmaDisc.AddShape(lsBorgRing);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2;
            dbPlasmaDisc.Location = new Point(x: left, y: top);

            m_palette = palBorg;
            m_lightPen = lpPlasma;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsBorgPlasma);
            m_lightShapes.Add(lsBorgRing);
            m_dbSprite = dbPlasmaDisc;
        }

        private void DemoLightning()
        {
            int fireWidth = 132;
            int fireHeight = 200;
            int magnification = 2;
            int left, top;

            Color[] palLightning = PaletteGenerator.GetHardCodedLightningPalette();
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(27);
            ILightPen lpLightning = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsLightning = new LightShapeLightning();
            lsLightning.SetPen(lpLightning);

            AbstractRealtimeLightEffect dbLightning = new RealtimeLightning();
            dbLightning.Initialize(fireWidth, fireHeight, magnification);
            dbLightning.SetCoolingStrategy(coolingStrategy);
            dbLightning.SetPalette(palLightning);
            dbLightning.AddShape(lsLightning);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2;
            dbLightning.Location = new Point(x: left, y: top);

            m_palette = palLightning;
            m_lightPen = lpLightning;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsLightning);
            m_dbSprite = dbLightning;
        }

        private void DemoBatman()
        {
            int left, top;
            int fireWidth = 500;
            int fireHeight = 300;
            int magnification = 1;
            //fireWidth = 250;
            //fireHeight = 150;
            //magnification = 2;

            Color[] palFire = PaletteGenerator.GetHardCodedFirePalette();
            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.4f, min: 5, max: 7, smoothing: 0);
            coolingStrategy = m_coolingStrategy;
            ILightPen lpBatman = new LightPen(fill: 0.45f, min: 54, max: 255, useFullRange: false);
            ILightShape lsBatman = new LightShapeBatman();
            lsBatman.SetPen(lpBatman);

            AbstractRealtimeLightEffect dbBatman = new RealtimeFireBatLogoOptimized();
            dbBatman.Initialize(fireWidth, fireHeight, magnification);
            dbBatman.SetCoolingStrategy(coolingStrategy);
            dbBatman.SetPalette(palFire);
            dbBatman.AddShape(lsBatman);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2;
            dbBatman.Location = new Point(x: left, y: top);

            m_palette = palFire;
            m_lightPen = lpBatman;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsBatman);
            m_dbSprite = dbBatman;
        }

        private void SimpleCandle()
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

            Color[] palCandle = PaletteGenerator.GetHardCodedFirePalette();

            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(fireWidth, fireHeight,
                rotate: true, shift: true,
                density: 0.4f, min: 5, max: 13, smoothing: 5);
            coolingStrategy = m_coolingStrategy;

            ILightPen lpCandle = new LightPen(fill: 1.0f, min: 54, max: 255, useFullRange: true);
            ILightShape lsCandle = new LightShapeCandle();
            lsCandle.SetPen(lpCandle);

            AbstractRealtimeLightEffect dbCandle = new RealtimeCandleflame();
            m_genericFlame = new GenericRealtimeFlame();
            dbCandle = m_genericFlame;
            dbCandle.Initialize(fireWidth, fireHeight, magnification);
            dbCandle.SetCoolingStrategy(coolingStrategy);
            dbCandle.SetPalette(palCandle);
            dbCandle.AddShape(lsCandle);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2;
            dbCandle.Location = new Point(x: left, y: top);

            m_palette = palCandle;
            m_lightPen = lpCandle;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsCandle);
            m_dbSprite = dbCandle;
        }

        private void buttonDemo_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                //UpdateFireDimensions();

                //if (rotateCoolingMapCheckBox.Checked)
                //    UpdateRotatingCoolingMap();

                this.BackColor = Color.Black;
                buttonDemo.Text = "Stop!";
                timer1.Interval = (int)(1000 / m_framesPerSecond);
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                buttonDemo.Text = "Start!";
                this.BackColor = Color.DarkGray;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (m_fUpdateFireDimensionsAfterNextFrame)
            //{
            //    UpdateFireDimensions();
            //    m_fUpdateFireDimensionsAfterNextFrame = false;
            //}

            // Draw the frame once per tick.
            m_dbSprite.RenderOneFrameToScreen(m_graph);
        }


        bool m_largerFlame = false;
        /// <summary>
        /// Just a proof-of-concept that I can still change things on the fly, even in the refactored form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChange_Click(object sender, EventArgs e)
        {
            //int len = m_palette.Length;
            //for (int i = 0; i < len / 2; ++i)
            //{
            //    Color temp = m_palette[i];
            //    m_palette[i] = m_palette[len - i - 1];
            //    m_palette[len - i - 1] = temp;
            //}
            m_largerFlame = !m_largerFlame;
            if (m_largerFlame)
                m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
            else
                m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true, f8: true);
        }
    }
}
