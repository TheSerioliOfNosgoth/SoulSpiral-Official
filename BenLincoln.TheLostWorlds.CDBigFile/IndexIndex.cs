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
using System.Threading;
using System.IO;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class IndexIndex : BF.Index
    {
        protected int mSubIndexType;

        #region Properties

        public int SubIndexType
        {
            get
            {
                return mSubIndexType;
            }
            set
            {
                mSubIndexType = value;
            }
        }

        #endregion

        public IndexIndex(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
            : base(name, parentBigFile, parentIndex, offset)
        {
            mSubIndexType = 0;
        }

        public override void ReadIndex()
        {
            ReadEntries();
            int numIndices = mEntries.GetUpperBound(0) + 1;
            Indices = new BF.Index[numIndices];
            for (int i = 0; i <= mEntries.GetUpperBound(0); i++)
            {
                BF.FileIndex tIndex = (BF.FileIndex)BF.Index.CreateIndex(mParentBigFile, mSubIndexType);
                tIndex.Name = "Sub-Index-" + string.Format("{0:D3}", i);
                tIndex.RawIndexData = mEntries[i];
                tIndex.Offset = mEntries[i][mOffsetPosition];
                //read subindex
                if (i > 0)
                {
                    mLoadedPercent = (((float)i / (float)mEntries.GetUpperBound(0)) * READ_CONTENT_PERCENT) + READ_INDEX_PERCENT;
                }
                //launch another sub-thread so that the loaded percent stays updated
                Thread siThread = new Thread(new ThreadStart(tIndex.ReadIndex));
                siThread.Start();
                do
                {
                    Thread.Sleep(1);
                }
                while (siThread.IsAlive);
                //tIndex.ReadIndex();
                Indices[i] = tIndex;
                mFileCount += tIndex.FileCount;
            }
        }
    }
}
