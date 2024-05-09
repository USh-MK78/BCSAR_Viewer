using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BCSARLibrary
{
    /// <summary>
    /// BCSAR
    /// </summary>
    public class BCSAR
    {
        public char[] CSAR_Header { get; set; }
        public byte[] BOM { get; set; }
        public short BCSARHeaderSize { get; set; }
        public byte[] Version { get; set; }
        public int FileSize { get; set; }
        public int PartitionCount { get; set; }
        public List<PartitionOffsetInfo> PartitionOffsetInfoList { get; set; }
        public class PartitionOffsetInfo
        {
            public BCSARHelper.Enum.PartitionReferenceID ReferenceID { get; set; } //STRG:0x2000, INFO:0x2001, FILE:2002
            public int Offset { get; set; } //From : CSAR_Header
            public int Length { get; set; }

            public Partition PartitionData { get; set; }
            public class Partition
            {
                public STRG Partition_STRG { get; set; }
                public INFO Partition_INFO { get; set; }
                public FILE Partition_FILE { get; set; }

                public Partition()
                {
                    Partition_STRG = new STRG();
                    Partition_INFO = new INFO();
                    Partition_FILE = new FILE();
                }

                public override string ToString()
                {
                    return "Partition";
                }
            }

            public void Read_PartitionOffsetInfo(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                ReferenceID = (BCSARHelper.Enum.PartitionReferenceID)BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                Length = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                if (Offset != 0)
                {
                    if (ReferenceID == BCSARHelper.Enum.PartitionReferenceID.STRG)
                    {
                        long Pos = br.BaseStream.Position;

                        //Move
                        br.BaseStream.Position = 0;

                        //Move Offset
                        br.BaseStream.Seek(Offset, SeekOrigin.Current);

                        PartitionData.Partition_STRG.ReadSTRG(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                    else if (ReferenceID == BCSARHelper.Enum.PartitionReferenceID.INFO)
                    {
                        long Pos = br.BaseStream.Position;

                        //Move
                        br.BaseStream.Position = 0;

                        //Move Offset
                        br.BaseStream.Seek(Offset, SeekOrigin.Current);

                        PartitionData.Partition_INFO.ReadINFO(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                    else if (ReferenceID == BCSARHelper.Enum.PartitionReferenceID.FILE)
                    {
                        long Pos = br.BaseStream.Position;

                        //Move
                        br.BaseStream.Position = 0;

                        //Move Offset
                        br.BaseStream.Seek(Offset, SeekOrigin.Current);

                        //PartitionData.Partition_FILE.ReadFILE(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                }
            }

            public void Write_PartitionOffsetInfo(BinaryWriter bw, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                bw.Write(endianConvert.Convert(BitConverter.GetBytes((int)ReferenceID)));
                bw.Write(endianConvert.Convert(BitConverter.GetBytes(Offset)));
                bw.Write(endianConvert.Convert(BitConverter.GetBytes(Length)));
            }

            public PartitionOffsetInfo()
            {
                ReferenceID = BCSARHelper.Enum.PartitionReferenceID.NotSet;
                Offset = 0;
                Length = 0;
                PartitionData = new Partition();
            }

            public override string ToString()
            {
                return "Partition : " + ReferenceID.ToString();
            }
        }

        public byte[] UnknownByteData0 { get; set; } //0x8

        public void Read_BCSAR(BinaryReader br)
        {
            CSAR_Header = br.ReadChars(4);
            if (new string(CSAR_Header) != "CSAR") throw new Exception("CSAR chunk doesn't match.");

            BOM = br.ReadBytes(2);

            EndianConvert endianConvert = new EndianConvert(BOM);

            BCSARHeaderSize = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
            Version = endianConvert.Convert(br.ReadBytes(4));
            FileSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            PartitionCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            for (int i = 0; i < PartitionCount; i++)
            {
                PartitionOffsetInfo partitionOffsetInfo = new PartitionOffsetInfo();
                partitionOffsetInfo.Read_PartitionOffsetInfo(br, BOM);
                PartitionOffsetInfoList.Add(partitionOffsetInfo);
            }

            UnknownByteData0 = endianConvert.Convert(br.ReadBytes(8));
        }

        public BCSAR()
        {
            CSAR_Header = "CSAR".ToCharArray();
            BOM = new byte[2];
            BCSARHeaderSize = 0;
            Version = new byte[4];
            FileSize = 0;
            PartitionCount = 0;
            PartitionOffsetInfoList = new List<PartitionOffsetInfo>();
            UnknownByteData0 = new byte[8];
        }
    }
}
