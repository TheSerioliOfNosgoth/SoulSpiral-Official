// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2018 Ben Lincoln
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
    public class SR1Proto1CompressedFile : BF.CompressedFile
    {
        public SR1Proto1CompressedFile(BF.BigFile parent, BF.Index parentIndex, uint[] rawIndexData, int hashNamePosition, int offsetPosition, int lengthPosition, int compressedLengthPosition)
            : base(parent, parentIndex, rawIndexData, hashNamePosition, offsetPosition, lengthPosition, compressedLengthPosition)
        {
        }

        public SR1Proto1CompressedFile(BF.BigFile parent, BF.Index parentIndex, uint[] rawIndexData, string hashedName, int offsetPosition, int lengthPosition, int compressedLengthPosition)
            : base(parent, parentIndex, rawIndexData, hashedName, offsetPosition, lengthPosition, compressedLengthPosition)
        {
        }

        public enum DecompressorState
        {
            ReadChunkLayout,
            ProcessCommand,
            ReadLiteralByte
        }

        public enum BlockType
        {
            LiteralByte,
            Command
        }

        public static byte ReverseBits(byte inByte)
        {
            byte result = 0x00;

            byte temp = (byte)((inByte & 0x80) >> 7);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x40) >> 5);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x20) >> 3);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x10) >> 1);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x08) << 1);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x04) << 3);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x02) << 5);
            result = (byte)(result | temp);
            temp = (byte)((inByte & 0x01) << 7);
            result = (byte)(result | temp);

            return result;
        }

        public static BlockType[] GetBlockTypesFromByte(byte inByte)
        {
            BlockType[] result = new BlockType[8];

            byte[] bitMasks = new byte[] {0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01};
            int[] bitShifts = new int[] { 7, 6, 5, 4, 3, 2, 1, 0 };

            byte reversedByte = ReverseBits(inByte);

            for (int i = 0; i < 8; i++)
            {
                byte temp = (byte)((reversedByte & bitMasks[i]) >> bitShifts[i]);
                if (temp == 0x01)
                {
                    result[i] = BlockType.Command;
                }
                else
                {
                    result[i] = BlockType.LiteralByte;
                }
            }

            return result;
        }

        public static BlockType[] GetBlockTypesFromBytes(byte byte1, byte byte2)
        {
            BlockType[] result = new BlockType[16];

            BlockType[] r1 = GetBlockTypesFromByte(byte1);
            BlockType[] r2 = GetBlockTypesFromByte(byte2);

            for (int i = 0; i < 8; i++)
            {
                result[i] = r1[i];
            }
            for (int i = 0; i < 8; i++)
            {
                result[i + 8] = r2[i];
            }

            return result;
        }

        //exports this file
        public override void Export(string path)
        {
            FileStream iStream;
            BinaryReader iReader;
            FileStream oStream;

            oStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            ExportToStream(oStream);

            oStream.Close();
        }
        
        //exports this file
        public override void ExportToStream(Stream oStream)
        {
            //FileStream iStream;
            //FileStream oStream;
            //byte[] byteBuffer = new byte[mLength];

            //iStream = new FileStream(mParentBigFile.Path, FileMode.Open, FileAccess.Read);
            //oStream = new FileStream(path, FileMode.Create, FileAccess.Write);

            //iStream.Seek(mOffset, SeekOrigin.Begin);
            //iStream.Read(byteBuffer, 0, CompressedLength);
            //oStream.Write(byteBuffer, 0, CompressedLength);

            //oStream.Close();
            //iStream.Close();

            FileStream iStream;
            BinaryReader iReader;
            //FileStream oStream;
            byte[] byteBuffer;
            byte[] decompressedData;

            //read in the compressed data
            iStream = new FileStream(mParentBigFile.Path, FileMode.Open, FileAccess.Read);
            iReader = new BinaryReader(iStream);
            iStream.Seek(mOffset, SeekOrigin.Begin);

            //decompress the data into RAM
            int decompressedAlloc = Length;
            decompressedData = new byte[decompressedAlloc];
            for (int i = 0; i < decompressedData.Length; i++)
            {
                decompressedData[i] = 0x00;
            }


            int decompressedIndex = 0;
            int compressedIndex = 0;

            do
            {
                // this compression algorithm uses the following scheme:
                // data is arranged in chunks of 16 literal bytes or LZSS-style backreferences to data which has already been decompressed
                // The chunk layout is represented as a bit flag array across two bytes
                // 1 == this space is a 2-byte command
                // 0 == this space is a 1-byte literal
                // They're reversed, for some reason, so if the layout is:
                // 0x2006
                // That's 00100000 00000110 in binary
                // Reverse the values in each byte to get:
                // 00000100 01100000
                // ...which means "read 5 literal bytes, then 1 2-byte command, then 3 literal bytes, then 2 2-byte commands, then 5 literal bytes
                // Commands are LZSS-style backreferences, where the second byte is the number of bytes to go back in the decompressed data, 
                // and the first byte is the number of bytes to copy starting at that location, minus 1
                // so 0x0204 means "go back 4 bytes, then copy three bytes"
                // 0x0402 means "go back 2 bytes, then copy five bytes"
                // as you may have guessed from that second example, like LZSS, it's possible for the number of bytes to copy to extend past 
                // the length of decompressed data when the copy started. e.g. if the last five bytes decompressed were 0xAABBCCDDEEFF, and 
                // the command 0x0502 followed that, then the decompressor would go back two bytes, repeat those two to give:
                // 0xAABBCCDDEEFFEEFF
                // then continue copying the newly-written bytes until six had been written in total:
                // 0xAABBCCDDEEFFEEFFEEFFEEFF
                //get the chunk layout
                byte layoutByte1 = iReader.ReadByte();
                byte layoutByte2 = iReader.ReadByte();
                compressedIndex += 2;

                BlockType[] currentLayout = GetBlockTypesFromBytes(layoutByte1, layoutByte2);

                for (int i = 0; i < currentLayout.Length; i++)
                {
                    if (decompressedIndex == 560)
                    {
                        Console.WriteLine("Debug here");
                    }
                    if (currentLayout[i] == BlockType.Command)
                    {
                        if (decompressedIndex < Length)
                        {
                            byte controlByte1 = iReader.ReadByte();
                            byte controlByte2 = iReader.ReadByte();
                            compressedIndex += 2;
                            // if any of the four most-significant bits of byte 1 are set, the control block is handled differently.
                            if ((controlByte1 & 0xF0) != 0)
                            {
                                bool handled = false;
                                int blockLength = 0;
                                int numBlocksToCopy = 0;
                                int numBytesToCopy = 0;
                                int startOffset = 0;
                                ushort distanceBack = 0;

                                //if ((controlByte1 & 0x80) == 0x80)
                                //{
                                //    handled = true;
                                //}

                                //if ((!handled) && (controlByte1 == 0x12) && (controlByte2 == 0x18))
                                //{
                                //    handled = true;
                                //    //blockLength = 1;
                                //    numBytesToCopy = 3;
                                //    distanceBack = 0x118;
                                //}

                                //if ((!handled) && (controlByte1 == 0x12) && (controlByte2 == 0x9C))
                                //{
                                //    handled = true;
                                //    //blockLength = 1;
                                //    numBytesToCopy = 2;
                                //    distanceBack = 0x19C;
                                //}

                                //if ((!handled) && (controlByte1 == 0x12) && (controlByte2 == 0x9C))
                                //{
                                //    handled = true;
                                //    //blockLength = 1;
                                //    numBytesToCopy = ((controlByte1 & 0xFC) >> 2) + 1;
                                //    distanceBack = controlByte2;
                                //    distanceBack |= (ushort)((ushort)(controlByte1 & 0x03) << 8);
                                // }

                                if (!handled)
                                {
                                    handled = true;
                                    //numBlocksToCopy = ((controlByte1 & 0xF0) >> 4);
                                    //blockLength = ((controlByte1 & 0x0C) >> 2);
                                    numBytesToCopy = ((controlByte1 & 0x0F)) + 1;
                                    distanceBack = controlByte2;
                                    distanceBack |= (ushort)((ushort)(controlByte1 & 0xF0) << 4);
                                }

                                if (handled)
                                {
                                    startOffset = decompressedIndex - distanceBack;

                                    //for (int j = 0; j < numBytesToCopy; j += 2)
                                    for (int j = 0; j < numBytesToCopy; j++)
                                    {
                                        if (decompressedIndex < Length)
                                        {
                                            if ((startOffset + j) >= 0)
                                            {
                                                decompressedData[decompressedIndex] = decompressedData[startOffset + j];
                                                //decompressedData[decompressedIndex + 1] = decompressedData[startOffset + j + 1];
                                                //decompressedIndex += 2;
                                                decompressedIndex++;
                                            }
                                        }
                                    }
                                }

                                //if ((controlByte1 == (byte)0x15) && (controlByte2 == (byte)0x54))
                                //{
                                //    handled = true;
                                //} 
                                //if ((controlByte1 == (byte)0x83) && (controlByte2 == (byte)0x04))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0x85) && (controlByte2 == (byte)0x22))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0x8F) && (controlByte2 == (byte)0x0A))
                                //{
                                //    handled = true;
                                //    //decompressedData[decompressedIndex] = decompressedData[decompressedIndex - 2];
                                //    //decompressedData[decompressedIndex + 1] = decompressedData[decompressedIndex - 1];
                                //    //decompressedIndex += 2;
                                //}
                                //if ((controlByte1 == (byte)0xB2) && (controlByte2 == (byte)0xCC))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xB2) && (controlByte2 == (byte)0xF8))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xB3) && (controlByte2 == (byte)0xD8))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xB3) && (controlByte2 == (byte)0xFC))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xB5) && (controlByte2 == (byte)0x0A))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xB7) && (controlByte2 == (byte)0xC6))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xC2) && (controlByte2 == (byte)0x3E))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xC4) && (controlByte2 == (byte)0x0A))
                                //{
                                //    handled = true;
                                //}
                                //if ((controlByte1 == (byte)0xCF) && (controlByte2 == (byte)0x00))
                                //{
                                //    handled = true;
                                //}
                                //if (!handled)
                                //{
                                //    //throw new Exception(string.Format("Unhandled: {0:X2} {1:X2}", controlByte1, controlByte2));
                                //    Console.WriteLine(string.Format("Unhandled: {0:X2} {1:X2}", controlByte1, controlByte2));
                                //}
                            }
                            else
                            {
                                int startOffset = decompressedIndex - controlByte2;
                                int numBytesToCopy = controlByte1 + 1;
                                for (int j = 0; j < numBytesToCopy; j++)
                                {
                                    if (decompressedIndex < Length)
                                    {
                                        decompressedData[decompressedIndex] = decompressedData[startOffset + j];
                                        decompressedIndex++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //iReader.BaseStream.Seek(compressedIndex, SeekOrigin.Begin);
                        if (decompressedIndex < Length)
                        {
                            byte literal = iReader.ReadByte();
                            compressedIndex++;
                            decompressedData[decompressedIndex] = literal;
                            decompressedIndex++;
                        }
                    }
                }
            } while (decompressedIndex < Length);

            iReader.Close();
            iStream.Close();

            if (decompressedIndex != Length)
            {
                throw new BF.DecompressionException("The uncompressed length of " + mName + " does not match the length listed in the index.");
            }

            //oStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            oStream.Write(decompressedData, 0, decompressedIndex);

            oStream.Close();
        }

    }
}
