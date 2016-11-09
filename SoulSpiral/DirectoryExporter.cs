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
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public class DirectoryExporter
    {
        protected string mTargetPath;
        protected BF.Directory mSourceDirectory;
        public int mTotalFiles;
        public int mCurrentFile;

        public DirectoryExporter(BF.Directory sourceDir, string targetPath)
        {
            mSourceDirectory = sourceDir;
            mTargetPath = targetPath;
            mTotalFiles = mSourceDirectory.FileCountRecursive;
        }

        public void DoExport()
        {
            mCurrentFile = 0;
            ExportDirectoryRecursive(mSourceDirectory, mTargetPath);
        }

        protected void ExportDirectoryRecursive(BF.Directory xDir, string targetPath)
        {
            if (!(System.IO.Directory.Exists(targetPath)))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }
            if (xDir.Directories.Count > 0)
            {
                foreach (BF.Directory subDir in xDir.Directories)
                {
                    ExportDirectoryRecursive(subDir, targetPath + "\\" + subDir.Name);
                }
            }

            if (xDir.Files.Count > 0)
            {
                foreach (BF.File currentFile in xDir.Files)
                {
                    currentFile.Export(targetPath + "\\" + currentFile.Name + "." + currentFile.FileExtension);
                    mCurrentFile++;
                }
            }
        }

    }
}
