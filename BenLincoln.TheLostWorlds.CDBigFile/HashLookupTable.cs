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

// note: this class is no longer used as of Soul Spiral version 1.5 because Microsoft dropped
// support for the JET database engine in Windows 7. FlatFileHashLookupTable is used instead.
// Thanks, Microsoft! No one was using JET databases anyway!

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.OleDb;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class HashLookupTable
    {
        protected string mName;
        protected string mPath;
        protected string mTableName;
        protected Hashtable mHashTable;

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

        public string Path
        {
            get
            {
                return mPath;
            }
            set
            {
                mPath = value;
            }
        }

        public string TableName
        {
            get
            {
                return mTableName;
            }
            set
            {
                mTableName = value;
            }
        }

        public Hashtable HashTable
        {
            get
            {
                return mHashTable;
            }
            set
            {
                mHashTable = value;
            }
        }


        #endregion

        public HashLookupTable(string name, string path, string tableName)
        {
            Name = name;
            Path = path;
            TableName = tableName;
            //LoadHashTable();
        }

        public string LookupHash(uint inHash)
        {
            if (mHashTable.Contains(inHash))
            {
                return (string)mHashTable[inHash];
            }
            return null;
        }

        public void LoadHashTable()
        {
            if (!(System.IO.File.Exists(mPath)))
            {
                throw new FileNotFoundException("Could not find the database file " + mPath + ".");
            }

            mHashTable = new Hashtable();

            OleDbConnection hashConn;
            OleDbCommand hashCommand;
            OleDbDataReader hashReader;

            hashConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mPath);
            hashCommand = new OleDbCommand("select HashNum, Path from " + mTableName, hashConn);
            try
            {
                hashConn.Open();
                hashReader = hashCommand.ExecuteReader();
                while (hashReader.Read())
                {
                    uint tHashNum = (uint)(hashReader.GetDouble(0));
                    string tPath = hashReader.GetString(1);
                    mHashTable.Add(tHashNum, tPath);
                }
                hashReader.Close();
                hashConn.Close();
            }
            catch (OleDbException ex)
            {
                mHashTable = new Hashtable();
                throw new HashTableLoadException("An OLEDB error occurred while reading the hash lookup table " + mTableName +
                    " from the database file " + mPath + ". The specific error was:\r\n" + ex.Message);
            }
            catch (IOException ex)
            {
                mHashTable = new Hashtable();
                throw new HashTableLoadException("An I/O error occurred while reading the hash lookup table " + mTableName +
                    " from the database file " + mPath + ". The specific error was:\r\n" + ex.Message);
            }

        }

    }
}
