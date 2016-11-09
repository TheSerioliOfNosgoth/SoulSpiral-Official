// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2012 Ben Lincoln
// http://www.thelostworlds.net/
//

// This file is part of BenLincoln.TheLostWorlds.CDBigFile.

// BenLincoln.TheLostWorlds.CDBigFile is free software: you can redistribute it and/or modify
// it under the terms of version 3 of the GNU General Public License as published by
// the Free Software Foundation.

// BenLincoln.TheLostWorlds.CDBigFile is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with BenLincoln.TheLostWorlds.CDBigFile (in the file LICENSE.txt).  
// If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class Directory : IComparable
    {
        protected string mName;
        protected BF.BigFile mParentBigFile;
        protected BF.Directory mParentDirectory;
        protected ArrayList mDirectories;
        protected ArrayList mFiles;
        protected Hashtable mDirectoryNames;
        protected Hashtable mFilenames;
        protected int mFileCountRecursive;

        #region Properties

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public BF.BigFile ParentBigFile
        {
            get
            {
                return mParentBigFile;
            }
            set
            {
                mParentBigFile = value;
            }
        }

        public BF.Directory ParentDirectory
        {
            get
            {
                return mParentDirectory;
            }
            set
            {
                mParentDirectory = value;
            }
        }

        public ArrayList Directories
        {
            get
            {
                return mDirectories;
            }
        }

        public ArrayList Files
        {
            get
            {
                return mFiles;
            }
        }

        public Hashtable DirectoryNames
        {
            get
            {
                return mDirectoryNames;
            }
            set
            {
                mDirectoryNames = value;
            }
        }

        public Hashtable Filenames
        {
            get
            {
                return mFilenames;
            }
            set
            {
                mFilenames = value;
            }
        }

        public int FileCountRecursive
        {
            get
            {
                return mFileCountRecursive;
            }
            set
            {
                mFileCountRecursive = value;
            }
        }

        #endregion

        public Directory()
        {
            mName = "New Directory";
            mParentBigFile = null;
            mParentDirectory = null;
            mDirectories = new ArrayList();
            mFiles = new ArrayList();
            mFilenames = new Hashtable();
            mDirectoryNames = new Hashtable();
            mFileCountRecursive = 0;
        }

        public void SortAll()
        {
            //sort the current directory and all subdirectories
            Directories.Sort();
            mFileCountRecursive = 0;
            foreach (BF.Directory subDir in Directories)
            {
                subDir.SortAll();
                mFileCountRecursive += subDir.FileCountRecursive;
            }
            Files.Sort();
            mFileCountRecursive += Files.Count;
        }

        public void AddDirectory(BF.Directory whichDir)
        {
            mDirectories.Add(whichDir);
            mDirectoryNames.Add(whichDir.Name, mDirectories.Count - 1);
            mFileCountRecursive += whichDir.FileCountRecursive;
        }

        public void AddFile(BF.File whichFile)
        {
            if (mFilenames.Contains(whichFile.Name + "." + whichFile.FileExtension))
            {
                whichFile.Name = GetNextUnusedName(mFilenames, whichFile);
            }
            mFiles.Add(whichFile);
            mFilenames.Add(whichFile.Name + "." + whichFile.FileExtension, mFiles.Count - 1);
        }

        protected string GetNextUnusedName(Hashtable currentList, BF.File currentFile)
        {
            int number = 0;
            string newName = currentFile.Name;
            do
            {
                newName = currentFile.Name + "-duplicate-" + string.Format("{0:000}", number);
                number++;
            } while (currentList.Contains(newName + "." + currentFile.FileExtension));

            return newName;
        }

        //for sorting
        public int CompareTo(object compObj)
        {
            if (!(compObj is BF.Directory))
            {
                throw new InvalidCastException("Can't compare BigFile Directory class with other classes.");
            }
            BF.Directory compDir = (BF.Directory)compObj;
            return this.Name.CompareTo(compDir.Name);
        }

        public virtual string GetInfo()
        {
            return
                "Directory Information\r\n---\r\n" +
                "Name: " + mName + "\r\n" +
                "Subdirectories: " + mDirectories.Count.ToString() + "\r\n" +
                "Files (at this level): " + mFiles.Count.ToString() + "\r\n" +
                "Files (including subfolders): " + mFileCountRecursive.ToString() + "\r\n";
        }

    }
}
