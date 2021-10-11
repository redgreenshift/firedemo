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
        Graphics m_graph = null;
        Color[] m_palette;
        CoolingStrategyMap m_coolingStrategy;
        ILightPen m_lightPen;
        List<ILightShape> m_lightShapes = new List<ILightShape>();
        DynamicSprite m_dbSprite;
        TextSprite m_dbText;
        List<SimpleSprite> m_dbSprites = new List<SimpleSprite>();
        private GenericRealtimeFlame m_genericFlame;
        readonly int m_framesPerSecond = 64;
        readonly int SecondsBeforeMovingTextAround = 15;
        readonly bool m_fHideTitlebarOnDemo = true;

        // Hacky solution. Windows auto scales stuff,
        // so that I need to undo it when running on Linux,
        // but C# doesn't make it easy to figure out the scale factor
        //float m_scaleFactor = 1.0f; // Windows
        readonly float m_scaleFactor = 1.25f; // Linux
        int Scaled(int i) => (int)(i * m_scaleFactor); // Do we still need this?
        Action m_callbackToChangeStuff = null;

        public Form2()
        {
            InitializeComponent();
            this.Size = new Size(1024, 600); // Enlarge to the size of the Raspberry Pi device screen
            this.Click += Form2_Click;
            if (Util.IsLinux)
            {
                this.WindowState = FormWindowState.Maximized;
                m_scaleFactor = 1.25f;
            }
            else
                m_scaleFactor = 1.0f;

            buttonDemo.Hide();
            buttonChange.Hide();
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
            this.CancelButton = buttonQuit;

            //SimpleCandle();
            //DemoBatman(multithreaded: true);
            //DemoLightning();
            //DemoBorg();
            //DemoPlasmaRainbow();

            //buttonDemo_Click(null, null);
            //UpdateVisibleUI();
            //this.BackColor = Color.Black;
            //groupBox1.Hide();
            //RenderAtTopSpeed();
            //buttonDndStatus_Click(null, null);
            //buttonRainbowFire_Click(null, null);
            //buttonAwayStatus_Click(null, null);
        }


        private void DemoPlasmaRainbow()
        {
            int ringWidth = 129;
            int ringHeight = 131;
            int magnification = 1;
            //ringWidth = 200;
            //ringHeight = 200;
            int left, top;

            Color[] palBorgRed = PalPlasma.New(Color.Orange);
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(7);
            ILightPen lpPlasma = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsBorgRing = new LightShapeBorgRing();
            ILightShape lsBorgPlasma = new LightShapeBorgPlasma();
            lsBorgRing.SetPen(lpPlasma);
            lsBorgPlasma.SetPen(lpPlasma);

            RealtimeLightEffect dbPlasmaDiscRed = new RealtimeLightning();
            dbPlasmaDiscRed.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscRed.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscRed.SetPalette(PalPlasma.New(Color.Red));
            dbPlasmaDiscRed.AddShape(lsBorgPlasma);
            dbPlasmaDiscRed.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscOrange = new RealtimeLightning();
            dbPlasmaDiscOrange.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscOrange.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscOrange.SetPalette(PalPlasma.New(Color.Orange));
            dbPlasmaDiscOrange.AddShape(lsBorgPlasma);
            dbPlasmaDiscOrange.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscYellow = new RealtimeLightning();
            dbPlasmaDiscYellow.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscYellow.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscYellow.SetPalette(PalPlasma.New(Color.Yellow));
            dbPlasmaDiscYellow.AddShape(lsBorgPlasma);
            dbPlasmaDiscYellow.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscGreen = new RealtimeLightning();
            dbPlasmaDiscGreen.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscGreen.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscGreen.SetPalette(PalPlasma.New(Color.Green));
            dbPlasmaDiscGreen.AddShape(lsBorgPlasma);
            dbPlasmaDiscGreen.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscBlue_Raw = new RealtimeLightning();
            dbPlasmaDiscBlue_Raw.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlue_Raw.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlue_Raw.SetPalette(PalPlasma.NewRaw(Color.Blue));
            dbPlasmaDiscBlue_Raw.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlue_Raw.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscViolet_Raw = new RealtimeLightning();
            dbPlasmaDiscViolet_Raw.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscViolet_Raw.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscViolet_Raw.SetPalette(PalPlasma.NewRaw(Color.Violet));
            dbPlasmaDiscViolet_Raw.AddShape(lsBorgPlasma);
            dbPlasmaDiscViolet_Raw.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscVioletEX1 = new RealtimeLightning();
            dbPlasmaDiscVioletEX1.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscVioletEX1.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscVioletEX1.SetPalette(PalPlasma.NewRaw(Color.Violet));
            dbPlasmaDiscVioletEX1.AddShape(lsBorgPlasma);
            dbPlasmaDiscVioletEX1.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscDarkViolet = new RealtimeLightning();
            dbPlasmaDiscDarkViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscDarkViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscDarkViolet.SetPalette(PalPlasma.New(Color.DarkViolet));
            dbPlasmaDiscDarkViolet.AddShape(lsBorgPlasma);
            dbPlasmaDiscDarkViolet.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscBlueViolet = new RealtimeLightning();
            dbPlasmaDiscBlueViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlueViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlueViolet.SetPalette(PalPlasma.New(Color.BlueViolet));
            dbPlasmaDiscBlueViolet.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlueViolet.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDiscBlue = new RealtimeLightning();
            dbPlasmaDiscBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlue.SetPalette(PalPlasma.New(Color.Blue));
            dbPlasmaDiscBlue.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlue.AddShape(lsBorgRing);

            // I really like this color, but it's more of a Cyan, instead of the deep BLUE I'm looking for
            // BUT, it might be hapfway between?
            RealtimeLightEffect dbPlasmaDiscBlueNICE = new RealtimeLightning();
            dbPlasmaDiscBlueNICE.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscBlueNICE.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscBlueNICE.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 128, blue: 255),
                Color.FromArgb(red: 192, green: 192, blue: 255)));
            dbPlasmaDiscBlueNICE.AddShape(lsBorgPlasma);
            dbPlasmaDiscBlueNICE.AddShape(lsBorgRing);

            // This is very close to actual Cyan, but looks a tad too yellow
            RealtimeLightEffect dbPlasmaDiscSkyBlue = new RealtimeLightning();
            dbPlasmaDiscSkyBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDiscSkyBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDiscSkyBlue.SetPalette(PalPlasma.New(Color.DeepSkyBlue));
            dbPlasmaDiscSkyBlue.AddShape(lsBorgPlasma);
            dbPlasmaDiscSkyBlue.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDisc_FavoriteLightBlue = new RealtimeLightning();
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

            RealtimeLightEffect dbPlasmaDisc_DarkBlue = new RealtimeLightning();
            dbPlasmaDisc_DarkBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_DarkBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_DarkBlue.SetPalette(PalPlasma.New(Color.DarkBlue));
            dbPlasmaDisc_DarkBlue.AddShape(lsBorgPlasma);
            dbPlasmaDisc_DarkBlue.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDisc_FavoriteBlue = new RealtimeLightning();
            dbPlasmaDisc_FavoriteBlue.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteBlue.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteBlue.SetPalette(PalPlasma.NewRaw(
                Color.FromArgb(red: 0, green: 64, blue: 255),
                Color.FromArgb(red: 224, green: 224, blue: 255)));
            dbPlasmaDisc_FavoriteBlue.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteBlue.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDisc_FavoriteViolet = new RealtimeLightning();
            dbPlasmaDisc_FavoriteViolet.Initialize(ringWidth, ringHeight, magnification);
            dbPlasmaDisc_FavoriteViolet.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc_FavoriteViolet.SetPalette(PalPlasma.New(Color.Magenta));
            dbPlasmaDisc_FavoriteViolet.AddShape(lsBorgPlasma);
            dbPlasmaDisc_FavoriteViolet.AddShape(lsBorgRing);

            RealtimeLightEffect dbPlasmaDisc_FavoriteDeepGreen = new RealtimeLightning();
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
            int magnification;
            //magnification = 2;
            //fireWidth = 200;
            //fireHeight = 200;
            magnification = 3;
            int left, top;

            Color[] palBorg;
            //palBorg = PaletteGenerator.GetHardCodedBorgPalette()
            //palBorg = PalPlasma.New(Color.DarkViolet);
            //palBorg = PalPlasma.NewRaw(
            //    Color.FromArgb(red: 0, green: 200, blue: 0),
            //    Color.FromArgb(red: 200, green: 200, blue: 200));
            //palBorg = PalPlasma.NewRaw(
            //    Color.FromArgb(red: 0, green: 128, blue: 255),
            //    Color.FromArgb(red: 192, green: 192, blue: 255));
            //palBorg = PalPlasma.NewRaw(
            //    Color.FromArgb(red: 0, green: 128, blue: 255),
            //    Color.FromArgb(red: 224, green: 224, blue: 255));
            palBorg = PalPlasma.New(Color.Green);

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

            RealtimeLightEffect dbPlasmaDisc = new RealtimeLightning();
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

            Color[] palLightning;
            //palLightning = PaletteGenerator.GetHardCodedLightningPalette();
            //palLightning = PalLightning.New(Color.Red);
            //palLightning = PalLightning.New(Color.Orange);
            //palLightning = PalLightning.New(Color.Yellow);
            //palLightning = PalLightning.New(Color.Green);
            //palLightning = PalLightning.New(Color.Blue);
            //palLightning = PalLightning.New(Color.Violet);
            palLightning = PalLightning.New();
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(27);
            ILightPen lpLightning = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsLightning = new LightShapeLightning();
            lsLightning.SetPen(lpLightning);

            RealtimeLightEffect dbLightning = new RealtimeLightning();
            dbLightning.Initialize(fireWidth, fireHeight, magnification);
            dbLightning.SetCoolingStrategy(coolingStrategy);
            dbLightning.SetPalette(palLightning);
            dbLightning.AddShape(lsLightning);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification);
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

            RealtimeLightEffect dbBatman;
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
            int top;
            int left;

            Color[] palCandle;
            //palCandle = PalRealisticFire.New();
            //palCandle = PalFlatPalette.New(Color.Orange);
            //palCandle = PalRealisticFlameCurve.New(Color.White);
            //palCandle = PalRealisticFlameCurve.New(Color.Black);
            //palCandle = PalFourPointLinear.New(Color.Black, Color.Orange, Color.Yellow, Color.Blue);
            //palCandle = PalRealisticFlameCurve.New(Color.FromArgb(223, 38, 38)); // This looks good at low resolution in an 8bit video game (better than 4 point linear)
            //palCandle = PalRealisticFlameCurve.New(Color.Maroon); // Color.Maroon == Color.FromArgb(128, 0, 0) not great
            //palCandle = PalRealisticFlameCurve.New(Color.FromArgb(255, 1, 1)); // Looks better as a "realistic" flame, not 8bit video game

            // Best, most realistic of 4-Point Linear method
            //Color c1 = Color.FromArgb(0, 0, 0);       // Black
            //Color c2 = Color.FromArgb(255, 185, 0);   // Orange
            //Color c3 = Color.FromArgb(255, 255, 127); // Bright Yellow
            //Color c4 = Color.FromArgb(212, 212, 255); // Light Blue
            //palCandle = PalFourPointLinear.New(c1, c2, c3, c4);

            palCandle = PalRealisticFire.New();
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

            RealtimeLightEffect dbCandle;
            //dbCandle = new RealtimeCandleflame();
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
                // stop the timer if running
                if (timer2 != null)
                {
                    timer2.Change(Timeout.Infinite, Timeout.Infinite);
                    timer2.Dispose();
                    timer2 = null;
                    m_iDemoBatmanState = 0;
                    m_iDemoHistoryState = 0;
                }

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

            Action bar = m_callbackToChangeStuff;
            m_callbackToChangeStuff = null;
            if (bar != null)
                bar.Invoke();

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

        Size originalSize = Size.Empty;
        private void UpdateVisibleUI()
        {
            if (timer1.Enabled)
            {
                if (m_fHideTitlebarOnDemo)
                {
                    // Fix resizing issue on Pi device (TODO: Account for multiple monitors)
                    Rectangle fullScreen = Screen.GetBounds(new Point(0, 0));
                    if (!fullScreen.IsEmpty && (
                        this.Size.Width > fullScreen.Width
                        || Size.Height > fullScreen.Height
                        || originalSize.Width > fullScreen.Width
                        || originalSize.Height > fullScreen.Height))
                    {
                        originalSize = new Size(fullScreen.Width, fullScreen.Height);
                    }
                    if (originalSize == Size.Empty)
                        originalSize = this.Size;
                    this.FormBorderStyle = FormBorderStyle.None; // Hide the Title Bar and other UI
                    this.Location = new Point(0, 0);
                    this.Size = originalSize;
                }
                groupBox1.Hide();
                groupBoxExperiment.Hide();
                buttonChange.Hide();
                buttonDemo.Hide();
                //Cursor = Cursors.UpArrow;
                //Cursor = Cursors.IBeam;
                //Cursor.Hide();
                // Move the mouse cursor out of the way
                Cursor.Position = new Point(this.Size.Width, this.Size.Height);

                if (m_graph == null)
                    m_graph = this.CreateGraphics();
            }
            else
            {
                //Cursor.Show();
                //Cursor = Cursors.Default;
                if (m_fHideTitlebarOnDemo)
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable; // Show the Title Bar
                    if (originalSize != Size.Empty)
                        this.Size = originalSize;
                    originalSize = Size.Empty;
                }
                groupBox1.Show();
                groupBoxExperiment.Show();
                //buttonChange.Show();
                //buttonDemo.Show();
            }
        }
        private void buttonAwayStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            SimpleCandle();

            m_dbSprite.Magnification = 7;
            int left = (this.Width - m_dbSprite.Width * m_dbSprite.Magnification) / 2;
            int top = (this.Height - m_dbSprite.Height * m_dbSprite.Magnification);
            m_dbSprite.Location = new Point(x: left, y: top);
            m_dbSprites.Add(m_dbSprite); // Add the candle

            m_coolingStrategy.SetMapParameters(width: m_dbSprite.Width, height: m_dbSprite.Height,
                density: 0.4f, min: 5, max: 13, smoothing: 5, // values from Original demo, doesn't produce the same results. Something is different in the refactored code (iFrame = 0 fixed it!!!)
                //density: 0.3f, min: 5, max: 23, smoothing: 1, // The numbers J liked for the 8bit TORCH2 (lower density, higher cooling, looks bad here)
                //density: 0.3f, min: 5, max: 23, smoothing: 5, // lower density, higher cooling, makes more dynamic flames
                //density: 0.2f, min: 5, max: 29, smoothing: 5, // best so far for single realistic flame demo (not bad, but no longer necessary)
                shift: true, rotate: true);

            m_largerFlame = true;
            m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);

            TextSprite tsAway = new TextSprite
            {
                //Text = "Away (if I'm not back in 5 minutes,\njust wait longer)",
                Text = "Away",
                Color = Color.Yellow,
                LocationRange = new Rectangle(x: 0, y: 0, width: 900, height: 50),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
            };
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
                Color = Color.DarkMagenta,
                LocationRange = new Rectangle(x: 0, y: 0, width: 900, height: 50),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
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
                Color = Color.Red,
                LocationRange = new Rectangle(x: 0, y: 0, width: 900, height: 50),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
            };
            m_dbSprites.Add(text);

            buttonDemo_Click(sender, e);
        }

        private void buttonAvailableStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoBorg();

            m_dbSprites.Add(m_dbSprite);
            TextSprite text = new TextSprite
            {
                Text = "Available (you will be assimilated)",
                Color = Color.LightGreen,
                LocationRange = new Rectangle(x: 0, y: 0, width: 850, height: 50),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
            };
            m_dbSprites.Add(text);

            buttonDemo_Click(sender, e);
        }

        private void buttonDndStatus_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();

            int ringWidth;
            int ringHeight;
            ringWidth = 129 / 2;
            ringHeight = 131 / 2;
            int ringMagnification;
            ringMagnification = 1;
            int left, top;

            Color[] palBorg = PalPlasma.New(Color.Red);
            ICoolingStrategy coolingStrategy = new CoolingStrategyConst(11);
            ILightPen lpPlasma = new LightPen(fill: 1.0f, min: 255, max: 255, useFullRange: false);
            ILightShape lsBorgRing = new LightShapeBorgRing();
            ILightShape lsBorgPlasma = new LightShapeBorgPlasma();
            lsBorgRing.SetPen(lpPlasma);
            lsBorgPlasma.SetPen(lpPlasma);

            RealtimeLightEffect dbPlasmaDisc = new RealtimeLightning();
            dbPlasmaDisc.Initialize(ringWidth, ringHeight, ringMagnification);
            dbPlasmaDisc.SetCoolingStrategy(coolingStrategy);
            dbPlasmaDisc.SetPalette(palBorg);
            dbPlasmaDisc.AddShape(lsBorgPlasma);
            dbPlasmaDisc.AddShape(lsBorgRing);
            left = (this.Width - ringWidth * ringMagnification) / 2;
            top = (this.Height - ringHeight * ringMagnification) / 2 + 50;
            dbPlasmaDisc.Location = new Point(x: left, y: top);

            HeroHoldingItem hero = new HeroHoldingItem
            {
                Magnification = 3,
                InterpolationMode = InterpolationMode.NearestNeighbor,
            };
            left = dbPlasmaDisc.Location.X;
            top = dbPlasmaDisc.Location.Y;
            left += (ringWidth / 2) - (hero.Width * hero.Magnification / 2);
            top += ringHeight * ringMagnification;
            hero.Location = new Point(left, top);


            OldMan man = new OldMan
            {
                Magnification = 3,
                InterpolationMode = InterpolationMode.NearestNeighbor
            };
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
            m_dbSprites.Add(hero);

            BrickWall brick1 = new BrickWall
            {
                InterpolationMode = InterpolationMode.NearestNeighbor,
                Magnification = 2
            };
            SpriteVideoGameBackground brickWall = new SpriteVideoGameBackground
            {
                Sprite = brick1
            };
            brickWall.Initialize();
            m_dbSprites.Add(brickWall);


            int cauldronFireWidth;
            int cauldronFireHeight;
            int cauldronFireMagnification;
            cauldronFireWidth = 19 * 2 - 1;
            cauldronFireHeight = 25 * 2;
            cauldronFireMagnification = 1;

            Color[] palCauldron1;
            Color[] palCauldron2;
            //palCauldron1 = PalRealisticFlameCurve.New(Color.DarkRed);
            //palCauldron1 = PalFourPointLinear.New(Color.Black, Color.Orange, Color.Yellow, Color.White);
            //palCauldron1 = PalFourPointLinear.New(Color.Black, Color.DarkOrange, Color.Yellow, Color.White);
            palCauldron1 = PalFourPointLinear.New(
                Color.Black,
                Color.FromArgb(255, 185, 0),
                Color.FromArgb(255, 255, 127),
                Color.FromArgb(212, 212, 255)); // great for larger bonfires, but not as nice for a small
            Color[] palTorch1 = PalRealisticFlameCurve.New(Color.FromArgb(223, 38, 38)); // Great 8 bit fire palette
            palCauldron1 = palTorch1;
            Color[] palTorch2 = PalRealisticFire.New();
            palCauldron2 = palTorch2;
            palCauldron1 = palTorch2;
            //palCauldron1 = PalFourPointLinear.New(Color.DarkOrange);

            CauldronBase cauldronBase1 = new CauldronBase
            {
                Location = new Point(
                    man.Location.X - man.Width * man.Magnification * 3,
                    man.Location.Y + man.Height * man.Magnification),
                Magnification = 5,
                InterpolationMode = InterpolationMode.NearestNeighbor,
            };
            m_dbSprites.Add(cauldronBase1);

            CoolingStrategyMap csBonfire1 = new CoolingStrategyMap();
            csBonfire1.SetMapParameters(width: cauldronFireWidth, height: cauldronFireHeight,
                //density: 0.5f, min: 8, max: 16, smoothing: 0, // great with 8bit Realistic palette Color.FromArgb(223, 38, 38)
                density: 0.4f, min: 5, max: 13, smoothing: 0, // great with Realistic palette
                //density: 0.6f, min: 8, max: 23, smoothing: 0,
                shift: true, rotate: true);

            ILightPen lpBonfire1;
            //lpBonfire1 = new LightPen(fill: 0.6f, min: 54, max: 255, useFullRange: false); // great for 8bit
            lpBonfire1 = new LightPen(fill: 1.0f, min: 0, max: 255, useFullRange: false); // great for realistic
            ILightShape lsBonfire1 = new LightShapeCandle();
            lsBonfire1.SetPen(lpBonfire1);

            RealtimeLightEffect dbCauldronFire1 = new RealtimeFire();
            dbCauldronFire1.Initialize(cauldronFireWidth, cauldronFireHeight, cauldronFireMagnification);
            dbCauldronFire1.InterpolationMode = InterpolationMode.NearestNeighbor;
            dbCauldronFire1.SetCoolingStrategy(csBonfire1);
            dbCauldronFire1.SetPalette(palCauldron1);
            dbCauldronFire1.AddShape(lsBonfire1);
            left = cauldronBase1.Location.X + 1;
            top = cauldronBase1.Location.Y - (dbCauldronFire1.Height - 1)* dbCauldronFire1.Magnification + 1;
            dbCauldronFire1.Location = new Point(x: left, y: top);
            m_dbSprites.Add(dbCauldronFire1);

            CauldronBase cauldronBase2 = new CauldronBase
            {
                Location = new Point(
                    man.Location.X + man.Width * man.Magnification * 3,
                    man.Location.Y + man.Height * man.Magnification),
                Magnification = cauldronBase1.Magnification,
                InterpolationMode = InterpolationMode.NearestNeighbor,
            };
            m_dbSprites.Add(cauldronBase2);


            // J likes this cauldron2 better, more pixelated, with:
            // cauldronFireWidth = 19 * 2 - 1;
            // cauldronFireHeight = 25 * 2;
            // density: 0.3f, min: 3, max: 35, smoothing: 0
            // fill: 0.7f, min: 54, max: 255, useFullRange: false
            RealtimeLightEffect dbCauldronFire2 = new RealtimeFire();
            CoolingStrategyMap csBonfire2 = new CoolingStrategyMap();
            csBonfire2.SetMapParameters(width: cauldronFireWidth, height: cauldronFireHeight,
                //density: 0.2f, min: 3, max: 20, smoothing: 0, // looks good with realistic palette? RealtimeFire_INCLUDING_COAL_SEED
                density: 0.3f, min: 3, max: 35, smoothing: 0, // looks good with realistic palette
                //density: 0.2f, min: 3, max: 15, smoothing: 0,
                //density: 0.6f, min: 6, max: 13, smoothing: 0, // looks good with 4 point linear palette
                //density: 0.7f, min: 9, max: 21, smoothing: 0,
                shift: true, rotate: true);

            ILightPen lpBonfire2 = new LightPen(fill: 0.7f, min: 54, max: 255, useFullRange: false);
            ILightShape lsBonfire2 = new LightShapeCandle();
            lsBonfire2.SetPen(lpBonfire2);

            dbCauldronFire2.Initialize(cauldronFireWidth, cauldronFireHeight, cauldronFireMagnification);
            dbCauldronFire2.InterpolationMode = InterpolationMode.NearestNeighbor;
            dbCauldronFire2.SetCoolingStrategy(csBonfire2);
            dbCauldronFire2.SetPalette(palCauldron2);
            dbCauldronFire2.AddShape(lsBonfire2);
            left = cauldronBase2.Location.X + 1;
            top = cauldronBase2.Location.Y - (dbCauldronFire2.Height - 1) * dbCauldronFire2.Magnification + 1;
            dbCauldronFire2.Location = new Point(x: left, y: top);
            m_dbSprites.Add(dbCauldronFire2);

            ILightPen lpTorch;
            //lpTorch = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
            //lpTorch = new LightPen(fill: 0.08f, min: 200, max: 255, useFullRange: true);
            //lpTorch = new LightPen(fill: 1.0f, min: 200, max: 255, useFullRange: true);
            //lpTorch = new LightPen(fill: 0.5f, min: 34, max: 255, useFullRange: true);
            lpTorch = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
            ILightShape lsTorch = new LightShapeCandle();
            lsTorch.SetPen(lpTorch);

            int torchFlameHeight;
            int torchFlameWidth;
            int torchMagnification;
            torchFlameHeight = 35;
            torchFlameWidth = 12;
            torchMagnification = 3;

            TorchHandle torchHandle1 = new TorchHandle
            {
                Location = new Point(
                man.Location.X - man.Width * man.Magnification * 5,
                man.Location.Y + (5 * man.Height * man.Magnification)),
                Magnification = 4,
                InterpolationMode = InterpolationMode.NearestNeighbor,
            };
            m_dbSprites.Add(torchHandle1);

            CoolingStrategyMap csTorch1 = new CoolingStrategyMap();
            csTorch1.SetMapParameters(width: torchFlameWidth, height: torchFlameHeight,
                density: 0.3f, min: 5, max: 23, smoothing: 1,
                shift: true, rotate: true);

            RealtimeLightEffect dbTorch1 = new RealtimeFire();
            dbTorch1.Initialize(torchFlameWidth, torchFlameHeight, magnification: torchMagnification);
            dbTorch1.InterpolationMode = InterpolationMode.NearestNeighbor;
            dbTorch1.SetCoolingStrategy(csTorch1);
            dbTorch1.SetPalette(palTorch1);
            dbTorch1.AddShape(lsTorch);
            left = torchHandle1.Location.X - 2;
            top = torchHandle1.Location.Y - (dbTorch1.Height - 1) * dbTorch1.Magnification + 1;
            dbTorch1.Location = new Point(x: left, y: top);
            m_dbSprites.Add(dbTorch1);

            TorchHandle torchHandle2 = new TorchHandle
            {
                Location = new Point(
                man.Location.X + man.Width * man.Magnification * 5,
                man.Location.Y + (5 * man.Height * man.Magnification)),
                Magnification = torchHandle1.Magnification,
                InterpolationMode = InterpolationMode.NearestNeighbor,
            };
            m_dbSprites.Add(torchHandle2);

            // J likes this torch2 better, moves more realistic, with:
            // torchFlameHeight = 35;
            // torchFlameWidth = 12;
            // density: 0.3f, min: 5, max: 23, smoothing: 1
            // lpTorch = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
            CoolingStrategyMap csTorch2 = new CoolingStrategyMap();
            csTorch2.SetMapParameters(width: torchFlameWidth, height: torchFlameHeight,
                density: 0.3f, min: 5, max: 23, smoothing: 1,
                shift: true, rotate: true);

            RealtimeLightEffect dbTorch2 = new RealtimeFire();
            dbTorch2.Initialize(torchFlameWidth, torchFlameHeight, magnification: torchMagnification);
            dbTorch2.InterpolationMode = InterpolationMode.NearestNeighbor;
            dbTorch2.SetCoolingStrategy(csTorch2);
            dbTorch2.SetPalette(palTorch2);
            dbTorch2.AddShape(lsTorch);
            left = torchHandle2.Location.X - 2;
            top = torchHandle2.Location.Y - (dbTorch2.Height - 1) * dbTorch2.Magnification + 1;
            dbTorch2.Location = new Point(x: left, y: top);
            m_dbSprites.Add(dbTorch2);


            TextSprite lifeText = new TextSprite
            {
                Text = "-LIFE-",
                Color = Color.Red,
                Font = new Font(family: SystemFonts.DefaultFont.FontFamily, emSize: 20.0f),
                Location = new Point(Width - (Width / 5), 0),
            };
            m_dbSprites.Add(lifeText);


            int dangerY = man.Location.Y - 95;
            TextSprite text = new TextSprite
            {
                Text = "It's dangerous to bother me!\nGO AWAY! (use email)",
                Font = new Font(family: SystemFonts.DefaultFont.FontFamily, emSize: 30.0f),
                SmoothTransition = true,
                Location = new Point(x: 32, y: dangerY),
                LocationRange = new Rectangle(x: 32, y: dangerY, width: 490-32, height: 0),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
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

            if (!timer1.Enabled && timer2 == null)
                buttonDemo_Click(sender, e);
        }

        private void buttonRainBORG_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoPlasmaRainbow();
            buttonDemo_Click(sender, e);
        }

        //System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
        System.Threading.Timer timer2; // timer for changing the demo at runtime
        private void buttonBatmanSingleThread_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoBatman(multithreaded: false);
            buttonDemo_Click(sender, e);
        }

        private void buttonRainbowBatman_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            DemoBatman(multithreaded: true);

            TextSprite text = new TextSprite
            {
                Text = "I'm Busy",
                Color = Color.Red,
                LocationRange = new Rectangle(x: 0, y: 0, width: 900, height: 50),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
            };
            m_dbText = text;
            m_dbSprites.Add(m_dbSprite); // Add the Flaming Batman logo
            m_dbSprites.Add(text); // Add the text sprite

            //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadCallbacktoDoStuff), 0);
            //ThreadPool.RegisterWaitForSingleObject()
            timer2 = new System.Threading.Timer(
                new TimerCallback(ThreadCallbackBatmanRainbowDemo),
                null,
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(2));

            buttonDemo_Click(sender, e);
        }

        struct BatDemoState
        {
            public string text;
            public Color textColor;
            public Color fireColor;
            public Color[] firePalette;
        };
        BatDemoState[] batDemoStates = {
                new BatDemoState
                {
                    text = "I'm Busy",
                    textColor = Color.Red,
                    firePalette = PalRealisticFire.New(),
                },
                new BatDemoState
                {
                    text = "I'm Joking!",
                    textColor = Color.Indigo,
                    fireColor = Color.Indigo,
                },
                new BatDemoState
                {
                    text = "Red",
                    textColor = Color.Red,
                    fireColor = Color.Red,
                },
                new BatDemoState
                {
                    text = "Orange",
                    textColor = Color.Orange,
                    fireColor = Color.Orange,
                },
                new BatDemoState
                {
                    text = "Yellow",
                    textColor = Color.Yellow,
                    fireColor = Color.Yellow,
                },
                new BatDemoState
                {
                    text = "Green",
                    textColor = Color.Green,
                    fireColor = Color.Green,
                },
                new BatDemoState
                {
                    text = "Blue",
                    textColor = Color.Blue,
                    fireColor = Color.Blue,
                },
                new BatDemoState
                {
                    text = "Violet",
                    textColor = Color.Violet,
                    fireColor = Color.Violet,
                },
                new BatDemoState
                {
                    text = "Black",
                    textColor = Color.White,
                    fireColor = Color.Black,
                },
            };
        int m_iDemoBatmanState = 0;
        private void ThreadCallbackBatmanRainbowDemo(object state)
        {
            int ii = ++m_iDemoBatmanState % batDemoStates.Length;

            Color textColor = batDemoStates[ii].textColor;
            Color[] palFire;            
            if (batDemoStates[ii].firePalette != null)
                palFire = batDemoStates[ii].firePalette;
            else
                palFire = PalRealisticFlameCurve.New(batDemoStates[ii].fireColor);

            m_dbSprite.SetPalette(palFire);
            m_dbText.Color = textColor;
            m_dbText.Text = batDemoStates[ii].text;
        }

        private void buttonHistoryOfFire_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();

            // 2001 - I started this project because someone said Smalltalk was too slow for realtime fire generation.
            // I remembered seeing a fire algorithm earlier that just averaged a few pixels and ended up creating
            // something that looked pretty realistic. Sadly, I couldn't find the code anymore, so I played
            // around with averaging different pixels until I figured out something that looked ok.
            // I came up with a proof of concept and was reasonably happy with what I created.
            // Proving that Smalltalk was fast "enough."
            //
            // 2015/2016 - When thinking about FHL ideas, I remembered that fun project from years ago and wanted
            // to extend it to create a toy program. The goals were twofold.
            // 1) create more realistic fire.
            // 2) allow changing all the parameters at runtime, via UI, so I could quickly see the differences
            // when averaging different pixels, and change the color too.
            // I remembered that project and wanted to try to make the fire look even more realistic.
            // I generated a better palette, and added cooling maps.
            // And created an even *better* palette... (NEW SLIDE)
            //
            // 2020 - Then I wanted to create fire in fun shapes, like the Batman Logo. (NEW SLIDE)
            // Purchased a Raspberry Pi device kit, learned Linux, well started anyway.
            //
            // 2020/2021? Then I had the idea to create a Borg Regeneration Ring.
            // Starting with lightning (SLIDE), I then progressed to the Plasma Disc (SLIDE).
            //
            // 2021 - the code was getting cumbersome and needed to be refactored
            // (one "boring" FHL refactoring in Smalltalk, and another porting the changes to C#,
            // but this sped up future development)
            //
            // 2021 - the Eye of Sauron (SLIDE)
            //
            // All because someone said realtime fire was impossible in Smalltalk.


            // DEMO:
            // 1. Squeak Palette
            // 2. Constant Cooling (good enough for a proof-of-concept)
            // Bilinear interpolation (default in C#):
            // 3. Better 4 point linear palette
            // 4. Cooling Map
            // 5. Shifting Cooling Map (looks more natural)
            // 6. Rotating Cooling Map (stops repeating)
            // 7. Curved Palette
            // 8. Average 4 pixels instead of 5
            // 9. Batman
            // 10. Lightning
            // 11. Borg Plasma
            // 12. Hero Cave
            // Bicubic interpolation:
            // 13. Sauron

            TextSprite text = new TextSprite
            {
                LocationRange = new Rectangle(0, 0, 50, 75),
            };
            m_dbText = text;

            timer2 = new System.Threading.Timer(
                new TimerCallback(ThreadCallbackFireHistoryDemo),
                null,
                TimeSpan.Zero,
                historyDemoPeriod);

            buttonDemo_Click(sender, e);
        }

        TimeSpan historyDemoPeriod = TimeSpan.FromMilliseconds(9000);
        int m_iDemoHistoryState = 0;
        private void ThreadCallbackFireHistoryDemo(object state)
        {
            Rectangle fullRange = new Rectangle(0, 0, 50, 75);
            //Func<int>[] variousStages =
            Action[] variousStages =
            {
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbSprites.Clear();
                    SimpleCandle();
                    int fireWidth = m_dbSprite.Width;
                    int fireHeight = 70;
                    int magnification = 7;
                    m_dbSprite.Initialize(fireWidth, fireHeight, magnification);
                    int left = (this.Width - fireWidth * magnification) / 2;
                    int top = (this.Height - fireHeight * magnification);
                    m_dbSprite.Location = new Point(left, top);
                    m_dbText.Text = "...in 2001, I wrote a prototype when someone\n" +
                    "said, \"Smalltalk is too slow for realtime fire\n" +
                    "generation.\" And I just had to prove them wrong.";
                    m_dbText.SetLocationParameters(newLocation: Point.Empty, range: fullRange);
                    m_dbText.Color = Color.Red;
                    m_dbSprites.Add(m_dbSprite); // Add the candle
                    m_dbSprites.Add(m_dbText); // Add the text sprite
                    m_dbSprite.InterpolationMode = InterpolationMode.NearestNeighbor;
                    m_genericFlame.SetPalette(PaletteGenerator.MakeTransparent(PalFlatPalette.New(Color.Orange)));
                    m_genericFlame.SetCoolingStrategy(new CoolingStrategyConst(0));
                    m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "I liked the result, even with plain constant cooling.\n" +
                    "The palette isn't great, but it's a proof-of-concept.";
                    // "(Flat 2-point linear palette algorithm)"
                    Rectangle partialRange = new Rectangle(fullRange.Location, fullRange.Size) { Width = 0 };
                    m_dbText.SetLocationParameters(newLocation: new Point(0, partialRange.Height),
                        smoothTransition: true,
                        period: historyDemoPeriod,
                        range: partialRange);
                    m_genericFlame.SetCoolingStrategy(new CoolingStrategyConst(2));
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    Color color = Color.FromArgb(64, 128, 255); // LightBlue
                    m_dbText.Color = color;
                    m_dbText.Text = "In blue, it really reminds me of the flame\n" +
                    "of a propane grill that I watched as a child\n" +
                    "while my dad cooked burgers.";
                    m_dbText.SetLocationParameters(range: fullRange);
                    m_genericFlame.SetCoolingStrategy(new CoolingStrategyConst(2));
                    m_genericFlame.SetPalette(PaletteGenerator.MakeTransparent(PalFlatPalette.New(color)));
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "15 years later, I remembered the challenge,\n" +
                    "improved the palettes and added more colors,\n" +
                    "but it's not quite realistic enough, so I...";
                    // "(4-point linear is better, but not quite realistic)"
                    m_dbText.Color = Color.DarkOrange;
                    Color c1 = Color.FromArgb(0, 0, 0);       // Black
                    Color c2 = Color.FromArgb(255, 185, 0);   // Orange
                    Color c3 = Color.FromArgb(255, 255, 127); // Bright Yellow
                    Color c4 = Color.FromArgb(212, 212, 255); // Light Blue
                    m_genericFlame.SetPalette(PaletteGenerator.MakeTransparent(PalFourPointLinear.New(c1, c2, c3, c4)));
                    //m_genericFlame.InterpolationMode = InterpolationMode.Default;
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    //m_dbText.Text = "Derived a palette formula curve for realistic color.\nProved the algorithm is fast enough for Smalltalk.";
                    //m_dbText.Text = "Derived a palette formula curve for realistic color,\nwith the EXACT same pixel averaging algorithm\nfrom 15 years earlier, JUST better palettes so far!";
                    m_dbText.Text = "Derived a palette formula curve for realistic color.\n" +
                    "This is only a palette change, further proving\n" +
                    "Smalltalk was fast enough for realistic fire.";
                    // 
                    // So far only changed the palette, therefore Smalltalk is fast enough to run the algorithm from 15 years prior.
                    // Still the EXACT same pixel averaging algorithm\nfrom 15 years earlier, JUST better palettes so far!
                    m_dbText.SetLocationParameters(newLocation: Point.Empty);
                    m_dbText.Color = Color.OrangeRed;
                    m_genericFlame.SetPalette(PaletteGenerator.MakeTransparent(PalRealisticFire.New()));
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "NOW with 15 additional years of processing power,\n" +
                    "I can use bicubic interpolation, and...";
                    if (Util.IsLinux)
                        m_genericFlame.InterpolationMode = InterpolationMode.Bicubic;
                    else
                        m_genericFlame.InterpolationMode = InterpolationMode.HighQualityBicubic;
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "...generate a Cooling Map for greatly improved realism.";
                    ICoolingStrategy coolingStrategy;
                    m_coolingStrategy = new CoolingStrategyMap();
                    m_coolingStrategy.SetMapParameters(width: m_genericFlame.Width, height: m_genericFlame.Height,
                        density: 0.4f, min: 5, max: 13, smoothing: 5,
                        shift: true, rotate: true);
                    coolingStrategy = m_coolingStrategy;
                    m_genericFlame.SetCoolingStrategy(coolingStrategy);
                }),
                //new Action(() =>
                //{
                //    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                //    m_dbText.Text = "...and enable changing parameters on the fly.";
                //    m_genericFlame.SetPixelMatrix(f8: true, f5: true, f1: true, f2: true, f3: true);
                //}),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "The generalized curved palette formula\nworks for arbitrary colors...";
                    m_dbText.Color = Color.LightBlue;
                    m_genericFlame.SetPalette(PaletteGenerator.MakeTransparent(PalRealisticFlameCurve.New(Color.Blue)));
                    m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "...ANY color!";
                    m_dbText.Font = new Font(m_dbText.Font, FontStyle.Italic);
                    m_dbText.Color = Color.SkyBlue;
                    m_dbSprites.Clear();
                    m_dbSprites.AddRange(CreateRainbowFlames(fBigRainbowFire: false));
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "From there, I wanted to create SHAPES\nout of the fire!";
                    m_dbText.Font = new Font(m_dbText.Font, FontStyle.Regular);
                    m_dbSprites.Clear();
                    m_dbText.Color = Color.Yellow;
                    DemoBatman(multithreaded: true);
                    m_dbSprites.Add(m_dbSprite);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    TimeSpan tsMax = TimeSpan.FromSeconds(3);
                    TimeSpan tsHalf = TimeSpan.FromTicks(historyDemoPeriod.Ticks / 2);
                    TimeSpan ts = tsHalf.CompareTo(tsMax) < 0 ? tsHalf : tsMax;
                    timer2.Change(ts, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Color = Color.LightCyan;
                    m_dbText.Text = "Then I had an idea for lightning,\nwhich was just a step toward making...";
                    m_dbSprites.Clear();
                    DemoLightning();
                    m_dbSprite.Initialize(m_dbSprite.Width * 2, m_dbSprite.Height * 2, 1);
                    m_dbSprites.Add(m_dbSprite);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Color = Color.LightGreen;
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "...plasma for the Borg Alcove Regeneration Disc.";
                    // Reset the text period timer so it randomly moves
                    // as the pictures change.
                    m_dbText.SetLocationParameters(
                        newLocation: Point.Empty,
                        period: historyDemoPeriod);
                    m_dbSprites.Clear();
                    DemoBorg();
                    m_dbSprite.Location.Offset(0, 50);
                    m_dbSprites.Add(m_dbSprite);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "And then took all these pieces from the previous\n" +
                    "iterations to refactor and recombine them into...";
                    m_dbText.Color = Color.LightSkyBlue;
                    // Demo RainBORG
                    m_dbSprites.Clear();
                    DemoPlasmaRainbow();
                    foreach (SimpleSprite s in m_dbSprites)
                    {
                        // 0, 196, 392
                        int addY = 150;
                        if (s.Location.Y > 0)
                            addY -= 50;
                        if (s.Location.Y > 200)
                            addY -= 50;
                        Point moveDown = new Point(0, addY);
                        s.Location.Offset(moveDown);
                    }
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    // Let Sauron go for double length, since I'm speeding up the demo
                    timer2.Change(historyDemoPeriod.Add(historyDemoPeriod), TimeSpan.Zero);
                    m_dbText.Color = Color.OrangeRed;
                    LayeredSprite dbEyeOfSauron = CreateEyeOfSauronV3();
                    int left = (this.Width - dbEyeOfSauron.Width * dbEyeOfSauron.Magnification) / 2;
                    int top = (this.Height - dbEyeOfSauron.Height * dbEyeOfSauron.Magnification) / 2;
                    dbEyeOfSauron.Location = new Point(left, top);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "...the Eye of Sauron!";
                    m_dbSprites.Clear();
                    m_dbSprites.Add(dbEyeOfSauron);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    // Let Sauron go for extra
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "And now I use these graphics to communicate\n" +
                    "whether or not I'm busy Working, all because...";
                }),
                //new Action(() =>
                //{
                //  timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                //}),
            };

            int ii = m_iDemoHistoryState++ % variousStages.Length;
            m_callbackToChangeStuff = variousStages[ii];
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAdvanced_Click(object sender, EventArgs e)
        {
            Form form = new Form1();
            form.ShowDialog();
        }

        private void buttonFastRender_Click(object sender, EventArgs e)
        {
            Form form = new Form3();
            form.ShowDialog();
        }

        // repurposed for v2
        private void buttonSauronV1_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            Size sceneSize = new Size(width: 200, height: 100);

            int magnification = 2;
            List<DynamicSprite> dbSprites = new List<DynamicSprite>()
            {
                CreateSauron_CornersOfEyeInward(),
                CreateSauronV3_Lightning(sceneSize, left: true), // Lightning bolts from the left
                CreateSauronV3_Lightning(sceneSize, left: false), // Lightning bolts from the right
                CreateSauron_PupilOutward(sceneSize, isNarrow: false),
            };

            Color[] palFire;
            palFire = PalRealisticFire.New();

            // Color.Transparent doesn't work in Linux, even though it works fine in Windows.
            // Setting alpha to zero (effectively the same thing) does work in Linux.
            palFire[0] = Color.FromArgb(0, palFire[0]);

            foreach (DynamicSprite temp in dbSprites)
            {
                temp.SetPalette(palFire);
            }

            LayeredSprite dbSauron = new LayeredSprite();

            int width = 0;
            int height = 0;
            foreach (DynamicSprite s in dbSprites)
            {
                s.Location = Point.Empty;
                width = Math.Max(width, s.Width);
                height = Math.Max(height, s.Height);
            }

            dbSauron.Initialize(width, height, magnification);
            int left = (Width - width * magnification) / 2;
            int top = (Height - height * magnification) / 2;
            dbSauron.Location = new Point(left, top);

            dbSauron.AddRange(dbSprites);

            m_dbSprites.Add(dbSauron);

            buttonDemo_Click(null, null);
        }

        private RealtimeLightEffect CreateSauronV36_SmallLightning(Size size, bool left)
        {
            int magnification = 1;
            float factor = 4.7f;
            int lightWidth = (int)(size.Width / factor / magnification);
            int lightHeight = (int)(size.Height / factor / magnification);
            int xCenter = size.Width / 2;
            int yCenter = size.Height / 2;
            int xOffset; //  = xCenter;
            int yOffset = (size.Height - lightHeight * magnification) / 2;

            if (left)
                xOffset = 0;
            else
                xOffset = size.Width - lightWidth * magnification;

            LightPen lpLightning = new LightPen(fill: 1, 255, 255, useFullRange: false);
            LightShapeLightning lsBolt = new LightShapeLightning();
            ICoolingStrategy csSauron = new CoolingStrategyConst(27);
            lsBolt.SetPen(lpLightning);
            RealtimeLightEffect dbLightningBolt = new RealtimeLightning();
            RotateFlipType direction = left ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone;
            //if (left)
            dbLightningBolt.Location = new Point(xOffset, yOffset);
            dbLightningBolt.LocationRange = new Rectangle(x: xOffset, y: size.Height / 4, width: 0, height: size.Height / 2);
            dbLightningBolt.LocationPeriod = TimeSpan.FromMilliseconds(100);
            dbLightningBolt.SmoothTransition = false;
            //else
            //    dbLightningBolt.Location = new Point(xOffset, 0);
            dbLightningBolt.Initialize(lightWidth, lightHeight, magnification, direction);
            dbLightningBolt.SetCoolingStrategy(csSauron);

            // This one I may want to remove the second set after eventually parametrizing the lightning,
            // so we can have more small bolts, but don't clutter them with too many at the same time,
            // but the v3.6 isn't looking as good as v3.5 so I may stop at 3.5 and stop developing v3.6
            dbLightningBolt.AddShape(lsBolt);
            dbLightningBolt.AddShape(lsBolt);

            return dbLightningBolt;
        }

        // repurposed for v3.5
        private void buttonSauronV2_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            LayeredSprite dbSauron = CreateEyeOfSauronV3();

            VectorSauronTowerSprite tower = new VectorSauronTowerSprite();
            //dbSauron.Add(tower);
            //dbSauron.InterpolationMode = InterpolationMode.NearestNeighbor;
            m_dbSprites.Add(dbSauron);
            m_dbSprites.Add(tower);

            // TextSprite "Eye see you, did you bring the ring?"
            //TextSprite text = new TextSprite()
            //{
            //    Color = Color.PaleGoldenrod, // Color.LightGoldenrodYellow,
            //    Text = "Eye see you,\ngive me the ring.",
            //    Location = new Point(370, 300),
            //};
            //m_dbSprites.Add(text);

            buttonDemo_Click(null, null);
        }

        // latest v3 (tempted to mirror everything except the pupil, which should dramatically increase speed)
        private void buttonSauronV3_Click(object sender, EventArgs e)
        {
            RunSauronFullHiRez();
        }

        void RunSauronQuickHiRez()
        {
            m_dbSprites.Clear();
            LayeredSprite dbSauron = CreateEyeOfSauronV3();


            // TODO: JRDV: If this works, then DELETE the HiRez stuff!!! Just layer the small lightning over top HERE!
            // It seems faster than the resized stuff, but at least no worse than doing it the more complicated way.
            // It's still 39-46 FPS, EVEN with NO additional lightning!
            // So this method of nesting LayeredSprites is a bit slower than the v3.5 method,
            // so maybe I don't want to pursue this line...
            //
            // What am I trying to solve?
            // 1) blinking in Windows (not a problem on Pi device)
            // 2) would like more varied smaller bolts (prototypes look bad)
            //
            // Time would be better spent:
            // 1) making the TOWER
            // 2) Parameterize the lightning code so I can rotate bolts and
            //      vary the forking and frequency more easily.
            LayeredSprite db2 = new LayeredSprite();
            db2.Initialize(dbSauron.Width * dbSauron.Magnification, dbSauron.Height * dbSauron.Magnification, 1);
            db2.Add(dbSauron);
            Size scene = new Size(db2.Width, db2.Height);
            Color[] palLightning;
            palLightning = PalFourPointLinear.New(Color.FromArgb(250, 219, 125));
            palLightning[0] = Color.FromArgb(0, palLightning[0]);

            RealtimeLightEffect dbExampleLightning = CreateSauronV36_SmallLightning(scene, left: true);
            dbExampleLightning.SetPalette(palLightning);
            db2.Add(dbExampleLightning);

            dbExampleLightning = CreateSauronV36_SmallLightning(scene, left: false);
            dbExampleLightning.SetPalette(palLightning);
            db2.Add(dbExampleLightning);


            db2.Location = dbSauron.Location;
            dbSauron.Location = Point.Empty;

            VectorSauronTowerSprite tower = new VectorSauronTowerSprite();
            m_dbSprites.Add(db2);
            m_dbSprites.Add(tower);

            buttonDemo_Click(null, null);
        }

        // This is slightly faster than the Quick method, continue this path
        // Removing the lightning runs at 56 FPS on Raspberry-Pi,
        // so if I can optimizethe lightning, then maybe this path is valid again
        void RunSauronFullHiRez()
        {
            m_dbSprites.Clear();
            LayeredSprite dbSauron = CreateEyeOfSauronV3(hiRez: true);

            VectorSauronTowerSprite tower = new VectorSauronTowerSprite();
            m_dbSprites.Add(dbSauron);
            m_dbSprites.Add(tower);

            buttonDemo_Click(null, null);
        }

        private LayeredSprite CreateEyeOfSauronV3(bool hiRez = false)
        {
            Size sceneSize = new Size(width: 350, height: 100);
            // HiRez so I can add smaller lightning bolts around the perimiter
            // And eliminate flicker in Windows
            if (hiRez)
                sceneSize = new Size(width: sceneSize.Width * 2, height: sceneSize.Height * 2);
            int magnification = hiRez ? 1 : 2;
            List<DynamicSprite> dbSprites;
            dbSprites = new List<DynamicSprite>
            {
                CreateSauronV3_SmokeOutward(sceneSize, left: true, hiRez), // Add sideways layer of 4 Point red smoke
                CreateSauronV3_SmokeOutward(sceneSize, left: false, hiRez), // Add sideways layer of 4 Point red smoke
                CreateSauronV3_Lightning(sceneSize, left: true, hiRez), // Lightning bolts from the left
                CreateSauronV3_Lightning(sceneSize, left: false, hiRez), // Lightning bolts from the right
                CreateSauron_EyeRingInward(sceneSize, hiRez), // Ring for the outside of the eyeball
                CreateSauron_PupilOutward(sceneSize, isNarrow: true, hiRez), // Center for the pupil
            };

            if (hiRez)
            {
                dbSprites.Add(CreateSauronV36_SmallLightning(sceneSize, left: true));
                dbSprites.Add(CreateSauronV36_SmallLightning(sceneSize, left: false));
            }
            LayeredSprite dbSauron = new LayeredSprite();

            Color[] palFire;
            //palFire = PalRealisticFire.New();
            //Color c1 = Color.FromArgb(0, 0, 0);       // Black
            //Color c2 = Color.FromArgb(255, 185, 0);   // Orange
            //Color c3 = Color.FromArgb(255, 255, 127); // Bright Yellow
            //Color c4 = Color.FromArgb(212, 212, 255); // Light Blue
            //palFire = PalFourPointLinear.New(c1, c2, c3, c4);
            palFire = PalRealisticFlameCurve.New(Color.FromArgb(255, 1, 1));


            Color[] palBackgroundSmoke;
            //palBackgroundSmoke = PalFourPointLinear.New(Color.Red);
            //palBackgroundSmoke = PalFourPointLinear.New(Color.OrangeRed);
            palBackgroundSmoke = PalFlatPalette.New(Color.OrangeRed);

            Color[] palLightning;
            //palLightning = PalRealisticFlameCurve.New(Color.FromArgb(255, 1, 1));
            //palLightning = PalFourPointLinear.New(Color.FromArgb(255, 1, 1));
            //palLightning = PalLightning.New(Color.FromArgb(255, 1, 1));
            //palLightning = PalLightning.New();
            palLightning = PalFourPointLinear.New(Color.FromArgb(250, 219, 125));


            for (int ii = 0; ii < 1; ++ii)
                palBackgroundSmoke[ii] = Color.FromArgb(0, palBackgroundSmoke[ii]);
            palFire = PaletteGenerator.MakeTransparent(palFire);
            palLightning = PaletteGenerator.MakeTransparent(palLightning);

            // Tried gradient of alpha across multiple colors so it blends better, but Linux doesn't honor alpha,
            // other than 0 and not-zero. Anything greater than zero is 100% opaque.
            //for (int ii = 1; ii < palLightning.Length; ++ii)
            //{
            //    palLightning[ii] = Color.FromArgb(128, palLightning[ii]);
            //}

            int iLayer = 0;
            dbSprites[iLayer++].SetPalette(palBackgroundSmoke);
            dbSprites[iLayer++].SetPalette(palBackgroundSmoke);
            dbSprites[iLayer++].SetPalette(palLightning);
            dbSprites[iLayer++].SetPalette(palLightning);
            dbSprites[iLayer++].SetPalette(palFire);
            dbSprites[iLayer++].SetPalette(palFire);
            if (hiRez)
            {
                dbSprites[iLayer++].SetPalette(palLightning);
                dbSprites[iLayer++].SetPalette(palLightning);
            }


            int width = sceneSize.Width;
            int height = sceneSize.Height;
            dbSauron.Initialize(width, height, magnification);
            int left = (Width - width * magnification) / 2;
            int top = (Height - height * magnification) / 10;
            dbSauron.Location = new Point(left, top);

            dbSauron.AddRange(dbSprites);

            if (hiRez && !Util.IsLinux)
            {
                // Reduce the flicker in Windows
                SimpleSprite spTower = new VectorSauronTowerSprite
                {
                    Location = new Point(-left, -top)
                };
                dbSauron.Add(spTower);
            }

            return dbSauron;
        }

        private RealtimeLightEffect CreateSauronV3_Lightning(Size size, bool left, bool hiRez = false)
        {
            int magnification = hiRez ? 2 : 1;
            int lightWidth = (int)(size.Width / 2 / magnification);
            int lightHeight = size.Height / magnification;
            int yOffset = (size.Height - lightHeight * magnification) / 2;
            int xCenter = size.Width / 2;
            int xOffset = xCenter;

            if (left)
                xOffset = xCenter - lightWidth * magnification;

            LightPen lpLightning = new LightPen(fill: 1, 127, 255, useFullRange: true);
            LightShapeLightning lsBolt = new LightShapeLightning();
            ICoolingStrategy csSauron = new CoolingStrategyConst(27);
            lsBolt.SetPen(lpLightning);
            RealtimeLightEffect dbLightningBolt = new RealtimeLightning();
            RotateFlipType direction = left ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone;
            if (left)
                dbLightningBolt.Location = new Point(xOffset, yOffset);
            else
                dbLightningBolt.Location = new Point(xCenter, yOffset);
            dbLightningBolt.Initialize(lightWidth, lightHeight, magnification, direction);
            dbLightningBolt.SetCoolingStrategy(csSauron);

            // I think I like this doubling, even after eventually parametrizing the lightning,
            // so we can sometimes have double the bolts in the same frame
            dbLightningBolt.AddShape(lsBolt);
            dbLightningBolt.AddShape(lsBolt);

            return dbLightningBolt;
        }


