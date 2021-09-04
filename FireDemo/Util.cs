using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDemo
{
    class Util
    {
		public static bool IsLinux
		{
			get
			{
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) // Unix
					|| (p == 6) // macOS
					|| (p == 128); // Mono
			}
		}
	}
}
