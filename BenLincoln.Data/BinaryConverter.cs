// BenLincoln.Data
// Copyright 2007-2018 Ben Lincoln
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
    public class BinaryConverter
    {
        public static uint ByteArrayToUInt(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new FormatException("Only arrays of four bytes may be converted to a uint");
            }
            uint result = 0;

            result |= (uint)((uint)bytes[0] << 24);
            result |= (uint)((uint)bytes[1] << 16);
            result |= (uint)((uint)bytes[2] << 8);
            result |= (uint)((uint)bytes[3]);

            return result;
        }

        public static byte[] UIntToByteArray(uint u)
        {
            byte[] result = new byte[4];

            uint ub1 = u & 0xFF000000;
            uint ub2 = u & 0x00FF0000;
            uint ub3 = u & 0x0000FF00;
            uint ub4 = u & 0x000000FF;

            result[0] = (byte)(ub1 >> 24);
            result[1] = (byte)(ub2 >> 16);
            result[2] = (byte)(ub3 >> 8);
            result[3] = (byte)(ub4);

            return result;
        }
    }
}
