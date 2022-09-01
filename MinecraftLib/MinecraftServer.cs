using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using MinecraftLib.Packets;

namespace MinecraftLib
{
    public class MinecraftServer
    {
        public String IP { get; set; }
        public int Port { get; set; }
        public String MotD { get; set; }
        public int ConnectedPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int PingTime { get; set; }

        public string Hostname
        {
            get
            {
                return IP + ":" + Port.ToString();
            }
            set
            {
                if (value.StartsWith("http://"))
                    value = value.Substring(7);
                if (value.Contains(":"))
                {
                    Port = int.Parse(value.Substring(value.IndexOf(":") + 1));
                    value = value.Remove(value.IndexOf(":"));
                }
                IP = value;
            }
        }

        public MinecraftServer()
        {
            this.MotD = "";
            this.ConnectedPlayers = 0;
            this.MaxPlayers = 20;
            this.Port = 25565;
            this.IP = "127.0.0.1";
        }

        public static MinecraftServer GetServer(String HostName)
        {
            MinecraftServer server = new MinecraftServer();
            int port = 25565;

            if (HostName.StartsWith("http://"))
                HostName = HostName.Substring(7);
            if (HostName.Contains(":"))
            {
                port = int.Parse(HostName.Substring(HostName.IndexOf(":") + 1));
                HostName = HostName.Remove(HostName.IndexOf(":"));
            }
            server.Port = port;
            server.IP = HostName;

            byte[] response;
            DateTime startTime = DateTime.Now;
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                socket.Connect(HostName, port);
                socket.Send(new byte[] { (byte)PacketType.ServerListPing });
                response = new byte[socket.ReceiveBufferSize];
                socket.Receive(response);
                socket.Disconnect(false);
            }
            catch { return server; }
            server.PingTime = (int)(DateTime.Now - startTime).TotalMilliseconds;

            if (response[0] != (byte)PacketType.Disconnect)
                return server;

            // Read out MotD
            StringBuilder sb = new StringBuilder();
            int i;
            for (i = 4; i < response.Length; i += 2)
            {
                if (response[i] == 0xA7)
                    break;
                sb.Append((char)response[i]);
            }
            server.MotD = sb.ToString();
            // Read out number of players
            sb = new StringBuilder();
            i += 2;
            for (; i < response.Length; i += 2)
            {
                if (response[i] == 0xA7)
                    break;
                sb.Append((char)response[i]);
            }
            server.ConnectedPlayers = int.Parse(sb.ToString());
            // Read out max players
            sb = new StringBuilder();
            i += 2;
            for (; i < response.Length; i += 2)
            {
                if (response[i] == 0x00)
                    break;
                sb.Append((char)response[i]);
            }
            server.MaxPlayers = int.Parse(sb.ToString());

            return server;
        }
    }
}
