using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeTombRaiderLegendPlayStation2Demo : BigFileType
    {
        public BigFileTypeTombRaiderLegendPlayStation2Demo()
            : base()
        {
            Name = "TRLPS2Demo";
            Description = "Tomb Raider: Legend Demo (PlayStation 2)";
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
