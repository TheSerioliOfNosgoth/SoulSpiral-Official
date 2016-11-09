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
using System.Collections.Generic;
using System.Text;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileType
    {
        protected string mName;
        protected string mDescription;
        protected int mMasterIndexType;
        protected BF.FileType[] mFileTypes;
        protected BF.FlatFileHashLookupTable mHashLookupTable;

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

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        public int MasterIndexType
        {
            get
            {
                return mMasterIndexType;
            }
            set
            {
                mMasterIndexType = value;
            }
        }

        public BF.FileType[] FileTypes
        {
            get
            {
                return mFileTypes;
            }
            set
            {
                mFileTypes = value;
            }
        }

        public BF.FlatFileHashLookupTable HashLookupTable
        {
            get
            {
                return mHashLookupTable;
            }
            set
            {
                mHashLookupTable = value;
            }
        }

        #endregion

        public BigFileType()
        {
            mName = "Unknown";
            mDescription = "Unknown BigFile type";
            mMasterIndexType = 0;
            mHashLookupTable = null;
        }

    }
}
