using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeSoulReaver2PC : BigFileType
    {
        public BigFileTypeSoulReaver2PC()
            : base()
        {
            Name = "SoulReaver2PC";
            Description = "Soul Reaver 2 (PC)";
            MasterIndexType = IndexType.SR2PC;
            HashLookupTable = new FlatFileHashLookupTable("SR2", Path.Combine(mDLLPath, "Hashes-SR2.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_STR_SR2_PC),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                new FileType()
            };
        }
    }
}
