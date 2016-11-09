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
using System.Threading;

namespace BenLincoln.UI
{
    public partial class ProgressWindow : Form
    {
        delegate void IntDelegate(int newValue);
        delegate void StringDelegate(string newValue);

        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void SetProgress(int newProgress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new IntDelegate(SetProgress), new object[] { newProgress });
                return;
            }
            pbProgress.Value = newProgress;
        }

        public void SetMessage(string newMessage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new StringDelegate(SetMessage), new object[] { newMessage });
                return;
            }
            lblText.Text = newMessage;
        }

        public void SetTitle(string newMessage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new StringDelegate(SetTitle), new object[] { newMessage });
                return;
            }
            this.Text = newMessage;
        }

        //public string Message
        //{
        //    get
        //    {
        //        return lblText.Text;
        //    }
        //    set
        //    {
        //        lblText.Text = value;
        //    }
        //}

        public string Title
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

    }
}