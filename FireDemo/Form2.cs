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
        List<AbstractDynamicSprite> m_dbSprites = new List<AbstractDynamicSprite>();
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
            //DemoBatman();
            //DemoLightning();
            DemoBorg();
            DemoPlasmaRainbow();

            buttonDemo_Click(null, null);
        }

        private void DemoPlasmaRainbow()
        {
            int ringWidth = 129;
            int ringHeight = 131;
            int magnification = 1;
            //fireWidth = 200;
            //fireHeight = 200;
            int left, top;

            Color[] palBorgRed = PalPlasma.New(Color.Orange);
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(7);
            ILightPen lpPlasma = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsBorgRing = new LightShapeBorgRing();
            ILightShape lsBorgPlasma = new LightShapeBorgPlasma();
            lsBorgRing.SetPen(lpPlasma);
            lsBorgPlasma.SetPen(lpPlasma);

            AbstractRealtimeLightEffect dbPlasmaDiscRed = new RealtimeLightning();
            dbPlasmaDiscRed.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscRed.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscRed.SetPalette(PalPlasma.New(Color.Red));
            dbPlasmaDiscRed.AddShape(lsBorgPlasma);
            dbPlasmaDiscRed.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscOrange = new RealtimeLightning();
            dbPlasmaDiscOrange.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscOrange.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscOrange.SetPalette(PalPlasma.New(Color.Orange));
            dbPlasmaDiscOrange.AddShape(lsBorgPlasma);
            dbPlasmaDiscOrange.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscYellow = new RealtimeLightning();
            dbPlasmaDiscYellow.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscYellow.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscYellow.SetPalette(PalPlasma.New(Color.Yellow));
            dbPlasmaDiscYellow.AddShape(lsBorgPlasma);
            dbPlasmaDiscYellow.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscGreen = new RealtimeLightning();
            dbPlasmaDiscGreen.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscGreen.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscGreen.SetPalette(PalPlasma.New(Color.Green));
            dbPlasmaDiscGreen.AddShape(lsBorgPlasma);
            dbPlasmaDiscGreen.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscBlue_Raw = new RealtimeLightning();
            dbPlasmaDiscBlue_Raw.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlue_Raw.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlue_Raw.SetPalette(PalPlasma.NewRaw(Color.Blue));
            dbPlasmaDiscBlue_Raw.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlue_Raw.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscViolet_Raw = new RealtimeLightning();
            dbPlasmaDiscViolet_Raw.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscViolet_Raw.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscViolet_Raw.SetPalette(PalPlasma.NewRaw(Color.Violet));
            dbPlasmaDiscViolet_Raw.AddShape(lsBorgPlasma);
            dbPlasmaDiscViolet_Raw.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscVioletEX1 = new RealtimeLightning();
            dbPlasmaDiscVioletEX1.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscVioletEX1.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscVioletEX1.SetPalette(PalPlasma.NewRaw(Color.Violet));
            dbPlasmaDiscVioletEX1.AddShape(lsBorgPlasma);
            dbPlasmaDiscVioletEX1.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscDarkViolet = new RealtimeLightning();
            dbPlasmaDiscDarkViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscDarkViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscDarkViolet.SetPalette(PalPlasma.New(Color.DarkViolet));
            dbPlasmaDiscDarkViolet.AddShape(lsBorgPlasma);
            dbPlasmaDiscDarkViolet.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscBlueViolet = new RealtimeLightning();
            dbPlasmaDiscBlueViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlueViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlueViolet.SetPalette(PalPlasma.New(Color.BlueViolet));
            dbPlasmaDiscBlueViolet.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlueViolet.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscBlueEX1 = new RealtimeLightning();
            dbPlasmaDiscBlueEX1.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlueEX1.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlueEX1.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 32, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255)));
            dbPlasmaDiscBlueEX1.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlueEX1.AddShape(lsBorgRing);

            // I really like this color, but it's more of a Cyan, instead of the deep BLUE I'm looking for
            AbstractRealtimeLightEffect dbPlasmaDiscBlueNICE = new RealtimeLightning();
            dbPlasmaDiscBlueNICE.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlueNICE.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlueNICE.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255)));
            dbPlasmaDiscBlueNICE.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlueNICE.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDiscSkyBlue = new RealtimeLightning();
            dbPlasmaDiscSkyBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscSkyBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscSkyBlue.SetPalette(PalPlasma.New(Color.DeepSkyBlue));
            dbPlasmaDiscSkyBlue.AddShape(lsBorgPlasma);
            dbPlasmaDiscSkyBlue.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteBlue = new RealtimeLightning();
            dbPlasmaDisc_FavoriteBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteBlue.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255)));
            dbPlasmaDisc_FavoriteBlue.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteBlue.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteViolet = new RealtimeLightning();
            dbPlasmaDisc_FavoriteViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteViolet.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 255, green: 0, blue: 255),
                Color.FromArgb(red: 255, green: 255, blue: 255)));
            dbPlasmaDisc_FavoriteViolet.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteViolet.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteDeepGreen = new RealtimeLightning();
            dbPlasmaDisc_FavoriteDeepGreen.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteDeepGreen.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteDeepGreen.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 222, blue: 0),
                Color.FromArgb(red: 222, green: 255, blue: 222)));
            dbPlasmaDisc_FavoriteDeepGreen.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteDeepGreen.AddShape(lsBorgRing);

            left = this.Width / 5;
            top = this.Height / 3 - 4;
            dbPlasmaDiscRed.Location = new Point(x: 0, y: 0);
            dbPlasmaDiscOrange.Location = new Point(x: left, y: 0);
            dbPlasmaDiscYellow.Location = new Point(x: left * 2, y: 0);
            dbPlasmaDiscGreen.Location = new Point(x: left * 3, y: 0);
            dbPlasmaDisc_FavoriteDeepGreen.Location = new Point(x: left * 4, y: 0);

            dbPlasmaDiscBlue_Raw.Location = new Point(x: 0, y: top);
            dbPlasmaDiscBlueEX1.Location = new Point(x: left, y: top);
            dbPlasmaDiscBlueNICE.Location = new Point(x: left * 2, y: top);
            dbPlasmaDiscSkyBlue.Location = new Point(x: left * 3, y: top);
            dbPlasmaDisc_FavoriteBlue.Location = new Point(x: left * 4, y: top);

            dbPlasmaDiscViolet_Raw.Location = new Point(x: 0, y: top * 2);
            dbPlasmaDiscVioletEX1.Location = new Point(x: left, y: top * 2);
            dbPlasmaDiscBlueViolet.Location = new Point(x: left * 2, y: top * 2);
            dbPlasmaDiscDarkViolet.Location = new Point(x: left * 3, y: top * 2);
            dbPlasmaDisc_FavoriteViolet.Location = new Point(x: left * 4, y: top * 2);

            // 1024 x 600
            // 1024 / 4 == 256

            m_dbSprites.Add(dbPlasmaDiscRed);
            m_dbSprites.Add(dbPlasmaDiscOrange);
            m_dbSprites.Add(dbPlasmaDiscYellow);
            m_dbSprites.Add(dbPlasmaDiscGreen);
            m_dbSprites.Add(dbPlasmaDiscBlue_Raw);
            m_dbSprites.Add(dbPlasmaDiscViolet_Raw);
            m_dbSprites.Add(dbPlasmaDiscDarkViolet);
            m_dbSprites.Add(dbPlasmaDiscBlueViolet);
            m_dbSprites.Add(dbPlasmaDiscVioletEX1);
            m_dbSprites.Add(dbPlasmaDiscBlueEX1);
            m_dbSprites.Add(dbPlasmaDiscBlueNICE);
            m_dbSprites.Add(dbPlasmaDiscSkyBlue);
            m_dbSprites.Add(dbPlasmaDisc_FavoriteBlue);
            m_dbSprites.Add(dbPlasmaDisc_FavoriteViolet);
            m_dbSprites.Add(dbPlasmaDisc_FavoriteDeepGreen);
        }

        private void DemoBorg()
        {
            int ringWidth = 129;
            int ringHeight = 131;
            int magnification = 2;
            //fireWidth = 200;
            //fireHeight = 200;
            magnification = 2;
            int left, top;

            Color[] palBorg = PaletteGenerator.GetHardCodedBorgPalette();
            palBorg = PalPlasma.New(Color.Orange);
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(7);
            ILightPen lpPlasma = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsBorgRing = new LightShapeBorgRing();
            ILightShape lsBorgPlasma = new LightShapeBorgPlasma();
            lsBorgRing.SetPen(lpPlasma);
            lsBorgPlasma.SetPen(lpPlasma);

            AbstractRealtimeLightEffect dbPlasmaDisc = new RealtimeLightning();
            dbPlasmaDisc.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc.SetPalette(palBorg);
            dbPlasmaDisc.AddShape(lsBorgPlasma);
            dbPlasmaDisc.AddShape(lsBorgRing);
            left = (this.Width - ringWidth * magnification) / 2;
            top = (this.Height - ringHeight * magnification) / 2;
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
            //fireWidth = 700;
            fireHeight = fireWidth * 3 / 5;
            //magnification = 2;

            Color[] palFire = PaletteGenerator.GetHardCodedFirePalette();
            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.4f, min: 5, max: 7, smoothing: 0);
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 15, smoothing: 0);
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 25, smoothing: 0);
            ////m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 25, smoothing: 0);
            coolingStrategy = m_coolingStrategy;
            ILightPen lpBatman = new LightPen(fill: 0.45f, min: 54, max: 255, useFullRange: false);
            ILightShape lsBatman = new LightShapeBatman();
            lsBatman.SetPen(lpBatman);

            AbstractRealtimeLightEffect dbBatman = new RealtimeFireBatLogoOptimized();
            //dbBatman = new RealtimeCandleflame();
            dbBatman.Initialize(fireWidth, fireHeight, magnification);
            dbBatman.SetCoolingStrategy(coolingStrategy);
            dbBatman.SetPalette(palFire);
            dbBatman.AddShape(lsBatman);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2 - 20;
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
            //Bitmap bmEmpty = new Bitmap(fireWidth, fireHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //Random rng = new Random();

            //for (int x = 0; x < maxWidth; ++x)
            //    for (int y = 0; y < maxHeight; ++y)
            //        bmEmpty.SetPixel(x, y, Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255)));

            //graph.DrawImage(bmEmpty, left, top, maxWidth*magnification, maxHeight * magnification);

            Color[] palCandle = PaletteGenerator.GetHardCodedFirePalette();
            palCandle = PalFlatPalette.New(Color.Orange);
            palCandle = PalRealisticFlameCurve.New(Color.White);
            palCandle = PalRealisticFlameCurve.New(Color.Black);
            //palCandle = PalFourPointLinear.New(Color.Black, Color.Orange, Color.Yellow, Color.Blue);

            Color c1 = Color.FromArgb(0, 0, 0);       // Black
            Color c2 = Color.FromArgb(255, 185, 0);   // Orange
            Color c3 = Color.FromArgb(255, 255, 127); // Bright Yellow
            Color c4 = Color.FromArgb(212, 212, 255); // Light Blue
            palCandle = PalFourPointLinear.New(c1, c2, c3, c4);

            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight,
                density: 0.4f, min: 5, max: 13, smoothing: 5,
                shift: true, rotate: true);
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
                this.BackColor = Color.DimGray;
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

            if (m_dbSprites.Count > 0)
            {
                foreach (AbstractDynamicSprite sprite in m_dbSprites)
                {
                    sprite.RenderOneFrameToScreen(m_graph);
                }
            }
            else
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
