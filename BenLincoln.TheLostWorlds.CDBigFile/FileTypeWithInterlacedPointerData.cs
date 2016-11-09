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

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class FileTypeWithInterlacedPointerData : FileTypeWithFixedHeader
    {
        class RelativeStream
        {
            private BinaryReader Reader = null;
            private Int64 Offset = 0;
            public RelativeStream(BinaryReader xReader, Int64 uOffset)
            {
                Reader = xReader;
                Offset = uOffset;
            }

            public Int64 Pos
            {
                get
                {
                    return (Int64)Reader.BaseStream.Position - Offset;
                }
                set
                {
                    Reader.BaseStream.Position = value + Offset;
                }
            }
        }

        protected int mNamePointerOffset;
        protected int mNameOffset;
 
        #region Properties

        public int NamePointerOffset
        {
            get
            {
                return mNamePointerOffset;
            }
            set
            {
                mNamePointerOffset = value;
            }
        }

        #endregion

        public FileTypeWithInterlacedPointerData()
            : base()
        {
            mHeaderOffset = 0x84;
            mNamePointerOffset = 0x54;
        }

        public override bool CheckType(BF.File checkFile)
        {
            if ((checkFile.HashedName & 7) != 0)
            {
                return false;
            }

            try
            {
                int realHeaderLocation = GetRealHeaderLocation(checkFile);
                return CompareHeaders(checkFile, realHeaderLocation);
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public override bool CompareHeaders(BF.File checkFile, int offset)
        {
            bool matchFound = true;

            //if the header data has already been read into the file, use its copy
            if (offset == 0)
            {
                if (mHeader.GetUpperBound(0) > checkFile.RawHeaderData.GetUpperBound(0))
                {
                    return false;
                }
                for (int i = 0; i <= mHeader.GetUpperBound(0); i++)
                {
                    if (mHeader[i] != checkFile.RawHeaderData[i])
                    {
                        matchFound = false;
                    }
                }
            }
            //otherwise read the appropriate bytes
            else
            {
                if ((offset + mHeader.GetUpperBound(0) > checkFile.Length))
                {
                    return false;
                }

                byte[] headerFromFile = new byte[mHeader.GetUpperBound(0) + 1];

                Stream iStream;
                BinaryReader iReader;

                try
                {
                    if (checkFile.GetType() == typeof(CompressedFile))
                    {
                        iStream = new MemoryStream(checkFile.Length);
                        CompressedFile compressedCheckFile = (CompressedFile)checkFile;
                        compressedCheckFile.ExportToStream(iStream);

                        iReader = new BinaryReader(iStream);
                        iStream.Seek(offset, SeekOrigin.Begin);

                        //read as many bytes as the header should hold
                        iStream.Read(headerFromFile, 0, headerFromFile.GetUpperBound(0) + 1);
                        for (int i = 0; i <= headerFromFile.GetUpperBound(0); i++)
                        {
                            if (mHeader[i] != headerFromFile[i])
                            {
                                matchFound = false;
                            }
                        }
                    }
                    else
                    {
                        iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                        iReader = new BinaryReader(iStream);
                        iStream.Seek(checkFile.Offset + offset, SeekOrigin.Begin);

                        //read as many bytes as the header should hold
                        iStream.Read(headerFromFile, 0, headerFromFile.GetUpperBound(0) + 1);
                        for (int i = 0; i <= headerFromFile.GetUpperBound(0); i++)
                        {
                            if (mHeader[i] != headerFromFile[i])
                            {
                                matchFound = false;
                            }
                        }
                    }

                    iReader.Close();
                    iStream.Close();
                }
                catch (Exception ex)
                {
                    throw new FileReadException
                        ("Failed to read file " + Name + "\r\n" +
                        "The specific error message is: \r\n" +
                        ex.Message
                        );
                }
            }

            return matchFound;
        }

        protected int GetRealHeaderLocation(BF.File checkFile)
        {
            int headerLocation = 0;
            Stream iStream;
            BinaryReader iReader;

            try
            {
                if (checkFile.GetType() == typeof(CompressedFile))
                {
                    iStream = new MemoryStream(checkFile.Length);
                    CompressedFile compressedCheckFile = (CompressedFile)checkFile;
                    compressedCheckFile.ExportToStream(iStream);
                    iStream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                    iStream.Seek(checkFile.Offset, SeekOrigin.Begin);
                }

                iReader = new BinaryReader(iStream);

                Int32 nameOffsetLocation = 0;
                Int32 nameOffset = 0;
                UInt32 startPosition = (UInt32)iReader.BaseStream.Position;
                UInt32 bytesRead = 0;
                RelativeStream relStream = new RelativeStream(iReader, startPosition);

                relStream.Pos = 0;
                relStream.Pos += 0x04;
                UInt32 regionCount = iReader.ReadUInt32();

                // AMF 02/08/14 - 500 seemed like a reasonable maximum
                if (regionCount > 500)
                {
                    throw new Exception("Too many regions to scan.");
                }

                UInt32 regionPosition = 0;
                UInt32[] regionSizes = new UInt32[regionCount];
                UInt32[] regionPositions = new UInt32[regionCount];
                UInt32[] objectDataSizes = new UInt32[regionCount];
                UInt32[] objectDataPositions = new UInt32[regionCount];
                for (UInt32 r = 0; r < regionCount; r++)
                {
                    regionSizes[r] = iReader.ReadUInt32();
                    regionPositions[r] = regionPosition;
                    regionPosition += regionSizes[r];
                    relStream.Pos += 0x08;
                }

                UInt32 regionDataSize = regionCount * 0x0C;
                UInt32 pointerDataPosition = (regionDataSize & 0x00000003) + ((regionDataSize + 0x17) & 0xFFFFFFF0);
                for (UInt32 r = 0; r < regionCount; r++)
                {
                    relStream.Pos = pointerDataPosition;
                    UInt32 pointerCount = iReader.ReadUInt32();
                    UInt32 pointerDataSize = pointerCount * 0x04;
                    UInt32 objectDataPosition = pointerDataPosition + ((pointerDataSize + 0x13) & 0xFFFFFFF0);
                    UInt32 objectDataSize = (regionSizes[r] + 0x0F) & 0xFFFFFFF0;

                    objectDataSizes[r] = objectDataSize;
                    objectDataPositions[r] = objectDataPosition;

                    relStream.Pos = objectDataPosition;
                    Byte[] objectData = iReader.ReadBytes((Int32)objectDataSize);
                    bytesRead += objectDataSize;

                    if (headerLocation == 0 && bytesRead >= mHeaderOffset)
                    {
                        Int64 overshot = (Int64)iReader.BaseStream.Position - (Int32)objectDataSize;
                        overshot += (Int64)mHeaderOffset;
                        headerLocation = ((Int32)overshot) - ((Int32)startPosition + 0x04);
                    }

                    if (nameOffsetLocation == 0 && bytesRead >= mNamePointerOffset)
                    {
                        Int64 overshot = (Int64)iReader.BaseStream.Position - (Int32)objectDataSize;
                        overshot += (Int64)mNamePointerOffset;
                        nameOffsetLocation = ((Int32)overshot) - ((Int32)startPosition + 0x04);
                    }

                    relStream.Pos = pointerDataPosition + 0x04;
                    UInt32[] pointers = new UInt32[pointerCount];
                    for (UInt32 p = 0; p < pointerCount; p++)
                    {
                        pointers[p] = iReader.ReadUInt32();
                    }

                    UInt32[] addresses = new UInt32[pointerCount];
                    for (UInt32 p = 0; p < pointerCount; p++)
                    {
                        relStream.Pos = objectDataPosition + pointers[p];

                        bool readingNameOffset = (relStream.Pos == nameOffsetLocation);

                        UInt32 value1 = iReader.ReadUInt32();
                        UInt32 value2 = value1 & 0x003FFFFF;
                        UInt32 value3 = value1 >> 0x16;

                        pointers[p] += regionPositions[r];
                        addresses[p] = regionPositions[value3] + value2;

                        if (readingNameOffset)
                        {
                            nameOffset = (Int32)addresses[p];
                        }
                    }

                    pointerDataPosition = objectDataPosition + objectDataSize;

                    if (headerLocation != 0 && nameOffsetLocation != 0)
                    {
                        break;
                    }
                }

                for (UInt32 r = 0; r < regionCount; r++)
                {
                    if (nameOffset < (Int32)objectDataSizes[r])
                    {
                        nameOffset += (Int32)objectDataPositions[r];
                        break;
                    }

                    if (r == (regionCount - 1))
                    {
                        nameOffset = 0;
                    }

                    nameOffset -= (Int32)objectDataSizes[r];
                }

                mNameOffset = nameOffset;

                iReader.Close();
                iStream.Close();
            }
            catch (Exception ex)
            {
                throw new FileReadException
                    ("Failed to read file " + Name + "\r\n" +
                    "The specific error message is: \r\n" +
                    ex.Message
                    );
            }

            return headerLocation;
        }

        public override string GetInternalName(BF.File checkFile)
        {
            return "";

            int nameOffset = mNameOffset;
            string internalName = "";

            Stream iStream;
            BinaryReader iReader;

            try
            {
                if (checkFile.GetType() == typeof(CompressedFile))
                {
                    iStream = new MemoryStream(checkFile.Length);
                    CompressedFile compressedCheckFile = (CompressedFile)checkFile;
                    compressedCheckFile.ExportToStream(iStream);
                    iStream.Seek(0, SeekOrigin.Begin);
                    iReader = new BinaryReader(iStream);
                    iStream.Seek(nameOffset, SeekOrigin.Begin);
                }
                else
                {
                    iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                    iStream.Seek(checkFile.Offset, SeekOrigin.Begin);
                    iReader = new BinaryReader(iStream);
                    iStream.Seek(checkFile.Offset + nameOffset, SeekOrigin.Begin);
                }

                byte[] nameBuffer = sanitizeByteArray(iReader.ReadBytes(8), (byte)95);
                internalName = new string(Encoding.ASCII.GetChars(nameBuffer, 0, 8));
                internalName = internalName.Trim(new char[] { (char)'_' });
                internalName = internalName.ToLower();

                //String strModelName = new String(iReader.ReadChars(10)); // Need to check
                //try
                //{
                //    internalName = strModelName.Substring(0, strModelName.IndexOf('\0'));
                //}
                //catch
                //{
                //    internalName = strModelName;
                //}

                iReader.Close();
                iStream.Close();
            }
            catch (Exception ex)
            {
                throw new FileReadException
                    ("Failed to read file " + Name + "\r\n" +
                    "The specific error message is: \r\n" +
                    ex.Message
                    );
            }
            return internalName;
        }

        protected byte[] sanitizeByteArray(byte[] inArray, byte defaultChar)
        {
            byte[] outArray;
            outArray = new byte[inArray.GetUpperBound(0) + 1];

            //sanitize name
            for (int s = 0; s <= inArray.GetUpperBound(0); s++)
            {
                //non-printable ASCII chars
                if (inArray[s] < 32)
                {
                    outArray[s] = defaultChar;
                }
                //various punctuation
                else if ((inArray[s] > 32) && (inArray[s] < 46))
                {
                    outArray[s] = defaultChar;
                }
                else if (inArray[s] == 47)
                {
                    outArray[s] = defaultChar;
                }
                else if ((inArray[s] > 57) && (inArray[s] < 65))
                {
                    outArray[s] = defaultChar;
                }
                else if ((inArray[s] > 90) && (inArray[s] < 95))
                {
                    outArray[s] = defaultChar;
                }
                else if (inArray[s] == 96)
                {
                    outArray[s] = defaultChar;
                }
                else if (inArray[s] > 122)
                {
                    outArray[s] = defaultChar;
                }
                //still valid
                else
                {
                    outArray[s] = inArray[s];
                }
            }
            return outArray;
        }
    }
}
