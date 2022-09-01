using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MinecraftLib;
using MinecraftLib.Packets;
using MinecraftLib.Blocks;

using System.Drawing;

namespace Tester
{
    class Program
    {
        static MinecraftClient client = null;
        static Dictionary<short, Color> blocks = new Dictionary<short, Color>()
            {
                {1, Color.Gray},
                {2, Color.Green},
                {3, Color.Brown},
                {8, Color.Blue},
                {9, Color.Blue},
                {10, Color.Orange},
                {11, Color.Orange},
                {12, Color.Tan},
                {18, Color.DarkGreen},
                {31, Color.DarkOliveGreen},
                {37, Color.Yellow},
                {38, Color.Red},
                {39, Color.Brown},
                {40, Color.Red},
                {78, Color.White}
            };

        static void Main(string[] args)
        {
            client = new MinecraftClient();
            MinecraftServer server = MinecraftServer.GetServer("192.168.1.109:25565");
            if (client.ConnectTo(server))
            {
                client.LogIn("bob", "");
                if (client.socket.Connected)
                {
                    Console.ReadLine();
                    client.Disconnect();
                    Console.WriteLine("Saving height map...");
                    Bitmap bitmap = new Bitmap((client.chunkXMax - client.chunkXMin + 1) * 16, (client.chunkZMax - client.chunkZMin + 1) * 16);
                    short blockId = 0;
                    Color col;
                    int y = 0;

                    for (int x = client.chunkXMin; x <= client.chunkXMax; x++)
                    {
                        for (int z = client.chunkZMin; z <= client.chunkZMax; z++)
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    y = GetHighestPoint(x, z, i, j, out blockId);
                                    if (y >= 0)
                                    {
                                        if (blocks.TryGetValue(blockId, out col))
                                            bitmap.SetPixel((x - client.chunkXMin) * 16 + i, (z - client.chunkZMin) * 16 + j,
                                                col);
                                        else
                                            bitmap.SetPixel((x - client.chunkXMin) * 16 + i, (z - client.chunkZMin) * 16 + j,
                                                Color.Black);
                                        //bitmap.SetPixel((x - client.chunkXMin) * 16 + i, (z - client.chunkZMin) * 16 + j,
                                        //    Color.FromArgb((int)(y / 128.0 * 255.0), (int)(y / 128.0 * 255.0), (int)(y / 128.0 * 255.0)));
                                    }
                                    else
                                        bitmap.SetPixel((x - client.chunkXMin) * 16 + i, (z - client.chunkZMin) * 16 + j,
                                                Color.Magenta);
                                }
                            }
                        }
                    }
                    bitmap.Save("colormap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    Console.WriteLine("height map saved!");
                }
                else
                    Console.WriteLine("Disconnected!");
            }
            else
                Console.WriteLine("Unabled to connect to server!");
            Console.ReadLine();
        }

        static int GetHighestPoint(int x, int z, int i, int j, out short id)
        {
            Vector3 chunkLocation = new Vector3(x, 7, z);
            Chunk c = null;
            int y = 0;
            Block b = null;
            while (chunkLocation.Y >= 0)
            {
                c = client.World.GetChunk(chunkLocation);
                if (c == null)
                    break;
                if (!c.IsAir)
                {
                    for (y = 15; y >= 0; y--)
                    {
                        b = c.GetBlock(new Vector3(i, y, j));
                        if (b.Id != 0)
                            break;
                    }
                    if (y >= 0)
                    {
                        id = b.Id;
                        return y + (int)chunkLocation.Y * 16;
                    }
                }
                chunkLocation.Y -= 1;
            }
            id = -1;
            return -1;
        }
    }
}
