using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace FireDemo
{
	/// <summary>
	/// An abstract base class for single-frame, pixel-based sprites that
	/// manages an internal bitmap and provides methods for scaling,
	/// compositing, pixel access, and rendering on a graphics surface.
	/// </summary>
	abstract class SimpleSprite
	{
		protected Bitmap Form { get; set; }
		public int Height { get; protected set; }
		public int Width { get; protected set; }
		public int Magnification = 1;
		public Point Location;
		public InterpolationMode InterpolationMode { get; set; }
		public CompositingMode CompositingMode { get; set; }

		/// <summary>
		/// Sets the sprite's base dimensions and magnification.
		/// </summary>
		/// <param name="width">The width in pixels for the sprite surface.</param>
		/// <param name="height">The height in pixels for the sprite surface.</param>
		/// <param name="magnification">The scaling multiplier used during rendering.</param>
		public virtual void Initialize(int width, int height, int magnification)
		{
			this.Magnification = magnification;
			this.Width = width;
			this.Height = height;

			//Form = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			//Form = new Bitmap(width, height, PixelFormat.Format16bppRgb565);
			//Form = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

			// 32bit with Alpha seems slightly faster, or at the very least not noticibly slower
			// for the largest fire shapes, so no need to parameterize the value for now.
			// Therefore this is fast enough for now.
			Form = new Bitmap(width, height, PixelFormat.Format32bppArgb);
		}

		/// <summary>
		/// Gets the color of the specified pixel.
		/// </summary>
		/// <param name="x">Horizontal coordinate of the pixel.</param>
		/// <param name="y">Vertical coordinate of the pixel.</param>
		/// <returns>The color of the pixel at the specified coordinates.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Throws an ArgumentOutOfRangeException if the coordinates are outside the sprite's bounds.</exception>
		public Color GetPixel(int x, int y)
		{
			return Form.GetPixel(x, y);
		}

		/// <inheritdoc cref="SimpleSprite"/>
		public SimpleSprite()
		{
			this.InterpolationMode = Util.IsLinux ? InterpolationMode.Bicubic : InterpolationMode.HighQualityBicubic; // Default to BEST quality
			this.CompositingMode = CompositingMode.SourceOver; // Default to best QUALITY option (no blinking
															   // due to blanking out the ENTIRE graph)
															   // Caller can use SourceCopy for some extra speed
															   // if they know it's safe (i.e. only a single bitmap
															   // will be rendered per frame)
			Location = new Point(0, 0);
			Magnification = 1;
		}

		/// <summary>
		/// Draws the sprite onto the graphics surface, applying the sprite's own compositing and interpolation settings.
		/// </summary>
		/// <param name="graph">The graphics surface on which to draw the sprite.</param>
		protected void DrawOn(Graphics graph)
		{
			CompositingMode cm = graph.CompositingMode; // Default SourceOver
			InterpolationMode im = graph.InterpolationMode; // Default Bilinear

			graph.CompositingMode = this.CompositingMode;
			if (Magnification == 1)
			{
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;
				// NOTE: While the Width/Height parameters to DrawImageUnscaled are
				// unused on Windows platforms, Mono on Linux respects the paremeters,
				// therefore they are required here.
				graph.DrawImageUnscaled(Form, Location.X, Location.Y, Width, Height);
			}
			else
			{
				graph.InterpolationMode = this.InterpolationMode;
				graph.DrawImage(Form, Location.X, Location.Y, Width * Magnification, Height * Magnification);
			}

			graph.CompositingMode = cm;
			graph.InterpolationMode = im;
		}

		// "Simple" sprites don't have frames. Dynamic sprites have frames,
		// so this probably should go in a derived class, but
		// what would that look like? TODO: Consider
		// 
		// DrawOn() => { RenderOneFrame(); base.DrawOn(graph); ProgressOneFrame(); }
		public virtual void RenderOneFrameToScreen(Graphics graph)
		{
			DrawOn(graph);
		}
	}

	// May want to introduce a GridSprite/SpriteGrid in the future, like for a game board.

	/// <summary>
	/// Aggregates multiple sprite layers into a single, complex composition.
	/// </summary>
	/// <remarks>
	/// <para>Blends multiple sprites using its assigned <see cref="CompositingMode"/>
	/// to reduce flicker during rendering.</para>
	/// <para>Typically used to eliminate flicker when drawing overlapping
	/// <see cref="DynamicSprite"/>s.<br/>It stands in contrast to the other "Background"
	/// SpriteCompositor <see cref="SpriteVideoGameBackground"/>, which is intended for
	/// flattening static images.</para>
	/// </remarks>
	class LayeredSprite : SimpleSprite
	{
		readonly List<SimpleSprite> m_dbSprites = new List<SimpleSprite>();

		/// <inheritdoc cref="LayeredSprite"/>
		public LayeredSprite() { }

		/// <summary>
		/// Adds a single sprite as a new layer to the composition.
		/// </summary>
		/// <param name="sprite">The sprite layer to add.</param>
		public void Add(SimpleSprite sprite)
		{
			m_dbSprites.Add(sprite);
		}
		/// <summary>
		/// Adds a collection of sprites as new layers to the composition.
		/// </summary>
		/// <param name="sprites">The collection of sprites to add.</param>
		public void AddRange(IEnumerable<SimpleSprite> sprites)
		{
			m_dbSprites.AddRange(sprites);
		}

		/// <summary>
		/// Temporary override to disgnose issues with overlay
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="magnification"></param>
		public override void Initialize(int width, int height, int magnification)
		{
			base.Initialize(width, height, magnification);
			m_internalGraphics = Graphics.FromImage(Form);
		}

		Graphics m_internalGraphics;
		public override void RenderOneFrameToScreen(Graphics graph)
		{
			Graphics g = m_internalGraphics;

			// blank the bitmap before compositing, to clear out the
			// tansparent areas that never update (i.e. fix stuck pixels)
			g.Clear(Color.Black);

			foreach (SimpleSprite sprite in m_dbSprites)
			{
				sprite.RenderOneFrameToScreen(g);
			}

			DrawOn(graph);
		}
	}

	class HeroHoldingItem : SimpleSprite
	{
		public HeroHoldingItem()
		{
			int[][] hero;
			Color[] pal;
			Color darkBrown, darkGreen, lightGreen, orangeShoe, pinkSkin, redShoe, reddishBrownHat, yellow, redMouth;

			Width = 16;
			Height = 20;

			lightGreen = Color.LightGreen;
			darkGreen = Color.Green;
			yellow = Color.Yellow;
			orangeShoe = Color.Orange;
			redShoe = Color.Red;
			redMouth = Color.Red;
			pinkSkin = Color.Salmon;
			reddishBrownHat = Color.Brown;
			darkBrown = Color.Brown;

			pal = new Color[]{
				Color.Transparent, //0 Transparent
				Color.Black, // "1"
				pinkSkin, // "2"
				lightGreen, // "3"
				darkGreen, // "4"
				darkBrown, // "5"
				reddishBrownHat, // "6"
				yellow, // "7"
				redShoe, // "8"
				orangeShoe, // "9"
				Color.White, // "10"
				redMouth, // "11"
			};

			//"1 black, 2 pink, 3 ltGreen, 4 dkGreen, 5 brown, 6 hatBrown, 7yellow, 8 orangeShoe, 9 redShoe, 10 eyes, 11 teeth"
			hero = new int[][]
			{
				new int[] { 1, 2, 2, 1, 0, 1, 1, 3, 3, 1, 1, 0, 1, 2, 2, 1, },
				new int[] { 1, 2, 2, 2, 1, 4, 3, 3, 3, 3, 4, 1, 2, 2, 2, 1, },
				new int[] { 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, },
				new int[] { 1, 5, 5, 1, 1, 6, 6, 6, 6, 6, 6, 1, 1, 5, 5, 1, },
				new int[] { 1, 5, 5, 1, 6, 1, 1, 1, 1, 1, 1, 6, 1, 5, 5, 1, },
				new int[] { 0, 1, 5, 1, 1, 10, 1, 2, 2, 1, 10, 1, 1, 5, 1, 0, },
				new int[] { 0, 1, 5, 1, 2, 10, 1, 2, 2, 1, 10, 2, 1, 5, 1, 0, },
				new int[] { 0, 1, 5, 2, 2, 10, 10, 2, 2, 10, 10, 2, 2, 5, 1, 0, },
				new int[] { 0, 1, 5, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 5, 1, 0, },
				new int[] { 0, 0, 1, 5, 1, 2, 2, 11, 11, 2, 2, 1, 5, 1, 0, 0, },
				new int[] { 0, 0, 1, 5, 1, 1, 2, 2, 2, 2, 1, 1, 5, 1, 0, 0, },
				new int[] { 0, 0, 0, 1, 4, 4, 1, 1, 1, 1, 4, 4, 1, 0, 0, 0, },
				new int[] { 0, 0, 0, 1, 4, 3, 4, 4, 4, 4, 3, 4, 1, 0, 0, 0, },
				new int[] { 0, 0, 0, 1, 4, 3, 3, 3, 3, 3, 3, 4, 1, 0, 0, 0, },
				new int[] { 0, 0, 1, 7, 1, 4, 4, 3, 3, 4, 4, 1, 7, 1, 0, 0, },
				new int[] { 0, 1, 4, 4, 7, 7, 1, 7, 7, 1, 7, 7, 4, 4, 1, 0, },
				new int[] { 0, 1, 1, 1, 4, 4, 7, 7, 7, 7, 4, 4, 1, 1, 1, 0, },
				new int[] { 1, 8, 9, 1, 1, 1, 4, 4, 4, 4, 1, 1, 1, 9, 8, 1, },
				new int[] { 1, 8, 8, 9, 1, 0, 1, 1, 1, 1, 0, 1, 9, 8, 8, 1, },
				new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, },
			};
			//"1 black, 2 pink, 3 ltGreen, 4 dkGreen, 5 brown, 6 hatBrown, 7yellow, 8 orangeShoe, 9 redShoe, 10 eyes, 11 teeth"

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = hero[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}

			}
		}
	}


	class OldMan : SimpleSprite
	{
		public OldMan()
		{
			int[][] pixels;
			Color[] pal;
			Color lightBrown, darkBrown, pinkSkin, reddishBrownHat, yellow;

			Width = 16;
			Height = 16;

			lightBrown = Color.SandyBrown;
			yellow = Color.Yellow;
			pinkSkin = Color.Salmon;
			reddishBrownHat = Color.Red;
			darkBrown = Color.Brown;

			lightBrown = Color.FromArgb(red: 236, green: 100, blue: 55);
			//medBrown = Color.FromArgb(red: 168, green: 35, blue: 11);
			darkBrown = Color.FromArgb(red: 71, green: 10, blue: 3);

			pal = new Color[]{
				Color.Transparent, //0 Transparent
				Color.Black, // "1"
				Color.White, // 2
				pinkSkin, // 3
				lightBrown, // 4
				darkBrown, // 5
				reddishBrownHat, // 6
				yellow, // 7
			};

			pixels = new int[][]
			{
				new int[] { 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, },
				new int[] { 0, 0, 0, 0, 7, 3, 7, 3, 3, 7, 3, 7, 0, 0, 0, 0, },
				new int[] { 0, 0, 0, 0, 3, 3, 5, 3, 3, 5, 3, 3, 0, 0, 0, 0, },
				new int[] { 0, 0, 0, 0, 3, 3, 5, 3, 3, 5, 3, 3, 0, 0, 0, 0, },
				new int[] { 0, 0, 0, 0, 4, 7, 3, 3, 3, 3, 7, 4, 0, 0, 0, 0, },
				new int[] { 0, 0, 0, 4, 2, 2, 2, 2, 2, 2, 2, 2, 4, 0, 0, 0, },
				new int[] { 0, 0, 4, 4, 2, 2, 1, 1, 1, 1, 2, 2, 4, 4, 0, 0, },
				new int[] { 5, 4, 4, 2, 2, 4, 2, 2, 2, 2, 4, 2, 2, 4, 4, 5, },
				new int[] { 3, 4, 4, 2, 4, 4, 2, 2, 2, 2, 4, 4, 2, 4, 4, 3, },
				new int[] { 3, 4, 4, 4, 4, 4, 2, 2, 2, 2, 4, 4, 4, 4, 4, 3, },
				new int[] { 3, 4, 6, 6, 5, 4, 4, 2, 2, 4, 4, 5, 6, 6, 4, 3, },
				new int[] { 5, 4, 6, 6, 5, 4, 6, 6, 6, 6, 4, 5, 6, 6, 4, 5, },
				new int[] { 0, 0, 6, 6, 5, 4, 6, 6, 6, 6, 4, 5, 6, 6, 0, 0, },
				new int[] { 0, 0, 0, 5, 6, 6, 6, 6, 6, 6, 6, 6, 5, 0, 0, 0, },
				new int[] { 0, 0, 0, 5, 6, 6, 6, 6, 6, 6, 6, 6, 5, 0, 0, 0, },
				new int[] { 0, 0, 0, 4, 6, 3, 3, 4, 4, 3, 3, 6, 4, 0, 0, 0, },
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = pixels[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}

			}
		}
	}

	class BrickWall : SimpleSprite
	{
		public BrickWall()
		{
			int[][] brick;
			Color[] pal;

			Width = 16;
			Height = 16;

			pal = new Color[]{
				Color.Transparent, //0 Transparent
				Color.Black, // "1"
				Color.Brown, // "2"
			};

			brick = new int[][]
			{
				new int[] { 2, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 2, },
				new int[] { 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 1, 1, 2, 2, 2, 2, },
				new int[] { 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 1, 1, 2, 2, 2, 1, },
				new int[] { 1, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 1, 1, 2, 1, 1, },
				new int[] { 1, 2, 2, 2, 1, 1, 2, 2, 2, 1, 1, 2, 1, 1, 2, 1, },
				new int[] { 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 1, },
				new int[] { 2, 2, 2, 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 2, 1, },
				new int[] { 2, 2, 2, 2, 1, 1, 2, 2, 2, 1, 2, 2, 1, 1, 1, 1, },
				new int[] { 2, 2, 2, 1, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2, 1, 1, },
				new int[] { 2, 2, 2, 1, 1, 1, 1, 2, 2, 1, 2, 2, 2, 2, 1, 1, },
				new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 2, 2, 2, 1, 1, 1, },
				new int[] { 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, },
				new int[] { 2, 1, 1, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 1, 2, },
				new int[] { 2, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 1, 1, 1, 2, },
				new int[] { 2, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
				new int[] { 1, 1, 2, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, },
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = brick[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}

			}
		}
	}

	class TorchHandle : SimpleSprite
	{
		public TorchHandle()
		{
			InitializeBitmap();
		}

		protected void InitializeBitmap(int heightLimit = -1)
		{
			int[][] pixels;
			Color[] pal;

			Width = 8;
			Height = 16;

			if (heightLimit != -1 && heightLimit < Height)
				Height = heightLimit;

			Color lightBrown = Color.FromArgb(red: 236, green: 100, blue: 55);
			Color medBrown = Color.FromArgb(red: 168, green: 35, blue: 11);
			Color darkBrown = Color.FromArgb(red: 71, green: 10, blue: 3);

			pal = new Color[]{
				Color.Transparent, //0 Transparent
				lightBrown, // "1"
				medBrown, // "2"
				darkBrown, // 3
				Color.White // 4
			};

			pixels = new int[][]
			{
				new int[]{1, 4, 4, 4, 4, 4, 4, 1},
				new int[]{1, 1, 1, 1, 1, 1, 1, 1},
				new int[]{0, 3, 3, 3, 3, 3, 3, 0},
				new int[]{0, 2, 1, 1, 2, 2, 2, 0},
				new int[]{0, 0, 3, 3, 3, 3, 0, 0},
				new int[]{0, 0, 0, 3, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = pixels[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}
			}
		}
	}

	class CauldronBase : TorchHandle
	{
		public CauldronBase()
		{
			// The "cauldron" is exactly the same as the first 5 lines of the torch
			InitializeBitmap(heightLimit: 5);
		}
	}

	class SaberHilt : SimpleSprite
	{
		public SaberHilt()
		{
			InitializeBitmap();
		}

		protected void InitializeBitmap(int heightLimit = -1)
		{
			int[][] pixels;
			Color[] pal;

			Width = 8;
			Height = 16;

			if (heightLimit != -1 && heightLimit < Height)
				Height = heightLimit;

			Color lightBrown = Color.LightGray;
			Color medBrown = Color.Silver;
			Color darkBrown = Color.DarkGray;

			pal = new Color[]{
				Color.Transparent, //0 Transparent
				lightBrown, // "1"
				medBrown, // "2"
				darkBrown, // 3
				Color.White // 4
			};

			// This was just a quick prototype for the sword hilt. It doesn't look the way I want
			pixels = new int[][]
			{
				new int[]{1, 4, 4, 4, 4, 4, 4, 1},
				new int[]{1, 1, 1, 1, 1, 1, 1, 1},
				new int[]{0, 3, 3, 3, 3, 3, 3, 0},
				new int[]{0, 2, 1, 1, 2, 2, 2, 0},
				new int[]{0, 0, 3, 3, 3, 3, 0, 0},
				new int[]{0, 0, 0, 3, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
				new int[]{0, 0, 0, 2, 3, 0, 0, 0},
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = pixels[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}
			}
		}
	}

    class Dumpster : SimpleSprite
	{
		public Dumpster()
		{
			InitializeBitmap();
		}

		protected void InitializeBitmap()
		{
			int[][] pixels;
			Color[] pal;

			Width = 45;
			Height = 21;

			Color light = Color.LightGreen;
			Color medium = Color.Green;
			Color dark = Color.DarkGreen;

			pal = new Color[]{
				Color.Transparent, // 0 Transparent
				light, // "1"
				medium, // "2"
				dark, // 3
				Color.Black // 4
			};

			// 5 units high
			// 10 units wide
			// 8 wide front, 2 deep, 1 high for lid
			pixels = new int[][]
			{
				new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 4, 0, 0},
				new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 3, 4, 0, 0},
				new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 3, 3, 2, 4, 4, 0},
				new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 3, 3, 2, 2, 2, 4, 4, 0},
				new int[]{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 2, 2, 2, 4, 4, 0},
				new int[]{0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 0},
				new int[]{3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 3, 3, 3, 4, 4, 0},
				new int[]{3, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 4, 4, 3, 3, 3, 3, 3, 4, 4, 0},
				new int[]{3, 4, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 2, 4, 4, 3, 3, 3, 3, 3, 4, 4, 0},
				new int[]{3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 3, 3, 3, 4, 4, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 4, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 4, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0},
				new int[]{0, 0, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 0, 4, 4, 0, 0, 0},
				new int[]{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0},
				new int[]{0, 0, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = pixels[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}
			}
		}
	}

	class Heart : SimpleSprite
	{
		public Heart(bool empty = false)
		{
			int[][] pixels;
			Color[] pal;

			Width = 8;
			Height = 8;

			pal = new Color[]{
				Color.Transparent, //0 Transparent
				Color.DarkRed, // "1"
				empty ? Color.Black :Color.Red, // 2
			};

			pixels = new int[][]
			{
				new int[] { 0, 0, 1, 0, 1, 0, 0, 0, },
				new int[] { 0, 1, 2, 1, 2, 1, 0, 0, },
				new int[] { 1, 2, 2, 2, 2, 2, 1, 0, },
				new int[] { 1, 2, 2, 2, 2, 2, 1, 0, },
				new int[] { 1, 2, 2, 2, 2, 2, 1, 0, },
				new int[] { 0, 1, 2, 2, 2, 1, 0, 0, },
				new int[] { 0, 0, 1, 2, 1, 0, 0, 0, },
				new int[] { 0, 0, 0, 1, 0, 0, 0, 0, },
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = pixels[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}

			}
		}
	}

	class RectangleSprite : SimpleSprite
	{
		public RectangleSprite(int width, int  height, Color color, bool fill = true)
		{
			Width = width;
			Height = height;

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color c = Color.Transparent;
					if (fill || x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
					{
						c = color;
					}

					Form.SetPixel(x, y, c);
				}

			}
		}
	}

	class XSprite : SimpleSprite
	{
		public XSprite(int width, int height, Color color)
		{
			Width = width;
			Height = height;

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color c = Color.Transparent;
					if (x == y || x + y + 1 == Width)
					{
						c = color;
					}

					Form.SetPixel(x, y, c);
				}

			}
		}
	}

	class YSprite : SimpleSprite
	{
		public YSprite(int width, int height, Color color)
		{
			Width = width;
			Height = height;

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color c = Color.Transparent;
					if (y > Height / 2)
					{
						if (x == Width / 2
							|| x == Width / 2 - 1)
							c = color;
					}
					else if (x == y || x + y + 1 == Width)
					{
						c = color;
					}

					Form.SetPixel(x, y, c);
				}

			}
		}
	}

	class RingSprite : SimpleSprite
	{
		public enum RColor {
			Gold = 0,
			White = 1,
		};
		public RingSprite(RColor c)
		{
			int[][] pixels;
			Color[] pal;

			Width = 8;
			Height = 8;

			switch (c)
			{
				default:
				case RColor.Gold:
					pal = new Color[]{
						Color.Transparent, //0 Transparent
						Color.Gold, // "1"
						Color.Yellow, // 2
					};
					break;
				case RColor.White:
					pal = new Color[]{
						Color.Transparent, //0 Transparent
						Color.White, // "1"
						Color.Gray, // 2
					};
					break;
			}

			//pixels = new int[][]
			//{
			//	new int[] { 0, 0, 1, 1, 2, 0, 0, 0, },
			//	new int[] { 0, 1, 2, 0, 1, 2, 0, 0, },
			//	new int[] { 1, 2, 0, 0, 0, 1, 2, 0, },
			//	new int[] { 1, 2, 0, 0, 0, 0, 1, 2, },
			//	new int[] { 1, 2, 0, 0, 0, 0, 1, 2, },
			//	new int[] { 0, 1, 2, 0, 0, 0, 1, 2, },
			//	new int[] { 0, 0, 1, 2, 0, 1, 2, 0, },
			//	new int[] { 0, 0, 0, 1, 1, 2, 0, 0, },
			//};

			//// better?
			//pixels = new int[][]
			//{
			//    new int[] { 0, 0, 1, 2, 0, 0, 0, 0, },
			//    new int[] { 0, 1, 2, 1, 2, 0, 0, 0, },
			//    new int[] { 1, 2, 0, 0, 1, 2, 0, 0, },
			//    new int[] { 1, 2, 0, 0, 0, 1, 2, 0, },
			//    new int[] { 1, 2, 0, 0, 0, 1, 2, 0, },
			//    new int[] { 0, 1, 2, 0, 0, 1, 2, 0, },
			//    new int[] { 0, 0, 1, 2, 1, 2, 0, 0, },
			//    new int[] { 0, 0, 0, 1, 2, 0, 0, 0, },
			//};

			pixels = new int[][]
			{
				new int[] { 0, 0, 1, 2, 0, 0, 0, 0, },
				new int[] { 0, 1, 2, 1, 2, 0, 0, 0, },
				new int[] { 1, 2, 0, 0, 1, 2, 0, 0, },
				new int[] { 1, 2, 0, 0, 1, 2, 0, 0, },
				new int[] { 1, 2, 0, 0, 1, 2, 0, 0, },
				new int[] { 1, 2, 0, 0, 1, 2, 0, 0, },
				new int[] { 0, 1, 2, 1, 2, 0, 0, 0, },
				new int[] { 0, 0, 1, 2, 0, 0, 0, 0, },
			};

			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Color color = Color.Transparent;
					int i = pixels[y][x];
					if (i > 0)
					{
						color = pal[i];
					}

					Form.SetPixel(x, y, color);
				}

			}
		}
	}


	// Prototyping, not the final implementation. Fast enough, but not written very well.
	// The idea is that I would like to be able to flatten multiple sprites into a single bitmap for speed,
	// and eventually generalize that in this class. For now, it's hard coded for the one composite "scene"
	// I want to create that is reminicient of the "It's dangerous to go alone, take this thing"
	// TODO: Generalize into a CompoundSprite/GridSprite or something named similar that can repeat sprites in a grid
	// perhaps use "Using(Graphics g onthe(Form))
	/// <summary>
	/// Flattens multiple static sprites into a single image which renders a lot faster than multiple smaller sprites
	/// </summary>
	class SpriteVideoGameBackground : SimpleSprite
	{
		public SimpleSprite Sprite { get; set; }

		public SpriteVideoGameBackground()
		{
			// TODO: parameterize this. For now I know the Pi device dimensions.
			Width = 1024;
			Height = 600;
			Magnification = 1;
		}

		private void DrawSprite(SimpleSprite source, Bitmap dest, int x, int y, int magnification)
		{
			for (int yy = y; yy < dest.Height && yy < y + source.Height; ++yy)
			{
				for (int xx = x; xx < dest.Width && xx < x + source.Width; ++xx)
				{
					// Starting at
					int sourceX = xx - x;
					int sourceY = yy - y;
					Color c = source.GetPixel(sourceX, sourceY);
					for (int y3 = 0; y3 < magnification; ++y3)
					{
						for (int x3 = 0; x3 < magnification; ++x3)
						{
							int destX = x + sourceX * magnification + x3;
							int destY = y + sourceY * magnification + y3;

							if (destX < dest.Width && destY < dest.Height)
								dest.SetPixel(destX, destY, c);
						}
					}
				}
			}
		}
		public void Initialize()
		{
			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			int xMax = Width / (Sprite.Width * Sprite.Magnification);
			int yMax = Height / (Sprite.Height * Sprite.Magnification);
			for (int yy = 0; yy < yMax; ++yy)
			{
				for (int xx = 0; xx < xMax; ++xx)
				{
					if ((xx < 1 || xx > xMax - 2) && (yy > 2)
						|| yy == 2
						|| yy == 3
						|| (yy > yMax - 2 && ((xx < xMax / 2 - 3) || (xx > xMax / 2 + 2))))
					{
						int x = xx * Sprite.Width * Sprite.Magnification;
						int y = yy * Sprite.Height * Sprite.Magnification;
						DrawSprite(Sprite, Form, x, y, Sprite.Magnification);
					}
				}
			}


			Heart heart = new Heart();
			Heart heartEmpty = new Heart(empty: true);
			int heartMag = 3;
			int heartY = 30;
			int filledHearts = 3;
			for (int ii = 0; ii < 6; ++ii)
			{
				--filledHearts;
				DrawSprite(filledHearts > 0 ? heart : heartEmpty, Form, Width - (Width / 5) + ii * heart.Width * heartMag, heartY, magnification: heartMag);
			}

			RectangleSprite grayRect = new RectangleSprite(100, 50, Color.Gray, fill: true);
			DrawSprite(grayRect, Form, 100, 10, 1);
			RectangleSprite greenRect = new RectangleSprite(8, 8, Color.LightGreen, fill: true);
			DrawSprite(greenRect, Form, 150, 45, 1);

			RectangleSprite blueRect = new RectangleSprite(20, 14, Color.Blue, fill: false);
			DrawSprite(blueRect, Form, 600, 10, 3);
			DrawSprite(blueRect, Form, 500, 10, 3);

			XSprite theX = new XSprite(width: 8, height: 8, Color.White);
			DrawSprite(theX, Form, 520, 5, 2);

			YSprite theY = new YSprite(width: 8, height: 8, Color.White);
			DrawSprite(theY, Form, 620, 5, 2);

			RingSprite ring = new RingSprite(RingSprite.RColor.Gold);
			DrawSprite(ring, Form, 420, 10, 2);

			RingSprite ringWhite = new RingSprite(RingSprite.RColor.White);
			DrawSprite(theX, Form, 437, 18, 1);
			DrawSprite(ringWhite, Form, 450, 10, 2);
		}

		//private void NaiveRender(Graphics graph)
		//{
		//	int xMax = Width / (Sprite.Width * Sprite.Magnification);
		//	int yMax = Height / (Sprite.Height * Sprite.Magnification);
		//	for (int yy = 0; yy < yMax; ++yy)
		//	{
		//		for (int xx = 0; xx < xMax; ++xx)
		//		{
		//			if (xx < 1 || xx > xMax - 2 || yy < 2 || (yy > yMax - 2 && ((xx < xMax / 2 - 3) || (xx > xMax / 2 + 2))))
		//			{
		//				Sprite.Location = new Point(xx * Sprite.Width * Sprite.Magnification, yy * Sprite.Height * Sprite.Magnification);
		//				Sprite.RenderOneFrameToScreen(graph);
		//			}
		//		}
		//	}
		//}

		int m_iNeedToRender = 0;
		public override void RenderOneFrameToScreen(Graphics graph)
		{
			// Don't need to spend time redrawing this every time, so we can just render once,
			// or every once in a while to make sure no artifacts.
			if (m_iNeedToRender++ % 60 == 5)
			{
				base.RenderOneFrameToScreen(graph);
				//NaiveRender(graph);
			}
		}
	}

	class VectorSauronTowerSprite : SimpleSprite
	{
		public override void RenderOneFrameToScreen(Graphics graph)
		{
			int offset;
			//offset = 149
			//Point[] points = {
			//	new Point(offset, 400),
			//	new Point(offset, 0),
			//	new Point(offset + 10, 0),
			//	new Point(offset + 10, 300),
			//	new Point(1000 - offset, 300),
			//	new Point(1000 - offset, 0),
			//	new Point(1010 - offset, 0),
			//	new Point(1010 - offset, 400),
			//};
			//Point[] pointsTowerCurve = {
			//	new Point(offset + 10, 0),
			//	new Point(350, 500),
			//	new Point(500, 599),
			//	new Point(650, 500),
			//	new Point(1000 - offset, 0),
			//};
			//graph.DrawLine(Pens.White, offset, 599, offset, 0);
			////graph.DrawLine(Pens.White, offset, 0, offset + 10, 0);
			////graph.DrawLine(Pens.White, offset + 10, 0, offset + 10, 300);
			////graph.DrawLine(Pens.White, offset + 10, 300, 1000- offset, 300);
			////graph.DrawLine(Pens.White, 1000- offset, 300, 1000- offset, 0);
			////graph.DrawLine(Pens.White, 1000 - offset, 0, 1010 - offset, 0);
			//graph.DrawLine(Pens.White, 1000 - offset, 0, 1000 - offset, 599);

			//graph.DrawCurve(Pens.White, pointsTowerCurve);
			float screenWidth = 1024;
			float screenHeight = 600;
			float towerWidth = 600.0f; // Sauron sprite width
			float towerHeight = screenHeight - 3;
			float elipseWidth = towerWidth + 100;
			float elipseHeight = towerHeight * 2;
			offset = (int)((screenWidth - elipseWidth) / 2.0f) + Location.X;
			Pen color = Pens.PaleGoldenrod;
			graph.DrawArc(color, x: offset, y: Location.Y + towerHeight - elipseHeight, width: elipseWidth, height: elipseHeight, startAngle: 0.0f, sweepAngle: 180.0f);

			// Tower edges
			int leftTower = offset - 10;
			int rightTower = (int)screenWidth - leftTower;
			graph.DrawLine(color, leftTower, screenHeight - 1, leftTower, 0);
			graph.DrawLine(color, rightTower, 0, rightTower, screenHeight - 1);

			// draw the middle bit
			int xCenter = (int)(screenWidth / 2 + 0.5f);
			int littleHeight = (int)(towerHeight / 6 + 0.5f);
			int littleWidth = littleHeight / 3;
			int littleBottom = (int)(towerHeight);

			graph.DrawLine(color, xCenter - littleWidth / 2, littleBottom, xCenter, towerHeight - littleHeight);
			graph.DrawLine(color, xCenter, towerHeight - littleHeight, xCenter + littleWidth / 2, littleBottom);
		}
	}

}
