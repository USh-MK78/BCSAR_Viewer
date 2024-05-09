using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BCSARLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class INFO
    {
        public char[] INFO_Header { get; set; }
        public int INFOPartitionSize { get; set; }

        public Dictionary<int, INFOTable> INFOTableDictionary { get; set; }
        public class INFOTable
        {
            public int INFOReferenceID { get; set; }
            public BCSARHelper.Enum.INFOReferenceID INFOReferenceType
            {
                get
                {
                    return (BCSARHelper.Enum.INFOReferenceID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOReferenceID), INFOReferenceID);
                }
            }

            public int TableOffset { get; set; }

            public AudioTable Audio_Table { get; set; }
            public class AudioTable
            {
                public int TableCount { get; set; }

                public List<AudioData> AudioDataList { get; set; }
                public class AudioData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        public byte PlayerID { get; set; }
                        public byte PlayerActorID { get; set; }
                        public byte[] UnknownByteData0 { get; set; } //0x2
                        public short UnknownData0 { get; set; }
                        public byte UnknownByte0 { get; set; }
                        public byte UnknownByte1 { get; set; }

                        public int Volume { get; set; } //MAX=7F(?)

                        public int INFODataBlockID { get; set; } //0x4
                        public BCSARHelper.Enum.INFODataBlockID INFODataBlockType
                        {
                            get
                            {
                                return (BCSARHelper.Enum.INFODataBlockID)Enum.ToObject(typeof(BCSARHelper.Enum.INFODataBlockID), INFODataBlockID);
                            }
                        }

                        //Size:0x4C
                        public ID_2201 DataID_2201 { get; set; }
                        public class ID_2201
                        {
                            public int UnknownData2 { get; set; }
                            public byte UnknownData3 { get; set; }
                            public byte UnknownData4 { get; set; }
                            public byte UnknownData5 { get; set; }
                            public byte UnknownData6 { get; set; }
                            public int UnknownID { get; set; }

                            public int UnknownData7 { get; set; }
                            public int UnknownData8 { get; set; }
                            public int UnknownData9 { get; set; }
                            public int UnknownData10 { get; set; }

                            public int UnknownData11 { get; set; } // or float(x2)
                            public int UnknownData12 { get; set; }
                            public float UnknownData13 { get; set; }

                            public UnknownFlag UnknownFlags { get; set; }
                            public class UnknownFlag
                            {
                                public byte f1 { get; set; }
                                public byte f2 { get; set; }
                                public byte f3 { get; set; }
                                public byte f4 { get; set; }
                                public byte f5 { get; set; }
                                public byte f6 { get; set; }
                                public byte f7 { get; set; }
                                public byte f8 { get; set; }

                                public void Read_UnknownFlagData(BinaryReader br)
                                {
                                    f1 = br.ReadByte();
                                    f2 = br.ReadByte();
                                    f3 = br.ReadByte();
                                    f4 = br.ReadByte();
                                    f5 = br.ReadByte();
                                    f6 = br.ReadByte();
                                    f7 = br.ReadByte();
                                    f8 = br.ReadByte();
                                }

                                public UnknownFlag()
                                {
                                    f1 = 0;
                                    f2 = 0;
                                    f3 = 0;
                                    f4 = 0;
                                    f5 = 0;
                                    f6 = 0;
                                    f7 = 0;
                                    f8 = 0;
                                }
                            }

                            public int d1 { get; set; }
                            public short d2 { get; set; }
                            public short d3 { get; set; }
                            public int d4 { get; set; }
                            public int d5 { get; set; }

                            /// <summary>
                            /// Read(ID:2201)
                            /// </summary>
                            /// <param name="br"></param>
                            /// <param name="BOM"></param>
                            public void Read_ID_2201(BinaryReader br, byte[] BOM)
                            {
                                //CSAR_Helper.BOMSetting bOMSetting = CSAR_Helper.BOMChecker(BOM);
                                EndianConvert endianConvert = new EndianConvert(BOM);

                                UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData3 = br.ReadByte();
                                UnknownData4 = br.ReadByte();
                                UnknownData5 = br.ReadByte();
                                UnknownData6 = br.ReadByte();
                                UnknownID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData12 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData13 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

                                UnknownFlags.Read_UnknownFlagData(br);

                                d1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d2 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                d3 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                d4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            }

                            public ID_2201()
                            {
                                UnknownData2 = 0;
                                UnknownData3 = 0;
                                UnknownData4 = 0;
                                UnknownData5 = 0;
                                UnknownData6 = 0;
                                UnknownID = 0;

                                UnknownData7 = 0;
                                UnknownData8 = 0;
                                UnknownData9 = 0;
                                UnknownData10 = 0;

                                UnknownData11 = 0;
                                UnknownData12 = 0;
                                UnknownData13 = 0;

                                UnknownFlags = new UnknownFlag();

                                d1 = 0;
                                d2 = 0;
                                d3 = 0;
                                d4 = 0;
                                d5 = 0;
                            }

                            public override string ToString()
                            {
                                return "ID_2201";
                            }
                        }

                        //Size0x60
                        public ID_2202 DataID_2202 { get; set; }
                        public class ID_2202
                        {
                            public int UnknownData2 { get; set; }
                            public byte UnknownData3 { get; set; }
                            public byte UnknownData4 { get; set; }
                            public byte UnknownData5 { get; set; }
                            public byte UnknownData6 { get; set; }
                            public int UnknownID { get; set; }

                            public int UnknownData7 { get; set; }
                            public int UnknownData8 { get; set; }
                            public int UnknownData9 { get; set; }
                            public int UnknownData10 { get; set; }

                            public int UnknownData11 { get; set; } // or float(x2)
                            public int UnknownData12 { get; set; }
                            public float UnknownData13 { get; set; }

                            public UnknownFlag UnknownFlags { get; set; }
                            public class UnknownFlag
                            {
                                public byte f1 { get; set; }
                                public byte f2 { get; set; }
                                public byte f3 { get; set; }
                                public byte f4 { get; set; }
                                public byte f5 { get; set; }
                                public byte f6 { get; set; }
                                public byte f7 { get; set; }
                                public byte f8 { get; set; }
                                public byte f9 { get; set; }
                                public byte f10 { get; set; }
                                public byte f11 { get; set; }
                                public byte f12 { get; set; }

                                public void ReadUnknownFlag(BinaryReader br)
                                {
                                    f1 = br.ReadByte();
                                    f2 = br.ReadByte();
                                    f3 = br.ReadByte();
                                    f4 = br.ReadByte();
                                    f5 = br.ReadByte();
                                    f6 = br.ReadByte();
                                    f7 = br.ReadByte();
                                    f8 = br.ReadByte();
                                    f9 = br.ReadByte();
                                    f10 = br.ReadByte();
                                    f11 = br.ReadByte();
                                    f12 = br.ReadByte();
                                }

                                public UnknownFlag()
                                {
                                    f1 = 0;
                                    f2 = 0;
                                    f3 = 0;
                                    f4 = 0;
                                    f5 = 0;
                                    f6 = 0;
                                    f7 = 0;
                                    f8 = 0;
                                    f9 = 0;
                                    f10 = 0;
                                    f11 = 0;
                                    f12 = 0;
                                }
                            }

                            public int d1 { get; set; }
                            public int d2 { get; set; }
                            public byte d3 { get; set; }
                            public byte d4 { get; set; }
                            public byte d5 { get; set; }
                            public byte d6 { get; set; }
                            public int d7 { get; set; }
                            public int d8 { get; set; }
                            public int d9 { get; set; }
                            public int d10 { get; set; }
                            public int d11 { get; set; }

                            /// <summary>
                            /// Read(ID:2202)
                            /// </summary>
                            /// <param name="br"></param>
                            /// <param name="BOM"></param>
                            public void Read_ID_2202(BinaryReader br, byte[] BOM)
                            {
                                //CSAR_Helper.BOMSetting bOMSetting = CSAR_Helper.BOMChecker(BOM);

                                EndianConvert endianConvert = new EndianConvert(BOM);

                                UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData3 = br.ReadByte();
                                UnknownData4 = br.ReadByte();
                                UnknownData5 = br.ReadByte();
                                UnknownData6 = br.ReadByte();
                                UnknownID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData12 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

                                UnknownFlags.ReadUnknownFlag(br);

                                d1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d3 = br.ReadByte();
                                d4 = br.ReadByte();
                                d5 = br.ReadByte();
                                d6 = br.ReadByte();
                                d7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            }

                            public ID_2202()
                            {
                                UnknownData2 = 0;
                                UnknownData3 = 0;
                                UnknownData4 = 0;
                                UnknownData5 = 0;
                                UnknownData6 = 0;
                                UnknownID = 0;

                                UnknownData7 = 0;
                                UnknownData8 = 0;
                                UnknownData9 = 0;
                                UnknownData10 = 0;

                                UnknownData11 = 0;
                                UnknownData12 = 0;
                                UnknownData13 = 0;

                                UnknownFlags = new UnknownFlag();

                                d1 = 0;
                                d2 = 0;
                                d3 = 0;
                                d4 = 0;
                                d5 = 0;
                                d6 = 0;
                                d7 = 0;
                                d8 = 0;
                                d9 = 0;
                                d10 = 0;
                                d11 = 0;
                            }

                            public override string ToString()
                            {
                                return "ID_2202";
                            }
                        }

                        public ID_2203 DataID_2203 { get; set; }
                        public class ID_2203
                        {
                            public int UnknownData2 { get; set; }
                            public byte UnknownData3 { get; set; }
                            public byte UnknownData4 { get; set; }
                            public byte UnknownData5 { get; set; }
                            public byte UnknownData6 { get; set; }
                            public int UnknownID { get; set; }

                            public int UnknownData7 { get; set; }
                            public int UnknownData8 { get; set; }
                            public int UnknownData9 { get; set; }
                            public int UnknownData10 { get; set; }

                            public int UnknownData11 { get; set; } // or float(x2)
                            public int UnknownData12 { get; set; }
                            public float UnknownData13 { get; set; }

                            public UnknownFlag UnknownFlags { get; set; }
                            public class UnknownFlag
                            {
                                public byte f1 { get; set; }
                                public byte f2 { get; set; }
                                public byte f3 { get; set; }
                                public byte f4 { get; set; }
                                public byte f5 { get; set; }
                                public byte f6 { get; set; }
                                public byte f7 { get; set; }
                                public byte f8 { get; set; }
                                public byte f9 { get; set; }
                                public byte f10 { get; set; }
                                public byte f11 { get; set; }
                                public byte f12 { get; set; }
                                public byte f13 { get; set; }
                                public byte f14 { get; set; }
                                public byte f15 { get; set; }
                                public byte f16 { get; set; }

                                public void Read_UnknownFlag(BinaryReader br)
                                {
                                    f1 = br.ReadByte();
                                    f2 = br.ReadByte();
                                    f3 = br.ReadByte();
                                    f4 = br.ReadByte();
                                    f5 = br.ReadByte();
                                    f6 = br.ReadByte();
                                    f7 = br.ReadByte();
                                    f8 = br.ReadByte();
                                    f9 = br.ReadByte();
                                    f10 = br.ReadByte();
                                    f11 = br.ReadByte();
                                    f12 = br.ReadByte();
                                    f13 = br.ReadByte();
                                    f14 = br.ReadByte();
                                    f15 = br.ReadByte();
                                    f16 = br.ReadByte();
                                }

                                public UnknownFlag()
                                {
                                    f1 = 0;
                                    f2 = 0;
                                    f3 = 0;
                                    f4 = 0;
                                    f5 = 0;
                                    f6 = 0;
                                    f7 = 0;
                                    f8 = 0;
                                    f9 = 0;
                                    f10 = 0;
                                    f11 = 0;
                                    f12 = 0;
                                    f13 = 0;
                                    f14 = 0;
                                    f15 = 0;
                                    f16 = 0;
                                }
                            }

                            public int d1 { get; set; }
                            public int d2 { get; set; }
                            public int d3 { get; set; }
                            public int d4 { get; set; }
                            public int d5 { get; set; }
                            public int UnknownDataCount { get; set; }
                            public List<UnknownData> unknownDatas { get; set; }
                            public class UnknownData
                            {
                                public short ResourceID { get; set; }
                                public byte UnknownData0 { get; set; }
                                public byte UnknownData1 { get; set; }

                                public void ReadUnknownData(BinaryReader br, byte[] BOM)
                                {
                                    EndianConvert endianConvert = new EndianConvert(BOM);
                                    ResourceID = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(3)), 0);
                                    UnknownData0 = br.ReadByte();
                                    UnknownData1 = br.ReadByte();
                                }

                                public UnknownData()
                                {
                                    ResourceID = 0;
                                    UnknownData0 = 0;
                                    UnknownData1 = 255;
                                }
                            }

                            /// <summary>
                            /// Read(ID:2203)
                            /// </summary>
                            /// <param name="br"></param>
                            /// <param name="BOM"></param>
                            public void Read_ID_2203(BinaryReader br, byte[] BOM)
                            {
                                EndianConvert endianConvert = new EndianConvert(BOM);

                                UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData3 = br.ReadByte();
                                UnknownData4 = br.ReadByte();
                                UnknownData5 = br.ReadByte();
                                UnknownData6 = br.ReadByte();
                                UnknownID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData12 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

                                UnknownFlags.Read_UnknownFlag(br);

                                d1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                d5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                                UnknownDataCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                                for (int Count = 0; Count < UnknownDataCount; Count++)
                                {
                                    UnknownData unknownData = new UnknownData();
                                    unknownData.ReadUnknownData(br, BOM);
                                    unknownDatas.Add(unknownData);
                                }
                            }

                            public ID_2203()
                            {
                                UnknownData2 = 0;
                                UnknownData3 = 0;
                                UnknownData4 = 0;
                                UnknownData5 = 0;
                                UnknownData6 = 0;
                                UnknownID = 0;

                                UnknownData7 = 0;
                                UnknownData8 = 0;
                                UnknownData9 = 0;
                                UnknownData10 = 0;

                                UnknownData11 = 0;
                                UnknownData12 = 0;
                                UnknownData13 = 0;

                                UnknownFlags = new UnknownFlag();

                                d1 = 0;
                                d2 = 0;
                                d3 = 0;
                                d4 = 0;
                                d5 = 0;
                                UnknownDataCount = 0;
                                unknownDatas = new List<UnknownData>();
                            }

                            public override string ToString()
                            {
                                return "ID_2203";
                            }
                        }

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);

                            PlayerID = br.ReadByte();
                            PlayerActorID = br.ReadByte();
                            UnknownByteData0 = endianConvert.Convert(br.ReadBytes(2));
                            UnknownData0 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownByte0 = br.ReadByte();
                            UnknownByte1 = br.ReadByte();

                            Volume = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            INFODataBlockID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2201)
                            {
                                DataID_2201.Read_ID_2201(br, BOM);
                            }
                            else if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2202)
                            {
                                DataID_2202.Read_ID_2202(br, BOM);
                            }
                            else if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2203)
                            {
                                DataID_2203.Read_ID_2203(br, BOM);
                            }
                        }

                        public SoundInfo(BCSARHelper.Enum.INFODataBlockID BlockID)
                        {
                            PlayerID = 255;
                            PlayerActorID = 0;
                            UnknownByteData0 = new byte[2];
                            UnknownData0 = 0;
                            UnknownByte0 = 0x00;
                            UnknownByte1 = 0x00;
                            Volume = 0;
                            INFODataBlockID = (int)BlockID;
                            if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2201)
                            {
                                DataID_2201 = new ID_2201();
                            }
                            else if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2202)
                            {
                                DataID_2202 = new ID_2202();
                            }
                            else if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2203)
                            {
                                DataID_2203 = new ID_2203();
                            }
                        }

                        public SoundInfo()
                        {
                            PlayerID = 255;
                            PlayerActorID = 0;
                            UnknownByteData0 = new byte[2];
                            UnknownData0 = 0;
                            UnknownByte0 = 0x00;
                            UnknownByte1 = 0x00;
                            Volume = 0;
                            INFODataBlockID = 0;
                            DataID_2201 = new ID_2201();
                            DataID_2202 = new ID_2202();
                            DataID_2203 = new ID_2203();
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (Audio)";
                        }
                    }

                    /// <summary>
                    /// Read Audio
                    /// </summary>
                    /// <param name="br"></param>
                    /// <param name="BOM"></param>
                    public void ReadAudioData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="INFODataBlockID"></param>
                    public AudioData(BCSARHelper.Enum.INFODataBlockID INFODataBlockID)
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo(INFODataBlockID);
                    }

                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="INFODataBlockID"></param>
                    public AudioData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "AudioData | TableType : " + INFOTableType;
                    }
                }

                public void ReadAudioTable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        AudioData audioData = new AudioData();
                        audioData.ReadAudioData(br, BOM, Pos);
                        AudioDataList.Add(audioData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public AudioTable()
                {
                    TableCount = 0;
                    AudioDataList = new List<AudioData>();
                }

                public override string ToString()
                {
                    return "Audio Table | Count : " + TableCount;
                }
            }

            public SetTable Set_Table { get; set; }
            public class SetTable
            {
                public int TableCount { get; set; }

                public List<SetData> SetDataList { get; set; }
                public class SetData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        //public short PlayerID { get; set; }
                        //public short PlayerActorID { get; set; }
                        //public int FirstSoundID { get; set; }
                        //public int LastSoundID { get; set; }

                        public short UnknownShortData0 { get; set; } //PlayerID
                        public short UnknownShortData1 { get; set; }
                        public short UnknownShortData2 { get; set; } //PlayerActorID
                        public short UnknownShortData3 { get; set; }
                        public short UnknownShortData4 { get; set; }
                        public short UnknownShortData5 { get; set; }

                        public int UnknownData2 { get; set; }
                        public int INFODataBlockID { get; set; } //ID_2205
                        public BCSARHelper.Enum.INFODataBlockID INFODataBlockType
                        {
                            get
                            {
                                return (BCSARHelper.Enum.INFODataBlockID)Enum.ToObject(typeof(BCSARHelper.Enum.INFODataBlockID), INFODataBlockID);
                            }
                        }

                        public ID_2205 Data_ID_2205 { get; set; }
                        public class ID_2205
                        {
                            public int UnknownData4 { get; set; }
                            public int UnknownData5 { get; set; }
                            public int UnknownData6 { get; set; }
                            public int UnknownData7 { get; set; }
                            public int UnknownData8 { get; set; }
                            
                            public short UnknownShortData12 { get; set; }
                            public short UnknownShortData13 { get; set; }

                            public int UnknownData9 { get; set; }
                            public int UnknownData10 { get; set; }
                            public int UnknownData11 { get; set; }

                            public void Read_ID_2205(BinaryReader br, byte[] BOM)
                            {
                                EndianConvert endianConvert = new EndianConvert(BOM);

                                UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                                UnknownShortData12 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                UnknownShortData13 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                                UnknownData9 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                                UnknownData11 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            }

                            public ID_2205()
                            {
                                UnknownData4 = 0;
                                UnknownData5 = 0;
                                UnknownData6 = 0;
                                UnknownData7 = 0;
                                UnknownData8 = 0;
                                UnknownShortData12 = 0;
                                UnknownShortData13 = 0;
                                UnknownData9 = 0;
                                UnknownData10 = 0;
                                UnknownData11 = 0;
                            }
                        }

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            UnknownShortData0 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownShortData1 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownShortData2 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownShortData3 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownShortData4 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownShortData5 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

                            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            INFODataBlockID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            if (INFODataBlockType == BCSARHelper.Enum.INFODataBlockID.ID_2205)
                            {
                                Data_ID_2205.Read_ID_2205(br, BOM);
                            }
                        }

                        public SoundInfo()
                        {
                            UnknownShortData0 = 0;
                            UnknownShortData1 = 0;
                            UnknownShortData2 = 0;
                            UnknownShortData3 = 0;
                            UnknownShortData4 = 0;
                            UnknownShortData5 = 0;

                            UnknownData2 = 0;
                            INFODataBlockID = 0;
                            Data_ID_2205 = new ID_2205();
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (Set)";
                        }
                    }

                    /// <summary>
                    /// Read_SetData
                    /// </summary>
                    /// <param name="br"></param>
                    /// <param name="BOM"></param>
                    public void Read_SetData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);

                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    public SetData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "SetData | TableType : " + INFOTableType;
                    }
                }

                public void ReadSetTable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        SetData setData = new SetData();
                        setData.Read_SetData(br, BOM, Pos);
                        SetDataList.Add(setData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public SetTable()
                {
                    TableCount = 0;
                    SetDataList = new List<SetData>();
                }

                public override string ToString()
                {
                    return "Set Table | Count : " + TableCount;
                }
            }

            public BankTable Bank_Table { get; set; }
            public class BankTable
            {
                public int TableCount { get; set; }

                public List<BankData> BankDataList { get; set; }
                public class BankData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        public byte PlayerID { get; set; }
                        public byte GroupID { get; set; }

                        public short UnknownData0 { get; set; }
                        public short UnknownData1 { get; set; }
                        public short UnknownData2 { get; set; }

                        public int UnknownData3 { get; set; }
                        public int UnknownData4 { get; set; }

                        public byte UnknownID_0 { get; set; }
                        public byte UnknownID_1 { get; set; }

                        public byte[] UnknownByteData { get; set; } //0x6

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);

                            PlayerID = br.ReadByte();
                            GroupID = br.ReadByte();

                            UnknownData0 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownData1 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                            UnknownData2 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);

                            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                            UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                            UnknownID_0 = br.ReadByte();
                            UnknownID_1 = br.ReadByte();

                            UnknownByteData = br.ReadBytes(6);
                        }

                        public SoundInfo()
                        {
                            PlayerID = 255;
                            GroupID = 0;
                            UnknownData0 = 0;
                            UnknownData1 = 0;
                            UnknownData2 = 0;
                            UnknownData3 = 0;
                            UnknownData4 = 0;
                            UnknownID_0 = 0x00;
                            UnknownID_1 = 0x00;
                            UnknownByteData = new byte[6];
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (Bank)";
                        }
                    }

                    /// <summary>
                    /// Read BankData
                    /// </summary>
                    /// <param name="br"></param>
                    /// <param name="BOM"></param>
                    public void ReadBankData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);

                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    public BankData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "BankData | TableType : " + INFOTableType;
                    }
                }

                public void ReadBankTable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        BankData bankData = new BankData();
                        bankData.ReadBankData(br, BOM, Pos);
                        BankDataList.Add(bankData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public BankTable()
                {
                    TableCount = 0;
                    BankDataList = new List<BankData>();
                }

                public override string ToString()
                {
                    return "Bank Table | Count : " + TableCount;
                }
            }

            public WAVArchiveTable WAVArchive_Table { get; set; }
            public class WAVArchiveTable
            {
                public int TableCount { get; set; }

                public List<WAVArchiveData> WAVArchiveDataList { get; set; }
                public class WAVArchiveData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        public byte PlayerID { get; set; }
                        public byte GroupID { get; set; }
                        public byte[] UnknownData0 { get; set; } //0x2
                        public byte[] UnknownData1 { get; set; } //0x4
                        public byte[] UnknownData2 { get; set; } //0x4

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);

                            PlayerID = br.ReadByte();
                            GroupID = br.ReadByte();
                            UnknownData0 = endianConvert.Convert(br.ReadBytes(2));
                            UnknownData1 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownData2 = endianConvert.Convert(br.ReadBytes(4));
                        }

                        public SoundInfo()
                        {
                            PlayerID = 255;
                            GroupID = 0;
                            UnknownData0 = new byte[2];
                            UnknownData1 = new byte[4];
                            UnknownData2 = new byte[4];
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (WAVArchive)";
                        }
                    }

                    /// <summary>
                    /// Read WAV Archive Data
                    /// </summary>
                    /// <param name="br"></param>
                    /// <param name="BOM"></param>
                    public void Read_WAVArchiveData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);

                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    public WAVArchiveData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "WAVArchiveData | TableType : " + INFOTableType;
                    }
                }

                public void ReadWAVArchiveTable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        WAVArchiveData wAVArchiveData = new WAVArchiveData();
                        wAVArchiveData.Read_WAVArchiveData(br, BOM, Pos);
                        WAVArchiveDataList.Add(wAVArchiveData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public WAVArchiveTable()
                {
                    TableCount = 0;
                    WAVArchiveDataList = new List<WAVArchiveData>();
                }

                public override string ToString()
                {
                    return "WAVArchive Table | Count : " + TableCount;
                }
            }

            public GroupTable Group_Table { get; set; }
            public class GroupTable
            {
                public int TableCount { get; set; }

                public List<GroupData> GroupDataList { get; set; }
                public class GroupData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        public byte PlayerID { get; set; }
                        public byte UnknownByte0 { get; set; }
                        public byte[] UnknownData0 { get; set; } //0x2

                        public byte[] UnknownData1 { get; set; } //0x4

                        public byte UnknownByte1 { get; set; }
                        public byte UnknownByte2 { get; set; }
                        public byte[] UnknownData2 { get; set; } //0x2

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            PlayerID = br.ReadByte();
                            UnknownByte0 = br.ReadByte();
                            UnknownData0 = endianConvert.Convert(br.ReadBytes(2));

                            UnknownData1 = endianConvert.Convert(br.ReadBytes(4));

                            UnknownByte1 = br.ReadByte();
                            UnknownByte2 = br.ReadByte();
                            UnknownData2 = endianConvert.Convert(br.ReadBytes(2));
                        }

                        public SoundInfo()
                        {
                            PlayerID = 0x00;
                            UnknownByte0 = 0x00;
                            UnknownData0 = new byte[2];

                            UnknownData1 = new byte[4];

                            UnknownByte1 = 0x00;
                            UnknownByte2 = 0x00;
                            UnknownData2 = new byte[2];
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (Group)";
                        }
                    }

                    public void ReadGroupData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        
                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    public GroupData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "GroupData | TableType : " + INFOTableType;
                    }
                }

                public void ReadGroupTable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        GroupData groupData = new GroupData();
                        groupData.ReadGroupData(br, BOM, Pos);
                        GroupDataList.Add(groupData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public GroupTable()
                {
                    TableCount = 0;
                    GroupDataList = new List<GroupData>();
                }

                public override string ToString()
                {
                    return "Group Table | Count : " + TableCount;
                }
            }

            public PlayerTable Player_Table { get; set; }
            public class PlayerTable
            {
                public int TableCount { get; set; }

                public List<PlayerData> PlayerDataList { get; set; }
                public class PlayerData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        public byte UnknownID_1 { get; set; }
                        public byte UnknownByte0 { get; set; }
                        public byte[] UnknownData0 { get; set; } //0x2

                        public byte UnknownID_2 { get; set; }
                        public byte UnknownByte1 { get; set; }
                        public byte[] UnknownData1 { get; set; } //0x2

                        public byte PlayerID { get; set; }
                        public byte PlayerActorID { get; set; }
                        public byte[] UnknownData2 { get; set; } //0x2

                        public byte[] UnknownData3 { get; set; } //0x4

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            UnknownID_1 = br.ReadByte();
                            UnknownByte0 = br.ReadByte();
                            UnknownData0 = endianConvert.Convert(br.ReadBytes(2));

                            UnknownID_2 = br.ReadByte();
                            UnknownByte1 = br.ReadByte();
                            UnknownData1 = endianConvert.Convert(br.ReadBytes(2));

                            PlayerID = br.ReadByte();
                            PlayerActorID = br.ReadByte();
                            UnknownData2 = endianConvert.Convert(br.ReadBytes(2));

                            UnknownData3 = endianConvert.Convert(br.ReadBytes(4));
                        }

                        public SoundInfo()
                        {
                            UnknownID_1 = 0x00;
                            UnknownByte0 = 0x00;
                            UnknownData0 = new byte[2];

                            UnknownID_2 = 0x00;
                            UnknownByte1 = 0x00;
                            UnknownData1 = new byte[2];

                            PlayerID = 0x00;
                            PlayerActorID = 0x00;
                            UnknownData2 = new byte[2];

                            UnknownData3 = new byte[4];
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (Player)";
                        }
                    }

                    public void ReadPlayerData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);

                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    public PlayerData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "PlayerData | TableType : " + INFOTableType;
                    }
                }

                public void ReadPlayerTable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        PlayerData playerData = new PlayerData();
                        playerData.ReadPlayerData(br, BOM, Pos);
                        PlayerDataList.Add(playerData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public PlayerTable()
                {
                    TableCount = 0;
                    PlayerDataList = new List<PlayerData>();
                }

                public override string ToString()
                {
                    return "Player Table | Count : " + TableCount;
                }
            }

            public FILETable FILE_Table { get; set; }
            public class FILETable
            {
                public int TableCount { get; set; }

                public List<FILEData> FILEDataList { get; set; }
                public class FILEData
                {
                    public int INFOTableID { get; set; }
                    public BCSARHelper.Enum.INFOTableID INFOTableType
                    {
                        get
                        {
                            return (BCSARHelper.Enum.INFOTableID)Enum.ToObject(typeof(BCSARHelper.Enum.INFOTableID), INFOTableID);
                        }
                    }

                    public int SoundInfoOffset { get; set; }

                    public SoundInfo Sound_Info { get; set; }
                    public class SoundInfo
                    {
                        public string FileName { get; set; }

                        public byte UnknownByte0 { get; set; }
                        public byte UnknownByte1 { get; set; }
                        public byte[] UnknownData0 { get; set; } //0x2
                        public byte[] UnknownData1 { get; set; } //0x4
                        public byte[] UnknownData2 { get; set; } //0x4

                        public void ReadSoundInfo(BinaryReader br, byte[] BOM)
                        {
                            EndianConvert endianConvert = new EndianConvert(BOM);
                            UnknownByte0 = br.ReadByte();
                            UnknownByte1 = br.ReadByte();
                            UnknownData0 = endianConvert.Convert(br.ReadBytes(2));
                            UnknownData1 = endianConvert.Convert(br.ReadBytes(4));
                            UnknownData2 = endianConvert.Convert(br.ReadBytes(4));

                            #region UnknownData1 => This Offset(?) [From : SoundInfoBeginPos]
                            BCSARHelper.ReadByteLine readByteLine = new BCSARHelper.ReadByteLine(new List<byte>());
                            readByteLine.ReadByte(br, 0x00);

                            FileName = new string(readByteLine.ConvertToCharArray());
                            #endregion
                        }

                        public SoundInfo()
                        {
                            FileName = " ";
                            UnknownByte0 = 0x00;
                            UnknownByte1 = 0x00;
                            UnknownData0 = new byte[2];
                            UnknownData1 = new byte[4];
                            UnknownData2 = new byte[4];
                        }

                        public override string ToString()
                        {
                            return "SoundInfo (FILE)";
                        }
                    }

                    public void ReadFILEData(BinaryReader br, byte[] BOM, long Pos)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);

                        INFOTableID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        SoundInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (SoundInfoOffset != 0)
                        {
                            long CurPos = br.BaseStream.Position;

                            br.BaseStream.Position = Pos;

                            br.BaseStream.Seek(SoundInfoOffset, SeekOrigin.Current);

                            Sound_Info.ReadSoundInfo(br, BOM);

                            br.BaseStream.Position = CurPos;
                        }
                    }

                    public string GetPath()
                    {
                        return Sound_Info.FileName;
                    }

                    public FILEData()
                    {
                        INFOTableID = 0;
                        SoundInfoOffset = 0;
                        Sound_Info = new SoundInfo();
                    }

                    public override string ToString()
                    {
                        return "FILEData | TableType : " + INFOTableType;
                    }
                }

                public void Read_FILETable(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);

                    long Pos = br.BaseStream.Position;

                    TableCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    for (int i = 0; i < TableCount; i++)
                    {
                        FILEData fILEData = new FILEData();
                        fILEData.ReadFILEData(br, BOM, Pos);
                        FILEDataList.Add(fILEData);
                    }

                    br.BaseStream.Position = INFODataBasePos;
                }

                public FILETable()
                {
                    TableCount = 0;
                    FILEDataList = new List<FILEData>();
                }

                public override string ToString()
                {
                    return "FILE Table | Count : " + TableCount;
                }
            }

            public FlieInfoTable FileInfo_Table { get; set; }
            public class FlieInfoTable
            {
                public FileInfoData FileInfo_Data { get; set; }
                public class FileInfoData
                {
                    public short MaxNumberSequence { get; set; }
                    public short MaxNumberSequenceTrack { get; set; }
                    public short MaxNumberStream { get; set; }
                    public short MaxNumberStreamTrack { get; set; }
                    public short MaxNumberWave { get; set; }
                    public short MaxNumberWaveTrack { get; set; }
                    public short StreamBufferTime { get; set; }
                    public short Option { get; set; }
                    public byte[] UnknownByteData { get; set; } //0x28

                    //public bool IncludeStringBlock {get; set; }

                    public void ReadFileInfoData(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        MaxNumberSequence = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        MaxNumberSequenceTrack = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        MaxNumberStream = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        MaxNumberStreamTrack = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        MaxNumberWave = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        MaxNumberWaveTrack = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        StreamBufferTime = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        Option = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                        UnknownByteData = endianConvert.Convert(br.ReadBytes(28));
                    }

                    public FileInfoData()
                    {
                        MaxNumberSequence = 0;
                        MaxNumberSequenceTrack = 0;
                        MaxNumberStream = 0;
                        MaxNumberStreamTrack = 0;
                        MaxNumberWave = 0;
                        MaxNumberWaveTrack = 0;
                        StreamBufferTime = 0;
                        Option = 0;
                        UnknownByteData = new byte[28];
                    }
                }

                public void ReadInformationTable(BinaryReader br, byte[] BOM)
                {
                    //DatabasePosition
                    long INFODataBasePos = br.BaseStream.Position;

                    FileInfo_Data.ReadFileInfoData(br, BOM);

                    br.BaseStream.Position = INFODataBasePos;
                }

                public FlieInfoTable()
                {
                    FileInfo_Data = new FileInfoData();
                }

                public override string ToString()
                {
                    return "File Info";
                }
            }

            public void Read_INFOTable(BinaryReader br, byte[] BOM, long Pos)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);

                INFOReferenceID = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.AudioTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        Audio_Table.ReadAudioTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.SetTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        Set_Table.ReadSetTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.BankTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        Bank_Table.ReadBankTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.WAVArchiveTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        WAVArchive_Table.ReadWAVArchiveTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.GroupTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        Group_Table.ReadGroupTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.PlayerTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        Player_Table.ReadPlayerTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.FILETable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        FILE_Table.Read_FILETable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
                else if (INFOReferenceType == BCSARHelper.Enum.INFOReferenceID.FileInfoTable)
                {
                    TableOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TableOffset != 0)
                    {
                        //Save CurrentPos
                        long CurPos = br.BaseStream.Position;

                        //Move TableOriginOffset
                        br.BaseStream.Position = Pos;

                        br.BaseStream.Seek(TableOffset, SeekOrigin.Current);

                        FileInfo_Table.ReadInformationTable(br, BOM);

                        //Move CurrentPos
                        br.BaseStream.Position = CurPos;
                    }
                }
            }

            public INFOTable()
            {
                INFOReferenceID = 0;
                TableOffset = 0;
                Audio_Table = new AudioTable();
                Set_Table = new SetTable();
                Bank_Table = new BankTable();
                WAVArchive_Table = new WAVArchiveTable();
                Group_Table = new GroupTable();
                Player_Table = new PlayerTable();
                FILE_Table = new FILETable();
                FileInfo_Table = new FlieInfoTable();
            }

            public override string ToString()
            {
                return INFOReferenceID.ToString();
            }
        }

        public void ReadINFO(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            INFO_Header = br.ReadChars(4);
            if (new string(INFO_Header) != "INFO") throw new Exception("INFO chunk doesn't match.");
            INFOPartitionSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            long Pos = br.BaseStream.Position;

            for (int i = 0; i < 7; i++)
            {
                INFOTable iNFOTable = new INFOTable();
                iNFOTable.Read_INFOTable(br, BOM, Pos);
                INFOTableDictionary.Add(i, iNFOTable);
            }
        }

        public INFO()
        {
            INFO_Header = "INFO".ToCharArray();
            INFOPartitionSize = 0;
            INFOTableDictionary = new Dictionary<int, INFOTable>();
        }

        public override string ToString()
        {
            return "INFO";
        }
    }
}
