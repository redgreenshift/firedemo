using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
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
        List<SimpleSprite> m_dbSprites = new List<SimpleSprite>();
        private GenericRealtimeFlame m_genericFlame;
        int m_framesPerSecond = 64;

        public Form2()
        {
            InitializeComponent();
            this.Size = new Size(1024, 600); // Enlarge to the size of the Raspberry Pi device screen
            m_graph = this.CreateGraphics();
            this.Click += Form2_Click;
        }

        private void Form2_Click(object sender, EventArgs e)
        {
            // Click anywhere on the window to stop the render.
            if (timer1.Enabled)
                buttonDemo_Click(null, null);
        }

        // Cheap way to calculate framerate.
        // No matter how fast it goes, doesn't go above 65 FPS
        int iFrame = 0;
        DateTime dtEnd = DateTime.Now;
        private void UpdateFramerate()
        {
            ++iFrame;
            DateTime dtNow = DateTime.Now;

            if (dtNow >= dtEnd)
            {
                this.Text = string.Format("-- Friendly Neighborhood Status Indicator -- FPS: {0}", iFrame);
                iFrame = 0;
                dtEnd = DateTime.Now.AddSeconds(1);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Location = new Point(0, 0);
            this.BackColor = Color.DimGray;
            this.Text = "-- Friendly Neighborhood Status Indicator --";

            //SimpleCandle();
            DemoBatman(multithreaded: true);
            //DemoLightning();
            //DemoBorg();
            //DemoPlasmaRainbow();

            //buttonDemo_Click(null, null);
            //UpdateVisibleUI();
            //this.BackColor = Color.Black;
            //groupBox1.Hide();
            //RenderAtTopSpeed();
//            buttonDndStatus_Click(null, null);
        }
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

            AbstractRealtimeLightEffect dbPlasmaDiscBlue = new RealtimeLightning();
            dbPlasmaDiscBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlue.SetPalette(PalPlasma.New(Color.Blue));
            dbPlasmaDiscBlue.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlue.AddShape(lsBorgRing);

            // I really like this color, but it's more of a Cyan, instead of the deep BLUE I'm looking for
            // BUT, it might be hapfway between?
            AbstractRealtimeLightEffect dbPlasmaDiscBlueNICE = new RealtimeLightning();
            dbPlasmaDiscBlueNICE.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlueNICE.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlueNICE.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255)));
            dbPlasmaDiscBlueNICE.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlueNICE.AddShape(lsBorgRing);

            // This is very close to actual Cyan, but looks a tad too yellow
            AbstractRealtimeLightEffect dbPlasmaDiscSkyBlue = new RealtimeLightning();
            dbPlasmaDiscSkyBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscSkyBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscSkyBlue.SetPalette(PalPlasma.New(Color.DeepSkyBlue));
            dbPlasmaDiscSkyBlue.AddShape(lsBorgPlasma);
            dbPlasmaDiscSkyBlue.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteLightBlue = new RealtimeLightning();
            dbPlasmaDisc_FavoriteLightBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteLightBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteLightBlue.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255)));
            dbPlasmaDisc_FavoriteLightBlue.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 255, green: 255, blue: 255)));
            dbPlasmaDisc_FavoriteLightBlue.SetPalette(PalPlasma.New(Color.LightBlue)); // Update the generator if I improve the color
            //dbPlasmaDisc_FavoriteLightBlue.SetPalette(PalPlasma.New(Color.DarkBlue)); // Update the generator if I improve the color
            dbPlasmaDisc_FavoriteLightBlue.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteLightBlue.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_DarkBlue = new RealtimeLightning();
            dbPlasmaDisc_DarkBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_DarkBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_DarkBlue.SetPalette(PalPlasma.New(Color.DarkBlue));
            dbPlasmaDisc_DarkBlue.AddShape(lsBorgPlasma);
            dbPlasmaDisc_DarkBlue.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteBlue = new RealtimeLightning();
            dbPlasmaDisc_FavoriteBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteBlue.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 64, blue: 255),
                Color.FromArgb(red: 224, green: 224, blue: 255)));
            dbPlasmaDisc_FavoriteBlue.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteBlue.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteViolet = new RealtimeLightning();
            dbPlasmaDisc_FavoriteViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteViolet.SetPalette(PalPlasma.New(Color.Magenta));
            dbPlasmaDisc_FavoriteViolet.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteViolet.AddShape(lsBorgRing);

            AbstractRealtimeLightEffect dbPlasmaDisc_FavoriteDeepGreen = new RealtimeLightning();
            dbPlasmaDisc_FavoriteDeepGreen.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteDeepGreen.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteDeepGreen.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 222, blue: 0),
                Color.FromArgb(red: 222, green: 255, blue: 222)));
            dbPlasmaDisc_FavoriteDeepGreen.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 200, blue: 0),
                Color.FromArgb(red: 240, green: 240, blue: 240)));
            dbPlasmaDisc_FavoriteDeepGreen.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 0),
                Color.FromArgb(red: 255, green: 255, blue: 255)));
            //dbPlasmaDisc_FavoriteDeepGreen.SetPalette(PalPlasma.New(Color.DarkGreen));
            dbPlasmaDisc_FavoriteDeepGreen.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteDeepGreen.AddShape(lsBorgRing);
            //m_dbSprites.Add(dbPlasmaDisc_FavoriteDeepGreen);

            left = this.Width / 5;
            top = this.Height / 3 - 4;
            dbPlasmaDiscRed.Location = new Point(x: 0, y: top * 0);
            dbPlasmaDiscOrange.Location = new Point(x: left, y: top * 0);
            dbPlasmaDiscYellow.Location = new Point(x: left * 2, y: top * 0);
            dbPlasmaDiscGreen.Location = new Point(x: left * 3, y: top * 0);
            dbPlasmaDisc_FavoriteDeepGreen.Location = new Point(x: left * 4, y: top * 0);

            dbPlasmaDisc_FavoriteLightBlue.Location = new Point(x: left * 0, y: top * 1);
            dbPlasmaDiscBlue.Location = new Point(x: left * 1, y: top * 1);
            dbPlasmaDisc_DarkBlue.Location = new Point(x: left * 2, y: top * 1);

            dbPlasmaDiscDarkViolet.Location = new Point(x: left * 0, y: top * 2);
            dbPlasmaDisc_FavoriteViolet.Location = new Point(x: left * 1, y: top * 2);

            dbPlasmaDisc_FavoriteBlue.Location = new Point(x: left * 4, y: top);


            dbPlasmaDiscSkyBlue.Location = new Point(x: left * 0, y: top);
            dbPlasmaDiscBlueNICE.Location = new Point(x: left * 1, y: top);
            dbPlasmaDiscBlue_Raw.Location = new Point(x: left * 2, y: top);

            dbPlasmaDiscBlueViolet.Location = new Point(x: left * 2, y: top * 2);
            dbPlasmaDiscVioletEX1.Location = new Point(x: left * 3, y: top * 2);
            dbPlasmaDiscViolet_Raw.Location = new Point(x: left * 4, y: top * 2);

            // 1024 x 600
            // 1024 / 4 == 256

            m_dbSprites.Add(dbPlasmaDiscRed);
            m_dbSprites.Add(dbPlasmaDiscOrange);
            m_dbSprites.Add(dbPlasmaDiscYellow);
            m_dbSprites.Add(dbPlasmaDiscGreen); // GOOD
            m_dbSprites.Add(dbPlasmaDiscBlue); // GOOD
            m_dbSprites.Add(dbPlasmaDiscDarkViolet); // GOOD
            m_dbSprites.Add(dbPlasmaDisc_FavoriteViolet); // GOOD Violet(basic/light)
            m_dbSprites.Add(dbPlasmaDisc_FavoriteLightBlue);
            m_dbSprites.Add(dbPlasmaDisc_DarkBlue);

            //m_dbSprites.Add(dbPlasmaDisc_FavoriteBlue);

            //m_dbSprites.Add(dbPlasmaDiscBlue_Raw);
            //m_dbSprites.Add(dbPlasmaDiscViolet_Raw);
            //m_dbSprites.Add(dbPlasmaDiscBlueViolet);
            //m_dbSprites.Add(dbPlasmaDiscVioletEX1);
            //m_dbSprites.Add(dbPlasmaDiscBlueNICE);
            //m_dbSprites.Add(dbPlasmaDisc_FavoriteBlue);

            foreach (SimpleSprite db in m_dbSprites)
            {
                // This is SLOWER than SourceCopy, but necessary
                // to be able to render multiple plasma discs in Linux
                db.CompositingMode = CompositingMode.SourceOver;
            }
            //m_dbSprites[0].CompositingMode = CompositingMode.SourceCopy; // let the first one draw faster (NO! It blinks if we do this!)
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
            palBorg = PalPlasma.New(Color.DarkViolet);
            palBorg = PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 200, blue: 0),
                Color.FromArgb(red: 200, green: 200, blue: 200));
            palBorg = PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255));
            palBorg = PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 224, green: 224, blue: 255));
            palBorg = PalPlasma.New(Color.Blue);

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
            palLightning = PalLightning.New(Color.Red);
            palLightning = PalLightning.New(Color.Orange);
            palLightning = PalLightning.New(Color.Yellow);
            palLightning = PalLightning.New(Color.Green);
            palLightning = PalLightning.New(Color.Blue);
            palLightning = PalLightning.New(Color.Violet);
            //palLightning = PalLightning.New();
            palLightning = PalLightning.New(Color.Orange);
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

        private void DemoBatman(bool multithreaded = false)
        {
            m_dbSprites.Clear();
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

            Color[] palFire = PalRealisticFire.New();
            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.4f, min: 5, max: 7, smoothing: 0, shift: true, rotate: false);
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 15, smoothing: 0, shift: true, rotate: false);
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 15, smoothing: 0, shift: true, rotate: false);
            ////m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 25, smoothing: 0, shift: true, rotate: false);
            coolingStrategy = m_coolingStrategy;
            //coolingStrategy = new CoolingStrategyConst(3);
            ILightPen lpBatman = new LightPen(fill: 0.7f, min: 54, max: 255, useFullRange: false);
            ILightShape lsBatman = new LightShapeBatman();
            lsBatman.SetPen(lpBatman);

            AbstractRealtimeLightEffect dbBatman;
            if (multithreaded)
                dbBatman = new RealtimeFireBatLogoOptimizedMT_ThreadPool();
            //dbBatman = new RealtimeFireBatLogoOptimizedMT_ManualLongThreads();
            //dbBatman = new RealtimeFireBatLogoOptimizedMT_NaiveSubclass();
            //dbBatman = new RealtimeFireBatLogoOptimizedMT_Naive();
            else
                dbBatman = new RealtimeFireBatLogoOptimized();
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
            m_dbSprite.CompositingMode = CompositingMode.SourceCopy; // This is FASTER than SourceOver (only use this for SINGLE bitmap scenarios!)
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

            Color[] palCandle = PalRealisticFire.New();
            palCandle = PalFlatPalette.New(Color.Orange);
            palCandle = PalRealisticFlameCurve.New(Color.White);
            palCandle = PalRealisticFlameCurve.New(Color.Black);
            //palCandle = PalFourPointLinear.New(Color.Black, Color.Orange, Color.Yellow, Color.Blue);

            Color c1 = Color.FromArgb(0, 0, 0);       // Black
            Color c2 = Color.FromArgb(255, 185, 0);   // Orange
            Color c3 = Color.FromArgb(255, 255, 127); // Bright Yellow
            Color c4 = Color.FromArgb(212, 212, 255); // Light Blue
            palCandle = PalFourPointLinear.New(c1, c2, c3, c4);

            //palCandle = PalLightning.New(Color.Yellow); // A way to test the lightning algorithm is to see how it looks in candle form

            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight,
                density: 0.4f, min: 5, max: 13, smoothing: 5,
                shift: true, rotate: true);
            coolingStrategy = m_coolingStrategy;

            ILightPen lpCandle = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
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

            UpdateVisibleUI();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateFramerate();
            //if (m_fUpdateFireDimensionsAfterNextFrame)
            //{
            //    UpdateFireDimensions();
            //    m_fUpdateFireDimensionsAfterNextFrame = false;
            //}

            // Draw the frame once per tick.

            if (m_dbSprites.Count > 0)
            {
                foreach (SimpleSprite sprite in m_dbSprites)
                {
                    sprite.RenderOneFrameToScreen(m_graph);
                }
            }
            else
                m_dbSprite.RenderOneFrameToScreen(m_graph);
        }


