// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2012 Ben Lincoln
// http://www.thelostworlds.net/
//

// This file is part of BenLincoln.TheLostWorlds.CDBigFile.

// BenLincoln.TheLostWorlds.CDBigFile is free software: you can redistribute it and/or modify
// it under the terms of version 3 of the GNU General Public License as published by
// the Free Software Foundation.

// BenLincoln.TheLostWorlds.CDBigFile is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with BenLincoln.TheLostWorlds.CDBigFile (in the file LICENSE.txt).  
// If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFileType
    {
        protected string mName;
        protected string mDescription;
        protected IndexType mMasterIndexType;
        protected BF.FileType[] mFileTypes;
        protected BF.FlatFileHashLookupTable mHashLookupTable;
        protected string mDLLPath;

        #region Properties

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        public IndexType MasterIndexType
        {
            get
            {
                return mMasterIndexType;
            }
            set
            {
                mMasterIndexType = value;
            }
        }

        public BF.FileType[] FileTypes
        {
            get
            {
                return mFileTypes;
            }
            set
            {
                mFileTypes = value;
            }
        }

        public BF.FlatFileHashLookupTable HashLookupTable
        {
            get
            {
                return mHashLookupTable;
            }
            set
            {
                mHashLookupTable = value;
            }
        }

        #endregion

        public BigFileType()
        {
            mName = "Unknown";
            mDescription = "Unknown BigFile type";
            mMasterIndexType = IndexType.Unknown;
            mHashLookupTable = null;
            mDLLPath = this.GetType().Assembly.Location;
            mDLLPath = Path.GetDirectoryName(mDLLPath);
        }

        public virtual void LoadHashLookupTable(string bigfilePath)
        {
            // this is for subclasses that load their lookup tables from the bigfile itself
        }

        public static BigFileTypeCollection GetAllTypes()
        {
            BigFileTypeCollection result = new BigFileTypeCollection();
            ArrayList nameList = new ArrayList();

            BF.BigFileTypeAkuji bftAkuji = new BigFileTypeAkuji();
            result.BigFileTypeHash.Add(bftAkuji.Name, bftAkuji);
            nameList.Add(bftAkuji.Name);

            BF.BigFileTypeBloodOmen bftBloodOmen = new BigFileTypeBloodOmen();
            result.BigFileTypeHash.Add(bftBloodOmen.Name, bftBloodOmen);
            nameList.Add(bftBloodOmen.Name);

            BF.BigFileTypeMadDashRacingBigFile bftMadDashRacingBigFile = new BigFileTypeMadDashRacingBigFile();
            result.BigFileTypeHash.Add(bftMadDashRacingBigFile.Name, bftMadDashRacingBigFile);
            nameList.Add(bftMadDashRacingBigFile.Name);

            BF.BigFileTypeDefiancePC bftDefiancePC = new BigFileTypeDefiancePC();
            result.BigFileTypeHash.Add(bftDefiancePC.Name, bftDefiancePC);
            nameList.Add(bftDefiancePC.Name);

            BF.BigFileTypeDefiancePlayStation2 bftDefiancePlayStation2 = new BigFileTypeDefiancePlayStation2();
            result.BigFileTypeHash.Add(bftDefiancePlayStation2.Name, bftDefiancePlayStation2);
            nameList.Add(bftDefiancePlayStation2.Name);

            BF.BigFileTypeGex1PlayStation bftGex1PlayStation = new BigFileTypeGex1PlayStation();
            result.BigFileTypeHash.Add(bftGex1PlayStation.Name, bftGex1PlayStation);
            nameList.Add(bftGex1PlayStation.Name);

            BF.BigFileTypeGex1Saturn bftGex1Saturn = new BigFileTypeGex1Saturn();
            result.BigFileTypeHash.Add(bftGex1Saturn.Name, bftGex1Saturn);
            nameList.Add(bftGex1Saturn.Name);

            BF.BigFileTypeGex2 bftGex2 = new BigFileTypeGex2();
            result.BigFileTypeHash.Add(bftGex2.Name, bftGex2);
            nameList.Add(bftGex2.Name);

            BF.BigFileTypeGex3 bftGex3 = new BigFileTypeGex3();
            result.BigFileTypeHash.Add(bftGex3.Name, bftGex3);
            nameList.Add(bftGex3.Name);

            /* BF.BigFileTypePandemonium bftPandemonium = new BigFileTypePandemonium();
            result.BigFileTypeHash.Add(bftPandemonium.Name, bftPandemonium);
            nameList.Add(bftPandemonium.Name); */

            BF.BigFileTypeSoulReaverProto1Demo bftSoulReaverProto1Demo = new BigFileTypeSoulReaverProto1Demo();
            result.BigFileTypeHash.Add(bftSoulReaverProto1Demo.Name, bftSoulReaverProto1Demo);
            nameList.Add(bftSoulReaverProto1Demo.Name);

            BF.BigFileTypeSoulReaverPlayStation bftSoulReaverPlayStation = new BigFileTypeSoulReaverPlayStation();
            result.BigFileTypeHash.Add(bftSoulReaverPlayStation.Name, bftSoulReaverPlayStation);
            nameList.Add(bftSoulReaverPlayStation.Name);

            BF.BigFileTypeSoulReaverPlayStationPALPrereleaseJuly1999 bftSoulReaverPlayStationPALPrereleaseJuly1999 = new BigFileTypeSoulReaverPlayStationPALPrereleaseJuly1999();
            result.BigFileTypeHash.Add(bftSoulReaverPlayStationPALPrereleaseJuly1999.Name, bftSoulReaverPlayStationPALPrereleaseJuly1999);
            nameList.Add(bftSoulReaverPlayStationPALPrereleaseJuly1999.Name);

            BF.BigFileTypeSoulReaverPlayStationPALRetail bftSoulReaverPlayStationPALRetail = new BigFileTypeSoulReaverPlayStationPALRetail();
            result.BigFileTypeHash.Add(bftSoulReaverPlayStationPALRetail.Name, bftSoulReaverPlayStationPALRetail);
            nameList.Add(bftSoulReaverPlayStationPALRetail.Name);

            BF.BigFileTypeSoulReaverPC bftSoulReaverPC = new BigFileTypeSoulReaverPC();
            result.BigFileTypeHash.Add(bftSoulReaverPC.Name, bftSoulReaverPC);
            nameList.Add(bftSoulReaverPC.Name);

            BF.BigFileTypeSoulReaverDreamcast bftSoulReaverDreamcast = new BigFileTypeSoulReaverDreamcast();
            result.BigFileTypeHash.Add(bftSoulReaverDreamcast.Name, bftSoulReaverDreamcast);
            nameList.Add(bftSoulReaverDreamcast.Name);

            BF.BigFileTypeSoulReaver2AirForgeDemo bftSoulReaver2AirForgeDemo = new BigFileTypeSoulReaver2AirForgeDemo();
            result.BigFileTypeHash.Add(bftSoulReaver2AirForgeDemo.Name, bftSoulReaver2AirForgeDemo);
            nameList.Add(bftSoulReaver2AirForgeDemo.Name);

            BF.BigFileTypeSoulReaver2PlayStation2 bftSoulReaver2PlayStation2 = new BigFileTypeSoulReaver2PlayStation2();
            result.BigFileTypeHash.Add(bftSoulReaver2PlayStation2.Name, bftSoulReaver2PlayStation2);
            nameList.Add(bftSoulReaver2PlayStation2.Name);

            BF.BigFileTypeSoulReaver2PC bftSoulReaver2PC = new BigFileTypeSoulReaver2PC();
            result.BigFileTypeHash.Add(bftSoulReaver2PC.Name, bftSoulReaver2PC);
            nameList.Add(bftSoulReaver2PC.Name);

            BF.BigFileTypeTombRaiderLegendPlayStation2Demo bftTombRaiderLegendPlayStation2Demo = new BigFileTypeTombRaiderLegendPlayStation2Demo();
            result.BigFileTypeHash.Add(bftTombRaiderLegendPlayStation2Demo.Name, bftTombRaiderLegendPlayStation2Demo);
            nameList.Add(bftTombRaiderLegendPlayStation2Demo.Name);

            BF.BigFileTypeTombRaiderLegendPlayStation2 bftTombRaiderLegendPlayStation2 = new BigFileTypeTombRaiderLegendPlayStation2();
            result.BigFileTypeHash.Add(bftTombRaiderLegendPlayStation2.Name, bftTombRaiderLegendPlayStation2);
            nameList.Add(bftTombRaiderLegendPlayStation2.Name);

            /* BF.BigFileTypeWhiplashBigFile bftWhiplashBigFile = new BigFileTypeWhiplashBigFile();
            result.BigFileTypeHash.Add(bftWhiplashBigFile.Name, bftWhiplashBigFile);
            nameList.Add(bftWhiplashBigFile.Name); */

            BF.BigFileTypeWaltDisneyMagicalRacingTour bftDisneyRacing = new BigFileTypeWaltDisneyMagicalRacingTour();
            result.BigFileTypeHash.Add(bftDisneyRacing.Name, bftDisneyRacing);
            nameList.Add(bftDisneyRacing.Name);

            result.BigFileTypeNames = (string[])nameList.ToArray("".GetType());

            return result;
        }

    }
}
