using System;
using System.Collections.Generic;
using System.Text;
using MinecraftLib.Entities;

namespace MinecraftLib
{
    public class BoundingBox
    {
        public double Width;
        public double Height;

        public BoundingBox(double Width, double Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
        public BoundingBox(Entity e)
        {
            if (e is Pickup)
            {
                Width = 0.125;
                Height = 0.25;
            }
            else if (e is PlayerEntity)
            {
                Width = 0.3;
                Height = 1.8;
            }
            else if (e is Mob)
            {
                Mob m = (Mob)e;
                switch (m.Type)
                {
                    case MobType.Creeper:
                    case MobType.Skeleton:
                    case MobType.Zombie:
                    case MobType.ZombiePigman:
                    case MobType.Wolf:
                    case MobType.Snowman:
                    case MobType.Villager:
                        Width = 0.3;
                        Height = 1.8;
                        break;
                    case MobType.Spider:
                    case MobType.CaveSpider:
                        Width = 0.7;
                        Height = 0.9;
                        break;
                    case MobType.Slime:
                    case MobType.MagmaCube:
                        Width = 0.3 * (byte)m.Metadata[16]["default"];
                        Height = Width * 2;
                        break;
                    case MobType.Ghast:
                        Width = 2;
                        Height = 4;
                        break;
                    case MobType.Enderman:
                        Width = 0.3;
                        Height = 2.8;
                        break;
                    case MobType.Pig:
                        Width = 0.45;
                        Height = 0.9;
                        break;
                    case MobType.Sheep:
                        Width = 0.3;
                        Height = 1.3;
                        break;
                    case MobType.Cow:
                        Width = 0.45;
                        Height = 1.9;
                        break;
                    case MobType.Duck:
                        Width = 0.15;
                        Height = 0.4;
                        break;
                    case MobType.Squid:
                        Width = 0.475;
                        Height = 0.95;
                        break;
                    case MobType.Silverfish:        // Unknown
                    case MobType.Blaze:
                    case MobType.EnderDragon:
                    case MobType.Mooshroom:
                        Width = Height = 0;
                        break;
                }
            }
        }
    }
}
