using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypePandemonium : BigFileType
    {
        public BigFileTypePandemonium()
            : base()
        {
            Name = "Pandemonium";
            Description = "Pandemonium / Pandemonium 2 / The Unholy War (PlayStation)";
            MasterIndexType = IndexType.Pandemonium;
            //HashLookupTable = new FlatFileHashLookupTable("BO1", Path.Combine(mDLLPath, "Hashes-BO1.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_4),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_8),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_16),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_24),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_PlayStation_SEQ),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_VAB),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_VAG),
                new FileType()
            };
        }
    }
}
