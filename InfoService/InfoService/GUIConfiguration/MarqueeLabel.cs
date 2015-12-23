using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InfoService.GUIConfiguration
{
    class MarqueeLabel : Label
    {
        private int CurrentPosition { get; set; }
        private Timer Timer { get; set; }
        private Size TextSize;

        public MarqueeLabel()
        {
            CurrentPosition = Width;
            UseCompatibleTextRendering = true;
            Timer = new Timer {Interval = 25};
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (CurrentPosition <= -TextSize.Width - 50)
                CurrentPosition = Width;
            else
                CurrentPosition -= 2;

            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            TextSize = TextRenderer.MeasureText(Text, Font);
            base.OnTextChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), (float)CurrentPosition, 0);
            //e.Graphics.TranslateTransform((float)CurrentPosition, 0);
            //base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Timer != null)
                    Timer.Dispose();
            }
            Timer = null;
        }
    }


}
