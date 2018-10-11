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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using BenLincoln.TheLostWorlds.CDBigFile;
using BF = BenLincoln.TheLostWorlds.CDBigFile;
using UI = BenLincoln.UI;
using System.Threading;

namespace SoulSpiral
{
    public partial class frmMain : Form
    {
        protected BF.BigFile mBigFile;
        protected Hashtable mTVLookupTable;
        protected bool mHashLookupFileIsPresent;

        protected BF.BigFile.DirectoryModes mDirectoryMode;
        protected bool mParseNamesFromKnownFileTypes;

        UI.ProgressWindow progressWindow;
        DirectoryExporter mExporter;

        public frmMain()
        {
            InitializeComponent();

            //these options will be replaced by registry key reads
            mnuDirectoryNames.Checked = true;
            mDirectoryMode = BigFile.DirectoryModes.Normal;
            mnuParseNames.Checked = true;
            mParseNamesFromKnownFileTypes = true;
            mnuAlwaysUseHash.Checked = false;
 
            //figure out if there is an ohrainBOWS file to work with
            BF.Fingerprint test = new BF.Fingerprint();
            string dllPath = test.GetType().Assembly.Location;
            int lastSlash = dllPath.LastIndexOf('\\') + 1;
            dllPath = dllPath.Substring(0, lastSlash);
            //if (System.IO.File.Exists(dllPath + "ohrainBOWS.mdb"))
            //{
            //    mHashLookupFileIsPresent = true;
            //}
            //else
            //{
            //    mHashLookupFileIsPresent = false;
            //    //UI.NotificationDialogue noDBFileDialogue = new BenLincoln.UI.NotificationDialogue();
            //    //noDBFileDialogue.SetTitle("No Rainbows");
            //    //noDBFileDialogue.SetMessage("Soul Spiral was unable to find the file 'ohrainBOWS.mdb' in the " +
            //    //    "application folder\r\n" + dllPath + "\r\nReverse hash name lookups will be disabled.");
            //    //noDBFileDialogue.SetIcon(UI.Dialogue.ICON_X);
            //    //noDBFileDialogue.ShowDialog();
            //    MessageBox.Show("Soul Spiral was unable to find the file 'ohrainBOWS.mdb' in the " +
            //        "application folder\r\n" + dllPath + "\r\nReverse hash name lookups will be disabled.",
            //        "No Rainbows",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Exclamation);
            //}

            //set up the tooltips
            CreateToolTip(this.btnOpen, "Open a BigFile");
            CreateToolTip(this.btnExport, "Export the currently-selected file");
            CreateToolTip(this.btnExportAll, "Export all files");
            CreateToolTip(this.btnReplace, "Replace the current file");
            CreateToolTip(this.btnHexEdit, "Hex edit the current file");

            //disable controls if necessary
            DisableMainIOControls();
        }

        protected void CreateProgressWindow()
        {
            if (progressWindow != null)
            {
                progressWindow.Dispose();
            }
            progressWindow = new BenLincoln.UI.ProgressWindow();
            progressWindow.Title = "Loading";
            progressWindow.SetMessage("Reading data from the BigFile");
            progressWindow.Icon = this.Icon;
            progressWindow.Owner = this;
            progressWindow.TopLevel = true;
            progressWindow.ShowInTaskbar = false;
            this.Enabled = false;
            progressWindow.Show();
        }

