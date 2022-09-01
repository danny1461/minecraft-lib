using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Entities
{
    public class Pickup : Entity
    {
        public short ItemId { get; set; }
        public byte Count { get; set; }
        public short DamageData { get; set; }

        public Pickup()
        {
            Location = Vector3.Zero;
            Rotation = Vector3.Zero;
        }
    }
}
