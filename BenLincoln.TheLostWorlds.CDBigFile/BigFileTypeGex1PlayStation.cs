using System;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeGex1PlayStation : BigFileType
    {
        public BigFileTypeGex1PlayStation()
            : base()
        {
            Name = "Gex1PlayStation";
            Description = "Gex (PlayStation)";
            MasterIndexType = IndexType.Gex1PlayStation;
            FileTypes = new FileType[]
            {
                new FileType()
            };
        }
    }
}
