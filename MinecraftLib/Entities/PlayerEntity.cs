using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Entities
{
    public class PlayerEntity : Entity
    {
        public const short CraftingOutput = 0;
        public const short CraftingOffset = 1;
        public const short ArmorOffset = 5;
        public const short InventoryOffset = 9;
        public const short HotbarOffset = 36;

        public String Name { get; set; }
        public short Health { get; set; }
        public double FallStart { get; set; }
        public double Stance { get; set; }
        public short Food { get; set; }
        public float FoodSaturation { get; set; }
        public WorldMode GameMode { get; set; }
        public float ExperienceProgress { get; set; }
        public short ExperienceLevel { get; set; }
        public short ExperienceTotal { get; set; }
        public EntityMetadata Metadata { get; set; }
        public bool IsFlying { get; set; }
        public Vector3 Spawn { get; set; }

        public Slot[] Inventory { get; set; }
        public int SelectedSlot { get; set; }
        public Slot InHand
        {
            get
            {
                return Inventory[SelectedSlot];
            }
            set
            {
                Inventory[SelectedSlot] = value;
            }
        }
        public Slot HeadPiece
        {
            get
            {
                return Inventory[5];
            }
            set
            {
                Inventory[5] = value;
            }
        }
        public Slot ChestPiece
        {
            get
            {
                return Inventory[6];
            }
            set
            {
                Inventory[6] = value;
            }
        }
        public Slot LegPiece
        {
            get
            {
                return Inventory[7];
            }
            set
            {
                Inventory[7] = value;
            }
        }
        public Slot ShoePiece
        {
            get
            {
                return Inventory[8];
            }
            set
            {
                Inventory[8] = value;
            }
        }

        public PlayerEntity()
        {
            Metadata = new EntityMetadata();
            BoundingBox = new BoundingBox(0.3, 1.8);
            Inventory = new Slot[45];
            for (int x = 0; x < Inventory.Length; x++)
                Inventory[x] = new Slot();
            SelectedSlot = 0;
        }
    }
}
