// BenLincoln.TheLostWorlds.CDBigFile
// Copyright 2006-2018 Ben Lincoln
// https://www.thelostworlds.net/
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
using System.Text;
using System.IO;
using BF = BenLincoln.TheLostWorlds.CDBigFile;
using BLD = BenLincoln.Data;
using System.Threading;
using System.Security.Cryptography;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class BigFile
    {
        protected string mName;
        protected string mPath;
        protected long mFileSize;
        protected byte[] mSHA256Hash;
        protected BF.Index mMasterIndex;
        protected BF.Directory mMasterDirectory;
        //protected BF.Directory mMasterRawDirectory;
        protected BF.FlatFileHashLookupTable mHashLookupTable;
        protected BF.Fingerprint mFingerprint;
        protected BF.Fingerprint[] mFingerprints;
        protected BigFileTypeCollection mBigFileTypes;
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

        //protected BF.FlatFileHashLookupTable hltBO1;
        //protected BF.FlatFileHashLookupTable hltSR1;
        //protected BF.FlatFileHashLookupTable hltSR2AirForge;
        //protected BF.FlatFileHashLookupTable hltSR2;
        //protected BF.FlatFileHashLookupTable hltDefiance;
        //protected BF.FlatFileHashLookupTable hltSR1Proto1;
        //protected BF.FlatFileHashLookupTable hltAkuji;
        //protected BF.BloodOmen2HashLookupTable hltBO2;
        

        //various option flags
        protected bool mDoHashLookups;

        // attempt to parse names out of known filetypes if there are no entries in the hash-lookup table
        protected bool mParseNamesFromKnownFileTypes;

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

        public byte[] SHA256Hash
        {
            get
            {
                return mSHA256Hash;
            }
            set
            {
                mSHA256Hash = value;
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

        public BigFileTypeCollection BigFileTypes
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

        public bool ParseNamesFromKnownFileTypes
        {
            get
            {
                return mParseNamesFromKnownFileTypes;
            }
            set
            {
                mParseNamesFromKnownFileTypes = value;
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
            try
            {
                FileStream hStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                hStream.Position = 0;
                SHA256 s256 = SHA256Managed.Create();
                mSHA256Hash = s256.ComputeHash(hStream);
                hStream.Close();
            }
            catch (Exception ex)
            {
                throw new BigFileOpenException
                    ("Unable to open the file " + Path + " to determine its hash." + Environment.NewLine +
                    "The specific error message is:" + Environment.NewLine + 
                    ex.Message
                    );
            }

            InitializeTypes();
            bool foundMatch = false;
            Fingerprint = new BF.Fingerprint();

            for (int i = 0; i < mFingerprints.GetUpperBound(0); i++)
            {
                if ((!foundMatch) && (mFingerprints[i].CheckBigFileForMatch(this, true)))
                {
                    Fingerprint = mFingerprints[i];
                    foundMatch = true;
                }
            }
            // check for matches twice - once based just on file size, and then using the SHA256 hash if there are 
            // multiple matches for the file size
            // this is to avoid lengthy hash calculations for large files if there is only one (or no) match 
            // based on file size alone
            //ArrayList matchList = new ArrayList();
            //for (int i = 0; i < mFingerprints.GetUpperBound(0); i++)
            //{
            //    if (mFingerprints[i].CheckBigFileForMatch(this, false))
            //    {
            //        matchList.Add(i);
            //    }
            //}
            //if (matchList.Count > 0)
            //{
            //    if (matchList.Count == 1)
            //    {
            //        Fingerprint = mFingerprints[(int)matchList[0]];
            //    }
            //    else
            //    {
            //        foreach (int i in matchList)
            //        {
            //            if ((!foundMatch) && (mFingerprints[i].CheckBigFileForMatch(this, false)))
            //            {
            //                Fingerprint = mFingerprints[i];
            //                foundMatch = true;
            //            }
            //        }
            //    }
            //}
        }

        public void SetParameters()
        {
            mDoHashLookups = true;
            mParseNamesFromKnownFileTypes = true;
        }

        public void LoadBigFile()
        {
            //Type.LoadHashLookupTable(Path);
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
            mType.LoadHashLookupTable(Path);
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
            //string dllPath = this.GetType().Assembly.Location;
            //int lastSlash = dllPath.LastIndexOf('\\') + 1;
            //dllPath = dllPath.Substring(0, lastSlash);

            //hltSR1 = new HashLookupTable("SR1", dllPath + "ohrainBOWS.mdb", "SR1");
            //hltSR2 = new HashLookupTable("SR2", dllPath + "ohrainBOWS.mdb", "SR2");
            //hltDefiance = new HashLookupTable("Defiance", dllPath + "ohrainBOWS.mdb", "Defiance");

            //hltBO1 = new FlatFileHashLookupTable("BO1", dllPath + "Hashes-BO1.txt");
            //hltSR1Proto1 = new FlatFileHashLookupTable("SR1Proto1", dllPath + "Hashes-SR1_Proto1.txt");
            //hltSR1 = new FlatFileHashLookupTable("SR1", dllPath + "Hashes-SR1.txt");
            //hltSR2AirForge = new FlatFileHashLookupTable("SR2AirForge", dllPath + "Hashes-SR2_Air_Forge.txt");
            //hltSR2 = new FlatFileHashLookupTable("SR2", dllPath + "Hashes-SR2.txt");
            //hltDefiance = new FlatFileHashLookupTable("Defiance", dllPath + "Hashes-Defiance.txt");
            //hltBO2 = new BloodOmen2HashLookupTable("BO2", Path);
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

            //mBigFileTypes = new BigFileType[]
            //{
            //    bftGex1PlayStation, bftGex1Saturn, bftBloodOmen, bftGex2, bftGex3, bftAkuji, bftSoulReaverProto1Demo, bftSoulReaverPlayStation,
            //    bftSoulReaverPlayStationPALPrereleaseJuly1999, bftSoulReaverPlayStationPALRetail, bftSoulReaverPC, bftSoulReaverDreamcast, 
            //    bftSoulReaver2AirForgeDemo, bftSoulReaver2PlayStation2, 
            //    bftSoulReaver2PC, bftDefiancePlayStation2, bftDefiancePC, bftTombRaiderLegendPlayStation2Demo, bftTombRaiderLegendPlayStation2, bftBloodOmen2
            //}; 

            mBigFileTypes = BigFileType.GetAllTypes();


            //BigFile fingerprints
            string dllPath = this.GetType().Assembly.Location;
            dllPath = System.IO.Path.GetDirectoryName(dllPath);
            string fpPath = System.IO.Path.Combine(dllPath, "Fingerprints.txt");
            string[] fingerprintLines = new string[] { };
            int contentLineCount = 0;
            if (System.IO.File.Exists(fpPath))
            {
                try
                {
                    FileStream inStream = new FileStream(fpPath, FileMode.Open, FileAccess.Read);
                    StreamReader inReader = new StreamReader(inStream);

                    string inData = inReader.ReadToEnd().Replace("\r\n", "\n");

                    fingerprintLines = inData.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    inReader.Close();
                    inStream.Close();
                    foreach (string fpl in fingerprintLines)
                    {
                        bool isValid = true;
                        if (fpl.StartsWith("#"))
                        {
                            isValid = false;
                        }
                        if (fpl.Trim() == "")
                        {
                            isValid = false;
                        }
                        if (isValid)
                        {
                            contentLineCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // do something with this later?
                }
            }

            ArrayList fingerprintList = new ArrayList();

            int fingerprintNum = 0;
            foreach (string fpl in fingerprintLines)
            {
                bool isValid = true;
                string fpTitle = "";
                string fpPlatform = "";
                string fpFormat = "";
                string fpLanguage = "";
                string fpReleaseID = "";
                string fpReleaseType = "";
                string fpFileName = "";
                string fpBuildDate = "";
                long fpFileSize = -1;
                string fpBigFileTypeString = "";
                string fpSHA256Hash = "";
                if (fpl.StartsWith("#"))
                {
                    isValid = false;
                }
                if (fpl.Trim() == "")
                {
                    isValid = false;
                }
                if (isValid)
                {
                    string[] lineSplit = fpl.Split(new string[] { "\t" }, StringSplitOptions.None);
                    if (lineSplit.Length >= 11)
                    {
                        fpTitle = lineSplit[0];
                        fpPlatform = lineSplit[1];
                        fpFormat = lineSplit[2];
                        fpLanguage = lineSplit[3];
                        fpReleaseID = lineSplit[4];
                        fpReleaseType = lineSplit[5];
                        fpFileName = lineSplit[6];
                        fpBuildDate = lineSplit[7];
                        fpFileSize = long.Parse(lineSplit[8]);
                        fpBigFileTypeString = lineSplit[9];
                        fpSHA256Hash = lineSplit[10];
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    BF.Fingerprint newFingerprint = new BF.Fingerprint();
                    if (BigFileTypes.BigFileTypeHash.ContainsKey(fpBigFileTypeString))
                    {
                        newFingerprint.Type = (BigFileType)BigFileTypes.BigFileTypeHash[fpBigFileTypeString];
                    }
                    else
                    {
                        isValid = false;
                    }
                    if (isValid)
                    {
                        newFingerprint.Title = fpTitle;
                        newFingerprint.Platform = fpPlatform;
                        newFingerprint.Format = fpFormat;
                        newFingerprint.Language = fpLanguage;
                        newFingerprint.ReleaseID = fpReleaseID;
                        newFingerprint.ReleaseType = fpReleaseType;
                        newFingerprint.FileName = fpFileName;
                        newFingerprint.BuildDate = fpBuildDate;
                        newFingerprint.FileSize = fpFileSize;
                        if (fpSHA256Hash.Trim() != "")
                        {
                            newFingerprint.SHA256Hash = BLD.HexConverter.HexStringToBytes(fpSHA256Hash);
                        }
                        fingerprintList.Add(newFingerprint);

                        fingerprintNum++;
                    }
                }
            }

            BF.Fingerprint dummy = new BF.Fingerprint();
            mFingerprints = (BF.Fingerprint[])fingerprintList.ToArray(dummy.GetType());

            //mFingerprints = new BF.Fingerprint[]
            //{
            //    fpGexPlaystationNTSC, fpGexSaturnNTSC, fpGex2PlaystationNTSC, 
            //    fpBloodOmenPlaystationNTSC, fpBloodOmenPlaystationPAL, fpBloodOmenKTV, fpBloodOmenPC, 
            //    fpAkujiDemoNTSC, 
            //    fpAkujiNTSC, fpGex3NTSC, fpSRPSLighthouseNTSC, fpSRPSLighthousePAL,
            //    fpSRPSBetaNTSC, fpSRPSFireDemoNTSC, fpSRPSFireDemoPAL, fpSRPSNTSC, fpSRPSPAL, fpSRPCFireDemo, fpSRPCQFM,
            //    fpSRPC, fpSRDCNTSC, fpSRDCDemo, fpSR2AirforgePS2NTSC, fpSR2AirforgePS2PAL, fpSR2PS2NTSC, 
            //    fpSR2PS2PAL, fpSR2PS2Japan, fpSR2PC, fpSR2PCDeutsch,
            //    fpDefiancePS2NTSC, fpDefiancePS2PAL, fpDefiancePS2PALPR20031010, fpDefiancePC, fpDefiancePCDeutsch,
            //    fpTRLPS2DemoPAL, fpTRLPS2NTSC, fpTRLPS2PAL, fpTRLPSPPAL,
            //    fpBloodOmen2PC,
            //    fpUnknown
            //};

            int d2 = 7;
        }

        #endregion

        public virtual string GetInfo()
        {
            string fileInfo;

            try
            {
                fileInfo =
                    "BigFile Information" + Environment.NewLine + "---" + Environment.NewLine +
                    "Name: " + Name + Environment.NewLine +
                    "Path: " + Path + Environment.NewLine +
                    "Size: " + FileSize.ToString() + " bytes" + Environment.NewLine +
                    "SHA256 Hash: 0x" + BLD.HexConverter.ByteArrayToHexString(mSHA256Hash) + Environment.NewLine +
                    "Fingerprint: " + Fingerprint.GetDisplayName() + Environment.NewLine;
                if (Fingerprint.Title != "Unrecognized File")
                {
                    fileInfo += "Title: " + Fingerprint.Title + Environment.NewLine;
                    if (Fingerprint.Platform != "")
                    {
                        fileInfo += "Platform: " + Fingerprint.Platform + Environment.NewLine;
                    }
                    if (Fingerprint.Format != "")
                    {
                        fileInfo += "Format/Region: " + Fingerprint.Format + Environment.NewLine;
                    }
                    if (Fingerprint.Language != "")
                    {
                        fileInfo += "Language: " + Fingerprint.Language + Environment.NewLine;
                    }
                    if (Fingerprint.ReleaseID != "")
                    {
                        fileInfo += "Release ID: " + Fingerprint.ReleaseID + Environment.NewLine;
                    }
                    if (Fingerprint.ReleaseType != "")
                    {
                        fileInfo += "Release Type: " + Fingerprint.ReleaseType + Environment.NewLine;
                    }
                    if (Fingerprint.FileName != "")
                    {
                        fileInfo += "FileName: " + Fingerprint.FileName + Environment.NewLine;
                    }
                    if (Fingerprint.BuildDate != "")
                    {
                        fileInfo += "Build Date: " + Fingerprint.BuildDate + Environment.NewLine;
                    }
                }
                if (mType == null)
                {
                    fileInfo += "Reading As: unknown filetype" + Environment.NewLine;
                }
                else
                {
                    fileInfo += "Reading As: " + mType.Description + Environment.NewLine;
                }

                if (mHashLookupTable != null)
                {
                    fileInfo += "Hash Lookup Table: " + HashLookupTable.Name + Environment.NewLine;
                }

                if (mMasterIndex != null)
                {
                    fileInfo += "Files Contained: " + mMasterIndex.FileCount;
                }
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
