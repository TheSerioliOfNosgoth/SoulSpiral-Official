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

        protected string ReplaceInvalidPathCharacters(string inputPath, bool replaceBackslash)
        {
            string result = inputPath;

            result = result.Replace(":", "[colon]");
            result = result.Replace("/", "[forward_slash]");

            if (replaceBackslash)
            {
                result = result.Replace("\\", "[backslash]");
            }

            return result;
        }

        protected void ExportDirectoryRecursive(BF.Directory xDir, string targetPath)
        {
            //string realTargetPath = ReplaceInvalidPathCharacters(targetPath, false);
            string realTargetPath = targetPath;
            if (!(System.IO.Directory.Exists(realTargetPath)))
            {
                System.IO.Directory.CreateDirectory(realTargetPath);
            }
            if (xDir.Directories.Count > 0)
            {
                foreach (BF.Directory subDir in xDir.Directories)
                {
                    string subDirName = ReplaceInvalidPathCharacters(subDir.Name, true);
                    ExportDirectoryRecursive(subDir, Path.Combine(targetPath, subDirName));
                }
            }

            if (xDir.Files.Count > 0)
            {
                foreach (BF.File currentFile in xDir.Files)
                {
                    string currentFileName = ReplaceInvalidPathCharacters(currentFile.Name + "." + currentFile.FileExtension, true);
                    currentFile.Export(Path.Combine(targetPath, currentFileName));
                    mCurrentFile++;
                }
            }
        }

    }
}
