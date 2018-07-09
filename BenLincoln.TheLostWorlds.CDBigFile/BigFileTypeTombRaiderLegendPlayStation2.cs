using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeTombRaiderLegendPlayStation2 : BigFileType
    {
        public BigFileTypeTombRaiderLegendPlayStation2()
            : base()
        {
            Name = "TRLPS2";
            Description = "Tomb Raider: Legend (PlayStation 2/PSP)";
            MasterIndexType = IndexType.TRLPS2;
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_MUL_Defiance),
                new FileType()
            };
        }
    }
}