        protected void DestroyProgressWindow()
        {
            this.Enabled = true;
            progressWindow.Hide();
            progressWindow.Dispose();
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

        protected void DisableMainIOControls()
        {
            this.mnuExport.Enabled = false;
            this.mnuImport.Enabled = false;
            this.mnuReplace.Enabled = false;
            this.mnuHexEdit.Enabled = false;
            this.btnExport.Enabled = false;
            this.btnExportAll.Enabled = false;
            this.btnReplace.Enabled = false;
            this.btnHexEdit.Enabled = false;
        }

        protected void EnableMainIOControls()
        {
            this.mnuExport.Enabled = true;
            this.mnuImport.Enabled = true;
            this.btnOpen.Enabled = true;
        }

        protected void EnableExportAll()
        {
            this.btnExportAll.Enabled = true;
        }

        protected void DisableAllControls()
        {
            DisableMainIOControls();
            this.mnuFile.Enabled = false;
            this.mnuOpen.Enabled = false;
            this.btnOpen.Enabled = false;
            this.mnuView.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
        }

        protected void EnableAllControls()
        {
            EnableMainIOControls();
            this.mnuFile.Enabled = true;
            this.mnuOpen.Enabled = true;
            this.mnuView.Enabled = true;
            this.Cursor = Cursors.Arrow;
        }

        protected void DisableExportCurrent()
        {
            this.btnExport.Enabled = false;
            this.mnuExportCurrent.Enabled = false;
        }

        protected void EnableExportCurrent()
        {
            this.btnExport.Enabled = true;
            this.mnuExportCurrent.Enabled = true;
        }

        protected void DisableExportDirectory()
        {
            this.mnuExportDirectory.Enabled = false;
        }

        protected void EnableExportDirectory()
        {
            this.mnuExportDirectory.Enabled = true;
        }

        protected void DisableReplace()
        {
            this.mnuReplace.Enabled = false;
            this.btnReplace.Enabled = false;
        }

        protected void EnableReplace()
        {
            this.mnuReplace.Enabled = true;
            this.btnReplace.Enabled = true;
        }

        protected void DisableHexEdit()
        {
            this.mnuHexEdit.Enabled = false;
            this.btnHexEdit.Enabled = false;
        }

        protected void EnableHexEdit()
        {
            this.mnuHexEdit.Enabled = true;
            this.btnHexEdit.Enabled = true;
        }

        #region File Open/Export/Replace

        protected void OpenBigFile()
        {
            OpenFileDialog openFile;
            DialogResult result;
            openFile = new OpenFileDialog();
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Title = "Select a Crystal Dynamics game data file to open...";
            //openFile.FileName = "bigfile.dat";
            openFile.DefaultExt = "*.*";
            //openFile.Filter = "Crystal Dynamics game data files (*.BIG, *.BG2, *.BGX, *.DAT, *.MP2, *.MXB, *.PKG, *.RKV)|*.BIG;*.BG2;*.BGX;*.DAT;*.MP2;*.MXB;*.PKG;*.RKVT|All files (*.*)|*.*";
            openFile.Filter = "Crystal Dynamics game data files (*.BGX, *.BIG, *.DAT)|*.BGX;*.BIG;*.DAT|All files (*.*)|*.*";
            result = openFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                StartBigFileLoad(openFile.FileName);
                //ReadBigFile();
            }
        }

        protected void StartBigFileLoad(string fileName)
        {
            mBigFile = new BF.BigFile(fileName);
            mBigFile.DirectoryMode = mDirectoryMode;
            mBigFile.ParseNamesFromKnownFileTypes = mParseNamesFromKnownFileTypes;
            Thread bfrThread = new Thread(new ThreadStart(ReadBigFile));
            bfrThread.Start();
        }

