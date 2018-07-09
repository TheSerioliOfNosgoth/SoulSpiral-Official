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
    public class HexConverter
    {
        public static byte[] HexStringToBytes(string inputString)
        {
            if ((inputString.Length % 2) != 0)
            {
                throw new FormatException("Input length must be a multiple of 2");
            }
            byte[] result = new byte[(inputString.Length / 2)];

            int byteNum = 0;
            for (int i = 0; i < inputString.Length; i += 2)
            {
                string currentBlock = inputString.Substring(i, 2);
                try
                {
                    byte currentByte = byte.Parse(currentBlock, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                    result[byteNum] = currentByte;
                    byteNum++;
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("Could not parse '{0}' as a byte in hexadecimal format.", currentBlock));
                }
            }

            return result;
        }

        public static string ByteArrayToHexString(byte[] inputArray)
        {
            StringBuilder builder = new StringBuilder();

            foreach (byte b in inputArray)
            {
                builder.Append(string.Format("{0:X2}", b).ToUpper());
            }

            return builder.ToString();
        }
    }
}