#if false // EXPERIMENTAL
        private AbstractRealtimeLightEffect CreateSauronV3_Lightning_OLD(Size size, bool left)
        {
            int magnification = 1;
            int lightWidth = 100;
            int lightHeight = 50;

            LightPen lpLightningBolt = new LightPen(fill: 1.0f, 255, 255, useFullRange: false);
            DirectedLightning lsBolt = new DirectedLightning
            {
                Angle = left ? 90 : 270,
                Length = 50, // lightWidth?
                Location = new Point(0, lightHeight / 2)
            };
            ICoolingStrategy csSauron = new CoolingStrategyConst(27);
            lsBolt.SetPen(lpLightningBolt);
            RealtimeLightning dbSauron = new RealtimeLightning();
            if (left)
                dbSauron.Location = new Point(size.Width / 2 - (lightWidth * magnification), 0);
            else
                dbSauron.Location = new Point(size.Width / 2, 0);
            dbSauron.Initialize(lightWidth, lightHeight, magnification);
            dbSauron.SetCoolingStrategy(csSauron);
            dbSauron.AddShape(lsBolt);

            return dbSauron;
        }
#endif


        private RealtimeLightEffect CreateSauronV3_SmokeOutward(Size size, bool left, bool hiRez = false)
        {
            int magnification = hiRez ? 6 : 3;
            int smokeWidth = size.Width / 2 / magnification;
            int smokeHeight = size.Height / magnification;
            int xCenter = size.Width / 2;
            int xOffset = xCenter;

            if (left)
                xOffset = xCenter - smokeWidth * magnification;

            // IDEA: Render a regular/upright flame, with a curved seed shape, then "transform" it 90 degrees?
            // Then "transform" it with a delta field to squish it into the eye shape?
            // Might be able to accomplish both transform functions simultaneously.
            // Do two of them. What do I mean two of them? OH, do two flames. Don't try to do both in the same layer
            LightPen lpSauronBackground = new LightPen(fill: 1, 200, 255, useFullRange: true);
            LightShapeCandle lsFireStick = new LightShapeCandle();
            ICoolingStrategy csSauron;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: smokeHeight, height: smokeWidth,
                //density: 0.4f, min: 3, max: 5, smoothing: 1,
                density: 0.45f, min: 2, max: 15, smoothing: 3,
                shift: true, rotate: false);
            csSauron = m_coolingStrategy;
            lsFireStick.SetPen(lpSauronBackground);
            GenericRealtimeFlame dbSauron = new GenericRealtimeFlame();
            RotateFlipType direction = left ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone;
            if (left)
                dbSauron.Location = new Point(xOffset, 0);
            else
                dbSauron.Location = new Point(xCenter, 0);
            dbSauron.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
            dbSauron.Initialize(smokeWidth, smokeHeight, magnification, direction);
            dbSauron.SetCoolingStrategy(csSauron);
            dbSauron.AddShape(lsFireStick);

            return dbSauron;
        }

        private RealtimeLightEffect CreateSauron_PupilOutward(Size size, bool isNarrow, bool hiRez = false)
        {
            int magnification = hiRez ? 2 : 1;
            int width = isNarrow ? 100 : 200;
            int height = isNarrow ? 100 : 100; // pupil calculations are too specific for now. Need to generalize
            int xCenter = (size.Width / 2);
            int xLocation = xCenter - (width * magnification / 2);
            int lookWidth = 30 * magnification;
            // Eye of Sauron
            // LightShape like a cat eye pupil (POC done)
            // Then define a fire blender that goes outward, (DONE)
            // or inward with a MASSIVE cooling map in the pupil? That would be more failthful to the source. V2? Or port Greenshift?
            // Then some directed lightning around the edge? (done)
            // Maybe some sideways fire to make the background (done)
            // Add the towers (in progress)
            LightPen lpSauronEye = new LightPen(fill: 0.28f, 200, 255, useFullRange: true);
            ILightShape lsPupil;
            if (isNarrow)
                lsPupil = new LightShapeSauronV3_PupilNarrow();
            else
                lsPupil = new LightShapeSauronV1_PupilOutward();
            ICoolingStrategy csSauron;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width, height,
                density: 0.3f, min: 3, max: 15, smoothing: 1,
                shift: true, rotate: false); // TODO: This may be worth the cost of rotating, since the pupil will likely be looked at more
            csSauron = m_coolingStrategy;
            lsPupil.SetPen(lpSauronEye);
            RealtimeSplitFire dbSauron = new RealtimeSplitFire
            {
                Inward = false,
                LocationStep = magnification,
                LocationPeriod = isNarrow ? TimeSpan.FromMilliseconds(1000) : TimeSpan.Zero,
                LocationRange = new Rectangle(x: xLocation - lookWidth / 2, y: 0 - lookWidth / 4, width: lookWidth, height: lookWidth / 2),
            };
            dbSauron.Initialize(width, height, magnification);
            dbSauron.SetCoolingStrategy(csSauron);
            dbSauron.AddShape(lsPupil);
            dbSauron.Location = new Point(xLocation, 0);

            if (!isNarrow)
            {
                LightShapeLightning lsLightning = new LightShapeLightning();
                dbSauron.AddShape(lsLightning);
            }

            return dbSauron;
        }

        /// <summary>
        /// Old version that doesn't get the right shape or look, and I'm not sure it ever will.
        /// </summary>
        /// <returns></returns>
        private RealtimeLightEffect CreateSauron_CornersOfEyeInward()
        {
            int width = 200;
            int height = 100;
            int magnification = 1;
            LightPen lpSauronEye = new LightPen(fill: 0.28f, 200, 255, useFullRange: true);
            ILightShape lsSauron = new LightShapeSauronV2_Inward();
            ICoolingStrategy csSauron;

            // Copied from BATMAN! But doesn't work well here because I need long flames to cover a large area.
            // m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 15, smoothing: 0, shift: true, rotate: false);
            //ILightPen lpBatman = new LightPen(fill: 0.7f, min: 54, max: 255, useFullRange: false);
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width, height,
                density: 0.1f, min: 3, max: 25, smoothing: 1,
                //density: 0.2f, min: 3, max: 15, smoothing: 0, // Copied from BATMAN
                shift: true, rotate: false);
            csSauron = m_coolingStrategy;
            lsSauron.SetPen(lpSauronEye);
            RealtimeLightEffect dbSauron = new RealtimeSplitFire
            {
                Inward = true
            };
            dbSauron.Initialize(width, height, magnification);
            dbSauron.SetCoolingStrategy(csSauron);
            dbSauron.AddShape(lsSauron);

            return dbSauron;
        }

        private RealtimeLightEffect CreateSauron_EyeRingInward(Size size, bool hiRez = false)
        {
            int magnification = hiRez ? 2 : 1;
            int width = Math.Min(size.Width, size.Height) / magnification;
            int height = width;

            LightPen lpSauronEye = new LightPen(fill: 0.28f, 200, 255, useFullRange: false);
            ILightShape lsSauron = new LightShapeBorgRing();
            ICoolingStrategy csSauron;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width, height,
                density: 0.1f, min: 3, max: 25, smoothing: 1,
                shift: true, rotate: false);
            csSauron = m_coolingStrategy;
            lsSauron.SetPen(lpSauronEye);
            RealtimeLightEffect dbSauron = new RealtimeSplitFire
            {
                Inward = true
            };
            dbSauron.Initialize(width, height, magnification);
            dbSauron.SetCoolingStrategy(csSauron);
            dbSauron.AddShape(lsSauron);
            dbSauron.Location = new Point((size.Width - width * magnification) / 2, 0);

            return dbSauron;
        }

        /// <summary>
        /// Caller is responsible for setting the palette
        /// </summary>
        /// <returns></returns>
        RealtimeLightEffect GenerateCandle(bool big = true)
        {
            int fireWidth = 21;
            int fireHeight = big ? 50 : 34;
            int magnification = 4;

            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight,
                density: 0.4f, min: 5, max: 13, smoothing: 5,
                //density: 0.3f, min: 5, max: 23, smoothing: 5,
                //density: 0.2f, min: 5, max: 29, smoothing: 5,
                shift: true, rotate: true);
            coolingStrategy = m_coolingStrategy;

            ILightPen lpCandle = new LightPen(fill: 0.08f, min: 54, max: 255, useFullRange: true);
            ILightShape lsCandle = new LightShapeCandle();
            lsCandle.SetPen(lpCandle);

            RealtimeLightEffect dbCandle;
            dbCandle = big ? new RealtimeFire() : (RealtimeLightEffect)new RealtimeCandleflame();
            dbCandle.Initialize(fireWidth, fireHeight, magnification);
            dbCandle.SetCoolingStrategy(coolingStrategy);
            dbCandle.AddShape(lsCandle);

            return dbCandle;
        }

        bool m_fBigRainbowFire = true;
        private void buttonRainbowFire_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            m_fBigRainbowFire = !m_fBigRainbowFire;

            m_dbSprites.AddRange(CreateRainbowFlames(m_fBigRainbowFire));

            buttonDemo_Click(null, null);
        }

        private List<SimpleSprite> CreateRainbowFlames(bool fBigRainbowFire)
        {
            List<SimpleSprite> dbSprites = new List<SimpleSprite>();
            Color[] colors = {
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Violet,
                Color.Cyan,
                Color.Magenta,
                Color.FromArgb(0, 255, 128), // BlueGreen
                Color.Indigo,
                Color.Black,
                Color.White,
                Color.Transparent, // REALISTIC
            };
            Color[] colors4Point = {
                Color.Transparent, // REALISTIC
                Color.OrangeRed, // Kinda like it better than 4-point "realistic" :p
                Color.FromArgb(0, 255, 128), // BlueGreen
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Violet,
                Color.Cyan,
                Color.Magenta,
            };

            Color[] palCandle;
            int left;
            int top;
            int width = -1;
            int height = -1;
            int magnification = -1;
            int bufferX = 45;
            int bufferY = 1;
            int iCandle = 0;
            foreach (Color color in colors)
            {
                if (color == Color.Transparent)
                    palCandle = PalRealisticFire.New();
                else
                    palCandle = PalRealisticFlameCurve.New(color);
                RealtimeLightEffect dbCandle = GenerateCandle(big: fBigRainbowFire);
                dbCandle.SetPalette(palCandle);

                // LAZY INITIALIZATION!
                if (width <= 0)
                {
                    width = dbCandle.Width;
                    height = dbCandle.Height;
                    magnification = dbCandle.Magnification;
                }

                left = (width * magnification + bufferX) * (iCandle % 8);
                top = (height * magnification + bufferY) * (iCandle / 8);
                if (!fBigRainbowFire)
                    top += 100;
                dbCandle.Location = new Point(x: left, y: top);

                dbSprites.Add(dbCandle);
                ++iCandle;
            }

            foreach (Color color in colors4Point)
            {
                if (color == Color.Transparent)
                {
                    Color c2 = Color.FromArgb(red: 255, green: 185, blue: 0);
                    Color c3 = Color.FromArgb(red: 255, green: 255, blue: 127);
                    Color c4Blue = Color.FromArgb(212, 212, 255);
                    palCandle = PalFourPointLinear.New(Color.Black, c2, c3, c4Blue);
                }
                else
                    palCandle = PalFourPointLinear.New(color);
                RealtimeLightEffect dbCandle = GenerateCandle(big: fBigRainbowFire);
                dbCandle.SetPalette(palCandle);
                left = (width * magnification + bufferX) * (iCandle % 8);
                top = (height * magnification + bufferY) * (iCandle / 8);
                if (!fBigRainbowFire)
                    top += 100;
                dbCandle.Location = new Point(x: left, y: top);

                dbSprites.Add(dbCandle);
                ++iCandle;
            }

            // Unsure if this actually speeds anything up, it's about the same speed
            // It's HALF the speed on Linux, so abandon this. Detecting Linux so we
            // can use a faster InterpolationMode in Windows is a better option at this point
            //CompoundSprite dbOptimizer = new LayeredSprite();
            //dbOptimizer.Initialize(1024, 600, 1);
            //dbOptimizer.AddRange(dbSprites);
            //dbSprites.Clear();
            //dbSprites.Add(dbOptimizer);

            return dbSprites;
        }

        private void buttonSauron_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            LayeredSprite dbSauron = CreateEyeOfSauronV3();

            VectorSauronTowerSprite tower = new VectorSauronTowerSprite();
            m_dbSprites.Add(dbSauron);
            m_dbSprites.Add(tower);

            //TextSprite "Eye see you, did you bring the ring?"
            Point topLeft = new Point(270, 300);
            TextSprite text = new TextSprite()
            {
                Color = Color.PaleGoldenrod, // Color.LightGoldenrodYellow,
                //Text = "Eye see you,\ngive me the ring.",
                Text = "Working, but it's OK to\nchat (I miss people :')",
                Location = topLeft,
                LocationRange = new Rectangle(topLeft.X, topLeft.Y, 100, 10),
                LocationPeriod = TimeSpan.FromSeconds(SecondsBeforeMovingTextAround),
            };
            m_dbSprites.Add(text);

            buttonDemo_Click(null, null);
        }

        private void buttonDemoFHL_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();

            //THE SHORTENED CONDENSED VERSION:
            //Every FHL I tackle a new problem to extend this Raspberry Pi project.

            //(AWAY) like realtime generated candle flame,
            //(BUSY)fire drawn in fun shapes,
            //(OOF) lightning,
            //(Available)and plasma

            //I spent the last 2 FHLs redesigning and re-factoring everything because it was just getting way too cumbersome, and hard to modify.

            //This time I’m back to making interesting realtime graphics. (DO NOT DISTURB)

            //This time due to the refactor I am now able to re-combine many of the previous components like Legos and create(CLICK) the Eye of Sauron.

            // Steps:
            // 1) Small flame
            // 2) Large flame
            // 3) Batman
            // 4) Lightning
            // 5) Plasma
            // ) Hero
            // ) Sauron


            TextSprite text = new TextSprite
            {
                LocationRange = new Rectangle(0, 0, 50, 75),
            };
            m_dbText = text;

            historyDemoPeriod = TimeSpan.FromMilliseconds(6000);
            timer2 = new System.Threading.Timer(
                new TimerCallback(ThreadCallbackFhlDemo),
                null,
                TimeSpan.Zero,
                historyDemoPeriod);

            buttonDemo_Click(sender, e);
        }

        //TimeSpan historyDemoPeriod = TimeSpan.FromMilliseconds(9000);
        //int m_iDemoHistoryState = 0;
        private void ThreadCallbackFhlDemo(object state)
        {
            Rectangle fullRange = new Rectangle(0, 0, 50, 75);
            //Func<int>[] variousStages =
            Action[] variousStages =
            {
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbSprites.Clear();
                    SimpleCandle();
                    int fireWidth = m_dbSprite.Width;
                    int fireHeight = 70;
                    int magnification = 7;
                    m_dbSprite.Initialize(fireWidth, fireHeight, magnification);
                    int left = (this.Width - fireWidth * magnification) / 2;
                    int top = (this.Height - fireHeight * magnification);
                    m_dbSprite.Location = new Point(left, top);
                    m_dbText.Text = "Every FHL, I tackle a new problem to extend\nthis Raspberry Pi project.";
                    m_dbText.SetLocationParameters(newLocation: Point.Empty, range: fullRange);
                    m_dbText.Color = Color.Red;
                    m_dbText.SetLocationParameters(range: new Rectangle(0, 0, 50, 75));
                    m_dbSprites.Add(m_dbSprite); // Add the candle
                    m_dbSprites.Add(m_dbText); // Add the text sprite
                    ICoolingStrategy coolingStrategy;
                    m_coolingStrategy = new CoolingStrategyMap();
                    m_coolingStrategy.SetMapParameters(width: m_genericFlame.Width, height: m_genericFlame.Height,
                        density: 0.4f, min: 5, max: 13, smoothing: 5,
                        shift: true, rotate: true);
                    coolingStrategy = m_coolingStrategy;
                    m_genericFlame.SetCoolingStrategy(coolingStrategy);
                    m_genericFlame.SetPixelMatrix(f8: true, f5: true, f1: true, f2: true, f3: true);
                    m_genericFlame.SetPalette(PaletteGenerator.MakeTransparent(PalRealisticFire.New()));
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "FHL1: realtime generated candle flame.";
                    m_dbText.Color = Color.DarkOrange;
                    Rectangle partialRange = new Rectangle(fullRange.Location, fullRange.Size) { Width = 0 };
                    m_dbText.SetLocationParameters(newLocation: new Point(0, partialRange.Height),
                        smoothTransition: true,
                        period: historyDemoPeriod,
                        range: partialRange);
                    m_genericFlame.SetPixelMatrix(f5: true, f1: true, f2: true, f3: true);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "FHL2: Fire drawn in FUN SHAPES!";
                    m_dbText.Font = new Font(m_dbText.Font, FontStyle.Regular);
                    m_dbSprites.Clear();
                    m_dbText.Color = Color.Yellow;
                    DemoBatman(multithreaded: true);
                    m_dbSprites.Add(m_dbSprite);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    TimeSpan tsMax = TimeSpan.FromSeconds(3);
                    TimeSpan tsHalf = TimeSpan.FromTicks(historyDemoPeriod.Ticks / 2);
                    TimeSpan ts = tsHalf.CompareTo(tsMax) < 0 ? tsHalf : tsMax;
                    timer2.Change(ts, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Color = Color.LightCyan;
                    m_dbText.Text = "FHL3: Lightning,\nwhich was just a step toward making...";
                    m_dbSprites.Clear();
                    DemoLightning();
                    m_dbSprite.Initialize(m_dbSprite.Width * 2, m_dbSprite.Height * 2, 1);
                    m_dbSprites.Add(m_dbSprite);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Color = Color.LightGreen;
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "FHL3: ...plasma Borg Regeneration Disc.";
                    // Reset the text period timer so it randomly moves
                    // as the pictures change.
                    m_dbText.SetLocationParameters(
                        newLocation: Point.Empty,
                        period: historyDemoPeriod);
                    m_dbSprites.Clear();
                    DemoBorg();
                    m_dbSprite.Location.Offset(0, 50);
                    m_dbSprites.Add(m_dbSprite);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    // I spent the last 2 FHLs redesigning and re-factoring everything because it was just getting way too cumbersome, and hard to modify.
                    m_dbText.Text = "FHL4&5: I spent a couple FHLs redesigning and\nre-factoring everything...";
                    m_dbText.Color = Color.LightSkyBlue;
                    // Demo RainBORG
                    m_dbSprites.Clear();
                    DemoPlasmaRainbow();
                    foreach (SimpleSprite s in m_dbSprites)
                    {
                        // 0, 196, 392
                        int addY = 150;
                        if (s.Location.Y > 0)
                            addY -= 50;
                        if (s.Location.Y > 200)
                            addY -= 50;
                        Point moveDown = new Point(0, addY);
                        s.Location.Offset(moveDown);
                    }
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "FHL4&5: And then recombined all these pieces\n" +
                    "from the previous iterations into...";
                    m_dbText.Font = new Font(m_dbText.Font, FontStyle.Regular);
                    m_dbText.Color = Color.SkyBlue;
                    Rectangle partialRange = new Rectangle(fullRange.Location, fullRange.Size) { Width = 0 };
                    m_dbText.SetLocationParameters(newLocation: new Point(0, 0),
                        smoothTransition: true,
                        period: historyDemoPeriod,
                        range: partialRange);
                    m_dbSprites.Clear();
                    m_dbSprites.AddRange(CreateRainbowFlames(fBigRainbowFire: false));
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    // Let Sauron go for double length, since I'm speeding up the demo
                    timer2.Change(historyDemoPeriod.Add(historyDemoPeriod), TimeSpan.Zero);
                    m_dbText.Color = Color.OrangeRed;
                    LayeredSprite dbEyeOfSauron = CreateEyeOfSauronV3();
                    int left = (this.Width - dbEyeOfSauron.Width * dbEyeOfSauron.Magnification) / 2;
                    int top = (this.Height - dbEyeOfSauron.Height * dbEyeOfSauron.Magnification) / 2;
                    dbEyeOfSauron.Location = new Point(left, top);
                    m_graph.Clear(Color.Black);
                    m_dbText.Text = "FHL6: the Eye of Sauron!";
                    m_dbSprites.Clear();
                    m_dbSprites.Add(dbEyeOfSauron);
                    m_dbSprites.Add(m_dbText);
                }),
                new Action(() =>
                {
                    // Let Sauron go for extra
                    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                    m_dbText.Text = "And now I use these graphics to communicate\n" +
                    "whether or not I'm busy Working.";
                }),
                new Action(() =>
                {
                    m_graph.Clear(Color.Black);
                    TimeSpan ts = TimeSpan.FromTicks(historyDemoPeriod.Ticks * 2);
                    timer2.Change(ts, TimeSpan.Zero);
                    buttonDndStatus_Click(null, null);
                }),
                //new Action(() =>
                //{
                //    timer2.Change(historyDemoPeriod, TimeSpan.Zero);
                //}),
            };

            int ii = m_iDemoHistoryState++ % variousStages.Length;
            m_callbackToChangeStuff = variousStages[ii];
        }

