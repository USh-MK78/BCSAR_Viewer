using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSARLibrary
{
    public class STRG
    {
        public char[] STRG_Header { get; set; } //0x4, STRG
        public int STRGPartitionSize { get; set; } //0x4
        public BCSARHelper.Enum.STRGTableType StringOffsetTableType { get; set; } //0x2400 (0x00002400), 0x4
        public int StringOffsetTable_Offset { get; set; } //0x4, From StringOffsetTableType (Begin)
        public BCSARHelper.Enum.STRGTableType StringTableLookupType { get; set; } //0x2401 (0x00002401), 0x4
        public int StringTableLookupType_Offset { get; set; } //0x4, Start position:from STRG header string
        public int FileNameCount { get; set; } //0x4, FileNameCount

        public List<StringTable> StringTableList { get; set; }
        public class StringTable
        {
            public string Name { get; set; }
            public byte[] NodeType { get; set; } //0x1F01 (0x00001F01), 0x4
            public int TableDataOffset { get; set; } //Start position:End of the STRG header, 0x4
            public int DataBufferLength { get; set; } //0x4
            
            public void ReadStringTable(BinaryReader br, byte[] BOM, long StringTableLoopOffset)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                NodeType = endianConvert.Convert(br.ReadBytes(4));
                TableDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                DataBufferLength = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (TableDataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Position = StringTableLoopOffset;
                    br.BaseStream.Seek(TableDataOffset, SeekOrigin.Current);

                    char[] vd = br.ReadChars(DataBufferLength);
                    Name = new string(vd);

                    br.BaseStream.Position = Pos;
                }
            }

            public StringTable(BCSARHelper.Enum.STRGTableType STRGTableType)
            {
                NodeType = BitConverter.GetBytes((int)STRGTableType);
                TableDataOffset = 0;
                DataBufferLength = 0;
                Name = "";
            }

            public StringTable()
            {
                NodeType = new byte[4];
                TableDataOffset = 0;
                DataBufferLength = 0;
                Name = "";
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public List<LookupTable> LookupTableList { get; set; }
        public class LookupTable
        {
            public short ResourceID { get; set; }
            public short UnknownData0 { get; set; }
            public short ID { get; set; }
            public byte UnknownByteData0 { get; set; }
            public byte DataTypeID { get; set; }
            public BCSARHelper.Enum.ResourceID DataType
            {
                get
                {
                    return (BCSARHelper.Enum.ResourceID)Enum.ToObject(typeof(BCSARHelper.Enum.ResourceID), DataTypeID);
                }
            }

            public short UnknownData1 { get; set; }
            public short UnknownData2 { get; set; } //Default : FF FF
            public int UnknownData3 { get; set; } //Default : FF FF FF FF
            public int UnknownData4 { get; set; } //Default : FF FF FF FF

            public UnknownByteData Unknown_ByteData { get; set; }
            public class UnknownByteData
            {
                public byte Data_0 { get; set; }
                public byte Data_1 { get; set; }
                public byte Data_2 { get; set; }
                public byte Data_3 { get; set; }
                public byte Data_4 { get; set; }
                public byte Data_5 { get; set; }
                public byte Data_6 { get; set; }
                public byte Data_7 { get; set; }

                public void Read_UnknownByteData(BinaryReader br)
                {
                    Data_0 = br.ReadByte();
                    Data_1 = br.ReadByte();
                    Data_2 = br.ReadByte();
                    Data_3 = br.ReadByte();
                    Data_4 = br.ReadByte();
                    Data_5 = br.ReadByte();
                    Data_6 = br.ReadByte();
                    Data_7 = br.ReadByte();
                }

                public UnknownByteData()
                {
                    Data_0 = 0xFF;
                    Data_1 = 0xFF;
                    Data_2 = 0xFF;
                    Data_3 = 0xFF;
                    Data_4 = 0xFF;
                    Data_5 = 0xFF;
                    Data_6 = 0xFF;
                    Data_7 = 0xFF;
                }
            }

            // if DataType = 6, (UnknownData5, UnknownBytData2) == NULL

            public short UnknownData5 { get; set; }

            public UnknownByteData2 Unknown_ByteData2 { get; set; }
            public class UnknownByteData2
            {
                public byte Data_0 { get; set; }
                public byte Data_1 { get; set; }
                public byte Data_2 { get; set; }
                public byte Data_3 { get; set; }
                public byte Data_4 { get; set; }
                public byte Data_5 { get; set; }
                public byte Data_6 { get; set; }
                public byte Data_7 { get; set; }
                public byte Data_8 { get; set; }
                public byte Data_9 { get; set; }

                public void Read_UnknownByteData2(BinaryReader br)
                {
                    Data_0 = br.ReadByte();
                    Data_1 = br.ReadByte();
                    Data_2 = br.ReadByte();
                    Data_3 = br.ReadByte();
                    Data_4 = br.ReadByte();
                    Data_5 = br.ReadByte();
                    Data_6 = br.ReadByte();
                    Data_7 = br.ReadByte();
                    Data_8 = br.ReadByte();
                    Data_9 = br.ReadByte();
                }

                public UnknownByteData2()
                {
                    Data_0 = 0xFF;
                    Data_1 = 0xFF;
                    Data_2 = 0xFF;
                    Data_3 = 0xFF;
                    Data_4 = 0xFF;
                    Data_5 = 0xFF;
                    Data_6 = 0xFF;
                    Data_7 = 0xFF;
                    Data_8 = 0xFF;
                    Data_9 = 0xFF;
                }
            }

            public void ReadLookupTable(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);

                ResourceID = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                UnknownData0 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                ID = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                UnknownByteData0 = br.ReadByte();
                DataTypeID = br.ReadByte();

                UnknownData1 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                UnknownData2 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                Unknown_ByteData.Read_UnknownByteData(br);
                UnknownData5 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                Unknown_ByteData2.Read_UnknownByteData2(br);
            }

            public LookupTable()
            {
                ResourceID = 0;
                UnknownData0 = 0;
                ID = 0;
                UnknownByteData0 = 0x00;
                DataTypeID = 0x00;

                UnknownData1 = 0;
                UnknownData2 = BitConverter.ToInt16(new byte[] { 0xFF, 0xFF }, 0);
                UnknownData3 = BitConverter.ToInt32(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, 0);
                UnknownData4 = BitConverter.ToInt32(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, 0);
                Unknown_ByteData = new UnknownByteData();
                UnknownData5 = BitConverter.ToInt16(new byte[] { 0xFF, 0xFF }, 0);
                Unknown_ByteData2 = new UnknownByteData2();
            }

            public override string ToString()
            {
                return "LokupTable";
            }
        }

        public byte[] UnknownBytes0 { get; set; }
        public byte[] UnknownBytes1 { get; set; }
        public byte[] UnknownBytes2 { get; set; }
        public byte[] UnknownBytes3 { get; set; }

        public void ReadSTRG(BinaryReader br, byte[] BOM)
        {
            STRG_Header = br.ReadChars(4);
            if (new string(STRG_Header) != "STRG") throw new Exception("STRG chunk doesn't match.");

            EndianConvert endianConvert = new EndianConvert(BOM);
            STRGPartitionSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            long LookUpTableOffsetPos = br.BaseStream.Position; //Save TablePos
            StringOffsetTableType = (BCSARHelper.Enum.STRGTableType)BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            long StringOffsetPos = br.BaseStream.Position; //Save StringDataPos
            StringOffsetTable_Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            StringTableLookupType = (BCSARHelper.Enum.STRGTableType)BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            StringTableLookupType_Offset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            long StringTableLoopOffset = br.BaseStream.Position;

            FileNameCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            if (StringOffsetTable_Offset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Position = StringOffsetPos;
                br.BaseStream.Seek(StringOffsetTable_Offset, SeekOrigin.Current);

                for (int i = 0; i < FileNameCount; i++)
                {
                    StringTable stringOffsetTable = new StringTable();
                    stringOffsetTable.ReadStringTable(br, BOM, StringTableLoopOffset);
                    StringTableList.Add(stringOffsetTable);
                }

                br.BaseStream.Position = Pos;
            }

            if (StringTableLookupType_Offset != 0)
            {
                //long Pos = br.BaseStream.Position;

                br.BaseStream.Position = LookUpTableOffsetPos;
                br.BaseStream.Seek(StringTableLookupType_Offset, SeekOrigin.Current);

                for (int i = 0; i < FileNameCount; i++)
                {
                    LookupTable lookupTable = new LookupTable();
                    lookupTable.ReadLookupTable(br, BOM);
                    LookupTableList.Add(lookupTable);
                }

                //br.BaseStream.Position = Pos;
            }

            UnknownBytes0 = endianConvert.Convert(br.ReadBytes(4));
            UnknownBytes1 = endianConvert.Convert(br.ReadBytes(4));
            UnknownBytes2 = endianConvert.Convert(br.ReadBytes(4));
            UnknownBytes3 = endianConvert.Convert(br.ReadBytes(4));
        }

        public STRG()
        {
            STRG_Header = "STRG".ToCharArray();
            STRGPartitionSize = 0;
            StringOffsetTableType = BCSARHelper.Enum.STRGTableType.NotSet;
            StringOffsetTable_Offset = 0;
            StringTableLookupType = BCSARHelper.Enum.STRGTableType.NotSet;
            StringTableLookupType_Offset = 0;
            FileNameCount = 0;
            StringTableList = new List<StringTable>();
            LookupTableList = new List<LookupTable>();
        }

        public override string ToString()
        {
            return "STRG";
        }
    }
}
