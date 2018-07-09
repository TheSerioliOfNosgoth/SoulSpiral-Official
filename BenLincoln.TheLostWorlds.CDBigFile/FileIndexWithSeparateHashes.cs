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
using System.IO;
using BF = BenLincoln.TheLostWorlds.CDBigFile;
using BLD = BenLincoln.Data;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class FileIndexWithSeparateHashes : BF.FileIndex
    {
        protected string[] mHashes;
        protected int mHashOffset = 0;

        #region Properties

        public string[] Hashes
        {
            get
            {
                return mHashes;
            }
            set
            {
                mHashes = value;
            }
        }

        public int HashOffset
        {
            get
            {
                return mHashOffset;
            }
            set
            {
                mHashOffset = value;
            }
        }

        #endregion

        public FileIndexWithSeparateHashes(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
            : base(name, parentBigFile, parentIndex, offset)
        {
        }

        public override void ReadIndex()
        {
            ReadEntries();
            int numFiles = mEntries.GetUpperBound(0) + 1;
            Files = new BF.File[numFiles];
            for (int i = 0; i < numFiles; i++)
            {
                BF.File tFile = new BF.File(mParentBigFile, this, mEntries[i], mHashes[i], mOffsetPosition, mLengthPosition);
                Files[i] = tFile;
                if (i > 0)
                {
                    mLoadedPercent = (((float)i / numFiles) * READ_CONTENT_PERCENT) + READ_INDEX_PERCENT;
                }
            }
        }

        protected override void ReadEntries()
        {
            string[] hashes;
            uint[][] entries;
            FileStream iStream;
            BinaryReader iReader;

            try
            {
                iStream = new FileStream(ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                iReader = new BinaryReader(iStream);
                iStream.Seek(Offset, SeekOrigin.Begin);

                //get the number of entries in the index
                int numEntries = iReader.ReadUInt16();
                //proceed to read the rest of the index - 4 bytes past the length indicator
                iStream.Seek(Offset + mFirstEntryOffset, SeekOrigin.Begin);
                hashes = new string[numEntries];
                for (int i = 0; i < numEntries; i++)
                {
                    hashes[i] = BLD.HexConverter.ByteArrayToHexString(BLD.BinaryConverter.UIntToByteArray(iReader.ReadUInt32()));
                }

                entries = new uint[numEntries][];
                for (int i = 0; i < numEntries; i++)
                {
                    entries[i] = new uint[mEntryLength];
                    for (int j = 0; j < mEntryLength; j++)
                    {
                        entries[i][j] = iReader.ReadUInt32();
                    }
                }

                iReader.Close();
                iStream.Close();
            }
            catch (Exception ex)
            {
                throw new BigFileIndexReadException
                    ("Failed to read index " + Name + "\r\n" +
                    "The specific error message is: \r\n" +
                    ex.Message
                    );
            }
            mHashes = hashes;
            mEntries = entries;
        }

    }
}
