using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace FireUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRealisticPalette()
        {
            Color[] expectedPalette = { Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(4, 0, 0), Color.FromArgb(8, 0, 0), Color.FromArgb(12, 0, 0), Color.FromArgb(16, 0, 0), Color.FromArgb(21, 0, 0), Color.FromArgb(25, 0, 0), Color.FromArgb(29, 0, 0), Color.FromArgb(33, 0, 0), Color.FromArgb(38, 0, 0), Color.FromArgb(42, 0, 0), Color.FromArgb(46, 0, 0), Color.FromArgb(50, 0, 0), Color.FromArgb(55, 0, 0), Color.FromArgb(59, 0, 0), Color.FromArgb(63, 0, 0), Color.FromArgb(67, 0, 0), Color.FromArgb(71, 0, 0), Color.FromArgb(76, 0, 0), Color.FromArgb(80, 0, 0), Color.FromArgb(84, 0, 0), Color.FromArgb(88, 0, 0), Color.FromArgb(93, 0, 0), Color.FromArgb(97, 0, 0), Color.FromArgb(101, 0, 0), Color.FromArgb(105, 0, 0), Color.FromArgb(110, 0, 0), Color.FromArgb(114, 0, 0), Color.FromArgb(118, 0, 0), Color.FromArgb(122, 0, 0), Color.FromArgb(127, 0, 0), Color.FromArgb(139, 18, 0), Color.FromArgb(152, 37, 0), Color.FromArgb(165, 55, 0), Color.FromArgb(178, 74, 0), Color.FromArgb(191, 92, 0), Color.FromArgb(203, 111, 0), Color.FromArgb(216, 129, 0), Color.FromArgb(229, 148, 0), Color.FromArgb(242, 166, 0), Color.FromArgb(255, 185, 0), Color.FromArgb(255, 187, 0), Color.FromArgb(255, 190, 0), Color.FromArgb(255, 193, 0), Color.FromArgb(255, 196, 0), Color.FromArgb(255, 199, 0), Color.FromArgb(255, 201, 0), Color.FromArgb(255, 204, 0), Color.FromArgb(255, 207, 0), Color.FromArgb(255, 210, 0), Color.FromArgb(255, 213, 0), Color.FromArgb(255, 215, 0), Color.FromArgb(255, 218, 0), Color.FromArgb(255, 221, 0), Color.FromArgb(255, 224, 0), Color.FromArgb(255, 227, 0), Color.FromArgb(255, 229, 0), Color.FromArgb(255, 232, 0), Color.FromArgb(255, 235, 0), Color.FromArgb(255, 238, 0), Color.FromArgb(255, 241, 0), Color.FromArgb(255, 243, 0), Color.FromArgb(255, 246, 0), Color.FromArgb(255, 249, 0), Color.FromArgb(255, 252, 0), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 255, 19), Color.FromArgb(255, 255, 39), Color.FromArgb(255, 255, 58), Color.FromArgb(255, 255, 78), Color.FromArgb(255, 255, 98), Color.FromArgb(255, 255, 117), Color.FromArgb(255, 255, 137), Color.FromArgb(255, 255, 156), Color.FromArgb(255, 255, 176), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 197), Color.FromArgb(255, 255, 197), Color.FromArgb(255, 255, 198), Color.FromArgb(255, 255, 198), Color.FromArgb(255, 255, 198), Color.FromArgb(255, 255, 199), Color.FromArgb(255, 255, 199), Color.FromArgb(255, 255, 200), Color.FromArgb(255, 255, 200), Color.FromArgb(255, 255, 200), Color.FromArgb(255, 255, 201), Color.FromArgb(255, 255, 201), Color.FromArgb(255, 255, 202), Color.FromArgb(255, 255, 202), Color.FromArgb(255, 255, 202), Color.FromArgb(255, 255, 203), Color.FromArgb(255, 255, 203), Color.FromArgb(255, 255, 204), Color.FromArgb(255, 255, 204), Color.FromArgb(255, 255, 204), Color.FromArgb(255, 255, 205), Color.FromArgb(255, 255, 205), Color.FromArgb(255, 255, 206), Color.FromArgb(255, 255, 206), Color.FromArgb(255, 255, 206), Color.FromArgb(255, 255, 207), Color.FromArgb(255, 255, 207), Color.FromArgb(255, 255, 208), Color.FromArgb(255, 255, 208), Color.FromArgb(255, 255, 209), Color.FromArgb(255, 255, 209), Color.FromArgb(255, 255, 209), Color.FromArgb(255, 255, 210), Color.FromArgb(255, 255, 210), Color.FromArgb(255, 255, 211), Color.FromArgb(255, 255, 211), Color.FromArgb(255, 255, 211), Color.FromArgb(255, 255, 212), Color.FromArgb(255, 255, 212), Color.FromArgb(255, 255, 213), Color.FromArgb(255, 255, 213), Color.FromArgb(255, 255, 213), Color.FromArgb(255, 255, 214), Color.FromArgb(255, 255, 214), Color.FromArgb(255, 255, 215), Color.FromArgb(255, 255, 215), Color.FromArgb(255, 255, 215), Color.FromArgb(255, 255, 216), Color.FromArgb(255, 255, 216), Color.FromArgb(255, 255, 217), Color.FromArgb(255, 255, 217), Color.FromArgb(255, 255, 217), Color.FromArgb(255, 255, 218), Color.FromArgb(255, 255, 218), Color.FromArgb(255, 255, 219), Color.FromArgb(255, 255, 219), Color.FromArgb(255, 255, 220), Color.FromArgb(255, 255, 220), Color.FromArgb(255, 255, 220), Color.FromArgb(255, 255, 221), Color.FromArgb(255, 255, 221), Color.FromArgb(255, 255, 222), Color.FromArgb(255, 255, 222), Color.FromArgb(255, 255, 222), Color.FromArgb(255, 255, 223), Color.FromArgb(255, 255, 223), Color.FromArgb(255, 255, 224), Color.FromArgb(255, 255, 224), Color.FromArgb(255, 255, 224), Color.FromArgb(255, 255, 225), Color.FromArgb(255, 255, 225), Color.FromArgb(255, 255, 226), Color.FromArgb(255, 255, 226), Color.FromArgb(255, 255, 226), Color.FromArgb(255, 255, 227), Color.FromArgb(255, 255, 227), Color.FromArgb(255, 255, 228), Color.FromArgb(255, 255, 228), Color.FromArgb(255, 255, 228), Color.FromArgb(255, 255, 229), Color.FromArgb(255, 255, 229), Color.FromArgb(255, 255, 230), Color.FromArgb(255, 255, 230), Color.FromArgb(255, 255, 230), Color.FromArgb(255, 255, 231), Color.FromArgb(255, 255, 231), Color.FromArgb(255, 255, 232), Color.FromArgb(255, 255, 232), Color.FromArgb(255, 255, 233), Color.FromArgb(255, 255, 233), Color.FromArgb(255, 255, 233), Color.FromArgb(255, 255, 234), Color.FromArgb(255, 255, 234), Color.FromArgb(255, 255, 235), Color.FromArgb(255, 255, 235), Color.FromArgb(255, 255, 235), Color.FromArgb(255, 255, 236), Color.FromArgb(255, 255, 236), Color.FromArgb(255, 255, 237), Color.FromArgb(255, 255, 237), Color.FromArgb(255, 255, 237), Color.FromArgb(255, 255, 238), Color.FromArgb(255, 255, 238), Color.FromArgb(255, 255, 239), Color.FromArgb(255, 255, 239), Color.FromArgb(255, 255, 239), Color.FromArgb(255, 255, 240), Color.FromArgb(255, 255, 240), Color.FromArgb(255, 255, 241), Color.FromArgb(255, 255, 241), Color.FromArgb(255, 255, 241), Color.FromArgb(255, 255, 242), Color.FromArgb(255, 255, 242), Color.FromArgb(255, 255, 243), Color.FromArgb(255, 255, 243), Color.FromArgb(255, 255, 244), Color.FromArgb(255, 255, 244), Color.FromArgb(255, 255, 244), Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 246), Color.FromArgb(255, 255, 246), Color.FromArgb(255, 255, 246), Color.FromArgb(255, 255, 247), Color.FromArgb(255, 255, 247), Color.FromArgb(255, 255, 248), Color.FromArgb(255, 255, 248), Color.FromArgb(255, 255, 248), Color.FromArgb(255, 255, 249), Color.FromArgb(255, 255, 249), Color.FromArgb(255, 255, 250), Color.FromArgb(255, 255, 250), Color.FromArgb(255, 255, 250), Color.FromArgb(255, 255, 251), Color.FromArgb(255, 255, 251), Color.FromArgb(255, 255, 252), Color.FromArgb(255, 255, 252), Color.FromArgb(255, 255, 252), Color.FromArgb(255, 255, 253), Color.FromArgb(255, 255, 253), Color.FromArgb(255, 255, 254), Color.FromArgb(255, 255, 254), Color.FromArgb(255, 255, 255), Color.FromArgb(252, 252, 255), Color.FromArgb(249, 249, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(243, 243, 255), Color.FromArgb(240, 240, 255), Color.FromArgb(237, 237, 255), Color.FromArgb(234, 234, 255), Color.FromArgb(232, 232, 255), Color.FromArgb(229, 229, 255), Color.FromArgb(226, 226, 255), Color.FromArgb(223, 223, 255), Color.FromArgb(220, 220, 255), Color.FromArgb(217, 217, 255), Color.FromArgb(214, 214, 255), Color.FromArgb(212, 212, 255) };
            Color[] generatedPalette = FireDemo.PaletteGenerator.GetRealPalette();

            Assert.AreEqual(expectedPalette.Length, generatedPalette.Length);
            for (int ii = 0; ii < generatedPalette.Length; ++ii)
                Assert.AreEqual(expectedPalette[ii], generatedPalette[ii]);
        }

        [TestMethod]
        public void TestRotatingCoolingMapGeneration()
        {
            int width = 21;
            int height = 75;

            // This is the result of the old code generating the coolingMap with a Seed value of 1.
            // Want to ensure the refactored code produces the same result.
            int[] expectedCoolingMap = { 1, 1, 2, 2, 2, 3, 2, 3, 2, 2, 1, 1, 1, 1, 1, 2, 3, 3, 3, 2, 1, 1, 1, 2, 2, 3, 3, 3, 3, 3, 2, 1, 1, 1, 1, 2, 2, 3, 3, 2, 1, 1, 1, 1, 2, 3, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 3, 3, 2, 2, 2, 2, 2, 1, 1, 2, 2, 3, 2, 2, 1, 1, 1, 1, 2, 2, 2, 3, 2, 2, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 1, 1, 1, 1, 2, 2, 2, 3, 2, 2, 1, 1, 1, 1, 1, 1, 2, 3, 3, 4, 2, 1, 0, 1, 2, 3, 3, 3, 3, 3, 2, 1, 1, 1, 1, 1, 1, 2, 3, 4, 3, 2, 1, 0, 1, 2, 3, 3, 3, 3, 3, 2, 1, 1, 1, 2, 2, 2, 2, 3, 4, 3, 2, 1, 0, 1, 2, 3, 4, 3, 3, 2, 2, 2, 1, 2, 2, 2, 2, 2, 3, 3, 3, 2, 1, 1, 1, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 0, 1, 2, 3, 2, 2, 2, 2, 2, 3, 2, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 3, 3, 3, 2, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 2, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 2, 3, 2, 1, 2, 2, 3, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 2, 3, 3, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 1, 1, 1, 0, 0, 0, 1, 2, 3, 3, 3, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 1, 1, 2, 2, 3, 3, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 4, 4, 3, 2, 1, 1, 2, 2, 3, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 3, 2, 1, 1, 2, 3, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 3, 2, 1, 0, 1, 2, 2, 2, 1, 1, 1, 2, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 0, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 2, 1, 1, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 2, 2, 2, 3, 3, 2, 2, 1, 1, 2, 2, 3, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 4, 3, 2, 1, 0, 1, 1, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 2, 1, 0, 0, 2, 3, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 2, 3, 2, 1, 1, 0, 0, 1, 2, 4, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 3, 4, 4, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 3, 4, 4, 3, 2, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0, 0, 0, 2, 3, 4, 4, 3, 2, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 0, 1, 1, 0, 1, 1, 3, 3, 3, 3, 2, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 3, 3, 3, 3, 2, 2, 2, 2, 3, 2, 2, 1, 2, 2, 1, 1, 0, 0, 1, 1, 2, 3, 4, 4, 3, 3, 2, 2, 2, 3, 2, 2, 2, 2, 2, 1, 1, 0, 1, 2, 3, 2, 3, 4, 4, 3, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 1, 1, 2, 3, 3, 3, 2, 2, 3, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 2, 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 2, 2, 1, 1, 2, 1, 1, 2, 2, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 2, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 2, 2, 3, 2, 2, 2, 3, 3, 3, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 3, 3, 4, 3, 3, 2, 2, 2, 1, 1, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 0, 1, 1, 2, 2, 2, 2, 1, 2, 2, 2, 3, 4, 4, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 3, 3, 2, 2, 1, 1, 1, 1, 2, 3, 2, 2, 2, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 3, 2, 1, 0, 0, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 2, 2, 1, 1, 0, 1, 1, 2, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 2, 3, 2, 3, 3, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 2, 3, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 3, 3, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 3, 3, 3, 2, 2, 2, 2, 2, 3, 3, 3, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 3, 3, 2, 2, 1, 1, 1, 1, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 1, 1, 1, 0, 0, 0, 1, 1, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 1, 1, 2, 1, 1, 1, 2, 2, 1, 1, 1, 1, 2, 2, 1, 2, 2, 2, 1, 0, 0, 1, 1, 2, 1, 1, 1, 2, 1, 1, 0, 1, 1, 2, 1, 1, 1, 2, 2, 2, 1, 0, 1, 1, 2, 1, 1, 1, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 0, 1, 1, 2, 1, 2, 2, 1, 1, 1, 1, 2, 1, 1, 0, 1, 1, 2, 2, 2, 1, 0, 1, 1, 2, 2, 2, 1, 2, 1, 2, 1, 1, 1, 0, 0, 0, 1, 2, 3, 2, 1, 1, 1, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 1, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 3, 2, 1, 1, 1, 0, 0, 0, 1, 2, 2, 2, 2, 2, 3, 2, 1, 0, 0, 1, 2, 2, 2, 2, 1, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 2, 2, 1, 0, 0, 1, 1, 2, 2, 2, 2, 2, 1, 1, 0, 0, 1, 2, 2, 2, 3, 2, 1, 1, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 3, 2, 2, 2 };
            FireDemo.CoolingStrategyMap generatedCoolingMap = new FireDemo.CoolingStrategyMap(new Random(Seed: 1));
            generatedCoolingMap.SetMapParameters(w: 21, h: 75,
                bRotate: true, bShift: true,
                nDensity: 40, nMin: 5, nMax: 13, nSmoothing: 5);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Assert.AreEqual(expectedCoolingMap[x + (y * width)], generatedCoolingMap.at(x, y));
                }
            }

            int m_iFrame = 0;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    int iOriginal = x + (y * width);
                    int iCoolingOffset = (iOriginal + (m_iFrame * width)) % expectedCoolingMap.Length;
                    Assert.AreEqual(expectedCoolingMap[iCoolingOffset], generatedCoolingMap.at(x, y));
                }
            }

            generatedCoolingMap.progressOneFrame();
            ++m_iFrame;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    int iOriginal = x + (y * width);
                    int iCoolingOffset = (iOriginal + (m_iFrame * width)) % expectedCoolingMap.Length;
                    Assert.AreEqual(expectedCoolingMap[iCoolingOffset], generatedCoolingMap.at(x, y));
                }
            }

            generatedCoolingMap.progressOneFrame();
            ++m_iFrame;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    int iOriginal = x + (y * width);
                    int iCoolingOffset = (iOriginal + (m_iFrame * width)) % expectedCoolingMap.Length;
                    Assert.AreEqual(expectedCoolingMap[iCoolingOffset], generatedCoolingMap.at(x, y));
                }
            }
        }
    }
}
