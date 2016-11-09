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
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuMain = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuOpen = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuDirectoryNames = new System.Windows.Forms.MenuItem();
            this.mnuRawIndexNames = new System.Windows.Forms.MenuItem();
            this.mnuExport = new System.Windows.Forms.MenuItem();
            this.mnuExportCurrent = new System.Windows.Forms.MenuItem();
            this.mnuExportDirectory = new System.Windows.Forms.MenuItem();
            this.mnuExportAll = new System.Windows.Forms.MenuItem();
            this.mnuExportIndexData = new System.Windows.Forms.MenuItem();
            this.mnuImport = new System.Windows.Forms.MenuItem();
            this.mnuReplace = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.mnuHexEdit = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.gboButtons = new System.Windows.Forms.GroupBox();
            this.btnHexEdit = new BenLincoln.UI.HoverImageButton();
            this.ilHexEditButton = new System.Windows.Forms.ImageList(this.components);
            this.btnOpen = new BenLincoln.UI.HoverImageButton();
            this.ilOpenButton = new System.Windows.Forms.ImageList(this.components);
            this.btnReplace = new BenLincoln.UI.HoverImageButton();
            this.ilReplaceButton = new System.Windows.Forms.ImageList(this.components);
            this.btnExport = new BenLincoln.UI.HoverImageButton();
            this.ilExportButton = new System.Windows.Forms.ImageList(this.components);
            this.btnExportAll = new BenLincoln.UI.HoverImageButton();
            this.ilExportAllButton = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tvBigfile = new System.Windows.Forms.TreeView();
            this.txtMain = new System.Windows.Forms.TextBox();
            this.pnlButtons.SuspendLayout();
            this.gboButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuView,
            this.mnuExport,
            this.mnuImport,
            this.mnuTools,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOpen,
            this.mnuExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuOpen
            // 
            this.mnuOpen.Index = 0;
            this.mnuOpen.Text = "&Open Bigfile";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 1;
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuView
            // 
            this.mnuView.Index = 1;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDirectoryNames,
            this.mnuRawIndexNames});
            this.mnuView.Text = "&View";
            // 
            // mnuDirectoryNames
            // 
            this.mnuDirectoryNames.Index = 0;
            this.mnuDirectoryNames.Text = "&Directory Names";
            this.mnuDirectoryNames.Click += new System.EventHandler(this.mnuDirectoryNames_Click);
            // 
            // mnuRawIndexNames
            // 
            this.mnuRawIndexNames.Index = 1;
            this.mnuRawIndexNames.Text = "&Raw Index Names";
            this.mnuRawIndexNames.Click += new System.EventHandler(this.mnuRawIndexNames_Click);
            // 
            // mnuExport
            // 
            this.mnuExport.Index = 2;
            this.mnuExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExportCurrent,
            this.mnuExportDirectory,
            this.mnuExportAll,
            this.mnuExportIndexData});
            this.mnuExport.Text = "&Export";
            // 
            // mnuExportCurrent
            // 
            this.mnuExportCurrent.Index = 0;
            this.mnuExportCurrent.Text = "Current &File";
            this.mnuExportCurrent.Click += new System.EventHandler(this.mnuExportCurrent_Click);
            // 
            // mnuExportDirectory
            // 
            this.mnuExportDirectory.Index = 1;
            this.mnuExportDirectory.Text = "Current &Directory";
            this.mnuExportDirectory.Click += new System.EventHandler(this.mnuExportDirectory_Click);
            // 
            // mnuExportAll
            // 
            this.mnuExportAll.Index = 2;
            this.mnuExportAll.Text = "&All Files";
            this.mnuExportAll.Click += new System.EventHandler(this.mnuExportAll_Click);
            // 
            // mnuExportIndexData
            // 
            this.mnuExportIndexData.Index = 3;
            this.mnuExportIndexData.Text = "&Raw Index Data";
            this.mnuExportIndexData.Click += new System.EventHandler(this.mnuExportIndexData_Click);
            // 
            // mnuImport
            // 
            this.mnuImport.Index = 3;
            this.mnuImport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuReplace});
            this.mnuImport.Text = "&Import";
            // 
            // mnuReplace
            // 
            this.mnuReplace.Index = 0;
            this.mnuReplace.Text = "&Replace Current File";
            this.mnuReplace.Click += new System.EventHandler(this.mnuReplace_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.Index = 4;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHexEdit,
            this.mnuOptions});
            this.mnuTools.Text = "&Tools";
            this.mnuTools.Visible = false;
            // 
            // mnuHexEdit
            // 
            this.mnuHexEdit.Index = 0;
            this.mnuHexEdit.Text = "&Hex Edit Selected File";
            this.mnuHexEdit.Click += new System.EventHandler(this.mnuHexEdit_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Index = 1;
            this.mnuOptions.Text = "&Options";
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 5;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAbout});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            this.mnuAbout.Text = "&About SoulSpiral";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.gboButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(991, 66);
            this.pnlButtons.TabIndex = 0;
            // 
            // gboButtons
            // 
            this.gboButtons.Controls.Add(this.btnHexEdit);
            this.gboButtons.Controls.Add(this.btnOpen);
            this.gboButtons.Controls.Add(this.btnReplace);
            this.gboButtons.Controls.Add(this.btnExport);
            this.gboButtons.Controls.Add(this.btnExportAll);
            this.gboButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gboButtons.Location = new System.Drawing.Point(0, 0);
            this.gboButtons.Name = "gboButtons";
            this.gboButtons.Size = new System.Drawing.Size(991, 66);
            this.gboButtons.TabIndex = 4;
            this.gboButtons.TabStop = false;
            // 
            // btnHexEdit
            // 
            this.btnHexEdit.FlatAppearance.BorderSize = 0;
            this.btnHexEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHexEdit.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnHexEdit.ImageIndex = 1;
            this.btnHexEdit.ImageList = this.ilHexEditButton;
            this.btnHexEdit.Location = new System.Drawing.Point(271, 8);
            this.btnHexEdit.Name = "btnHexEdit";
            this.btnHexEdit.Size = new System.Drawing.Size(54, 54);
            this.btnHexEdit.TabIndex = 4;
            this.btnHexEdit.UseVisualStyleBackColor = true;
            this.btnHexEdit.Click += new System.EventHandler(this.btnHexEdit_Click);
            // 
            // ilHexEditButton
            // 
            this.ilHexEditButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilHexEditButton.ImageStream")));
            this.ilHexEditButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilHexEditButton.Images.SetKeyName(0, "Button-HexEdit-Disabled.PNG");
            this.ilHexEditButton.Images.SetKeyName(1, "Button-HexEdit-Enabled.PNG");
            this.ilHexEditButton.Images.SetKeyName(2, "Button-HexEdit-Hover.PNG");
            // 
            // btnOpen
            // 
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnOpen.ImageIndex = 1;
            this.btnOpen.ImageList = this.ilOpenButton;
            this.btnOpen.Location = new System.Drawing.Point(4, 8);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(54, 54);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // ilOpenButton
            // 
            this.ilOpenButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilOpenButton.ImageStream")));
            this.ilOpenButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilOpenButton.Images.SetKeyName(0, "Button-Open-Enabled.PNG");
            this.ilOpenButton.Images.SetKeyName(1, "Button-Open-Enabled.PNG");
            this.ilOpenButton.Images.SetKeyName(2, "Button-Open-Hover.PNG");
            // 
            // btnReplace
            // 
            this.btnReplace.FlatAppearance.BorderSize = 0;
            this.btnReplace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplace.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnReplace.ImageIndex = 1;
            this.btnReplace.ImageList = this.ilReplaceButton;
            this.btnReplace.Location = new System.Drawing.Point(205, 8);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(54, 54);
            this.btnReplace.TabIndex = 3;
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // ilReplaceButton
            // 
            this.ilReplaceButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilReplaceButton.ImageStream")));
            this.ilReplaceButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilReplaceButton.Images.SetKeyName(0, "Button-Replace-Disabled.PNG");
            this.ilReplaceButton.Images.SetKeyName(1, "Button-Replace-Enabled.PNG");
            this.ilReplaceButton.Images.SetKeyName(2, "Button-Replace-Hover.PNG");
            // 
            // btnExport
            // 
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnExport.ImageIndex = 1;
            this.btnExport.ImageList = this.ilExportButton;
            this.btnExport.Location = new System.Drawing.Point(71, 8);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(54, 54);
            this.btnExport.TabIndex = 1;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ilExportButton
            // 
            this.ilExportButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilExportButton.ImageStream")));
            this.ilExportButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilExportButton.Images.SetKeyName(0, "Button-Export-Disabled.PNG");
            this.ilExportButton.Images.SetKeyName(1, "Button-Export-Enabled.PNG");
            this.ilExportButton.Images.SetKeyName(2, "Button-Export-Hover.PNG");
            // 
            // btnExportAll
            // 
            this.btnExportAll.FlatAppearance.BorderSize = 0;
            this.btnExportAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportAll.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnExportAll.ImageIndex = 1;
            this.btnExportAll.ImageList = this.ilExportAllButton;
            this.btnExportAll.Location = new System.Drawing.Point(138, 8);
            this.btnExportAll.Name = "btnExportAll";
            this.btnExportAll.Size = new System.Drawing.Size(54, 54);
            this.btnExportAll.TabIndex = 2;
            this.btnExportAll.UseVisualStyleBackColor = true;
            this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
            // 
            // ilExportAllButton
            // 
            this.ilExportAllButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilExportAllButton.ImageStream")));
            this.ilExportAllButton.TransparentColor = System.Drawing.Color.Transparent;
            this.ilExportAllButton.Images.SetKeyName(0, "Button-ExportAll-Disabled.PNG");
            this.ilExportAllButton.Images.SetKeyName(1, "Button-ExportAll-Enabled.PNG");
            this.ilExportAllButton.Images.SetKeyName(2, "Button-ExportAll-Hover.PNG");
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 66);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(991, 472);
            this.pnlMain.TabIndex = 1;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tvBigfile);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.txtMain);
            this.splitMain.Size = new System.Drawing.Size(991, 472);
            this.splitMain.SplitterDistance = 269;
            this.splitMain.TabIndex = 0;
            // 
            // tvBigfile
            // 
            this.tvBigfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvBigfile.Location = new System.Drawing.Point(0, 0);
            this.tvBigfile.Name = "tvBigfile";
            this.tvBigfile.Size = new System.Drawing.Size(269, 472);
            this.tvBigfile.TabIndex = 0;
            this.tvBigfile.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBigfile_AfterSelect);
            // 
            // txtMain
            // 
            this.txtMain.BackColor = System.Drawing.SystemColors.Window;
            this.txtMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMain.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMain.Location = new System.Drawing.Point(0, 0);
            this.txtMain.Multiline = true;
            this.txtMain.Name = "txtMain";
            this.txtMain.ReadOnly = true;
            this.txtMain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMain.Size = new System.Drawing.Size(718, 472);
            this.txtMain.TabIndex = 0;
            this.txtMain.WordWrap = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 538);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.menuMain;
            this.Name = "frmMain";
            this.Text = "Soul Spiral";
            this.pnlButtons.ResumeLayout(false);
            this.gboButtons.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu menuMain;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuOpen;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuExport;
        private System.Windows.Forms.MenuItem mnuExportCurrent;
        private System.Windows.Forms.MenuItem mnuExportAll;
        private System.Windows.Forms.MenuItem mnuTools;
        private System.Windows.Forms.MenuItem mnuOptions;
        private System.Windows.Forms.MenuItem mnuHelp;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tvBigfile;
        private System.Windows.Forms.TextBox txtMain;
        private System.Windows.Forms.MenuItem mnuView;
        private System.Windows.Forms.MenuItem mnuDirectoryNames;
        private System.Windows.Forms.MenuItem mnuRawIndexNames;
        private System.Windows.Forms.MenuItem mnuExportDirectory;
        private System.Windows.Forms.ImageList ilOpenButton;
        private System.Windows.Forms.ImageList ilExportButton;
        private System.Windows.Forms.ImageList ilExportAllButton;
        private System.Windows.Forms.ImageList ilReplaceButton;
        private BenLincoln.UI.HoverImageButton btnOpen;
        private BenLincoln.UI.HoverImageButton btnReplace;
        private BenLincoln.UI.HoverImageButton btnExportAll;
        private BenLincoln.UI.HoverImageButton btnExport;
        private System.Windows.Forms.GroupBox gboButtons;
        private System.Windows.Forms.MenuItem mnuExportIndexData;
        private System.Windows.Forms.MenuItem mnuImport;
        private System.Windows.Forms.MenuItem mnuReplace;
        private BenLincoln.UI.HoverImageButton btnHexEdit;
        private System.Windows.Forms.ImageList ilHexEditButton;
        private System.Windows.Forms.MenuItem mnuHexEdit;
    }
}

