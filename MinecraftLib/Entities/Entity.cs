using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public bool IsOnFire { get; set; }
        public bool IsCrouching { get; set; }
        public bool IsRiding { get; set; }
        public bool IsSprinting { get; set; }
        public bool IsEating { get; set; }
        public bool OnGround { get; set; }
        public Vector3 Location { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Rotation { get; set; }
        public Dimension Dimension { get; set; }
        public BoundingBox BoundingBox { get; set; }

        public Entity()
        {
            Location = Vector3.Zero;
            Velocity = Vector3.Zero;
            Rotation = Vector3.Zero;
        }
    }
    public enum DamageCause
    {
        Contact,
        EntityAttack,
        Projectile,
        Suffocation,
        Fall,
        Fire,
        FireBurn,
        Lava,
        Drowning,
        BlockExplosion,
        EntityExplosion,
        Void,
        Lightning,
        Cactus,
        Custom
    }
}
