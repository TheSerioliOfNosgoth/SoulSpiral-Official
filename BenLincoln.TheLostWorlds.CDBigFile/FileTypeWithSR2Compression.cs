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
    public class FileTypeWithSR2Compression : FileTypeWithFixedHeader
    {
        protected int mPointerOffset;
 
        #region Properties

        public int PointerOffset
        {
            get
            {
                return mPointerOffset;
            }
            set
            {
                mPointerOffset = value;
            }
        }

        #endregion

        public FileTypeWithSR2Compression()
            : base()
        {
            mHeaderOffset = 0;
            mPointerOffset = 4;
        }

        public override bool CheckType(BF.File checkFile)
        {
            int realHeaderLocation = GetRealHeaderLocation(checkFile);
            return CompareHeaders(checkFile, realHeaderLocation + mHeaderOffset);
        }

        protected int GetRealHeaderLocation(BF.File checkFile)
        {
            int headerLocation = 0;
            FileStream iStream;
            BinaryReader iReader;

            try
            {
                iStream = new FileStream(checkFile.ParentBigFile.Path, FileMode.Open, FileAccess.Read);
                iReader = new BinaryReader(iStream);
                iStream.Seek(checkFile.Offset + mPointerOffset, SeekOrigin.Begin);

                //Read the number of bytes to jump ahead
                int numBytes = iReader.ReadUInt16();
                headerLocation = numBytes + mPointerOffset;

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
    }
}
