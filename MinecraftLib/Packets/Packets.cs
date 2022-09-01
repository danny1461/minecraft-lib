using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using MinecraftLib.Entities;

namespace MinecraftLib.Packets
{
    public abstract class Packet
    {
        public abstract void Read(NetworkStream s);
        public abstract void Write(NetworkStream s);

        public PacketType GetPacketType
        {
            get { return PacketMap.GetPacketType(GetType()); }
        }
    }

    public class KeepAlivePacket : Packet
    {
        public int RandomID { get; set; }

        public override void Read(NetworkStream s)
        {
            RandomID = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, RandomID);
        }
    }
    public class LoginRequestPacket : Packet
    {
        public int ProtocolOrEntityId { get; set; }
        public String Username { get; set; }
        public String LevelType { get; set; }
        public int ServerMode { get; set; }
        public int Dimension { get; set; }
        public sbyte Difficulty { get; set; }
        public byte NotUsed { get; set; }
        public byte MaxPlayers { get; set; }

        public override void Read(NetworkStream s)
        {
            ProtocolOrEntityId = StreamHelper.ReadInt(s);
            Username = StreamHelper.ReadString(s);
            LevelType = StreamHelper.ReadString(s);
            ServerMode = StreamHelper.ReadInt(s);
            Dimension = StreamHelper.ReadInt(s);
            Difficulty = StreamHelper.ReadSByte(s);
            NotUsed = StreamHelper.ReadByte(s);
            MaxPlayers = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, ProtocolOrEntityId);
            StreamHelper.Write(s, Username);
            StreamHelper.Write(s, LevelType);
            StreamHelper.Write(s, ServerMode);
            StreamHelper.Write(s, Dimension);
            StreamHelper.Write(s, Difficulty);
            StreamHelper.Write(s, NotUsed);
            StreamHelper.Write(s, MaxPlayers);
        }
    }
    public class HandshakePacket : Packet
    {
        public string UsernameAndHostOrHash { get; set; }

        public override void Read(NetworkStream s)
        {
            UsernameAndHostOrHash = StreamHelper.ReadString(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, UsernameAndHostOrHash);
        }
    }
    public class ChatMessagePacket : Packet
    {
        public String Message { get; set; }

        public override void Read(NetworkStream s)
        {
            Message = StreamHelper.ReadString(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Message);
        }
    }
    public class TimeUpdatePacket : Packet
    {
        public long Time { get; set; }

        public override void Read(NetworkStream s)
        {
            Time = StreamHelper.ReadLong(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Time);
        }
    }
    public class EntityEquipmentPacket : Packet
    {
        public int EntityId { get; set; }
        public short Slot { get; set; }
        public short ItemId { get; set; }
        public short Damage { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            Slot = StreamHelper.ReadShort(s);
            ItemId = StreamHelper.ReadShort(s);
            Damage = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, Slot);
            StreamHelper.Write(s, ItemId);
            StreamHelper.Write(s, Damage);
        }
    }
    public class SpawnPositionPacket : Packet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
        }
    }
    public class UseEntityPacket : Packet
    {
        public int User { get; set; }
        public int Target { get; set; }
        public bool LeftClick { get; set; }

        public override void Read(NetworkStream s)
        {
            User = StreamHelper.ReadInt(s);
            Target = StreamHelper.ReadInt(s);
            LeftClick = StreamHelper.ReadBoolean(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, User);
            StreamHelper.Write(s, Target);
            StreamHelper.Write(s, LeftClick);
        }
    }
    public class UpdateHealthPacket : Packet
    {
        public short Health { get; set; }
        public short Food { get; set; }
        public float FoodSaturation { get; set; }

        public override void Read(NetworkStream s)
        {
            Health = StreamHelper.ReadShort(s);
            Food = StreamHelper.ReadShort(s);
            FoodSaturation = StreamHelper.ReadFloat(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Health);
            StreamHelper.Write(s, Food);
            StreamHelper.Write(s, FoodSaturation);
        }
    }
    public class RespawnPacket : Packet
    {
        public int Dimension { get; set; }
        public sbyte Difficulty { get; set; }
        public sbyte CreativeMode { get; set; }
        public short WorldHeight { get; set; }
        public String LevelType { get; set; }

        public override void Read(NetworkStream s)
        {
            Dimension = StreamHelper.ReadInt(s);
            Difficulty = (sbyte)StreamHelper.ReadByte(s);
            CreativeMode = (sbyte)StreamHelper.ReadByte(s);
            WorldHeight = StreamHelper.ReadShort(s);
            LevelType = StreamHelper.ReadString(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Dimension);
            StreamHelper.Write(s, Difficulty);
            StreamHelper.Write(s, CreativeMode);
            StreamHelper.Write(s, WorldHeight);
            StreamHelper.Write(s, LevelType);
        }
    }
    public class PlayerPacket : Packet
    {
        public bool OnGround { get; set; }

        public override void Read(NetworkStream s)
        {
            OnGround = StreamHelper.ReadBoolean(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, OnGround);
        }
    }
    public class PlayerPositionPacket : PlayerPacket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Stance { get; set; }
        public double Z { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadDouble(s);
            Y = StreamHelper.ReadDouble(s);
            Stance = StreamHelper.ReadDouble(s);
            Z = StreamHelper.ReadDouble(s);
            base.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Stance);
            StreamHelper.Write(s, Z);
            base.Write(s);
        }
    }
    public class PlayerRotationPacket : PlayerPacket
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public override void Read(NetworkStream s)
        {
            Yaw = StreamHelper.ReadFloat(s);
            Pitch = StreamHelper.ReadFloat(s);
            base.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
            base.Write(s);
        }
    }
    public class PlayerPositionRotationPacket : PlayerPacket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Stance { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadDouble(s);
            Stance = StreamHelper.ReadDouble(s);
            Y = StreamHelper.ReadDouble(s);
            Z = StreamHelper.ReadDouble(s);
            Yaw = StreamHelper.ReadFloat(s);
            Pitch = StreamHelper.ReadFloat(s);
            base.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Stance);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
            base.Write(s);
        }
    }
    public class PlayerDiggingPacket : Packet
    {
        public sbyte Status { get; set; }
        public int X { get; set; }
        public sbyte Y { get; set; }
        public int Z { get; set; }
        public sbyte Face { get; set; }

        public override void Read(NetworkStream s)
        {
            Status = StreamHelper.ReadSByte(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadSByte(s);
            Z = StreamHelper.ReadInt(s);
            Face = StreamHelper.ReadSByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Status);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Face);
        }
    }
    public class PlayerBlockPlacementPacket : Packet
    {
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }
        public sbyte Direction { get; set; }
        public Slot HeldItem { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Y = (byte)StreamHelper.ReadByte(s);
            Z = StreamHelper.ReadInt(s);
            Direction = StreamHelper.ReadSByte(s);
            HeldItem = new Slot();
            HeldItem.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Direction);
            // Write SlotData
        }
    }
    public class HeldItemChangePacket : Packet
    {
        public short SlotId { get; set; }

        public override void Read(NetworkStream s)
        {
            SlotId = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, SlotId);
        }
    }
    public class UseBedPacket : Packet
    {
        public int EntityId { get; set; }
        public sbyte Unknown { get; set; }
        public int BedX { get; set; }
        public sbyte BedY { get; set; }
        public int BedZ { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            Unknown = StreamHelper.ReadSByte(s);
            BedX = StreamHelper.ReadInt(s);
            BedY = StreamHelper.ReadSByte(s);
            BedZ = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, Unknown);
            StreamHelper.Write(s, BedX);
            StreamHelper.Write(s, BedY);
            StreamHelper.Write(s, BedZ);
        }
    }
    public class AnimationPacket : Packet
    {
        public int EntityId { get; set; }
        public sbyte Animation { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            Animation = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, Animation);
        }
    }
    public class EntityActionPacket : Packet
    {
        public int EntityId { get; set; }
        public sbyte ActionId { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            ActionId = StreamHelper.ReadSByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, ActionId);
        }
    }
    public class SpawnNamedEntityPacket : Packet
    {
        public int EntityId { get; set; }
        public String PlayerName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public sbyte Yaw { get; set; }
        public sbyte Pitch { get; set; }
        public short CurrentItem { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            PlayerName = StreamHelper.ReadString(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            Yaw = (sbyte)StreamHelper.ReadByte(s);
            Pitch = (sbyte)StreamHelper.ReadByte(s);
            CurrentItem = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, PlayerName);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
            StreamHelper.Write(s, Math.Max(CurrentItem, (short)0));
        }
    }
    public class SpawnDroppedItemPacket : Packet
    {
        public int EntityId { get; set; }
        public short ItemId { get; set; }
        public sbyte Count { get; set; }
        public short Damage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public sbyte Yaw { get; set; }
        public sbyte Pitch { get; set; }
        public sbyte Roll { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            ItemId = StreamHelper.ReadShort(s);
            Count = StreamHelper.ReadSByte(s);
            Damage = StreamHelper.ReadShort(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            Yaw = (sbyte)StreamHelper.ReadByte(s);
            Pitch = (sbyte)StreamHelper.ReadByte(s);
            Roll = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, ItemId);
            StreamHelper.Write(s, Count);
            StreamHelper.Write(s, Damage);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
            StreamHelper.Write(s, Roll);
        }
    }
    public class CollectItemPacket : Packet
    {
        public int CollectedEntityId { get; set; }
        public int CollectorEntityId { get; set; }

        public override void Read(NetworkStream s)
        {
            CollectedEntityId = StreamHelper.ReadInt(s);
            CollectorEntityId = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, CollectedEntityId);
            StreamHelper.Write(s, CollectorEntityId);
        }
    }
    public class SpawnObjectVehiclePacket : Packet
    {
        public int ObjectId { get; set; }
        public sbyte ObjectType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int FireballThrowerId { get; set; }
        public short SpeedX { get; set; }
        public short SpeedY { get; set; }
        public short SpeedZ { get; set; }

        public override void Read(NetworkStream s)
        {
            ObjectId = StreamHelper.ReadInt(s);
            ObjectType = (sbyte)StreamHelper.ReadByte(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            FireballThrowerId = StreamHelper.ReadInt(s);
            if (FireballThrowerId > 0)
            {
                SpeedX = StreamHelper.ReadShort(s);
                SpeedY = StreamHelper.ReadShort(s);
                SpeedZ = StreamHelper.ReadShort(s);
            }
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, ObjectId);
            StreamHelper.Write(s, ObjectType);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, FireballThrowerId);
            if (FireballThrowerId > 0)
            {
                StreamHelper.Write(s, SpeedX);
                StreamHelper.Write(s, SpeedY);
                StreamHelper.Write(s, SpeedZ);
            }
        }
    }
    public class SpawnMobPacket : Packet
    {
        public int EntityId { get; set; }
        public byte Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public sbyte Yaw { get; set; }
        public sbyte Pitch { get; set; }
        public sbyte HeadYaw { get; set; }
        public EntityMetadata Metadata { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            Type = StreamHelper.ReadByte(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            Yaw = StreamHelper.ReadSByte(s);
            Pitch = StreamHelper.ReadSByte(s);
            HeadYaw = StreamHelper.ReadSByte(s);
            Metadata = new EntityMetadata();
            Metadata.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
            foreach (int index in Metadata.data.Keys)
            {
                StreamHelper.Write(s, (byte)Metadata[index].Type);
                if (Metadata[index].Type == MetadataValueType.Byte)
                    StreamHelper.Write(s, (byte)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.Short)
                    StreamHelper.Write(s, (short)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.Int)
                    StreamHelper.Write(s, (int)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.Float)
                    StreamHelper.Write(s, (float)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.String16)
                    StreamHelper.Write(s, (String)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.ShortByteShort)
                {
                    StreamHelper.Write(s, (short)Metadata[index]["id"]);
                    StreamHelper.Write(s, (byte)Metadata[index]["count"]);
                    StreamHelper.Write(s, (short)Metadata[index]["damage"]);
                }
                else if (Metadata[index].Type == MetadataValueType.IntIntInt)
                {
                    StreamHelper.Write(s, (int)Metadata[index]["val1"]);
                    StreamHelper.Write(s, (int)Metadata[index]["val2"]);
                    StreamHelper.Write(s, (int)Metadata[index]["val3"]);
                }
            }
            StreamHelper.Write(s, (byte)127);
        }
    }
    public class SpawnPaintingPacket : Packet
    {
        public int EntityId { get; set; }
        public String Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Direction { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            Title = StreamHelper.ReadString(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            Direction = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, Title);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Direction);
        }
    }
    public class SpawnExperienceOrbPacket : Packet
    {
        public int EntityId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public short Count { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            Count = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Count);
        }
    }
    public class EntityVelocityPacket : Packet
    {
        public int EntityId { get; set; }
        public short VelocityX { get; set; }
        public short VelocityY { get; set; }
        public short VelocityZ { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
            VelocityX = StreamHelper.ReadShort(s);
            VelocityY = StreamHelper.ReadShort(s);
            VelocityZ = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
            StreamHelper.Write(s, VelocityX);
            StreamHelper.Write(s, VelocityY);
            StreamHelper.Write(s, VelocityZ);
        }
    }
    public class DestroyEntityPacket : Packet
    {
        public int EntityId { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
        }
    }
    public class EntityPacket : Packet
    {
        public int EntityId { get; set; }

        public override void Read(NetworkStream s)
        {
            EntityId = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, EntityId);
        }
    }
    public class EntityRelativeMovePacket : EntityPacket
    {
        public sbyte dX { get; set; }
        public sbyte dY { get; set; }
        public sbyte dZ { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            dX = (sbyte)StreamHelper.ReadByte(s);
            dY = (sbyte)StreamHelper.ReadByte(s);
            dZ = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, dX);
            StreamHelper.Write(s, dY);
            StreamHelper.Write(s, dZ);
        }
    }
    public class EntityLookPacket : EntityPacket
    {
        public sbyte Yaw { get; set; }
        public sbyte Pitch { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            Yaw = (sbyte)StreamHelper.ReadByte(s);
            Pitch = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
        }
    }
    public class EntityLookAndRelativeMovePacket : EntityPacket
    {
        public sbyte dX { get; set; }
        public sbyte dY { get; set; }
        public sbyte dZ { get; set; }
        public sbyte Yaw { get; set; }
        public sbyte Pitch { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            dX = (sbyte)StreamHelper.ReadByte(s);
            dY = (sbyte)StreamHelper.ReadByte(s);
            dZ = (sbyte)StreamHelper.ReadByte(s);
            Yaw = (sbyte)StreamHelper.ReadByte(s);
            Pitch = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, dX);
            StreamHelper.Write(s, dY);
            StreamHelper.Write(s, dZ);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
        }
    }
    public class EntityTeleportPacket : EntityPacket
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public sbyte Yaw { get; set; }
        public sbyte Pitch { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            Yaw = (sbyte)StreamHelper.ReadByte(s);
            Pitch = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Yaw);
            StreamHelper.Write(s, Pitch);
        }
    }
    public class EntityHeadLookPacket : EntityPacket
    {
        public sbyte HeadYaw { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            HeadYaw = StreamHelper.ReadSByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, HeadYaw);
        }
    }
    public class EntityStatusPacket : EntityPacket
    {
        public byte Status { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            Status = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, Status);
        }
    }
    public class AttachEntityPacket : EntityPacket
    {
        public int VehicleId { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            VehicleId = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, VehicleId);
        }
    }
    public class EntityMetadataPacket : EntityPacket
    {
        public EntityMetadata Metadata { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            Metadata = new EntityMetadata();
            Metadata.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            foreach (int index in Metadata.data.Keys)
            {
                StreamHelper.Write(s, (byte)Metadata[index].Type);
                if (Metadata[index].Type == MetadataValueType.Byte)
                    StreamHelper.Write(s, (byte)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.Short)
                    StreamHelper.Write(s, (short)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.Int)
                    StreamHelper.Write(s, (int)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.Float)
                    StreamHelper.Write(s, (float)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.String16)
                    StreamHelper.Write(s, (String)Metadata[index]["default"]);
                else if (Metadata[index].Type == MetadataValueType.ShortByteShort)
                {
                    StreamHelper.Write(s, (short)Metadata[index]["id"]);
                    StreamHelper.Write(s, (byte)Metadata[index]["count"]);
                    StreamHelper.Write(s, (short)Metadata[index]["damage"]);
                }
                else if (Metadata[index].Type == MetadataValueType.IntIntInt)
                {
                    StreamHelper.Write(s, (int)Metadata[index]["val1"]);
                    StreamHelper.Write(s, (int)Metadata[index]["val2"]);
                    StreamHelper.Write(s, (int)Metadata[index]["val3"]);
                }
            }
            StreamHelper.Write(s, (byte)127);
        }
    }
    public class EntityEffectPacket : EntityPacket
    {
        public enum Effects : sbyte
        {
            MoveSpeedUp = 1,
            MoveSlowDown = 2,
            DigSpeedUp = 3,
            DigSlowDown = 4,
            DamageBoost = 5,
            Heal = 6,
            Harm = 7,
            Jump = 8,
            Confusion = 9,
            Regeneration = 10,
            Resistance = 11,
            FireResistance = 12,
            WaterBreathing = 13,
            Invisibility = 14,
            Blindness = 15,
            NightVision = 16,
            Hunger = 17,
            Weakness = 18,
            Poison = 19
        }

        public Effects Effect { get; set; }
        public sbyte Amplifier { get; set; }
        public short Duration { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            Effect = (Effects)StreamHelper.ReadSByte(s);
            Amplifier = StreamHelper.ReadSByte(s);
            Duration = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, (sbyte)Effect);
            StreamHelper.Write(s, Amplifier);
            StreamHelper.Write(s, Duration);
        }
    }
    public class RemoveEntityEffectPacket : EntityPacket
    {
        public sbyte EffectId { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            EffectId = (sbyte)StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, EffectId);
        }
    }
    public class SetExperiencePacket : Packet
    {
        public float BarProgress { get; set; }
        public short Level { get; set; }
        public short TotalExperience { get; set; }

        public override void Read(NetworkStream s)
        {
            BarProgress = StreamHelper.ReadFloat(s);
            Level = StreamHelper.ReadShort(s);
            TotalExperience = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, BarProgress);
            StreamHelper.Write(s, Level);
            StreamHelper.Write(s, TotalExperience);
        }
    }
    public class ChunkAllocationPacket : Packet
    {
        public int ChunkXCoor { get; set; }
        public int ChunkZCoor { get; set; }
        public bool LoadChunk { get; set; }

        public override void Read(NetworkStream s)
        {
            ChunkXCoor = StreamHelper.ReadInt(s);
            ChunkZCoor = StreamHelper.ReadInt(s);
            LoadChunk = StreamHelper.ReadByte(s) == 1;
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, ChunkXCoor);
            StreamHelper.Write(s, ChunkZCoor);
            StreamHelper.Write(s, LoadChunk);
        }
    }
    public class ChunkDataPacket : Packet
    {
        public int X { get; set; }
        public int Z { get; set; }
        public bool GroundUpContinuous { get; set; }
        public short PrimaryBitMap { get; set; }
        public short AddBitMap { get; set; }
        public byte[] CompressedData { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
            GroundUpContinuous = StreamHelper.ReadBoolean(s);
            PrimaryBitMap = StreamHelper.ReadShort(s);
            AddBitMap = StreamHelper.ReadShort(s);
            int compressedSize = StreamHelper.ReadInt(s);
            StreamHelper.ReadInt(s);
            CompressedData = new byte[compressedSize];
            CompressedData = StreamHelper.ReadBytes(s, compressedSize);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, GroundUpContinuous);
            StreamHelper.Write(s, PrimaryBitMap);
            StreamHelper.Write(s, AddBitMap);
            StreamHelper.Write(s, CompressedData.Length);
            StreamHelper.Write(s, 0);
            StreamHelper.Write(s, CompressedData);
        }
    }
    public class MultiBlockChangePacket : Packet
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public byte[] BlockMetadata { get; set; }
        public short[] BlockId { get; set; }
        public Vector3[] BlockCoordinate { get; set; }

        public override void Read(NetworkStream s)
        {
            ChunkX = StreamHelper.ReadInt(s);
            ChunkZ = StreamHelper.ReadInt(s);
            int RecordCount = StreamHelper.ReadShort(s);
            StreamHelper.ReadInt(s);
            StreamHelper sh;
            BlockMetadata = new byte[RecordCount];
            BlockId = new short[RecordCount];
            BlockCoordinate = new Vector3[RecordCount];
            for (int i = 0; i < RecordCount; i++)
            {
                sh = new StreamHelper(s, 4);
                BlockMetadata[i] = sh.ReadBitsToByte(4);
                BlockId[i] = sh.ReadBitsToShort(12);
                BlockCoordinate[i] = new Vector3(sh.ReadBitsToInt(8), sh.ReadBitsToInt(4), sh.ReadBitsToInt(4));
                BlockCoordinate[i] = new Vector3(BlockCoordinate[i].Z, BlockCoordinate[i].X, BlockCoordinate[i].Y);
            }
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, ChunkX);
            StreamHelper.Write(s, ChunkZ);
            StreamHelper.Write(s, BlockId.Length);
            StreamHelper.Write(s, BlockId.Length * 4);
            for (int i = 0; i < BlockId.Length; i++)
            {
                StreamHelper.Write(s, (short)(((BlockMetadata[i] << 12) | (UInt16)BlockId[i])));
                StreamHelper.Write(s, (byte)BlockCoordinate[i].Y);
                StreamHelper.Write(s, (byte)(((int)BlockCoordinate[i].Z << 4) | (int)(BlockCoordinate[i].X)));
            }
        }
    }
    public class BlockChangePacket : Packet
    {
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }
        public byte BlockType { get; set; }
        public byte Metadata { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadByte(s);
            Z = StreamHelper.ReadInt(s);
            BlockType = StreamHelper.ReadByte(s);
            Metadata = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, BlockType);
            StreamHelper.Write(s, Metadata);
        }
    }
    public class BlockActionPacket : Packet
    {
        public int X { get; set; }
        public short Y { get; set; }
        public int Z { get; set; }
        public byte Byte1 { get; set; }
        public byte Byte2 { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadShort(s);
            Z = StreamHelper.ReadInt(s);
            Byte1 = StreamHelper.ReadByte(s);
            Byte2 = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Byte1);
            StreamHelper.Write(s, Byte2);
        }
    }
    public class ExplosionPacket : Packet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Unknown { get; set; }
        public byte[,] Records { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadDouble(s);
            Y = StreamHelper.ReadDouble(s);
            Z = StreamHelper.ReadDouble(s);
            Unknown = StreamHelper.ReadFloat(s);
            int RecordCount = StreamHelper.ReadInt(s);
            Records = new byte[RecordCount, 3];
            for (int x = 0; x < RecordCount; x++)
            {
                for (int y = 0; y < 3; y++)
                    Records[x, y] = (byte)StreamHelper.ReadByte(s);
            }
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Unknown);
            int RecordCount = Records.GetLength(0);
            StreamHelper.Write(s, RecordCount);
            for (int x = 0; x < RecordCount; x++)
            {
                for (int y = 0; y < 3; y++)
                    StreamHelper.Write(s, Records[x, y]);
            }
        }
    }
    public class SoundParticleEffectPacket : Packet
    {
        public enum Effects : int
        {
            Click2 = 1000,
            Click1 = 1001,
            Bow_Fire = 1002,
            Door_Toggle = 1003,
            Extinguish = 1004,
            Music_Disc = 1005,
            Charge = 1007,
            Fireball1 = 1008,
            Fireball2 = 1009,
            Zombie_Bang_Door = 1010,
            Zombie_Bang_Iron_Door = 1011,
            Zombie_Break_Door = 1012,
            Smoke = 2000,
            Block_Break = 2001,
            Splash_Potion = 2002,
            Eye_Of_Ender = 2003,
            Mob_Spawn_Particles = 2004
        }

        public Effects Effect { get; set; }
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }
        public int Data { get; set; }

        public override void Read(NetworkStream s)
        {
            Effect = (Effects)StreamHelper.ReadInt(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadByte(s);
            Z = StreamHelper.ReadInt(s);
            Data = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, (int)Effect);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Data);
        }
    }
    public class ChangeGameStatePacket : Packet
    {
        public enum ReasonEnum : byte
        {
            InvalidBed = 0,
            BeginRaining = 1,
            EndRaining = 2,
            ChangeGameMode = 3,
            EnterCredits = 4
        }

        public ReasonEnum Reason { get; set; }
        public byte GameMode { get; set; }

        public override void Read(NetworkStream s)
        {
            this.Reason = (ReasonEnum)StreamHelper.ReadByte(s);
            GameMode = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, (byte)Reason);
            StreamHelper.Write(s, GameMode);
        }
    }
    public class ThunderBoltPacket : EntityPacket
    {
        public bool Unknown { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override void Read(NetworkStream s)
        {
            base.Read(s);
            Unknown = StreamHelper.ReadBoolean(s);
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadInt(s);
            Z = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            base.Write(s);
            StreamHelper.Write(s, Unknown);
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
        }
    }
    public class OpenWindowPacket : Packet
    {
        public byte WindowId { get; set; }
        public byte InventoryType { get; set; }
        public String WindowTitle { get; set; }
        public byte NumberOfSlots { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = StreamHelper.ReadByte(s);
            InventoryType = StreamHelper.ReadByte(s);
            WindowTitle = StreamHelper.ReadString(s);
            NumberOfSlots = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
            StreamHelper.Write(s, InventoryType);
            StreamHelper.Write(s, WindowTitle);
            StreamHelper.Write(s, NumberOfSlots);
        }
    }
    public class CloseWindowPacket : Packet
    {
        public byte WindowId { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
        }
    }
    public class ClickWindowPacket : Packet
    {
        public byte WindowId { get; set; }
        public short Slot { get; set; }
        public byte RightClick { get; set; }
        public short ActionNumber { get; set; }
        public bool Shift { get; set; }
        public Slot ClickedItem { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = (byte)StreamHelper.ReadByte(s);
            Slot = StreamHelper.ReadShort(s);
            RightClick = (byte)StreamHelper.ReadByte(s);
            ActionNumber = StreamHelper.ReadShort(s);
            Shift = StreamHelper.ReadBoolean(s);
            ClickedItem = new Slot();
            ClickedItem.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            throw new NotImplementedException();
        }
    }
    public class SetSlotPacket : Packet
    {
        public byte WindowId { get; set; }
        public short Slot { get; set; }
        public Slot Data { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = StreamHelper.ReadByte(s);
            Slot = StreamHelper.ReadShort(s);
            Data = new Slot();
            Data.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
            StreamHelper.Write(s, Slot);
            Data.Write(s);
        }
    }
    public class SetWindowItemsPacket : Packet
    {
        public byte WindowId { get; set; }
        Slot[] SlotArray { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = (byte)StreamHelper.ReadByte(s);
            short Count = StreamHelper.ReadShort(s);
            SlotArray = new Slot[Count];
            for (int x = 0; x < Count; x++)
            {
                SlotArray[x] = new Slot();
                SlotArray[x].Read(s);
            }
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
            StreamHelper.Write(s, (short)SlotArray.Length);
            for (int x = 0; x < SlotArray.Length; x++)
                SlotArray[x].Write(s);
        }
    }
    public class UpdateWindowPropertyPacket : Packet
    {
        public byte WindowId { get; set; }
        public short Property { get; set; }
        public short Value { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = (byte)StreamHelper.ReadByte(s);
            Property = StreamHelper.ReadShort(s);
            Value = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
            StreamHelper.Write(s, Property);
            StreamHelper.Write(s, Value);
        }
    }
    public class ConfirmTransactionPacket : Packet
    {
        public byte WindowId { get; set; }
        public short ActionNumber { get; set; }
        public bool Accepted { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = (byte)StreamHelper.ReadByte(s);
            ActionNumber = StreamHelper.ReadShort(s);
            Accepted = StreamHelper.ReadBoolean(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
            StreamHelper.Write(s, ActionNumber);
            StreamHelper.Write(s, Accepted);
        }
    }
    public class CreativeInventoryActionPacket : Packet
    {
        public short SlotNumber { get; set; }
        public Slot Slot { get; set; }

        public override void Read(NetworkStream s)
        {
            SlotNumber = StreamHelper.ReadShort(s);
            Slot = new Slot();
            Slot.Read(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, SlotNumber);
            Slot.Write(s);
        }
    }
    public class EnchantItemPacket : Packet
    {
        public byte WindowId { get; set; }
        public byte Enchantment { get; set; }

        public override void Read(NetworkStream s)
        {
            WindowId = StreamHelper.ReadByte(s);
            Enchantment = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, WindowId);
            StreamHelper.Write(s, Enchantment);
        }
    }
    public class UpdateSignPacket : Packet
    {
        public int X { get; set; }
        public short Y { get; set; }
        public int Z { get; set; }
        public String Line1 { get; set; }
        public String Line2 { get; set; }
        public String Line3 { get; set; }
        public String Line4 { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadShort(s);
            Z = StreamHelper.ReadInt(s);
            Line1 = StreamHelper.ReadString(s);
            Line2 = StreamHelper.ReadString(s);
            Line3 = StreamHelper.ReadString(s);
            Line4 = StreamHelper.ReadString(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Line1);
            StreamHelper.Write(s, Line2);
            StreamHelper.Write(s, Line3);
            StreamHelper.Write(s, Line4);
        }
    }
    public class ItemDataPacket : Packet
    {
        public short ItemType { get; set; }
        public short ItemId { get; set; }
        public byte[] Data { get; set; }

        public override void Read(NetworkStream s)
        {
            ItemType = StreamHelper.ReadShort(s);
            ItemId = StreamHelper.ReadShort(s);
            byte dataLength = StreamHelper.ReadByte(s);
            Data = new byte[dataLength];
            Data = StreamHelper.ReadBytes(s, dataLength);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, ItemType);
            StreamHelper.Write(s, ItemId);
            StreamHelper.Write(s, (byte)Data.Length);
            StreamHelper.Write(s, Data);
        }
    }
    public class UpdateTileEntityPacket : Packet
    {
        public int X { get; set; }
        public short Y { get; set; }
        public int Z { get; set; }
        public byte Action { get; set; }
        public int Custom1 { get; set; }
        public int Custom2 { get; set; }
        public int Custom3 { get; set; }

        public override void Read(NetworkStream s)
        {
            X = StreamHelper.ReadInt(s);
            Y = StreamHelper.ReadShort(s);
            Z = StreamHelper.ReadInt(s);
            Action = StreamHelper.ReadByte(s);
            Custom1 = StreamHelper.ReadInt(s);
            Custom2 = StreamHelper.ReadInt(s);
            Custom3 = StreamHelper.ReadInt(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, X);
            StreamHelper.Write(s, Y);
            StreamHelper.Write(s, Z);
            StreamHelper.Write(s, Action);
            StreamHelper.Write(s, Custom1);
            StreamHelper.Write(s, Custom2);
            StreamHelper.Write(s, Custom3);
        }
    }
    public class IncrementStatisticPacket : Packet
    {
        public enum Statistics
        {
            StartGame = 1000,
            CreateWorld = 1001,
            LoadWorld = 1002,
            JoinMultiplayer = 1003,
            LeaveGame = 1004,
            PlayOneMinute = 1100,
            WalkOneCm = 2000,
            SwimOneCm = 2001,
            FallOneCm = 2002,
            ClimbOneCm = 2003,
            FlyOneCm = 2004,
            DiveOneCm = 2005,
            MinecartOneCm = 2006,
            BoatOneCm = 2007,
            PigOneCm = 2008,
            Jump = 2010,
            Drop = 2011,
            DamageDealt = 2020,
            DamageTaken = 2021,
            Deaths = 2022,
            MobKills = 2023,
            PlayerKills = 2024,
            FishCaught = 2025,
            MineBlock = 16777216,	// Note: Add an item ID to this value
            CraftItem = 16842752,	// Note: Add an item ID to this value
            UseItem = 16908288,		// Note: Add an item ID to this value
            BreakItem = 16973824	// Note: Add an item ID to this value
        }

        public Statistics Statistic { get; set; }
        public byte Amount { get; set; }

        public override void Read(NetworkStream s)
        {
            Statistic = (Statistics)StreamHelper.ReadInt(s);
            Amount = StreamHelper.ReadByte(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, (int)Statistic);
            StreamHelper.Write(s, Amount);
        }
    }
    public class PlayerListItemPacket : Packet
    {
        public String PlayerName { get; set; }
        public bool Online { get; set; }
        public short Ping { get; set; }

        public override void Read(NetworkStream s)
        {
            PlayerName = StreamHelper.ReadString(s);
            Online = StreamHelper.ReadBoolean(s);
            Ping = StreamHelper.ReadShort(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, PlayerName);
            StreamHelper.Write(s, Online);
            StreamHelper.Write(s, Ping);
        }
    }
    public class PlayerAbilitiesPacket : Packet
    {
        public bool Invulnerability { get; set; }
        public bool IsFlying { get; set; }
        public bool CanFly { get; set; }
        public bool InstantDestroy { get; set; }

        public override void Read(NetworkStream s)
        {
            Invulnerability = StreamHelper.ReadBoolean(s);
            IsFlying = StreamHelper.ReadBoolean(s);
            CanFly = StreamHelper.ReadBoolean(s);
            InstantDestroy = StreamHelper.ReadBoolean(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Invulnerability);
            StreamHelper.Write(s, IsFlying);
            StreamHelper.Write(s, CanFly);
            StreamHelper.Write(s, InstantDestroy);
        }
    }
    public class PluginMessagePacket : Packet
    {
        public String Channel { get; set; }
        public byte[] Data { get; set; }

        public override void Read(NetworkStream s)
        {
            Channel = StreamHelper.ReadString(s);
            short dataLength = StreamHelper.ReadShort(s);
            Data = new byte[dataLength];
            Data = StreamHelper.ReadBytes(s, dataLength);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Channel);
            StreamHelper.Write(s, (short)Data.Length);
            StreamHelper.Write(s, Data);
        }
    }
    public class ServerListPingPacket : Packet
    {
        public override void Read(NetworkStream s)
        {
            throw new NotImplementedException();
        }
        public override void Write(NetworkStream s) { }
    }
    public class DisconnectPacket : Packet
    {
        public String Reason { get; set; }

        public DisconnectPacket() { }
        public DisconnectPacket(String Reason)
        {
            this.Reason = Reason;
        }
        public override void Read(NetworkStream s)
        {
            Reason = StreamHelper.ReadString(s);
        }
        public override void Write(NetworkStream s)
        {
            StreamHelper.Write(s, Reason);
        }
    }
}
