//#define PARALLEL_8BIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace FireDemo
{
	//struct ColorRange
	//{
	//	public ColorRange(Color color, int range)
	//	{
	//		this.color = color;
	//		this.range = range;
	//	}
	//	public int range;
	//	public readonly Color color;
	//}

	public partial class Form1 : Form
	{
		Random rng = new Random();
		Graphics graph;
		int[] flameIntensityMatrixFront;
		int[] flameIntensityMatrixBackBuffer;
		int[] coolingMap;
		int[] rotatingCoolingMap; // the smoothed version
		int[] originalCoolingMap; // not yet smoothed
		int m_iFrame = 0; // keep track of the frame so we can scroll the cooling map
		bool m_fUpdateFireDimensionsAfterNextFrame = false;
		bool m_fLighting = false;
		bool m_fBorg = false;
		bool m_fPallete = true;
		Color[] thePalette = null;
		Color singleColorFlame;
		// all 4 colors, so we can preserve the value across Stop/Start the Fire!
		Color color1;
		Color color2;
		Color color3;
		Color color4;
		Color userSelectedColor1;
		Color userSelectedColor2;
		Color userSelectedColor3;
		Color userSelectedColor4;

		Bitmap bmToDraw;
		// the dimensions are now configurable.  It's okay if I have to reset the flame, but I want to be able to change this at runtime.
		int fireWidth = 5;
		int fireHeight = 5;
		int drawnWidth = 200;
		int drawnHeight = 200;
		int drawingX = 0;
		int drawingY = 0;
#if PARALLEL_8BIT
        Thread[] bgTasks;
#endif
		public Form1()
		{
			InitializeComponent();
			graph = this.CreateGraphics();

			// Using more realistic colors now (leaving this so the default user selected colors are better)
			InitializeBlueYellowFlame();
			InitializePalette(color1, color2, color3, color4);
			color1 = userSelectedColor1 = thePalette[0];
			color2 = userSelectedColor2 = thePalette[85];
			color3 = userSelectedColor3 = thePalette[170];
			color4 = userSelectedColor4 = thePalette[255];
			customColorButton1.BackColor = color1;
			customColorButton1.ForeColor = Color.White;
			customColorButton2.BackColor = color2;
			customColorButton3.BackColor = color3;
			customColorButton4.BackColor = color4;

			// Using even more realistic colors now
			m_fOverrideEnabledForSpecialFlame = true;
			InitializeRealisticFlame();

			// Make sure the value in UI matches what we use
			heightTrackBar.Value = (int)heightNumericUpDown.Value;
			widthTrackBar.Value = (int)widthNumericUpDown.Value;

			UpdateUI_Visibility();

			// TODO: JRDV: The next thing to tackle is parallelizing, or adding the pixels above the "current" pixel. (What?  I don't remember what I mean...)
#if PARALLEL_8BIT
			if (System.Environment.ProcessorCount > 1)
			{
				bgTasks = new Thread[System.Environment.ProcessorCount - 1];
				// Now loop and create all the Threads?

				for (int i = 0; i < bgTasks.Length; ++i)
				{
					bgTasks[i] = new Thread(new ParameterizedThreadStart(ParameterizedParallelProcessingFunction));
					bgTasks[i].Start(i);
				}
			}
#endif
		}

#if PARALLEL_8BIT
		bool m_fWorkToDo = false;
		void ParameterizedParallelProcessingFunction(object o)
		{
			int iThread = (int)o;
			//int iBgThreads = System.Environment.ProcessorCount - 1;

			Wait:
			Thread.CurrentThread.Suspend();

			int iRowsPerThread = (fireHeight - 2) / (bgTasks.Length + 1); // add 1, so the main thread does some work.
			int yMin = 1 + iRowsPerThread * iThread;
			int yMax = yMin + iRowsPerThread - 1;
			DoThe8bitWork(yMin, yMax);
			if (m_fWorkToDo)
			{
			}

			goto Wait;
		}

#endif
		/// <summary>
		/// Ensure the controls are visible/enabled/disabled as appropriate.
		/// </summary>
		private void UpdateUI_Visibility()
		{
			coolingMapShiftCheckBox.Enabled = coolingMapRadioButton.Checked;
			coolingMapDensityNumericUpDown.Enabled = coolingMapRadioButton.Checked;
			coolingMapMinNumericUpDown.Enabled = coolingMapRadioButton.Checked;
			coolingMapMaxNumericUpDown.Enabled = coolingMapRadioButton.Checked;
			coolingMapSmoothingNumericUpDown.Enabled = coolingMapRadioButton.Checked;
			rotateCoolingMapCheckBox.Enabled = coolingMapRadioButton.Checked;

			coalSeedMinNumericUpDown.Enabled = customSeedRadioButton.Checked;
			coalSeedMaxNumericUpDown.Enabled = customSeedRadioButton.Checked;
			coalSeedRangeCheckBox.Enabled = customSeedRadioButton.Checked;
			coalSeedPercentNumericUpDown.Enabled = customSeedRadioButton.Checked;

			bool fVisible = !FUseSingleColorFlame();
			//colorPickerButton.Visible = fVisible; // Always want to be able to pick a color
			customColorLabel.Visible = fVisible;
			customColorButton1.Visible = fVisible;
			customColorButton2.Visible = fVisible;
			customColorButton3.Visible = fVisible;
			customColorButton4.Visible = fVisible;
			copy4PointButton.Visible = fVisible;
			squeakPaletteButton.Visible = false; // now that we have a flat palette mode, no need for this button
			if (fVisible)
			{
				customColorButton1.BackColor = userSelectedColor1;
				customColorButton2.BackColor = userSelectedColor2;
				customColorButton3.BackColor = userSelectedColor3;
				customColorButton4.BackColor = userSelectedColor4;

				customColorButton1.ForeColor = IsLightColor(userSelectedColor1) ? Color.Black : Color.White;
				customColorButton2.ForeColor = IsLightColor(userSelectedColor2) ? Color.Black : Color.White;
				customColorButton3.ForeColor = IsLightColor(userSelectedColor3) ? Color.Black : Color.White;
				customColorButton4.ForeColor = IsLightColor(userSelectedColor4) ? Color.Black : Color.White;
			}
		}

		bool IsLightColor(Color c)
		{
			return c.GetBrightness() >= 0.5f;
		}

		private void UpdateFireDimensions()
		{
			SetFireDimensions(widthTrackBar.Value, heightTrackBar.Value);
			if (m_fPallete)
				Initialize8Bit();
			else
				Initialize32Bit();
		}


		private void SetFireDimensions(int width, int height)
		{
			this.fireWidth = width;
			this.fireHeight = height;

			int advancedGroupBoxRightEdge = advancedGroupBox.Location.X + advancedGroupBox.Size.Width + advancedGroupBox.Margin.Right;
			int advancedGroupBoxTopEdge = advancedGroupBox.Location.Y + advancedGroupBox.Margin.Top;
			int maxWidth = this.Size.Width - advancedGroupBoxRightEdge - 15; // There is some other factor I'm not accounting for
			int maxHeight = this.Size.Height - advancedGroupBoxTopEdge - 15; // There is some other factor I'm not accounting for

			// Clear the drawing region to eliminate artifacts from the previous flames
			Bitmap bmEmpty = new Bitmap(maxWidth, maxHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			graph.DrawImage(bmEmpty, advancedGroupBoxRightEdge, advancedGroupBoxTopEdge, maxWidth, maxHeight);

			if (this.fitScreenCheckBox.Checked)
			{
				// how many withds fit? how many heights?  Which one is smaller?  Now calculate based on that.
				int maxPossibleWidths = maxWidth / fireWidth;
				int maxPossigleHeights = maxHeight / fireHeight;
				int maxAllowedMultiple = Math.Min(maxPossibleWidths, maxPossigleHeights);
				if (maxAllowedMultiple < 1)
					maxAllowedMultiple = 1; // TODO: May want to do something smarter

				this.drawnWidth = fireWidth * maxAllowedMultiple;
				this.drawnHeight = fireHeight * maxAllowedMultiple;
			}
			else
			{
				// TODO: JRDV: Flame curve calculation is off when not using max value for the primary color component.
				// I didn't like the new curve as much, so restored the old curve.

				// TODO: JRDV: Can we have an inline color picker??? I think I'd have to build it.
				// TODO: JRDV: Figure out why we can't draw to the bottom half of the window after maximizing
				// TODO: JRDV: Implement explicit multiples
				this.drawnWidth = width;
				this.drawnHeight = height;
			}

			this.drawingX = advancedGroupBoxRightEdge;
			this.drawingY = advancedGroupBoxTopEdge;
		}


		#region Cooling Map

		private void FillCoolingMap(int[] theMap, int start, int end)
		{
			for (int i = start; i < end; ++i)
			{
				if (coolingMapDensityNumericUpDown.Value > rng.Next(100))
					theMap[i] = rng.Next((int)coolingMapMinNumericUpDown.Value, (int)coolingMapMaxNumericUpDown.Value + 1);
				else
					theMap[i] = 0;
			}
		}
		private void UpdateRotatingCoolingMap()
		{
			int fireSize = fireHeight * fireWidth;
			int rotatingCoolingMapSize = fireSize * 2;
			if (originalCoolingMap == null || originalCoolingMap.Length != rotatingCoolingMapSize
				|| rotatingCoolingMap == null || rotatingCoolingMap.Length != rotatingCoolingMapSize
				|| coolingMap == null || coolingMap.Length != fireSize)
			{
				originalCoolingMap = new int[rotatingCoolingMapSize];
				rotatingCoolingMap = new int[rotatingCoolingMapSize];
				coolingMap = new int[fireSize];
				//rng = new Random(1); // Revert this!!! Just generating the test data for unit tests
				FillCoolingMap(originalCoolingMap, 0, rotatingCoolingMapSize);
			}
			else
			{
				for (int ii = 0; ii < fireSize; ++ii)
				{
					originalCoolingMap[ii] = originalCoolingMap[ii + fireSize];
				}
				FillCoolingMap(originalCoolingMap, fireSize, rotatingCoolingMapSize);
			}

			if ((int)coolingMapSmoothingNumericUpDown.Value == 0)
			{
				for (int ii = 0; ii < coolingMap.Length; ++ii)
				{
					coolingMap[ii] = originalCoolingMap[ii];
				}
				return;
			}
			else
			{
				SmoothCoolingMap(ref originalCoolingMap, ref rotatingCoolingMap);
			}
			if ((int)coolingMapSmoothingNumericUpDown.Value > 1)
			{
				SmoothCoolingMapDoubleBuffer(ref rotatingCoolingMap, (int)coolingMapSmoothingNumericUpDown.Value - 1);
			}

			// TODO: JRDV: Optimize.  For now, just copy over the bits
			for (int ii = 0; ii < coolingMap.Length; ++ii)
			{
				coolingMap[ii] = rotatingCoolingMap[ii];
			}

			//// Revert this!!! Just generating the test data for unit tests
			//string result = "int[] expectedCoolingMap = { ";
			//for (int ii = 0; ii < coolingMap.Length; ++ii)
			//{
			//    result += string.Format(" {0},", coolingMap[ii]);
			//}

			//result = result.Substring(0, result.Length - 1);
			//result += "};";

			//string final = result;
		}

		// TODO: JRDV: Shift cooling map each frame?  Make the cooling map accessible via the UI.
		private void InitializeCoolingMap()
		{
			if (rotateCoolingMapCheckBox.Checked && coolingMap != null)
				return;

			int size = fireHeight * fireWidth;
			if (coolingMap == null || coolingMap.Length != size)
				coolingMap = new int[size];

			FillCoolingMap(coolingMap, 0, size);

			//			for (i = 0; i < coolingMapSmoothingNumericUpDown.Value; ++i)
			SmoothCoolingMapDoubleBuffer((int)coolingMapSmoothingNumericUpDown.Value);
		}

		private void SmoothCoolingMapDoubleBuffer(ref int[] source, int cIterations)
		{
			int[] destinationMap = new int[source.Length];
			for (int i = 0; i < cIterations; ++i)
			{
				int[] swapMap;
				SmoothCoolingMap(ref source, ref destinationMap);
				swapMap = source;
				source = destinationMap;
				destinationMap = swapMap;
				swapMap = null;
			}
			destinationMap = null;
		}
		private void SmoothCoolingMapDoubleBuffer(int cIterations)
		{
			SmoothCoolingMapDoubleBuffer(ref coolingMap, cIterations);
		}
		// Smoothed the top and bottom of the buffer, so we don't get a "seam" when scrolling.
		private void SmoothCoolingMap(ref int[] sourceMap, ref int[] destinationMap)
		{
			int x, y;

			// Top Row
			for (x = 1; x < fireWidth - 1; ++x)
			{
				int iFinal = 0;
				//int p1, p2, p3, p4, p5, p6, p7, p8, p9;
				int iOriginal, iNextRow, iPreviousRow;

				iOriginal = x + (0 * fireWidth);
				iNextRow = iOriginal + fireWidth;
				iPreviousRow = (fireHeight * fireWidth) + iOriginal - fireWidth;

				iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
				destinationMap[iOriginal] = iFinal;
			}

			// All the Middle Rows
			for (y = 1; y < fireHeight - 1; ++y)
			{
				for (x = 1; x < fireWidth - 1; ++x) // Don't include the left and right edges
				{
					// Get the surrounding colors, subtract some amount, average them, then write the result
					int iFinal = 0;

					int iOriginal, iNextRow, iPreviousRow;

					iOriginal = x + (y * fireWidth);
					iNextRow = iOriginal + fireWidth;
					iPreviousRow = iOriginal - fireWidth;
					iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
					destinationMap[iOriginal] = iFinal;
				}
			}

			// Bottom Row
			for (x = 1; x < fireWidth - 1; ++x)
			{
				int iFinal = 0;
				//int p1, p2, p3, p4, p5, p6, p7, p8, p9;
				int iOriginal, iNextRow, iPreviousRow;

				iOriginal = x + (fireHeight * fireWidth) - fireWidth;
				iNextRow = iOriginal % fireWidth;
				iPreviousRow = iOriginal - fireWidth;

				iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
				destinationMap[iOriginal] = iFinal;
			}

			// The left side
			for (y = 1; y < fireHeight - 1; ++y)
			{
				// Get the surrounding colors, subtract some amount, average them, then write the result
				int iFinal = 0;

				int iOriginal, iNextRow, iPreviousRow;

				iOriginal = 0 + (y * fireWidth);
				iNextRow = iOriginal + fireWidth;
				iPreviousRow = iOriginal - fireWidth;
				iFinal = (sourceMap[iOriginal + fireWidth - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
				destinationMap[iOriginal] = iFinal;
			}

			// The right side
			for (y = 1; y < fireHeight - 1; ++y)
			{
				// Get the surrounding colors, subtract some amount, average them, then write the result
				int iFinal = 0;

				int iOriginal, iNextRow, iPreviousRow;

				iOriginal = fireWidth - 1 + (y * fireWidth);
				iNextRow = iOriginal + fireWidth;
				iPreviousRow = iOriginal - fireWidth;
				iFinal = (sourceMap[iOriginal - 1] + sourceMap[iOriginal] + sourceMap[iOriginal + 1 - fireWidth] + sourceMap[iPreviousRow] + sourceMap[iNextRow]) / 5;
				destinationMap[iOriginal] = iFinal;
			}

			// The 4 corners!
			int p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12;

			//          1 2
			//        3 4 5 6
			//        7 8 9 10
			//         11 12
			p1 = sourceMap[(fireWidth * fireHeight) - 1 - fireWidth];
			p2 = sourceMap[(fireWidth * fireHeight) - fireWidth - fireWidth];
			p3 = sourceMap[(fireWidth * fireHeight) - 2];
			p4 = sourceMap[(fireWidth * fireHeight) - 1];
			p5 = sourceMap[(fireWidth * fireHeight) - fireWidth];
			p6 = sourceMap[(fireWidth * fireHeight) - fireWidth + 1];
			p7 = sourceMap[fireWidth - 2];
			p8 = sourceMap[fireWidth - 1];
			p9 = sourceMap[0];
			p10 = sourceMap[1];
			p11 = sourceMap[(2 * fireWidth) - 1];
			p12 = sourceMap[fireWidth];

			destinationMap[0] = (p9 + p5 + p8 + p10 + p12) / 5; // top left
			destinationMap[fireWidth - 1] = (p8 + p4 + p7 + p9 + p11) / 5; // top right
			destinationMap[fireWidth * fireHeight - fireWidth] = (p5 + p2 + p4 + p6 + p9) / 5; // bottom left
			destinationMap[fireWidth * fireHeight - 1] = (p4 + p1 + p3 + p5 + p8) / 5; // bottom right
		}
		private void Initialize8Bit()
		{
			int i;
			m_fPallete = true;
			if (thePalette == null)
			{
				InitializeRealisticFlame();
			}

			flameIntensityMatrixFront = new int[fireHeight * fireWidth];
			flameIntensityMatrixBackBuffer = new int[fireHeight * fireWidth];
			InitializeCoolingMap();
			i = fireHeight * fireWidth;
			while (i-- > 0)
			{
				flameIntensityMatrixFront[i] = 0;
				flameIntensityMatrixBackBuffer[i] = 0;
			}

			// TODO: JRDV: May want to actually use indexed values (why?  Is it faster?  Anyway, 32bit format is fast enough today, so this is a potential optimization in the future)
			//bmToDraw = new Bitmap(fireWidth, fireHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
			if (bmToDraw == null || bmToDraw.Height != fireHeight || bmToDraw.Width != fireWidth)
			{
				bmToDraw = new Bitmap(fireWidth, fireHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			}
		}

		#endregion

		#region Palette Helper/Setters

		// range INCLUDES start, and also INCLUDES end
		// Used by both the old 4 point flame palette, and new more realistic flame palette curve code
		private void SetPaletteRangeInclusive(int start, int end, Color c1, Color c2)
		{
			int i;
			for (i = start; i <= end; ++i)
			{
				//double fPercent = ((double)i - start) / (end - start + 1); // <=== this is not what we want.
				// include both endpoints, then we correctly set the values for the full range.
				double fPercent = ((double)i - start) / (end - start);
				int r, g, b;

				r = (int)(c1.R + (fPercent * (c2.R - c1.R)));
				g = (int)(c1.G + (fPercent * (c2.G - c1.G)));
				b = (int)(c1.B + (fPercent * (c2.B - c1.B)));

				thePalette[i] = Color.FromArgb(r, g, b);
			}
		}

		// For the Old 4 point style
		private void InitializePalette(Color c1, Color c2, Color c3, Color c4)
		{
#if true // TODO: This will overwrite the user's selection, but makes it easier to tweak
			customColorButton1.BackColor = c1;
			customColorButton2.BackColor = c2;
			customColorButton3.BackColor = c3;
			customColorButton4.BackColor = c4;
			userSelectedColor1 = c1;
			userSelectedColor2 = c2;
			userSelectedColor3 = c3;
			userSelectedColor4 = c4;
#endif
			//m_fSingleColorFlame = false;

			if (thePalette == null || thePalette.Length != 256)
				thePalette = new Color[256]; // 85 per each range

			// Including the final value.
			SetPaletteRangeInclusive(0, 85, c1, c2); // 85 in this range
			SetPaletteRangeInclusive(85, 170, c2, c3); // 86 in this range
			SetPaletteRangeInclusive(170, 255, c3, c4); // 85 in this range
		}
		// For the realistic, hand tuned palettes, AND the generalized flame curve function calculation
		private void SetPaleteFromColorRange(ColorRange[] colorRange)
		{
			if (thePalette == null || thePalette.Length != 256)
				thePalette = new Color[256];

			int rangeSum = 0;
			foreach (ColorRange cr in colorRange)
			{
				if (cr.range != -1)
					rangeSum += cr.range;
			}

			int rangeStart = 0;
			int rangeEnd = 0;
			for (int i = 0; i < colorRange.Length - 1; ++i)
			{
				rangeStart = rangeEnd;
				if (colorRange[i].range == -1)
					colorRange[i].range = thePalette.Length - rangeSum - 1;
				rangeEnd += colorRange[i].range;

				// Including the final value.
				SetPaletteRangeInclusive(rangeStart, rangeEnd, colorRange[i].color, colorRange[i + 1].color);
			}
		}

		private bool FUseSingleColorFlame()
		{
			return (flatRenderMethodRadioButton.Checked || realisticRenderMethodRadioButton.Checked || intensityRenderMethodRadioButton.Checked);
		}
		private void SetSingleColorFlame(Color color)
		{
			m_fOverrideEnabledForSpecialFlame = false;
			singleColorFlame = color;

			SetPaletteUsingSingleColor(color);
		}

		private void SetFlatPalette(Color color)
		{
			int R = color.R;
			int G = color.G;
			int B = color.B;

			float fRed = (R / 255.0f);
			float fGreen = (G / 255.0f);
			float fBlue = (B / 255.0f);

			for (int i = 0; i < thePalette.Length; ++i)
			{
				thePalette[i] = Color.FromArgb(
									(int)(i * fRed),
									(int)(i * fGreen),
									(int)(i * fBlue));
			}
		}
		private void SetPaletteUsingSingleColor(Color color)
		{
			if (thePalette == null)
				thePalette = new Color[256];

			if (realisticRenderMethodRadioButton.Checked || intensityRenderMethodRadioButton.Checked)
			{
				if (color == Color.DarkOrange)
					color = Color.FromArgb(255, 1, 1); // JRDV: faking Orange since the default didn't look good?  Try it again soon.

				float intensity = 1;

				if (intensityRenderMethodRadioButton.Checked)
					intensity = ((float)intensityUpDown.Value / 100);

				if (color == Color.White)
					InitializeWhiteFlameCurve();
				else if (color == Color.Black)
					InitializeBlackFlameCurve();
				else

					InitializeRealisticFlameCurve(color, intensity);
				return;
			}
			else if (linearRenderMethodRadioButton.Checked)
			{
				//if (color == Color.DarkOrange)
				//    color = Color.FromArgb(255, 128, 0);
				//SetPaletteRangeInclusive(0, 255, Color.Black, colorDialog1.Color);
				// TODO: JRDV: I think this is essentially dead code.... it used to do something, but now we always overwrite with the 4 Point Linear Palette calculation
				SetPaletteRangeInclusive(0, 170, Color.Black, color);
				SetPaletteRangeInclusive(170, 255, color, Color.White);
			}
			else if (flatRenderMethodRadioButton.Checked)
			{
				SetFlatPalette(color);
			}
		}

		private void RefreshTheFlamePalette()
		{
			if (FUseSingleColorFlame())
			{
				if (m_fOverrideEnabledForSpecialFlame)
				{
					if (flatRenderMethodRadioButton.Checked)
					{
						SetSingleColorFlame(Color.Orange);
						//SetSingleColorFlame(Color.LightCoral);
						//SetSingleColorFlame(Color.FromArgb(255, 128, 0));
						m_fOverrideEnabledForSpecialFlame = true;
						return;
					}
					// This is the Realistic.
					// It's the only one that isn't defined by a generic curve
					// I don't like having to override this
					InitializeRealisticFlame();
					return;
				}

				SetPaletteUsingSingleColor(singleColorFlame);
			}
			else // 4 Point linear palette
			{
				InitializePalette(color1, color2, color3, color4);
			}
		}

		void InitializeWhiteFlameCurve()
        {
			//	"Simplify the palette generation porocess by using a function based on a single color"

			//	| c0 c1 c2 c3 c4 colorRangeGenerated sourceColor palWhite c cAvg |
#if false
			Color c0;
			Color c1;
			Color c2;
			Color c3;
			Color c4;
			Color sourceColor;

			Color sourceColor = Color.White;
			Color c0 = GetColorCurve(sourceColor, 0.5f, 32, 0.3f);
			Color c1 = GetColorCurve(sourceColor, 1.0f, 32, 0.3f);
			Color c2 = GetColorCurve(sourceColor, 1.0f, 127, 0.3f);
			Color c3 = GetColorCurve(sourceColor, 1.0f, 238, 0.3f);
			Color c4 = GetColorCurve(sourceColor, 1.0f, 64, 0.3f);

	colorRangeGenerated := {
				ColorRange color: (Color white) range: 10.
		ColorRange color: (Color black) range: 30.
		ColorRange color: (c0)range: 10.
		ColorRange color: (c1)range: 25.
		ColorRange color: (c2)range: 10.
		ColorRange color: (c3)range: 10.
		ColorRange color: (Color black) range: -1.
		ColorRange color: (Color white) range: 15.
		ColorRange color: ((Color black)) range: 0.
	}.

			sourceColor = Color.White;
			c0 = GetColorCurve(sourceColor, 0.5f, 32, 0.3f);
			c1 = GetColorCurve(sourceColor, 0.7f, 32, 0.3f);
			c2 = GetColorCurve(sourceColor, 1.0f, 127, 0.3f);
			c3 = GetColorCurve(sourceColor, 1.0f, 238, 0.3f);
			c4 = GetColorCurve(sourceColor, 1.0f, 64, 0.3f);

   colorRangeGenerated := {
				ColorRange color: (Color black) range: 10.
		ColorRange color: (Color black) range: 30.
		ColorRange color: (c0)range: 10.
		ColorRange color: (c1)range: 25.
		ColorRange color: (c2)range: 10.
		ColorRange color: (c3)range: 10.
		ColorRange color: (Color white) range: -1.
		ColorRange color: (Color black) range: 15.
		ColorRange color: ((Color black)) range: 0.
	}.

	colorRangeGenerated:= {
		ColorRange color: (self gray: 0) range: 10.
		ColorRange color: (self gray: 0) range: 30.
		ColorRange color: (self gray: 1.0) range: 10.
		ColorRange color: (self gray: 1.0) range: 25.
		ColorRange color: (self gray: 1.0) range: 10.
		ColorRange color: (self gray: 1.0) range: 10.
		ColorRange color: (self gray: 1.0) range: -1.
		ColorRange color: (self gray: 0) range: 15.
		ColorRange color: (self gray: 0) range: 0.
	}.

			ColorRange[] colorRangeGenerated = {
				new ColorRange(Color.FromArgb(0, 0, 0), 10),
                new ColorRange(Color.FromArgb(0, 0, 0), 30),
				new ColorRange(Color.FromArgb(255, 255, 255), 10),
				new ColorRange(Color.FromArgb(255, 255, 255), 25),
				new ColorRange(Color.FromArgb(255, 255, 255), 10),
				new ColorRange(Color.FromArgb(255, 255, 255), 10),
				new ColorRange(Color.FromArgb(255, 255, 255), -1),
				new ColorRange(Color.FromArgb(0, 0, 0), 15),
				new ColorRange(Color.FromArgb(0, 0, 0), 0),
			};

			SetPaleteFromColorRange(colorRangeGenerated);
#endif

//			InitializeRealisticPalette();
//			InitializeRealisticFlame
			blueYellowFlameButton_Click(null, null);
			
			for (int i = 0; i < thePalette.Length; ++i)
            {
				Color c0 = thePalette[i];
				int cAvg = (c0.R + c0.G + c0.B) / 3;
				thePalette[i] = Color.FromArgb(cAvg, cAvg, cAvg);
			}
		}

		void InitializeBlackFlameCurve()
        {
			InitializeWhiteFlameCurve();

			// Invert the "White" flame
			for (int i = 0; i < thePalette.Length; ++i)
            {
				Color c = thePalette[i];

				thePalette[i] = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
            }
#if false
	"Get rid of the white rectangle!"
	"16 is definitely too small,
	32 has too much white
	40 is nice
	44 is too light

	48 is nice but maybe too white ?

	56 is too large
	64 is too large"
#endif

			// now tweak it!
			int cDampen = 16;
			int colTarget = thePalette[cDampen].R;
			for (int i = 0; i < cDampen; ++i)
            {
				int iIntensity = colTarget * (i / cDampen);
				thePalette[i] = Color.FromArgb(iIntensity, iIntensity, iIntensity);
            }
        }

		#endregion

		#region Initialize old style 4 point flames

		private void InitializeRedFlame() // good
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(255, 0, 0);    // Red
			Color c3 = Color.FromArgb(255, 170, 0);  // Orange
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeOrangeFlame() // good
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(255, 170, 0);  // Orange
			Color c3 = Color.FromArgb(255, 200, 0);  // Yellow
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeYellowFlame() // good-ish (could be better)
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			//Color c2 = Color.FromArgb(170, 170, 0);  // Darker Yellow
			//Color c2 = Color.FromArgb(170, 120, 0);  // Darker Yellow
			//Color c3 = Color.FromArgb(255, 255, 0);  // Bright Yellow
			Color c2 = Color.FromArgb(200, 200, 0);  // Darker Yellow
			Color c3 = Color.FromArgb(255, 255, 0);  // Bright Yellow
			c2 = Color.FromArgb(255, 255, 0);  // Bright Yellow
			c3 = Color.FromArgb(255, 255, 128);  // Darker Yellow
			Color c4 = Color.FromArgb(255, 255, 255);// White
			//Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeBlueFlame() // good
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			//Color c2 = Color.FromArgb(0, 0, 128 + 64); // DarkBlue
			//Color c3 = Color.FromArgb(0, 0, 255);    // Blue
			Color c2 = Color.FromArgb(0, 0, 255);    // Blue
			Color c3 = Color.FromArgb(0, 255, 255);    // Light Blue
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeBlueYellowFlame() // Realistic-ish
		{
			//Color c1 = Color.FromArgb(0, 0, 0);       // Black
			//Color c2 = Color.FromArgb(255, 170, 0);   // Orange
			//Color c3 = Color.FromArgb(255, 255, 64);  // Bright Yellow
			//Color c4 = Color.FromArgb(196, 196, 255); // Light Blue

			Color c1 = Color.FromArgb(0, 0, 0);       // Black
			Color c2 = Color.FromArgb(255, 185, 0);   // Orange
			Color c3 = Color.FromArgb(255, 255, 127); // Bright Yellow
			Color c4 = Color.FromArgb(212, 212, 255); // Light Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeCyanFlame() // good
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(0, 255, 255);    // Cyan
			//Color c3 = Color.FromArgb(0, 128+64, 128+64);   // Cyan 3/4
			Color c3 = Color.FromArgb(128, 255, 255);   // Cyan + some red
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeBlueGreenFlame() // good
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(0, 0, 255);    // Blue
			//Color c3 = Color.FromArgb(100, 255, 0);// Green
			Color c3 = Color.FromArgb(0, 128, 64);   // Green - I like this better
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeGreenFlame() // good-ish (could be better)
		{
			//(int)(0.6 * i), // about i / 4
			//(int)(0.1 * i)); // about i / 25)
			//Color c1 = Color.FromArgb(0, 10, 0);
			//Color c2 = Color.FromArgb(0, 63, 0);
			//Color c3 = Color.FromArgb(0, 255, 0);
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(0, 255, 0);    // Green 1/2
			Color c3 = Color.FromArgb(128, 255, 128);    // Green
			//Color c4 = Color.FromArgb(128, 255, 128);// White
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeMagentaFlame() // needs more defined edge work (could be better)
		{
			//(int)(0.6 * i), // about i / 4
			//(int)(0.1 * i)); // about i / 25)
			//Color c1 = Color.FromArgb(0, 10, 0);
			//Color c2 = Color.FromArgb(0, 63, 0);
			//Color c3 = Color.FromArgb(0, 255, 0);
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(128 + 64, 0, 128 + 64);    // Magenta 3/4
			Color c3 = Color.FromArgb(255, 0, 255);    // Magenta
			c2 = Color.FromArgb(255, 0, 255);    // Magenta
			c3 = Color.FromArgb(255, 128, 255);    // Magenta ++
			Color c4 = Color.FromArgb(255, 128 + 64, 255);// White Magenta
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeVioletFlame() // needs more defined edge work (could be better)
		{
			//(int)(0.6 * i), // about i / 4
			//(int)(0.1 * i)); // about i / 25)
			//Color c1 = Color.FromArgb(0, 10, 0);
			//Color c2 = Color.FromArgb(0, 63, 0);
			//Color c3 = Color.FromArgb(0, 255, 0);
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			//Color c2 = Color.FromArgb(64, 0, 128);    // Violet 1/2
			//Color c3 = Color.FromArgb(128, 0, 255);    // Violet
			Color c2 = Color.FromArgb(64 + 32, 0, 128 + 64);    // Violet 3/4
			Color c3 = Color.FromArgb(128, 0, 255);    // Violet
			c2 = Color.FromArgb(128, 0, 255);    // Violet
			c3 = Color.FromArgb(128, 128, 255);    // Violet 3/4
			//Color c2 = Color.FromArgb(128, 0, 255);    // Violet
			//Color c3 = Color.FromArgb(64, 0, 128);    // Violet 1/2
			//Color c4 = Color.FromArgb(255, 255, 255);// White
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeBlackFlame() // needs more defined edge work (could be better)
		{
			InitializeWhiteFlame();
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(64 + 32, 0, 128 + 64);    // Violet 3/4
			Color c3 = Color.FromArgb(128, 0, 255);    // Violet
			c2 = Color.FromArgb(128, 0, 255);    // Violet
			c3 = Color.FromArgb(128, 128, 255);    // Violet 3/4
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue

			c4 = color1;
			c3 = color2;
			c2 = color3;
			c1 = color4;

			color1 = Color.Black;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void InitializeWhiteFlame() // needs more defined edge work (could be better)
		{
			Color c1 = Color.FromArgb(0, 0, 0);      // Black
			Color c2 = Color.FromArgb(64 + 32, 0, 128 + 64);    // Violet 3/4
			Color c3 = Color.FromArgb(128, 0, 255);    // Violet
			c2 = Color.FromArgb(255, 255, 255);    // White
			c3 = Color.FromArgb(192, 192, 192);    // White 3/4
			Color c4 = Color.FromArgb(212, 212, 255);// White Blue
			color1 = c1;
			color2 = c2;
			color3 = c3;
			color4 = c4;

			//InitializePalette(c1, c2, c3, c4);
		}

		private void FlattenPalettetoGrayscale()
        {
			for (int i = 0; i < thePalette.Length; ++i)
            {
				Color c = thePalette[i];
				int cAvg = (int)((((float)(c.R + c.G + c.B)) / 3) + 0.5f);
				thePalette[i] = Color.FromArgb(cAvg, cAvg, cAvg);
            }
        }

		#endregion

		#region Initialize Realistic Flames
		private void InitializeRealisticFlame()
		{
			float fullIntensity = 1.0f;
			float mutedIntensity = 1.0f;
			if (intensityRenderMethodRadioButton.Checked)
			{
				fullIntensity = (float)(intensityUpDown.Value / 100);
				mutedIntensity = (float)((intensityUpDown.Value / 4 + 75) / 100);
			}
			ColorRange[] colorRangeOld = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 0, 0), 14),     // Red
                new ColorRange(Color.FromArgb(255, 185, 0), 55),   // Orange
                new ColorRange(Color.FromArgb(255, 255, 196), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 255), 5), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 185, 0), 5),   // Orange
                new ColorRange(Color.FromArgb(212, 212, 255), 0), // Light Blue
            };

			ColorRange[] colorRange2 = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 0, 0), 10),     // Red
                new ColorRange(Color.FromArgb(255, 185, 0), 25),   // Orange
                new ColorRange(Color.FromArgb(255, 255, 196), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 255), 25), // White
                new ColorRange(Color.FromArgb(255, 185, 0), 5),   // Orange
                new ColorRange(Color.FromArgb(212, 212, 255), 0), // Light Blue
            };

			ColorRange[] colorRangeBlueWhiteOrangeRed = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 0, 0), 10),     // Red
                new ColorRange(Color.FromArgb(255, 185, 0), 25),   // Orange
                new ColorRange(Color.FromArgb(255, 255, 0), 10),  // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 196), 10), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 196), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 255), 15), // White
                //new ColorRange(Color.FromArgb(255, 185, 0), 5),   // Orange
                new ColorRange(Color.FromArgb(212, 212, 255), 0), // Light Blue
            };

			ColorRange[] colorRangeBlueWhiteOrangeRed_WithIntensity = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb((int)(127), 0, 0), 10),     // Red
                new ColorRange(Color.FromArgb((int)(255), (int)(185), (int)(0)), 25),   // Orange
                new ColorRange(Color.FromArgb((int)(fullIntensity * 255), (int)(fullIntensity * 255), (int)(fullIntensity * 0)), 10),  // Bright Yellow
                new ColorRange(Color.FromArgb((int)(fullIntensity * 255), (int)(fullIntensity * 255), (int)(fullIntensity * 196)), 10), // Bright Yellow
                new ColorRange(Color.FromArgb((int)(fullIntensity * 255), (int)(fullIntensity * 255), (int)(fullIntensity * 196)), -1), // Bright Yellow
                new ColorRange(Color.FromArgb((int)(fullIntensity * 255), (int)(fullIntensity * 255), (int)(fullIntensity * 255)), 15), // White
                //new ColorRange(Color.FromArgb(255, 185, 0), 5),   // Orange
                new ColorRange(Color.FromArgb(212, 212, 255), 0), // Light Blue
            };

			ColorRange[] colorRangeBlueWhiteOrangeRed_WithIntensity_Modified = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb((int)(127), 0, 0), 10),     // Red
                new ColorRange(Color.FromArgb((int)(255), (int)(185), (int)(0)), 25),   // Orange
                new ColorRange(Color.FromArgb((int)(255), (int)(mutedIntensity * 255), (int)(fullIntensity * 0)), 10),  // Bright Yellow
                new ColorRange(Color.FromArgb((int)(255), (int)(mutedIntensity * 255), (int)(fullIntensity * 196)), 10), // Bright Yellow
                new ColorRange(Color.FromArgb((int)(255), (int)(mutedIntensity * 255), (int)(fullIntensity * 196)), -1), // Bright Yellow
                new ColorRange(Color.FromArgb((int)(255), (int)(mutedIntensity * 255), (int)(fullIntensity * 255)), 15), // White
                //new ColorRange(Color.FromArgb(255, 185, 0), 5),   // Orange
                new ColorRange(Color.FromArgb(212, 212, 255), 0), // Light Blue
            };

			m_fOverrideEnabledForSpecialFlame = true;

			SetPaleteFromColorRange(colorRangeBlueWhiteOrangeRed_WithIntensity_Modified);

			// Revert this! Just generating the test data for unit tests
			//string result = "Color[] theRealPalette = { ";
			//foreach (Color c in thePalette)
			//{
			//	result += string.Format("Color.FromArgb({0}, {1}, {2}), ", c.R, c.G, c.B);
			//}
			//result = result.Substring(0, result.Length - 1);
			//result += "};";

			//string final = result;
		}

		private void InitializeRealisticFlameOld()
		{
			Color[] colors = {
                Color.FromArgb(0, 0, 0),       // Black
                Color.FromArgb(255, 0, 0),     // Red
                Color.FromArgb(255, 185, 0),   // Orange
                Color.FromArgb(255, 255, 196), // Bright Yellow
                Color.FromArgb(255, 255, 255), // Bright Yellow
                Color.FromArgb(255, 185, 0),   // Orange
                Color.FromArgb(212, 212, 255), // Light Blue
            };

			thePalette = new Color[256]; // 85 per each range

			int[] ranges = { 10, 5, 10, -1, 20, 10 };
			int rangeSum = 0;
			foreach (int r in ranges)
			{
				if (r != -1)
					rangeSum += r;
			}

			int rangeStart = 0;
			int rangeEnd = 0;
			for (int i = 0; i < colors.Length - 1; ++i)
			{
				rangeStart = rangeEnd;
				if (ranges[i] == -1)
					ranges[i] = thePalette.Length - rangeSum - 1;
				rangeEnd += ranges[i];

				// Including the final value.
				SetPaletteRangeInclusive(rangeStart, rangeEnd, colors[i], colors[i + 1]);
			}
		}

		private void InitializeRealisticRedFlame()
		{
			ColorRange[] colorRangeRed_Decent = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 71*32/238, 32), 10),     // Dark Red
                new ColorRange(Color.FromArgb(255, 71*32/238, 32), 25),     // Red
                new ColorRange(Color.FromArgb(255, 71*127/238, 127), 10), // Bright Red
                new ColorRange(Color.FromArgb(255, 71, 238), 10), // Bright Red
                new ColorRange(Color.FromArgb(255, 71, 238), -1), // Bright Red
                new ColorRange(Color.FromArgb(255, 71*64/238, 64), 15), // ???
                new ColorRange(Color.FromArgb(255, 71*32/238, 32), 0),      // Red
            };

			ColorRange[] colorRangeRed = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 71*32/238, 32), 10),     // Dark Red
                new ColorRange(Color.FromArgb(255, 71*32/238, 32), 25),     // Red
                new ColorRange(Color.FromArgb(255, 71*127/238, 127), 10), // Bright Red
                new ColorRange(Color.FromArgb(255, 71, 238), 10), // Bright Red
                new ColorRange(Color.FromArgb(255, 71, 238), -1), // Bright Red
                new ColorRange(Color.FromArgb(255, 71*64/238, 64), 15), // ???
                new ColorRange(Color.FromArgb(255, 71*32/238, 32), 0),      // Red
            };

			SetPaleteFromColorRange(colorRangeRed);
		}

		private void InitializeRealisticOrangeFlame()
		{
			ColorRange[] colorRangeOrange = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 32, 71*32/238), 10),     // Dark Orange
                new ColorRange(Color.FromArgb(255, 32, 71*32/238), 25),     // Orange
                new ColorRange(Color.FromArgb(255, 127, 71*127/238), 10), // Bright Orange
                new ColorRange(Color.FromArgb(255, 238, 71), 10), // Bright Orange
                new ColorRange(Color.FromArgb(255, 238, 71), -1), // Bright Orange
                new ColorRange(Color.FromArgb(255, 64, 71*64/238), 15), // ???
                new ColorRange(Color.FromArgb(255, 32, 71*32/238), 0),      // Orange
            };

			SetPaleteFromColorRange(colorRangeOrange);
		}

		private void InitializeRealisticYellowFlame()
		{
			ColorRange[] colorRangeYellow = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 127, 32), 10),     // Dark Yellow
                new ColorRange(Color.FromArgb(255, 255, 32), 25),     // Yellow
                new ColorRange(Color.FromArgb(255, 255, 127), 10), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 238), 10), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 238), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(255, 255, 64), 15), // ???
                new ColorRange(Color.FromArgb(255, 255, 32), 0),      // Yellow
            };

			SetPaleteFromColorRange(colorRangeYellow);
		}

		private void InitializeRealisticGreenFlame()
		{
			ColorRange[] colorRangeGreenGood = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),     // Green
                new ColorRange(Color.FromArgb(127, 255, 127), 10), // Bright Green
                new ColorRange(Color.FromArgb(196, 255, 196), 10), // Bright Green
                new ColorRange(Color.FromArgb(196, 255, 196), -1), // Bright Green
                new ColorRange(Color.FromArgb(255, 255, 255), 15), // White
                new ColorRange(Color.FromArgb(0, 255, 0), 0),      // Green
            };

			ColorRange[] colorRangeGreen_NotBad = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),     // Green
                new ColorRange(Color.FromArgb(64, 255, 64), 10), // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 127), 10), // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 127), -1), // Bright Green
                new ColorRange(Color.FromArgb(32, 255, 64), 15), // ???
                new ColorRange(Color.FromArgb(0, 255, 32), 0),      // Green
            };

			ColorRange[] colorRangeGreen_TooTurquoise = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(71*32/238, 127, 32), 10),     // Dark Blue
                new ColorRange(Color.FromArgb(71*32/238, 255, 32), 25),     // Blue
                new ColorRange(Color.FromArgb(71*127/238, 255, 127), 10), // Bright Blue
                new ColorRange(Color.FromArgb(71, 255, 238), 10), // Bright Blue
                new ColorRange(Color.FromArgb(71, 255, 238), -1), // Bright Blue
                new ColorRange(Color.FromArgb(71*64/238, 255, 64), 15), // ???
                new ColorRange(Color.FromArgb(71*32/238, 255, 32), 0),      // Blue
            };

			ColorRange[] colorRangeGreen_TooMuchYellow_TryToToneDown = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(32, 127, 71*32/238), 10),     // Dark Green
                new ColorRange(Color.FromArgb(32, 255, 71*32/238), 25),     // Dark Green
                new ColorRange(Color.FromArgb(127, 255, 71*127/238), 10), // Bright Green
                new ColorRange(Color.FromArgb(238, 255, 71), 10), // Bright Yelow
                new ColorRange(Color.FromArgb(238, 255, 71), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(64, 255, 71*64/238), 15), // ???
                new ColorRange(Color.FromArgb(32, 255, 71*32/238), 0),      // Dark Green
            };

			ColorRange[] colorRangeGreen = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(32, 127, 71*32/238), 10),     // Dark Green
                new ColorRange(Color.FromArgb(32, 255, 71*32/238), 25),     // Dark Green
                new ColorRange(Color.FromArgb(127, 255, 71*127/238), 10), // Bright Green
                new ColorRange(Color.FromArgb(71, 255, 71), 10), // Bright Yelow
                new ColorRange(Color.FromArgb(16*127/71, 255, 71), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(16, 255, 127), 15), // ???
                new ColorRange(Color.FromArgb(0, 200, 127), 0),      // Dark Green
            };

			SetPaleteFromColorRange(colorRangeGreen);
		}

		private void InitializeRealisticDarkGreenFlame()
		{
			ColorRange[] colorRangeGreenGood = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),     // Green
                new ColorRange(Color.FromArgb(127, 255, 127), 10), // Bright Green
                new ColorRange(Color.FromArgb(196, 255, 196), 10), // Bright Green
                new ColorRange(Color.FromArgb(196, 255, 196), -1), // Bright Green
                new ColorRange(Color.FromArgb(255, 255, 255), 15), // White
                new ColorRange(Color.FromArgb(0, 255, 0), 0),      // Green
            };

			ColorRange[] colorRangeGreen_NotBad = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),     // Green
                new ColorRange(Color.FromArgb(64, 255, 64), 10), // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 127), 10), // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 127), -1), // Bright Green
                new ColorRange(Color.FromArgb(32, 255, 64), 15), // ???
                new ColorRange(Color.FromArgb(0, 255, 32), 0),      // Green
            };

			ColorRange[] colorRangeGreen = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(32, 127, 71*32/238), 10),     // Dark Green
                new ColorRange(Color.FromArgb(32, 255, 71*32/238), 25),     // Dark Green
                new ColorRange(Color.FromArgb(127, 255, 71*127/238), 10), // Bright Green
                new ColorRange(Color.FromArgb(71, 255, 71), 10), // Bright Yelow
                new ColorRange(Color.FromArgb(16*127/71, 255, 71), -1), // Bright Yellow
                new ColorRange(Color.FromArgb(16, 255, 127), 15), // ???
                new ColorRange(Color.FromArgb(0, 200, 127), 0),      // Dark Green
            };

			ColorRange[] colorRangeGreen_Dark = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),     // Green
                new ColorRange(Color.FromArgb(64, 255, 64), 10), // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 127), 10), // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 127), -1), // Bright Green
                new ColorRange(Color.FromArgb(32, 255, 64), 15), // ???
                new ColorRange(Color.FromArgb(0, 255, 32), 0),      // Green
            };

			SetPaleteFromColorRange(colorRangeGreen_Dark);
		}
		private void InitializeRealisticBlueFlame()
		{
			ColorRange[] colorRangeBlueGood = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 0, 127), 10),     // Dark Blue
                new ColorRange(Color.FromArgb(0, 0, 255), 25),     // Blue
                new ColorRange(Color.FromArgb(127, 127, 255), 10), // Bright Blue
                new ColorRange(Color.FromArgb(196, 196, 255), 10), // Bright Blue
                new ColorRange(Color.FromArgb(196, 196, 255), -1), // Bright Blue
                new ColorRange(Color.FromArgb(255, 255, 255), 15), // White
                new ColorRange(Color.FromArgb(0, 0, 255), 0),      // Blue
            };

			ColorRange[] colorRangeBlue_MuchBetter = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 0, 127), 10),     // Dark Blue
                new ColorRange(Color.FromArgb(0, 0, 255), 25),     // Blue
                new ColorRange(Color.FromArgb(71*127/238, 127, 255), 10), // Bright Blue
                new ColorRange(Color.FromArgb(71, 238, 255), 10), // Bright Blue
                new ColorRange(Color.FromArgb(71, 238, 255), -1), // Bright Blue
                new ColorRange(Color.FromArgb(32, 64, 255), 15), // ???
                new ColorRange(Color.FromArgb(0, 32, 255), 0),      // Blue
            };

			ColorRange[] colorRangeBlue = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(71*32/238, 32, 127), 10),     // Dark Blue
                new ColorRange(Color.FromArgb(71*32/238, 32, 255), 25),     // Blue
                new ColorRange(Color.FromArgb(71*127/238, 127, 255), 10), // Bright Blue
                new ColorRange(Color.FromArgb(71, 238, 255), 10), // Bright Blue
                new ColorRange(Color.FromArgb(71, 238, 255), -1), // Bright Blue
                new ColorRange(Color.FromArgb(71*64/238, 64, 255), 15), // ???
                new ColorRange(Color.FromArgb(71*32/238, 32, 255), 0),      // Blue
            };

			SetPaleteFromColorRange(colorRangeBlue);
		}

		private void InitializeRealisticBlueGreenFlame()
		{
			ColorRange[] colorRange2 = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),   // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 0), 10),  // Bright Yellow
                //new ColorRange(Color.FromArgb(255, 255, 196), 10), // Bright Yellow
                new ColorRange(Color.FromArgb(0, 127+64+32, 0), -1), // Dark Green
                new ColorRange(Color.FromArgb(0, 127+32, 0), 10), // Dark Green
                // TODO: JRDV: Below here, need more blue
                new ColorRange(Color.FromArgb(0, 255, 0), 10), // Bright Green
                new ColorRange(Color.FromArgb(0, 63, 255), 10), // Dark Blue
                new ColorRange(Color.FromArgb(0x06, 0x1f, 0xa4), 0), // Dark Blue
            };
			ColorRange[] colorRangeBlueGreen = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(0, 127, 0), 10),     // Dark Green
                new ColorRange(Color.FromArgb(0, 255, 0), 25),   // Bright Green
                new ColorRange(Color.FromArgb(127, 255, 0), 10),  // Bright Yellow
                //new ColorRange(Color.FromArgb(255, 255, 196), 10), // Bright Yellow
                new ColorRange(Color.FromArgb(0, 127+64+32, 0), -1), // Dark Green
                new ColorRange(Color.FromArgb(0, 127+32, 0), 10), // Dark Green
                // TODO: JRDV: Below here, need more blue.  This looks nice.
                new ColorRange(Color.FromArgb(0, 127, 64), 10), // Bright Green
                new ColorRange(Color.FromArgb(0, 64, 127), 10), // Dark Blue
                new ColorRange(Color.FromArgb(100, 100, 255), 0), // Dark Blue
            };

			SetPaleteFromColorRange(colorRangeBlueGreen);
		}

		private void InitializeRealisticMagentaFlame()
		{
			ColorRange[] colorRangeLightViolet = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 32, 127), 10),     // Dark Magenta
                new ColorRange(Color.FromArgb(255, 32, 255), 25),     // Magenta
                new ColorRange(Color.FromArgb(255, 127, 255), 10), // Bright Magenta
                new ColorRange(Color.FromArgb(255, 238, 255), 10), // Bright Magenta
                new ColorRange(Color.FromArgb(255, 238, 255), -1), // Bright Magenta
                new ColorRange(Color.FromArgb(255, 64, 255), 15), // Magenta
                new ColorRange(Color.FromArgb(255, 32, 255), 0),      // Magenta
            };

			ColorRange[] colorRangeViolet = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 16, 127), 10),     // Dark Magenta
                new ColorRange(Color.FromArgb(255, 16, 255), 25),     // Magenta
                new ColorRange(Color.FromArgb(255, 64, 255), 10), // Bright Magenta
                new ColorRange(Color.FromArgb(255, 127, 255), 10), // Bright Magenta
                new ColorRange(Color.FromArgb(255, 127, 255), -1), // Bright Magenta
                new ColorRange(Color.FromArgb(255, 32, 255), 15), // Magenta
                new ColorRange(Color.FromArgb(255, 16, 255), 0),      // Magenta
            };

			SetPaleteFromColorRange(colorRangeViolet);
		}
		#endregion

		#region Generalized Flame Curve Calculation

		void GetColorForThreeHelper_Old(ref int primary1, ref int primary2, ref int secondary, float factor1, int factor2, float factor3)
		{
			primary1 = (int)(primary1 * factor1);
			primary2 = (int)(primary2 * factor1);
			//secondary = secondary * ((float)factor2) ; // TODO: JRDV: this should be a multiplication? Yes, try it next.  The color curve is currently off when the primary color is not 255
			secondary += factor2;
			if (secondary > 255)
				secondary = 255;
		}

		void GetColorForThree_Old(ref int R, ref int G, ref int B, float factor1, int factor2, float factor3)
		{
			bool fRedLeast = false;
			bool fGreenLeast = false;

			if (R <= G)
			{
				if (R <= B)
					fRedLeast = true;
			}
			else if (G <= B)
				fGreenLeast = true;

			if (fRedLeast)
				GetColorForThreeHelper_Old(ref G, ref B, ref R, factor1, factor2, factor3);
			else if (fGreenLeast)
				GetColorForThreeHelper_Old(ref R, ref B, ref G, factor1, factor2, factor3);
			else //if (fBlueLeast)
				GetColorForThreeHelper_Old(ref R, ref G, ref B, factor1, factor2, factor3);
		}

		void GetColorForSecondary_Old(ref int primary1, ref int primary2, ref int secondary, float factor1, int factor2, float factor3)
		{
			primary1 = (int)(primary1 * factor1);
			primary2 = (int)(primary2 * factor1);
			//secondary = secondary * ((float)factor2) ; // TODO: JRDV: this should be a multiplication? Yes, try it next. The color curve is currently off when the primary color is not 255
			secondary += factor2;
			if (secondary > 255)
				secondary = 255;
		}

		void GetColorForPrimary_Old(ref int primary, ref int secondary, ref int tertiary, float factor1, int factor2, float factor3)
		{
			primary = (int)(primary * factor1);
			secondary += factor2;
			//secondary = secondary + (int)(primary * factor2 / 255.0f);
			if (secondary > 255)
				secondary = 255;
			tertiary = (int)(secondary * factor3);
		}

		Color GetColorCurve_Old(Color target, float factor1, int factor2, float factor3)
		{
			bool fRed = (target.R > 0);
			bool fGreen = (target.G > 0);
			bool fBlue = (target.B > 0);
			int R = target.R;
			int G = target.G;
			int B = target.B;

			if (fRed && fBlue && fGreen)
			{
				if (G == 1 && B == 1)
				{
					// Faking orange, since the colors don't seem right when using the color curve function...
					// TODO: JRDV: investigate and see if there is a bug in the curve function.
					G = 0;
					B = 0;
					GetColorForPrimary_Old(ref R, ref G, ref B, factor1, factor2, factor3);
				}
				else
				{
					GetColorForThree_Old(ref R, ref G, ref B, factor1, factor2, factor3);
				}
			}
			else if (fRed && fBlue) // Violet or Magenta
			{
				GetColorForSecondary_Old(ref B, ref R, ref G, factor1, factor2, factor3);
			}
			else if (fBlue && fGreen) // Cyan
			{
				GetColorForSecondary_Old(ref B, ref G, ref R, factor1, factor2, factor3);
			}
			else if (fRed && fGreen) // Yellow
			{
				GetColorForSecondary_Old(ref R, ref G, ref B, factor1, factor2, factor3);
			}
			else if (fRed)
			{
				GetColorForPrimary_Old(ref R, ref B, ref G, factor1, factor2, factor3);
			}
			else if (fBlue)
			{
				GetColorForPrimary_Old(ref B, ref G, ref R, factor1, factor2, factor3);
			}
			else if (fGreen)
			{
				// Thought it was too turquoise, but it actually does look accurate to some flames I see on the internet :P
				GetColorForPrimary_Old(ref G, ref B, ref R, factor1, factor2, factor3);
			}

			return Color.FromArgb(R, G, B);
		}







		void GetColorForThreeHelper(ref int primary1, ref int primary2, ref int secondary, float factor1, float factor2, float factor3)
		{
			primary1 = (int)(primary1 * factor1);
			primary2 = (int)(primary2 * factor1);
			secondary = (int)(secondary * factor2); // TODO: JRDV: this should be a multiplication? Yes, try it next.  The color curve is currently off when the primary color is not 255
			if (secondary > 255)
				secondary = 255;
		}

		void GetColorForThree(ref int R, ref int G, ref int B, float factor1, float factor2, float factor3)
		{
			bool fRedLeast = false;
			bool fGreenLeast = false;

			if (R <= G)
			{
				if (R <= B)
					fRedLeast = true;
			}
			else if (G <= B)
				fGreenLeast = true;

			if (fRedLeast)
				GetColorForThreeHelper(ref G, ref B, ref R, factor1, factor2, factor3);
			else if (fGreenLeast)
				GetColorForThreeHelper(ref R, ref B, ref G, factor1, factor2, factor3);
			else //if (fBlueLeast)
				GetColorForThreeHelper(ref R, ref G, ref B, factor1, factor2, factor3);
		}

		void GetColorForSecondary(ref int primary1, ref int primary2, ref int secondary, float factor1, float factor2, float factor3)
		{
			primary1 = (int)(primary1 * factor1);
			primary2 = (int)(primary2 * factor1);
			secondary = (int)((primary1 + primary2) / 2.0f * factor2); // This starts at zero, so multiplying secondary doesn't DO anything!
			if (secondary > 255)
				secondary = 255;
		}

		void GetColorForPrimary(ref int primary, ref int secondary, ref int tertiary, float factor1, float factor2, float factor3)
		{
			primary = (int)(primary * factor1);
			secondary = (int)(primary * factor2);
			if (secondary > 255)
				secondary = 255;
			tertiary = (int)(secondary * factor3);
		}

		Color GetColorCurve(Color target, float factor1, float factor2, float factor3)
		{
			bool fRed = (target.R > 0);
			bool fGreen = (target.G > 0);
			bool fBlue = (target.B > 0);
			int R = target.R;
			int G = target.G;
			int B = target.B;

			if (fRed && fBlue && fGreen)
			{
				if (G == 1 && B == 1)
				{
					// Faking orange, since the colors don't seem right when using the color curve function...
					// TODO: JRDV: investigate and see if there is a bug in the curve function.
					G = 128;
					B = 0;
					GetColorForPrimary(ref R, ref G, ref B, factor1, factor2, factor3);
				}
				else
				{
					GetColorForThree(ref R, ref G, ref B, factor1, factor2, factor3);
				}
			}
			else if (fRed && fBlue) // Violet or Magenta
			{
				GetColorForSecondary(ref B, ref R, ref G, factor1, factor2, factor3);
			}
			else if (fBlue && fGreen) // Cyan
			{
				GetColorForSecondary(ref B, ref G, ref R, factor1, factor2, factor3);
			}
			else if (fRed && fGreen) // Yellow or Orange
			{
				GetColorForSecondary(ref R, ref G, ref B, factor1, factor2, factor3);
			}
			else if (fRed)
			{
				GetColorForPrimary(ref R, ref B, ref G, factor1, factor2, factor3);
			}
			else if (fBlue)
			{
				GetColorForPrimary(ref B, ref G, ref R, factor1, factor2, factor3);
			}
			else if (fGreen)
			{
				// Thought it was too turquoise, but it actually does look accurate to some flames I see on the internet :P
				GetColorForPrimary(ref G, ref B, ref R, factor1, factor2, factor3);
			}

			return Color.FromArgb(R, G, B);
		}

		private void InitializeRealisticFlameCurve(Color target, float fIntensity = 1.0f)
		{
#if false
            // secondary intensity
            int n1 = 32;
            int n2 = 127;
            int n3 = 238;
            int n4 = 64;

            // tertiary intensity
            float fI = 0.3f;
#endif

#if true
			Color c0 = GetColorCurve_Old(target, 0.5f, (int)(fIntensity * 32), 0.3f);
			Color c1 = GetColorCurve_Old(target, 1.0f, (int)(fIntensity * 32), 0.3f);
			Color c2 = GetColorCurve_Old(target, 1.0f, (int)(fIntensity * 127), 0.3f);
			Color c3 = GetColorCurve_Old(target, 1.0f, (int)(fIntensity * 238), 0.3f);
			Color c4 = GetColorCurve_Old(target, 1.0f, (int)(fIntensity * 64), 0.3f);
#else
            Color c0 = GetColorCurve(target, 0.5f, (fIntensity * (32 / 255.0f)), 0.3f);
            Color c1 = GetColorCurve(target, 1.0f, (fIntensity * (32 / 255.0f)), 0.3f);
            Color c2 = GetColorCurve(target, 1.0f, (fIntensity * (127 / 255.0f)), 0.3f);
            Color c3 = GetColorCurve(target, 1.0f, (fIntensity * (238 / 255.0f)), 0.3f);
            Color c4 = GetColorCurve(target, 1.0f, (fIntensity * (64 / 255.0f)), 0.3f);
#endif
			ColorRange[] colorRangeBlue = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(71*32/238, 32, 127), 10),     // Dark Blue  (HALF, 9, 32)     #0
                new ColorRange(Color.FromArgb(71*32/238, 32, 255), 25),     // Blue       (TARGET, 9, 32)   #1
                new ColorRange(Color.FromArgb(71*127/238, 127, 255), 10), // Bright Blue  (target, 37, 127) #2
                new ColorRange(Color.FromArgb(71, 238, 255), 10), // Bright Blue          (target, 71, 238) #3
                new ColorRange(Color.FromArgb(71, 238, 255), -1), // Bright Blue          (target, 71, 238) #3
                new ColorRange(Color.FromArgb(71*64/238, 64, 255), 15), // ???            (target, 19, 64)  #4
                new ColorRange(Color.FromArgb(71*32/238, 32, 255), 0),      // Blue       (target, 9, 32)   #1  always about FLOOR(secondary * 0.3)
            };

			ColorRange[] colorRangeGenerated = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(c0, 10),
                new ColorRange(c1, 25),
                new ColorRange(c2, 10),
                new ColorRange(c3, 10),
                new ColorRange(c3, -1),
                new ColorRange(c4, 15),
                new ColorRange(c1, 0),
            };

			SetPaleteFromColorRange(colorRangeGenerated);
		}
		#endregion

		/// <summary>
		/// GREEN is too turquoise, and this method is my attempt to change that, but this turned out WAAAAY too turquoise, and it turns out green flames often have turquoise, so the original experiment was a success
		/// 
		/// This method is deprecated unless I want to try another experiment.
		/// </summary>
		/// <param name="target"></param>
		private void InitializeExperimentalFlameCurveV2(Color target)
		{
			Color c0 = GetColorCurve(target, 0.5f, 0, 0.0f);
			Color c1 = GetColorCurve(target, 1.0f, 185, 0.0f);
			Color c2 = GetColorCurve(target, 1.0f, 255, 0.0f);
			Color c3 = GetColorCurve(target, 1.0f, 255, (196.0f / 255.0f));
			Color c4 = GetColorCurve(target, 1.0f, 255, 1.0f);
			Color c5 = Color.FromArgb(212, 212, 255);

			ColorRange[] colorRangeBlueWhiteOrangeRed = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(Color.FromArgb(127, 0, 0), 10),     // Red                #0
                new ColorRange(Color.FromArgb(255, 185, 0), 25),   // Orange             #1
                new ColorRange(Color.FromArgb(255, 255, 0), 10),  // Bright Yellow       #2
                new ColorRange(Color.FromArgb(255, 255, 196), 10), // Bright Yellow      #3
                new ColorRange(Color.FromArgb(255, 255, 196), -1), // Bright Yellow      #3
                new ColorRange(Color.FromArgb(255, 255, 255), 15), // White              #4
                //new ColorRange(Color.FromArgb(255, 185, 0), 5),   // Orange
                new ColorRange(Color.FromArgb(212, 212, 255), 0), // Light Blue          #5
            };

			ColorRange[] colorRangeExperiment = {
                new ColorRange(Color.FromArgb(0, 0, 0), 10),       // Black
                new ColorRange(Color.FromArgb(0, 0, 0), 30),       // Black
                new ColorRange(c0, 10),     // Dark Blue  (HALF, 9, 32)     #0
                new ColorRange(c1, 25),     // Blue       (TARGET, 9, 32)   #1
                new ColorRange(c2, 10), // Bright Blue  (target, 37, 127) #2
                new ColorRange(c3, 10), // Bright Blue          (target, 71, 238) #3
                new ColorRange(c3, -1), // Bright Blue          (target, 71, 238) #3
                new ColorRange(c4, 15), // ???            (target, 19, 64)  #4
                new ColorRange(c5, 0),      // Blue       (target, 9, 32)   #1  always about FLOOR(secondary * 0.3)
            };


			SetPaleteFromColorRange(colorRangeExperiment);
		}

		// JRDV: (Mostly) Unused right now.  I'm happy with the palleteized version
		#region Unfinished 32bit Mode
		private void Initialize32Bit()
		{
			m_fPallete = false;
			if (bmToDraw == null || bmToDraw.Height != fireHeight || bmToDraw.Width != fireWidth)
			{
				bmToDraw = new Bitmap(fireWidth, fireHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			}
		}

		private void Draw32Bit()
		{
			int x, y;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				int rTemp = 255 * rng.Next(2);
				int gTemp = 255 * rng.Next(2);
				int bTemp = 255 * rng.Next(2);
				Color seed = Color.FromArgb(rTemp, gTemp, bTemp);
				bmToDraw.SetPixel(x, fireHeight - 1, seed);
			}
			for (y = 1; y < fireHeight - 1; ++y)
				for (x = 1; x < fireWidth - 1; ++x)
				{
					// Get the surrounding colors, subtract some amount, average them, then write the result
					Color cFinal;
					Color up, left, right, down, original;
					Color leftCorner, rightCorner;
					int r1, g1, b1;

					up = bmToDraw.GetPixel(x, y - 1);
					left = bmToDraw.GetPixel(x - 1, y);
					original = bmToDraw.GetPixel(x, y);
					right = bmToDraw.GetPixel(x + 1, y);
					leftCorner = bmToDraw.GetPixel(x - 1, y + 1);
					down = bmToDraw.GetPixel(x, y + 1);
					rightCorner = bmToDraw.GetPixel(x + 1, y + 1);

					//r1 = (original.R + up.R + left.R + right.R + down.R) / 5;
					//g1 = (original.G + up.G + left.G + right.G + down.G) / 5;
					//b1 = (original.B + up.B + left.B + right.B + down.B) / 5;

					// Rule1 : New = (A+B+C)/3
					//r1 = (leftCorner.R + down.R + rightCorner.R) / 3;
					//g1 = (leftCorner.G + down.G + rightCorner.G) / 3;
					//b1 = (leftCorner.B + down.B + rightCorner.B) / 3;

					// Rule2 : New = (original+A+B+C)/4
					r1 = (original.R + leftCorner.R + down.R + rightCorner.R) / 4;
					g1 = (original.G + leftCorner.G + down.G + rightCorner.G) / 4;
					b1 = (original.B + leftCorner.B + down.B + rightCorner.B) / 4;

#if true // AH!  This is for 32bit, and not used currently
					if (r1 > 0)
						--r1;
					if (g1 > 0)
						--g1;
					if (b1 > 0)
						--b1;
#endif
					cFinal = Color.FromArgb(r1, g1, b1);
					bmToDraw.SetPixel(x, y, cFinal);
				}
			graph.DrawImage(bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);
		}
		#endregion

		#region Seed Coal Values

		private void Seed8bitSqueak()
		{
			int x;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				int seed = rng.Next(54, 256);
				flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = seed;
			}
		}
		private void Seed8bitSqueakNOT()
		{
			int x;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				int seed = rng.Next(200, 256);
				flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = seed;
			}
		}
		private void Seed8bitExtremes()
		{
			int x;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				int seed = 255 * rng.Next(2);
				flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = seed;
			}
		}
		private void Seed8bitFullRange()
		{
			int x;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				int seed = rng.Next(256);
				flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = seed;
			}
		}
		private void Seed8bitKeepMostOfTheOldValues()
		{
			int x;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				int seed = rng.Next(26);
				if (seed == 0)
					flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = 255;
				else if (seed == 1)
					flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = 0;
			}
		}
		private void Seed8BitCustom()
		{
			int x;
			// Randomly seed the final line
			for (x = 0; x < fireWidth; ++x)
			{
				if (coalSeedPercentNumericUpDown.Value > rng.Next(100))
				{
					if (coalSeedRangeCheckBox.Checked)
					{
						int seed = rng.Next(2);
						if (seed == 0)
							flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = (int)coalSeedMaxNumericUpDown.Value;
						else if (seed == 1)
							flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = (int)coalSeedMinNumericUpDown.Value;
					}
					else
						flameIntensityMatrixFront[x + fireWidth * (fireHeight - 1)] = rng.Next((int)coalSeedMinNumericUpDown.Value,
																							(int)coalSeedMaxNumericUpDown.Value);
				}
			}
		}


        #region Expensive Bolt Experiment
        class BoltNode
        {
			public BoltNode(int x, int y, float potential = 0.5f)
            {
				this.x = x;
				this.y = y;
				this.potential = potential;
            }

			public readonly int x;
			public readonly int y;

			public bool potentialBolt = true; // set to FALSE to commit the pixel
			public float potential = 0.5f;
			public int intensity = 10; // TODO: later only set the main trunk of the tree to 255.
			public BoltNode left = null;
			public BoltNode right = null;
			public BoltNode down = null;

			public float TotalPotential()
            {
				if (potentialBolt)
					return potential;

				float fullPotential = 0;

				if (down != null)
					fullPotential += down.TotalPotential();

				if (left != null)
					fullPotential += left.TotalPotential();

				if (right != null)
					fullPotential += right.TotalPotential();

				return fullPotential;
			}

			public bool BoostIntensityOfMainBolt(int sentinel)
			{
				if (this.y == sentinel)
				{
					this.intensity = 255;
					return true;
				}

				if (down != null && down.BoostIntensityOfMainBolt(sentinel)
					|| left != null && left.BoostIntensityOfMainBolt(sentinel)
					|| right != null && right.BoostIntensityOfMainBolt(sentinel))
				{
					this.intensity = 255;
					return true;
				}

				return false;
            }

			public BoltNode InsertNewBolt(ref float winner)
            {
				BoltNode nodeWinner = null;
				if (winner <= 0)
					return null;

				if (potentialBolt)
				{
					if (potential < winner)
					{
						winner -= potential; // LOST! Subtract out our potential
						return null;
					}

					// WON!
					potentialBolt = false;
					winner = 0;

					// TODO: JRDV: Add a parameter to set the potential, so anything outside the visible portion gets ZERO potential
//					this.left = new BoltNode(this.x - 1, y);
//					this.down = new BoltNode(this.x , y + 1);
//					this.right = new BoltNode(this.x + 1, y);

					return this;
				}

				//winner -= potential; // LOST! Subtract out our potential

				if (winner > 0 && down != null)
					nodeWinner = down.InsertNewBolt(ref winner);

				if (winner > 0 && left != null)
					nodeWinner = left.InsertNewBolt(ref winner);

				if (winner > 0 && right != null)
					nodeWinner = right.InsertNewBolt(ref winner);

				return nodeWinner;
			}

			public bool IsExists(int x, int y)
            {
				bool exists = false;

				if (this.x == x && this.y == y)
					return true;

				if (down != null)
					exists = down.IsExists(x, y);

				if (!exists && left != null)
					exists = left.IsExists(x, y);

				if (!exists && right != null)
					exists = right.IsExists(x, y);

				return exists;
			}
		}


		BoltNode CreateNewBoltNode(BoltNode root, int x, int y)
		{
			if (root.IsExists(x, y))
				return null;

			float potential = ((float)((y + 1) * 10) / (float)fireHeight);

			if (x < 0 || x >= fireWidth)
				potential = 0;

			if (y < 0 || y >= fireHeight)
				potential = 0;

			return new BoltNode(x, y, potential);
		}

		private void Seed8BitLightning_Branching_EXPENSIVE()
		{
			// starting with the center point for now. Could also randomize the starting point.
			BoltNode root = new BoltNode(fireWidth / 2, 1);
			BoltNode nodeWinner;

			do
			{
				float winner = (float)(rng.NextDouble() * root.TotalPotential());
				nodeWinner = root.InsertNewBolt(ref winner);

				if (nodeWinner == null)
					break;

				// TODO: JRDV: Add a parameter to set the potential, so anything outside the visible portion gets ZERO potential
				nodeWinner.left = CreateNewBoltNode(root, nodeWinner.x - 1, nodeWinner.y);
				nodeWinner.down = CreateNewBoltNode(root, nodeWinner.x, nodeWinner.y + 1);
				nodeWinner.right = CreateNewBoltNode(root, nodeWinner.x + 1, nodeWinner.y);

			} while (/*nodeWinner.down != null &&*/ nodeWinner.y + 1 < this.fireHeight); // DONE

			// Algorithm idea.
			// Linked list of pixels?
			// Tree of pixels, which includes the bolt path so far, plus neighbors.
			// Start with seed.
			// Each neighbor has a potential/probability for getting picked.
			// Perhaps, start with equal probability, and figure out how to fine tune it later? Slightly weight in favor of toward the ground?

			// Iterate until a bolt hits the ground.
			// set intensity for main section to 255
			// set all others to some lower value.
			// Render all the pixels.
			// TODO: Store the path for later, so we can optimize the decay algorithm (VERY IMPORTANT FOR NDADD)

			// Render the BOLT!
			root.BoostIntensityOfMainBolt(fireHeight);
			RenderBoltTree(root);
		}

		void RenderBoltTree(BoltNode current)
        {
			if (current == null || current.potentialBolt) // not realized
				return;

			int x = current.x;
			int y = current.y;
			flameIntensityMatrixFront[x + y * fireWidth] = current.intensity;

			RenderBoltTree(current.left);
			RenderBoltTree(current.down);
			RenderBoltTree(current.right);
		}

		#endregion
		private void Seed8BitLightning_Linear()
		{
			int x = fireWidth / 2;
			// Randomly seed the lightning path
			for (int y = 0; y < fireHeight - 1; ++y)
			{
				int diff = rng.Next(3) - 1;

				x += diff;
				if (x < 0)
					x = 0;
				if (x > fireWidth)
					x = fireWidth;

				flameIntensityMatrixFront[x + y * fireWidth] = 255;
			}
		}

		private void Seed8BitLightning_Branching_Cheap_DOTS()
		{
			List<int> nodes = new List<int>(1);

			nodes.Add(fireWidth / 2);

			// Randomly seed the lightning path
			for (int y = 0; y < fireHeight - 1; ++y)
			{
				/// FIDDLE WITH THE RARITY!
				/// Fork the bolt sometimes.
				/// 
				if (rng.Next(160) == 0)
				{
					int nodeToFork = rng.Next(nodes.Count);

					if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
					{
						nodes.Add(nodes[nodeToFork] + 2);
						nodes[nodeToFork] -= 2;
					}
				}

				for (int n = 0; n < nodes.Count; ++n)
				{
					int diff = rng.Next(-2, 3);

					int x = nodes[n];
					x += diff;
					if (x < 0)
						x = 0;
					if (x > fireWidth)
						x = fireWidth;

					nodes[n] = x;

					flameIntensityMatrixFront[x + y * fireWidth] = 255;
				}
			}
		}
		private void Seed8BitLightning_Branching_Cheap_LINES()
		{
			List<int> nodes = new List<int>(1);

			nodes.Add(fireWidth / 2);

			int yBranchMore = fireHeight * 2 / 3;

			// Randomly seed the lightning path
			for (int y = 0; y < fireHeight - 1; ++y)
			{
				/// FIDDLE WITH THE RARITY!
				/// Fork the bolt sometimes.
				/// 
				if (rng.Next(y > yBranchMore ? 50 : 360) == 0)
				{
					int nodeToFork = rng.Next(nodes.Count);

					if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
					{
						int diff1 = rng.Next(2, 4);
						int diff2 = rng.Next(2, 4);
						nodes.Add(nodes[nodeToFork] + diff1);
						nodes[nodeToFork] -= diff2;
					}
				}

				for (int n = 0; n < nodes.Count; ++n)
				{
					int diff = rng.Next(-2, 3);

					int x = nodes[n];
					x += diff;
					if (x < 0)
						x = 0;
					if (x > fireWidth)
						x = fireWidth;

					//nodes[n] = x;

					//flameIntensityMatrixFront[x + y * fireWidth] = 255;

					int delta = x - nodes[n];
					int step = 0;
					if (delta < 0)
						step = -1;
					else if (delta > 0)
						step = 1;

					int eachx = nodes[n];
					bool isFirstIteration = true;
					do
					{
						if (!isFirstIteration)
							eachx += step;

						isFirstIteration = false;

						flameIntensityMatrixFront[eachx + y * fireWidth] = 255;

					} while (eachx != x);

					nodes[n] = x; // TODO: Draw every pixel between the last position and this position! Then increase the variance (the random delta above)
				}
			}
		}

		private void Seed8BitLightning_BorgRing()
		{
			Seed8BitLightning_BorgRing(255, -2);
		}

		private void Seed8BitLightning_BorgRing(int intensity, int adjust)
		{
			int xCenter = fireWidth / 2;
			int yCenter = fireHeight / 2;
			int radius = Math.Min(xCenter, yCenter) + adjust;
			int xLast = radius;

			for (int yy = yCenter - radius; yy <= yCenter + radius; ++yy)
			{
				//int x = xCenter - radius;
				// x^2 + y+2 == radius^2
				// x = SQRT(radius^2 - y^2);
				int y = (yy - yCenter);
				int x = (int)(Math.Sqrt(radius * radius - y * y));
				int xx = (x + xCenter);
				flameIntensityMatrixFront[xx + yy * fireWidth] = intensity;
				flameIntensityMatrixFront[(fireWidth - xx) + yy * fireWidth] = intensity;

				if (yy < yCenter)
				{
					for (int xxx = xLast; xxx < xx; ++xxx)
					{
						flameIntensityMatrixFront[xxx + yy * fireWidth] = intensity;
						flameIntensityMatrixFront[(fireWidth - xxx) + yy * fireWidth] = intensity;
					}
				}
				else
				{
					for (int xxx = xx; xxx < xLast; ++xxx)
					{
						flameIntensityMatrixFront[xxx + (yy) * fireWidth] = intensity;
						flameIntensityMatrixFront[(fireWidth - xxx) + (yy) * fireWidth] = intensity;
					}
				}
				xLast = xx;
			}
		}

		private void Seed8BitLightning_LinearBorg()
		{
			int xCenter = fireWidth / 2;
			int yCenter = fireHeight / 2;
			int radius = Math.Min(xCenter, yCenter) - 1;

			int x = xCenter;
			// Randomly seed the lightning path
			for (int y = yCenter; y < yCenter + radius; ++y)
			{
				int diff = rng.Next(3) - 1;

				x += diff;
				if (x < 0)
					x = 0;
				if (x > fireWidth)
					x = fireWidth;

				flameIntensityMatrixFront[x + y * fireWidth] = 255;
			}



			// Seed UP from center!
			x = xCenter;

			// Randomly seed the lightning path
			for (int y = yCenter; y > yCenter - radius; --y)
			{
				int diff = rng.Next(3) - 1;

				x += diff;
				if (x < 0)
					x = 0;
				if (x > fireWidth)
					x = fireWidth;

				flameIntensityMatrixFront[x + y * fireWidth] = 255;
			}

		}

		private void Seed8BitLightning_LinearBorg_RandomRotation()
		{
			int xCenter = fireWidth / 2;
			int yCenter = fireHeight / 2;
			int radius = Math.Min(xCenter, yCenter) - 2;
			int rotationAngle = rng.Next(0, 360);

			int x = xCenter;
			// Randomly seed the lightning path
			for (int y = yCenter; y < yCenter + radius; ++y)
			{
				int diff = rng.Next(-2, 3);

				x += diff;
				if (x < 0)
					x = 0;
				if (x > fireWidth)
					x = fireWidth;

				int xTemp = x - xCenter;
				int yTemp = y - yCenter;

				int xRender = (int)(xTemp * Math.Cos(rotationAngle) - yTemp * Math.Sin(rotationAngle)) + xCenter;
				int yRender = (int)(xTemp * Math.Sin(rotationAngle) + yTemp * Math.Cos(rotationAngle)) + yCenter;

				flameIntensityMatrixFront[xRender + yRender * fireWidth] = 255;
			}
		}

		private void Seed8BitLightning_ForkingBorg_RandomRotation()
		{
			List<int> nodes = new List<int>(1);

			int xCenter = fireWidth / 2;
			int yCenter = fireHeight / 2;
			int radius = Math.Min(xCenter, yCenter) - 2;
			int rotationAngle = rng.Next(0, 360);

			nodes.Add(xCenter);

			// Randomly seed the lightning path
			for (int y = yCenter; y < yCenter + radius; ++y)
			{
				/// FIDDLE WITH THE RARITY!
				/// Fork the bolt sometimes.
				/// 
				if (rng.Next(106) == 0)
				{
					int nodeToFork = rng.Next(nodes.Count);

					if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
					{
						nodes.Add(nodes[nodeToFork] + 2);
						nodes[nodeToFork] -= 2;
					}
				}

				for (int n = 0; n < nodes.Count; ++n)
				{
					int diff = rng.Next(-2, 3);

					int x = nodes[n];
					x += diff;
					if (x < 0)
						x = 0;
					if (x > fireWidth)
						x = fireWidth;

					nodes[n] = x; // TODO: Draw every pixel between the last position and this position! Then increase the variance (the random delta above)

					int xTemp = x - xCenter;
					int yTemp = y - yCenter;

					// if we exceed teh ring DONE!
					if (xTemp * xTemp + yTemp * yTemp > radius * radius)
						return;

					int xRender = (int)(xTemp * Math.Cos(rotationAngle) - yTemp * Math.Sin(rotationAngle)) + xCenter;
					int yRender = (int)(xTemp * Math.Sin(rotationAngle) + yTemp * Math.Cos(rotationAngle)) + yCenter;

					flameIntensityMatrixFront[xRender + yRender * fireWidth] = 255;
				}
			}
		}

		private void Seed8BitLightning_ForkingBorg_RandomRotation_EXPERIMENT()
		{
			List<int> nodes = new List<int>(1);

			int xCenter = fireWidth / 2;
			int yCenter = fireHeight / 2;
			int radius = Math.Min(xCenter, yCenter) - 2;
			int rotationAngle = rng.Next(0, 360);

			nodes.Add(xCenter);

			// Randomly seed the lightning path
			for (int y = yCenter; y < yCenter + radius; ++y)
			{
				/// FIDDLE WITH THE RARITY!
				/// Fork the bolt sometimes.
				/// 
				if (rng.Next(106) == 0)
				{
					int nodeToFork = rng.Next(nodes.Count);

					if (nodes[nodeToFork] > 1 && nodes[nodeToFork] < fireWidth - 2)
					{
						int diff1 = rng.Next(1, 4);
						int diff2 = rng.Next(1, 4);
						nodes.Add(nodes[nodeToFork] + diff1);
						nodes[nodeToFork] -= diff2;
					}
				}

				for (int n = 0; n < nodes.Count; ++n)
				{
					int diff = rng.Next(-2, 3);

					int x = nodes[n];
					x += diff;
					if (x < 0)
						x = 0;
					if (x > fireWidth)
						x = fireWidth;


					int delta = x - nodes[n];
					int step = 0;
					if (delta < 0)
						step = -1;
					else if (delta > 0)
						step = 1;

					int eachx = nodes[n];
					bool isFirstIteration = true;
					do
					{
						if (!isFirstIteration)
							eachx += step;

						isFirstIteration = false;

						int xTemp = eachx - xCenter;
						int yTemp = y - yCenter;

						// if we exceed teh ring DONE!
						if (xTemp * xTemp + yTemp * yTemp > radius * radius)
							return;

						int xRender = (int)(xTemp * Math.Cos(rotationAngle) - yTemp * Math.Sin(rotationAngle)) + xCenter;
						int yRender = (int)(xTemp * Math.Sin(rotationAngle) + yTemp * Math.Cos(rotationAngle)) + yCenter;

						flameIntensityMatrixFront[xRender + yRender * fireWidth] = 255;

					} while (eachx != x);

					nodes[n] = x; // TODO: Draw every pixel between the last position and this position! Then increase the variance (the random delta above)
				}
			}
		}

