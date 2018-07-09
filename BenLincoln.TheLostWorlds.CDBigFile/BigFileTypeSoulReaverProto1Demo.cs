using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeSoulReaverProto1Demo : BigFileType
    {
        public BigFileTypeSoulReaverProto1Demo()
            : base()
        {
            Name = "SoulReaverProto1Demo";
            Description = "Soul Reaver Proto1/Lighthouse Demo (PlayStation)";
            MasterIndexType = IndexType.Gex2;
            HashLookupTable = new FlatFileHashLookupTable("SR1Proto1", Path.Combine(mDLLPath, "Hashes-SR1_Proto1.txt"));
            FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_SND_Akuji),
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
