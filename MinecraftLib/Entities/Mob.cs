using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Entities
{
    public enum MobType : byte
    {
        Creeper = 50,
        Skeleton = 51,
        Spider = 52,
        GiantZombie = 53,
        Zombie = 54,
        Slime = 55,
        Ghast = 56,
        ZombiePigman = 57,
        Enderman = 58,
        CaveSpider = 59,
        Silverfish = 60,
        Blaze = 61,
        MagmaCube = 62,
        EnderDragon = 63,
        Pig = 90,
        Sheep = 91,
        Cow = 92,
        Duck = 93,
        Squid = 94,
        Wolf = 95,
        Mooshroom = 96,
        Snowman = 97,
        Villager = 120
    }
    public class Mob : Entity
    {
        public short Health { get; set; }
        public double FallStart { get; set; }
        public MobType Type { get; set; }
        public bool IsHostile
        {
            get
            {
                return (byte)Type < 90;
            }
        }
        public EntityMetadata Metadata { get; set; }
        public sbyte HeadYaw { get; set; }

        public Mob()
        {
            Location = Vector3.Zero;
            Rotation = Vector3.Zero;
            Metadata = new EntityMetadata();
        }
    }
}