#endregion

		// TODO: JRDV: Multiple processors.  Parallelize?  Split the work into N sets of rows.
#region Parellelized 8bit work

#if PARALLEL_8BIT

		private void DoThe8bitWork(int yMin, int yMax)
		{
			bool fDecayConstant = coolingFactorConstantRadioButton.Checked;
			bool fDecayCoolingMap = coolingMapRadioButton.Checked;
			bool fShiftCoolingMap = coolingMapShiftCheckBox.Checked;
			int coolingFactor = (int)decayUpDown.Value;
			int cPixelsToAverage = 0;
			bool p1, p2, p3, p4, p5, p6, p7, p8, p9;

			if (p1 = checkBox1.Checked)
				++cPixelsToAverage;
			if (p2 = checkBox2.Checked)
				++cPixelsToAverage;
			if (p3 = checkBox3.Checked)
				++cPixelsToAverage;
			if (p4 = checkBox4.Checked)
				++cPixelsToAverage;
			if (p5 = checkBox5.Checked)
				++cPixelsToAverage;
			if (p6 = checkBox6.Checked)
				++cPixelsToAverage;
			if (p7 = checkBox7.Checked)
				++cPixelsToAverage;
			if (p8 = checkBox8.Checked)
				++cPixelsToAverage;
			if (p9 = checkBox9.Checked)
				++cPixelsToAverage;

			if (cPixelsToAverage < 1)
				cPixelsToAverage = 1;

			for (int y = yMin; y < yMax; ++y)
			{
				for (int x = 1; x < fireWidth - 1; ++x)
				{
					// Get the surrounding colors, average them, subtract some amount, then write the result
					int iFinal = 0; // intensity index, 0 to 255

					int iOriginal, iNextRow, iPreviousRow;

					iOriginal = x + (y * fireWidth);
					iNextRow = iOriginal + fireWidth;
					iPreviousRow = iOriginal - fireWidth;

					// Pixels are numbered like the Numeric Keypad:
					// 7 8 9
					// 4 5 6
					// 1 2 3
					if (p7)
						iFinal += flameIntensityMatrixFront[iPreviousRow - 1];
					if (p8)
						iFinal += flameIntensityMatrixFront[iPreviousRow];
					if (p9)
						iFinal += flameIntensityMatrixFront[iPreviousRow + 1];
					if (p4)
						iFinal += flameIntensityMatrixFront[iOriginal - 1];
					if (p5)
						iFinal += flameIntensityMatrixFront[iOriginal];
					if (p6)
						iFinal += flameIntensityMatrixFront[iOriginal + 1];
					if (p1)
						iFinal += flameIntensityMatrixFront[iNextRow - 1];
					if (p2)
						iFinal += flameIntensityMatrixFront[iNextRow];
					if (p3)
						iFinal += flameIntensityMatrixFront[iNextRow + 1];

#if NEVER
					if (y < fireHeight - 2)
					{
						iFinal += flameIntensityMatrix[iNextRow + fireWidth];
						iFinal /= cPixelsToAverage;
					}
					else
					{
						iFinal /= (cPixelsToAverage-1);
					}
#endif
					iFinal /= cPixelsToAverage;

					if (fDecayConstant)
					{
						if (iFinal > coolingFactor)
							iFinal -= coolingFactor;
						else if (iFinal > 0)
							iFinal = 0;
					}
					else if (fDecayCoolingMap)
					{
						// scroll the cooling map with the flames as they rise
						int iCoolingOffset = iOriginal;
						if (fShiftCoolingMap)
						{
							iCoolingOffset = (iOriginal + (m_iFrame * fireWidth)) % coolingMap.Length;
						}

						if (iFinal > coolingMap[iCoolingOffset])
							iFinal -= coolingMap[iCoolingOffset];
						else
							iFinal = 0;

						// Uncomment to help debug the cooling map shift
						//if (iCoolingOffset == 10)
						//	iFinal = 255;
					}

					flameIntensityMatrixBackBuffer[iOriginal] = iFinal;

					bmToDraw.SetPixel(x, y, thePalette[iFinal]);
				} // end for(x)
			} // end for(y)

		}

		private void Draw8bitParallelized()
		{
			bool fShiftCoolingMap = coolingMapShiftCheckBox.Checked;
			int x, y;

			if (customSeedRadioButton.Checked)
				Seed8BitCustom();
			else if (fullRadioButton.Checked)
				Seed8bitFullRange();
			else if (extremeRadioButton.Checked)
				Seed8bitExtremes();
			else if (squeakRadioButton.Checked)
				Seed8bitSqueak();
			else if (notSqueakRadioButton.Checked)
				Seed8bitSqueakNOT();
			else
				Seed8bitKeepMostOfTheOldValues();

			// TODO: JRDV: Kick threads to parallelize work
			for (int i = 0; i < bgTasks.Length; ++i)
			{
				bgTasks[i].Resume();
			}

			int iRowsPerThread = (fireHeight - 2) / (bgTasks.Length + 1); // add 1, so the main thread does some work.
			int yMin = 1 + iRowsPerThread * bgTasks.Length;
			//int yMax = yMin + iRowsPerThread - 1;
			DoThe8bitWork(yMin, fireHeight - 1);
			// TODO: JRDV: Wait for threads to complete (probably a better way to do this)
			bool fCompleted = true;
			do
			{
				for (int i = 0; i < bgTasks.Length; ++i)
				{
					if (bgTasks[i].ThreadState == ThreadState.Running)
						fCompleted = false;
				}
			} while (!fCompleted);

			graph.DrawImage(this.bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);

			if (fShiftCoolingMap)
			{
				++m_iFrame;
				if (m_iFrame > fireHeight)
				{
					m_iFrame = 0;
					if (rotateCoolingMapCheckBox.Checked)
						UpdateRotatingCoolingMap();
				}
			}

			// copy the Coal Seed values to the back buffer.
			// NOTE: Don't the values get overwritten before the next time we compute the fire values?  Investigate, and verify.
			for (x = 0; x < fireWidth; ++x)
			{
				int iOffset = x + (fireWidth * fireHeight) - fireWidth;
				flameIntensityMatrixBackBuffer[iOffset] = flameIntensityMatrixFront[iOffset];
			}

			// swap the buffers.
			int[] tempBuffer = flameIntensityMatrixBackBuffer;
			flameIntensityMatrixBackBuffer = flameIntensityMatrixFront;
			flameIntensityMatrixFront = tempBuffer;
		}

