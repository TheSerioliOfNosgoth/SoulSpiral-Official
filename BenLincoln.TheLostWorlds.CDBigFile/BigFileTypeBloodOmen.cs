using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeBloodOmen : BigFileType
    {
        public BigFileTypeBloodOmen()
            : base()
        {
            Name = "BloodOmen";
            Description = "Blood Omen (PlayStation/PC)";
            MasterIndexType = IndexType.BloodOmen;
            HashLookupTable = new FlatFileHashLookupTable("BO1", Path.Combine(mDLLPath, "Hashes-BO1.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_4),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_8),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_16),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_24),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_VAB_Headerless),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_VAG_Headerless),
                new FileType()
            };
        }
    }
}
