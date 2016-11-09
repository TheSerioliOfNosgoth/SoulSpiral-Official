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
using CDBigFile = BenLincoln.TheLostWorlds.CDBigFile;

namespace SoulSpiral
{
    public partial class frmHexEdit : Form
    {
        protected string mBigFilePath;
        protected CDBigFile.File mEditFile;

        public frmHexEdit()
        {
            InitializeComponent();

            CreateToolTip(this.btnSave, "Save the changes to the BigFile");
            CreateToolTip(this.btnRevert, "Revert to the existing version in the BigFile");
        }

        protected void CreateToolTip(System.Windows.Forms.Control boundControl, string message)
        {
            ToolTip tTip = new ToolTip();
            tTip.AutoPopDelay = 1000;
            tTip.InitialDelay = 1000;
            tTip.ReshowDelay = 500;
            tTip.ShowAlways = true;
            tTip.SetToolTip(boundControl, message);
        }

        public void LoadFile(string fileName, CDBigFile.File editFile, bool useStreamingMode)
        {
            mBigFilePath = fileName;
            mEditFile = editFile;
            hpHex.Read(fileName, editFile.Offset, editFile.Length, useStreamingMode);
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            ConfirmSave();
        }

        private void mnuRevert_Click(object sender, EventArgs e)
        {
            ConfirmRevert();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            ConfirmExit();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfirmSave();
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            ConfirmRevert();
        }

        protected void ConfirmSave()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to write the changes to the BigFile? " +
                "This operation cannot be undone.", "Confirm Write", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                SaveChanges();
            }
        }

        protected void ConfirmRevert()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to revert to the last saved version of this file? " +
                "This operation cannot be undone.", "Confirm Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                RevertChanges();
            }
        }

        protected void ConfirmExit()
        {
            //exit immediately if there are no unsaved changes
            if (!hpHex.DataAlteredByUser)
            {
                return;
            }
            if (CheckWriteUnsavedChanges())
            {
                SaveChanges();
            }
        }

        protected bool CheckWriteUnsavedChanges()
        {
            DialogResult result = MessageBox.Show("You have modified the data in the file but have not " +
                "saved your changes. If you do not save now, the changes will be lost. Do you want to save your changes to the BigFile?",
                "Unsaved Changes",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }

        protected void SaveChanges()
        {
            //hpHex.Write();
            hpHex.Write(mBigFilePath, mEditFile.Offset);
        }

        protected void RevertChanges()
        {
            hpHex.Revert();
        }

        //override the form closing event
        private void frmHexEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfirmExit();
        }



    }
}