#endif

#endregion

#region Original Draw8bit

		private void Draw8bitFire()
		{
			if (customSeedRadioButton.Checked)
				Seed8BitCustom();
			else if (fullRadioButton.Checked)
				Seed8bitFullRange();
			else if (extremeRadioButton.Checked)
				Seed8bitExtremes();
			else if (squeakRadioButton.Checked)
				Seed8bitSqueak();
			else if (notSqueakRadioButton.Checked)
				Seed8bitSqueakNOT();
			else
				Seed8bitKeepMostOfTheOldValues();

			RenderDissipation8bit();

			// copy the Coal Seed values to the back buffer.
			// NOTE: Don't the values get overwritten before the next time we compute the fire values?  Investigate, and verify.
			for (int x = 0; x < fireWidth; ++x)
			{
				int iOffset = x + (fireWidth * fireHeight) - fireWidth;
				flameIntensityMatrixBackBuffer[iOffset] = flameIntensityMatrixFront[iOffset];
			}

			Draw8bitAndSwapBuffers();
		}

		// Just isolating the code so I can make experimental changes without breaking the Fire code
		// I will likely keep the ability to use the decay stuff long term. but
		// I think it will look better if we clear it more directly,
		// or rather it ought to be MASSIVELY faster to not run the decay algorithm over the full grid,
		// but only over the lightning path
		//
		// For this implementation, the big difference is the seed location is determined by the lightning path, which is not fixed.
		private void Draw8bitLightning()
		{
			if (m_fBorg)
			{
				//Seed8BitLightning_LinearBorg();
				//Seed8BitLightning_LinearBorg_RandomRotation();
				for (int i = rng.Next(1, 3); i > 0; --i)
				{
					//Seed8BitLightning_ForkingBorg_RandomRotation();
					Seed8BitLightning_ForkingBorg_RandomRotation_EXPERIMENT();
				}

				Seed8BitLightning_BorgRing(255, -2);
			}
			else
            {
				bool fSeed = rng.Next(25) == 0;
				if (fSeed)
				{
					//Seed8BitLightning_Linear();
					//Seed8BitLightning_Branching_EXPENSIVE(); // The main bolt looks nice, and I bet the secondary branches would look nice too, but it's really expensive.
					//Seed8BitLightning_Branching_Cheap_DOTS();
					Seed8BitLightning_Branching_Cheap_LINES();
				}
			}

		//	if (m_fBorg)
		//	{
		//		Seed8BitLightning_BorgRing(0, -1);
		//		Seed8BitLightning_BorgRing(0, 0);
		//		Seed8BitLightning_BorgRing(0, 1);
		//	}

			RenderDissipation8bit();
			Draw8bitAndSwapBuffers();

#if false // fake it for screen captures :)
			if (fSeed)
			{
				timer1.Enabled = false;
				startTheFireButton.Text = "Start the Fire!";
			}
#endif
		}

		private void RenderDissipation8bit()
		{
			bool fDecayConstant = coolingFactorConstantRadioButton.Checked;
			bool fDecayCoolingMap = coolingMapRadioButton.Checked;
			bool fShiftCoolingMap = coolingMapShiftCheckBox.Checked;
			int coolingFactor = (int)decayUpDown.Value;
			int cPixelsToAverage = 0;
			int x, y;
			bool p1, p2, p3, p4, p5, p6, p7, p8, p9;

			if (p1 = checkBox1.Checked)
				++cPixelsToAverage;
			if (p2 = checkBox2.Checked)
				++cPixelsToAverage;
			if (p3 = checkBox3.Checked)
				++cPixelsToAverage;
			if (p4 = checkBox4.Checked)
				++cPixelsToAverage;
			if (p5 = checkBox5.Checked)
				++cPixelsToAverage;
			if (p6 = checkBox6.Checked)
				++cPixelsToAverage;
			if (p7 = checkBox7.Checked)
				++cPixelsToAverage;
			if (p8 = checkBox8.Checked)
				++cPixelsToAverage;
			if (p9 = checkBox9.Checked)
				++cPixelsToAverage;

			if (cPixelsToAverage < 1)
				cPixelsToAverage = 1;

			for (y = 1; y < fireHeight - 1; ++y)
			{
				for (x = 1; x < fireWidth - 1; ++x)
				{
					// Get the surrounding colors, average them, subtract some amount, then write the result
					int iFinal = 0; // intensity index, 0 to 255

					int iOriginal, iNextRow, iPreviousRow;

					iOriginal = x + (y * fireWidth);
					iNextRow = iOriginal + fireWidth;
					iPreviousRow = iOriginal - fireWidth;

					// Pixels are numbered like the Numeric Keypad:
					// 7 8 9
					// 4 5 6
					// 1 2 3
					if (p7)
						iFinal += flameIntensityMatrixFront[iPreviousRow - 1];
					if (p8)
						iFinal += flameIntensityMatrixFront[iPreviousRow];
					if (p9)
						iFinal += flameIntensityMatrixFront[iPreviousRow + 1];
					if (p4)
						iFinal += flameIntensityMatrixFront[iOriginal - 1];
					if (p5)
						iFinal += flameIntensityMatrixFront[iOriginal];
					if (p6)
						iFinal += flameIntensityMatrixFront[iOriginal + 1];
					if (p1)
						iFinal += flameIntensityMatrixFront[iNextRow - 1];
					if (p2)
						iFinal += flameIntensityMatrixFront[iNextRow];
					if (p3)
						iFinal += flameIntensityMatrixFront[iNextRow + 1];

#if NEVER
					if (y < fireHeight - 2)
					{
						iFinal += flameIntensityMatrix[iNextRow + fireWidth];
						iFinal /= cPixelsToAverage;
					}
					else
					{
						iFinal /= (cPixelsToAverage-1);
					}
#endif
					iFinal /= cPixelsToAverage;

					if (fDecayConstant)
					{
						if (iFinal > coolingFactor)
							iFinal -= coolingFactor;
						else if (iFinal > 0)
							iFinal = 0;
					}
					else if (fDecayCoolingMap)
					{
						// scroll the cooling map with the flames as they rise
						int iCoolingOffset = iOriginal;
						if (fShiftCoolingMap)
						{
							iCoolingOffset = (iOriginal + (m_iFrame * fireWidth)) % coolingMap.Length;
						}
						else
							iCoolingOffset = (iOriginal) % coolingMap.Length; // HMM, shouldn't be necessary, but fixes a crash if shift is disabled

						if (iFinal > coolingMap[iCoolingOffset])
							iFinal -= coolingMap[iCoolingOffset];
						else
							iFinal = 0;

						// Uncomment to help debug the cooling map shift
						//if (iCoolingOffset == 10)
						//	iFinal = 255;
					}

					flameIntensityMatrixBackBuffer[iOriginal] = iFinal;

					bmToDraw.SetPixel(x, y, thePalette[iFinal]);
				} // end for(x)
			} // end for(y)

			if (fShiftCoolingMap)
			{
				++m_iFrame;
				if (m_iFrame > fireHeight)
				{
					m_iFrame = 0;
					if (rotateCoolingMapCheckBox.Checked)
						UpdateRotatingCoolingMap();
				}
			}
		}

		private void Draw8bitAndSwapBuffers()
        {
			// TODO: Bicubic intrpolation??
			graph.DrawImage(this.bmToDraw, this.drawingX, this.drawingY, this.drawnWidth, this.drawnHeight);

			// swap the buffers.
			int[] tempBuffer = flameIntensityMatrixBackBuffer;
			flameIntensityMatrixBackBuffer = flameIntensityMatrixFront;
			flameIntensityMatrixFront = tempBuffer;
		}

