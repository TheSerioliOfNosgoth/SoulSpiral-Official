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
using BF = BenLincoln.TheLostWorlds.CDBigFile;

namespace BenLincoln.TheLostWorlds.CDBigFile
{
    public class FileType
    {
        protected string mName;
        protected string mDescription;
        protected string mFileExtension;

        public const int FILE_TYPE_Invalid = 0;
        public const int FILE_TYPE_Unknown = 1;
        public const int FILE_TYPE_RAWImage = 2;
        public const int FILE_TYPE_RAWImage_Compressed = 3;
        public const int FILE_TYPE_RAWImage_Compressed_AirForge = 29;
        public const int FILE_TYPE_SND_Akuji = 4;
        public const int FILE_TYPE_PMF = 5;
        public const int FILE_TYPE_PNF = 6;
        public const int FILE_TYPE_SMF = 7;
        public const int FILE_TYPE_SMP = 8;
        public const int FILE_TYPE_SND = 9;
        public const int FILE_TYPE_SNF = 10;
        public const int FILE_TYPE_DRM_SR1_Room = 11;
        public const int FILE_TYPE_DRM_SR1_Object = 31;
        public const int FILE_TYPE_MessageList_SR1 = 12;
        public const int FILE_TYPE_DRM = 13;
        public const int FILE_TYPE_VRM = 14;
        public const int FILE_TYPE_LoadParms_SR1 = 15;
        public const int FILE_TYPE_TIM_4 = 16;
        public const int FILE_TYPE_TIM_8 = 17;
        public const int FILE_TYPE_TIM_16 = 18;
        public const int FILE_TYPE_TIM_24 = 19;
        public const int FILE_TYPE_SAM = 20;
        public const int FILE_TYPE_MUS = 21;
        public const int FILE_TYPE_INP = 22;
        public const int FILE_TYPE_VAG_Headerless = 23;
        public const int FILE_TYPE_VAB_Headerless = 24;
        public const int FILE_TYPE_DRM_SR2 = 25;
        public const int FILE_TYPE_MUL_Defiance = 26;
        public const int FILE_TYPE_STR_SR2_PS2 = 27;
        public const int FILE_TYPE_STR_SR2_PC = 30;
        public const int FILE_TYPE_CRM_SR1 = 28;
        public const int FILE_TYPE_RAW_SR2_PS2 = 32;
        public const int FILE_TYPE_DRM_SR2_Object = 33;
        public const int FILE_TYPE_DRM_SR2_Room_RETAIL = 34;
        public const int FILE_TYPE_DRM_SR2_Room_DEMO_NTSC = 35;
        public const int FILE_TYPE_DRM_SR2_Room_DEMO_PAL = 36;

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

        public string FileExtension
        {
            get
            {
                return mFileExtension;
            }
            set
            {
                mFileExtension = value;
            }
        }

        #endregion

        public FileType()
        {
            Name = "Unrecognized";
            Description = "A file that is not recognized by Soul Spiral";
            FileExtension = "dat";
        }

        //in this generic class, a type match is always true
        public virtual bool CheckType(BF.File checkFile)
        {
            return true;
        }

        //very few subclasses will implement this, but it is handy
        public virtual string GetInternalName(BF.File checkFile)
        {
            return "";
        }

