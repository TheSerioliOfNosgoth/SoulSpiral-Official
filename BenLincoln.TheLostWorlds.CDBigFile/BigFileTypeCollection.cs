using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeCollection
    {
        protected Hashtable mBigFileTypeHash;
        protected string[] mBigFileTypeNames;

        # region Properties

        public Hashtable BigFileTypeHash
        {
            get
            {
                return mBigFileTypeHash;
            }
            set
            {
                mBigFileTypeHash = value;
            }
        }

        public string[] BigFileTypeNames
        {
            get
            {
                return mBigFileTypeNames;
            }
            set
            {
                mBigFileTypeNames = value;
            }
        }

        #endregion

        public BigFileTypeCollection()
        {
            mBigFileTypeHash = new Hashtable();
            mBigFileTypeNames = new string[] {};
        }

        public BigFileType GetTypeByName(string typeName)
        {
            return (BigFileType)BigFileTypeHash[typeName];
        }
    }
}
