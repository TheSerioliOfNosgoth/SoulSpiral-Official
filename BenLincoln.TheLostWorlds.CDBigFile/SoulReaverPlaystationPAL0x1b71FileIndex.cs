using System;
using System.Collections.Generic;
using System.Text;

using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    /* Some prerelease PAL builds of the game use a different XOR key for the alternate index values */
    public class SoulReaverPlaystationPAL0x1b71FileIndex : SoulReaverPlaystationPALFileIndex
    {
        public SoulReaverPlaystationPAL0x1b71FileIndex(string name, BF.BigFile parentBigFile, BF.Index parentIndex, long offset)
            : base(name, parentBigFile, parentIndex, offset)
        {
            _XorValue16 = 0x1B71;
            _XorValue32 = 0x1B711B71;
        }
    }
}
