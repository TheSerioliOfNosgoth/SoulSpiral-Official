using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;
using BLD = BenLincoln.Data;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BloodOmen2HashLookupTable : BF.FlatFileHashLookupTable
    {
        public BloodOmen2HashLookupTable(string name, string path)
            : base(name, path)
        {
        }

        public override string LookupHash(string inHash)
        {

            // If I used the whole thing, the hash would be unique, but some files would be be missing their names.

            // use just the last two bytes under the new model of matching against an ASCII hex string;
            byte[] bInHash = BLD.HexConverter.HexStringToBytes(inHash);
            if (bInHash.Length > 3)
            {
                string inHash2 = string.Format("0000{0:X2}{1:X2}", bInHash[2], bInHash[3]).ToUpper();
                if (_HashTable.Contains(inHash2))
                {
                    return ReplaceSpecialCharacters((string)_HashTable[inHash2]);
                }
            }
            
            return null;
        }

        public override void LoadHashTable()
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
                BinaryReader iReader = new BinaryReader(iStream);

                iStream.Position += 0x20;

                uint index = 0;
                int numLines = iReader.ReadInt32();

                while (index < numLines && iStream.Position < iStream.Length)
                {
                    string currentLine = "";
                    while (iStream.Position < iStream.Length)
                    {
                        char ch = (char)iReader.ReadByte();
                        if (ch == 0)
                        {
                            break;
                        }

                        if (ch == '/')
                        {
                            ch = '\\';
                        }

                        currentLine += ch;
                    }

                    if (currentLine.Trim() != "")
                    {
                        string hashKey = BLD.HexConverter.ByteArrayToHexString(BLD.BinaryConverter.UIntToByteArray(index));
                        string hashValue = currentLine;
                        if (!HashTable.Contains(hashKey))
                        {
                            HashTable.Add(hashKey, hashValue);
                        }
                    }

                    index++;
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