#endregion

#region Various Control Event Handlers

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (m_fUpdateFireDimensionsAfterNextFrame)
			{
				UpdateFireDimensions();
				m_fUpdateFireDimensionsAfterNextFrame = false;
			}
			// Draw the frame once per tick.
			// Select 32bit vs pallete color algorithm... and flame decay, intensity, color, etc...
			if (m_fPallete)
			{
				if (m_fLighting)
					Draw8bitLightning();
				else
#if PARALLEL_8BIT
					Draw8bitParallelized();
#else
					Draw8bitFire();
#endif
			}
			else
				Draw32Bit();
		}

		private void startTheFireButton_Click(object sender, EventArgs e)
		{
			if (!timer1.Enabled)
			{
				UpdateFireDimensions();

				if (rotateCoolingMapCheckBox.Checked)
					UpdateRotatingCoolingMap();

				for (int xx = 0; xx < fireWidth; ++xx)
					for (int yy = 0; yy < fireHeight; ++yy)
					{
						bmToDraw.SetPixel(xx, yy, Color.Black);
					}
				startTheFireButton.Text = "Stop the Fire!";
				timer1.Interval = (int)(1000 / fpsNumericUpDown.Value);
				timer1.Enabled = true;
			}
			else
			{
				timer1.Enabled = false;
				startTheFireButton.Text = "Start the Fire!";
			}
		}

		private void colorPickerButton_Click(object sender, EventArgs e)
		{
			DialogResult dr = colorDialog1.ShowDialog();
			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				SetSingleColorFlame(colorDialog1.Color);
			}
		}

		private void firePaletteButton_Click(object sender, EventArgs e) // RED!
		{
			SetSingleColorFlame(Color.FromArgb(255, 0, 0));
			InitializeRedFlame(); // 4 point
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void orangeFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.DarkOrange);
			InitializeOrangeFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void yellowFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(255, 255, 0));
			InitializeYellowFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void blueFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(0, 0, 255));
			InitializeBlueFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void cyanFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(0, 255, 255));
			InitializeCyanFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void blueGreenFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(0, 255, 128));
			InitializeBlueGreenFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void greenFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(0, 255, 0));
			InitializeGreenFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void violetFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(128, 0, 255));
			InitializeVioletFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void magentaFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.FromArgb(255, 0, 255));
			InitializeMagentaFlame();
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void blackFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.Black);
			InitializeBlackFlame();
			m_fOverrideEnabledForSpecialFlame = false;
			RefreshTheFlamePalette();
			//InitializeBlackFlameCurve();
			UpdateUI_Visibility();
		}

		private void whiteFlameButton_Click(object sender, EventArgs e)
		{
			SetSingleColorFlame(Color.White);
			InitializeWhiteFlame();
			m_fOverrideEnabledForSpecialFlame = false;
			RefreshTheFlamePalette();
			//InitializeWhiteFlameCurve();
			UpdateUI_Visibility();
		}

		private void pinkLighningButton_Click(object sender, EventArgs e)
		{
			// I think a core problem is that the FLAME curve algorithm doesn't pick great values for lightning.
			InitializeMagentaFlame();
			SetSingleColorFlame(Color.FromArgb(255, 236, 236)); // GREEN (looks good for lightning, but unnatural. Looks BAD for BORG. Looks OK for flame.)
			SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK

			RefreshTheFlamePalette();
			realisticRenderMethodRadioButton.Checked = true;
			//InitializeWhiteFlameCurve();
			UpdateUI_Visibility();
		}

		private void blueLighningButton_Click(object sender, EventArgs e)
		{
			// I think a core problem is that the FLAME curve algorithm doesn't pick great values for lightning.
			InitializeCyanFlame();
			SetSingleColorFlame(Color.FromArgb(255, 236, 236)); // GREEN (looks good for lightning, but unnatural. Looks BAD for BORG. Looks OK for flame.)
			SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK
			SetSingleColorFlame(Color.FromArgb(128, 255, 255)); // BLUE

			RefreshTheFlamePalette();
			realisticRenderMethodRadioButton.Checked = true;
			//InitializeWhiteFlameCurve();
			UpdateUI_Visibility();
		}

		private void realisticLightningButton_Click(object sender, EventArgs e)
		{
			// I think a core problem is that the FLAME curve algorithm doesn't pick great values for lightning.
			InitializeCyanFlame();
			SetSingleColorFlame(Color.FromArgb(255, 236, 236)); // GREEN (looks good for lightning, but unnatural. Looks BAD for BORG. Looks OK for flame.)
			SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK
			SetSingleColorFlame(Color.FromArgb(128, 255, 255)); // BLUE
//			SetSingleColorFlame(Color.Indigo);
			m_fOverrideEnabledForSpecialFlame = false;

#if false
			linearRenderMethodRadioButton.Checked = true;
			color1 = Color.Black;
			color2 = Color.FromArgb(255, 225, 255);
			color3 = Color.FromArgb(0, 225, 255);
			color4 = Color.White;
			InitializePalette(color1, color2, color3, color4);
#endif

			SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK
			RefreshTheFlamePalette();

			for (int i = 0; i < thePalette.Length; ++i)
			{
				Color c = thePalette[i];
				thePalette[i] = Color.FromArgb(c.G, c.B, (int)(c.R * 1));
			}

			SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK
			RefreshTheFlamePalette();

			Color[] theOldPalette = thePalette;
			thePalette = null;
			SetSingleColorFlame(Color.FromArgb(128, 255, 255)); // BLUE
			RefreshTheFlamePalette();
			realisticRenderMethodRadioButton.Checked = true;

			int switchPoint = 382;
			int whitePoint = 380; // Yes, I know setting the WhitePoint below the SwitchPoint means we don't use the old palette....
			// it looks better without the pink ring inside.
			// Looks better to start with blue, and transition to red (the logic, which may be completely false, is particles go fast-->blue, slow to red as it fades).
            for (int i = 0; i < switchPoint && i < thePalette.Length; ++i)
			{
				Color c1 = theOldPalette[i];
				Color c2 = thePalette[i];
				float pBlue = 0.55f;
				float pRed = (1.0f - pBlue);
				thePalette[i] = Color.FromArgb(
					(int)((c1.R * pRed + c2.R * pBlue) + 0.5f),
					(int)((c1.G * pRed + c2.G * pBlue) + 0.5f),
					(int)((c1.B * pRed + c2.B * pBlue) + 0.5f));
			}

            for (int i = switchPoint; i < thePalette.Length; ++i)
            {
                Color c = theOldPalette[i];
                thePalette[i] = c;
            }

            //SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK
            //SetSingleColorFlame(Color.FromArgb(128, 255, 255)); // BLUE
            //RefreshTheFlamePalette();

            for (int i = whitePoint; i < thePalette.Length; ++i)
            {
                thePalette[i] = Color.White;
            }


            //InitializeWhiteFlameCurve();
            UpdateUI_Visibility();
		}


		private void squeakPaletteButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < thePalette.Length; ++i)
			{
				thePalette[i] = Color.FromArgb(
									i,
									(int)(0.6 * i), // about i / 4
									(int)(0.1 * i)); // about i / 25)
			}

			color1 = thePalette[0];
			color2 = thePalette[85];
			color3 = thePalette[170];
			color4 = thePalette[255];
		}

		bool m_fOverrideEnabledForSpecialFlame = false;
		private void blueYellowFlameButton_Click(object sender, EventArgs e)
		{
			m_fOverrideEnabledForSpecialFlame = true;

			InitializeBlueYellowFlame();
			//InitializeRealisticFlame(); // This is the Realistic, don't change it
			RefreshTheFlamePalette();
			//RefreshTheFlamePalette();
			//InitializePalette(color1, color2, color3, color4);
			UpdateUI_Visibility();
		}

		private void widthTrackBar_Scroll(object sender, EventArgs e)
		{
			widthNumericUpDown.Value = widthTrackBar.Value;
		}

		private void heightTrackBar_Scroll(object sender, EventArgs e)
		{
			heightNumericUpDown.Value = heightTrackBar.Value;
		}

		private void widthNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			widthTrackBar.Value = (int)widthNumericUpDown.Value;
			m_fUpdateFireDimensionsAfterNextFrame = true;
		}

		private void heightNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			heightTrackBar.Value = (int)heightNumericUpDown.Value;
			m_fUpdateFireDimensionsAfterNextFrame = true;
		}

		private void customColorButton1_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = userSelectedColor1;
			DialogResult dr = colorDialog1.ShowDialog();
			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				userSelectedColor1 = colorDialog1.Color;
				customColorButton1.BackColor = userSelectedColor1;

				color1 = userSelectedColor1;
				color2 = userSelectedColor2;
				color3 = userSelectedColor3;
				color4 = userSelectedColor4;

				InitializePalette(userSelectedColor1, userSelectedColor2, userSelectedColor3, userSelectedColor4);
				UpdateUI_Visibility();
			}
		}

		private void customColorButton2_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = userSelectedColor2;
			DialogResult dr = colorDialog1.ShowDialog();
			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				userSelectedColor2 = colorDialog1.Color;
				customColorButton2.BackColor = userSelectedColor2;

				color1 = userSelectedColor1;
				color2 = userSelectedColor2;
				color3 = userSelectedColor3;
				color4 = userSelectedColor4;

				InitializePalette(userSelectedColor1, userSelectedColor2, userSelectedColor3, userSelectedColor4);
				UpdateUI_Visibility();
			}
		}

		private void customColorButton3_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = userSelectedColor3;
			DialogResult dr = colorDialog1.ShowDialog();
			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				userSelectedColor3 = colorDialog1.Color;
				customColorButton3.BackColor = userSelectedColor3;

				color1 = userSelectedColor1;
				color2 = userSelectedColor2;
				color3 = userSelectedColor3;
				color4 = userSelectedColor4;

				InitializePalette(userSelectedColor1, userSelectedColor2, userSelectedColor3, userSelectedColor4);
				UpdateUI_Visibility();
			}
		}

		private void customColorButton4_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = color4;
			DialogResult dr = colorDialog1.ShowDialog();
			if (dr == System.Windows.Forms.DialogResult.OK)
			{
				userSelectedColor4 = colorDialog1.Color;
				customColorButton4.BackColor = userSelectedColor4;

				color1 = userSelectedColor1;
				color2 = userSelectedColor2;
				color3 = userSelectedColor3;
				color4 = userSelectedColor4;

				InitializePalette(userSelectedColor1, userSelectedColor2, userSelectedColor3, userSelectedColor4);
				UpdateUI_Visibility();
			}
		}

		private void coolingFactorConstantRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			decayUpDown.Enabled = coolingFactorConstantRadioButton.Checked;
		}

		private void coolingMapRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			InitializeCoolingMap();

			UpdateUI_Visibility();
		}

		private void coolingMapMinNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			coolingMapMaxNumericUpDown.Minimum = coolingMapMinNumericUpDown.Value;
			coolingMapMinNumericUpDown.Maximum = coolingMapMaxNumericUpDown.Value;
			InitializeCoolingMap();
		}

		private void coolingMapMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			coolingMapMaxNumericUpDown.Minimum = coolingMapMinNumericUpDown.Value;
			coolingMapMinNumericUpDown.Maximum = coolingMapMaxNumericUpDown.Value;
			InitializeCoolingMap();
		}

		private void coolingMapSmoothingNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			InitializeCoolingMap();
		}

		private void coolingMapDensityNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			InitializeCoolingMap();
		}

		private void customSeedRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			UpdateUI_Visibility();
		}

		private void coalSeedMinNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			coalSeedMaxNumericUpDown.Minimum = coalSeedMinNumericUpDown.Value;
			coalSeedMinNumericUpDown.Maximum = coalSeedMaxNumericUpDown.Value;
		}

		private void coalSeedMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			coalSeedMaxNumericUpDown.Minimum = coalSeedMinNumericUpDown.Value;
			coalSeedMinNumericUpDown.Maximum = coalSeedMaxNumericUpDown.Value;
		}

		private void draw32BitCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			// Switch to 32bit mode!
			m_fPallete = !draw32BitCheckBox.Checked;
