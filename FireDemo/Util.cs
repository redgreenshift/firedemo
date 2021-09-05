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
				PlatformID pid = Environment.OSVersion.Platform;
				return (pid == PlatformID.Unix)
					|| (pid == PlatformID.MacOSX)
					|| ((int)pid == 128); // Mono
			}
		}
	}
}
