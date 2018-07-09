// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2014 Ben Lincoln
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
    public class BloodOmen2WrappedFile : BF.File
    {
        public BloodOmen2WrappedFile(BF.BigFile parent, BF.Index parentIndex, uint[] rawIndexData, string hashedName, int offset, int length)
            : base()
        {
            mParentBigFile = parent;
            mParentIndex = parentIndex;
            mRawIndexData = rawIndexData;

            mHashedName = hashedName;
            mOffset = offset;
            mLength = length;

            CheckFileDataForSanity();
            if (mIsValidReference)
            {
                GetHeaderData();
                mType = GetFileType();
                mCanBeReplaced = true;
            }
            else
            {
                mType = BF.FileType.FromType(BF.FileType.FILE_TYPE_Invalid);
            }
            GetNameComponents();
        }

        protected override void GetNameComponents()
        {
            base.GetNameComponents();

            string fileType = "";
            for (int i = 0; i < 2; i++)
            {
                uint reversed = mRawIndexData[i];
                byte[] rvBytes = BitConverter.GetBytes(reversed);
                fileType += BytesToASCII(rvBytes, "");
            }

            mFileExtension = fileType.Trim(new char[] { '_' }); ;

            if (mFileExtension == "texture")
            {
                mFileExtension = "dds";
            }
        }

        protected override string GetGenericInfo()
        {
            string fileType = "";
            string description = "";
            for (int i = 0; i < 2; i++)
            {
                uint reversed = mRawIndexData[i];
                byte[] rvBytes = BitConverter.GetBytes(reversed);
                fileType += BytesToASCII(rvBytes, "");
            }

            fileType = fileType.Trim(new char[] { '_' });
            fileType = (Char.ToUpper(fileType[0]) + fileType.Substring(1));

            description = fileType;
            if (description == "Texture")
            {
                description = "Texture (Direct Draw Surface)";
            }

            return
                "File Information\r\n---\r\n" +
                "Name: " + mName + "." + mFileExtension + "\r\n" +
                "Offset: " + mOffset + "\r\n" +
                "Length: " + mLength + "\r\n" +
                //"Offset:" + "0x" + String.Format("{0:X8}", mOffset) + "\r\n" +
                //"Length:" + "0x" + String.Format("{0:X8}", mLength) + "\r\n" +
                "Type Name: " + fileType + "\r\n" +
                "Type Description: " + description + "\r\n"
                ;
        }
    }
}
