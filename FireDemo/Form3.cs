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
        System.Threading.Timer timer2;

        public Form3()
        {
            InitializeComponent();
            m_graph = this.CreateGraphics(); // TODO: JRDV Delete this!!!

            //TimerCallback callback = new TimerCallback(() =>
            //{
            //    return null;
            //});
            timer2 = new System.Threading.Timer(MyTimerCallback, null, 10, 10);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            DemoBatman();
            DemoBatman_LowerCooling_HigherFire();
            this.BackColor = Color.Black;
            timer1.Interval = (int)(1000 / m_framesPerSecond);
            //timer1.Enabled = true;
        }

        delegate void TTimerCallback(string str);
        public void MyTimerCallback(Object obj)
        {
            // 1) How to check whether we're on the correct thread?
            // 2) How to create a delegate to Invoke to the correct thread?
            if (System.Threading.Thread.CurrentThread.IsBackground)
            {
                TTimerCallback del4 = name => { timer1_Tick(null, null); };
                this.Invoke(del4, "");
            }
            else
            {
                timer1_Tick(null, null);
            }
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
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.4f, min: 5, max: 7, smoothing: 0);
            //m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.3f, min: 5, max: 15, smoothing: 0);
            //            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 25, smoothing: 0);
            m_coolingStrategy.SetMapParameters(width: fireWidth, height: fireHeight, density: 0.2f, min: 3, max: 15, smoothing: 0, shift: true, rotate: false);
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
            ILightPen lpBatman = new LightPen(fill: 0.7f, min: 54, max: 255, useFullRange: false);
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

        // TODO: JRDV: Throw this away!!! Want to implement using alternate suggestion:
        // https://stackoverflow.com/questions/11020710/is-graphics-drawimage-too-slow-for-bigger-images
        int iFrame = 0;
        DateTime dtEnd = DateTime.Now;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ++iFrame;
            DateTime dtNow = DateTime.Now;

            if (dtNow >= dtEnd)
            {
                this.Text = string.Format("FPS: {0}", iFrame);
                iFrame = 0;
                dtEnd = DateTime.Now.AddSeconds(1);
            }

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
    }
}
