using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using MinecraftLib.Packets;

namespace MinecraftLib
{
    public class NBTTag
    {
        public enum Tags : byte
        {
            END = 0,
            BYTE = 1,
            SHORT = 2,
            INT = 3,
            LONG = 4,
            FLOAT = 5,
            DOUBLE = 6,
            BYTE_ARRAY = 7,
            STRING = 8,
            LIST = 9,
            COMPOUND = 10,
            INT_ARRAY = 11
        }

        public Tags TagId;
        public String Description = null;
        public object Value = null;
    }
    public class NBTList : NBTTag
    {
        public Tags ValueIds;
        public List<NBTTag> Values = new List<NBTTag>();

        public NBTList()
        {
            TagId = Tags.LIST;
        }
        public NBTTag this[int i]
        {
            get
            {
                return Values[i];
            }
            set
            {
                Values[i] = value;
            }
        }
    }
    public class NBTCompound : NBTTag
    {
        public List<NBTTag> Values = new List<NBTTag>();

        public NBTCompound()
        {
            TagId = Tags.COMPOUND;
        }
        public NBTTag this[String name]
        {
            get
            {
                foreach (NBTTag tag in Values)
                    if (tag.Description == name)
                        return tag;
                return null;
            }
            set
            {
                for (int x = 0; x < Values.Count; x++)
                    if (Values[x].Description == name)
                        Values[x] = value;
            }
        }
    }
    public class NBT
    {
        public NBTCompound parent;

        public void Read(String FilePath)
        {
            byte[] buffer = new byte[4096];
            FileStream fs = new FileStream(FilePath, FileMode.Open);
            bool success = false;
            try
            {
                Read(fs);
                success = true;
            }
            catch { }
            if (!success)
            {
                fs.Position = 0;
                try
                {
                    using (GZipInputStream gZip = new GZipInputStream(fs))
                    {
                        using (MemoryStream memStr = new MemoryStream())
                        {
                            StreamUtils.Copy(gZip, memStr, buffer);
                            memStr.Position = 0;
                            Read(memStr);
                        }
                    }
                }
                catch
                {
                    throw new Exception("This file does not appear to be in NBT format!");
                }
            }
        }
        public void Read(Stream stream)
        {
            parent = new NBTCompound();
            parent.TagId = (NBTTag.Tags)StreamHelper.ReadByte(stream);
            parent.Description = StreamHelper.ReadAsciiString(stream);
            ReadCompound(stream, parent);
        }
        private void ReadCompound(Stream stream, NBTCompound parent)
        {
            NBTTag.Tags childTag = 0;
            while (true)
            {
                childTag = (NBTTag.Tags)StreamHelper.ReadByte(stream);
                if ((byte)childTag > 11)
                    throw new Exception();
                else if (childTag == NBTTag.Tags.END)
                    break;
                else if (childTag == NBTTag.Tags.LIST)
                {
                    NBTList listTag = new NBTList();
                    listTag.Description = StreamHelper.ReadAsciiString(stream); ;
                    listTag.ValueIds = (NBTTag.Tags)stream.ReadByte();
                    ReadList(stream, listTag, StreamHelper.ReadInt(stream));
                    parent.Values.Add(listTag);
                }
                else if (childTag == NBTTag.Tags.COMPOUND)
                {
                    NBTCompound compTag = new NBTCompound();
                    compTag.Description = StreamHelper.ReadAsciiString(stream); ;
                    ReadCompound(stream, compTag);
                    parent.Values.Add(compTag);
                }
                else
                {
                    NBTTag tag = new NBTTag();
                    tag.TagId = childTag;
                    tag.Description = StreamHelper.ReadAsciiString(stream);
                    ReadTag(stream, tag);
                    parent.Values.Add(tag);
                }
            }
            parent.Values.TrimExcess();
        }
        private void ReadList(Stream stream, NBTList parent, int size)
        {
            for (int x = 0; x < size; x++)
            {
                if (parent.ValueIds == NBTTag.Tags.COMPOUND)
                {
                    NBTCompound compTag = new NBTCompound();
                    ReadCompound(stream, compTag);
                    parent.Values.Add(compTag);
                }
                else if (parent.ValueIds == NBTTag.Tags.LIST)
                {
                    NBTList listTag = new NBTList();
                    ReadList(stream, listTag, StreamHelper.ReadInt(stream));
                }
                else
                {
                    NBTTag tag = new NBTTag();
                    tag.TagId = parent.ValueIds;
                    ReadTag(stream, tag);
                    parent.Values.Add(tag);
                }
            }
            parent.Values.TrimExcess();
        }
        private void ReadTag(Stream stream, NBTTag tag)
        {
            switch (tag.TagId)
            {
                case NBTTag.Tags.BYTE:
                    tag.Value = StreamHelper.ReadSByte(stream);
                    break;
                case NBTTag.Tags.SHORT:
                    tag.Value = StreamHelper.ReadShort(stream);
                    break;
                case NBTTag.Tags.INT:
                    tag.Value = StreamHelper.ReadInt(stream);
                    break;
                case NBTTag.Tags.LONG:
                    tag.Value = StreamHelper.ReadLong(stream);
                    break;
                case NBTTag.Tags.FLOAT:
                    tag.Value = StreamHelper.ReadFloat(stream);
                    break;
                case NBTTag.Tags.DOUBLE:
                    tag.Value = StreamHelper.ReadDouble(stream);
                    break;
                case NBTTag.Tags.BYTE_ARRAY:
                    tag.Value = StreamHelper.ReadSignedBytes(stream, StreamHelper.ReadInt(stream));
                    break;
                case NBTTag.Tags.STRING:
                    tag.Value = StreamHelper.ReadAsciiString(stream);
                    break;
                case NBTTag.Tags.INT_ARRAY:
                    tag.Value = StreamHelper.ReadInts(stream, StreamHelper.ReadInt(stream));
                    break;
            }
        }

