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
using System.Text;
using System.IO;
using BF = BenLincoln.TheLostWorlds.CDBigFile;
using BD = BenLincoln.Data;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class WhiplashBigFileFileIndex : MadDashRacingBigFileIndex
    {
        public WhiplashBigFileFileIndex(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
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
                iStream.Seek(Offset, SeekOrigin.Begin);

                String fileType = new String(iReader.ReadChars(8), 0, 8);
                if (fileType != "goefile\0")
                {
                    // Could also mean there are symbols. Not 100% sure yet.
                    throw new Exception("This file does not contain the 'goefile' header, and therefore cannot be read as the selected file type.");
                }

                uint mdrFileType1 = iReader.ReadUInt32();
                uint mdrFileType2 = iReader.ReadUInt32();
                //if (mdrFileType2 == 0)
                //{
                //    // Could also mean there are symbols. Not 100% sure yet.
                //    throw new Exception("Not a BO2 archive file.");
                //}

                int mainSymListOffset = (int)iStream.Position;
                String symlistCheck = new String(iReader.ReadChars(8), 0, 8);
                if (symlistCheck != "symlist\0")
                {
                    throw new Exception("This file does not contain the 'symlist' sub-header at the expected position, and therefore cannot be read as the selected file type.");
                }

                int mainSymListLength = iReader.ReadInt32();
                if (mainSymListLength <= 0)
                {
                    throw new Exception("When parsing this file as the selected file type, the number of entries indicated is zero, or negative.");
                }

                uint unknown1 = iReader.ReadUInt32();
                uint unknown2 = iReader.ReadUInt32();

                int entryOffset = mainSymListOffset + mainSymListLength;
                List<int> entryPositions = new List<int>();
                while (entryOffset < iStream.Length)
                {
                    //iStream.Position = entryOffset;
                    iStream.Seek(entryOffset, SeekOrigin.Begin);
                    uint unknown3 = iReader.ReadUInt32();
                    uint unknown4 = iReader.ReadUInt32();
                    int entryLength = iReader.ReadInt32();
                    if (entryLength < 0)
                    {
                        throw new Exception("When parsing this file as the selected file type, one of the files contained has a negative length. This generally indicates that the file is not of the selected type.");
                    }
                    entryLength = entryLength * 2;
                    uint unknown5 = iReader.ReadUInt32();
                    //uint fileHashedName = iReader.ReadUInt32();
                    byte hashNameB1 = iReader.ReadByte();
                    byte hashNameB2 = iReader.ReadByte();
                    byte hashNameB3 = iReader.ReadByte();
                    byte hashNameB4 = iReader.ReadByte();
                    //string fileHashedName = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", hashNameB1, hashNameB2, hashNameB3, hashNameB4).ToUpper();
                    string fileHashedName = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", hashNameB4, hashNameB3, hashNameB2, hashNameB1).ToUpper();
                    uint unknown6 = iReader.ReadUInt32();
                    int contentOffset = (int)iStream.Position;
                    //int nextEntryOffset = entryOffset + entryLength;
                    int nextEntryOffset = contentOffset + entryLength;
                    //// files offsets are aligned to multiples of 16 bytes
                    //nextEntryOffset |= 0x10;
                    //nextEntryOffset &= 0xFFFFFF0;

                    //// Add 15 & And 16 seems to round up nicely.
                    nextEntryOffset += 15;
                    nextEntryOffset &= 0xFFFFFF0;

                    int fileLength = nextEntryOffset - contentOffset;


                    // If something other than ReadIndex called this function, then we don't need these.
                    if (mEntryOffsets != null) mEntryOffsets.Add(entryOffset);
                    if (mFileHashedNames != null) mFileHashedNames.Add(fileHashedName);
                    if (mFileOffsets != null) mFileOffsets.Add(contentOffset);
                    if (mFileLengths != null) mFileLengths.Add(fileLength);

                    // But we do need this for the section below, even though it's a copy of mEntryPositions.
                    entryPositions.Add(entryOffset);
                    entryOffset = nextEntryOffset;
                }

                int numEntries = entryPositions.Count;
                iStream.Seek(Offset + mFirstEntryOffset, SeekOrigin.Begin);
                entries = new uint[numEntries][];
                for (int i = 0; i < numEntries; i++)
                {
                    iStream.Position = entryPositions[i];
                    entries[i] = new uint[mEntryLength];
                    for (int j = 0; j < mEntryLength; j++)
                    {
                        entries[i][j] = iReader.ReadUInt32();
                    }
                    if (i > 0)
                    {
                        mLoadedPercent = (((float)i / numEntries) * READ_INDEX_PERCENT);
                    }
                }

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
    }
}
