using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeGex2 : BigFileType
    {
        public BigFileTypeGex2()
            : base()
        {
            Name = "Gex2";
            Description = "Gex: Enter the Gecko (PlayStation)";
            MasterIndexType = IndexType.Gex2;
            HashLookupTable = new FlatFileHashLookupTable("Gex2", Path.Combine(mDLLPath, "Hashes-Gex2.txt"));
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
