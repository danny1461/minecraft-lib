using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace MinecraftLib.Packets
{
    public class StreamHelper
    {
        private byte[] Data;
        private byte ReadBits = 0;

        public StreamHelper(Stream s, int bytesToRead)
        {
            Data = new byte[bytesToRead];
            s.Read(Data, 0, bytesToRead);
        }
        public byte ReadBitsToByte(int bits)
        {
            if (bits <= 0 || bits > 8)
                throw new ArgumentOutOfRangeException("bits", "Should be 1-8.");
            if (ReadBits + bits > Data.Length * 8)
                throw new IndexOutOfRangeException("You are attempting to read more bits than in byte selection.");
            byte result = 0;
            int arrayStart = 0, bitsLeftInByte = 0, mask = 0, bitsLeft = bits;
            while (bits > 0)
            {
                result = (byte)(result << bitsLeft);
                arrayStart = ReadBits / 8;
                bitsLeftInByte = 8 - (ReadBits % 8);
                mask = 0;
                for (int i = bitsLeftInByte - 1; i >= 0 && i >= (bitsLeftInByte - bits); i--)
                {
                    mask = mask | (int)Math.Pow(2, i);
                    ReadBits++;
                    bitsLeft--;
                }
                result = (byte)(result | ((mask & Data[arrayStart]) >> (bitsLeftInByte - bits + bitsLeft)));
                bits = bitsLeft;
            }
            return result;
        }
        public short ReadBitsToShort(int bits)
        {
            if (bits <= 0 || bits > 16)
                throw new ArgumentOutOfRangeException("bits", "Should be 1-16.");
            if (ReadBits + bits > Data.Length * 8)
                throw new IndexOutOfRangeException("You are attempting to read more bits than in byte selection.");
            short result = 0;
            while (bits > 8)
            {
                bits -= 8;
                result = (short)((result << 8) | ReadBitsToByte(8));
            }
            return (short)((result << bits) | ReadBitsToByte(bits));
        }
        public int ReadBitsToInt(int bits)
        {
            if (bits <= 0 || bits > 32)
                throw new ArgumentOutOfRangeException("bits", "Should be 1-32.");
            if (ReadBits + bits > Data.Length * 8)
                throw new IndexOutOfRangeException("You are attempting to read more bits than in byte selection.");
            int result = 0;
            while (bits > 8)
            {
                bits -= 8;
                result = (result << 8) | ReadBitsToByte(8);
            }
            return (result << bits) | ReadBitsToByte(bits);
        }

        public static byte ReadByte(Stream s)
        {
            return (byte)s.ReadByte();
        }
        public static sbyte ReadSByte(Stream s)
        {
            return unchecked((sbyte)s.ReadByte());
        }
        public static int ReadInt(Stream s)
        {
            return IPAddress.HostToNetworkOrder((int)Read(s, 4));
        }
        public static int[] ReadInts(Stream s, int count)
        {
            int[] result = new int[count];
            for (int x = 0; x < count; x++)
                result[x] = ReadInt(s);
            return result;
        }
        public static short ReadShort(Stream s)
        {
            return IPAddress.HostToNetworkOrder((short)Read(s, 2));
        }
        public static long ReadLong(Stream s)
        {
            return IPAddress.HostToNetworkOrder((long)Read(s, 8));
        }
        public static double ReadDouble(Stream s)
        {
            byte[] r = new byte[8];
            for (int i = 7; i >= 0; i--)
                r[i] = ReadByte(s);
            return BitConverter.ToDouble(r, 0);
        }
        public unsafe static float ReadFloat(Stream s)
        {
            int i = ReadInt(s);
            return *(float*)&i;
        }
        public static bool ReadBoolean(Stream s)
        {
            return s.ReadByte() == 1;
        }
        public static byte[] ReadBytes(Stream s, int count)
        {
            byte[] result = new byte[count];
            s.Read(result, 0, count);
            return result;
        }
        public static sbyte[] ReadSignedBytes(Stream s, int count)
        {
            sbyte[] result = new sbyte[count];
            for (int x = 0; x < count; x++)
                result[x] = unchecked((sbyte)s.ReadByte());
            return result;
        }
        public static String ReadString(Stream s)
        {
            short len = ReadShort(s);

            byte[] b = new byte[len * 2];
            for (int i = 0; i < len * 2; i++)
                b[i] = (byte)s.ReadByte();
            return ASCIIEncoding.BigEndianUnicode.GetString(b);
        }
        public static String ReadAsciiString(Stream s)
        {
            short len = ReadShort(s);

            byte[] b = new byte[len];
            s.Read(b, 0, len);
            return ASCIIEncoding.Default.GetString(b);
        }
        public static Object Read(Stream s, int num)
        {
            byte[] b = new byte[num];
            s.Read(b, 0, num);
            switch (num)
            {
                case 4:
                    return BitConverter.ToInt32(b, 0);
                case 8:
                    return BitConverter.ToInt64(b, 0);
                case 2:
                    return BitConverter.ToInt16(b, 0);
                default:
                    return 0;
            }
        }

        public static void Write(Stream s, byte value)
        {
            s.WriteByte(value);
        }
        public static void Write(Stream s, sbyte value)
        {
            s.WriteByte((byte)value);
        }
        public static void Write(Stream s, int value)
        {
            byte[] a = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            s.Write(a, 0, a.Length);
        }
        public static void Write(Stream s, int[] value)
        {
            for (int x = 0; x < value.Length; x++)
                Write(s, value[x]);
        }
        public static void Write(Stream s, short value)
        {
            byte[] a = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            s.Write(a, 0, a.Length);
        }
        public static void Write(Stream s, long value)
        {
            byte[] a = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
            s.Write(a, 0, a.Length);
        }
        public static unsafe void Write(Stream s, double value)
        {
            Write(s, *(long*)&value);
        }
        public static unsafe void Write(Stream s, float value)
        {
            Write(s, *(int*)&value);
        }
        public static void Write(Stream s, bool value)
        {
            s.WriteByte((byte)(value ? 1 : 0));
        }
        public static void Write(Stream s, byte[] value)
        {
            s.Write(value, 0, value.Length);
        }
        public static void Write(Stream s, sbyte[] value)
        {
            for (int x = 0; x < value.Length; x++)
                s.WriteByte(unchecked((byte)value[x]));
        }
        public static void Write(Stream s, String value)
        {
            Write(s, (short)value.Length);
            Write(s, ASCIIEncoding.BigEndianUnicode.GetBytes(value));
        }
    }
}
