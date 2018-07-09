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
using BF = BenLincoln.TheLostWorlds.CDBigFile;
using BLD = BenLincoln.Data;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class Fingerprint
    {
        protected string mTitle;
        protected string mPlatform;
        protected string mFormat;
        protected string mLanguage;
        protected string mReleaseType;
        protected string mReleaseID;
        protected string mFileName;
        protected string mBuildDate;
        protected long mFileSize;
        protected byte[] mSHA256Hash;
        protected BF.BigFileType mType;

        #region Properties
 
        public long FileSize
        {
            get
            {
                return mFileSize;
            }
            set
            {
                mFileSize = value;
            }
        }

        public string Title
        {
            get
            {
                return mTitle;
            }
            set
            {
                mTitle = value;
            }
        }

        public string Platform
        {
            get
            {
                return mPlatform;
            }
            set
            {
                mPlatform = value;
            }
        }

        public string Format
        {
            get
            {
                return mFormat;
            }
            set
            {
                mFormat = value;
            }
        }

        public string Language
        {
            get
            {
                return mLanguage;
            }
            set
            {
                mLanguage = value;
            }
        }

        public string ReleaseType
        {
            get
            {
                return mReleaseType;
            }
            set
            {
                mReleaseType = value;
            }
        }

        public string ReleaseID
        {
            get
            {
                return mReleaseID;
            }
            set
            {
                mReleaseID = value;
            }
        }

        public string FileName
        {
            get
            {
                return mFileName;
            }
            set
            {
                mFileName = value;
            }
        }

        public string BuildDate
        {
            get
            {
                return mBuildDate;
            }
            set
            {
                mBuildDate = value;
            }
        }

        public BF.BigFileType Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        public byte[] SHA256Hash
        {
            get
            {
                return mSHA256Hash;
            }
            set
            {
                mSHA256Hash = value;
            }
        }

        #endregion

        public Fingerprint()
        {
            Title = "Unrecognized File";
            Platform = "";
            Format = "";
            Language = "";
            ReleaseType = "";

            BuildDate = "";
            FileSize = 0;
            Type = new BigFileType();
            mSHA256Hash = BLD.HexConverter.HexStringToBytes("0000000000000000000000000000000000000000000000000000000000000000");
        }

        public virtual string GetDisplayName()
        {
            string result = string.Format("{0} ({1}", Title, Platform);
            if (Format != "")
            {
                result = result + string.Format("/{0}", Format);
                //return string.Format("{0} ({1}/{2}/{3} - {4} - {5})", Title, Platform, Format, Language, ReleaseType, BuildDate);
            }
            result = result + string.Format("/{0}", Language);
            result = result + ")";
            if (ReleaseID != "")
            {
                result = result + string.Format(" - {0}", ReleaseID);
            }
            result = result + string.Format(" - {0}", ReleaseType);
            if (BuildDate != "")
            {
                result = result + string.Format(" - {0}", BuildDate);
            }
            if (FileName != "")
            {
                result = result + string.Format(" ({0})", FileName);
            } 
            return result;
        }

        //if the file size has not been specified, treat this as a generic "unrecognized" fingerprint
        public virtual bool CheckBigFileForMatch(BF.BigFile compareFile, bool checkHash)
        {
            if (FileSize == 0)
            {
                return false;
            }
            else
            {
                bool isMatch = true;
                if (FileSize != compareFile.FileSize)
                {
                    isMatch = false;
                }
                else
                {
                    if (checkHash)
                    {
                        string strHash = BLD.HexConverter.ByteArrayToHexString(SHA256Hash);
                        if (strHash != "0000000000000000000000000000000000000000000000000000000000000000")
                        {
                            if (strHash != BLD.HexConverter.ByteArrayToHexString(compareFile.SHA256Hash))
                            {
                                isMatch = false;
                            }
                        }
                    }
                }
                return isMatch;
            }
        }

    }
}
