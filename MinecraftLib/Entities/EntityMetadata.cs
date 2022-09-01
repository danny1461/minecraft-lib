using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using MinecraftLib.Packets;

namespace MinecraftLib.Entities
{
    public enum MetadataValueType : byte
    {
        Byte = 0x00,
        Short = 0x01,
        Int = 0x02,
        Float = 0x03,
        String16 = 0x04,
        ShortByteShort = 0x05,
        IntIntInt = 0x06
    }
    public class MetadataValue
    {
        public MetadataValueType Type;
        public Dictionary<String, object> data = new Dictionary<string,object>();

        public object this[String index]
        {
            get { return data[index]; }
            set
            {
                if (data.ContainsKey(index))
                    data[index] = value;
                else
                    data.Add(index, value);
                if (value is byte)
                    Type = MetadataValueType.Byte;
                else if (value is short)
                    Type = MetadataValueType.Short;
                else if (value is int)
                    Type = MetadataValueType.Int;
                else if (value is float)
                    Type = MetadataValueType.Float;
                else if (value is string)
                    Type = MetadataValueType.String16;
            }
        }
    }
    public class EntityMetadata
    {
        public Dictionary<int, MetadataValue> data = new Dictionary<int, MetadataValue>();

        public MetadataValue this[int index]
        {
            get { return data[index]; }
            set
            {
                if (data.ContainsKey(index))
                    data[index] = value;
                else
                    data.Add(index, value);
            }
        }
        public void Read(NetworkStream stream)
        {
            byte x = (byte)stream.ReadByte();
            byte index, type;
            while (x != 127)
            {
                index = (byte)(x & 0x1F);
                type = (byte)(x >> 5);
                MetadataValue value = new MetadataValue();
                if (type == 0)
                    value["default"] = (byte)stream.ReadByte();
                else if (type == 1)
                    value["default"] = StreamHelper.ReadShort(stream);
                else if (type == 2)
                    value["default"] = StreamHelper.ReadInt(stream);
                else if (type == 3)
                    value["default"] = StreamHelper.ReadFloat(stream);
                else if (type == 4)
                    value["default"] = StreamHelper.ReadString(stream);
                else if (type == 5)
                {
                    value["id"] = StreamHelper.ReadShort(stream);
                    value["count"] = (byte)stream.ReadByte();
                    value["damage"] = StreamHelper.ReadShort(stream);
                    value.Type = MetadataValueType.ShortByteShort;
                }
                else if (type == 6)
                {
                    value["val1"] = StreamHelper.ReadInt(stream);
                    value["val2"] = StreamHelper.ReadInt(stream);
                    value["val3"] = StreamHelper.ReadInt(stream);
                    value.Type = MetadataValueType.IntIntInt;
                }
                this[index] = value;
                x = (byte)stream.ReadByte();
            }
        }
        public void Merge(EntityMetadata Metadata)
        {
            foreach (int x in Metadata.data.Keys)
                this[x] = Metadata[x];
        }
    }
}
