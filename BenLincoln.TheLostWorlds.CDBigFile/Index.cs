// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2018 Ben Lincoln
// https://www.thelostworlds.net/
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
    public enum IndexType
    {
        Unknown,
        Pandemonium,
        Gex1PlayStation,
        Gex1Saturn,
        BloodOmen,
        Gex2,
        SR1PC,
        SR2AirForgeDemo,
        SR2PS2,
        SR2PC,
        SR1PS1MainIndex,
        SR1PS1SubIndex,
        SR1PS1PALRetailMainIndex,
        SR1PS1PALRetailSubIndex,
        SR1PS1PALJuly1999MainIndex,
        SR1PS1PALJuly1999SubIndex,
        MadDashRacingBigFile,
        WhiplashBigFile,
        TRLPS2Demo,
        TRLPS2
    }

    public class Index
    {
        protected string mName;
        protected BF.BigFile mParentBigFile;
        protected BF.Index mParentIndex;

        protected BD.Endianness _Endianness;

        //offset in the parent bigfile
        protected long mOffset;

        //length of each entry in UInts
        protected int mEntryLength;

        //positions of various information in each entry
        protected int mFirstEntryOffset;
        protected int mOffsetPosition;

        //various factors
        protected int mOffsetMultiplier;

        //raw data
        protected uint[][] mEntries;

        //contained items
        protected BF.Index[] mIndices;
        protected BF.File[] mFiles;

        //recursive count of all files in this index and below
        protected int mFileCount;

        //percent loaded
        public float mLoadedPercent = 0;
        protected const float READ_INDEX_PERCENT = 10;
        protected const float READ_CONTENT_PERCENT = 90;

        /*public const int itGex1PlayStation = 100;
        public const int itGex1Saturn = 110;
        public const int itBloodOmen = 200;
        public const int itGex2 = 300;
        public const int itSoulReaverPC = 400;
        public const int itSoulReaver2AirForgeDemoFileIndex = 450;
        public const int itSoulReaver2PS2FileIndex = 500;
        public const int itSoulReaver2PC = 550;
        public const int itTombRaiderLegendPlayStation2Demo = 600;
        public const int itTombRaiderLegendPlayStation2 = 650;
        public const int itSoulReaverPlayStationMainIndex = 2100;
        public const int itSoulReaverPlayStationSubIndex = 2150;
        public const int itSoulReaverPlayStationPALRetailMainIndex = 2200;
        public const int itSoulReaverPlaystationPALRetailSubIndex = 2250;
        public const int itSoulReaverPlaystationPALPrereleaseJuly1999MainIndex = 2201;
        public const int itSoulReaverPlaystationPALPrereleaseJuly1999SubIndex = 2251;
        public const int itBloodOmen2FileIndex = 1000;*/

        //maximum number of entries (to help weed out bogus indices)
        protected const int MAX_ENTRIES = 32768;
        //protected const int MAX_ENTRIES = 65530;

        protected bool mIsValidIndex = true;

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

        public BF.Index ParentIndex
        {
            get
            {
                return mParentIndex;
            }
            set
            {
                mParentIndex = value;
            }
        }

        public BD.Endianness Endianness
        {
            get
            {
                return _Endianness;
            }
            set
            {
                _Endianness = value;
            }
        }

        public long Offset
        {
            get
            {
                return mOffset;
            }
            set
            {
                mOffset = value;
            }
        }

        public int EntryLength
        {
            get
            {
                return mEntryLength;
            }
            set
            {
                mEntryLength = value;
            }
        }

        public int FirstEntryOffset
        {
            get
            {
                return mFirstEntryOffset;
            }
            set
            {
                mFirstEntryOffset = value;
            }
        }

        public int OffsetPosition
        {
            get
            {
                return mOffsetPosition;
            }
            set
            {
                mOffsetPosition = value;
            }
        }

        public int OffsetMultiplier
        {
            get
            {
                return mOffsetMultiplier;
            }
            set
            {
                mOffsetMultiplier = value;
            }
        }

        public uint[][] Entries
        {
            get
            {
                return mEntries;
            }
        }

        public BF.Index[] Indices
        {
            get
            {
                return mIndices;
            }
            set
            {
                mIndices = value;
            }
        }

        public BF.File[] Files
        {
            get
            {
                return mFiles;
            }
            set
            {
                mFiles = value;
            }
        }

        public int FileCount
        {
            get
            {
                return mFileCount;
            }
        }

        public bool IsValidIndex
        {
            get
            {
                return mIsValidIndex;
            }
        }


        #endregion

        public Index()
        {
            Initialize();
            Endianness = BenLincoln.Data.Endianness.Little;
        }

        public Index(string name, BF.BigFile parentBigFile, long offset)
        {
            Initialize();
            Name = name;
            ParentBigFile = parentBigFile;
        }

        public Index(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
        {
            Initialize();
            Name = name;
            ParentBigFile = parentBigFile;
            ParentIndex = parentIndex;
            Offset = offset;
        }

        protected virtual void Initialize()
        {
            Name = "";
            ParentBigFile = null;
            ParentIndex = null;
            Offset = 0;
            mFirstEntryOffset = 4;
            EntryLength = 0;
            mOffsetMultiplier = 1;
            Indices = null;
            Files = null;
            mFileCount = 0;
        }

        protected virtual void ReadEntries()
        {
            uint[][] entries;
            FileStream iStream;
            BinaryReader iReader;

            try
            {
                iStream = new FileStream(ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                iReader = new BinaryReader(iStream);
                iStream.Seek(Offset, SeekOrigin.Begin);

                //get the number of entries in the index
                //ushort numEntriesUShort = iReader.ReadUInt16();
                //if (Endianness == BenLincoln.Data.Endianness.Big)
                //{
                //    numEntriesUShort = BD.EndianConverter.ReverseUShort(numEntriesUShort);
                //}
                //int numEntries = numEntriesUShort;
                uint numEntriesUInt = iReader.ReadUInt32();
                if (Endianness == BenLincoln.Data.Endianness.Big)
                {
                    numEntriesUInt = BD.EndianConverter.ReverseUInt(numEntriesUInt);
                }
                int numEntries = 0;
                if (numEntriesUInt <= MAX_ENTRIES)
                {
                    numEntries = (int)numEntriesUInt;
                }

                // This was going negative and sneaking through the check.
                if (numEntriesUInt > (UInt32)MAX_ENTRIES)
                {
                    iReader.Close();
                    iStream.Close();
                    AlternateIndexRead();
                }
                else
                {
                    //proceed to read the rest of the index - 4 bytes past the length indicator
                    iStream.Seek(Offset + mFirstEntryOffset, SeekOrigin.Begin);
                    entries = new uint[numEntries][];
                    for (int i = 0; i < numEntries; i++)
                    {
                        entries[i] = new uint[mEntryLength];
                        for (int j = 0; j < mEntryLength; j++)
                        {
                            entries[i][j] = iReader.ReadUInt32();
                            if (Endianness == BenLincoln.Data.Endianness.Big)
                            {
                                entries[i][j] = BD.EndianConverter.ReverseUInt(entries[i][j]);
                            }
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

        protected virtual void AlternateIndexRead()
        {
            //no alternative for the base class
            //throw new BigFileIndexReadException("Index " + mName + " has too many entries: " + mFileCount);
            mIsValidIndex = false;
        }

        public virtual void ReadIndex()
        {
        }

        public uint[] GetRawDataForEntry(int entryNum)
        {
            uint[] rawData = new uint[mEntryLength];

            for (int i = 0; i < mEntryLength; i++)
            {
                rawData[i] = mEntries[entryNum][i];
            }

            return rawData;
        }

        public BF.Directory BuildDirectoryFromFileData()
        {
            BF.Directory returnDir = new Directory();
            returnDir.Name = "BigFile";

            //get all sub-files, recursively
            ArrayList allFiles = new ArrayList();
            mFileCount = 0;
            allFiles = GetAllFilesRecursively(allFiles);
            mFileCount = allFiles.Count;

            //figure out where to put the file
            foreach (BF.File currentFile in allFiles)
            {
                BF.Directory targetDir = returnDir;
                string[] dirSplit = currentFile.DirectoryName.Split('\\');
                //recursively get a pointer to the target directory
                for (int i = 0; i < dirSplit.GetUpperBound(0); i++)
                {
                    //create subdirectory if necessary
                    if (!(targetDir.DirectoryNames.Contains(dirSplit[i])))
                    {
                        BF.Directory newDir = new BF.Directory();
                        newDir.Name = dirSplit[i];
                        targetDir.AddDirectory(newDir);
                    }
                    targetDir = (BF.Directory)(targetDir.Directories[(int)(targetDir.DirectoryNames[dirSplit[i]])]);
                }

                //add the file
                //if (targetDir.Filenames.Contains(currentFile.Name + "." + currentFile.FileExtension))
                //{
                //    currentFile.Name = GetNextUnusedName(targetDir.Filenames, currentFile);
                //}
                targetDir.AddFile(currentFile);
            }



            return returnDir;
        }

        public ArrayList GetAllFilesRecursively(ArrayList currentList)
        {
            //call subindices if necessary
            if ((mIndices != null) && (mIndices.GetUpperBound(0) > 0))
            {
                foreach (BF.Index nextIndex in mIndices)
                {
                    currentList = nextIndex.GetAllFilesRecursively(currentList);
                    mFileCount += nextIndex.FileCount;
                }
            }

            //add files at the current level
            if ((mFiles != null) && (mFiles.GetUpperBound(0) > 0))
            {
                foreach (BF.File currentFile in mFiles)
                {
                    currentList.Add(currentFile);
                    mFileCount++;
                }
            }

            return currentList;
        }

        public BF.Directory BuildDirectoryFromRawIndex()
        {
            BF.Directory returnDir = new BF.Directory();
            returnDir.Name = mName;
            returnDir.ParentBigFile = mParentBigFile;

            if ((mIndices != null) && (mIndices.GetUpperBound(0) > 0))
            {
                foreach (BF.Index nextIndex in mIndices)
                {
                    returnDir.Directories.Add(nextIndex.BuildDirectoryFromRawIndex());
                }
            }

            if ((mFiles != null) && (mFiles.GetUpperBound(0) > 0))
            {
                foreach (BF.File currentFile in mFiles)
                {
                    //if (returnDir.Filenames.Contains(currentFile.Name + "." + currentFile.FileExtension))
                    //{
                    //    currentFile.Name = GetNextUnusedName(returnDir.Filenames, currentFile);
                    //}
                    returnDir.AddFile(currentFile);
                }
            }

            return returnDir;
        }

        //protected string GetNextUnusedName(Hashtable currentList, BF.File currentFile)
        //{
        //    int number = 0;
        //    string newName = currentFile.Name;
        //    do
        //    {
        //        newName = currentFile.Name + "-duplicate-" + string.Format("{0:000}", number);
        //        number++;
        //    } while (currentList.Contains(newName + "." + currentFile.FileExtension));

        //    return newName;
        //}

        public static BF.Index CreateMasterIndex(BF.BigFile bigfile)
        {
            return CreateIndex(bigfile, bigfile.Type.MasterIndexType);
        }

        public static BF.Index CreateIndex(BF.BigFile bigfile, IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.Pandemonium:
                    BF.PandemoniumFileIndex pfi = new PandemoniumFileIndex("Index", bigfile, null, 0);
                    pfi.EntryLength = 2;
                    pfi.OffsetPosition = 0;
                    pfi.NameHashPosition = 1;
                    return pfi;
                    break;
                case IndexType.SR1PS1MainIndex:
                    BF.IndexIndex ixiSoulReaverPlayStationMainIndex;
                    ixiSoulReaverPlayStationMainIndex = new BF.IndexIndex("Index", bigfile, null, 0);
                    ixiSoulReaverPlayStationMainIndex.EntryLength = 2;
                    ixiSoulReaverPlayStationMainIndex.OffsetPosition = 1;
                    //??Position = 0;
                    ixiSoulReaverPlayStationMainIndex.SubIndexType = IndexType.SR1PS1SubIndex;
                    return ixiSoulReaverPlayStationMainIndex;
                    break;
                case IndexType.SR1PS1SubIndex:
                    BF.FileIndex ixfSoulReaverPlayStationSubIndex;
                    ixfSoulReaverPlayStationSubIndex = new BF.FileIndex("Index", bigfile, null, 0);
                    ixfSoulReaverPlayStationSubIndex.EntryLength = 4;
                    ixfSoulReaverPlayStationSubIndex.NameHashPosition = 0;
                    ixfSoulReaverPlayStationSubIndex.LengthPosition = 1;
                    ixfSoulReaverPlayStationSubIndex.OffsetPosition = 2;
                    //??Position = 3;
                    return ixfSoulReaverPlayStationSubIndex;
                    break;
                case IndexType.SR1PS1PALRetailMainIndex:
                    BF.IndexIndex ixiSoulReaverPlayStationPALRetailMainIndex;
                    ixiSoulReaverPlayStationPALRetailMainIndex = new BF.IndexIndex("Index", bigfile, null, 0);
                    ixiSoulReaverPlayStationPALRetailMainIndex.EntryLength = 2;
                    ixiSoulReaverPlayStationPALRetailMainIndex.OffsetPosition = 1;
                    //??Position = 0;
                    ixiSoulReaverPlayStationPALRetailMainIndex.SubIndexType = IndexType.SR1PS1PALRetailSubIndex;
                    return ixiSoulReaverPlayStationPALRetailMainIndex;
                    break;
                case IndexType.SR1PS1PALRetailSubIndex:
                    BF.SoulReaverPlaystationPALFileIndex ixfSoulReaverPlaystationPALRetailSubIndex;
                    ixfSoulReaverPlaystationPALRetailSubIndex = new BF.SoulReaverPlaystationPALFileIndex("Index", bigfile, null, 0);
                    ixfSoulReaverPlaystationPALRetailSubIndex.EntryLength = 4;
                    ixfSoulReaverPlaystationPALRetailSubIndex.NameHashPosition = 0;
                    ixfSoulReaverPlaystationPALRetailSubIndex.LengthPosition = 1;
                    ixfSoulReaverPlaystationPALRetailSubIndex.OffsetPosition = 2;
                    //??Position = 3;
                    return ixfSoulReaverPlaystationPALRetailSubIndex;
                    break;
                case IndexType.SR1PS1PALJuly1999MainIndex:
                    BF.IndexIndex ixiSoulReaverPlayStationPALPrereleaseJuly1999MainIndex;
                    ixiSoulReaverPlayStationPALPrereleaseJuly1999MainIndex = new BF.IndexIndex("Index", bigfile, null, 0);
                    ixiSoulReaverPlayStationPALPrereleaseJuly1999MainIndex.EntryLength = 2;
                    ixiSoulReaverPlayStationPALPrereleaseJuly1999MainIndex.OffsetPosition = 1;
                    //??Position = 0;
                    ixiSoulReaverPlayStationPALPrereleaseJuly1999MainIndex.SubIndexType = IndexType.SR1PS1PALJuly1999SubIndex;
                    return ixiSoulReaverPlayStationPALPrereleaseJuly1999MainIndex;
                    break;
                case IndexType.SR1PS1PALJuly1999SubIndex:
                    BF.SoulReaverPlaystationPALFileIndex ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex;
                    ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex = new BF.SoulReaverPlaystationPAL0x1b71FileIndex("Index", bigfile, null, 0);
                    ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex.EntryLength = 4;
                    ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex.NameHashPosition = 0;
                    ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex.LengthPosition = 1;
                    ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex.OffsetPosition = 2;
                    //??Position = 3;
                    return ixfSoulReaverPlaystationPALPrereleaseJuly1999SubIndex;
                    break;
                case IndexType.Gex1PlayStation:
                    BF.FileIndex ixfSI100;
                    ixfSI100 = new BF.FileIndex("Index", bigfile, null, 0);
                    ixfSI100.FirstEntryOffset = 20;
                    ixfSI100.EntryLength = 4;
                    ixfSI100.NameHashPosition = 0;
                    ixfSI100.LengthPosition = 1;
                    ixfSI100.OffsetPosition = 2;
                    //??Position = 3;
                    return ixfSI100;
                    break;
                case IndexType.Gex1Saturn:
                    BF.FileIndex ixfSI110;
                    ixfSI110 = new BF.FileIndex("Index", bigfile, null, 0);
                    ixfSI110.Endianness = BenLincoln.Data.Endianness.Big;
                    ixfSI110.FirstEntryOffset = 20;
                    ixfSI110.EntryLength = 4;
                    ixfSI110.NameHashPosition = 0;
                    ixfSI110.LengthPosition = 1;
                    ixfSI110.OffsetPosition = 2;
                    //??Position = 3;
                    return ixfSI110;
                    break;
                case IndexType.BloodOmen:
                    BF.FileIndex ixfSI200;
                    ixfSI200 = new BF.FileIndex("Index", bigfile, null, 0);
                    ixfSI200.EntryLength = 3;
                    ixfSI200.NameHashPosition = 0;
                    ixfSI200.LengthPosition = 1;
                    ixfSI200.OffsetPosition = 2;
                    return ixfSI200;
                    break;
                case IndexType.Gex2:
                    BF.SR1Proto1FileIndex ixfSI300;
                    ixfSI300 = new BF.SR1Proto1FileIndex("Index", bigfile, null, 0);
                    ixfSI300.EntryLength = 6;
                    ixfSI300.NameHashPosition = 0;
                    ixfSI300.LengthPosition = 1;
                    ixfSI300.OffsetPosition = 3;
                    ixfSI300.CompressedLengthPosition = 2;
                    //ixfSI300.
                    //??Position = 2;
                    //??Position = 4;
                    //??Position = 5;
                    return ixfSI300;
                    break;
                case IndexType.SR1PC:
                    BF.FileIndex ixfSI400;
                    ixfSI400 = new BF.FileIndex("Index", bigfile, null, 0);
                    ixfSI400.EntryLength = 4;
                    ixfSI400.NameHashPosition = 0;
                    ixfSI400.LengthPosition = 1;
                    ixfSI400.OffsetPosition = 2;
                    //??Position = 3;
                    return ixfSI400;
                    break;
                case IndexType.SR2AirForgeDemo:
                    BF.SoulReaver2AirForgeDemoFileIndex ixfSoulReaver2AirForgeDemo;
                    ixfSoulReaver2AirForgeDemo = new BF.SoulReaver2AirForgeDemoFileIndex("Index", bigfile, null, 0);
                    ixfSoulReaver2AirForgeDemo.EntryLength = 4;
                    ixfSoulReaver2AirForgeDemo.NameHashPosition = 0;
                    ixfSoulReaver2AirForgeDemo.LengthPosition = 1;
                    ixfSoulReaver2AirForgeDemo.OffsetPosition = 2;
                    ixfSoulReaver2AirForgeDemo.CompressedLengthPosition = 3;
                    return ixfSoulReaver2AirForgeDemo;
                    break;
                case IndexType.SR2PS2:
                    BF.SoulReaver2PS2FileIndex ixfSoulReaver2PS2FileIndex;
                    ixfSoulReaver2PS2FileIndex = new BF.SoulReaver2PS2FileIndex("Index", bigfile, null, 0);
                    ixfSoulReaver2PS2FileIndex.EntryLength = 3;
                    ixfSoulReaver2PS2FileIndex.LengthPosition = 0;
                    ixfSoulReaver2PS2FileIndex.OffsetPosition = 1;
                    ixfSoulReaver2PS2FileIndex.CompressedLengthPosition = 2;
                    return ixfSoulReaver2PS2FileIndex;
                    break;
                case IndexType.SR2PC:
                    BF.FileIndexWithSeparateHashes ixfSI550;
                    ixfSI550 = new BF.FileIndexWithSeparateHashes("Index", bigfile, null, 0);
                    ixfSI550.EntryLength = 3;
                    ixfSI550.LengthPosition = 0;
                    ixfSI550.OffsetPosition = 1;
                    //??Position = 2;
                    return ixfSI550;
                    break;
                case IndexType.TRLPS2Demo:
                    BF.FileIndexWithSeparateHashes ixfSI600;
                    ixfSI600 = new BF.FileIndexWithSeparateHashes("Index", bigfile, null, 0);
                    ixfSI600.EntryLength = 4;
                    ixfSI600.LengthPosition = 0;
                    ixfSI600.OffsetPosition = 1;
                    //??Position = 2;
                    //??Position = 3;
                    return ixfSI600;
                    break;
                case IndexType.TRLPS2:
                    BF.FileIndexWithSeparateHashes ixfSI650;
                    ixfSI650 = new BF.FileIndexWithSeparateHashes("Index", bigfile, null, 0);
                    ixfSI650.EntryLength = 4;
                    ixfSI650.LengthPosition = 0;
                    ixfSI650.OffsetPosition = 1;
                    //??Position = 2;
                    //??Position = 3;
                    //this index type uses offset values divided by 2048
                    ixfSI650.OffsetMultiplier = 2048;
                    return ixfSI650;
                    break;
                case IndexType.MadDashRacingBigFile:
                    BF.MadDashRacingBigFileIndex ixfBloodOmen2FileIndex;
                    ixfBloodOmen2FileIndex = new BF.MadDashRacingBigFileIndex("Index", bigfile, null, 0);
                    ixfBloodOmen2FileIndex.FirstEntryOffset = 0;
                    ixfBloodOmen2FileIndex.EntryLength = 10;
                    ixfBloodOmen2FileIndex.OffsetPosition = 0;
                    return ixfBloodOmen2FileIndex;
                    break;
                case IndexType.WhiplashBigFile:
                    BF.WhiplashBigFileFileIndex ixfWhiplashBigFileFileIndex;
                    ixfWhiplashBigFileFileIndex = new BF.WhiplashBigFileFileIndex("Index", bigfile, null, 0);
                    ixfWhiplashBigFileFileIndex.FirstEntryOffset = 0;
                    ixfWhiplashBigFileFileIndex.EntryLength = 10;
                    ixfWhiplashBigFileFileIndex.OffsetPosition = 0;
                    return ixfWhiplashBigFileFileIndex;
                    break;
            }

            return null;
        }


    }
}
