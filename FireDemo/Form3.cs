using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FireDemo
{
    public partial class Form3 : Form
    {
        //Color[] m_palette;
        CoolingStrategyMap m_coolingStrategy;
        //ILightPen m_lightPen;
        List<ILightShape> m_lightShapes = new List<ILightShape>();
        SimpleSprite m_dbSprite;
        List<SimpleSprite> m_dbSprites = new List<SimpleSprite>();
        //private GenericRealtimeFlame m_genericFlame;
        //int m_framesPerSecond = 64;
        System.Threading.Timer timer2;
        bool ClosingSoShutdownStuff = false;

        public Form3()
        {
            InitializeComponent();

            //TimerCallback callback = new TimerCallback(() =>
            //{
            //    return null;
            //});
            this.FormClosing += Form3_FormClosing;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ClosingSoShutdownStuff = true;
            timer2.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            timer2 = new System.Threading.Timer(MyTimerCallback, null, 0, Timeout.Infinite);

            DemoBatman();
            DemoBatman_LowerCooling_HigherFire();
            this.BackColor = Color.Black;
        }

        // Want to implement using alternate suggestion:
        // https://stackoverflow.com/questions/11020710/is-graphics-drawimage-too-slow-for-bigger-images
        // TODO: JRDV: NEAT! I seem to have *actually* doubled the framerate!
        // Verify that I'm doing it right, and maybe see if it translates to the Pi?
        // Nope, doesn't work on the Raspberry Pi for some reason it renders exactly one frame, and that's it.
        // Double the framerate in Windows is meaningless if it doesn't translate to Pi.
        //delegate void TTimerCallback(string str);
        public void MyTimerCallback(Object obj)
        {
            if (this.ClosingSoShutdownStuff)
                return;

            // 1) How to check whether we're on the correct thread?
            // 2) How to create a delegate to Invoke to the correct thread?
            //Rectangle rc = new Rectangle(100, 1000, 1001, 501);
            //Rectangle rc = new Rectangle(this.Location, this.Size);
            //this.Invalidate(rc);
            this.Invalidate();

            //if (System.Threading.Thread.CurrentThread.IsBackground)
            //{
            //    TTimerCallback del4 = name => {
            //        this.Invalidate();
            //        // TODO: JRDV: Render to buffer here, and then queue an OnPaint call?
            //        //timer1_Tick(null, null);
            //    };
            //    this.Invoke(del4, "");
            //}
            //else
            //{
            //}
        }


        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    OnPaintOneFrame(e);
        //}

        bool m_fNeedBackgroundFill = true;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.Black, 0.0f, 0.0f, 1.0f, 1.0f);
            if (m_fNeedBackgroundFill)
            {
                e.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Size.Width, this.Size.Height);
                //e.Graphics.DrawString("FPS", Font, Brushes.White, 0.0f, 0.0f);
                m_fNeedBackgroundFill = false;
            }
            OnPaintOneFrame(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            m_fNeedBackgroundFill = true;
        }

        void OnPaintOneFrame(PaintEventArgs e)
        {
            UpdateFramerate();
            // Draw the frame once per tick.
            if (m_dbSprites.Count > 0)
            {
                foreach (SimpleSprite sprite in m_dbSprites)
                {
                    sprite.RenderOneFrameToScreen(e.Graphics);
                }
            }
            else
                m_dbSprite.RenderOneFrameToScreen(e.Graphics);

            // That is strange. 0 dueTime means this runs about 180-240 FPS on my desktop.
            // 1 dueTime makes it run SLOWER than the original timer1_Tick implementation,
            // and the framerate is inconsistent, fluctuating from 30-50-60 and back to 30 FPS.
            //
            // Given how this is proving problematic on Linux, and inconsistent on Windows,
            // I think my effort is better spent on paralellizing the implementation,
            // and only later explore this path again. Yes I know this is theoretically
            // not as good, but for now it IS producing better results.
            timer2.Change(dueTime: 0, period: Timeout.Infinite);
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

            Color[] palFire = PalRealisticFire.New();
            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.4f, min: 5, max: 7, smoothing: 0);
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 15, smoothing: 0);
//            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 25, smoothing: 0);
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 25, smoothing: 0, shift: true, rotate: false);
            ////m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 25, smoothing: 0);
            coolingStrategy = m_coolingStrategy;

            // CoolingStrategyMap seems to be a bottleneck for Batman flames!
            // The CoolingStrategyMap.at takes 9-10% of the time
            // CoolingStrategyMap.FillCoolingMap takes about 30% of the time
            // Meaning the CoolingMap takes 40% of the time!
            // A Constant cooling map will significantly improve performance, or simply not rotating it
            // should help the 30% stat... assuming it looks a whole lot better with a cooling map...
            // *sigh* It does look a lot better with the map

            //coolingStrategy = new CoolingStrategyConst(3);
            ILightPen lpBatman = new LightPen(fill: 0.75f, min: 54, max: 255, useFullRange: false);
            ILightShape lsBatman = new LightShapeBatman();
            lsBatman.SetPen(lpBatman);

            //AbstractRealtimeLightEffect dbBatman = new RealtimeFireBatLogoOptimized();
            AbstractRealtimeLightEffect dbBatman = new RealtimeFireBatLogoOptimizedMT_ThreadPool();
            //dbBatman = new RealtimeCandleflame();
            dbBatman.Initialize(fireWidth, fireHeight, magnification);
            dbBatman.SetCoolingStrategy(coolingStrategy);
            dbBatman.SetPalette(palFire);
            dbBatman.AddShape(lsBatman);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2 - 20;
            dbBatman.Location = new Point(x: left, y: top);

            //m_palette = palFire;
            //m_lightPen = lpBatman;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsBatman);
            m_dbSprite = dbBatman;
        }

        private void DemoBatman_LowerCooling_HigherFire()
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

            Color[] palFire = PalRealisticFire.New();
            ICoolingStrategy coolingStrategy;
            m_coolingStrategy = new CoolingStrategyMap();
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.4f, min: 5, max: 7, smoothing: 0, shift: true, rotate: false);
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 15, smoothing: 0, shift: true, rotate: false);
            //            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 25, smoothing: 0);
