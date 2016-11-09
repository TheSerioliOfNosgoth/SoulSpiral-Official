// BenLincoln.Data
// Copyright 2007-2012 Ben Lincoln
// http://www.thelostworlds.net/
//

// This file is part of BenLincoln.Data.

// BenLincoln.Data is free software: you can redistribute it and/or modify
// it under the terms of version 3 of the GNU General Public License as published by
// the Free Software Foundation.

// BenLincoln.Data is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with BenLincoln.Data (in the file LICENSE.txt).  
// If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.Data
{
    public enum Endianness
    {
        Little,
        Big
    }

    public enum ByteSwapMode
    {
        UInt16,
        UInt32
    }

    public abstract class EndianConverter
    {
        public static uint ReverseUInt(uint inVal)
        {
            uint byte1 = inVal & 0xFF;
            uint byte2 = inVal & 0xFF00;
            uint byte3 = inVal & 0xFF0000;
            uint byte4 = inVal & 0xFF000000;

            byte1 <<= 24;
            byte2 <<= 8;
            byte3 >>= 8;
            byte4 >>= 24;

            return byte1 | byte2 | byte3 | byte4;
        }

        public static ushort ReverseUShort(ushort inVal)
        {
            ushort byte1 = (ushort)(inVal & 0xFF);
            ushort byte2 = (ushort)(inVal & 0xFF00);

            byte1 <<= 8;
            byte2 >>= 8;

            return (ushort)(byte1 | byte2);
        }

        public static byte[] SwapBytes(byte[] bytes, ByteSwapMode mode)
        {
            byte[] outBytes = new byte[bytes.Length];

            switch (mode)
            {
                case ByteSwapMode.UInt16:
                    for (int i = 0; i < bytes.Length; i += 2)
                    {
                        outBytes[i] = bytes[i + 1];
                        outBytes[i + 1] = bytes[i];
                    }
                    break;
                case ByteSwapMode.UInt32:
                    for (int i = 0; i < bytes.Length; i += 4)
                    {
                        outBytes[i] = bytes[i + 3];
                        outBytes[i + 1] = bytes[i + 2];
                        outBytes[i + 2] = bytes[i + 1];
                        outBytes[i + 3] = bytes[i];
                    }
                    break;
            }

            return outBytes;
        }
    }
}
