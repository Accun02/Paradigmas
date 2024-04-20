using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public struct Time
    {
        private static DateTime _startTime;
        private static float _lastTimeFrame;
        public static float DeltaTime;

        public void Initialize() => _startTime = DateTime.Now;

        public void Update()
        {
            float currentTime = (float)(DateTime.Now - _startTime).TotalSeconds;
            DeltaTime = currentTime - _lastTimeFrame;
            _lastTimeFrame = currentTime;
        }
    }
}
