using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace FireDemo
{
	abstract class SimpleSprite
	{
		protected Bitmap Form { get; set; }
		public int Height { get; protected set; }
		public int Width { get; protected set; }
		public int Magnification { get; set; }
		public Point Location { get; set; }
		public InterpolationMode InterpolationMode { get; set; }
		public CompositingMode CompositingMode { get; set; }

		public Color GetPixel(int x, int y)
        {
			return Form.GetPixel(x, y);
		}

		public SimpleSprite()
		{
			this.InterpolationMode = InterpolationMode.Bicubic; // Default to BEST quality
			this.CompositingMode = CompositingMode.SourceOver; // Default to best QUALITY option (no blinking
															   // due to blanking out the ENTIRE graph)
															   // Caller can use SourceCopy for some extra speed
															   // if they know it's safe (i.e. only a single bitmap
															   // will be rendered per frame)
			Location = new Point(0, 0);
			Magnification = 1;
		}

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

		public virtual void RenderOneFrameToScreen(Graphics graph)
		{
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

			lightGreen= Color.LightGreen;
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
		public RingSprite(RColor c = RColor.Gold) // TODO: Parameterize the color???
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
	class SpriteCompositor : SimpleSprite
    {
		public SimpleSprite Sprite { get; set; }

		public SpriteCompositor()
        {
			// TODO: Define how to parameterize this? For now I know I want the Pi device.
			Width = 1024;
			Height = 600;
			Magnification = 1;
		}


		void Draw(SimpleSprite source, Bitmap dest, int x, int y, int magnification)
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
			// TODO: Define how to parameterize this? For now I know I want the Pi device.
			Width = 1024;
			Height = 600;
			Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

			int xMax = Width / (Sprite.Width * Sprite.Magnification);
			int yMax = Height / (Sprite.Height * Sprite.Magnification);
			for (int yy = 0; yy < yMax; ++yy)
			{
				for (int xx = 0; xx < xMax; ++xx)
				{
					if ((xx < 1  || xx > xMax - 2 ) && (yy > 2) 
						|| yy == 2 
						|| yy == 3 
						|| (yy > yMax - 2 && ((xx < xMax / 2 - 3) || (xx > xMax / 2 + 2))))
					{
						int x = xx * Sprite.Width * Sprite.Magnification;
						int y = yy * Sprite.Height * Sprite.Magnification;
						Draw(Sprite, Form, x, y, Sprite.Magnification);
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
				Draw(filledHearts > 0 ? heart : heartEmpty, Form, Width - (Width / 5) + ii * heart.Width * heartMag, heartY, magnification: heartMag);
			}

			RectangleSprite grayRect = new RectangleSprite(100, 50, Color.Gray, fill: true);
			Draw(grayRect, Form, 100, 10, 1);
			RectangleSprite greenRect = new RectangleSprite(8, 8, Color.LightGreen, fill: true);
			Draw(greenRect, Form, 150, 45, 1);

			RectangleSprite blueRect = new RectangleSprite(20, 14, Color.Blue, fill: false);
			Draw(blueRect, Form, 600, 10, 3);
			Draw(blueRect, Form, 500, 10, 3);

			XSprite theX = new XSprite(width: 8, height: 8, Color.White);
			Draw(theX, Form, 520, 5, 2);

			YSprite theY = new YSprite(width: 8, height: 8, Color.White);
			Draw(theY, Form, 620, 5, 2);

			RingSprite ring = new RingSprite(0);
			Draw(ring, Form, 420, 10, 2);

			RingSprite ringWhite = new RingSprite(RingSprite.RColor.White);
			Draw(theX, Form, 437, 18, 1);
			Draw(ringWhite, Form, 450, 10, 2);
		}

		//public void SetGrid(List<Point> grid)
		//{
		//    int bmWidth = 16;
		//    int bmHeight = 16;
		//    Bitmap foo = new Bitmap(bmWidth, bmHeight, PixelFormat.Format32bppArgb);
		//    // TODO: Define how to parameterize this? For now I know I want the Pi device.
		//    Width = 1024;
		//    Height = 600;
		//    Form = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

		//    int xMax = Width / Sprite.Width;
		//    for (int xx = 0; xx < xMax; ++xx)
		//    {
		//        //Sprite.RenderOneFrameToScreen();
		//        Form.
		//    }

		//}

		private void NaiveRender(Graphics graph)
        {
			int xMax = Width / (Sprite.Width * Sprite.Magnification);
			int yMax = Height / (Sprite.Height * Sprite.Magnification);
			for (int yy = 0; yy < yMax; ++yy)
			{
				for (int xx = 0; xx < xMax; ++xx)
				{
					if (xx < 1 || xx > xMax - 2 || yy < 2 || (yy > yMax - 2 && ((xx < xMax / 2 - 3) || (xx > xMax / 2 + 2))))
					{
						Sprite.Location = new Point(xx * Sprite.Width * Sprite.Magnification, yy * Sprite.Height * Sprite.Magnification);
						Sprite.RenderOneFrameToScreen(graph);
					}
				}
			}
		}

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

}
