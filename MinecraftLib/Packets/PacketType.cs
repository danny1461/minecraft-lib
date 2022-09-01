using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Packets
{
    public enum PacketType : byte
    {
        KeepAlive = 0x00,                   // c <-> s
        LoginRequest = 0x01,                //   <->
        Handshake = 0x02,                   //   <->
        ChatMessage = 0x03,                 //   <->
        TimeUpdate = 0x04,                  //   <--
        EntityEquipment = 0x05,             //   <->
        SpawnPosition = 0x06,               //   <--
        UseEntity = 0x07,                   //   -->
        UpdateHealth = 0x08,                //   <--
        Respawn = 0x09,                     //   <->
        Player = 0x0A,                      //   -->
        PlayerPosition = 0x0B,              //   -->
        PlayerRotation = 0x0C,              //   -->
        PlayerPositionRotation = 0x0D,      //   -->
        PlayerDigging = 0x0E,               //   -->
        PlayerBlockPlacement = 0x0F,        //   -->
        HeldItemChange = 0x10,              //   <->
        UseBed = 0x11,                      //   <->
        Animation = 0x12,                   //   <->
        EntityAction = 0x13,                //   <--
        SpawnNamedEntity = 0x14,            //   <--
        SpawnDroppedItem = 0x15,            //   <->
        CollectItem = 0x16,                 //   <--
        SpawnObjectVehicle = 0x17,          //   <--
        SpawnMob = 0x18,                    //   <--
        SpawnPainting = 0x19,               //   <--
        SpawnExperienceOrb = 0x1A,          //   <--
        EntityVelocity = 0x1C,              //   <--
        DestroyEntity = 0x1D,               //   <--
        Entity = 0x1E,                      //   <--
        EntityRelativeMove = 0x1F,          //   <--
        EntityLook = 0x20,                  //   <--
        EntityLookAndRelativeMove = 0x21,   //   <--
        EntityTeleport = 0x22,              //   <--
        EntityHeadLook = 0x23,              //   <--
        EntityStatus = 0x26,                //   <--
        AttachEntity = 0x27,                //   <--
        EntityMetadata = 0x28,              //   <--
        EntityEffect = 0x29,                //   <->
        RemoveEntityEffect = 0x2A,          //   <->
        SetExperience = 0x2B,               //   <--
        ChunkAllocation = 0x32,             //   <--
        ChunkData = 0x33,                   //   <--
        MultiBlockChange = 0x34,            //   <--
        BlockChange = 0x35,                 //   <--
        BlockAction = 0x36,                 //   <--
        Explosion = 0x3C,                   //   <--
        SoundParticleEffect = 0x3D,         //   <--
        ChangeGameState = 0x46,             //   <--
        Thunderbolt = 0x47,                 //   <--
        OpenWindow = 0x64,                  //   <--
        CloseWindow = 0x65,                 //   <--
        ClickWindow = 0x66,                 //   -->
        SetSlot = 0x67,                     //   <--
        SetWindowItems = 0x68,              //   <--
        UpdateWindowProperty = 0x69,        //   <--
        ConfirmTransaction = 0x6A,          //   <->
        CreativeInventoryAction = 0x6B,     //   <--
        EnchantItem = 0x6C,                 //   <--
        UpdateSign = 0x82,                  //   <->
        ItemData = 0x83,                    //   -->
        UpdateTileEntry = 0x84,             //   <--
        IncrementStatistic = 0xC8,          //   ???
        PlayerListItem = 0xC9,              //   <--
        PlayerAbilities = 0xCA,             //   <--
        PluginMessage = 0xFA,               //   <->
        ServerListPing = 0xFE,              //   -->
        Disconnect = 0xFF                   //   <->
    }
    static class PacketMap
    {
        private static readonly Dictionary<Type, PacketType> map = new Dictionary<Type, PacketType>
        {
            { typeof(SpawnObjectVehiclePacket), PacketType.SpawnObjectVehicle },
            { typeof(AnimationPacket), PacketType.Animation },
            { typeof(AttachEntityPacket), PacketType.AttachEntity },
            { typeof(BlockChangePacket), PacketType.BlockChange },
            { typeof(BlockActionPacket), PacketType.BlockAction },
            { typeof(ChatMessagePacket), PacketType.ChatMessage },
            { typeof(CloseWindowPacket), PacketType.CloseWindow },
            { typeof(CollectItemPacket), PacketType.CollectItem },
			{ typeof(CreativeInventoryActionPacket), PacketType.CreativeInventoryAction },
            { typeof(DestroyEntityPacket), PacketType.DestroyEntity },
            { typeof(DisconnectPacket), PacketType.Disconnect },
            { typeof(EntityPacket), PacketType.Entity },
            { typeof(EntityActionPacket), PacketType.EntityAction },
            { typeof(EntityEffectPacket), PacketType.EntityEffect },
            { typeof(EntityEquipmentPacket), PacketType.EntityEquipment },
            { typeof(EntityLookPacket), PacketType.EntityLook },
            { typeof(EntityLookAndRelativeMovePacket), PacketType.EntityLookAndRelativeMove },
            { typeof(EntityMetadataPacket), PacketType.EntityMetadata },
            { typeof(SpawnPaintingPacket), PacketType.SpawnPainting },
            { typeof(EntityRelativeMovePacket), PacketType.EntityRelativeMove },
            { typeof(EntityStatusPacket), PacketType.EntityStatus },
            { typeof(EntityTeleportPacket), PacketType.EntityTeleport },
            { typeof(EntityVelocityPacket), PacketType.EntityVelocity },
            { typeof(SpawnExperienceOrbPacket), PacketType.SpawnExperienceOrb },
            { typeof(ExplosionPacket), PacketType.Explosion },
            { typeof(HandshakePacket), PacketType.Handshake },
            { typeof(HeldItemChangePacket), PacketType.HeldItemChange },
            { typeof(KeepAlivePacket), PacketType.KeepAlive },
            { typeof(IncrementStatisticPacket), PacketType.IncrementStatistic },
            { typeof(LoginRequestPacket), PacketType.LoginRequest },
            { typeof(ChunkAllocationPacket), PacketType.ChunkAllocation },
            { typeof(ChunkDataPacket), PacketType.ChunkData },
            { typeof(SpawnMobPacket), PacketType.SpawnMob },
            { typeof(MultiBlockChangePacket), PacketType.MultiBlockChange },
            { typeof(SpawnNamedEntityPacket), PacketType.SpawnNamedEntity },
            { typeof(ChangeGameStatePacket), PacketType.ChangeGameState },
            { typeof(OpenWindowPacket), PacketType.OpenWindow },
            { typeof(SpawnDroppedItemPacket), PacketType.SpawnDroppedItem },
            { typeof(PlayerPacket), PacketType.Player },
            { typeof(PlayerBlockPlacementPacket), PacketType.PlayerBlockPlacement },
            { typeof(PlayerDiggingPacket), PacketType.PlayerDigging },
            { typeof(PlayerListItemPacket), PacketType.PlayerListItem },
            { typeof(PlayerPositionPacket), PacketType.PlayerPosition },
            { typeof(PlayerPositionRotationPacket), PacketType.PlayerPositionRotation },
            { typeof(PlayerRotationPacket), PacketType.PlayerRotation },
            { typeof(RemoveEntityEffectPacket), PacketType.RemoveEntityEffect },
            { typeof(RespawnPacket), PacketType.Respawn },
            { typeof(ServerListPingPacket), PacketType.ServerListPing },
            { typeof(SetSlotPacket), PacketType.SetSlot },
            { typeof(SoundParticleEffectPacket), PacketType.SoundParticleEffect },
            { typeof(SpawnPositionPacket), PacketType.SpawnPosition },
            { typeof(TimeUpdatePacket), PacketType.TimeUpdate },
            { typeof(ThunderBoltPacket), PacketType.Thunderbolt },
            { typeof(ConfirmTransactionPacket), PacketType.ConfirmTransaction },
            { typeof(UpdateHealthPacket), PacketType.UpdateHealth },
            { typeof(UpdateWindowPropertyPacket), PacketType.UpdateWindowProperty },
            { typeof(UpdateSignPacket), PacketType.UpdateSign },
            { typeof(UseBedPacket), PacketType.UseBed },
            { typeof(UseEntityPacket), PacketType.UseEntity },
            { typeof(ClickWindowPacket), PacketType.ClickWindow },
            { typeof(SetWindowItemsPacket), PacketType.SetWindowItems }
        };

        public static PacketType GetPacketType(Type type)
        {
            PacketType packetType;
            if (map.TryGetValue(type, out packetType))
                return packetType;

            throw new KeyNotFoundException();
        }
    }
}
