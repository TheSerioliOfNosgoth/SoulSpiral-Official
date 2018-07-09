using System;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeSoulReaverPlayStationPALRetail : BigFileTypeSoulReaverPlayStation
    {
        public BigFileTypeSoulReaverPlayStationPALRetail()
            : base()
        {
            Name = "SoulReaverPlayStationPALRetail";
            Description = "Soul Reaver (PlayStation - PAL Retail)";
            MasterIndexType = IndexType.SR1PS1PALRetailMainIndex;
        }
    }
}
