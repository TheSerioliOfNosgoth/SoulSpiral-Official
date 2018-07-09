using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeSoulReaver2AirForgeDemo : BigFileType
    {
        public BigFileTypeSoulReaver2AirForgeDemo()
            : base()
        {
            Name = "SoulReaver2AirForgeDemo";
            Description = "Soul Reaver 2 Air Forge Demo (PlayStation 2)";
            MasterIndexType = IndexType.SR2AirForgeDemo;
            HashLookupTable = new FlatFileHashLookupTable("SR2AirForge", Path.Combine(mDLLPath, "Hashes-SR2_Air_Forge.txt"));
            FileTypes = new FileType[]
            {
               BF.FileType.FromType(BF.FileType.FILE_TYPE_RAW_SR2_PS2),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR2_Room_DEMO_NTSC),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR2_Room_DEMO_PAL),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR2_Object),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_STR_SR2_PS2),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_SNF),
               //BF.FileType.FromType(BF.FileType.FILE_TYPE_VRM),
               new FileType()
            };
        }
    }
}
