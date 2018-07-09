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
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class FlatFileHashLookupTable
    {
        protected string _Name;
        protected string _Path;
        protected Hashtable _HashTable;

        #region Properties

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

        public Hashtable HashTable
        {
            get
            {
                return _HashTable;
            }
            set
            {
                _HashTable = value;
            }
        }


        #endregion

        public FlatFileHashLookupTable(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public virtual string LookupHash(string inHash)
        {
            string normalizedInHash = inHash.Trim().ToUpper();
            if (_HashTable.Contains(normalizedInHash))
            {
                return (string)_HashTable[normalizedInHash];
            }
            return null;
        }

        public virtual void LoadHashTable()
        {
            if (!(System.IO.File.Exists(Path)))
            {
                HashTable = new Hashtable();
                return;
                //throw new FileNotFoundException("Could not find the hash lookup file " + Path + ".");
            }

            HashTable = new Hashtable();

            try
            {
                FileStream iStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                StreamReader iReader = new StreamReader(iStream);

                //while (iStream.Position < iStream.Length)
                while (!iReader.EndOfStream)
                {
                    try
                    {
                        string currentLine = iReader.ReadLine();
                        if (currentLine.Trim() != "")
                        {
                            string[] cl = currentLine.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (cl.Length > 1)
                            {
                                string hashKey = cl[0].Trim().ToUpper();
                                string hashValue = cl[1].Trim();
                                try
                                {
                                    //uint hashKeyInt = uint.Parse(hashKey);
                                    if (!HashTable.Contains(hashKey))
                                    {
                                        HashTable.Add(hashKey, hashValue);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Debug: couldn't parse '" + hashKey + "' as an integer. " + ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Debug: exception thrown: " + ex.Message);
                    }
                }
            }
            catch (IOException ex)
            {
                HashTable = new Hashtable();
                throw new HashTableLoadException("An I/O error occurred while reading the hash lookup file " + Path + ". The specific error was:\r\n" + ex.Message);
            }

        }

        protected virtual string ReplaceSpecialCharacters(string inString)
        {
            string result = inString;
            result = result.Replace(':', '\\');
            result = result.Replace('/', '\\');
            return result;
        }
    }
}
