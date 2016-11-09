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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UI = BenLincoln.UI;

namespace BenLincoln.UI
{
    public partial class Dialogue : Form
    {
        protected int mResult;
        protected int mIcon;
        protected int mMinWidth = 500;
        protected int mMinHeight = 64;

        protected const int MAX_WINDOW_WIDTH = 1024;

        public const int ICON_QUESTION = 0;
        public const int ICON_EXCLAMATION = 1;
        public const int ICON_X = 2;

        public Dialogue()
        {
            InitializeComponent();
            mResult = UI.DialogueResult.NONE;
            this.StartPosition = FormStartPosition.CenterParent;
            mMinWidth = pnlIcon.Width + pnlRight.Width;
        }

        #region Properties

        public int Result
        {
            get
            {
                return mResult;
            }
        }

        public Image IconImage
        {
            get
            {
                return imgIcon.Image;
            }
            set
            {
                imgIcon.Image = value;
                imgIcon.Width = value.Width;
                imgIcon.Height = value.Height;
            }
        }

        #endregion

        public void SetMessage(string newMessage)
        {
            lblMessage.Text = newMessage;
            this.Invalidate();
        }

        public void SetTitle(string newTitle)
        {
            this.Text = newTitle;
        }

        public void SetIcon(int whichIcon)
        {
            mIcon = whichIcon;
            switch (mIcon)
            {
                case ICON_QUESTION:
                    imgIcon.Image = BenLincoln.UI.Properties.Resources.DialogueIcons_Small_Question;
                    break;
                case ICON_EXCLAMATION:
                    imgIcon.Image = BenLincoln.UI.Properties.Resources.DialogueIcons_Small_Exclamation;
                    break;
                case ICON_X:
                    imgIcon.Image = BenLincoln.UI.Properties.Resources.DialogueIcons_Small_X;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            AutoResize(e);
        }

        //get the farthest right occupied position of the buttons panel
        protected int getFirstUnoccupiedPixel()
        {
            int pixelNum = 0;
            if (this.pnlButtons.Controls.Count > 0)
            {
                foreach (Control button in this.pnlButtons.Controls)
                {
                    pixelNum = Math.Max(pixelNum, button.Location.X + button.Width);
                }
            }
            return pixelNum;
        }

        protected void AutoResize(PaintEventArgs e)
        {
            //resize icon panel first
            pnlIcon.Width = imgIcon.Image.Width + 24;
            int minHeight = imgIcon.Image.Height + 24;

            //next resize buttons panel
            if (pnlButtons.Controls.Count > 0)
            {
                int maxButtonHeight = 23;
                int leftMostPosition = pnlButtons.Width;
                foreach (Control button in pnlButtons.Controls)
                {
                    maxButtonHeight = Math.Max(maxButtonHeight, button.Height);
                    leftMostPosition -= (button.PreferredSize.Width + 16);
                }
                //add back one 8 to fix the right margin
                leftMostPosition += 8;
                pnlButtons.Height = maxButtonHeight + 8;
                //if the panel isn't wide enough, make it wide enough
                if (leftMostPosition < 0)
                {
                    mMinWidth += Math.Abs(leftMostPosition) + 32;
                    leftMostPosition = 8;
                }
                //distribute buttons in panel
                foreach (Control button in pnlButtons.Controls)
                {
                    button.Location = new Point(leftMostPosition, button.Location.Y);
                    leftMostPosition += (button.PreferredSize.Width + 16);
                }           
            }

            //now resize text area
            int newHeight = 0;
            int maxWidth = 0;
            string[] lines = lblMessage.Text.Split(new string[] {"\r\n"}, StringSplitOptions.None);
            foreach (string line in lines)
            {
                SizeF currentSize = e.Graphics.MeasureString(line, lblMessage.Font);
                int currentWidth = (int)currentSize.Width;
                if (currentWidth > maxWidth)
                {
                    maxWidth = currentWidth;
                }
                newHeight += (int)currentSize.Height;
            }
            lblMessage.Width = maxWidth;
            lblMessage.Height = newHeight;

            //margin of 12
            this.Width = Math.Min(Math.Max(mMinWidth,
                maxWidth + pnlIcon.Width), MAX_WINDOW_WIDTH);
            //this.Width = widthResize + pnlIcon.Width + 12 + pnlButtons.PreferredSize.Width;
            //36 is roughly the height of the title bar plus the margin for the icon
            minHeight = Math.Max(imgIcon.Image.Height, lblMessage.Height + pnlButtons.Height) + 56;
            this.Height = Math.Max(minHeight, newHeight);

            OnAutoResize();
        }

        //for subclasses
        protected virtual void OnAutoResize()
        {
        }

        protected SizeF getStringSize(Graphics g, string text, Font font)
        {
            return g.MeasureString(text, font);
        }
    }
}