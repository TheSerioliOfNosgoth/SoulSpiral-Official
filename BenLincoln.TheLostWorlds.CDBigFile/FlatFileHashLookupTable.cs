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

        public string LookupHash(uint inHash)
        {
            if (_HashTable.Contains(inHash))
            {
                return (string)_HashTable[inHash];
            }
            return null;
        }

        public void LoadHashTable()
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
                                string hashKey = cl[0].Trim();
                                string hashValue = cl[1].Trim();
                                try
                                {
                                    uint hashKeyInt = uint.Parse(hashKey);
                                    if (!HashTable.Contains(hashKeyInt))
                                    {
                                        HashTable.Add(hashKeyInt, hashValue);
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
    }
}
