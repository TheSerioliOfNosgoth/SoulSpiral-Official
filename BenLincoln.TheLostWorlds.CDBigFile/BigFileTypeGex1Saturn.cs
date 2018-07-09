using System;
using System.Collections.Generic;
using System.Text;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileTypeGex1Saturn : BigFileType
    {
        public BigFileTypeGex1Saturn()
            : base()
        {
            Name = "Gex1Saturn";
            Description = "Gex (Saturn)";
            MasterIndexType = IndexType.Gex1Saturn;
            FileTypes = new FileType[]
            {
                new FileType()
            };
        }
    }
}