#if false
        struct TheModes
        {
            public TheModes(string s, InterpolationMode m)
            {
                name = s;
                mode = m;
            }
            public string name;
            public InterpolationMode mode;

        };

        private void ChangeMode()
        {
            TheModes[] modes = {
                new TheModes("NearestNeighbor", InterpolationMode.NearestNeighbor), // PI (YES)
                new TheModes("Low", InterpolationMode.Low), // PI (NO)
                new TheModes("Default", InterpolationMode.Default), // PI (YES, probably Bilinear)
                new TheModes("High", InterpolationMode.High), // PI (NO)
                new TheModes("Bilinear", InterpolationMode.Bilinear), // PI (Yes)
                new TheModes("HighQualityBilinear", InterpolationMode.HighQualityBilinear), // PI (NO)
                new TheModes("Bicubic", InterpolationMode.Bicubic), // PI (YES!)
                new TheModes("HighQualityBicubic", InterpolationMode.HighQualityBicubic), // PI (NO)
            };

            int ii;
            for (ii = 0; ii < modes.Length; ++ii)
            {
                if (m_dbSprite.InterpolationMode == modes[ii].mode)
                    break;
            }
            ++ii;
            if (ii == modes.Length)
                ii = 0;
            buttonChange.Text = modes[ii].name;
            m_dbSprite.InterpolationMode = modes[ii].mode;
        }
