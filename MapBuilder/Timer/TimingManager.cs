using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MapBuilder.Timer
{
    static class TimingManager
    {
        private static Stopwatch _sw = new Stopwatch();
        
        public static void Start()
        {
            _sw.Start();
        }

        public static void Stop()
        {
            _sw.Stop();
        }

        public static void Reset()
        {
            _sw.Reset();
        }

        public static long GetTime()
        {
            return _sw.ElapsedMilliseconds;
        }


    }
}
