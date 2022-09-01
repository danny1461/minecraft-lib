using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using MinecraftLib.Packets;
using MinecraftLib.Entities;
using MinecraftLib.Blocks;

namespace MinecraftLib
{
    public class MinecraftClient
    {
        // Minecraft 1.2.5 protocol is 29
        public const int MINECRAFT_PROTOCOL = 29;

        // Properties
        public MinecraftServer Server { get; set; }
        public PlayerEntity Player { get; set; }
        public World World { get; set; }

        // Network vars
        public Socket socket;
        private NetworkStream stream;
        private Queue<Packet> sendPacketQueue;
        private object lockObject = new object();

        // Server Tick vars
        private Thread clientTick;
        private DateTime lastTick;
        private double totalTickingTime = 0;
        private int tickCount = 0;
        public int TPS = 0;
        public int ThreadSleepTime = 50;

        // Event Handlers
        public TickHandler OnTick;
        public ChatHandler OnChat;
        public HealthHandler OnHealthChange;
        public HungerHandler OnHungerChange;

        // Methods
        public bool ConnectTo(MinecraftServer server)
        {
            this.Server = server;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.ReceiveBufferSize = 8192;
            socket.SendBufferSize = 4096;
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(this.Server.IP), this.Server.Port);
                socket.Connect(ipEndPoint);
            }
            catch
            {
                return false;
            }
            stream = new NetworkStream(socket);
            sendPacketQueue = new Queue<Packet>();
            return true;
        }
        public void LogIn(String Username, String Password)
        {
            if (!socket.Connected)
                throw new Exception("Not connected to any server!");

            HandshakePacket handshake = new HandshakePacket();
            handshake.UsernameAndHostOrHash = Username + ";" + Server.IP + ":" + Server.Port;
            SendPacketNow(handshake);

            stream.ReadByte();
            handshake.Read(stream);
            if (handshake.UsernameAndHostOrHash != "-")
            {
                // Server is in online mode - Not implemented
                Disconnect();
                throw new FailedAuthentication();
            }

            LoginRequestPacket loginRequest = new LoginRequestPacket();
            loginRequest.ProtocolOrEntityId = MINECRAFT_PROTOCOL;
            loginRequest.Username = Username;
            loginRequest.LevelType = "";
            SendPacketNow(loginRequest);

            PacketType resp = (PacketType)stream.ReadByte();
            if (resp == PacketType.Disconnect)
            {
                // Rejected by the server. Maybe it's full...
                Disconnect();
                return;
            }
            loginRequest.Read(stream);
            Player = new PlayerEntity();
            Player.Dimension = (Dimension)loginRequest.Dimension;
            Player.Id = loginRequest.ProtocolOrEntityId;
            World = new MinecraftLib.World();
            World.LevelType = loginRequest.LevelType;
            World.Difficulty = (Difficulty)loginRequest.Difficulty;

            // Connected - Start up listening thread
            lastTick = DateTime.Now;
            clientTick = new Thread(new ThreadStart(this.Tick));
            clientTick.Start();
        }
        public void Disconnect()
        {
            lock (lockObject)
            {
                DisconnectPacket disconnect = new DisconnectPacket();
                disconnect.Reason = "I quit!";
                SendPacketNow(disconnect);
                Thread.Sleep(100);
                socket.Disconnect(false);
                stream.Dispose();
                stream = null;
            }
        }

        // Private Methods
        private void Tick()
        {
            TimeSpan gameTime;
            while (stream != null)
            {
                gameTime = DateTime.Now - lastTick;
                lastTick = DateTime.Now;

                // Should we close the loop?
                if (!socket.Connected)
                    break;

                // OnTick
                if (OnTick != null)
                    OnTick(gameTime);

                lock (lockObject)
                {
                    // Handle packets
                    //stream.Flush();
                    while (stream != null && stream.DataAvailable)
                    {
                        PacketType packetType = (PacketType)stream.ReadByte();
                        HandlePacket(packetType);
                    }

                    // Do Clienty Things
                    DoStuff(gameTime);

                    if (stream != null)
                        ProcessSendPacketQueue();
                }

                // Calculate Ticks Per Second
                totalTickingTime += gameTime.TotalMilliseconds;
                tickCount++;
                if (totalTickingTime >= 1000)
                {
                    if (tickCount < 20)
                        ThreadSleepTime -= 1;
                    else if (tickCount > 20)
                        ThreadSleepTime += 1;
                    TPS = tickCount;
                    tickCount = 0;
                    totalTickingTime = 0;
                }

                // Wait a little
                Thread.Sleep(ThreadSleepTime);
            }
        }
        private void DoStuff(TimeSpan gameTime)
        {
            DoPlayerGravity();

            // Send Player Position And Rotation
            SendPlayerPositionRotation();
        }
        private void DoPlayerGravity()
        {
            if (!Player.IsFlying && !Player.OnGround)
            {
                Player.Velocity.Y -= 0.09;
                Player.Velocity.Y = Math.Max(Player.Velocity.Y, -0.9);

                // Check the highest block beneath our feet

                // Check how far away it is

                // If the distance is <= Player.Velocity.Y then we hit this tick
                /* if (distToGround <= Player.Velocity.Y)
                {
                    Player.Location.Y = [TopOfBlockY];
                }
                else
                    Player.Location.Y += Player.Velocity.Y;*/

                // Check if we hit the ground
            }
        }
        private void SendPlayerPositionRotation()
        {
            PlayerPositionRotationPacket packet = new PlayerPositionRotationPacket();
            packet.X = Player.Location.X;
            packet.Y = Player.Location.Y;
            packet.Z = Player.Location.Z;
            packet.Stance = Player.Location.Y + 1.6;
            packet.Yaw = (float)Player.Rotation.X;
            packet.Pitch = (float)Player.Rotation.Y;
            SendPacket(packet);
        }
        
        #region Network Methods
        private void HandlePacket(PacketType packetType)
        {
            switch (packetType)
            {
                case PacketType.KeepAlive:                      HandleKeepAlive(); break;
                case PacketType.ChatMessage:                    HandleChatMessage();  break;
                case PacketType.TimeUpdate:                     HandleTimeUpdate(); break;
                case PacketType.EntityEquipment:                HandleEntityEquipment(); break;
                case PacketType.SpawnPosition:                  HandleSpawnPosition(); break;
                //case PacketType.UseEntity: break;
                case PacketType.UpdateHealth:                   HandleUpdateHealth(); break;
                //case PacketType.Respawn: break;
                //case PacketType.Player: break;
                //case PacketType.PlayerPosition: break;
                //case PacketType.PlayerRotation: break;
                case PacketType.PlayerPositionRotation:         HandlePlayerPositionRotation(); break;
                //case PacketType.PlayerDigging: break;
                //case PacketType.PlayerBlockPlacement: break;
                //case PacketType.HeldItemChange: break;
                //case PacketType.UseBed: break;
                case PacketType.Animation:                      HandleAnimation(); break;
                //case PacketType.EntityAction: break;
                case PacketType.SpawnNamedEntity:               HandleSpawnNamedEntity(); break;
                case PacketType.SpawnDroppedItem:               HandleSpawnDroppedItem(); break;
                //case PacketType.CollectItem: break;
                case PacketType.SpawnObjectVehicle:             HandleSpawnObjectVehicle(); break;
                case PacketType.SpawnMob:                       HandleSpawnMob(); break;
                //case PacketType.SpawnPainting: break;
                //case PacketType.SpawnExperienceOrb: break;
                case PacketType.EntityVelocity:                 HandleEntityVelocity(); break;
                case PacketType.DestroyEntity:                  HandleDestroyEntity(); break;
                case PacketType.Entity:                         HandleEntity(); break;
                case PacketType.EntityRelativeMove:             HandleEntityRelativeMove(); break;
                case PacketType.EntityLook:                     HandleEntityLook(); break;
                case PacketType.EntityLookAndRelativeMove:      HandleEntityLookAndRelativeMove(); break;
                case PacketType.EntityTeleport:                 HandleEntityTeleport(); break;
                case PacketType.EntityHeadLook:                 HandleEntityHeadLook(); break;
                case PacketType.EntityStatus:                   HandleEntityStatus(); break;
                //case PacketType.AttachEntity: break;
                case PacketType.EntityMetadata:                 HandleEntityMetadata(); break;
                case PacketType.EntityEffect:                   HandleEntityEffect(); break;
                case PacketType.RemoveEntityEffect:             HandleRemoveEntityEffect(); break;
                case PacketType.SetExperience:                  HandleSetExperience(); break;
                case PacketType.ChunkAllocation:                HandleChunkAllocation(); break;
                case PacketType.ChunkData:                      HandleChunkData(); break;
                case PacketType.MultiBlockChange:               HandleMultiBlockChange(); break;
                case PacketType.BlockChange:                    HandleBlockChange(); break;
                case PacketType.BlockAction:                    HandleBlockAction(); break;
                //case PacketType.Explosion: break;
                case PacketType.SoundParticleEffect:            HandleSoundParticleEffect(); break;
                case PacketType.ChangeGameState:                HandleChangeGameState(); break;
                //case PacketType.Thunderbolt: break;
                //case PacketType.OpenWindow: break;
                //case PacketType.CloseWindow: break;
                case PacketType.SetSlot:                        HandleSetSlot(); break;
                case PacketType.SetWindowItems:                 HandleSetWindowItems(); break;
                //case PacketType.UpdateWindowProperty: break;
                //case PacketType.ConfirmTransaction: break;
                //case PacketType.CreativeInventoryAction: break;
                //case PacketType.EnchantItem: break;
                //case PacketType.UpdateSign: break;
                //case PacketType.ItemData: break;
                case PacketType.UpdateTileEntry:                HandleUpdateTileEntry(); break;
                //case PacketType.IncrementStatistic: break;
                case PacketType.PlayerListItem:                 HandlePlayerListItem(); break;
                case PacketType.PlayerAbilities:                HandlePlayerAbilities(); break;
                //case PacketType.PluginMessage: break;
                //case PacketType.ServerListPing: break;
                case PacketType.Disconnect:                     HandleDisconnect(); break;
                default:
                    Console.WriteLine("Unhandled packet: " + packetType.ToString());
                    while (stream.DataAvailable)
                        stream.ReadByte();
                    break;
            }
        }
        private void SendPacketNow(Packet packet)
        {
            stream.WriteByte((byte)packet.GetPacketType);
            packet.Write(stream);
        }
        private void SendPacket(Packet packet)
        {
            sendPacketQueue.Enqueue(packet);
        }
        private void ProcessSendPacketQueue()
        {
            Packet packet;
            while (sendPacketQueue.Count > 0)
            {
                packet = sendPacketQueue.Dequeue();
                Console.WriteLine("Sending: " + packet.GetPacketType.ToString());
                stream.WriteByte((byte)packet.GetPacketType);
                packet.Write(stream);
                Thread.Sleep(250);
            }
        }

        private void HandleKeepAlive()
        {
            KeepAlivePacket packet = new KeepAlivePacket();
            packet.Read(stream);

            packet.RandomID = 0;
            SendPacket(packet);
        }
        private void HandleChatMessage()
        {
            ChatMessagePacket packet = new ChatMessagePacket();
            packet.Read(stream);

            if (OnChat != null)
                OnChat(new ChatEventArgs(packet.Message));
        }
        private void HandleTimeUpdate()
        {
            TimeUpdatePacket packet = new TimeUpdatePacket();
            packet.Read(stream);

            World.Time = packet.Time;
        }
        private void HandleEntityEquipment()
        {
            EntityEquipmentPacket packet = new EntityEquipmentPacket();
            packet.Read(stream);

            Entity entity = World.GetEntity(packet.EntityId);
            if (entity is NamedEntity)
            {
                NamedEntity named = (NamedEntity)entity;
                named.Inventory[packet.Slot].ItemId = packet.ItemId;
            }
        }
        private void HandleSpawnPosition()
        {
            SpawnPositionPacket packet = new SpawnPositionPacket();
            packet.Read(stream);

            Player.Spawn = new Vector3(packet.X, packet.Y, packet.Z);
            Player.Velocity = Vector3.Zero;
        }
        private void HandleUpdateHealth()
        {
            UpdateHealthPacket packet = new UpdateHealthPacket();
            packet.Read(stream);

            // Event Handlers
            if (OnHealthChange != null && Player.Health != packet.Health)
                OnHealthChange(new HealthUpdateEventArgs(Player.Health, packet.Health));
            if (OnHungerChange != null && Player.Food != packet.Food)
                OnHungerChange(new HungerUpdateEventArgs(Player.Food, packet.Food));

            Player.Health = packet.Health;
            Player.Food = packet.Food;
            Player.FoodSaturation = packet.FoodSaturation;
        }
        //private void HandleRespawn()
        //private void HandlePlayer()
        //private void HandlePlayerPosition()
        //private void HandlePlayerRotation()
        private void HandlePlayerPositionRotation()
        {
            PlayerPositionRotationPacket packet = new PlayerPositionRotationPacket();
            packet.Read(stream);

            Player.Location.X = packet.X;
            Player.Location.Y = packet.Y;
            Player.Location.Z = packet.Z;
            Player.Rotation.X = packet.Yaw;
            Player.Rotation.Y = packet.Pitch;

            // Respond to server
            SendPacket(packet);
        }
        //private void HandlePlayerDigging()
        //private void HandlePlayerBlockPlacement()
        //private void HandleHeldItemChange()
        //private void HandleUseBed()
        private void HandleAnimation()
        {
            AnimationPacket packet = new AnimationPacket();
            packet.Read(stream);

            // Update entity animation
        }
        //private void HandleEntityAction()
        private void HandleSpawnNamedEntity()
        {
            SpawnNamedEntityPacket packet = new SpawnNamedEntityPacket();
            packet.Read(stream);

            NamedEntity entity = new NamedEntity();
            entity.Id = packet.EntityId;
            entity.Name = packet.PlayerName;
            entity.Location.X = packet.X / 32.0;
            entity.Location.Y = packet.Y / 32.0;
            entity.Location.Z = packet.Z / 32.0;
            entity.Rotation = new Vector3(packet.Yaw, packet.Pitch, 0);
            entity.InHand.ItemId = packet.CurrentItem;
            World.Entities.Add(entity);
        }
        private void HandleSpawnDroppedItem()
        {
            SpawnDroppedItemPacket packet = new SpawnDroppedItemPacket();
            packet.Read(stream);
        }
        //private void HandleCollectItem()
        private void HandleSpawnObjectVehicle()
        {
            SpawnObjectVehiclePacket packet = new SpawnObjectVehiclePacket();
            packet.Read(stream);
        }
        private void HandleSpawnMob()
        {
            SpawnMobPacket packet = new SpawnMobPacket();
            packet.Read(stream);
        }
        //private void HandleSpawnPainting()
        //private void HandleSpawnExperienceOrb()
        private void HandleEntityVelocity()
        {
            EntityVelocityPacket packet = new EntityVelocityPacket();
            packet.Read(stream);

            Entity entity = World.GetEntity(packet.EntityId);
            if (entity != null)
            {
                entity.Velocity.X = packet.VelocityX / 32000.0;
                entity.Velocity.Y = packet.VelocityY / 32000.0;
                entity.Velocity.Z = packet.VelocityZ / 32000.0;
            }
        }
        private void HandleDestroyEntity()
        {
            DestroyEntityPacket packet = new DestroyEntityPacket();
            packet.Read(stream);

            World.DestroyEntity(packet.EntityId);
        }
        private void HandleEntity()
        {
            EntityPacket packet = new EntityPacket();
            packet.Read(stream);
        }
        private void HandleEntityRelativeMove()
        {
            EntityRelativeMovePacket packet = new EntityRelativeMovePacket();
            packet.Read(stream);

            Entity entity = World.GetEntity(packet.EntityId);
            if (entity != null)
            {
                entity.Location.X += packet.dX / 32.0;
                entity.Location.Y += packet.dY / 32.0;
                entity.Location.Z += packet.dZ / 32.0;
            }
        }
        private void HandleEntityLook()
        {
            EntityLookPacket packet = new EntityLookPacket();
            packet.Read(stream);

            Entity entity = World.GetEntity(packet.EntityId);
            if (entity != null)
                entity.Rotation = new Vector3(packet.Yaw, packet.Pitch, 0);
        }
        private void HandleEntityLookAndRelativeMove()
        {
            EntityLookAndRelativeMovePacket packet = new EntityLookAndRelativeMovePacket();
            packet.Read(stream);

            Entity entity = World.GetEntity(packet.EntityId);
            if (entity != null)
            {
                entity.Location.X += packet.dX / 32.0;
                entity.Location.Y += packet.dY / 32.0;
                entity.Location.Z += packet.dZ / 32.0;
                entity.Rotation = new Vector3(packet.Yaw, packet.Pitch, 0);
            }
        }
        private void HandleEntityTeleport()
        {
            EntityTeleportPacket packet = new EntityTeleportPacket();
            packet.Read(stream);

            Entity entity = World.GetEntity(packet.EntityId);
            if (entity != null)
            {
                entity.Location.X = packet.X / 32.0;
                entity.Location.Y = packet.Y / 32.0;
                entity.Location.Z = packet.Z / 32.0;
                entity.Rotation = new Vector3(packet.Yaw, packet.Pitch, 0);
            }
        }
        private void HandleEntityHeadLook()
        {
            EntityHeadLookPacket packet = new EntityHeadLookPacket();
            packet.Read(stream);

            Mob entity = (Mob)World.GetEntity(packet.EntityId);
            if (entity != null)
                entity.HeadYaw = packet.HeadYaw;
        }
        private void HandleEntityStatus()
        {
            EntityStatusPacket packet = new EntityStatusPacket();
            packet.Read(stream);
        }
        //private void HandleAttachEntity()
        private void HandleEntityMetadata()
        {
            EntityMetadataPacket packet = new EntityMetadataPacket();
            packet.Read(stream);

            Mob entity = (Mob)World.GetEntity(packet.EntityId);
            if (entity != null)
                entity.Metadata = packet.Metadata;
        }
        private void HandleEntityEffect()
        {
            EntityEffectPacket packet = new EntityEffectPacket();
            packet.Read(stream);
        }
        private void HandleRemoveEntityEffect()
        {
            RemoveEntityEffectPacket packet = new RemoveEntityEffectPacket();
            packet.Read(stream);
        }
        private void HandleSetExperience()
        {
            SetExperiencePacket packet = new SetExperiencePacket();
            packet.Read(stream);

            Player.ExperienceLevel = packet.Level;
            Player.ExperienceProgress = packet.BarProgress;
            Player.ExperienceTotal = packet.TotalExperience;
        }
        private void HandleChunkAllocation()
        {
            ChunkAllocationPacket packet = new ChunkAllocationPacket();
            packet.Read(stream);

            // Remember world or do as the nice server asks?
            if (!packet.LoadChunk)
            {
                Vector3 offset = new Vector3(packet.ChunkXCoor, 0, packet.ChunkZCoor);
                Region r = World.GetRegion(offset);
                offset -= r.Location;
                r.DeleteColumn(offset);
            }
        }
        public int chunkXMin = 0, chunkXMax = 0, chunkZMin = 0, chunkZMax = 0;
        private void HandleChunkData()
        {
            ChunkDataPacket packet = new ChunkDataPacket();
            packet.Read(stream);
            
            Console.WriteLine("Chunk Received: " + packet.X + " , " + packet.Z);
            chunkXMin = Math.Min(chunkXMin, packet.X);
            chunkXMax = Math.Max(chunkXMax, packet.X);
            chunkZMin = Math.Min(chunkZMin, packet.Z);
            chunkZMax = Math.Max(chunkZMax, packet.Z);

            Vector3 ChunkLocation = new Vector3(packet.X, 0, packet.Z);
            Region r = World.GetRegion(ChunkLocation);
            ChunkLocation -= r.Location;

            InflaterInputStream inflater = new InflaterInputStream(new MemoryStream(packet.CompressedData));
            Chunk c = null;
            byte temp = 0;

            // Block Ids
            for (int i = 0; i < 16; i++)
            {
                ChunkLocation.Y = i;
                c = r.GetChunk(ChunkLocation);
                if ((packet.PrimaryBitMap & (1 << i)) != 0)
                {
                    c.IsAir = false;

                    // Block Ids
                    for (int j = 0; j < 4096; j++)
                        c.Blocks[j] = (short)inflater.ReadByte();
                }
                else
                    c.IsAir = true;
            }
            // Metadata
            for (int i = 0; i < 16; i++)
            {
                if ((packet.PrimaryBitMap & (1 << 1)) != 0)
                {
                    ChunkLocation.Y = i;
                    c = r.GetChunk(ChunkLocation);
                    inflater.Read(c.Metadata.byteArray, 0, 2048);
                }
            }
            // Block Lights
            for (int i = 0; i < 16; i++)
            {
                if ((packet.PrimaryBitMap & (1 << 1)) != 0)
                {
                    ChunkLocation.Y = i;
                    c = r.GetChunk(ChunkLocation);
                    inflater.Read(c.BlockLight.byteArray, 0, 2048);
                }
            }
            // Sky Lights
            for (int i = 0; i < 16; i++)
            {
                if ((packet.PrimaryBitMap & (1 << 1)) != 0)
                {
                    ChunkLocation.Y = i;
                    c = r.GetChunk(ChunkLocation);
                    inflater.Read(c.SkyLight.byteArray, 0, 2048);
                }
            }
            // Add Arrays
            for (int i = 0; i < 16; i++)
            {
                if ((packet.AddBitMap & (1 << 1)) != 0)
                {
                    ChunkLocation.Y = i;
                    c = r.GetChunk(ChunkLocation);
                    for (int j = 0; j < 4096; j += 2)
                    {
                        temp = (byte)inflater.ReadByte();
                        c.Blocks[j] |= (short)((temp & 0xF0) << 4);
                        c.Blocks[j + 1] |= (short)((temp & 0x0F) << 8);
                    }
                }
            }
            // Read Biome Data
            // TODO

            inflater.Close();
        }
        private void HandleMultiBlockChange()
        {
            MultiBlockChangePacket packet = new MultiBlockChangePacket();
            packet.Read(stream);

            Region r = World.GetRegion(new Vector3(packet.ChunkX, 0, packet.ChunkZ));
            Vector3 offset = Vector3.Zero;
            Chunk c = null;
            Block b = null;

            for (int i = 0; i < packet.BlockId.Length; i++)
            {
                offset = packet.BlockCoordinate[i];
                offset.Y = (int)offset.Y;
                c = r.GetChunk(offset);
                offset.Y = packet.BlockCoordinate[i].Y - offset.Y * 16;
                b = packet.BlockId[i];
                b.Metadata = packet.BlockMetadata[i];
                c.SetBlock(offset, b);
            }
        }
        private void HandleBlockChange()
        {
            BlockChangePacket packet = new BlockChangePacket();
            packet.Read(stream);

            Vector3 offset = new Vector3(packet.X, packet.Y, packet.Z);
            Chunk c = World.GetChunk(offset);
        }
        private void HandleBlockAction()
        {
            BlockActionPacket packet = new BlockActionPacket();
            packet.Read(stream);
        }
        //private void HandleExplosion()
        private void HandleSoundParticleEffect()
        {
            SoundParticleEffectPacket packet = new SoundParticleEffectPacket();
            packet.Read(stream);
        }
        private void HandleChangeGameState()
        {
            ChangeGameStatePacket packet = new ChangeGameStatePacket();
            packet.Read(stream);
        }
        //private void HandleThunderbolt()
        //private void HandleOpenWindow()
        //private void HandleCloseWindow()
        private void HandleSetSlot()
        {
            SetSlotPacket packet = new SetSlotPacket();
            packet.Read(stream);
        }
        private void HandleSetWindowItems()
        {
            SetWindowItemsPacket packet = new SetWindowItemsPacket();
            packet.Read(stream);
        }
        //private void HandleUpdateWindowProperty()
        //private void HandleConfirmTransaction()
        //private void HandleCreativeInventoryAction()
        //private void HandleEnchantItem()
        //private void HandleUpdateSign()
        //private void HandleItemData()
        private void HandleUpdateTileEntry()
        {
            UpdateTileEntityPacket packet = new UpdateTileEntityPacket();
            packet.Read(stream);
        }
        //private void HandleIncrementStatistic()
        private void HandlePlayerListItem()
        {
            PlayerListItemPacket packet = new PlayerListItemPacket();
            packet.Read(stream);
        }
        private void HandlePlayerAbilities()
        {
            PlayerAbilitiesPacket packet = new PlayerAbilitiesPacket();
            packet.Read(stream);
        }
        //private void HandlePluginMessage()
        //private void HandleServerListPing()
        private void HandleDisconnect()
        {
            DisconnectPacket packet = new DisconnectPacket();
            packet.Read(stream);

            Console.WriteLine("Disconnected: " + packet.Reason);
            socket.Disconnect(false);
            stream.Dispose();
            stream = null;
        }
        #endregion
    }
}
