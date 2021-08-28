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

	class LinkHoldingItem : SimpleSprite
    {
		public LinkHoldingItem()
		{
			int[][] link;
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
			link = new int[][]
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
					int i = link[y][x];
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
			Color lightBrown, darkBrown, darkGreen, lightGreen, orangeShoe, pinkSkin, redShoe, reddishBrownHat, yellow, redMouth;

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

			//"1 black, 2 pink, 3 ltGreen, 4 dkGreen, 5 brown, 6 hatBrown, 7yellow, 8 orangeShoe, 9 redShoe, 10 eyes, 11 teeth"
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
			//"1 black, 2 pink, 3 ltGreen, 4 dkGreen, 5 brown, 6 hatBrown, 7yellow, 8 orangeShoe, 9 redShoe, 10 eyes, 11 teeth"

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

			//"1 black, 2 pink, 3 ltGreen, 4 dkGreen, 5 brown, 6 hatBrown, 7yellow, 8 orangeShoe, 9 redShoe, 10 eyes, 11 teeth"
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
			//"1 black, 2 pink, 3 ltGreen, 4 dkGreen, 5 brown, 6 hatBrown, 7yellow, 8 orangeShoe, 9 redShoe, 10 eyes, 11 teeth"

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


	class CauldronBase : SimpleSprite
	{
		public CauldronBase()
		{
			int[][] pixels;
			Color[] pal;

			Width = 8;
			Height = 16;

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

	class TorchHandle : SimpleSprite
	{
		public TorchHandle()
		{
			int[][] pixels;
			Color[] pal;

			Width = 8;
			Height = 5;

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


}
