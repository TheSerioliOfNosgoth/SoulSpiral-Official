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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class CompressedFile : BF.File
    {
        protected int mCompressedLength;

        #region Properties

        public int CompressedLength
        {
            get
            {
                return mCompressedLength;
            }
            set
            {
                mCompressedLength = value;
            }
        }

        #endregion

        public CompressedFile(BF.BigFile parent, BF.Index parentIndex, uint[] rawIndexData, int hashNamePosition, int offsetPosition, int lengthPosition, int compressedLengthPosition)
            : base(parent, parentIndex, rawIndexData, hashNamePosition, offsetPosition, lengthPosition)
        {
            mCompressedLength = (int)rawIndexData[compressedLengthPosition];
            mCanBeReplaced = false;
        }

        public CompressedFile(BF.BigFile parent, BF.Index parentIndex, uint[] rawIndexData, uint hashedName, int offsetPosition, int lengthPosition, int compressedLengthPosition)
            : base(parent, parentIndex, rawIndexData, hashedName, offsetPosition, lengthPosition)
        {
            mCompressedLength = (int)rawIndexData[compressedLengthPosition];
            mCanBeReplaced = false;
        }

        protected override string GetGenericInfo()
        {
            return
                "Compressed File Information\r\n---\r\n" +
                "Name: " + mName + "." + mFileExtension + "\r\n" +
                "Offset: " + mOffset + "\r\n" +
                "Compressed Length: " + mCompressedLength + "\r\n" +
                "Uncompressed Length: " + mLength + "\r\n" +
                "Type Name: " + mType.Name + "\r\n" +
                "Type Description: " + mType.Description + "\r\n"
                ;
        }

        //exports this file
        public override void Export(string path)
        {
            FileStream iStream;
            BinaryReader iReader;
            FileStream oStream;
            byte[] byteBuffer;
            int sectionNumber = 0;
            long currentSectionAStart = 0;
            long currentSectionAEnd = 0;
            int currentSectionALength = 0;
            long currentSectionBStart = 0;
            long currentSectionBEnd = 0;
            int currentSectionBLength = 0;
            int sectionCount = 0;
            byte[] decompressedData;

            //read in the compressed data
            iStream = new FileStream(mParentBigFile.Path, FileMode.Open, FileAccess.Read);
            iReader = new BinaryReader(iStream);
            iStream.Seek(mOffset, SeekOrigin.Begin);

            //read in all the codebooks and dictionaries
            ArrayList codebooks = new ArrayList();
            ArrayList dictionaries = new ArrayList();
            do
            {
                //catch 0-spaced or not markers
                currentSectionBLength = (int)iReader.ReadUInt32();
                if ((currentSectionBLength < 1) || (currentSectionBLength > 98304))
                {
                    iStream.Seek(-2, SeekOrigin.Current);
                    currentSectionBLength = (int)iReader.ReadUInt32();
                }
                currentSectionALength = Math.Max(0, iReader.ReadUInt16() - 2);
                //sections must be an even number of bytes long
                if ((currentSectionALength % 2) == 1)
                {
                    currentSectionALength++;
                }
                //if ((currentSectionBLength % 2) == 1)
                //{
                //    currentSectionBLength++;
                //}
                currentSectionAStart = iStream.Position;
                currentSectionAEnd = currentSectionAStart + currentSectionALength;
                currentSectionBStart = currentSectionALength + currentSectionAStart;
                currentSectionBLength -= (currentSectionALength + 2);
                currentSectionBEnd = currentSectionBStart + currentSectionBLength;

                if (currentSectionBLength > 0)
                {
                    //get the code book for this section
                    byte[] currentCodebook = GetByteArrayFromFile(iStream, currentSectionAStart, currentSectionALength);
                    //get the dictionary for this section
                    byte[] currentDictionary = GetByteArrayFromFile(iStream, currentSectionBStart, currentSectionBLength);

                    codebooks.Add(currentCodebook);
                    dictionaries.Add(currentDictionary);

                    sectionNumber++;
                }
            } while (iStream.Position < mOffset + mCompressedLength - 3);

            iReader.Close();
            iStream.Close();

            sectionCount = sectionNumber;

            //decompress the data into RAM
            int decompressedAlloc = 65536 * sectionCount;
            if (decompressedAlloc > ((Math.Max(65536, mLength * 4))))    //1073741824 = 1GB
            {
                throw new Exception("Decompressed data is calculated at greater than four times the size listed in the index. This is unlikely to correct.");
            }
            decompressedData = new byte[decompressedAlloc];

            //process the codebook
            int decompressedIndex = 0;
            //sectionCount = 1;
            for (int i = 0; i < sectionCount; i++)
            {
                byte[] currentCodeBook = (byte[])codebooks[i];
                byte[] currentDictionary = (byte[])dictionaries[i];
                //process the current codebook
                int codebookIndex = 0;
                int dictionaryIndex = 0;
                int codebookLength = currentCodeBook.GetUpperBound(0);
                //int codebookLength = 1024;
                do
                {
                    //get the current byte
                    ushort currentData = (ushort)currentCodeBook[codebookIndex];
                    //if the byte doesn't have the MSB set, read blocks from the dictionary
                    if ((currentData & 0x80) == 0)
                    {
                        int numBlocks = (int)currentData + 1;
                        for (int j = 0; j < numBlocks; j++)
                        {
                            decompressedData[decompressedIndex] = currentDictionary[dictionaryIndex];
                            decompressedData[decompressedIndex + 1] = currentDictionary[dictionaryIndex + 1];
                            decompressedIndex += 2;
                            dictionaryIndex += 2;
                        }
                        codebookIndex++;
                    }
                    //otherwise, process it as a repetition of a chunk of decompressed data
                    else
                    {
                        //get the second byte
                        currentData <<= 8;
                        currentData |= (ushort)currentCodeBook[codebookIndex + 1];
                        //figure out which type of 2-byte code it is, and get the parameters
                        ushort backBlocks = 0;
                        ushort writeBlocks = 0;
                        if ((currentData & 0xC000) == 0xC000)
                        {
                            backBlocks = (ushort)(currentData & 0x0F);
                            writeBlocks = (ushort)(((currentData & 0x3FF0) >> 4) + 2);
                        }
                        else
                        {
                            backBlocks = (ushort)(currentData & 0x07FF);
                            writeBlocks = (ushort)(((currentData & 0x3800) >> 11) + 2);
                        }
                        //copy the data
                        int copyIndex = decompressedIndex - (backBlocks * 2);
                        for (int j = 0; j < writeBlocks; j++)
                        {
                            decompressedData[decompressedIndex] = decompressedData[copyIndex + (j * 2)];
                            decompressedData[decompressedIndex + 1] = decompressedData[copyIndex + (j * 2) + 1];
                            decompressedIndex += 2;
                        }
                        codebookIndex += 2;
                    }
                } while (codebookIndex < codebookLength);
                //if there is anything left in the dictionary, write it
                if (dictionaryIndex <= currentDictionary.GetUpperBound(0))
                {
                    for (int j = dictionaryIndex; j <= currentDictionary.GetUpperBound(0); j++)
                    {
                        decompressedData[decompressedIndex] = currentDictionary[j];
                        decompressedIndex++;
                    }
                }
            }

            if (decompressedIndex != mLength)
            {
                throw new BF.DecompressionException("The uncompressed length of " + mName + " does not match the length listed in the index.");
            }

            oStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            oStream.Write(decompressedData, 0, decompressedIndex);

            oStream.Close();
        }

        protected byte[] GetByteArrayFromFile(FileStream inStream, long startOffset, int numBytes)
        {
            byte[] byteBuffer = new byte[numBytes];
            inStream.Seek(startOffset, SeekOrigin.Begin);
            inStream.Read(byteBuffer, 0, numBytes);
            return byteBuffer;
        }

    }
}
