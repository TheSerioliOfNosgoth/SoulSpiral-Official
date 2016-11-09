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

namespace SoulSpiral
{
    partial class frmHexEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHexEdit));
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.gboButtons = new System.Windows.Forms.GroupBox();
            this.btnRevert = new BenLincoln.UI.HoverImageButton();
            this.ilRevertButton = new System.Windows.Forms.ImageList(this.components);
            this.btnSave = new BenLincoln.UI.HoverImageButton();
            this.ilSaveButton = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.hpHex = new HexEditor.HexPanel();
            this.menuHexEdit = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRevert = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlButtons.SuspendLayout();
            this.gboButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.menuHexEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.gboButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Location = new System.Drawing.Point(0, 24);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(748, 66);
            this.pnlButtons.TabIndex = 0;
            // 
            // gboButtons
            // 
            this.gboButtons.Controls.Add(this.btnRevert);
            this.gboButtons.Controls.Add(this.btnSave);
            this.gboButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gboButtons.Location = new System.Drawing.Point(0, 0);
            this.gboButtons.Name = "gboButtons";
            this.gboButtons.Size = new System.Drawing.Size(748, 66);
            this.gboButtons.TabIndex = 0;
            this.gboButtons.TabStop = false;
            // 
            // btnRevert
            // 
            this.btnRevert.FlatAppearance.BorderSize = 0;
            this.btnRevert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevert.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnRevert.ImageIndex = 1;
            this.btnRevert.ImageList = this.ilRevertButton;
            this.btnRevert.Location = new System.Drawing.Point(65, 10);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(54, 54);
            this.btnRevert.TabIndex = 1;
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // ilRevertButton
            // 
            this.ilRevertButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilRevertButton.ImageStream")));
            this.ilRevertButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilRevertButton.Images.SetKeyName(0, "Decompose-Disabled.PNG");
            this.ilRevertButton.Images.SetKeyName(1, "Decompose-Enabled.PNG");
            this.ilRevertButton.Images.SetKeyName(2, "Decompose-Hover.PNG");
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnSave.ImageIndex = 1;
            this.btnSave.ImageList = this.ilSaveButton;
            this.btnSave.Location = new System.Drawing.Point(5, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 54);
            this.btnSave.TabIndex = 0;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ilSaveButton
            // 
            this.ilSaveButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSaveButton.ImageStream")));
            this.ilSaveButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSaveButton.Images.SetKeyName(0, "Compose-Disabled.PNG");
            this.ilSaveButton.Images.SetKeyName(1, "Compose-Enabled.PNG");
            this.ilSaveButton.Images.SetKeyName(2, "Compose-Hover.PNG");
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.hpHex);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 90);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(748, 527);
            this.pnlMain.TabIndex = 1;
            // 
            // hpHex
            // 
            this.hpHex.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.hpHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hpHex.isSplitterFixed = true;
            this.hpHex.Location = new System.Drawing.Point(0, 0);
            this.hpHex.Name = "hpHex";
            this.hpHex.numberOfColumns = 16;
            this.hpHex.Size = new System.Drawing.Size(748, 527);
            this.hpHex.splitterDistance = 480;
            this.hpHex.TabIndex = 0;
            // 
            // menuHexEdit
            // 
            this.menuHexEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.menuHexEdit.Location = new System.Drawing.Point(0, 0);
            this.menuHexEdit.Name = "menuHexEdit";
            this.menuHexEdit.Size = new System.Drawing.Size(748, 24);
            this.menuHexEdit.TabIndex = 2;
            this.menuHexEdit.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.mnuRevert,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(35, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(152, 22);
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuRevert
            // 
            this.mnuRevert.Name = "mnuRevert";
            this.mnuRevert.Size = new System.Drawing.Size(152, 22);
            this.mnuRevert.Text = "&Revert";
            this.mnuRevert.Click += new System.EventHandler(this.mnuRevert_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(152, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // frmHexEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 617);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.menuHexEdit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHexEdit";
            this.Text = "frmHexEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHexEdit_FormClosing);
            this.pnlButtons.ResumeLayout(false);
            this.gboButtons.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.menuHexEdit.ResumeLayout(false);
            this.menuHexEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.GroupBox gboButtons;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.MenuStrip menuHexEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuRevert;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private HexEditor.HexPanel hpHex;
        private BenLincoln.UI.HoverImageButton btnSave;
        private System.Windows.Forms.ImageList ilSaveButton;
        private BenLincoln.UI.HoverImageButton btnRevert;
        private System.Windows.Forms.ImageList ilRevertButton;
    }
}