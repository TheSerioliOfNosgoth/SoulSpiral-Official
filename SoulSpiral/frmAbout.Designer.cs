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
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSite1 = new System.Windows.Forms.Label();
            this.lblCreator = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblJohnDoom1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(252, 347);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "OK";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSite1
            // 
            this.lblSite1.Location = new System.Drawing.Point(20, 234);
            this.lblSite1.Name = "lblSite1";
            this.lblSite1.Size = new System.Drawing.Size(560, 23);
            this.lblSite1.TabIndex = 16;
            this.lblSite1.Text = "https://www.thelostworlds.net/";
            this.lblSite1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCreator
            // 
            this.lblCreator.Location = new System.Drawing.Point(20, 210);
            this.lblCreator.Name = "lblCreator";
            this.lblCreator.Size = new System.Drawing.Size(560, 23);
            this.lblCreator.TabIndex = 15;
            this.lblCreator.Text = "Copyright 2006-2018 Ben Lincoln";
            this.lblCreator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(20, 186);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(560, 23);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "Version 0.2 (Beta Release)";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(20, 162);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(560, 23);
            this.lblProduct.TabIndex = 13;
            this.lblProduct.Text = "A tool for extracting the contents of Crystal Dynamics game data files";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imgLogo
            // 
            this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
            this.imgLogo.Location = new System.Drawing.Point(12, 12);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(569, 138);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgLogo.TabIndex = 12;
            this.imgLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(569, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Hex Editor panel component and numerous other contributions by Andrew Fradley (ht" +
    "tps://github.com/afradley/)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(560, 23);
            this.label2.TabIndex = 19;
            this.label2.Text = "Additional contributions by Eoghan Driver, and Alexey Kragin";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblJohnDoom1
            // 
            this.lblJohnDoom1.Location = new System.Drawing.Point(12, 290);
            this.lblJohnDoom1.Name = "lblJohnDoom1";
            this.lblJohnDoom1.Size = new System.Drawing.Size(569, 23);
            this.lblJohnDoom1.TabIndex = 20;
            this.lblJohnDoom1.Text = "Blood Omen 2 support based on code by John Doom (http://veryitalianproject.alterv" +
    "ista.org/downloads/tools.php)";
            this.lblJohnDoom1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 382);
            this.Controls.Add(this.lblJohnDoom1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblSite1);
            this.Controls.Add(this.lblCreator);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.imgLogo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(609, 420);
            this.Name = "frmAbout";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Soul Spiral";
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Label lblSite1;
        internal System.Windows.Forms.Label lblCreator;
        internal System.Windows.Forms.Label lblVersion;
        internal System.Windows.Forms.Label lblProduct;
        internal System.Windows.Forms.PictureBox imgLogo;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label lblJohnDoom1;
    }
}