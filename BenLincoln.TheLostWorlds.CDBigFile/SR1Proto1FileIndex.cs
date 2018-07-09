// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2018 Ben Lincoln
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
    public class SR1Proto1FileIndex : BF.FileIndexWithCompressedFiles
    {
        public SR1Proto1FileIndex(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
            : base(name, parentBigFile, parentIndex, offset)
        {
            mCompressedLengthPosition = 2;
        }

        public override void ReadIndex()
        {
            ReadEntries();
            int numFiles = mEntries.GetUpperBound(0) + 1;
            Files = new BF.File[numFiles];
            for (int i = 0; i < numFiles; i++)
            {
                BF.File tFile;
                if (mEntries[i][mCompressedLengthPosition] > 0)
                {
                    tFile = new BF.SR1Proto1CompressedFile(mParentBigFile, this, mEntries[i], mNameHashPosition, mOffsetPosition, mLengthPosition, mCompressedLengthPosition);
                }
                else
                {
                    tFile = new BF.File(mParentBigFile, this, mEntries[i], mNameHashPosition, mOffsetPosition, mLengthPosition);
                }
                Files[i] = tFile;
                if (i > 0)
                {
                    mLoadedPercent = (((float)i / (float)numFiles) * READ_CONTENT_PERCENT) + READ_INDEX_PERCENT;
                }
            }
        }
    }
}
