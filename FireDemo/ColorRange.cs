using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FireDemo
{
	struct ColorRange
	{
		public ColorRange(Color color, int range)
		{
			this.color = color;
			this.range = range;
		}
		public int range;
		public readonly Color color;
	}
}