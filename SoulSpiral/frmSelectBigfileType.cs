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
using UI = BenLincoln.UI;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace SoulSpiral
{
    public class frmSelectBigfileType : UI.Dialogue
    {
        private System.Windows.Forms.ComboBox cbxTypes;
        protected BF.BigFileType mSelectedType;

        public frmSelectBigfileType()
            : base()
        {
            //mMinWidth = 700;
            mMinWidth = 582;
            //SetIcon(UI.Dialogue.ICON_QUESTION);
            this.Text = "Select BigFile type";

            lblMessage.Text = "Please select the type of BigFile you are trying to open.";
            cbxTypes = new ComboBox();
            cbxTypes.Width = 400;
            pnlText.Controls.Add(cbxTypes);
            cbxTypes.Location = new Point(8, cbxTypes.Location.Y);

            //add buttons
            UI.ResizingButton btnOK = new UI.ResizingButton();
            btnOK.Text = "Use the selected file type";
            btnOK.Click += new EventHandler(btnOK_Click);
            pnlButtons.Controls.Add(btnOK);

            UI.ResizingButton btnCancel = new UI.ResizingButton();
            btnCancel.Text = "Cancel the file load";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            pnlButtons.Controls.Add(btnCancel);
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.pnlIcon.SuspendLayout();
            this.pnlText.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Location = new System.Drawing.Point(0, 37);
            this.pnlButtons.Size = new System.Drawing.Size(344, 45);
            // 
            // pnlIcon
            // 
            this.pnlIcon.Size = new System.Drawing.Size(88, 82);
            // 
            // pnlText
            // 
            this.pnlText.Size = new System.Drawing.Size(344, 82);
            // 
            // frmSelectBigfileType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(532, 82);
            this.Name = "frmSelectBigfileType";
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.pnlIcon.ResumeLayout(false);
            this.pnlIcon.PerformLayout();
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            this.ResumeLayout(false);

        }

        public void SetDropDownOptions(BF.BigFileTypeCollection options)
        {
            for (int i = 0; i < options.BigFileTypeNames.Length; i++)
            {
                BF.BigFileType bft = options.GetTypeByName(options.BigFileTypeNames[i]);
                cbxTypes.Items.Add(bft.Description);
            }
            if (cbxTypes.Items.Count > 0)
            {
                cbxTypes.SelectedIndex = 0;
            }
        }

        public void SetSelectedType(int whichIndex)
        {
            cbxTypes.SelectedIndex = whichIndex;
        }

        public int SelectedItem
        {
            get
            {
                return cbxTypes.SelectedIndex;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            cbxTypes.Location = new Point(cbxTypes.Location.X, lblMessage.Location.Y + lblMessage.Height + 12);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            mResult = UI.DialogueResult.YES;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mResult = UI.DialogueResult.ABORT_PROCESS;
            this.Hide();
        }

    }
}
