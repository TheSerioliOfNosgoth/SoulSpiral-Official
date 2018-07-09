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
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeAkuji : BigFileType
    {
        public BigFileTypeAkuji()
            : base()
        {
            Name = "Akuji";
            Description = "Akuji: the Heartless (PlayStation)";
            MasterIndexType = IndexType.Gex2;
            HashLookupTable = new FlatFileHashLookupTable("Akuji", Path.Combine(mDLLPath, "Hashes-Akuji.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_SND_Akuji),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_4),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_8),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_16),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_24),
                new FileType()
            };
        }
    }
}