#if false
			if (draw32BitCheckBox.Checked)
				this.Initialize32Bit();
			else
				this.Initialize8Bit();
#endif
		}

		private void rotateCoolingMapCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			// revert to non rotating
			if (!rotateCoolingMapCheckBox.Checked)
			{
				InitializeCoolingMap();
			}
			else if (rotatingCoolingMap == null)
			{
				UpdateRotatingCoolingMap();
			}
		}

		private void intensityUpDown_ValueChanged(object sender, EventArgs e)
		{
			RefreshTheFlamePalette();
		}

		private void realisticRenderMethodRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void intensityRenderMethodRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void flatRenderMethodRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void linearRenderMethodRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			RefreshTheFlamePalette();
			UpdateUI_Visibility();
		}

		private void copy4PointButton_Click(object sender, EventArgs e)
		{
			userSelectedColor1 = color1;
			userSelectedColor2 = color2;
			userSelectedColor3 = color3;
			userSelectedColor4 = color4;
			UpdateUI_Visibility();
			// JRDV: Maroon == Color.FromArgb(223, 38, 38)
		}
		private void fitScreenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			m_fUpdateFireDimensionsAfterNextFrame = true;
		}

		private void demoButton_Click(object sender, EventArgs e)
		{
			// Reset the Demo!
			resetButton_Click(sender, e);

			widthNumericUpDown.Value = 21;
			heightNumericUpDown.Value = 75;
			flatRenderMethodRadioButton.Checked = true;
			rotateCoolingMapCheckBox.Checked = false;
			coolingMapShiftCheckBox.Checked = false;
			coalSeedRangeCheckBox.Checked = false;
			coolingFactorOffRadioButton.Checked = true;
		}

		private void resetButton_Click(object sender, EventArgs e)
		{
			m_fBorg = false;
			m_fLighting = false;
			widthNumericUpDown.Value = 21;
			heightNumericUpDown.Value = 75;
			decayUpDown.Value = 2;
			flatRenderMethodRadioButton.Checked = true;
			rotateCoolingMapCheckBox.Checked = false;
			coolingMapShiftCheckBox.Checked = true;
			coolingMapRadioButton.Checked = true;
			coolingMapDensityNumericUpDown.Value = 40;
			coolingMapMinNumericUpDown.Value = 0; // temporarily ensure that setting max does not exceed the min
			coolingMapMaxNumericUpDown.Value = 13;
			coolingMapMinNumericUpDown.Value = 5;
			coolingMapSmoothingNumericUpDown.Value = 5;
			rotateCoolingMapCheckBox.Checked = true;

			customSeedRadioButton.Checked = true;
			coalSeedRangeCheckBox.Checked = false;
			coalSeedMinNumericUpDown.Value = 0; // temporarily ensure that setting max does not exceed the min
			coalSeedMaxNumericUpDown.Value = 255;
			coalSeedMinNumericUpDown.Value = 54;
			coalSeedPercentNumericUpDown.Value = 8;
			coalSeedRangeCheckBox.Checked = false;

			intensityUpDown.Value = 75;

			checkBox7.Checked = false;
			checkBox8.Checked = true;
			checkBox9.Checked = false;
			checkBox4.Checked = false;
			checkBox5.Checked = true;
			checkBox6.Checked = false;
			checkBox1.Checked = true;
			checkBox2.Checked = true;
			checkBox3.Checked = true;

			realisticRenderMethodRadioButton.Checked = true;
			m_fOverrideEnabledForSpecialFlame = true;
			InitializeBlueYellowFlame();
			RefreshTheFlamePalette();
		}

		private void presetCandleButton_Click(object sender, EventArgs e)
		{
			// Reset the Demo!
			resetButton_Click(sender, e);

			widthNumericUpDown.Value = 21;
			heightNumericUpDown.Value = 75;
			flatRenderMethodRadioButton.Checked = true;
			rotateCoolingMapCheckBox.Checked = false;
			coolingMapShiftCheckBox.Checked = true;
			coolingMapRadioButton.Checked = true;
			coolingMapDensityNumericUpDown.Value = 40;
			coolingMapMinNumericUpDown.Value = 0; // temporarily ensure that setting max does not exceed the min
			coolingMapMaxNumericUpDown.Value = 13;
			coolingMapMinNumericUpDown.Value = 5;
			coolingMapSmoothingNumericUpDown.Value = 5;
			rotateCoolingMapCheckBox.Checked = true;

			customSeedRadioButton.Checked = true;
			coalSeedRangeCheckBox.Checked = false;
			coalSeedMinNumericUpDown.Value = 0; // temporarily ensure that setting max does not exceed the min
			coalSeedMaxNumericUpDown.Value = 255;
			coalSeedMinNumericUpDown.Value = 200;
			coalSeedPercentNumericUpDown.Value = 8;
			coalSeedRangeCheckBox.Checked = false;

			intensityUpDown.Value = 75;

			checkBox7.Checked = false;
			checkBox8.Checked = true;
			checkBox9.Checked = false;
			checkBox4.Checked = false;
			checkBox5.Checked = true;
			checkBox6.Checked = false;
			checkBox1.Checked = true;
			checkBox2.Checked = true;
			checkBox3.Checked = true;

			realisticRenderMethodRadioButton.Checked = true;
			m_fOverrideEnabledForSpecialFlame = true;
			InitializeBlueYellowFlame();
			RefreshTheFlamePalette();
		}

		private void presetBonfireButton_Click(object sender, EventArgs e)
		{
			// Reset the Demo!
			resetButton_Click(sender, e);

			widthNumericUpDown.Value = 100;
			heightNumericUpDown.Value = 50;
			flatRenderMethodRadioButton.Checked = true;
			rotateCoolingMapCheckBox.Checked = false;
			coolingMapShiftCheckBox.Checked = true;
			coolingMapRadioButton.Checked = true;
			coolingMapDensityNumericUpDown.Value = 40;
			coolingMapMinNumericUpDown.Value = 0; // temporarily ensure that setting max does not exceed the min
			coolingMapMaxNumericUpDown.Value = 13;
			coolingMapMinNumericUpDown.Value = 5;
			coolingMapSmoothingNumericUpDown.Value = 0;
			rotateCoolingMapCheckBox.Checked = true;

			customSeedRadioButton.Checked = true;
			coalSeedRangeCheckBox.Checked = false;
			coalSeedMinNumericUpDown.Value = 0; // temporarily ensure that setting max does not exceed the min
			coalSeedMaxNumericUpDown.Value = 255;
			coalSeedMinNumericUpDown.Value = 54;
			coalSeedPercentNumericUpDown.Value = 8;
			coalSeedRangeCheckBox.Checked = true;

			intensityUpDown.Value = 75;

			checkBox7.Checked = false;
			checkBox8.Checked = false;
			checkBox9.Checked = false;
			checkBox4.Checked = false;
			checkBox5.Checked = true;
			checkBox6.Checked = false;
			checkBox1.Checked = true;
			checkBox2.Checked = true;
			checkBox3.Checked = true;

			blueYellowFlameButton_Click(sender, e);
			linearRenderMethodRadioButton.Checked = true;
		}

		private void presetLightningButton_Click(object sender, EventArgs e)
		{
			// Reset the Demo!
			resetButton_Click(sender, e);

			m_fLighting = true;
			widthNumericUpDown.Value = 132;
			heightNumericUpDown.Value = 200;
			coolingFactorConstantRadioButton.Checked = true;
			decayUpDown.Value = 27; // 39;

			checkBox7.Checked = false;
			checkBox8.Checked = true;
			checkBox9.Checked = false;
			checkBox4.Checked = true;
			checkBox5.Checked = true;
			checkBox6.Checked = true;
			checkBox1.Checked = false;
			checkBox2.Checked = true;
			checkBox3.Checked = false;

			SetSingleColorFlame(Color.FromArgb(255, 221, 221)); // BORG palette? Possibly a good place to start
			//realisticRenderMethodRadioButton.Checked = true;
			//SetSingleColorFlame(Color.White);
			//SetSingleColorFlame(Color.Black);
			InitializeMagentaFlame();
			SetSingleColorFlame(Color.FromArgb(128, 255, 255)); // BLUE (Zoe says looks more realistic)
			SetSingleColorFlame(Color.FromArgb(221, 221, 255)); // GOOD! Trying higher values now
			SetSingleColorFlame(Color.FromArgb(241, 241, 255));
			SetSingleColorFlame(Color.FromArgb(236, 236, 255));
			//SetSingleColorFlame(Color.FromArgb(251, 251, 255));
			//RefreshTheFlamePalette();
			realisticLightningButton_Click(null, null);
		}

