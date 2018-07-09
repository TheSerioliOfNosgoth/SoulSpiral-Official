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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class PandemoniumFileIndex : BF.FileIndex
    {
        public PandemoniumFileIndex(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
            : base(name, parentBigFile, parentIndex, offset)
        {
        }

        protected override void ReadEntries()
        {
            uint[][] entries;
            FileStream iStream;
            BinaryReader iReader;

            try
            {
                iStream = new FileStream(ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                iReader = new BinaryReader(iStream);
                iStream.Seek(0, SeekOrigin.Begin);

                int numEntries = 0;

                bool doneReading = false;

                uint lastOffset = 0;

                ArrayList entryList = new ArrayList();

                while (!doneReading)
                {
                    bool gotOffset = false;
                    uint offset = 0;
                    uint hash = 0;

                    while (!gotOffset)
                    {
                        offset = iReader.ReadUInt32();
                        if (offset == 0xFFFFFFFF)
                        {
                            // ignore
                        }
                        else
                        {
                            if (offset == 0)
                            {
                                doneReading = true;
                            }
                            if (offset < lastOffset)
                            {
                                doneReading = true;
                            }
                            if (offset > iStream.Length)
                            {
                                doneReading = true;
                            }
                            gotOffset = true;
                        }
                    }

                    if (!doneReading)
                    {
                        lastOffset = offset;
                        hash = iReader.ReadUInt32();
                        if (Endianness == BenLincoln.Data.Endianness.Big)
                        {
                            offset = BenLincoln.Data.EndianConverter.ReverseUInt(offset);
                            hash = BenLincoln.Data.EndianConverter.ReverseUInt(hash);
                        }
                        uint[] currentEntry = new uint[] { offset, hash };
                        entryList.Add(currentEntry);

                        numEntries++;
                        if (numEntries > MAX_ENTRIES)
                        {
                            throw new BigFileIndexReadException("Exceeded the maximum allowed number of files for this bigfile type - an incorrect type has most likely been selected.");
                        }
                    }
                }
                uint[] proto = new uint[] {};
                entries = (uint[][])entryList.ToArray(proto.GetType());

                mLoadedPercent = (READ_INDEX_PERCENT);

                iReader.Close();
                iStream.Close();
                mEntries = entries;
                
            }
            catch (Exception ex)
            {
                throw new BigFileIndexReadException
                    ("Failed to read index " + Name + "\r\n" +
                    "The specific error message is: \r\n" +
                    ex.Message
                    );
            }
        }

        public override void ReadIndex()
        {
            ReadEntries();
            if (mEntries != null)
            {
                int numFiles = mEntries.GetUpperBound(0) + 1;
                mFileCount = numFiles;
                if (IsValidIndex)
                {
                    Files = new BF.File[numFiles];
                    for (int i = 0; i < numFiles; i++)
                    {
                        BF.File tFile = new BF.File(mParentBigFile, this, mEntries[i], mNameHashPosition, mOffsetPosition, mLengthPosition);
                        Files[i] = tFile;
                        if (i > 0)
                        {
                            Files[i - 1].Length = (int)(Files[i].Offset - Files[i - 1].Offset);
                        }
                        if (i == (numFiles - 1))
                        {
                            Files[i].Length = (int)(mParentBigFile.FileSize - Files[i].Offset);
                        }
                        if (i > 0)
                        {
                            mLoadedPercent = (((float)i / (float)numFiles) * READ_CONTENT_PERCENT) + READ_INDEX_PERCENT;
                        }
                    }
                }
            }
            else
            {
                mFileCount = 0;
            }
        }
    }
}