#if false // ExtraLargeRainbow (too slow)

        private void LargerRainbowFire_Click(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            m_fBigRainbowFire = !m_fBigRainbowFire;

            Color[] colors = {
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Violet,
                Color.Cyan,
                Color.Magenta,
                Color.Transparent, // REALISTIC
                Color.FromArgb(0, 255, 128), // BlueGreen
                Color.Indigo,
                Color.Black,
                Color.White,
            };
            Color[] colors4Point = {
                Color.FromArgb(0, 255, 128), // BlueGreen
                Color.Black,
                Color.White,
                Color.MediumVioletRed,
                Color.Transparent, // REALISTIC
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Violet,
                Color.Cyan,
                Color.Magenta,
                Color.OrangeRed, // Kinda like it better than 4-point "realistic" :p
            };

            Color[] palCandle;
            int left;
            int top;
            int width = -1;
            int height = -1;
            int magnification = -1;
            int bufferX = 33;
            int bufferY = 1;
            int iCandle = 0;
            foreach (Color color in colors)
            {
                if (color == Color.Transparent)
                    palCandle = PalRealisticFire.New();
                else
                    palCandle = PalRealisticFlameCurve.New(color);
                AbstractRealtimeLightEffect dbCandle = GenerateCandle(big: m_fBigRainbowFire);
                dbCandle.SetPalette(palCandle);

                // LAZY INITIALIZATION!
                if (width <= 0)
                {
                    width = dbCandle.Width;
                    height = dbCandle.Height;
                    magnification = dbCandle.Magnification;
                }

                left = (width * magnification + bufferX) * (iCandle % 9);
                top = (height * magnification + bufferY) * (iCandle / 9);
                dbCandle.Location = new Point(x: left, y: top);

                m_dbSprites.Add(dbCandle);
                ++iCandle;
            }

            foreach (Color color in colors4Point)
            {
                if (color == Color.Transparent)
                {
                    Color c2 = Color.FromArgb(red: 255, green: 185, blue: 0);
                    Color c3 = Color.FromArgb(red: 255, green: 255, blue: 127);
                    Color c4Blue = Color.FromArgb(212, 212, 255);
                    palCandle = PalFourPointLinear.New(Color.Black, c2, c3, c4Blue);
                }
                else
                    palCandle = PalFourPointLinear.New(color);
                AbstractRealtimeLightEffect dbCandle = GenerateCandle(big: m_fBigRainbowFire);
                dbCandle.SetPalette(palCandle);
                left = (width * magnification + bufferX) * (iCandle % 9);
                top = (height * magnification + bufferY) * (iCandle / 9);
                dbCandle.Location = new Point(x: left, y: top);

                m_dbSprites.Add(dbCandle);
                ++iCandle;
            }

            buttonDemo_Click(null, null);
        }

        private void ExtraLargeRainbow(object sender, EventArgs e)
        {
            m_dbSprites.Clear();
            m_fBigRainbowFire = !m_fBigRainbowFire;

            Color[] colors = {
                // REALISTIC is #1 and handled outside the loop!
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Green,
                Color.Blue,
                Color.Violet,
                Color.Cyan,
                Color.Magenta,
                Color.Transparent,
                Color.Indigo,
                Color.FromArgb(0, 255, 128), // BlueGreen
                Color.OrangeRed,
                Color.Black,
                Color.White,
            };

            Color[] palCandle;
            palCandle = PalRealisticFire.New();
            AbstractRealtimeLightEffect realCandle = GenerateCandle(big: m_fBigRainbowFire);
            realCandle.SetPalette(palCandle);

            int left;
            int top;
            realCandle.Location = new Point(x: 0, y: 0);

            m_dbSprites.Add(realCandle);


            int width = realCandle.Width;
            int height = realCandle.Height;
            int magnification = realCandle.Magnification;
            int bufferX = 21;
            int bufferY = 1;
            int iCandle = 1;
            foreach (Color color in colors)
            {
                palCandle = PalRealisticFlameCurve.New(color);
                AbstractRealtimeLightEffect dbCandle = GenerateCandle(big: m_fBigRainbowFire);
                dbCandle.SetPalette(palCandle);
                left = (width * magnification + bufferX) * (iCandle % 5);
                top = (height * magnification + bufferY) * (iCandle / 5);
                dbCandle.Location = new Point(x: left, y: top);

                m_dbSprites.Add(dbCandle);
                ++iCandle;
            }


            Color c2 = Color.FromArgb(red: 255, green: 185, blue: 0);
            Color c3 = Color.FromArgb(red: 255, green: 255, blue: 127);
            Color c4Blue = Color.FromArgb(212, 212, 255);
            palCandle = PalFourPointLinear.New(Color.Black, c2, c3, c4Blue);
            realCandle = GenerateCandle(big: m_fBigRainbowFire);
            realCandle.SetPalette(palCandle);
            realCandle.Location = new Point(x: Width / 2, y: 0);

            m_dbSprites.Add(realCandle);

            iCandle = 1;
            foreach (Color color in colors)
            {
                palCandle = PalFourPointLinear.New(color);
                AbstractRealtimeLightEffect dbCandle = GenerateCandle(big: m_fBigRainbowFire);
                dbCandle.SetPalette(palCandle);
                left = Width / 2 + (width * magnification + bufferX) * (iCandle % 5);
                top = (height * magnification + bufferY) * (iCandle / 5);
                dbCandle.Location = new Point(x: left, y: top);

                m_dbSprites.Add(dbCandle);
                ++iCandle;
            }

            buttonDemo_Click(null, null);
        }
#endif // ExtraLargeRainbow (too slow)
    }
}