        public void Write(String FilePath, bool compress)
        {
            FileStream stream = new FileStream(FilePath, FileMode.Create);
            if (compress)
            {
                GZipOutputStream gZip = new GZipOutputStream(stream);
                Write(gZip);
            }
            else
                Write(stream);

            stream.Flush();
            stream.Close();
            stream.Dispose();
        }
        public void Write(Stream stream)
        {
            WriteCompound(stream, parent, true);
        }
        private void WriteCompound(Stream stream, NBTCompound parent, bool header)
        {
            if (header)
            {
                StreamHelper.Write(stream, (byte)NBTTag.Tags.COMPOUND);
                StreamHelper.Write(stream, parent.Description);
            }
            foreach (NBTTag tag in parent.Values)
            {
                if (tag is NBTCompound)
                {
                    WriteCompound(stream, (NBTCompound)tag, true);
                }
                else if (tag is NBTList)
                {
                    WriteList(stream, (NBTList)tag, true);
                }
                else
                    WriteTag(stream, tag, true);
            }
            StreamHelper.Write(stream, (byte)NBTTag.Tags.END);
        }
        private void WriteList(Stream stream, NBTList parent, bool header)
        {
            if (header)
            {
                StreamHelper.Write(stream, (byte)NBTTag.Tags.LIST);
                StreamHelper.Write(stream, parent.Description);
            }
            StreamHelper.Write(stream, (byte)parent.ValueIds);
            StreamHelper.Write(stream, parent.Values.Count);
            foreach (NBTTag tag in parent.Values)
            {
                if (tag is NBTCompound)
                {
                    WriteCompound(stream, (NBTCompound)tag, false);
                }
                else if (tag is NBTList)
                {
                    WriteList(stream, (NBTList)tag, false);
                }
                else
                    WriteTag(stream, tag, false);
            }
        }
        private void WriteTag(Stream stream, NBTTag tag, bool header)
        {
            if (header)
            {
                StreamHelper.Write(stream, (byte)tag.TagId);
                StreamHelper.Write(stream, tag.Description);
            }
            switch (tag.TagId)
            {
                case NBTTag.Tags.BYTE:
                    StreamHelper.Write(stream, (sbyte)tag.Value);
                    break;
                case NBTTag.Tags.SHORT:
                    StreamHelper.Write(stream, (short)tag.Value);
                    break;
                case NBTTag.Tags.INT:
                    StreamHelper.Write(stream, (int)tag.Value);
                    break;
                case NBTTag.Tags.LONG:
                    StreamHelper.Write(stream, (long)tag.Value);
                    break;
                case NBTTag.Tags.FLOAT:
                    StreamHelper.Write(stream, (float)tag.Value);
                    break;
                case NBTTag.Tags.DOUBLE:
                    StreamHelper.Write(stream, (double)tag.Value);
                    break;
                case NBTTag.Tags.BYTE_ARRAY:
                    sbyte[] bytes = (sbyte[])tag.Value;
                    StreamHelper.Write(stream, bytes.Length);
                    StreamHelper.Write(stream, bytes);
                    break;
                case NBTTag.Tags.STRING:
                    StreamHelper.Write(stream, (String)tag.Value);
                    break;
                case NBTTag.Tags.INT_ARRAY:
                    int[] ints = (int[])tag.Value;
                    StreamHelper.Write(stream, ints.Length);
                    StreamHelper.Write(stream, ints);
                    break;
            }
        }
    }
}
