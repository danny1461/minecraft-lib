using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib
{
    public class NibbleArray
    {
        public byte[] byteArray;

        public byte this[int i]
        {
            get
            {
                int index = i / 2;
                if (i % 2 == 0)
                    return (byte)(byteArray[index] >> 4);
                else
                    return (byte)(byteArray[index] & 0xF0);
            }
            set
            {
                int index = i / 2;
                if (i % 2 == 0)
                    byteArray[index] = (byte)((byteArray[index] & 0x0F) | (value << 4));
                else
                    byteArray[index] = (byte)((byteArray[index] & 0x0F) | value);
            }
        }

        public NibbleArray(int Size)
        {
            byteArray = new byte[Size / 2];
        }
    }
}
