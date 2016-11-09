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
    public class FileTypeWithFixedHeader : BF.FileType
    {
        protected byte[] mHeader;
        protected int mHeaderOffset;

        #region Properties

        public byte[] Header
        {
            get
            {
                return mHeader;
            }
            set
            {
                mHeader = value;
            }
        }

        public int HeaderOffset
        {
            get
            {
                return mHeaderOffset;
            }
            set
            {
                mHeaderOffset = value;
            }
        }

        #endregion

        public FileTypeWithFixedHeader()
        {
            Name = "Unrecognized";
            Description = "A file that is not recognized by Soul Spiral";
            FileExtension = "dat";
            mHeader = new byte[] { 0x00, 0x00, 0x19, 0x11, 0x19, 0x11 };
            mHeaderOffset = 0;
        }

        public override bool CheckType(BF.File checkFile)
        {
            return CompareHeaders(checkFile, mHeaderOffset);
        }

        public virtual bool CompareHeaders(BF.File checkFile, int offset)
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

                FileStream iStream;
                BinaryReader iReader;

                try
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
    }
}
