using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSARLibrary
{
    public class BCSARHelper
    {
        public class Enum
        {
            public enum PartitionReferenceID
            {
                //STRG:0x2000, INFO:0x2001, FILE:2002
                STRG = 8192,
                INFO = 8193,
                FILE = 8194,
                NotSet
            }

            public enum STRGTableType
            {
                OffsetTable = 9216,
                LookupTable = 9217,
                NotSet
            }

            public enum ResourceID
            {
                Sound = 1,
                SoundList = 2,
                SoundBank = 3,
                SoundPlayerName = 4,
                SongGroup = 6,
                Unknown,
                Unused = 255
            }

            public enum INFOReferenceID
            {
                AudioTable = 8448, //0x2100
                SetTable = 8452, //0x2104
                BankTable = 8449, //0x2101
                WAVArchiveTable = 8451, //0x2103
                GroupTable = 8453, //0x2105
                PlayerTable = 8450, //0x2102
                FILETable = 8454, //0x2106
                FileInfoTable = 8459 //0x220B
            }

            public enum INFOTableID
            {
                AudioTable = 8704, //0x2200
                SetTable = 8708, //0x2204
                BankTable = 8710, //0x2206
                WAVArchiveTable = 8711, //0x2207
                GroupTable = 8712, //0x2208
                PlayerTable = 8713, //0x2209
                FILETable = 8714, //0x220A
                FileInfoTable,
                Unknown
            }

            public enum INFODataBlockID
            {
                ID_2201 = 8705,
                ID_2202 = 8706, //Sound WAVE Entry
                ID_2203 = 8707,
                //ID_2204 = 8708,
                ID_2205 = 8709, //SetData Entry
                //ID_0100 = 256,
                //ID_2208 = 8712,
                ID_220D = 8717,
                Null = 65535
            }
        }

        public class ReadByteLine
        {
            public List<byte> charByteList { get; set; }

            public ReadByteLine(List<byte> Input)
            {
                charByteList = Input;
            }

            public ReadByteLine(List<char> Input)
            {
                charByteList = Input.Select(x => (byte)x).ToArray().ToList();
            }

            public void ReadByte(BinaryReader br, byte Split = 0x00)
            {
                //var br = br.BaseStream;
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    byte PickStr = br.ReadByte();
                    charByteList.Add(PickStr);
                    if (PickStr == Split)
                    {
                        break;
                    }
                }
            }

            public void ReadMultiByte(BinaryReader br, byte Split = 0x00)
            {
                //var br = br.BaseStream;
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    byte[] PickStr = br.ReadBytes(2);
                    charByteList.Add(PickStr[0]);
                    charByteList.Add(PickStr[1]);
                    if (PickStr[0] == Split)
                    {
                        break;
                    }
                }
            }

            public void ReadByte(BinaryReader br, char Split = '\0')
            {
                //var br = br.BaseStream;
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    byte PickStr = br.ReadByte();
                    charByteList.Add(PickStr);
                    if (PickStr == Split)
                    {
                        break;
                    }
                }
            }

            public void WriteByte(BinaryWriter bw)
            {
                bw.Write(ConvertToCharArray());
            }

            public char[] ConvertToCharArray()
            {
                return charByteList.Select(x => (char)x).ToArray();
            }

            public int GetLength()
            {
                return charByteList.ToArray().Length;
            }
        }
    }
}
