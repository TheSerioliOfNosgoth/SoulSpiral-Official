using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;
namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeDefiancePlayStation2 : BigFileType
    {
        public BigFileTypeDefiancePlayStation2()
            : base()
        {
            Name = "DefiancePS2";
            Description = "Legacy of Kain: Defiance (PlayStation 2)";
            MasterIndexType = IndexType.SR2PC;
            HashLookupTable = new FlatFileHashLookupTable("Defiance", Path.Combine(mDLLPath, "Hashes-Defiance.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_MUL_Defiance),
                new FileType()
            };
        }
    }
}