        protected void ReadBigFile()
        {
            Invoke(new MethodInvoker(DisableAllControls));
            bool useAutoResults = false;
            int typeIndex = 0;
            string BigFileTypeName = "";
            string typeMessage = "Soul Spiral was unable to automatically determine this file's type.\r\n" + 
                "Please select the type of BigFile you are opening.";
            if (mBigFile.Fingerprint.Type.MasterIndexType != 0)
            {
                frmRecognizedBigfile recognized = new frmRecognizedBigfile();
                recognized.SetMessage(
                    "Soul Spiral recognizes this file as '" +
                    mBigFile.Fingerprint.GetDisplayName() +
                    ".'\r\n" + 
                    "Do you want to use the automatic settings associated with that type?"
                    );
                typeMessage = "You have chosen to override the automatic settings for this file type.\r\n" + 
                    "Please select the type of BigFile you are opening.";
                for (int i = 0; i < mBigFile.BigFileTypes.BigFileTypeNames.Length; i++)
                {
                    if (mBigFile.Fingerprint.Type == mBigFile.BigFileTypes.BigFileTypeHash[mBigFile.BigFileTypes.BigFileTypeNames[i]])
                    {
                        typeIndex = i;
                        BigFileTypeName = mBigFile.BigFileTypes.BigFileTypeNames[i];
                    }
                }
                recognized.IconImage = SoulSpiral.Properties.Resources.Fingerprint;
                recognized.ShowDialog();
                switch (recognized.Result)
                {
                    case UI.DialogueResult.ABORT_PROCESS:
                        Invoke(new MethodInvoker(EnableAllControls));
                        return;
                        break;
                    case UI.DialogueResult.YES:
                        useAutoResults = true;  
                        break;
                    case UI.DialogueResult.NO:
                        useAutoResults = false;
                        break;
                }
            }
            if (useAutoResults)
            {
                mBigFile.SetType(mBigFile.Fingerprint.Type);
            }
            else
            {
                frmSelectBigfileType selectType = new frmSelectBigfileType();
                selectType.SetDropDownOptions(mBigFile.BigFileTypes);
                selectType.SetMessage(typeMessage);
                selectType.SetSelectedType(typeIndex);
                selectType.IconImage = SoulSpiral.Properties.Resources.Question;
                selectType.ShowDialog();
                switch (selectType.Result)
                {
                    case UI.DialogueResult.ABORT_PROCESS:
                        Invoke(new MethodInvoker(EnableAllControls));
                        return;
                        break;
                    case UI.DialogueResult.YES:
                        string typeName = mBigFile.BigFileTypes.BigFileTypeNames[selectType.SelectedItem];
                        mBigFile.SetType(mBigFile.BigFileTypes.GetTypeByName(typeName));
                        break;
                }
            }

            //proceed with bigfile load

            Invoke(new MethodInvoker(CreateProgressWindow));
            //Invoke(new MethodInvoker(progressWindow.Show));
            //Thread pwThread = new Thread(new ThreadStart(progressWindow.Show));
            //pwThread.Start();
            Thread bfThread = new Thread(new ThreadStart(mBigFile.LoadBigFile));
            //bfThread.Priority = ThreadPriority.BelowNormal;
            bfThread.Start();
            do
            {
                progressWindow.SetProgress(mBigFile.mLoadedPercent);
                progressWindow.SetMessage(mBigFile.LoadState);
                Thread.Sleep(5);
            }
            while (bfThread.IsAlive);
            //pwThread.Abort();
            //mBigFile.LoadBigFile();
            Invoke(new MethodInvoker(DestroyProgressWindow));

            //BuildTreeview();
            Invoke (new MethodInvoker(BuildTreeview));

            //hash table was not loaded even though it should have been, notify user
            if ((mBigFile.HashLookupTable != null) && (mBigFile.HashLookupTable.HashTable.Count == 0))
            {
                //UI.NotificationDialogue noHashRowsDialogue = new BenLincoln.UI.NotificationDialogue();
                //noHashRowsDialogue.SetTitle("No Rainbows");
                //noHashRowsDialogue.SetMessage(
                //    "Soul Spiral can perform reverse hash lookups for the file names in this BigFile,\r\n" + 
                //    "but the appropriate hash list can not be found.\r\n" + 
                //    "You are probably missing a 'Hashes-*.txt' file.");
                //noHashRowsDialogue.SetIcon(UI.Dialogue.ICON_X);
                //noHashRowsDialogue.ShowDialog();
                MessageBox.Show("Soul Spiral can perform reverse hash lookups for the file names in this BigFile, " +
                    "but the appropriate hash list can not be found.\r\n" + 
                    "You are probably missing a 'Hashes-*.txt' file.",
                    "No hash list",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            //if the user manually selected a file type, check to see if there are invalid file references
            //and warn them
            if (!useAutoResults)
            {
                Invoke(new MethodInvoker(CheckAllFilesInDirectory));
            }
            //Invoke(new MethodInvoker(EnableMainIOControls));
            Invoke(new MethodInvoker(EnableAllControls));
            Invoke(new MethodInvoker(EnableExportAll));
            //EnableMainIOControls();
            //MessageBox.Show("Done");
        }

        protected void CheckAllFilesInDirectory()
        {
            int badRefCount = CheckFilesRecursive(mBigFile.MasterDirectory);
            if (badRefCount > 0)
            {
                MessageBox.Show
                (
                "Using the specified file type '" + mBigFile.Type.Description + "', " + 
                "Soul Spiral encountered " + badRefCount + " invalid file references.\r\n" + 
                "This usually means that the BigFile is not of that type, or that it is a variant " + 
                "which is not currently interpreted correctly.",
                "Error Reading References",
                MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
        }

        protected int CheckFilesRecursive(BF.Directory xDir)
        {
            int retCount = 0;
            if (xDir.Directories.Count > 0)
            {
                foreach (BF.Directory subDir in xDir.Directories)
                {
                    retCount += CheckFilesRecursive(subDir);
                }
            }

            if (xDir.Files.Count > 0)
            {
                foreach (BF.File currentFile in xDir.Files)
                {
                    if (!currentFile.IsValidReference)
                    {
                        retCount++;
                    }
                }
            }

            return retCount;
        }

        #endregion

 
        //build the treeview based on the main directory from the bigfile
        //eventually this will switch between raw and normal modes, just raw for now.
        public void BuildTreeview()
        {
            tvBigfile.Nodes.Clear();
            mTVLookupTable = new Hashtable();
            tvBigfile.Nodes.Add(getIndexNodes(mBigFile.MasterDirectory, "\\"));
        }

        public TreeNode getIndexNodes(BF.Directory currentDir, string currentPath)
        {
            TreeNode returnNode = new TreeNode(currentDir.Name);
            mTVLookupTable.Add(currentPath, currentDir);

            if ((currentDir.Directories != null) && (currentDir.Directories.Count > 0))
            {
                foreach (BF.Directory nextDir in currentDir.Directories)
                {
                    returnNode.Nodes.Add(getIndexNodes(nextDir, currentPath + nextDir.Name + "\\"));
                }
            }

            if ((currentDir.Files != null) && (currentDir.Files.Count > 0))
            {
                foreach (BF.File currentFile in currentDir.Files)
                {
                    returnNode.Nodes.Add(currentFile.Name + "." + currentFile.FileExtension);
                    mTVLookupTable.Add(currentPath + currentFile.Name + "." + currentFile.FileExtension, currentFile);
                }
            }

            return returnNode;
        }

        #region Menu/Button Functions

        protected void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenBigFile();
        }

        protected void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuDirectoryNames_Click(object sender, EventArgs e)
        {
            mnuDirectoryNames.Checked = true;
            mnuRawIndexNames.Checked = false;
            mDirectoryMode = BF.BigFile.DirectoryModes.Normal;
            if ((mBigFile != null) && (mBigFile.MasterDirectory != null))
            {
                PromptUserToReload();
            }
        }

        private void mnuRawIndexNames_Click(object sender, EventArgs e)
        {
            mnuDirectoryNames.Checked = false;
            mnuRawIndexNames.Checked = true;
            mDirectoryMode = BF.BigFile.DirectoryModes.Raw;
            if ((mBigFile != null) && (mBigFile.MasterDirectory != null))
            {
                PromptUserToReload();
            }
        }

        private void mnuParseNames_Click(object sender, EventArgs e)
        {
            mnuParseNames.Checked = true;
            mParseNamesFromKnownFileTypes = true;
            mnuAlwaysUseHash.Checked = false;
            if ((mBigFile != null) && (mBigFile.MasterDirectory != null))
            {
                PromptUserToReload();
            }
        }

        private void mnuAlwaysUseHash_Click(object sender, EventArgs e)
        {
            mnuParseNames.Checked = false;
            mParseNamesFromKnownFileTypes = false;
            mnuAlwaysUseHash.Checked = true;
            if ((mBigFile != null) && (mBigFile.MasterDirectory != null))
            {
                PromptUserToReload();
            }
        }

        protected void PromptUserToReload()
        {
            DialogResult result = MessageBox.Show
            (
                "The change you have made will not take effect until you reload the current BigFile.\r\n" + 
                "Would you like to do that now?",
                "Reload BigFile?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                StartBigFileLoad(mBigFile.Path);
            }
        }

        protected void mnuExportCurrent_Click(object sender, EventArgs e)
        {
            ExportCurrentFile();
        }

        private void mnuExportDirectory_Click(object sender, EventArgs e)
        {
            ExportCurrentDirectory();
        }

        protected void mnuExportAll_Click(object sender, EventArgs e)
        {
            ExportAll();
        }

        private void mnuExportIndexData_Click(object sender, EventArgs e)
        {
            ExportIndexData();
        }

        private void mnuReplace_Click(object sender, EventArgs e)
        {
            ReplaceCurrentFile();
        }

        protected void mnuOptions_Click(object sender, EventArgs e)
        {

        }

        private void mnuHexEdit_Click(object sender, EventArgs e)
        {
            LaunchHexEditor();
        }

        protected void mnuAbout_Click(object sender, EventArgs e)
        {
            frmAbout aboutWindow = new frmAbout();
            aboutWindow.ShowDialog();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenBigFile();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportCurrentFile();
        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            ExportAll();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ReplaceCurrentFile();
        }

        private void btnHexEdit_Click(object sender, EventArgs e)
        {
            LaunchHexEditor();
        }


        #endregion

        #region File-browsing

        private void tvBigfile_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string info = GetSelectedItemInfo(tvBigfile);
            if (info != null)
            {
                txtMain.Text = info;
            }
        }

        protected string GetSelectedItemInfo(TreeView tv)
        {
            string fullPath = tv.SelectedNode.FullPath;
            int padLength = tv.Nodes[0].FullPath.Length;
            fullPath = fullPath.Substring(padLength);

            //return nothing if nothing is selected
            if (tv.SelectedNode == null)
            {
                //MessageBox.Show("No item is currently selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisableExportCurrent();
                DisableExportDirectory();
                DisableReplace();
                DisableHexEdit();
                return null;
            }
            if ((mTVLookupTable != null) && (fullPath.Trim() != "") && (mTVLookupTable.Contains(fullPath)) && (tv.SelectedNode.GetNodeCount(false) == 0))
            {
                //a file is selected
                BF.File selectedFile = (BF.File)mTVLookupTable[fullPath];
                EnableExportCurrent();
                DisableExportDirectory();
                if ((selectedFile != null) && (selectedFile.CanBeReplaced))
                {
                    EnableReplace();
                    EnableHexEdit();
                }
                else
                {
                    DisableReplace();
                    DisableHexEdit();
                }
                return selectedFile.GetInfo();
            }
            else
            {
                //a node is selected, so either return information about the entire bigfile or the currently-selected 
                //directory
                DisableExportCurrent();
                EnableExportDirectory();
                DisableReplace();
                DisableHexEdit();
                if (fullPath.Contains("\\"))
                {
                    BF.Directory selectedDir = (BF.Directory)mTVLookupTable[fullPath + "\\"];
                    return selectedDir.GetInfo();
                }
                else
                {
                    return mBigFile.GetInfo();
                }
            }

            return null;
        }

        #endregion

        #region Import/Export

        protected BF.Directory GetCurrentDirectory(TreeView tv)
        {
            //return nothing if nothing is selected
            if (tv.SelectedNode == null)
            {
                //MessageBox.Show("No item is currently selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string fullPath = tv.SelectedNode.FullPath;
            int padLength = tv.Nodes[0].FullPath.Length;
            fullPath = fullPath.Substring(padLength);

            if (tv.SelectedNode.GetNodeCount(false) != 0)
            {
                //a directory is selected
                if (fullPath.Contains("\\"))
                {
                    return (BF.Directory)mTVLookupTable[fullPath + "\\"];
                }
                else
                {
                    return mBigFile.MasterDirectory;
                }
            }
            else
            {
                //file is selected, so we need to trim off its name and get the current directory
                int lastSlash = fullPath.LastIndexOf('\\');
                fullPath = fullPath.Substring(0, lastSlash);
                return (BF.Directory)mTVLookupTable[fullPath + "\\"];
            }

            return null;
        }

        protected BF.File GetSelectedFile(TreeView tv)
        {
            //return nothing if nothing is selected
            if (tv.SelectedNode == null)
            {
                return null;
            }

            string fullPath = tv.SelectedNode.FullPath;
            int padLength = tv.Nodes[0].FullPath.Length;
            fullPath = fullPath.Substring(padLength);

            if (tv.SelectedNode.GetNodeCount(false) == 0)
            {
                //a file is selected
                BF.File selectedFile = (BF.File)mTVLookupTable[fullPath];
                return selectedFile;
            }

            return null;
        }

        protected void ExportCurrentFile()
        {
            BF.File xFile;
            SaveFileDialog sDialogue;
            DialogResult result;

            xFile = GetSelectedFile(tvBigfile);
            if (xFile == null)
            {
                //UI.NotificationDialogue errorDialogue = new BenLincoln.UI.NotificationDialogue();
                //errorDialogue.SetTitle("Export Error");
                //errorDialogue.SetMessage("No file is currently selected.");
                //errorDialogue.SetIcon(UI.Dialogue.ICON_X);
                //errorDialogue.ShowDialog();
                MessageBox.Show("No file is currently selected.",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            sDialogue = new SaveFileDialog();
            sDialogue.AddExtension = true;
            sDialogue.DefaultExt = "*." + xFile.FileExtension;
            sDialogue.Filter = xFile.Type.Name + " (*." + xFile.FileExtension + ")|*." + xFile.FileExtension;
            sDialogue.FileName = xFile.Name + "." + xFile.FileExtension;
            sDialogue.OverwritePrompt = true;
            sDialogue.Title = "Export Item...";

            result = sDialogue.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    xFile.Export(sDialogue.FileName);
                }
                catch (Exception ex)
                {
                    //UI.NotificationDialogue errorDialogue = new BenLincoln.UI.NotificationDialogue();
                    //errorDialogue.SetTitle("Export Error");
                    //errorDialogue.SetMessage("An error occurred while exporting the file '" + xFile.Name + ".'\r\n" + 
                    //    "The specific error message was:\r\n" + ex.Message);
                    //errorDialogue.SetIcon(UI.Dialogue.ICON_X);
                    //errorDialogue.ShowDialog();
                    MessageBox.Show("An error occurred while exporting the file '" + xFile.Name + ".'\r\n" + 
                        "The specific error message was:\r\n" + ex.Message,
                        "Export Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                //UI.NotificationDialogue doneDialogue = new BenLincoln.UI.NotificationDialogue();
                //doneDialogue.SetTitle("Export Complete");
                //doneDialogue.SetMessage("Done");
                //doneDialogue.SetIcon(UI.Dialogue.ICON_EXCLAMATION);
                //doneDialogue.ShowDialog();
                MessageBox.Show("Export Complete",
                    "Done",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        protected void ExportCurrentDirectory()
        {
            BF.Directory currentDir = GetCurrentDirectory(tvBigfile);
            if (currentDir == null)
            {
                MessageBox.Show("No directory is currently selected.",
                   "Export Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return;
            }
            else
            {
                ExportDirectoryConfirm(currentDir);
            }
        }

        protected void ExportAll()
        {
            ExportDirectoryConfirm(mBigFile.MasterDirectory);
        }

        protected void ExportDirectoryConfirm(BF.Directory exportDir)
        {
            string tPath;
            FolderBrowserDialog fDialogue;
            DialogResult result1, result2;
            fDialogue = new FolderBrowserDialog();
            fDialogue.ShowNewFolderButton = true;
            result1 = fDialogue.ShowDialog();

            if (result1 == DialogResult.OK)
            {
                tPath = fDialogue.SelectedPath;
                result2 = MessageBox.Show("Any files in the selected folder with names identical to those being exported will be overwritten.\n" +
                    "Are you sure you want to proceed?", "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result2 == DialogResult.Yes)
                {
                    mExporter = new DirectoryExporter(exportDir, tPath);
                    Thread xThread = new Thread(new ThreadStart(ExportDirectory));
                    xThread.Start();
                }
            }
        }

        protected void ExportDirectory()
        {
            Invoke(new MethodInvoker(DisableAllControls));
            Invoke(new MethodInvoker(CreateProgressWindow));
            progressWindow.SetTitle("Exporting");
            Thread x2Thread = new Thread(new ThreadStart(mExporter.DoExport));
            x2Thread.Start();
            do
            {
                progressWindow.SetProgress((int)((float)mExporter.mCurrentFile / (float)mExporter.mTotalFiles * 100));
                progressWindow.SetMessage("Exporting file " + mExporter.mCurrentFile + " of " + mExporter.mTotalFiles);
                Thread.Sleep(5);
            }
            while (x2Thread.IsAlive);
            Invoke(new MethodInvoker(DestroyProgressWindow));
            Invoke(new MethodInvoker(EnableAllControls));
            Invoke(new MethodInvoker(EnableExportAll));
        }


        protected void ExportIndexData()
        {
            if (mBigFile.MasterIndex == null)
            {
                MessageBox.Show("This BigFile does not appear to have a valid master index.",
                   "Export Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog sDialogue;
            DialogResult result;

            sDialogue = new SaveFileDialog();
            sDialogue.AddExtension = true;
            sDialogue.DefaultExt = "*.CSV";
            sDialogue.Filter = "Text File With Comma-Separated Values (*.CSV)|*.CSV";
            sDialogue.OverwritePrompt = true;
            sDialogue.Title = "Export Index Data...";

            result = sDialogue.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    //create the new output file
                    FileStream oStream;
                    StreamWriter oWriter;

                    //open the file stream and enter the name of the current index
                    oStream = new FileStream(sDialogue.FileName, FileMode.Create, FileAccess.Write);
                    oWriter = new StreamWriter(oStream);

                    oWriter.WriteLine("Raw index data for " + mBigFile.Path);

                    oWriter.Close();
                    oStream.Close();

                    ExportIndexRecursive(mBigFile.MasterIndex, sDialogue.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while exporting the index data.\r\n" +
                        "The specific error message was:\r\n" + ex.Message,
                        "Export Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                MessageBox.Show("Export Complete",
                    "Done",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        protected void ExportIndexRecursive(BF.Index whichIndex, string targetPath)
        {
            FileStream oStream;
            StreamWriter oWriter;

            //open the file stream and enter the name of the current index
            oStream = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
            oWriter = new StreamWriter(oStream);

            //if the current index has a separate table of hashes, dump that first
            bool hasSeparateHashes = true;
            try
            {
                BF.FileIndexWithSeparateHashes testCast = (BF.FileIndexWithSeparateHashes)whichIndex;
            }
            catch (InvalidCastException ex)
            {
                hasSeparateHashes = false;
            }

            if (hasSeparateHashes)
            {
                BF.FileIndexWithSeparateHashes hashIndex = (BF.FileIndexWithSeparateHashes)whichIndex;
                oWriter.WriteLine("Filename Hashes");
                oWriter.WriteLine("Entry Number,Hex,Dec");
                int hashNum = 0;
                foreach (string currentHash in hashIndex.Hashes)
                {
                    string info = hashNum.ToString() + ",";
                    //info += string.Format("{0:X8},", currentHash);
                    //info += string.Format("{0:000000000000}", currentHash);
                    info += currentHash;
                    oWriter.WriteLine(info);
                    hashNum++;
                }
            }

            //dump the raw contents of the current index
            oWriter.WriteLine("Index Name, Entry Number, Values in Hex, Values in Dec");
            int entryNum = 0;
            foreach (uint[] currentEntry in whichIndex.Entries)
            {
                string info = whichIndex.Name + "," + entryNum + ",";
                foreach (uint currentValue in currentEntry)
                {
                    info += string.Format("{0:X8},", currentValue);
                }
                info += " ,";
                foreach (uint currentValue in currentEntry)
                {
                    info += string.Format("{0:000000000000},", currentValue);
                }
                oWriter.WriteLine(info);
                entryNum++;
            }

            oWriter.Close();
            oStream.Close();

            //if this index has subindices, process them
            if ((whichIndex.Indices != null) && (whichIndex.Indices.GetUpperBound(0) > 0))
            {
                foreach (BF.Index nextIndex in whichIndex.Indices)
                {
                    ExportIndexRecursive(nextIndex, targetPath);
                }
            }
        }

        protected void ReplaceCurrentFile()
        {
            BF.File xFile;
            OpenFileDialog oDialogue;
            DialogResult result;

            xFile = GetSelectedFile(tvBigfile);
            if (xFile == null)
            {
                //UI.NotificationDialogue errorDialogue = new BenLincoln.UI.NotificationDialogue();
                //errorDialogue.SetTitle("Export Error");
                //errorDialogue.SetMessage("No file is currently selected.");
                //errorDialogue.SetIcon(UI.Dialogue.ICON_X);
                //errorDialogue.ShowDialog();
                MessageBox.Show("No file is currently selected.",
                    "Replace Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            if (!xFile.CanBeReplaced)
            {

                MessageBox.Show("The selected file is of a type that cannot be replaced.",
                    "Replace Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            oDialogue = new OpenFileDialog();
            oDialogue.Filter = "All Files (*.*)|*.*";
            oDialogue.Title = "Replace Item...";

            result = oDialogue.ShowDialog();

            if (result == DialogResult.OK)
            {
                DialogResult result2 = MessageBox.Show("Replacing a file cannot be undone.\r\n" +
                    "This action will permanently alter the BigFile that is currently open.\n" +
                    "It is highly recommended that you make a backup of the BigFile before using this function.\n" +
                    "Are you sure you want to proceed?", "Replace", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result2 == DialogResult.Yes)
                {
                    try
                    {
                        xFile.Replace(oDialogue.FileName);
                        MessageBox.Show("Replace Complete",
                           "Done",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
                    }
                    catch (FileSizeMismatchException fEx)
                    {
                        MessageBox.Show("Only files with identical sizes can be replaced.",
                            "Replace Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while replacing the file '" + xFile.Name + ".'\r\n" +
                            "The specific error message was:\r\n" + ex.Message,
                            "Replace Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        protected void LaunchHexEditor()
        {
            BF.File xFile;
            SaveFileDialog sDialogue;
            DialogResult result;

            xFile = GetSelectedFile(tvBigfile);
            if (xFile == null)
            {
                //UI.NotificationDialogue errorDialogue = new BenLincoln.UI.NotificationDialogue();
                //errorDialogue.SetTitle("Export Error");
                //errorDialogue.SetMessage("No file is currently selected.");
                //errorDialogue.SetIcon(UI.Dialogue.ICON_X);
                //errorDialogue.ShowDialog();
                MessageBox.Show("No file is currently selected.",
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            frmHexEdit hexEdit = new frmHexEdit();
            hexEdit.LoadFile(mBigFile.Path, xFile, false);
            hexEdit.Show();
        }

        protected byte[] sanitizeByteArray(byte[] inArray, byte defaultChar)
        {
            byte[] outArray;
            outArray = new byte[inArray.GetUpperBound(0) + 1];

            //sanitize name
            for (int s = 0; s <= inArray.GetUpperBound(0); s++)
            {
                //non-printable ASCII chars
                if (inArray[s] < 32)
                {
                    outArray[s] = defaultChar;
                }
                else if (inArray[s] > 126)
                {
                    outArray[s] = defaultChar;
                }
                //still valid
                else
                {
                    outArray[s] = inArray[s];
                }
            }
            return outArray;
        }

        #endregion



    }
}