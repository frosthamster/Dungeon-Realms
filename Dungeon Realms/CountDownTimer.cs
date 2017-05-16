using System;
using System.Windows.Forms;

namespace Dungeon_Realms
{
    class CountDownTimer
    {
        private readonly Timer timer;
        public CountDownTimer(int interval, Action tick)
        {
            timer = new Timer { Interval = interval };
            timer.Tick += (sender, args) => tick();
            timer.Tick += (sender, args) => timer.Stop();
        }

        public void Start() => timer.Start();
    }
}
