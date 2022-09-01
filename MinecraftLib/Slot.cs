using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using MinecraftLib.Packets;
using MinecraftLib.Items;

namespace MinecraftLib
{
    public class Slot
    {
        public short ItemId;
        public byte ItemCount;
        public short Damage;
        public short DataSize;
        public NBT Data;

        public Slot()
        {
            ItemId = 0;
            ItemCount = 1;
            Damage = 0;
            DataSize = 0;
            Data = new NBT();
        }
        public void Read(Stream s)
        {
            ItemId = StreamHelper.ReadShort(s);
            if (ItemId == -1)
                return;
            ItemCount = (byte)s.ReadByte();
            Damage = StreamHelper.ReadShort(s);
            if (EnchantableItems.CanEnchant(ItemId))
            {
                // We could probably just give gZip to the NBTData reader
                DataSize = StreamHelper.ReadShort(s);
                byte[] buffer = new byte[DataSize];
                GZipInputStream gZip = new GZipInputStream(s);
                gZip.Read(buffer, 0, DataSize);
                gZip.Dispose();
                MemoryStream memStr = new MemoryStream(buffer);
                memStr.Position = 0;
                Data = new NBT();
                Data.Read(memStr);
                memStr.Dispose();
            }
        }
        public void Write(Stream s)
        {
            StreamHelper.Write(s, ItemId);
            if (ItemId == -1)
                return;
            StreamHelper.Write(s, ItemCount);
            StreamHelper.Write(s, Damage);
            if (EnchantableItems.CanEnchant(ItemId))
            {
                // Forgot to write datasize
                GZipOutputStream gZip = new GZipOutputStream(s);
                Data.Write(gZip);
                gZip.Dispose();
            }
        }
    }
}
