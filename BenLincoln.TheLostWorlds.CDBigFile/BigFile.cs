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
using System.Collections.Generic;
using System.Text;
using System.IO;
using BF = BenLincoln.TheLostWorlds.CDBigFile;
using System.Threading;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFile
    {
        protected string mName;
        protected string mPath;
        protected long mFileSize;
        protected BF.Index mMasterIndex;
        protected BF.Directory mMasterDirectory;
        //protected BF.Directory mMasterRawDirectory;
        protected BF.FlatFileHashLookupTable mHashLookupTable;
        protected BF.Fingerprint mFingerprint;
        protected BF.Fingerprint[] mFingerprints;
        protected BF.BigFileType[] mBigFileTypes;
        protected BF.BigFileType mType;
        protected BF.FileType[] mFileTypes;

        public int mLoadedPercent = 0;
        public string LoadState = "";

        public enum DirectoryModes
        {
            Normal,
            Raw
        }

        protected DirectoryModes mDirectoryMode;

        public DirectoryModes DirectoryMode
        {
            get
            {
                return mDirectoryMode;
            }
            set
            {
                mDirectoryMode = value;
            }
        }

        //public int LoadedPercent
        //{
        //    get
        //    {
        //        return mLoadedPercent;
        //    }
        //}

        //object definitions
        //protected BF.FileType ftUnknown;

        //protected BF.HashLookupTable hltSR1;
        //protected BF.HashLookupTable hltSR2;
        //protected BF.HashLookupTable hltDefiance;

        protected BF.FlatFileHashLookupTable hltBO1;
        protected BF.FlatFileHashLookupTable hltSR1;
        protected BF.FlatFileHashLookupTable hltSR2;
        protected BF.FlatFileHashLookupTable hltDefiance;

        //various option flags
        protected bool mDoHashLookups;

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

        public string Path
        {
            get
            {
                return mPath;
            }
            set
            {
                mPath = value;
            }
        }

        public long FileSize
        {
            get
            {
                return mFileSize;
            }
            set
            {
                mFileSize = value;
            }
        }

        public BF.Index MasterIndex
        {
            get
            {
                return mMasterIndex;
            }
            set
            {
                mMasterIndex = value;
            }
        }

        public BF.Directory MasterDirectory
        {
            get
            {
                return mMasterDirectory;
            }
            set
            {
                mMasterDirectory = value;
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

        public BF.Fingerprint Fingerprint
        {
            get
            {
                return mFingerprint;
            }
            set
            {
                mFingerprint = value;
            }
        }

        public BF.BigFileType[] BigFileTypes
        {
            get
            {
                return mBigFileTypes;
            }
        }

        public BF.FileType[] FileTypes
        {
            get
            {
                return mFileTypes;
            }
        }

        public BF.BigFileType Type
        {
            get
            {
                return mType;
            }
        }

        public bool DoHashLookups
        {
            get
            {
                return mDoHashLookups;
            }
            set
            {
                mDoHashLookups = value;
            }
        }


    #endregion

        public BigFile(string filePath)
        {
            SetParameters();
            Path = filePath;
            int lastBackSlash = filePath.LastIndexOf('\\') + 1;
            Name = filePath.Substring(lastBackSlash, filePath.Length - lastBackSlash);
            FileSize = 0;
            try
            {
                FileInfo fInfo = new FileInfo(Path);
                FileSize = fInfo.Length;
            }
            catch (Exception ex)
            {
                throw new BigFileOpenException
                    ("Unable to open the file " + Path + " to determine its size.\r\n" + 
                    "The specific error message is: \r\n" + 
                    ex.Message
                    );
            }

            InitializeTypes();
            bool foundMatch = false;
            Fingerprint = new BF.Fingerprint();
            for (int i = 0; i < mFingerprints.GetUpperBound(0); i++)
            {
                if ((!foundMatch) && (mFingerprints[i].CheckBigFileForMatch(this)))
                {
                    Fingerprint = mFingerprints[i];
                    foundMatch = true;
                }
            }
        }

        public void SetParameters()
        {
            mDoHashLookups = true;
        }

        public void LoadBigFile()
        {
            LoadState = "Reading indices";
            mMasterIndex = BF.Index.CreateMasterIndex(this);
            Thread indexThread = new Thread(new ThreadStart(mMasterIndex.ReadIndex));
            //indexThread.Priority = ThreadPriority.AboveNormal;
            indexThread.Start();
            do
            {
                LoadState = "Reading index contents (" + (int)mMasterIndex.mLoadedPercent + "%)";
                mLoadedPercent = (int)(mMasterIndex.mLoadedPercent * 0.98);
                Thread.Sleep(5);
            }
            while (indexThread.IsAlive);
            //mMasterIndex.ReadIndex();
            LoadState = "Sorting directories";
            BuildMasterDirectory();
            mLoadedPercent = 100;
            LoadState = "Done";
        }

        public void BuildMasterDirectory()
        {
            if (mDirectoryMode == DirectoryModes.Normal)
            {
                mMasterDirectory = mMasterIndex.BuildDirectoryFromFileData();
                mMasterDirectory.SortAll();
            }
            else
            {
                mMasterDirectory = mMasterIndex.BuildDirectoryFromRawIndex();
                mMasterDirectory.SortAll();
            }
        }

        public void SetType(BF.BigFileType newType)
        {
            mType = newType;
            mFileTypes = mType.FileTypes;
            mHashLookupTable = mType.HashLookupTable;
            if (mHashLookupTable != null)
            {
                try
                {
                    mHashLookupTable.LoadHashTable();
                }
                catch (HashTableLoadException htlEx)
                {
                    mDoHashLookups = false;
                    //log to file if necessary
                }
            }
        }

        #region Type Initialization

        public void InitializeTypes()
        {
            if (mDoHashLookups)
            {
                InitializeHashLookupTables();
            }
            InitializeSpecificTypes();
        }

        public void InitializeHashLookupTables()
        {
            //get the folder that the DLL for this library is in
            string dllPath = this.GetType().Assembly.Location;
            int lastSlash = dllPath.LastIndexOf('\\') + 1;
            dllPath = dllPath.Substring(0, lastSlash);

            //hltSR1 = new HashLookupTable("SR1", dllPath + "ohrainBOWS.mdb", "SR1");
            //hltSR2 = new HashLookupTable("SR2", dllPath + "ohrainBOWS.mdb", "SR2");
            //hltDefiance = new HashLookupTable("Defiance", dllPath + "ohrainBOWS.mdb", "Defiance");

            hltBO1 = new FlatFileHashLookupTable("BO1", dllPath + "Hashes-BO1.txt");
            hltSR1 = new FlatFileHashLookupTable("SR1", dllPath + "Hashes-SR1.txt");
            hltSR2 = new FlatFileHashLookupTable("SR2", dllPath + "Hashes-SR2.txt");
            hltDefiance = new FlatFileHashLookupTable("Defiance", dllPath + "Hashes-Defiance.txt");
        }

        protected void InitializeFileTypes()
        {
            BF.FileType ftUnknown = new BF.FileType();

            mFileTypes = new FileType[] 
            {
                ftUnknown
            };
        }

        public void InitializeSpecificTypes()
        {
            //file types
            BF.FileType ftUnknown = new FileType();

            //BigFile types

            BF.BigFileType bft100;
            BF.BigFileType bft110;
            BF.BigFileType bft200;
            BF.BigFileType bft300;
            //BF.BigFileType bft400;
            BF.BigFileType bft500;
            BF.BigFileType bft600;
            BF.BigFileType bft650;
            BF.BigFileType bft700;
            BF.BigFileType bft800;
            BF.BigFileType bft900;
            BF.BigFileType bft1000;
            BF.BigFileType bft1100;
            BF.BigFileType bft1200;
            BF.BigFileType bft1300;
            BF.BigFileType bft1400;
            BF.BigFileType bft1500;

            bft100 = new BF.BigFileType();
            bft100.Name = "Type 100";
            bft100.Description = "Gex 1 (Playstation)";
            bft100.MasterIndexType = BF.Index.INDEX_TYPE_SI100;
            bft100.FileTypes = new FileType[]
            {
                ftUnknown
            };

            bft110 = new BF.BigFileType();
            bft110.Name = "Type 110";
            bft110.Description = "Gex 1 (Sega Saturn)";
            bft110.MasterIndexType = BF.Index.INDEX_TYPE_SI110;
            bft110.FileTypes = new FileType[]
            {
                ftUnknown
            };

            bft200 = new BF.BigFileType();
            bft200.Name = "Type 200";
            bft200.Description = "Blood Omen (Playstation/PC)";
            bft200.MasterIndexType = BF.Index.INDEX_TYPE_SI200;
            bft200.HashLookupTable = hltBO1;
            bft200.FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_4),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_8),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_16),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_24),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_VAB_Headerless),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_VAG_Headerless),
                ftUnknown
            };

            bft300 = new BF.BigFileType();
            bft300.Name = "Type 300";
            bft300.Description = "Akuji/Gex 2/Gex 3 (Playstation)";
            bft300.MasterIndexType = BF.Index.INDEX_TYPE_SI300;
            bft300.FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_SND_Akuji),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_4),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_8),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_16),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_TIM_24),
                ftUnknown
            };

            //bft400 = new BF.BigFileType();
            //bft400.Name = "Type 400";
            //bft400.Description = "Gex 3 (Playstation)";
            //bft400.MasterIndexType = BF.Index.INDEX_TYPE_SI300;
            //bft400.FileTypes = new FileType[]
            //{
            //    ftUnknown
            //};

            bft500 = new BF.BigFileType();
            bft500.Name = "Type 500";
            bft500.Description = "Soul Reaver Lighthouse Demo (Playstation)";
            bft500.MasterIndexType = BF.Index.INDEX_TYPE_SI300;
            bft500.FileTypes = new FileType[]
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
                ftUnknown
            };

            bft600 = new BF.BigFileType();
            bft600.Name = "Type 600";
            bft600.Description = "Soul Reaver (Playstation - All Versions Except PAL Retail)";
            bft600.MasterIndexType = BF.Index.INDEX_TYPE_MI100;
            bft600.HashLookupTable = hltSR1;
            bft600.FileTypes = new FileType[]
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
                ftUnknown
            };

            bft650 = new BF.BigFileType();
            bft650.Name = "Type 650";
            bft650.Description = "Soul Reaver (Playstation - PAL Retail)";
            bft650.MasterIndexType = BF.Index.INDEX_TYPE_MI200;
            bft650.HashLookupTable = hltSR1;
            bft650.FileTypes = new FileType[]
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
                ftUnknown
            };

            bft700 = new BF.BigFileType();
            bft700.Name = "Type 700";
            bft700.Description = "Soul Reaver (PC)";
            bft700.MasterIndexType = BF.Index.INDEX_TYPE_SI400;
            bft700.HashLookupTable = hltSR1;
            bft700.FileTypes = new FileType[]
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
                ftUnknown
            };

            bft800 = new BF.BigFileType();
            bft800.Name = "Type 800";
            bft800.Description = "Soul Reaver (Dreamcast)";
            bft800.MasterIndexType = BF.Index.INDEX_TYPE_SI400;
            bft800.FileTypes = new FileType[]
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
                ftUnknown
            };

            bft900 = new BF.BigFileType();
            bft900.Name = "Type 900";
            bft900.Description = "Soul Reaver 2 Air Forge Demo (Playstation 2)";
            bft900.MasterIndexType = BF.Index.INDEX_TYPE_SI450;
            bft900.FileTypes = new FileType[]
            {
               BF.FileType.FromType(BF.FileType.FILE_TYPE_RAW_SR2_PS2),
               ftUnknown
            };

            bft1000 = new BF.BigFileType();
            bft1000.Name = "Type 1000";
            bft1000.Description = "Soul Reaver 2 (Playstation 2)";
            bft1000.MasterIndexType = BF.Index.INDEX_TYPE_SI500;
            bft1000.HashLookupTable = hltSR2;
            bft1000.FileTypes = new FileType[]
            {
                //BF.FileType.FromType(BF.FileType.FILE_TYPE_STR_SR2_PS2),
                //BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAW_SR2_PS2),
                ftUnknown
            };

            bft1100 = new BF.BigFileType();
            bft1100.Name = "Type 1100";
            bft1100.Description = "Soul Reaver 2 (PC)";
            bft1100.MasterIndexType = BF.Index.INDEX_TYPE_SI550;
            bft1100.HashLookupTable = hltSR2;
            bft1100.FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_STR_SR2_PC),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                ftUnknown
            };

            bft1200 = new BF.BigFileType();
            bft1200.Name = "Type 1200";
            bft1200.Description = "Legacy of Kain: Defiance (Playstation 2)";
            bft1200.MasterIndexType = BF.Index.INDEX_TYPE_SI550;
            bft1200.HashLookupTable = hltDefiance;
            bft1200.FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_MUL_Defiance),
                ftUnknown
            };

            bft1300 = new BF.BigFileType();
            bft1300.Name = "Type 1300";
            bft1300.Description = "Legacy of Kain: Defiance (PC)";
            bft1300.MasterIndexType = BF.Index.INDEX_TYPE_SI550;
            bft1300.HashLookupTable = hltDefiance;
            bft1300.FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_MUL_Defiance),
                ftUnknown
            };

            bft1400 = new BF.BigFileType();
            bft1400.Name = "Type 1400";
            bft1400.Description = "Tomb Raider: Legend Demo (Playstation 2)";
            bft1400.MasterIndexType = BF.Index.INDEX_TYPE_SI600;
            bft1400.FileTypes = new FileType[]
            {
               BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
               BF.FileType.FromType(BF.FileType.FILE_TYPE_MUL_Defiance),
               ftUnknown
            };

            bft1500 = new BF.BigFileType();
            bft1500.Name = "Type 1500";
            bft1500.Description = "Tomb Raider: Legend (Playstation 2/PSP)";
            bft1500.MasterIndexType = BF.Index.INDEX_TYPE_SI650;
            bft1500.FileTypes = new FileType[]
            {
                BF.FileType.FromType(BF.FileType.FILE_TYPE_RAWImage),
                BF.FileType.FromType(BF.FileType.FILE_TYPE_MUL_Defiance),
                ftUnknown
            };

            mBigFileTypes = new BigFileType[]
            {
                bft100, bft110, bft200, bft300, bft500, bft600, bft650, bft700, bft800, bft900, bft1000, 
                bft1100, bft1200, bft1300, bft1400, bft1500
            }; //bft400


            //BigFile fingerprints

            BF.Fingerprint fpUnknown = new BF.Fingerprint();

            BF.Fingerprint fpGexPlaystationNTSC = new BF.Fingerprint();
            fpGexPlaystationNTSC.Name = "Gex (Playstation/NTSC)";
            fpGexPlaystationNTSC.FileSize = 117831680;
            fpGexPlaystationNTSC.Type = bft100;

            BF.Fingerprint fpGexSaturnNTSC = new BF.Fingerprint();
            fpGexSaturnNTSC.Name = "Gex (Sega Saturn/NTSC)";
            fpGexSaturnNTSC.FileSize = 158287872;
            fpGexSaturnNTSC.Type = bft110;

            BF.Fingerprint fpBloodOmenPlaystationNTSC = new BF.Fingerprint();
            fpBloodOmenPlaystationNTSC.Name = "Blood Omen: Legacy of Kain (Playstation/NTSC)";
            fpBloodOmenPlaystationNTSC.FileSize = 113647616;
            fpBloodOmenPlaystationNTSC.Type = bft200;

            BF.Fingerprint fpBloodOmenPlaystationPAL = new BF.Fingerprint();
            fpBloodOmenPlaystationPAL.Name = "Blood Omen: Legacy of Kain (Playstation/PAL)";
            fpBloodOmenPlaystationPAL.FileSize = 113879040;
            fpBloodOmenPlaystationPAL.Type = bft200;

            BF.Fingerprint fpBloodOmenKTV = new BF.Fingerprint();
            fpBloodOmenKTV.Name = "Kain the Vampire (Playstation/Nippon)";
            fpBloodOmenKTV.FileSize = 111960064;
            fpBloodOmenKTV.Type = bft200;

            BF.Fingerprint fpBloodOmenPC = new BF.Fingerprint();
            fpBloodOmenPC.Name = "Blood Omen: Legacy of Kain (PC)";
            fpBloodOmenPC.FileSize = 163998755;
            fpBloodOmenPC.Type = bft200;

            BF.Fingerprint fpGex2PlaystationNTSC = new BF.Fingerprint();
            fpGex2PlaystationNTSC.Name = "Gex 2: Enter the Gecko (Playstation/NTSC)";
            fpGex2PlaystationNTSC.FileSize = 70731776;
            fpGex2PlaystationNTSC.Type = bft300;

            BF.Fingerprint fpAkujiDemoNTSC = new BF.Fingerprint();
            fpAkujiDemoNTSC.Name = "Akuji Demo (Playstation/NTSC)";
            fpAkujiDemoNTSC.FileSize = 498441;
            fpAkujiDemoNTSC.Type = bft300;

            BF.Fingerprint fpAkujiNTSC = new BF.Fingerprint();
            fpAkujiNTSC.Name = "Akuji (Playstation/NTSC)";
            fpAkujiNTSC.FileSize = 42952704;
            fpAkujiNTSC.Type = bft300;

            BF.Fingerprint fpGex3NTSC = new BF.Fingerprint();
            fpGex3NTSC.Name = "Gex 3 (Playstation/NTSC)";
            fpGex3NTSC.FileSize = 61751296;
            fpGex3NTSC.Type = bft300;

            BF.Fingerprint fpSRPSLighthouseNTSC = new BF.Fingerprint();
            fpSRPSLighthouseNTSC.Name = "Soul Reaver Lighthouse Demo (Playstation/NTSC)";
            fpSRPSLighthouseNTSC.FileSize = 2168832;
            fpSRPSLighthouseNTSC.Type = bft500;

            BF.Fingerprint fpSRPSLighthousePAL = new BF.Fingerprint();
            fpSRPSLighthousePAL.Name = "Soul Reaver Lighthouse Demo (Playstation/PAL)";
            fpSRPSLighthousePAL.FileSize = 2174976;
            fpSRPSLighthousePAL.Type = bft500;

            BF.Fingerprint fpSRPSBetaNTSC = new BF.Fingerprint();
            fpSRPSBetaNTSC.Name = "Soul Reaver [Beta] (Playstation/NTSC)";
            fpSRPSBetaNTSC.FileSize = 240783360;
            fpSRPSBetaNTSC.Type = bft600;
            //fpSRPSBetaNTSC.HasRainbowTable = true;
            //fpSRPSBetaNTSC.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPSFireDemoNTSC = new BF.Fingerprint();
            fpSRPSFireDemoNTSC.Name = "Soul Reaver Fire Glyph Demo (Playstation/NTSC)";
            fpSRPSFireDemoNTSC.FileSize = 15108096;
            fpSRPSFireDemoNTSC.Type = bft600;
            //fpSRPSFireDemoNTSC.HasRainbowTable = true;
            //fpSRPSFireDemoNTSC.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPSFireDemoPAL = new BF.Fingerprint();
            fpSRPSFireDemoPAL.Name = "Soul Reaver Fire Glyph Demo (Playstation/PAL)";
            fpSRPSFireDemoPAL.FileSize = 14977024;
            fpSRPSFireDemoPAL.Type = bft600;
            //fpSRPSFireDemoPAL.HasRainbowTable = true;
            //fpSRPSFireDemoPAL.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPSNTSC = new BF.Fingerprint();
            fpSRPSNTSC.Name = "Soul Reaver (Playstation/NTSC)";
            fpSRPSNTSC.FileSize = 233248768;
            fpSRPSNTSC.Type = bft600;
            //fpSRPSNTSC.HasRainbowTable = true;
            //fpSRPSNTSC.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPSPAL = new BF.Fingerprint();
            fpSRPSPAL.Name = "Soul Reaver (Playstation/PAL)";
            fpSRPSPAL.FileSize = 233209856;
            fpSRPSPAL.Type = bft650;
            //fpSRPSPAL.HasRainbowTable = true;
            //fpSRPSPAL.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPCFireDemo = new BF.Fingerprint();
            fpSRPCFireDemo.Name = "Soul Reaver Fire Glyph Demo (PC)";
            fpSRPCFireDemo.FileSize = 7777;
            fpSRPCFireDemo.Type = bft700;
            //fpSRPCFireDemo.HasRainbowTable = true;
            //fpSRPCFireDemo.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPC = new BF.Fingerprint();
            fpSRPC.Name = "Soul Reaver (PC)";
            fpSRPC.FileSize = 85923840;
            fpSRPC.Type = bft700;
            //fpSRPC.HasRainbowTable = true;
            //fpSRPC.RainbowTableName = "SR1";

            BF.Fingerprint fpSRPCQFM = new BF.Fingerprint();
            fpSRPCQFM.Name = "Soul Reaver: Quest for Melchiah (PC)";
            fpSRPCQFM.FileSize = 39225344;
            fpSRPCQFM.Type = bft700;
            //fpSRPCQFM.HasRainbowTable = true;
            //fpSRPCQFM.RainbowTableName = "SR1";

            BF.Fingerprint fpSRDCNTSC = new BF.Fingerprint();
            fpSRDCNTSC.Name = "Soul Reaver (Dreamcast)";
            fpSRDCNTSC.FileSize = 63281152;
            fpSRDCNTSC.Type = bft800;
            //fpSRDCNTSC.HasRainbowTable = true;
            //fpSRDCNTSC.RainbowTableName = "SR1";

            BF.Fingerprint fpSRDCDemo = new BF.Fingerprint();
            fpSRDCDemo.Name = "Soul Reaver Fire Glyph Demo (Dreamcast)";
            fpSRDCDemo.FileSize = 25595904;
            fpSRDCDemo.Type = bft800;
            //fpSRDCDemo.HasRainbowTable = true;
            //fpSRDCDemo.RainbowTableName = "SR1";

            BF.Fingerprint fpSR2AirforgePS2NTSC = new BF.Fingerprint();
            fpSR2AirforgePS2NTSC.Name = "Soul Reaver 2 Air Forge Demo (Playstation 2/NTSC)";
            fpSR2AirforgePS2NTSC.FileSize = 26941440;
            fpSR2AirforgePS2NTSC.Type = bft900;

            BF.Fingerprint fpSR2AirforgePS2PAL = new BF.Fingerprint();
            fpSR2AirforgePS2PAL.Name = "Soul Reaver 2 Air Forge Demo (Playstation 2/PAL)";
            fpSR2AirforgePS2PAL.FileSize = 29591552;
            fpSR2AirforgePS2PAL.Type = bft900;

            BF.Fingerprint fpSR2PS2NTSC = new BF.Fingerprint();
            fpSR2PS2NTSC.Name = "Soul Reaver 2 (Playstation 2/NTSC)";
            fpSR2PS2NTSC.FileSize = 1411893248;
            fpSR2PS2NTSC.Type = bft1000;
            //fpSR2PS2NTSC.HasRainbowTable = true;
            //fpSR2PS2NTSC.RainbowTableName = "SR2";

            BF.Fingerprint fpSR2PS2PAL = new BF.Fingerprint();
            fpSR2PS2PAL.Name = "Soul Reaver 2 (Playstation 2/PAL)";
            fpSR2PS2PAL.FileSize = 1995837440;
            fpSR2PS2PAL.Type = bft1000;
            //fpSR2PS2PAL.HasRainbowTable = true;
            //fpSR2PS2PAL.RainbowTableName = "SR2";

            BF.Fingerprint fpSR2PS2Japan = new BF.Fingerprint();
            fpSR2PS2Japan.Name = "Soul Reaver 2 (Playstation 2/Nippon)";
            fpSR2PS2Japan.FileSize = 1406451712;
            fpSR2PS2Japan.Type = bft1000;

            BF.Fingerprint fpSR2PC = new BF.Fingerprint();
            fpSR2PC.Name = "Soul Reaver 2 (PC)";
            fpSR2PC.FileSize = 827439104;
            fpSR2PC.Type = bft1100;
            //fpSR2PC.HasRainbowTable = true;
            //fpSR2PC.RainbowTableName = "SR2";

            BF.Fingerprint fpSR2PCDeutsch = new BF.Fingerprint();
            fpSR2PCDeutsch.Name = "Soul Reaver 2 (PC/Deutschland)";
            fpSR2PCDeutsch.FileSize = 816603136;
            fpSR2PCDeutsch.Type = bft1100;
            //fpSR2PCDeutsch.HasRainbowTable = true;
            //fpSR2PCDeutsch.RainbowTableName = "SR2";

            BF.Fingerprint fpDefiancePS2NTSC = new BF.Fingerprint();
            fpDefiancePS2NTSC.Name = "Defiance (Playstation 2/NTSC)";
            fpDefiancePS2NTSC.FileSize = 1782990848;
            fpDefiancePS2NTSC.Type = bft1200;
            //fpDefiancePS2NTSC.HasRainbowTable = true;
            //fpDefiancePS2NTSC.RainbowTableName = "DefiancePS2";

            BF.Fingerprint fpDefiancePS2PAL = new BF.Fingerprint();
            fpDefiancePS2PAL.Name = "Defiance (Playstation 2/PAL)";
            fpDefiancePS2PAL.FileSize = 3473287168;
            fpDefiancePS2PAL.Type = bft1200;

            BF.Fingerprint fpDefiancePC = new BF.Fingerprint();
            fpDefiancePC.Name = "Defiance (PC)";
            fpDefiancePC.FileSize = 1844969472;
            fpDefiancePC.Type = bft1300;
            //fpDefiancePC.HasRainbowTable = true;
            //fpDefiancePC.RainbowTableName = "Defiance";

            BF.Fingerprint fpDefiancePCDeutsch = new BF.Fingerprint();
            fpDefiancePCDeutsch.Name = "Defiance (PC/Deutschland)";
            fpDefiancePCDeutsch.FileSize = 1844443136;
            fpDefiancePCDeutsch.Type = bft1300;

            BF.Fingerprint fpTRLPS2DemoPAL = new BF.Fingerprint();
            fpTRLPS2DemoPAL.Name = "Tomb Raider: Legend Demo (Playstation 2/PAL)";
            fpTRLPS2DemoPAL.FileSize = 361828352;
            fpTRLPS2DemoPAL.Type = bft1400;

            BF.Fingerprint fpTRLPS2NTSC = new BF.Fingerprint();
            fpTRLPS2NTSC.Name = "Tomb Raider: Legend (Playstation 2/NTSC)";
            fpTRLPS2NTSC.FileSize = 2697117696;
            fpTRLPS2NTSC.Type = bft1500;

            BF.Fingerprint fpTRLPS2PAL = new BF.Fingerprint();
            fpTRLPS2PAL.Name = "Tomb Raider: Legend (Playstation 2/PAL)";
            fpTRLPS2PAL.FileSize = 3247683584;
            fpTRLPS2PAL.Type = bft1500;

            BF.Fingerprint fpTRLPSPPAL = new BF.Fingerprint();
            fpTRLPSPPAL.Name = "Tomb Raider: Legend (PSP/PAL)";
            fpTRLPSPPAL.FileSize = 1451984896;
            fpTRLPSPPAL.Type = bft1500;

            mFingerprints = new BF.Fingerprint[]
            {
                fpGexPlaystationNTSC, fpGexSaturnNTSC, fpGex2PlaystationNTSC, 
                fpBloodOmenPlaystationNTSC, fpBloodOmenPlaystationPAL, fpBloodOmenKTV, fpBloodOmenPC, 
                fpAkujiDemoNTSC, 
				fpAkujiNTSC, fpGex3NTSC, fpSRPSLighthouseNTSC, fpSRPSLighthousePAL,
				fpSRPSBetaNTSC, fpSRPSFireDemoNTSC, fpSRPSFireDemoPAL, fpSRPSNTSC, fpSRPSPAL, fpSRPCFireDemo, fpSRPCQFM,
				fpSRPC, fpSRDCNTSC, fpSRDCDemo, fpSR2AirforgePS2NTSC, fpSR2AirforgePS2PAL, fpSR2PS2NTSC, 
				fpSR2PS2PAL, fpSR2PS2Japan, fpSR2PC, fpSR2PCDeutsch,
				fpDefiancePS2NTSC, fpDefiancePS2PAL, fpDefiancePC, fpDefiancePCDeutsch,
				fpTRLPS2DemoPAL, fpTRLPS2NTSC, fpTRLPS2PAL, fpTRLPSPPAL, 
                fpUnknown
            };
        }

#endregion

        public virtual string GetInfo()
        {
            string fileInfo;

            try
            {
                fileInfo =
                    "BigFile Information\r\n---\r\n" +
                    "Name: " + Name + "\r\n" +
                    "Path: " + Path + "\r\n" +
                    "Size: " + FileSize.ToString() + " bytes\r\n" +
                    "Fingerprint: " + Fingerprint.Name + "\r\n" +
                    "Reading As: " + mType.Description + "\r\n";

                if (mHashLookupTable != null)
                {
                    fileInfo += "Hash Lookup Table: " + HashLookupTable.Name + "\r\n";
                }

                fileInfo += "Files Contained: " + mMasterIndex.FileCount;
            }
            catch (Exception ex)
            {
                fileInfo = "BigFile is not open yet";
            }

            return fileInfo;
        }

        public virtual string GetTypeDescription()
        {
            return "Generic Bigfile";
        }

    }
}