//            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 15, smoothing: 0, shift: true, rotate: false);
            ////m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 25, smoothing: 0, shift: true, rotate: false);
            coolingStrategy = m_coolingStrategy;

            // CoolingStrategyMap seems to be a bottleneck for Batman flames!
            // The CoolingStrategyMap.at takes 9-10% of the time
            // CoolingStrategyMap.FillCoolingMap takes about 30% of the time
            // Meaning the CoolingMap takes 40% of the time!
            // A Constant cooling map will significantly improve performance, or simply not rotating it
            // should help the 30% stat... assuming it looks a whole lot better with a cooling map...
            // *sigh* It does look a lot better with the map

            //coolingStrategy = new CoolingStrategyConst(3);
            ILightPen lpBatman = new LightPen(fill: 0.7f, min: 54, max: 255, useFullRange: false);
            ILightShape lsBatman = new LightShapeBatman();
            lsBatman.SetPen(lpBatman);

            //AbstractRealtimeLightEffect dbBatman = new RealtimeFireBatLogoOptimized();
            AbstractRealtimeLightEffect dbBatman = new RealtimeFireBatLogoOptimizedMT_ThreadPool();
            //dbBatman = new RealtimeCandleflame();
            dbBatman.Initialize(fireWidth, fireHeight, magnification);
            dbBatman.SetCoolingStrategy(coolingStrategy);
            dbBatman.SetPalette(palFire);
            dbBatman.AddShape(lsBatman);
            left = (this.Width - fireWidth * magnification) / 2;
            top = (this.Height - fireHeight * magnification) / 2 - 20;
            dbBatman.Location = new Point(x: left, y: top);

            //m_palette = palFire;
            //m_lightPen = lpBatman;
            m_lightShapes.Clear();
            m_lightShapes.Add(lsBatman);
            m_dbSprite = dbBatman;
        }

        int iFrame = 0;
        DateTime dtEnd = DateTime.Now;
        private void UpdateFramerate()
        {
            ++iFrame;
            DateTime dtNow = DateTime.Now;

            if (dtNow >= dtEnd)
            {
                this.Text = string.Format("FPS: {0}", iFrame);
                iFrame = 0;
                dtEnd = DateTime.Now.AddSeconds(1);
            }
        }
    }
}