#endregion

        private void presetAssimilateButton_Click(object sender, EventArgs e)
        {
			// Reset the Demo!
			resetButton_Click(sender, e);

			m_fLighting = true;
			m_fBorg = true;
			widthNumericUpDown.Value = 196;
			heightNumericUpDown.Value = 200;
			widthNumericUpDown.Value = 129;
			heightNumericUpDown.Value = 131;
			coolingFactorConstantRadioButton.Checked = true;
			//decayUpDown.Value = 7; // 7 worked better before drawing the full lines between steps
			decayUpDown.Value = 11;

			checkBox7.Checked = false;
			checkBox8.Checked = true;
			checkBox9.Checked = false;
			checkBox4.Checked = true;
			checkBox5.Checked = true;
			checkBox6.Checked = true;
			checkBox1.Checked = false;
			checkBox2.Checked = true;
			checkBox3.Checked = false;

//			InitializeGreenFlame(); // TODO: these should populate the custom color pickers!
//			InitializeRealisticDarkGreenFlame();
			SetSingleColorFlame(Color.FromArgb(255, 221, 221)); // BORG palette? Possibly a good place to start
			SetSingleColorFlame(Color.FromArgb(0, 255, 0)); // BORG palette? Possibly a good place to start


			linearRenderMethodRadioButton.Checked = true;
			color1 = Color.FromArgb(0, 0, 0);
			color2 = Color.FromArgb(0, 96, 0);
			color3 = Color.FromArgb(0, 255, 0);
			color4 = Color.FromArgb(255, 255, 255);

			color1 = Color.FromArgb(0, 0, 0);
			color2 = Color.FromArgb(0, 255, 0);
			color3 = Color.FromArgb(255, 255, 255);
			color4 = Color.FromArgb(255, 255, 255);

			// Really like these colrs. Create a new preset?
			//color1 = Color.FromArgb(0, 0, 0);
			//color2 = Color.FromArgb(0, 128, 0);
			//color3 = Color.FromArgb(128, 255, 128);
			//color4 = Color.FromArgb(255, 255, 255);

			InitializePalette(color1, color2, color3, color4);
			UpdateUI_Visibility();

//			thePalette[255] = Color.FromArgb(0, 255, 0);
		}

        private void presetRedBorgButton_Click(object sender, EventArgs e)
        {
			// Reset the Demo!
			resetButton_Click(sender, e);

			m_fLighting = true;
			m_fBorg = true;
			widthNumericUpDown.Value = 196;
			heightNumericUpDown.Value = 200;
			widthNumericUpDown.Value = 129;
			heightNumericUpDown.Value = 131;
			coolingFactorConstantRadioButton.Checked = true;
			decayUpDown.Value = 7;

			checkBox7.Checked = false;
			checkBox8.Checked = true;
			checkBox9.Checked = false;
			checkBox4.Checked = true;
			checkBox5.Checked = true;
			checkBox6.Checked = true;
			checkBox1.Checked = false;
			checkBox2.Checked = true;
			checkBox3.Checked = false;

			//			InitializeGreenFlame(); // TODO: these should populate the custom color pickers!
			//			InitializeRealisticDarkGreenFlame();
			SetSingleColorFlame(Color.FromArgb(255, 221, 221)); // BORG palette? Possibly a good place to start
			SetSingleColorFlame(Color.FromArgb(255, 0, 0)); // BORG palette? Possibly a good place to start


			linearRenderMethodRadioButton.Checked = true;

			color1 = Color.FromArgb(0, 0, 0);
			color2 = Color.FromArgb(255, 0, 0);
			color3 = Color.FromArgb(255, 192, 192);
			color4 = Color.FromArgb(255, 255, 255);
			InitializePalette(color1, color2, color3, color4);
			UpdateUI_Visibility();
		}
    }
}
