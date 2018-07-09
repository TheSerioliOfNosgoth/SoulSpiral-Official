using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeSoulReaver2PlayStation2 : BigFileType
    {
        public BigFileTypeSoulReaver2PlayStation2()
            : base()
        {
            Name = "SoulReaver2PS2";
            Description = "Soul Reaver 2 (PlayStation 2)";
            MasterIndexType = IndexType.SR2PS2;
            HashLookupTable = new FlatFileHashLookupTable("SR2", Path.Combine(mDLLPath, "Hashes-SR2.txt"));
            FileTypes = new FileType[]
            {
                //BF.FileType.FromType(BF.FileType.FILE_TYPE_STR_SR2_PS2),
                //BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAW_SR2_PS2),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR2_Room_RETAIL),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_DRM_SR2_Object),
                new FileType()
            };
        }
    }
}