        public static BF.FileType FromType(int newFileType)
        {
            switch (newFileType)
            {
                case FILE_TYPE_Invalid:
                    FileType ftInvalid = new FileType();
                    ftInvalid.Name = "Invalid Entry";
                    ftInvalid.Description = "Appears if the BigFile is not being read correctly";
                    ftInvalid.FileExtension = "null";
                    return ftInvalid;
                    break;
                case FILE_TYPE_Unknown:
                    return new FileType();
                    break;
                case FILE_TYPE_RAWImage:
                    FileTypeWithFixedHeader ftRAWImage = new FileTypeWithFixedHeader();
                    ftRAWImage.Name = "RAW Image";
                    ftRAWImage.Description = "Bitmap Image";
                    ftRAWImage.FileExtension = "raw";
                    ftRAWImage.Header = new byte[] { 0x21, 0x57, 0x41, 0x52 };
                    return ftRAWImage;
                    break;
                case FILE_TYPE_RAWImage_Compressed:
                    break;
                case FILE_TYPE_RAWImage_Compressed_AirForge:
                    break;
                case FILE_TYPE_SND_Akuji:
                    FileTypeWithFixedHeader ftSNDAkuji = new FileTypeWithFixedHeader();
                    ftSNDAkuji.Name = "SND (Akuji/Gex3)";
                    ftSNDAkuji.Description = "Collection of sound clips from Akuji or Gex 3";
                    ftSNDAkuji.FileExtension = "snd";
                    ftSNDAkuji.Header = new byte[] { 0x00, 0x00, 0x44, 0x4E, 0x53, 0x61 };
                    return ftSNDAkuji;
                    break;
                case FILE_TYPE_PMF:
                    FileTypeWithFixedHeader ftPMF = new FileTypeWithFixedHeader();
                    ftPMF.Name = "PMF";
                    ftPMF.Description = "Collection of sound clips from the PC version of Soul Reaver";
                    ftPMF.FileExtension = "pmf";
                    ftPMF.Header = new byte[] { 0x46, 0x4D, 0x50, 0x61 };
                    return ftPMF;
                    break;
                case FILE_TYPE_PNF:
                    FileTypeWithFixedHeader ftPNF = new FileTypeWithFixedHeader();
                    ftPNF.Name = "PNF";
                    ftPNF.Description = "Unknown";
                    ftPNF.FileExtension = "pnf";
                    ftPNF.Header = new byte[] { 0x46, 0x4E, 0x50, 0x61 };
                    return ftPNF;
                    break;
                case FILE_TYPE_SMF:
                    FileTypeWithFixedHeader ftSMF = new FileTypeWithFixedHeader();
                    ftSMF.Name = "SMF";
                    ftSMF.Description = "Collection of sound clips from the Playstation version of Soul Reaver";
                    ftSMF.FileExtension = "smf";
                    ftSMF.Header = new byte[] { 0x46, 0x4D, 0x53, 0x61 };
                    return ftSMF;
                    break;
                case FILE_TYPE_SMP:
                    FileTypeWithFixedHeader ftSMP = new FileTypeWithFixedHeader();
                    ftSMP.Name = "SMP";
                    ftSMP.Description = "Unknown";
                    ftSMP.FileExtension = "smp";
                    ftSMP.Header = new byte[] { 0x50, 0x4D, 0x53, 0x61 };
                    return ftSMP;
                    break;
                case FILE_TYPE_SND:
                    FileTypeWithFixedHeader ftSND = new FileTypeWithFixedHeader();
                    ftSND.Name = "SND";
                    ftSND.Description = "Collection of sound clips?";
                    ftSND.FileExtension = "snd";
                    ftSND.Header = new byte[] { 0x44, 0x4E, 0x53, 0x61 };
                    return ftSND;
                    break;
                case FILE_TYPE_SNF:
                    FileTypeWithFixedHeader ftSNF = new FileTypeWithFixedHeader();
                    ftSNF.Name = "SNF";
                    ftSNF.Description = "Unknown";
                    ftSNF.FileExtension = "snf";
                    ftSNF.Header = new byte[] { 0x46, 0x4E, 0x53, 0x61 };
                    return ftSNF;
                    break;
                case FILE_TYPE_DRM_SR1_Room:
                    FileTypeWithBlockHeader ftDRMSR1Room = new FileTypeWithBlockHeader();
                    ftDRMSR1Room.Name = "DRM - Room (Soul Reaver)";
                    ftDRMSR1Room.Description = "A room definition from Soul Reaver";
                    ftDRMSR1Room.FileExtension = "drm";
                    ftDRMSR1Room.Header = new byte[] { 0xE0, 0x01, 0x00, 0x00 };
                    ftDRMSR1Room.HeaderOffset = 96;
                    ftDRMSR1Room.IndexBlockSize = 2048;
                    ftDRMSR1Room.NameOffsetIsPointer = true;
                    ftDRMSR1Room.NameOffset = 152;
                    return ftDRMSR1Room;
                    break;
                case FILE_TYPE_DRM_SR1_Object:
                    FileTypeWithBlockHeader ftDRMSR1Object = new FileTypeWithBlockHeader();
                    ftDRMSR1Object.Name = "DRM - Object (Soul Reaver)";
                    ftDRMSR1Object.Description = "An object definition from Soul Reaver";
                    ftDRMSR1Object.FileExtension = "drm";
                    ftDRMSR1Object.Header = new byte[] { 0x64, 0x00, 0x00, 0x00 };
                    ftDRMSR1Object.HeaderOffset = 12;
                    ftDRMSR1Object.IndexBlockSize = 2048;
                    ftDRMSR1Object.NameOffset = 76;
                    return ftDRMSR1Object;
                    break;
                case FILE_TYPE_MessageList_SR1:
                    FileTypeWithFixedHeader ftMessageListSR1 = new FileTypeWithFixedHeader();
                    ftMessageListSR1.Name = "Message List (Soul Reaver)";
                    ftMessageListSR1.Description = "The error/status messages for the program";
                    ftMessageListSR1.FileExtension = "dat";
                    ftMessageListSR1.Header = new byte[] { 0x66, 0x6F, 0x72, 0x6D, 0x61, 0x74 };
                    ftMessageListSR1.HeaderOffset = 477;
                    return ftMessageListSR1;
                    break;
                case FILE_TYPE_DRM:
                    break;
                case FILE_TYPE_VRM:
                    break;
                case FILE_TYPE_LoadParms_SR1:
                    FileTypeWithFixedHeader ftLoadParmsSR1 = new FileTypeWithFixedHeader();
                    ftLoadParmsSR1.Name = "Load Parameters (Soul Reaver)";
                    ftLoadParmsSR1.Description = "The load parameters/argument list for Soul Reaver (retail versions)";
                    ftLoadParmsSR1.FileExtension = "dat";
                    ftLoadParmsSR1.Header = new byte[] { 0x75, 0x6E, 0x64, 0x65, 0x72, 0x20, 0x31, 0x20 };
                    return ftLoadParmsSR1;
                    break;
                case FILE_TYPE_TIM_4:
                    FileTypeWithFixedHeader ftTIM4 = new FileTypeWithFixedHeader();
                    ftTIM4.Name = "4-bit TIM";
                    ftTIM4.Description = "A Playstation bitmap image format";
                    ftTIM4.FileExtension = "tim";
                    ftTIM4.Header = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
                    return ftTIM4;
                    break;
                case FILE_TYPE_TIM_8:
                    FileTypeWithFixedHeader ftTIM8 = new FileTypeWithFixedHeader();
                    ftTIM8.Name = "8-bit TIM";
                    ftTIM8.Description = "A Playstation bitmap image format";
                    ftTIM8.FileExtension = "tim";
                    ftTIM8.Header = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00 };
                    return ftTIM8;
                    break;
                case FILE_TYPE_TIM_16:
                    FileTypeWithFixedHeader ftTIM16 = new FileTypeWithFixedHeader();
                    ftTIM16.Name = "16-bit TIM";
                    ftTIM16.Description = "A Playstation bitmap image format";
                    ftTIM16.FileExtension = "tim";
                    ftTIM16.Header = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00 };
                    return ftTIM16;
                    break;
                case FILE_TYPE_TIM_24:
                    FileTypeWithFixedHeader ftTIM24 = new FileTypeWithFixedHeader();
                    ftTIM24.Name = "24-bit TIM";
                    ftTIM24.Description = "A Playstation bitmap image format";
                    ftTIM24.FileExtension = "tim";
                    ftTIM24.Header = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00 };
                    return ftTIM24;
                    break;
                case FILE_TYPE_SAM:
                    FileTypeWithFixedHeader ftSAM = new FileTypeWithFixedHeader();
                    ftSAM.Name = "SAM";
                    ftSAM.Description = "Sound sample(s)?";
                    ftSAM.FileExtension = "sam";
                    ftSAM.Header = new byte[] { 0x53, 0x61, 0x6D, 0x21 };
                    return ftSAM;
                    break;
                case FILE_TYPE_MUS:
                    FileTypeWithFixedHeader ftMUS = new FileTypeWithFixedHeader();
                    ftMUS.Name = "MUS";
                    ftMUS.Description = "In-game music?";
                    ftMUS.FileExtension = "mus";
                    ftMUS.Header = new byte[] { 0x4D, 0x75, 0x73, 0x21 };
                    return ftMUS;
                    break;
                case FILE_TYPE_INP:
                    FileTypeWithFixedHeader ftINP = new FileTypeWithFixedHeader();
                    ftINP.Name = "INP";
                    ftINP.Description = "Input method definition?";
                    ftINP.FileExtension = "inp";
                    ftINP.Header = new byte[] { 0x69, 0x6E, 0x70, 0x21 };
                    return ftINP;
                    break;
                case FILE_TYPE_VAG_Headerless:
                    FileTypeWithFixedHeader ftVAGH = new FileTypeWithFixedHeader();
                    ftVAGH.Name = "Headerless VAG Audio File";
                    ftVAGH.Description = "A standard Playstation sound file, minus the standard header";
                    ftVAGH.FileExtension = "vag";
                    ftVAGH.Header = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    return ftVAGH;
                    break;
                case FILE_TYPE_VAB_Headerless:
                    FileTypeWithFixedHeader ftVABH = new FileTypeWithFixedHeader();
                    ftVABH.Name = "Headerless VAB Audio File";
                    ftVABH.Description = "A standard Playstation sound library file, minus the standard header";
                    ftVABH.FileExtension = "vab";
                    ftVABH.Header = new byte[] { 0x56, 0x41, 0x47, 0x70 };
                    return ftVABH;
                    break;
                case FILE_TYPE_DRM_SR2:
                    FileTypeWithFixedHeader ftDRMSR2 = new FileTypeWithFixedHeader();
                    ftDRMSR2.Name = "DRM (Soul Reaver 2)";
                    ftDRMSR2.Description = "Object/Character or 'Room' definition";
                    ftDRMSR2.FileExtension = "drm";
                    ftDRMSR2.Header = new byte[] { 0xE0, 0x01, 0x00, 0x00 };
                    return ftDRMSR2;
                    break;
                case FILE_TYPE_MUL_Defiance:
                    FileTypeWithFixedHeader ftMULDefiance = new FileTypeWithFixedHeader();
                    ftMULDefiance.Name = "MUL";
                    ftMULDefiance.Description = "Sound library";
                    ftMULDefiance.FileExtension = "mul";
                    ftMULDefiance.Header = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    ftMULDefiance.HeaderOffset = 4;
                    return ftMULDefiance;
                    break;
                case FILE_TYPE_STR_SR2_PS2:
                    FileTypeWithFixedHeader ftSTRSR2PS2 = new FileTypeWithFixedHeader();
                    ftSTRSR2PS2.Name = "STR (Soul Reaver 2 - Playstation 2)";
                    ftSTRSR2PS2.Description = "Sony compressed ADPCM sound library";
                    ftSTRSR2PS2.FileExtension = "str";
                    ftSTRSR2PS2.Header = new byte[] { 0x44, 0xAC, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF };
                    return ftSTRSR2PS2;
                    break;
                case FILE_TYPE_STR_SR2_PC:
                    FileTypeWithFixedHeader ftSTRSR2PC = new FileTypeWithFixedHeader();
                    ftSTRSR2PC.Name = "STR (Soul Reaver 2 - PC)";
                    ftSTRSR2PC.Description = "Ogg Vorbis sound library";
                    ftSTRSR2PC.FileExtension = "str";
                    ftSTRSR2PC.Header = new byte[] { 0x44, 0xAC, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF };
                    return ftSTRSR2PC;
                    break;
                case FILE_TYPE_CRM_SR1:
                    FileTypeWithFixedHeader ftCRMSR1 = new FileTypeWithFixedHeader();
                    ftCRMSR1.Name = "CRM (Soul Reaver)";
                    ftCRMSR1.Description = "Texture data for an object or room in the Playstation versions of Soul Reaver";
                    ftCRMSR1.FileExtension = "crm";
                    ftCRMSR1.Header = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x04, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    return ftCRMSR1;
                    break;
                case FILE_TYPE_RAW_SR2_PS2:
                    BF.FileTypeWithSR2Compression ftRAWSR2PS2 = new BF.FileTypeWithSR2Compression();
                    ftRAWSR2PS2.Name = "RAW Image (Soul Reaver 2 Compressed)";
                    ftRAWSR2PS2.Description = "Bitmap image (compressed)";
                    ftRAWSR2PS2.FileExtension = "raw";
                    ftRAWSR2PS2.Header = new byte[] { 0x21, 0x57, 0x41, 0x52 };
                    return ftRAWSR2PS2;
                    break;
                case FILE_TYPE_DRM_SR2_Room_RETAIL:
                    FileTypeWithInterlacedPointerData ftDRMSR2RoomRetail = new FileTypeWithInterlacedPointerData();
                    ftDRMSR2RoomRetail.Name = "DRM - Room (Soul Reaver 2)";
                    ftDRMSR2RoomRetail.Description = "A room definition from Soul Reaver 2";
                    ftDRMSR2RoomRetail.FileExtension = "drm";
                    ftDRMSR2RoomRetail.Header = new byte[] { 0x1D, 0x04, 0xC2, 0x04 }; // Retail
                    ftDRMSR2RoomRetail.HeaderOffset = 0x84;
                    ftDRMSR2RoomRetail.NamePointerOffset = 0x54;
                    return ftDRMSR2RoomRetail;
                    break;
                case FILE_TYPE_DRM_SR2_Room_DEMO_NTSC:
                    FileTypeWithInterlacedPointerData ftDRMSR2RoomDemoNTSC = new FileTypeWithInterlacedPointerData();
                    ftDRMSR2RoomDemoNTSC.Name = "DRM - Room (Soul Reaver 2)";
                    ftDRMSR2RoomDemoNTSC.Description = "A room definition from Soul Reaver 2";
                    ftDRMSR2RoomDemoNTSC.FileExtension = "drm";
                    ftDRMSR2RoomDemoNTSC.Header = new byte[] { 0x72, 0x41, 0x20, 0x4C }; // NTSC Demo
                    ftDRMSR2RoomDemoNTSC.HeaderOffset = 0x84;
                    ftDRMSR2RoomDemoNTSC.NamePointerOffset = 0x54;
                    return ftDRMSR2RoomDemoNTSC;
                    break;
                case FILE_TYPE_DRM_SR2_Room_DEMO_PAL:
                    FileTypeWithInterlacedPointerData ftDRMSR2RoomDemoPAL = new FileTypeWithInterlacedPointerData();
                    ftDRMSR2RoomDemoPAL.Name = "DRM - Room (Soul Reaver 2)";
                    ftDRMSR2RoomDemoPAL.Description = "A room definition from Soul Reaver 2";
                    ftDRMSR2RoomDemoPAL.FileExtension = "drm";
                    ftDRMSR2RoomDemoPAL.Header = new byte[] { 0x19, 0x04, 0xC2, 0x04 }; // PAL Demo
                    ftDRMSR2RoomDemoPAL.HeaderOffset = 0x84;
                    ftDRMSR2RoomDemoPAL.NamePointerOffset = 0x54;
                    return ftDRMSR2RoomDemoPAL;
                    break;
                case FILE_TYPE_DRM_SR2_Object:
                    FileTypeWithInterlacedPointerData ftDRMSR2Object = new FileTypeWithInterlacedPointerData();
                    ftDRMSR2Object.Name = "DRM - Object (Soul Reaver 2)";
                    ftDRMSR2Object.Description = "An object definition from Soul Reaver 2";
                    ftDRMSR2Object.FileExtension = "drm";
                    ftDRMSR2Object.Header = new byte[] { 0xA0, 0x0F, 0x10, 0x27, 0x70, 0x17, 0xE0, 0x2E };
                    ftDRMSR2Object.HeaderOffset = 0x1C;
                    ftDRMSR2Object.NamePointerOffset = 0x2C;
                    return ftDRMSR2Object;
                    break;
                default:
                    return new FileType();
                    break;
            }
            return new FileType();
        }
    }
}
