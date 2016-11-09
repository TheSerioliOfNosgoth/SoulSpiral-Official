// Soul Spiral
// Copyright 2006-2012 Ben Lincoln
// http://www.thelostworlds.net/
//

// This file is part of Soul Spiral.

// Soul Spiral is free software: you can redistribute it and/or modify
// it under the terms of version 3 of the GNU General Public License as published by
// the Free Software Foundation.

// Soul Spiral is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Soul Spiral (in the file LICENSE.txt).  
// If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace SoulSpiral
{
    public partial class frmAbout : Form
    {
        protected int mVerMaj;
        protected int mVerMin;
        protected int mBuild;
        protected const string mVerComment = "";

        public frmAbout()
        {
            InitializeComponent();

            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            AssemblyName assemblyName = currentAssembly.GetName();

            mVerMaj = assemblyName.Version.Major;
            mVerMin = assemblyName.Version.Minor;
            mBuild = assemblyName.Version.Build;
            lblVersion.Text = "Version " + mVerMaj + "." + mVerMin + mVerComment + ", Build " + mBuild;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}