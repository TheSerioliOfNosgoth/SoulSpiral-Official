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
    public class FileTypeWithBlockHeader : FileTypeWithFixedHeader
    {
        protected int mIndexEntryLength;
        protected int mIndexBlockSize;
        protected int mNameOffset;
        protected bool mNameOffsetIsPointer;

        #region Properties

        public int IndexEntryLength
        {
            get
            {
                return mIndexEntryLength;
            }
            set
            {
                mIndexEntryLength = value;
            }
        }

        public int IndexBlockSize
        {
            get
            {
                return mIndexBlockSize;
            }
            set
            {
                mIndexBlockSize = value;
            }
        }

        public int NameOffset
        {
            get
            {
                return mNameOffset;
            }
            set
            {
                mNameOffset = value;
            }
        }

        public bool NameOffsetIsPointer
        {
            get
            {
                return mNameOffsetIsPointer;
            }
            set
            {
                mNameOffsetIsPointer = value;
            }
        }


        #endregion

        public FileTypeWithBlockHeader()
            : base()
        {
            mIndexEntryLength = 4;
            mIndexBlockSize = 2048;
            mHeaderOffset = 0;
            mNameOffset = 0;
            mNameOffsetIsPointer = false;
        }

        public override bool CheckType(BF.File checkFile)
        {
            int endOfIndexOffset = GetEndOfIndex(checkFile);
            return CompareHeaders(checkFile, endOfIndexOffset + mHeaderOffset);
        }

        public override string GetInternalName(BF.File checkFile)
        {
            int endOfIndexOffset = GetEndOfIndex(checkFile);
            int nameOffset = mNameOffset;
            string internalName = "";

            FileStream iStream;
            BinaryReader iReader;

            if (NameOffsetIsPointer)
            {

                try
                {
                    iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                    iReader = new BinaryReader(iStream);
                    iStream.Seek(checkFile.Offset + endOfIndexOffset + nameOffset, SeekOrigin.Begin);

                    nameOffset = iReader.ReadUInt16();

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

            try
            {
                iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                iReader = new BinaryReader(iStream);
                iStream.Seek(checkFile.Offset + endOfIndexOffset + nameOffset, SeekOrigin.Begin);

                byte[] nameBuffer = sanitizeByteArray(iReader.ReadBytes(8), (byte)95);
                internalName = new string(Encoding.ASCII.GetChars(nameBuffer, 0, 8));

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

        protected int GetEndOfIndex(BF.File checkFile)
        {
            int endOfIndexOffset = 0;

            FileStream iStream;
            BinaryReader iReader;

            try
            {
                iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                iReader = new BinaryReader(iStream);
                iStream.Seek(checkFile.Offset, SeekOrigin.Begin);

                //read the count of entries in the index for this file
                int numEntries = iReader.ReadUInt16();
                endOfIndexOffset = (numEntries * mIndexEntryLength)
                    - ((numEntries * mIndexEntryLength) % mIndexBlockSize)
                    + mIndexBlockSize;

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

            return endOfIndexOffset;
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
