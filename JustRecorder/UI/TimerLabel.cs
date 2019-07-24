using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JustRecorder.UI
{
    public class TimerLabel : Label
    {
        private readonly Timer timer = new Timer()
        {
            Interval = 1000,
        };
        private int secCount = 0;

        public TimerLabel() : base()
        {
            timer.Tick += (s, e) =>
            {
                secCount++;
                UpdateLabelText();
            };
            UpdateLabelText();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            secCount = 0;
            UpdateLabelText();
        }

        private void UpdateLabelText()
        {
            int hour = secCount / 3600;
            int minute = secCount % 3600 / 60;
            int second = secCount % 3600 % 60;
            Text = $"{hour.ToString("D2")}:{minute.ToString("D2")}:{second.ToString("D2")}";
        }
    }
}
