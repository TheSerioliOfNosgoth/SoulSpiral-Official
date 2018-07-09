// BenLincoln.UI
// Copyright 2006-2012 Ben Lincoln
// http://www.thelostworlds.net/
//

// This file is part of BenLincoln.UI.

// BenLincoln.UI is free software: you can redistribute it and/or modify
// it under the terms of version 3 of the GNU General Public License as published by
// the Free Software Foundation.

// BenLincoln.UI is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with BenLincoln.UI (in the file LICENSE.txt).  
// If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace BenLincoln.UI
{
    public partial class TimeDisplay : System.Windows.Forms.Panel
    {
        // total time in seconds
        protected double _TotalTime = 0;

        // elapsed percent (0 - 1)
        protected double _PercentElapsed = 0;

        // cached total time string
        protected string _TotalTimeString = "";

        // image for display
        protected PictureBox imgDisplay;

        // is the control drawing?
        protected bool _IsDrawing = false;

        public TimeDisplay()
        {
            // initialize the total time string
            _TotalTimeString = GetTimeString(_TotalTime);

            // initialize the picturebox
            imgDisplay = new PictureBox();
            imgDisplay.Dock = DockStyle.Fill;
            imgDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(imgDisplay);
        }

        public void SetTotalTime(double totalTime)
        {
            _TotalTime = totalTime;
            _PercentElapsed = 0;

            // cache the total time
            _TotalTimeString = GetTimeString(totalTime);
            this.Invalidate();
        }

        public void SetElapsedPercent(double newPercent)
        {
            _PercentElapsed = newPercent;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_IsDrawing)
            {
                base.OnPaint(e);
                return;
            }
            _IsDrawing = true;
            Bitmap tempBitmap = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(tempBitmap);
            Clear(g, BackColor);
            DisplayTime(g);
            imgDisplay.Image = tempBitmap;
            _IsDrawing = false;
            //this.Invalidate();
        }

        protected void Clear(Graphics g, System.Drawing.Color backgroundColour)
        {
            // clear the background
            Rectangle drawArea = new Rectangle(0, 0, this.Width, this.Height);
            SolidBrush bgBrush = new SolidBrush(backgroundColour);
            g.FillRectangle(bgBrush, drawArea);
        }

        protected void DisplayTime(Graphics g)
        {
            // figure out how long the current clip is
            string currentTime = GetTimeString(_TotalTime * _PercentElapsed);
            string time = currentTime + " / " + _TotalTimeString;

            // figure out where the text should start to put it in the center
            SizeF messageSize = g.MeasureString(time, Font);
            int startX = (this.Width - (int)messageSize.Width) / 2;
            int startY = ((this.Height - (int)messageSize.Height)) / 2;

            SolidBrush messageBrush = new SolidBrush(ForeColor);
            g.DrawString(time, Font, messageBrush, startX, startY);
        }

        public static string GetTimeString(double lengthInSeconds)
        {
            // get number of hours
            int lenHours = GetTimeUnits(lengthInSeconds, 3600);
            // remove hours from total
            lengthInSeconds -= (double)(lenHours * 3600);

            // get number of minutes
            int lenMinutes = GetTimeUnits(lengthInSeconds, 60);
            // remove hours from total
            lengthInSeconds -= (double)(lenMinutes * 60);

            // get number of seconds
            int lenSeconds = GetTimeUnits(lengthInSeconds, 1);
            // remove whole seconds
            lengthInSeconds -= (double)(lenSeconds);

            // get fractions of a second
            int lenFractions = (int)((double)1000 * lengthInSeconds);

            string time = "";
            // only display hours on the off chance that the sample actually *is* at least an hour long
            if (lenHours > 0)
            {
                time = string.Format("{0:00}:", lenHours);
            }
            time += string.Format("{0:00}:", lenMinutes);
            time += string.Format("{0:00}.", lenSeconds);
            time += string.Format("{0:000}", lenFractions);

            return time;
        }

        protected static int GetTimeUnits(double numSecondsTotal, double numSecondsPerUnit)
        {
            int lenUnits = (int)(numSecondsTotal / numSecondsPerUnit);
            // make sure it wasn't rounded up
            if ((lenUnits * numSecondsPerUnit) > numSecondsTotal)
            {
                lenUnits--;
            }
            return lenUnits;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
