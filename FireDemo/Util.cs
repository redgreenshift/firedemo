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

        private static int? RandomSeed = null;
        //private static Random SingletonRNG = new Random();

        /// <summary>
        /// Simple way to enforce that multiple instances of Random created in rapid succession don't get initialized with the same seed.
        /// </summary>
        /// <returns></returns>
        public static Random NewRandom()
        {
            //return SingletonRNG;
            Random rng;

            if (RandomSeed != null)
            {
                rng = new Random(RandomSeed.Value);
                ++RandomSeed;
            }
            else
            {
                rng = new Random();
                RandomSeed = rng.Next();
            }

            return rng;
        }
    }
}
