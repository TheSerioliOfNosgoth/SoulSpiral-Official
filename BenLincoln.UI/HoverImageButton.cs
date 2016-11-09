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

//quick button subclass that switches between three images in its ImageList depending on state

using System;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.UI
{
    public class HoverImageButton : System.Windows.Forms.Button
    {
        protected enum mMode
        {
            Disabled,
            Enabled,
            Hover
        }

        public HoverImageButton()
        {
        }

        protected override void OnCreateControl()
        {
            this.Text = "";
            base.OnCreateControl();
            if (this.Enabled)
            {
                SwitchImage(mMode.Enabled);
            }
            else
            {
                SwitchImage(mMode.Disabled);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SwitchImage(mMode.Hover);
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SwitchImage(mMode.Enabled);
            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.Enabled)
            {
                SwitchImage(mMode.Enabled);
            }
            else
            {
                SwitchImage(mMode.Disabled);
            }
            base.OnEnabledChanged(e);
        }

        protected void SwitchImage(mMode newMode)
        {
            //if the control is disabled, always switch to 0 if possible
            if (!this.Enabled)
            {
                newMode = mMode.Disabled;
            }

            //if there are no images in the image list, abort
            if ((this.ImageList == null) || (this.ImageList.Images == null) || (this.ImageList.Images.Empty))
            {
                return;
            }

            //otherwise, if the image list contains the entry number of the state
            //switch to it
            switch (newMode)
            {
                case mMode.Disabled:
                    this.ImageIndex = 0;
                    this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    break;
                case mMode.Enabled:
                    if (this.ImageList.Images.Count > 1)
                    {
                        this.ImageIndex = 1;
                    }
                    else
                    {
                        this.ImageIndex = 0;
                    }
                    this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    break;
                case mMode.Hover:
                    if (this.ImageList.Images.Count > 2)
                    {
                        this.ImageIndex = 2;
                    }
                    else
                    {
                        this.ImageIndex = 0;
                    }
                    this.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                    break;
            }

        }
    }
}
