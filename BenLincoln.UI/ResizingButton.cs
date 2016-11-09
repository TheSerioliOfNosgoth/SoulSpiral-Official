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
using System.Text;
using System.Drawing;

namespace BenLincoln.UI
{
    public class ResizingButton : System.Windows.Forms.Button
    {
        protected const int MINWIDTH = 24;
        protected const int MINHEIGHT = 23;

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            //resize the button to accomodate the text
            SizeF currentSize = pevent.Graphics.MeasureString(this.Text, this.Font);
            this.Width = Math.Max(MINWIDTH, (int)currentSize.Width + 10);
            this.Height = Math.Max(MINHEIGHT, (int)currentSize.Height + 10);
        }
    }
}
