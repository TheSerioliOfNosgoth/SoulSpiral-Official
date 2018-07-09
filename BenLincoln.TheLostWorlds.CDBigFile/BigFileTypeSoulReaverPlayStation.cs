using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeSoulReaverPlayStation : BigFileType
    {
        public BigFileTypeSoulReaverPlayStation()
            : base()
        {
            Name = "SoulReaverPlayStation";
            Description = "Soul Reaver (PlayStation - NTSC - Retail and Beta Versions)";
            MasterIndexType = IndexType.SR1PS1MainIndex;
            HashLookupTable = new FlatFileHashLookupTable("SR1", Path.Combine(mDLLPath, "Hashes-SR1.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR1_Object),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR1_Room),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_CRM_SR1),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_PMF),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_PNF),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_SMF),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_SNF),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_4),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_8),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_16),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_24),
                new FileType()
            };
        }
    }
}
