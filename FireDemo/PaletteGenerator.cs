using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FireDemo
{
    public class PaletteGenerator
    {
        public static Color[] GetHardCodedFirePalette()
        {
            Color[] tempPalette = { Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(4, 0, 0), Color.FromArgb(8, 0, 0), Color.FromArgb(12, 0, 0), Color.FromArgb(16, 0, 0), Color.FromArgb(21, 0, 0), Color.FromArgb(25, 0, 0), Color.FromArgb(29, 0, 0), Color.FromArgb(33, 0, 0), Color.FromArgb(38, 0, 0), Color.FromArgb(42, 0, 0), Color.FromArgb(46, 0, 0), Color.FromArgb(50, 0, 0), Color.FromArgb(55, 0, 0), Color.FromArgb(59, 0, 0), Color.FromArgb(63, 0, 0), Color.FromArgb(67, 0, 0), Color.FromArgb(71, 0, 0), Color.FromArgb(76, 0, 0), Color.FromArgb(80, 0, 0), Color.FromArgb(84, 0, 0), Color.FromArgb(88, 0, 0), Color.FromArgb(93, 0, 0), Color.FromArgb(97, 0, 0), Color.FromArgb(101, 0, 0), Color.FromArgb(105, 0, 0), Color.FromArgb(110, 0, 0), Color.FromArgb(114, 0, 0), Color.FromArgb(118, 0, 0), Color.FromArgb(122, 0, 0), Color.FromArgb(127, 0, 0), Color.FromArgb(139, 18, 0), Color.FromArgb(152, 37, 0), Color.FromArgb(165, 55, 0), Color.FromArgb(178, 74, 0), Color.FromArgb(191, 92, 0), Color.FromArgb(203, 111, 0), Color.FromArgb(216, 129, 0), Color.FromArgb(229, 148, 0), Color.FromArgb(242, 166, 0), Color.FromArgb(255, 185, 0), Color.FromArgb(255, 187, 0), Color.FromArgb(255, 190, 0), Color.FromArgb(255, 193, 0), Color.FromArgb(255, 196, 0), Color.FromArgb(255, 199, 0), Color.FromArgb(255, 201, 0), Color.FromArgb(255, 204, 0), Color.FromArgb(255, 207, 0), Color.FromArgb(255, 210, 0), Color.FromArgb(255, 213, 0), Color.FromArgb(255, 215, 0), Color.FromArgb(255, 218, 0), Color.FromArgb(255, 221, 0), Color.FromArgb(255, 224, 0), Color.FromArgb(255, 227, 0), Color.FromArgb(255, 229, 0), Color.FromArgb(255, 232, 0), Color.FromArgb(255, 235, 0), Color.FromArgb(255, 238, 0), Color.FromArgb(255, 241, 0), Color.FromArgb(255, 243, 0), Color.FromArgb(255, 246, 0), Color.FromArgb(255, 249, 0), Color.FromArgb(255, 252, 0), Color.FromArgb(255, 255, 0), Color.FromArgb(255, 255, 19), Color.FromArgb(255, 255, 39), Color.FromArgb(255, 255, 58), Color.FromArgb(255, 255, 78), Color.FromArgb(255, 255, 98), Color.FromArgb(255, 255, 117), Color.FromArgb(255, 255, 137), Color.FromArgb(255, 255, 156), Color.FromArgb(255, 255, 176), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 196), Color.FromArgb(255, 255, 197), Color.FromArgb(255, 255, 197), Color.FromArgb(255, 255, 198), Color.FromArgb(255, 255, 198), Color.FromArgb(255, 255, 198), Color.FromArgb(255, 255, 199), Color.FromArgb(255, 255, 199), Color.FromArgb(255, 255, 200), Color.FromArgb(255, 255, 200), Color.FromArgb(255, 255, 200), Color.FromArgb(255, 255, 201), Color.FromArgb(255, 255, 201), Color.FromArgb(255, 255, 202), Color.FromArgb(255, 255, 202), Color.FromArgb(255, 255, 202), Color.FromArgb(255, 255, 203), Color.FromArgb(255, 255, 203), Color.FromArgb(255, 255, 204), Color.FromArgb(255, 255, 204), Color.FromArgb(255, 255, 204), Color.FromArgb(255, 255, 205), Color.FromArgb(255, 255, 205), Color.FromArgb(255, 255, 206), Color.FromArgb(255, 255, 206), Color.FromArgb(255, 255, 206), Color.FromArgb(255, 255, 207), Color.FromArgb(255, 255, 207), Color.FromArgb(255, 255, 208), Color.FromArgb(255, 255, 208), Color.FromArgb(255, 255, 209), Color.FromArgb(255, 255, 209), Color.FromArgb(255, 255, 209), Color.FromArgb(255, 255, 210), Color.FromArgb(255, 255, 210), Color.FromArgb(255, 255, 211), Color.FromArgb(255, 255, 211), Color.FromArgb(255, 255, 211), Color.FromArgb(255, 255, 212), Color.FromArgb(255, 255, 212), Color.FromArgb(255, 255, 213), Color.FromArgb(255, 255, 213), Color.FromArgb(255, 255, 213), Color.FromArgb(255, 255, 214), Color.FromArgb(255, 255, 214), Color.FromArgb(255, 255, 215), Color.FromArgb(255, 255, 215), Color.FromArgb(255, 255, 215), Color.FromArgb(255, 255, 216), Color.FromArgb(255, 255, 216), Color.FromArgb(255, 255, 217), Color.FromArgb(255, 255, 217), Color.FromArgb(255, 255, 217), Color.FromArgb(255, 255, 218), Color.FromArgb(255, 255, 218), Color.FromArgb(255, 255, 219), Color.FromArgb(255, 255, 219), Color.FromArgb(255, 255, 220), Color.FromArgb(255, 255, 220), Color.FromArgb(255, 255, 220), Color.FromArgb(255, 255, 221), Color.FromArgb(255, 255, 221), Color.FromArgb(255, 255, 222), Color.FromArgb(255, 255, 222), Color.FromArgb(255, 255, 222), Color.FromArgb(255, 255, 223), Color.FromArgb(255, 255, 223), Color.FromArgb(255, 255, 224), Color.FromArgb(255, 255, 224), Color.FromArgb(255, 255, 224), Color.FromArgb(255, 255, 225), Color.FromArgb(255, 255, 225), Color.FromArgb(255, 255, 226), Color.FromArgb(255, 255, 226), Color.FromArgb(255, 255, 226), Color.FromArgb(255, 255, 227), Color.FromArgb(255, 255, 227), Color.FromArgb(255, 255, 228), Color.FromArgb(255, 255, 228), Color.FromArgb(255, 255, 228), Color.FromArgb(255, 255, 229), Color.FromArgb(255, 255, 229), Color.FromArgb(255, 255, 230), Color.FromArgb(255, 255, 230), Color.FromArgb(255, 255, 230), Color.FromArgb(255, 255, 231), Color.FromArgb(255, 255, 231), Color.FromArgb(255, 255, 232), Color.FromArgb(255, 255, 232), Color.FromArgb(255, 255, 233), Color.FromArgb(255, 255, 233), Color.FromArgb(255, 255, 233), Color.FromArgb(255, 255, 234), Color.FromArgb(255, 255, 234), Color.FromArgb(255, 255, 235), Color.FromArgb(255, 255, 235), Color.FromArgb(255, 255, 235), Color.FromArgb(255, 255, 236), Color.FromArgb(255, 255, 236), Color.FromArgb(255, 255, 237), Color.FromArgb(255, 255, 237), Color.FromArgb(255, 255, 237), Color.FromArgb(255, 255, 238), Color.FromArgb(255, 255, 238), Color.FromArgb(255, 255, 239), Color.FromArgb(255, 255, 239), Color.FromArgb(255, 255, 239), Color.FromArgb(255, 255, 240), Color.FromArgb(255, 255, 240), Color.FromArgb(255, 255, 241), Color.FromArgb(255, 255, 241), Color.FromArgb(255, 255, 241), Color.FromArgb(255, 255, 242), Color.FromArgb(255, 255, 242), Color.FromArgb(255, 255, 243), Color.FromArgb(255, 255, 243), Color.FromArgb(255, 255, 244), Color.FromArgb(255, 255, 244), Color.FromArgb(255, 255, 244), Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 246), Color.FromArgb(255, 255, 246), Color.FromArgb(255, 255, 246), Color.FromArgb(255, 255, 247), Color.FromArgb(255, 255, 247), Color.FromArgb(255, 255, 248), Color.FromArgb(255, 255, 248), Color.FromArgb(255, 255, 248), Color.FromArgb(255, 255, 249), Color.FromArgb(255, 255, 249), Color.FromArgb(255, 255, 250), Color.FromArgb(255, 255, 250), Color.FromArgb(255, 255, 250), Color.FromArgb(255, 255, 251), Color.FromArgb(255, 255, 251), Color.FromArgb(255, 255, 252), Color.FromArgb(255, 255, 252), Color.FromArgb(255, 255, 252), Color.FromArgb(255, 255, 253), Color.FromArgb(255, 255, 253), Color.FromArgb(255, 255, 254), Color.FromArgb(255, 255, 254), Color.FromArgb(255, 255, 255), Color.FromArgb(252, 252, 255), Color.FromArgb(249, 249, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(243, 243, 255), Color.FromArgb(240, 240, 255), Color.FromArgb(237, 237, 255), Color.FromArgb(234, 234, 255), Color.FromArgb(232, 232, 255), Color.FromArgb(229, 229, 255), Color.FromArgb(226, 226, 255), Color.FromArgb(223, 223, 255), Color.FromArgb(220, 220, 255), Color.FromArgb(217, 217, 255), Color.FromArgb(214, 214, 255), Color.FromArgb(212, 212, 255) };
            return tempPalette;
        }

        public static Color[] GetHardCodedLightningPalette()
        {
            Color[] theLightningPalette = { Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), Color.FromArgb(6, 4, 4), Color.FromArgb(13, 8, 8), Color.FromArgb(20, 12, 12), Color.FromArgb(27, 16, 16), Color.FromArgb(33, 20, 21), Color.FromArgb(41, 24, 25), Color.FromArgb(47, 28, 29), Color.FromArgb(54, 32, 33), Color.FromArgb(61, 37, 38), Color.FromArgb(67, 41, 42), Color.FromArgb(74, 45, 46), Color.FromArgb(81, 49, 50), Color.FromArgb(87, 53, 55), Color.FromArgb(94, 57, 59), Color.FromArgb(101, 61, 63), Color.FromArgb(108, 65, 67), Color.FromArgb(114, 69, 71), Color.FromArgb(122, 73, 76), Color.FromArgb(128, 77, 80), Color.FromArgb(135, 81, 84), Color.FromArgb(142, 85, 88), Color.FromArgb(148, 90, 93), Color.FromArgb(155, 94, 97), Color.FromArgb(162, 98, 101), Color.FromArgb(169, 102, 105), Color.FromArgb(175, 106, 110), Color.FromArgb(182, 110, 114), Color.FromArgb(189, 114, 118), Color.FromArgb(195, 118, 122), Color.FromArgb(203, 123, 127), Color.FromArgb(203, 135, 139), Color.FromArgb(203, 147, 152), Color.FromArgb(203, 160, 165), Color.FromArgb(203, 172, 178), Color.FromArgb(203, 185, 191), Color.FromArgb(203, 196, 203), Color.FromArgb(203, 209, 216), Color.FromArgb(203, 221, 229), Color.FromArgb(203, 234, 242), Color.FromArgb(203, 246, 255), Color.FromArgb(204, 246, 255), Color.FromArgb(207, 246, 255), Color.FromArgb(209, 246, 255), Color.FromArgb(211, 246, 255), Color.FromArgb(213, 246, 255), Color.FromArgb(215, 246, 255), Color.FromArgb(217, 246, 255), Color.FromArgb(219, 246, 255), Color.FromArgb(221, 246, 255), Color.FromArgb(224, 246, 255), Color.FromArgb(225, 246, 255), Color.FromArgb(227, 246, 255), Color.FromArgb(230, 246, 255), Color.FromArgb(232, 246, 255), Color.FromArgb(234, 246, 255), Color.FromArgb(236, 246, 255), Color.FromArgb(238, 246, 255), Color.FromArgb(240, 246, 255), Color.FromArgb(242, 246, 255), Color.FromArgb(245, 246, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(248, 246, 255), Color.FromArgb(251, 246, 255), Color.FromArgb(253, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(255, 246, 255), Color.FromArgb(254, 246, 255), Color.FromArgb(254, 246, 255), Color.FromArgb(254, 246, 255), Color.FromArgb(254, 246, 255), Color.FromArgb(253, 246, 255), Color.FromArgb(253, 246, 255), Color.FromArgb(253, 246, 255), Color.FromArgb(253, 246, 255), Color.FromArgb(253, 246, 255), Color.FromArgb(252, 246, 255), Color.FromArgb(252, 246, 255), Color.FromArgb(252, 246, 255), Color.FromArgb(252, 246, 255), Color.FromArgb(251, 246, 255), Color.FromArgb(251, 246, 255), Color.FromArgb(251, 246, 255), Color.FromArgb(251, 246, 255), Color.FromArgb(251, 246, 255), Color.FromArgb(250, 246, 255), Color.FromArgb(250, 246, 255), Color.FromArgb(249, 246, 255), Color.FromArgb(249, 246, 255), Color.FromArgb(249, 246, 255), Color.FromArgb(249, 246, 255), Color.FromArgb(249, 246, 255), Color.FromArgb(248, 246, 255), Color.FromArgb(248, 246, 255), Color.FromArgb(248, 246, 255), Color.FromArgb(248, 246, 255), Color.FromArgb(247, 246, 255), Color.FromArgb(247, 246, 255), Color.FromArgb(247, 246, 255), Color.FromArgb(247, 246, 255), Color.FromArgb(247, 246, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(246, 246, 255), Color.FromArgb(245, 246, 255), Color.FromArgb(245, 246, 255), Color.FromArgb(245, 246, 255), Color.FromArgb(245, 246, 255), Color.FromArgb(244, 246, 255), Color.FromArgb(244, 246, 255), Color.FromArgb(244, 246, 255), Color.FromArgb(243, 246, 255), Color.FromArgb(243, 246, 255), Color.FromArgb(243, 246, 255), Color.FromArgb(243, 246, 255), Color.FromArgb(242, 246, 255), Color.FromArgb(242, 246, 255), Color.FromArgb(242, 246, 255), Color.FromArgb(242, 246, 255), Color.FromArgb(242, 246, 255), Color.FromArgb(241, 246, 255), Color.FromArgb(241, 246, 255), Color.FromArgb(241, 246, 255), Color.FromArgb(241, 246, 255), Color.FromArgb(240, 246, 255), Color.FromArgb(240, 246, 255), Color.FromArgb(240, 246, 255), Color.FromArgb(240, 246, 255), Color.FromArgb(240, 246, 255), Color.FromArgb(239, 246, 255), Color.FromArgb(239, 246, 255), Color.FromArgb(238, 246, 255), Color.FromArgb(238, 246, 255), Color.FromArgb(238, 246, 255), Color.FromArgb(238, 246, 255), Color.FromArgb(238, 246, 255), Color.FromArgb(237, 246, 255), Color.FromArgb(237, 246, 255), Color.FromArgb(237, 246, 255), Color.FromArgb(237, 246, 255), Color.FromArgb(236, 246, 255), Color.FromArgb(236, 246, 255), Color.FromArgb(236, 246, 255), Color.FromArgb(236, 246, 255), Color.FromArgb(236, 246, 255), Color.FromArgb(235, 246, 255), Color.FromArgb(235, 246, 255), Color.FromArgb(235, 246, 255), Color.FromArgb(235, 246, 255), Color.FromArgb(235, 246, 255), Color.FromArgb(234, 246, 255), Color.FromArgb(234, 246, 255), Color.FromArgb(234, 246, 255), Color.FromArgb(234, 246, 255), Color.FromArgb(233, 246, 255), Color.FromArgb(233, 246, 255), Color.FromArgb(233, 246, 255), Color.FromArgb(232, 246, 255), Color.FromArgb(232, 246, 255), Color.FromArgb(232, 246, 255), Color.FromArgb(232, 246, 255), Color.FromArgb(231, 246, 255), Color.FromArgb(231, 246, 255), Color.FromArgb(231, 246, 255), Color.FromArgb(231, 246, 255), Color.FromArgb(231, 246, 255), Color.FromArgb(230, 246, 255), Color.FromArgb(230, 246, 255), Color.FromArgb(230, 246, 255), Color.FromArgb(230, 246, 255), Color.FromArgb(229, 246, 255), Color.FromArgb(229, 246, 255), Color.FromArgb(229, 246, 255), Color.FromArgb(229, 246, 255), Color.FromArgb(229, 246, 255), Color.FromArgb(228, 246, 255), Color.FromArgb(228, 246, 255), Color.FromArgb(227, 246, 255), Color.FromArgb(227, 246, 255), Color.FromArgb(227, 246, 255), Color.FromArgb(227, 246, 255), Color.FromArgb(227, 246, 255), Color.FromArgb(226, 246, 255), Color.FromArgb(226, 246, 255), Color.FromArgb(226, 246, 255), Color.FromArgb(226, 246, 255), Color.FromArgb(225, 246, 255), Color.FromArgb(225, 246, 255), Color.FromArgb(225, 246, 255), Color.FromArgb(225, 246, 255), Color.FromArgb(225, 246, 255), Color.FromArgb(224, 246, 255), Color.FromArgb(224, 246, 255), Color.FromArgb(224, 246, 255), Color.FromArgb(224, 246, 255), Color.FromArgb(224, 246, 255), Color.FromArgb(223, 246, 255), Color.FromArgb(223, 246, 255), Color.FromArgb(223, 246, 255), Color.FromArgb(223, 246, 255), Color.FromArgb(222, 246, 255), Color.FromArgb(222, 246, 255), Color.FromArgb(222, 246, 255), Color.FromArgb(221, 246, 255), Color.FromArgb(221, 246, 255), Color.FromArgb(221, 246, 255), Color.FromArgb(221, 246, 255), Color.FromArgb(220, 246, 255), Color.FromArgb(220, 246, 255), Color.FromArgb(220, 246, 255), Color.FromArgb(219, 246, 255), Color.FromArgb(218, 246, 255), Color.FromArgb(216, 246, 255), Color.FromArgb(215, 246, 255), Color.FromArgb(214, 246, 255), Color.FromArgb(213, 246, 255), Color.FromArgb(212, 246, 255), Color.FromArgb(210, 246, 255), Color.FromArgb(209, 246, 255), Color.FromArgb(208, 246, 255), Color.FromArgb(207, 246, 255), Color.FromArgb(206, 246, 255), Color.FromArgb(205, 246, 255), Color.FromArgb(204, 246, 255), Color.FromArgb(203, 246, 255) };
            return theLightningPalette;
        }

        public static Color[] GetHardCodedBorgPalette()
        {
            Color[] theBorgPalette = { Color.FromArgb(0, 0, 0), Color.FromArgb(0, 3, 0), Color.FromArgb(0, 6, 0), Color.FromArgb(0, 9, 0), Color.FromArgb(0, 12, 0), Color.FromArgb(0, 15, 0), Color.FromArgb(0, 18, 0), Color.FromArgb(0, 21, 0), Color.FromArgb(0, 24, 0), Color.FromArgb(0, 27, 0), Color.FromArgb(0, 30, 0), Color.FromArgb(0, 33, 0), Color.FromArgb(0, 36, 0), Color.FromArgb(0, 39, 0), Color.FromArgb(0, 42, 0), Color.FromArgb(0, 45, 0), Color.FromArgb(0, 48, 0), Color.FromArgb(0, 51, 0), Color.FromArgb(0, 54, 0), Color.FromArgb(0, 57, 0), Color.FromArgb(0, 60, 0), Color.FromArgb(0, 63, 0), Color.FromArgb(0, 66, 0), Color.FromArgb(0, 69, 0), Color.FromArgb(0, 72, 0), Color.FromArgb(0, 75, 0), Color.FromArgb(0, 78, 0), Color.FromArgb(0, 81, 0), Color.FromArgb(0, 84, 0), Color.FromArgb(0, 87, 0), Color.FromArgb(0, 90, 0), Color.FromArgb(0, 93, 0), Color.FromArgb(0, 96, 0), Color.FromArgb(0, 99, 0), Color.FromArgb(0, 102, 0), Color.FromArgb(0, 105, 0), Color.FromArgb(0, 108, 0), Color.FromArgb(0, 111, 0), Color.FromArgb(0, 114, 0), Color.FromArgb(0, 117, 0), Color.FromArgb(0, 120, 0), Color.FromArgb(0, 123, 0), Color.FromArgb(0, 126, 0), Color.FromArgb(0, 129, 0), Color.FromArgb(0, 132, 0), Color.FromArgb(0, 135, 0), Color.FromArgb(0, 138, 0), Color.FromArgb(0, 141, 0), Color.FromArgb(0, 144, 0), Color.FromArgb(0, 147, 0), Color.FromArgb(0, 150, 0), Color.FromArgb(0, 153, 0), Color.FromArgb(0, 156, 0), Color.FromArgb(0, 159, 0), Color.FromArgb(0, 162, 0), Color.FromArgb(0, 165, 0), Color.FromArgb(0, 168, 0), Color.FromArgb(0, 171, 0), Color.FromArgb(0, 174, 0), Color.FromArgb(0, 177, 0), Color.FromArgb(0, 180, 0), Color.FromArgb(0, 183, 0), Color.FromArgb(0, 186, 0), Color.FromArgb(0, 189, 0), Color.FromArgb(0, 192, 0), Color.FromArgb(0, 195, 0), Color.FromArgb(0, 198, 0), Color.FromArgb(0, 201, 0), Color.FromArgb(0, 204, 0), Color.FromArgb(0, 207, 0), Color.FromArgb(0, 210, 0), Color.FromArgb(0, 213, 0), Color.FromArgb(0, 216, 0), Color.FromArgb(0, 219, 0), Color.FromArgb(0, 222, 0), Color.FromArgb(0, 225, 0), Color.FromArgb(0, 228, 0), Color.FromArgb(0, 231, 0), Color.FromArgb(0, 234, 0), Color.FromArgb(0, 237, 0), Color.FromArgb(0, 240, 0), Color.FromArgb(0, 243, 0), Color.FromArgb(0, 246, 0), Color.FromArgb(0, 249, 0), Color.FromArgb(0, 252, 0), Color.FromArgb(0, 255, 0), Color.FromArgb(3, 255, 3), Color.FromArgb(6, 255, 6), Color.FromArgb(9, 255, 9), Color.FromArgb(12, 255, 12), Color.FromArgb(15, 255, 15), Color.FromArgb(18, 255, 18), Color.FromArgb(21, 255, 21), Color.FromArgb(24, 255, 24), Color.FromArgb(27, 255, 27), Color.FromArgb(30, 255, 30), Color.FromArgb(33, 255, 33), Color.FromArgb(36, 255, 36), Color.FromArgb(39, 255, 39), Color.FromArgb(42, 255, 42), Color.FromArgb(45, 255, 45), Color.FromArgb(48, 255, 48), Color.FromArgb(51, 255, 51), Color.FromArgb(54, 255, 54), Color.FromArgb(57, 255, 57), Color.FromArgb(60, 255, 60), Color.FromArgb(63, 255, 63), Color.FromArgb(66, 255, 66), Color.FromArgb(69, 255, 69), Color.FromArgb(72, 255, 72), Color.FromArgb(75, 255, 75), Color.FromArgb(78, 255, 78), Color.FromArgb(81, 255, 81), Color.FromArgb(84, 255, 84), Color.FromArgb(87, 255, 87), Color.FromArgb(90, 255, 90), Color.FromArgb(93, 255, 93), Color.FromArgb(96, 255, 96), Color.FromArgb(99, 255, 99), Color.FromArgb(102, 255, 102), Color.FromArgb(105, 255, 105), Color.FromArgb(108, 255, 108), Color.FromArgb(111, 255, 111), Color.FromArgb(114, 255, 114), Color.FromArgb(117, 255, 117), Color.FromArgb(120, 255, 120), Color.FromArgb(123, 255, 123), Color.FromArgb(126, 255, 126), Color.FromArgb(129, 255, 129), Color.FromArgb(132, 255, 132), Color.FromArgb(135, 255, 135), Color.FromArgb(138, 255, 138), Color.FromArgb(141, 255, 141), Color.FromArgb(144, 255, 144), Color.FromArgb(147, 255, 147), Color.FromArgb(150, 255, 150), Color.FromArgb(153, 255, 153), Color.FromArgb(156, 255, 156), Color.FromArgb(159, 255, 159), Color.FromArgb(162, 255, 162), Color.FromArgb(165, 255, 165), Color.FromArgb(168, 255, 168), Color.FromArgb(171, 255, 171), Color.FromArgb(174, 255, 174), Color.FromArgb(177, 255, 177), Color.FromArgb(180, 255, 180), Color.FromArgb(183, 255, 183), Color.FromArgb(186, 255, 186), Color.FromArgb(189, 255, 189), Color.FromArgb(192, 255, 192), Color.FromArgb(195, 255, 195), Color.FromArgb(198, 255, 198), Color.FromArgb(201, 255, 201), Color.FromArgb(204, 255, 204), Color.FromArgb(207, 255, 207), Color.FromArgb(210, 255, 210), Color.FromArgb(213, 255, 213), Color.FromArgb(216, 255, 216), Color.FromArgb(219, 255, 219), Color.FromArgb(222, 255, 222), Color.FromArgb(225, 255, 225), Color.FromArgb(228, 255, 228), Color.FromArgb(231, 255, 231), Color.FromArgb(234, 255, 234), Color.FromArgb(237, 255, 237), Color.FromArgb(240, 255, 240), Color.FromArgb(243, 255, 243), Color.FromArgb(246, 255, 246), Color.FromArgb(249, 255, 249), Color.FromArgb(252, 255, 252), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255) };
            return theBorgPalette;
        }

        #region Palette Helper/Setters

        // range INCLUDES start, and also INCLUDES end
        // Used by both the old 4 point flame palette, and new more realistic flame palette curve code
        protected static void SetPaletteRangeInclusive(Color[] thePalette, int start, int end, Color c1, Color c2)
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

        // For the realistic, hand tuned palettes, AND the generalized flame curve function calculation
        static protected void SetPaletteFromColorRange(Color[] thePalette, ColorRange[] colorRange)
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
                SetPaletteRangeInclusive(thePalette, rangeStart, rangeEnd, colorRange[i].color, colorRange[i + 1].color);
            }
        }

        #endregion
    }

    /// <summary>
    /// Very simplistic, linear gradient from specified color to black.
    /// </summary>
    public class PalFlatPalette_OriginalSqueak : PaletteGenerator
    {
        public static Color[] New(Color color)
        {
            Color[] thePalette = new Color[256];
            SetFlatPalette(thePalette, color);
            return thePalette;
        }

        static private void SetFlatPalette(Color[] thePalette, Color color)
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
    }

    /// <summary>
    /// Very simplistic, linear gradient from specified color to black.
    /// </summary>
    public class PalFlatPalette : PaletteGenerator
    {
        static public Color[] New(Color color)
        {
            Color[] thePalette = new Color[256];
            SetFlatPalette(thePalette, color);
            return thePalette;
        }

        static private void SetFlatPalette(Color[] thePalette, Color color)
        {
            ColorRange[] colorRangeFlat = {
                new ColorRange(Color.Black, -1),
                new ColorRange(color, 0),
            };

            SetPaletteFromColorRange(thePalette, colorRangeFlat);
        }
    }

    /// <summary>
    /// Simplistic, linear gradient between 4 specified colors.
    /// </summary>
    public class PalFourPointLinear_OLD : PaletteGenerator
    {
        public static Color[] New(Color c1, Color c2, Color c3, Color c4)
        {
            Color[] thePalette = new Color[256];
            InitializePalette(thePalette, c1, c2, c3, c4);
            return thePalette;
        }

        private static void InitializePalette(Color[] thePalette, Color c1, Color c2, Color c3, Color c4)
        {
            // Including the final value.
            SetPaletteRangeInclusive(thePalette, 0, 85, c1, c2); // 85 in this range
            SetPaletteRangeInclusive(thePalette, 85, 170, c2, c3); // 86 in this range
            SetPaletteRangeInclusive(thePalette, 170, 255, c3, c4); // 85 in this range
        }

    }

    /// <summary>
    /// Simplistic, linear gradient between 4 specified colors.
    /// </summary>
    public class PalFourPointLinear : PaletteGenerator
    {
        public static Color[] New(Color c1, Color c2, Color c3, Color c4)
        {
            Color[] thePalette = new Color[256];
            Fill(thePalette, c1, c2, c3, c4);
            return thePalette;
        }
        public static void Fill(Color[] thePalette, Color c1, Color c2, Color c3, Color c4)
        {
            InitializePalette(thePalette, c1, c2, c3, c4);
        }

        private static void InitializePalette(Color[] thePalette, Color c1, Color c2, Color c3, Color c4)
        {
            ColorRange[] colorRange4PointLinear= {
                new ColorRange(c1, 85),
                new ColorRange(c2, 85),
                new ColorRange(c3, 85),
                new ColorRange(c4, 0),
            };
            SetPaletteFromColorRange(thePalette, colorRange4PointLinear);
        }
    }
    // TODO: JRDV: Investigate implementing Plasma palette with the Lightning palette instead of FourPointLinear
    public class PalPlasma : PalFourPointLinear
    {
        public static Color[] New(Color color)
        {
            Color[] thePalette = new Color[256];
            Fill(thePalette, color);
            return thePalette;
        }

        public static Color[] NewRaw(Color c2)
        {
            return NewRaw(c2, Color.White);
        }

        public static Color[] NewRaw(Color c2, Color c3)
        {
            Color[] thePalette = new Color[256];
            PalFourPointLinear.Fill(thePalette, Color.Black, c2, c3, Color.White);
            return thePalette;
        }

        public static void Fill(Color[] thePalette, Color color)
        {
            // Tweak the basic colors to what I think looks best
            if (color == Color.Red)
            {
                Color c3 = Color.FromArgb(red: 255, green: 192, blue: 192); // slightly darken the mid-range
                PalFourPointLinear.Fill(thePalette, Color.Black, color, c3, Color.White);
            }
            else if (color == Color.Orange)
            {
                Color c2 = Color.FromArgb(red: 255, green: 128, blue: 0); // darken the base color
                Color c3 = Color.FromArgb(red: 255, green: 224, blue: 192); // darken the mid-range
                PalFourPointLinear.Fill(thePalette, Color.Black, c2, c3, Color.White);
            }
            //else if (color == Color.Yellow) // Looks good
            else if (color == Color.Green)
            {
                Color c2 = Color.FromArgb(red: 0, green: 255, blue: 0); // brighten the base color to Pure Green
                PalFourPointLinear.Fill(thePalette, Color.Black, c2, Color.White, Color.White);
            }
            else if (color == Color.LightBlue)
            {
                //Color c2 = Color.FromArgb(red: 0, green: 128, blue: 255);
                //Color c3 = Color.FromArgb(red: 192, green: 192, blue: 255); // I liked this, but it was too dark
                Color c2 = Color.FromArgb(red: 0, green: 128, blue: 255); // significantly deepen the base color
                Color c3 = Color.FromArgb(red: 240, green: 240, blue: 255); // slightly darken the mid-range
                PalFourPointLinear.Fill(thePalette, Color.Black, c2, c3, Color.White);
            }
            else if (color == Color.Blue)
            {
                Color c2 = Color.FromArgb(red: 0, green: 80, blue: 255); // significantly deepen the base color
                Color c3 = Color.FromArgb(red: 240, green: 240, blue: 255); // slightly darken the mid-range
                PalFourPointLinear.Fill(thePalette, Color.Black, c2, c3, Color.White);
            }
            else if (color == Color.DarkBlue)
            {
                // Originally thought it was halfway between Blue and this LightBlue
                //Color c2 = Color.FromArgb(red: 0, green: 0, blue: 255);
                //Color c3 = Color.FromArgb(red: 255, green: 255, blue: 255);
                // AND
                //Color c2 = Color.FromArgb(red: 0, green: 128, blue: 255);
                //Color c3 = Color.FromArgb(red: 192, green: 192, blue: 255);
                // 
                // Which produced FavoriteBlue1:
                //Color c2 = Color.FromArgb(red: 0, green: 64, blue: 255);
                //Color c3 = Color.FromArgb(red: 224, green: 224, blue: 255);
                // BUUUUT it wasn't quite bright enough, or deep enough? So need to brigten c3, and deepen c2
                // Somewhere between 
                //Color c2 = Color.FromArgb(red: 0, green: 0, blue: 255);
                //Color c3 = Color.FromArgb(red: 255, green: 255, blue: 255);
                // AND
                //Color c2 = Color.FromArgb(red: 0, green: 64, blue: 255);
                //Color c3 = Color.FromArgb(red: 224, green: 224, blue: 255);
                // 
                // BUUUUUUT maybe I need to define a better "curve" through the color space?
                // TODO: JRDV: Explore using the lighting palette for plasma,
                // which first requires generalizing the lighting palette algorithm.
                Color c2 = Color.FromArgb(red: 0, green: 32, blue: 255); // slightly brighten the base color
                Color c3 = Color.FromArgb(red: 240, green: 240, blue: 255); // slightly darken the mid-range
                PalFourPointLinear.Fill(thePalette, Color.Black, c2, c3, Color.White);
            }
            //else if (color == Color.DarkViolet) // Looks good (slightly better than DarkMagenta)
            //else if (color == Color.Violet) // I like Magenta better, but we have a named color for that. No reason to override Violet
            //{
            //    Color c2 = Color.FromArgb(red: 255, green: 0, blue: 255); // This is Magenta, just use Color.Magenta;
            //    PalFourPointLinear.Fill(thePalette, Color.Black, c2, Color.White, Color.White);
            //}
            else
                PalFourPointLinear.Fill(thePalette, Color.Black, color, Color.White, Color.White);
        }
    }

    /// <summary>
    /// Generalized function for calculating a curved gradient, based on a single color.
    /// </summary>
    public class PalRealisticFlameCurve : PaletteGenerator
    {
        static public Color[] New(Color color)
        {
            Color[] thePalette = new Color[256];
            if (color == Color.White)
                InitializeWhiteFlameCurve(thePalette);
            else if (color == Color.Black)
                InitializeBlackFlameCurve(thePalette);
            else
                InitializeRealisticFlameCurve(thePalette, color);
            return thePalette;
        }
        static private void InitializeRealisticFlameCurve(Color[] thePalette, Color target, float fIntensity = 1.0f)
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

            SetPaletteFromColorRange(thePalette, colorRangeGenerated);
        }


        private static void InitializeWhiteFlameCurve(Color[] thePalette)
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

            //	InitializeRealisticPalette();
            //	InitializeRealisticFlame
            // OH! I called this to press the "Realistic" button, which will EITHER use 4 Point Linear
            // or the InitializeRealisticFlame code depending on the selected radio button.
            //throw new Exception("need to replicate blueYellowFlameButton_Click(null, null);");
            Color[] palReal = PalRealisticFire.New();

            for (int i = 0; i < palReal.Length; ++i)
            {
                Color c0 = palReal[i];
                int cAvg = (c0.R + c0.G + c0.B) / 3;
                thePalette[i] = Color.FromArgb(cAvg, cAvg, cAvg);
            }
        }

        private static void InitializeBlackFlameCurve(Color[] thePalette)
        {
            InitializeWhiteFlameCurve(thePalette);

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


        #region Generalized Flame Curve Calculation

        static void GetColorForThreeHelper_Old(ref int primary1, ref int primary2, ref int secondary, float factor1, int factor2, float factor3)
        {
            primary1 = (int)(primary1 * factor1);
            primary2 = (int)(primary2 * factor1);
            //secondary = secondary * ((float)factor2) ; // TODO: JRDV: this should be a multiplication? Yes, try it next.  The color curve is currently off when the primary color is not 255
            secondary += factor2;
            if (secondary > 255)
                secondary = 255;
        }

        static void GetColorForThree_Old(ref int R, ref int G, ref int B, float factor1, int factor2, float factor3)
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

        static void GetColorForSecondary_Old(ref int primary1, ref int primary2, ref int secondary, float factor1, int factor2, float factor3)
        {
            primary1 = (int)(primary1 * factor1);
            primary2 = (int)(primary2 * factor1);
            //secondary = secondary * ((float)factor2) ; // TODO: JRDV: this should be a multiplication? Yes, try it next. The color curve is currently off when the primary color is not 255
            secondary += factor2;
            if (secondary > 255)
                secondary = 255;
        }

        static void GetColorForPrimary_Old(ref int primary, ref int secondary, ref int tertiary, float factor1, int factor2, float factor3)
        {
            primary = (int)(primary * factor1);
            secondary += factor2;
            //secondary = secondary + (int)(primary * factor2 / 255.0f);
            if (secondary > 255)
                secondary = 255;
            tertiary = (int)(secondary * factor3);
        }

        static Color GetColorCurve_Old(Color target, float factor1, int factor2, float factor3)
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
        #endregion
    }



    public class PalRealisticFlameCurveV2_EXPERIMENTAL : PaletteGenerator
    {
        public static Color[] New(Color color)
        {
            Color[] thePalette = new Color[256];
            InitializeExperimentalFlameCurveV2(thePalette, color);
            return thePalette;
        }

        /// <summary>
        /// GREEN is too turquoise, and this method is my attempt to change that, but this turned out WAAAAY too turquoise, and it turns out green flames often have turquoise, so the original experiment was a success
        /// 
        /// This method is deprecated unless I want to try another experiment.
        /// </summary>
        /// <param name="target"></param>
        private static void InitializeExperimentalFlameCurveV2(Color[] thePalette, Color target)
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


            SetPaletteFromColorRange(thePalette, colorRangeExperiment);
        }
        private static void GetColorForThreeHelper(ref int primary1, ref int primary2, ref int secondary, float factor1, float factor2, float factor3)
        {
            primary1 = (int)(primary1 * factor1);
            primary2 = (int)(primary2 * factor1);
            secondary = (int)(secondary * factor2); // TODO: JRDV: this should be a multiplication? Yes, try it next.  The color curve is currently off when the primary color is not 255
            if (secondary > 255)
                secondary = 255;
        }

        private static void GetColorForThree(ref int R, ref int G, ref int B, float factor1, float factor2, float factor3)
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

        private static void GetColorForSecondary(ref int primary1, ref int primary2, ref int secondary, float factor1, float factor2, float factor3)
        {
            primary1 = (int)(primary1 * factor1);
            primary2 = (int)(primary2 * factor1);
            secondary = (int)((primary1 + primary2) / 2.0f * factor2); // This starts at zero, so multiplying secondary doesn't DO anything!
            if (secondary > 255)
                secondary = 255;
        }

        private static void GetColorForPrimary(ref int primary, ref int secondary, ref int tertiary, float factor1, float factor2, float factor3)
        {
            primary = (int)(primary * factor1);
            secondary = (int)(primary * factor2);
            if (secondary > 255)
                secondary = 255;
            tertiary = (int)(secondary * factor3);
        }

        private static Color GetColorCurve(Color target, float factor1, float factor2, float factor3)
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
    }


    /// <summary>
    /// Hand tuned, to generate a traditional flame color
    /// </summary>
    public class PalRealisticFire : PaletteGenerator
    {
        public static Color[] New()
        {
            Color[] thePalette = new Color[256];
            InitializeRealisticFlame(thePalette);
            return thePalette;
        }
        private static void InitializeRealisticFlame(Color[] thePalette)
        {
            float fullIntensity = 1.0f;
            float mutedIntensity = 1.0f;
            //if (intensityRenderMethodRadioButton.Checked)
            //{
            //    fullIntensity = (float)(intensityUpDown.Value / 100);
            //    mutedIntensity = (float)((intensityUpDown.Value / 4 + 75) / 100);
            //}
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

            SetPaletteFromColorRange(thePalette, colorRangeBlueWhiteOrangeRed_WithIntensity_Modified);
        }
    }

    public class PalLightning : PalRealisticFlameCurve
    {
        public static Color[] New()
        {
            Color[] thePalette = new Color[256];
            Fill(thePalette);
            return thePalette;
        }

        public static void Fill(Color[] thePalette)
        {
            Color[] palPink = PalRealisticFlameCurve.New(Color.FromArgb(236, 236, 255)); // PINK
            Color[] palBlue = PalRealisticFlameCurve.New(Color.FromArgb(128, 255, 255)); // BLUE

            int switchPoint = 382;
            int whitePoint = 380; // Yes, I know setting the WhitePoint below the SwitchPoint means we don't use the old palette....
                                  // it looks better without the pink ring inside.
                                  // Looks better to start with blue, and transition to red (the logic, which may be completely false, is particles go fast-->blue, slow to red as it fades).
            for (int i = 0; i < switchPoint && i < thePalette.Length; ++i)
            {
                Color c1 = palPink[i];
                Color c2 = palBlue[i];
                float pBlue = 0.55f;
                float pRed = (1.0f - pBlue);
                thePalette[i] = Color.FromArgb(
                    (int)((c1.R * pRed + c2.R * pBlue) + 0.5f),
                    (int)((c1.G * pRed + c2.G * pBlue) + 0.5f),
                    (int)((c1.B * pRed + c2.B * pBlue) + 0.5f));
            }

            for (int i = switchPoint; i < thePalette.Length; ++i)
            {
                Color c = palPink[i];
                thePalette[i] = c;
            }

            //SetSingleColorFlame(Color.FromArgb(236, 236, 255)); // PINK
            //SetSingleColorFlame(Color.FromArgb(128, 255, 255)); // BLUE
            //RefreshTheFlamePalette();

            for (int i = whitePoint; i < thePalette.Length; ++i)
            {
                thePalette[i] = Color.White;
            }
        }
    }
}