#endif
        bool m_largerFlame = false;
        /// <summary>
        /// Just a proof-of-concept that I can still change things on the fly, even in the refactored form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChange_Click(object sender, EventArgs e)
        {
            m_largerFlame = !m_largerFlame;
            if (m_largerFlame)
                m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
            else
                m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true, f8: true);

            UpdateVisibleUI();
        }

        private void UpdateVisibleUI()
        {
            if (timer1.Enabled)
            {
                this.FormBorderStyle = FormBorderStyle.None; // Hide the Title Bar and other UI
                this.Location = new Point(0, 0);
                groupBox1.Hide();
                buttonChange.Hide();
                buttonDemo.Hide();
            }
            else
            {
                Cursor.Show();
                Cursor = Cursors.Default;
                this.FormBorderStyle = FormBorderStyle.Sizable; // Show the Title Bar
                groupBox1.Show();
                buttonChange.Show();
                buttonDemo.Show();
            }
        }
        private void buttonAwayStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            //SimpleCandle();
            int fireWidth = 21;
            int fireHeight = 75;
            int magnification = 5;
            int top = buttonDemo.Location.Y + buttonDemo.Size.Height;
            int left = buttonDemo.Location.X + buttonDemo.Size.Width;

            Color[] palCandle = PalRealisticFire.New();
            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight,
                density: 0.4f, min: 5, max: 13, smoothing: 5,
                shift: true, rotate: true);
            coolingStrategy = m_coolingStrategy;

            ILightPen lpCandle = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
            ILightShape lsCandle = new LightShapeCandle();
            lsCandle.SetPen(lpCandle);

            AbstractRealtimeLightEffect dbCandle = new RealtimeCandleflame();
            m_genericFlame = new GenericRealtimeFlame();
            m_largerFlame = true;
            m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
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

            TextSprite tsAway = new TextSprite
            {
                Location = new Point(0, 0),
                Text = "Away",
                Color = Color.Yellow,
            };
            m_dbSprites.Add(dbCandle);
            m_dbSprites.Add(tsAway);

            buttonDemo_Click(sender, e);
        }

        private void buttonOofStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoLightning();

            m_dbSprites.Add(m_dbSprite);
            TextSprite text = new TextSprite
            {
                Text = "I'm OOF",
                Location = new Point(300, 0),
                Color = Color.DarkMagenta
            };
            m_dbSprites.Add(text);

            buttonDemo_Click(sender, e);
        }

        private void buttonBusyStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            UpdateVisibleUI();

            DemoBatman(multithreaded: true);

            m_dbSprites.Add(m_dbSprite);
            TextSprite text = new TextSprite
            {
                Text = "I'm Busy",
                Location = new Point(120, 0),
                Color = Color.Red
            };
            m_dbSprites.Add(text);

            buttonDemo_Click(sender, e);
        }

        private void buttonAvailableStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            //DemoBorg();

            int ringWidth = 129;
            int ringHeight = 131;
            int magnification = 2;
            //fireWidth = 200;
            //fireHeight = 200;
            magnification = 3;
            int left, top;

            Color[] palBorg = PalPlasma.New(Color.Green);

            ICoolingStrategy coolingStrategy;
            //coolingStrategy = new CoolingStrategyConst(7);
            // Varying the density showed no improvement over constant cooling (for Plasma)
            //m_coolingStrategy = new CoolingStrategyMap();
            //m_coolingStrategy.SetMapParameters(width: ringWidth, height: ringHeight,
            //    density: 1.0f, min: 10, max: 13, smoothing: 0,
            //    shift: false, rotate: false);
            //coolingStrategy = m_coolingStrategy;
            coolingStrategy = new CoolingStrategyConst(11); // 11 looks better. Looks faster than 7
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


            m_dbSprites.Add(m_dbSprite);
            TextSprite text = new TextSprite
            {
                Text = "Available",
                Location = new Point(120, 0),
                Color = Color.LightGreen
            };
            m_dbSprites.Add(text);

            buttonDemo_Click(sender, e);
        }

        private void buttonDndStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            //DemoBorg();
            int ringWidth = 129;
            int ringHeight = 131;
            ringWidth = 129 / 2;
            ringHeight = 131 / 2;
            int magnification = 2;
            //fireWidth = 200;
            //fireHeight = 200;
            magnification = 1;
            int left, top;

            Color[] palBorg = PalPlasma.New(Color.Red);
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(11);
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
            top = (this.Height - ringHeight * magnification) / 2 + 50;
            dbPlasmaDisc.Location = new Point(x: left, y: top);

            LinkHoldingItem link = new LinkHoldingItem();
            link.Magnification = 3;
            left = dbPlasmaDisc.Location.X;
            top = dbPlasmaDisc.Location.Y;
            left +=   (ringWidth / 2) - (link.Width * link.Magnification / 2);
            top += ringHeight * magnification;
            link.Location = new Point(left, top);
            link.InterpolationMode = InterpolationMode.NearestNeighbor;


            OldMan man = new OldMan();
            man.Magnification = 3;
            man.InterpolationMode = InterpolationMode.NearestNeighbor;
            left = dbPlasmaDisc.Location.X;
            top = dbPlasmaDisc.Location.Y;
            left += (ringWidth / 2) - (man.Width * man.Magnification / 2);
            top -= 2 * man.Height * man.Magnification;
            man.Location = new Point(left, top);
            m_dbSprites.Add(man);


            m_palette = palBorg;
            m_lightPen = lpPlasma;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsBorgPlasma);
            m_lightShapes.Add(lsBorgRing);
            m_dbSprites.Add(dbPlasmaDisc);

            m_dbSprites.Add(link);

            //BrickWall brick = new BrickWall();
            //brick.InterpolationMode = InterpolationMode.NearestNeighbor;
            //brick.Magnification = 2;
            //int fudgeFactor = brick.Magnification / 2;
            //m_dbSprites.Add(brick);
            //BrickWall brick2 = new BrickWall();
            //brick2.InterpolationMode = InterpolationMode.NearestNeighbor;
            //brick2.Magnification = brick.Magnification;
            //brick2.Location = new Point(brick.Width * brick.Magnification - fudgeFactor, 0);
            //m_dbSprites.Add(brick2);
            //BrickWall brick3 = new BrickWall();
            //brick3.InterpolationMode = InterpolationMode.NearestNeighbor;
            //brick3.Magnification = brick.Magnification;
            //brick3.Location = new Point(0, brick.Height * brick.Magnification - fudgeFactor);
            //m_dbSprites.Add(brick3);
            //BrickWall brick4 = new BrickWall();
            //brick4.InterpolationMode = InterpolationMode.NearestNeighbor;
            //brick4.Magnification = brick.Magnification;
            //brick4.Location = new Point(brick.Width * brick.Magnification - fudgeFactor, brick.Height * brick.Magnification - fudgeFactor);
            //m_dbSprites.Add(brick4);


            BrickWall brick1 = new BrickWall();
            brick1.InterpolationMode = InterpolationMode.NearestNeighbor;
            brick1.Magnification = 2;
            SpriteCompositor brickWall = new SpriteCompositor();
            brickWall.Sprite = brick1;
            brickWall.Initialize();
            m_dbSprites.Add(brickWall);


            int fireWidth = 20;
            int fireHeight = 30;
            int fireMagnification = 2;

            CauldronBase cauldron1 = new CauldronBase();
            cauldron1.Location = new Point(man.Location.X - man.Width * man.Magnification * 3, man.Location.Y + man.Height * man.Magnification);
            cauldron1.Magnification = 5;
            cauldron1.InterpolationMode = InterpolationMode.NearestNeighbor;
            m_dbSprites.Add(cauldron1);

            Color[] palCandle = PalRealisticFire.New();
            CoolingStrategyMap csBonfire1 = new CoolingStrategyMap();
            csBonfire1.SetMapParameters(width: fireWidth, height: fireHeight,
                density: 0.6f, min: 8, max: 16, smoothing: 0,
                shift: true, rotate: true);

            ILightPen lpCandle = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
            ILightShape lsCandle = new LightShapeCandle();
            lsCandle.SetPen(lpCandle);

            AbstractRealtimeLightEffect dbCandle = new RealtimeFire();
            dbCandle.Initialize(fireWidth, fireHeight, fireMagnification);
            dbCandle.SetCoolingStrategy(csBonfire1);
            dbCandle.SetPalette(palCandle);
            dbCandle.AddShape(lsCandle);
            left = cauldron1.Location.X;
            top = cauldron1.Location.Y - dbCandle.Height * dbCandle.Magnification + 0;
            dbCandle.Location = new Point(x: left, y: top);
            m_dbSprites.Add(dbCandle);


            CauldronBase cauldron2 = new CauldronBase();
            cauldron2.Location = new Point(man.Location.X + man.Width * man.Magnification * 3, man.Location.Y + man.Height * man.Magnification);
            cauldron2.Magnification = cauldron1.Magnification;
            cauldron2.InterpolationMode = InterpolationMode.NearestNeighbor;
            m_dbSprites.Add(cauldron2);

            AbstractRealtimeLightEffect dbCandle2 = new RealtimeFire();
            CoolingStrategyMap csBonfire2 = new CoolingStrategyMap();
            csBonfire2.SetMapParameters(width: fireWidth, height: fireHeight,
                density: 0.6f, min: 8, max: 16, smoothing: 0,
                shift: true, rotate: true);
            dbCandle2.Initialize(fireWidth, fireHeight, fireMagnification);
            dbCandle2.SetCoolingStrategy(csBonfire2);
            dbCandle2.SetPalette(palCandle);
            dbCandle2.AddShape(lsCandle);
            left = cauldron2.Location.X;
            top = cauldron2.Location.Y - dbCandle2.Height * dbCandle2.Magnification + 0;
            dbCandle2.Location = new Point(x: left, y: top);
            m_dbSprites.Add(dbCandle2);


            TextSprite text = new TextSprite
            {
                Text = "It's dangerous to bother me!\nGO AWAY! (use email)",
                Location = new Point(man.Location.X - 120, man.Location.Y - 75)
            };
            m_dbSprites.Add(text);

            foreach (SimpleSprite db in m_dbSprites)
            {
                // This is SLOWER than SourceCopy, but necessary
                // to be able to render multiple plasma discs in Linux
                db.CompositingMode = CompositingMode.SourceOver;
            }
            // Commenting out. This is why it blinks on the Pi.
            //m_dbSprites[0].CompositingMode = CompositingMode.SourceCopy; // let the first one draw faster (NO! It blinks if we do this!)

            buttonDemo_Click(sender, e);
        }

        private void buttonRainBORG_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoPlasmaRainbow();
            buttonDemo_Click(sender, e);
        }

        private void buttonBatmanMultiThread_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoBatman(multithreaded: false);
            buttonDemo_Click(sender, e);
        }
    }
}
