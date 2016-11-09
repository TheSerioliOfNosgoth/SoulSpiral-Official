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

namespace SoulSpiral
{
    public partial class frmRecognizedBigfile : UI.Dialogue
    {
        public frmRecognizedBigfile()
            : base()
        {
            //mMinWidth = 700;
            //SetIcon(UI.Dialogue.ICON_QUESTION);
            this.Text = "This BigFile is recognized";

            //add buttons
            UI.ResizingButton btnYes = new UI.ResizingButton();
            btnYes.Text = "Use the automatic settings";
            btnYes.Click += new EventHandler(btnYes_Click);
            pnlButtons.Controls.Add(btnYes);

            UI.ResizingButton btnNo = new UI.ResizingButton();
            btnNo.Text = "Choose the file type manually";
            btnNo.Click += new EventHandler(btnNo_Click);
            pnlButtons.Controls.Add(btnNo);

            UI.ResizingButton btnCancel = new UI.ResizingButton();
            btnCancel.Text = "Cancel the file load";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            pnlButtons.Controls.Add(btnCancel);
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            mResult = UI.DialogueResult.YES;
            this.Hide();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            mResult = UI.DialogueResult.NO;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mResult = UI.DialogueResult.ABORT_PROCESS;
            this.Hide();
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
            this.pnlButtons.Location = new System.Drawing.Point(0, 41);
            this.pnlButtons.Size = new System.Drawing.Size(404, 45);
            // 
            // pnlIcon
            // 
            this.pnlIcon.Size = new System.Drawing.Size(88, 86);
            // 
            // txtMessage
            // 
            this.lblMessage.Size = new System.Drawing.Size(0, 0);
            // 
            // pnlText
            // 
            this.pnlText.Size = new System.Drawing.Size(404, 86);
            // 
            // frmRecognizedBigfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(492, 86);
            this.Name = "frmRecognizedBigfile";
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.pnlIcon.ResumeLayout(false);
            this.pnlIcon.PerformLayout();
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}