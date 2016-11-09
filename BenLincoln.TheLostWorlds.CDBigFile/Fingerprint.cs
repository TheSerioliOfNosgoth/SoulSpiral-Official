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
    public class Fingerprint
    {
        protected string mName;
        protected long mFileSize;
        protected BF.BigFileType mType;

        #region Properties
 
        public long FileSize
        {
            get
            {
                return mFileSize;
            }
            set
            {
                mFileSize = value;
            }
        }

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

        public BF.BigFileType Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        #endregion

        public Fingerprint()
        {
            Name = "Unrecognized File";
            FileSize = 0;
        }

        //if the file size has not been specified, treat this as a generic "unrecognized" fingerprint
        public virtual bool CheckBigFileForMatch(BF.BigFile compareFile)
        {
            if (FileSize == 0)
            {
                return true;
            }
            else
            {
                if (FileSize == compareFile.FileSize)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
